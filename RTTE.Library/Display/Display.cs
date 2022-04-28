using LanguageExt;
using RTTE.Library.Common.Interfaces;
using System.Drawing;

namespace RTTE.Library.Display;

public sealed class Display : IDisplay {
    private readonly string name;
    private readonly Rectangle area;
    private readonly Option<Image> image;

    private Display(IScreenGrabber screenGrabber, string name, Rectangle area) {
        this.name = name;
        this.area = area;
        image = screenGrabber.Capture(area);
    }

    public string GetName() => name;
    public Rectangle GetArea() => area;
    public Option<Image> GetImage() => image;

    public static Display Create(IScreenGrabber screenGrabber, string deviceName, Rectangle area)
        => new(screenGrabber, deviceName, area);
}
