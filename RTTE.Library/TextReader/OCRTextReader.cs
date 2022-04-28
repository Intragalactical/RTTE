using RTTE.Library.Common.Interfaces;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Tesseract;

namespace RTTE.Library.TextReader;

public sealed class OCRTextReader : ITextReader, IDisposable {
    private TesseractEngine Engine { get; }
    private Rectangle Area { get; }

    public OCRTextReader(Rectangle area) {
        Engine = new(@".\tessdata", "jpn", EngineMode.Default);
        Area = area;
    }

    public static Image GetImageOfArea(Rectangle area) {
        Image capture = new Bitmap(area.Width, area.Height, PixelFormat.Format32bppArgb);
        using var gfx = Graphics.FromImage(capture);
        gfx.CopyFromScreen(area.Left, area.Top, 0, 0, area.Size, CopyPixelOperation.SourceCopy);
        return capture;
    }

    public string Read() {
        Image capture = GetImageOfArea(Area);
        using MemoryStream ms = new();
        capture.Save(ms, System.Drawing.Imaging.ImageFormat.Tiff);

        using Page page = Engine.Process(Pix.LoadFromMemory(ms.ToArray()));
        return page.GetText();
    }

    public void Dispose() {
        Engine.Dispose();
    }
}
