using LanguageExt;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryDevMode = RTTE.Library.Native.DevMode;

namespace RTTE.Library.Tests.Native {
    public class DevMode {
        [TestCase(
            0, 0, 1000, 1000,
            0, 0, 1000, 1000
        )]
        [TestCase(
            300, 300, 1000, 1000,
            300, 300, 1000, 1000
        )]
        [TestCase(
            250, 400, 1500, 750,
            250, 400, 1500, 750
        )]
        [TestCase(
            -250, 420, 500, 600,
            -250, 420, 500, 600
        )]
        [TestCase(
            -275, -420, 2000, 3000,
            -275, -420, 2000, 3000
        )]
        [TestCase(
            0, -425, 7000, 100,
            0, -425, 7000, 100
        )]
        [TestCase(
            1, -430, 1, 2,
            1, -430, 1, 2
        )]
        [TestCase(
            0, 0, 0, 0,
            0, 0, 0, 0
        )]
        [TestCase(
            1, -1, 0, 0,
            1, -1, 0, 0
        )]
        public void GetArea_ShouldCreateRectangleWithCorrectDimension_WhenSizeIsNotNegative(
            int dmPositionX,
            int dmPositionY,
            int dmPelsWidth,
            int dmPelsHeight,
            int expectedX,
            int expectedY,
            int expectedWidth,
            int expectedHeight
        ) {
            LibraryDevMode devMode = new LibraryDevMode() {
                dmPositionX = dmPositionX,
                dmPositionY = dmPositionY,
                dmPelsWidth = dmPelsWidth,
                dmPelsHeight = dmPelsHeight
            };
            Option<Rectangle> resultArea = devMode.GetArea();
            _ = resultArea.Match(rectangle => {
                Assert.IsInstanceOf(typeof(Rectangle), rectangle);
                Assert.AreEqual(expectedX, rectangle.X, "Area x position is not correct!");
                Assert.AreEqual(expectedY, rectangle.Y, "Area y position is not correct!");
                Assert.AreEqual(expectedWidth, rectangle.Width, "Area width is not correct!");
                Assert.AreEqual(expectedHeight, rectangle.Height, "Area height is not correct!");
            }, () => Assert.Fail());
        }

        [TestCase(0, 0, -1, -1)]
        [TestCase(0, 0, 0, -1)]
        [TestCase(0, 0, -1, 0)]
        [TestCase(1, 0, -1, -1)]
        [TestCase(1, 1, -1, -1)]
        [TestCase(0, 1, -1, -1)]
        public void GetArea_ShouldCreateNone_WhenSizeIsNegative(
            int dmPositionX,
            int dmPositionY,
            int dmPelsWidth,
            int dmPelsHeight
        ) {
            LibraryDevMode devMode = new LibraryDevMode() {
                dmPositionX = dmPositionX,
                dmPositionY = dmPositionY,
                dmPelsWidth = dmPelsWidth,
                dmPelsHeight = dmPelsHeight
            };
            Option<Rectangle> resultArea = devMode.GetArea();
            _ = resultArea.Match(rectangle => Assert.Fail(), () => Assert.Pass());
        }
    }
}
