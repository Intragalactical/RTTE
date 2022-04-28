using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using LanguageExt;
using RTTE.Library.Common;
using RTTE.Library.Common.Interfaces;
using RTTE.Library.Native;

namespace RTTE.Library.Process;

public sealed class AdvancedProcess : IAdvancedProcess {
    private readonly string name;
    private readonly Option<string> fullPath;
    private readonly int id;
    private readonly Option<string> mainWindowTitle;
    private readonly IntPtr mainWindowHandle;
    private readonly Option<Image> previewImage;
    private readonly Option<ProcessArchitecture> architecture;

    private IWinAPI WinAPI { get; }

    private AdvancedProcess(
        IWinAPI winAPI,
        int processId,
        Option<string> fullPath,
        string processName,
        Option<string> mainWindowTitle,
        IntPtr mainWindowHandle,
        Option<Image> previewImage,
        Option<ProcessArchitecture> architecture
    ) {
        WinAPI = winAPI;
        id = processId;
        this.fullPath = fullPath;
        name = processName;
        this.mainWindowTitle = mainWindowTitle;
        this.mainWindowHandle = mainWindowHandle;
        this.previewImage = previewImage;
        this.architecture = architecture;
    }

    public Option<string> GetMainWindowTitle() => mainWindowTitle;
    public IntPtr GetMainWindowHandle() => mainWindowHandle;
    public string GetName() => name;
    public int GetId() => id;
    public Option<IntPtr> GetHandle() => Utils.GetHandleSafe(WinAPI, GetId());
    public string GetArchitectureString() => architecture.Match(architecture => architecture.ToString(), () => "");
    public Option<Image> GetPreviewImage() => previewImage;
    public Option<ProcessArchitecture> GetArchitecture() => architecture;
    public Option<string> GetFullPath() => fullPath;

    public string AsString() {
        return $"{GetMainWindowTitle()}{GetName()}{GetArchitectureString()}{GetId()}";
    }

    private static Option<Rectangle> GetWindowRect(IWinAPI winAPI, IntPtr windowHandle) {
        bool result = winAPI.GetWindowRect(windowHandle, out NativeRectangle rect);
        return result ?
            Option<Rectangle>.Some(rect) :
            Option<Rectangle>.None;
    }

    private static Image DrawAsPreview(Image appImage)
        => appImage.ConformToSize(new(50, 32));

    private static Option<Image> GetMainWindowImage(IWinAPI winAPI, IntPtr windowHandle) {
        Option<Rectangle> windowRectangle = GetWindowRect(winAPI, windowHandle);

        static Option<Image> getImageWithWindowRectangle(IWinAPI winAPI, IntPtr mainWindowHandle, Rectangle windowRectangle) {
            Image img = new Bitmap(windowRectangle.Width, windowRectangle.Height, PixelFormat.Format32bppArgb);
            using Graphics gfx = Graphics.FromImage(img);
            IntPtr hdcBitmap = gfx.GetHdc();

            bool success = winAPI.PrintWindow(mainWindowHandle, hdcBitmap, 0);

            gfx.ReleaseHdc(hdcBitmap);

            return success ? img : Option<Image>.None;
        }

        return windowRectangle.Match(windowRectangle =>
            windowRectangle.Width > 0 && windowRectangle.Height > 0 ?
                getImageWithWindowRectangle(winAPI, windowHandle, windowRectangle) :
                Option<Image>.None,
            Option<Image>.None
        );
    }

    private static Option<ProcessArchitecture> GetProcessArchitecture(IPeFileParser parser, string processImageName) {
        Option<IPeFile> processPeFile = parser.TryParse(processImageName);
        return processPeFile.Match(
            processPeFile => processPeFile.GetArchitecture(),
            Option<ProcessArchitecture>.None
        );
    }

    private static Option<string> QueryFullProcessImageName(IWinAPI winAPI, IntPtr processHandle) {
        int characterCount = 256;
        StringBuilder sb = new(characterCount);
        bool success = winAPI.QueryFullProcessImageName(processHandle, 0, sb, ref characterCount);

        return success ? sb.ToString() : Option<string>.None;
    }

    public static IAdvancedProcess Create(
        IWinAPI winAPI,
        IPeFileParser parser,
        int processId,
        string processName,
        Option<string> mainWindowTitle,
        IntPtr mainWindowHandle
    ) {
        Option<IntPtr> handle = Utils.GetHandleSafe(winAPI, processId);
        Option<string> fullPath = handle.Match(
            handle => QueryFullProcessImageName(winAPI, handle),
            Option<string>.None
        );
        string realProcessName = fullPath.Match(path => {
            string fileName = Path.GetFileName(path);
            return fileName.Equals(string.Empty) ?
                processName :
                fileName;
        }, () => processName);

        Option<Image> mainWindowImage = GetMainWindowImage(winAPI, mainWindowHandle);
        Option<Image> previewImage = mainWindowImage.Match(
            image => DrawAsPreview(image),
            Option<Image>.None
        );

        Option<ProcessArchitecture> architecture = fullPath.Match(
            fullPath => GetProcessArchitecture(parser, fullPath),
            Option<ProcessArchitecture>.None
        );

        return new AdvancedProcess(
            winAPI,
            processId,
            fullPath,
            realProcessName,
            mainWindowTitle,
            mainWindowHandle,
            previewImage,
            architecture
        );
    }

    public static IAdvancedProcess Create(IWinAPI winAPI, IPeFileParser parser, IProcess process)
        => Create(
            winAPI,
            parser,
            process.GetId(),
            process.GetName(),
            process.GetMainWindowTitle(),
            process.GetMainWindowHandle()
        );
}
