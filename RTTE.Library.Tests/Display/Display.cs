using NUnit.Framework;
using System.Drawing;
using NSubstitute;
using System.Drawing.Imaging;
using RTTE.Library.Native;
using LibraryDisplay = RTTE.Library.Display.Display;
using LanguageExt;
using RTTE.Library.Common.Interfaces;

namespace RTTE.Library.Tests.Display {
    public class Display {
        [TestCase]
        public void Create_ShouldCreateNewInstanceOfDisplay_Always() {
            IScreenGrabber grabber = Substitute.For<IScreenGrabber>();
            _ = grabber.Capture(Arg.Any<Rectangle>()).ReturnsForAnyArgs((callInfo) => {
                Rectangle rect = callInfo.Arg<Rectangle>();

                if (rect.Width <= 0 || rect.Height <= 0)
                    return Option<Image>.None;

                Image img = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb);
                return img;
            });

            DevMode devMode = new() { dmPositionX = 0, dmPositionY = 0, dmPelsWidth = 1000, dmPelsHeight = 1000 };

            LibraryDisplay result = LibraryDisplay.Create(grabber, "TestDevice", new Rectangle(0, 0, 1000, 1000));
            Assert.IsInstanceOf(typeof(LibraryDisplay), result);
        }

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
        public void GetArea_ShouldReturnRectangleWithCorrectDimension_Always(
            int dmPositionX,
            int dmPositionY,
            int dmPelsWidth,
            int dmPelsHeight,
            int expectedX,
            int expectedY,
            int expectedWidth,
            int expectedHeight
        ) {
            IScreenGrabber grabber = Substitute.For<IScreenGrabber>();
            _ = grabber.Capture(Arg.Any<Rectangle>()).ReturnsForAnyArgs((callInfo) => {
                Rectangle rect = callInfo.Arg<Rectangle>();

                if (rect.Width <= 0 || rect.Height <= 0)
                    return Option<Image>.None;

                Image img = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb);
                return img;
            });

            Rectangle rect = new(dmPositionX, dmPositionY, dmPelsWidth, dmPelsHeight);

            LibraryDisplay result = LibraryDisplay.Create(grabber, "TestDevice", rect);
            Rectangle resultArea = result.GetArea();
            Assert.IsInstanceOf(typeof(Rectangle), resultArea);
            Assert.AreEqual(expectedX, resultArea.X, "Area x position is not correct!");
            Assert.AreEqual(expectedY, resultArea.Y, "Area y position is not correct!");
            Assert.AreEqual(expectedWidth, resultArea.Width, "Area width is not correct!");
            Assert.AreEqual(expectedHeight, resultArea.Height, "Area height is not correct!");
        }

        [TestCase(200, 200)]
        [TestCase(500, 700)]
        [TestCase(1, 1)]
        [TestCase(1, 1000)]
        public void GetImage_ShouldReturnSomeImageWithCorrectSize_IfHasValidArea(
            int dmPelsWidth,
            int dmPelsHeight
        ) {
            IScreenGrabber grabber = Substitute.For<IScreenGrabber>();
            _ = grabber.Capture(Arg.Any<Rectangle>()).ReturnsForAnyArgs((callInfo) => {
                Rectangle rect = callInfo.Arg<Rectangle>();

                if (rect.Width <= 0 || rect.Height <= 0)
                    return Option<Image>.None;

                Image img = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb);
                return img;
            });

            Rectangle rect = new(0, 0, dmPelsWidth, dmPelsHeight);

            LibraryDisplay result = LibraryDisplay.Create(grabber, "TestDevice", rect);
            Option<Image> resultImage = result.GetImage();
            _ = resultImage.Match(resultImage => {
                Assert.IsInstanceOf(typeof(Image), resultImage);
                Assert.AreEqual(dmPelsWidth, resultImage.Width, "Image width is not correct!");
                Assert.AreEqual(dmPelsHeight, resultImage.Height, "Image height is not correct!");
            }, () => Assert.Fail());
        }

        [TestCase(-1, -1)]
        [TestCase(0, 0)]
        public void GetImage_ShouldReturnNone_IfHasInvalidArea(
            int dmPelsWidth,
            int dmPelsHeight
        ) {
            IScreenGrabber grabber = Substitute.For<IScreenGrabber>();
            _ = grabber.Capture(Arg.Any<Rectangle>()).ReturnsForAnyArgs((callInfo) => {
                Rectangle rect = callInfo.Arg<Rectangle>();

                if (rect.Width <= 0 || rect.Height <= 0)
                    return Option<Image>.None;

                Image img = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb);
                return img;
            });

            Rectangle rect = new(0, 0, dmPelsWidth, dmPelsHeight);

            LibraryDisplay result = LibraryDisplay.Create(grabber, "TestDevice", rect);
            Option<Image> resultImage = result.GetImage();
            _ = resultImage.Match(resultImage => Assert.Fail(), () => Assert.Pass());
        }

        [TestCase("hello world!")]
        [TestCase("")]
        [TestCase("!!!!!!!")]
        [TestCase(null)]
        public void GetName_ShouldReturnPassedInName_Always(
            string name
        ) {
            IScreenGrabber grabber = Substitute.For<IScreenGrabber>();
            _ = grabber.Capture(Arg.Any<Rectangle>()).Returns(Option<Image>.None);

            Rectangle rect = new(0, 0, 1, 1);

            LibraryDisplay result = LibraryDisplay.Create(grabber, name, rect);
            string resultName = result.GetName();
            Assert.AreEqual(name, resultName, "Name is not correct!");
        }
    }
}
