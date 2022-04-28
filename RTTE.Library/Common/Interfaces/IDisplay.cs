using LanguageExt;
using System.Drawing;

namespace RTTE.Library.Common.Interfaces;

public interface IDisplay {
    public string GetName();
    public Rectangle GetArea();
    public Option<Image> GetImage();
}
