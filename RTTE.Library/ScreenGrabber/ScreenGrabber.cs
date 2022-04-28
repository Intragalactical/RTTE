using LanguageExt;
using RTTE.Library.Common.Interfaces;
using System.Drawing;
using System.Drawing.Imaging;

namespace RTTE.Library.ScreenGrabber;

public sealed class ScreenGrabber : IScreenGrabber {
    private ScreenGrabber() { }

    public Option<Image> Capture(Rectangle area) {
        static Image continueCapture(Rectangle area) {
            Image capture = new Bitmap(area.Width, area.Height, PixelFormat.Format32bppArgb);
            using var gfx = Graphics.FromImage(capture);
            gfx.CopyFromScreen(area.Left, area.Top, 0, 0, area.Size, CopyPixelOperation.SourceCopy);
            return capture;
        }

        return area.Width > 0 && area.Height > 0 ?
            continueCapture(area) :
            Option<Image>.None;
    }

    public static IScreenGrabber Create()
        => new ScreenGrabber();
}
