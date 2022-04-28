using LanguageExt;
using System.Drawing;

namespace RTTE.Library.Common.Interfaces;

public interface IScreenGrabber {
    public Option<Image> Capture(Rectangle area);
}
