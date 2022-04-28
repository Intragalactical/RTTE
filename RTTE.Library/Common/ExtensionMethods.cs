using System;
using System.Drawing;

namespace RTTE.Library.Common;

public static class ExtensionMethods {
    public static Image ConformToSize(this Image image, Size size) {
        Image previewImg = new Bitmap(size.Width, size.Height, image.PixelFormat);
        using Graphics gfx = Graphics.FromImage(previewImg);
        gfx.DrawImage(image, new Rectangle(0, 0, size.Width, size.Height), new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);
        return previewImg;
    }

    public static byte[] ToLPTStr(this string str) {
        var lptArray = new byte[str.Length + 1];

        int index = 0;
        foreach (char c in str.ToCharArray())
            lptArray[index++] = Convert.ToByte(c);

        lptArray[index] = Convert.ToByte('\0');

        return lptArray;
    }
}
