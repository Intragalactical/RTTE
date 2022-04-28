using System.Drawing;
using System.Runtime.InteropServices;

namespace RTTE.Library.Native;

[StructLayout(LayoutKind.Sequential)]
public struct NativeRectangle {
    public int X { get; set; }
    public int Y { get; set; }
    public int Left {
        get => X;
        set { X = value; }
    }
    public int Top {
        get => Y;
        set { Y = value; }
    }
    public int Right { get; set; }
    public int Bottom { get; set; }
    public int Height {
        get => Bottom - Y;
        set { Bottom = value + Y; }
    }
    public int Width {
        get => Right - X;
        set { Right = value + X; }
    }

    public Point Location {
        get => new(Left, Top);
        set {
            X = value.X;
            Y = value.Y;
        }
    }

    public Size Size {
        get => new(Width, Height);
        set {
            Right = value.Width + X;
            Bottom = value.Height + Y;
        }
    }

    public NativeRectangle(NativeRectangle Rectangle) : this(Rectangle.Left, Rectangle.Top, Rectangle.Right, Rectangle.Bottom) {
    }

    public NativeRectangle(int Left, int Top, int Right, int Bottom) {
        X = Left;
        Y = Top;
        this.Right = Right;
        this.Bottom = Bottom;
    }

    public static implicit operator Rectangle(NativeRectangle Rectangle)
        => new(Rectangle.Left, Rectangle.Top, Rectangle.Width, Rectangle.Height);

    public static implicit operator NativeRectangle(Rectangle Rectangle)
        => new(Rectangle.Left, Rectangle.Top, Rectangle.Right, Rectangle.Bottom);

    public static bool operator ==(NativeRectangle Rectangle1, NativeRectangle Rectangle2)
        => Rectangle1.Equals(Rectangle2);

    public static bool operator !=(NativeRectangle Rectangle1, NativeRectangle Rectangle2)
        => !Rectangle1.Equals(Rectangle2);

    public override string ToString()
        => "{Left: " + X + "; " + "Top: " + Y + "; Right: " + Right + "; Bottom: " + Bottom + "}";

    public override int GetHashCode()
        => ToString().GetHashCode();

    public bool Equals(NativeRectangle Rectangle)
        => Rectangle.Left == X && Rectangle.Top == Y && Rectangle.Right == Right && Rectangle.Bottom == Bottom;

    public override bool Equals(object Object) {
        if (Object is NativeRectangle nativeRectangle) {
            return Equals(nativeRectangle);
        } else if (Object is Rectangle rectangle) {
            return Equals(new NativeRectangle(rectangle));
        }

        return false;
    }
}
