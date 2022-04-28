using RTTE.Library.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DisplayMonitor = RTTE.Library.Display.Display;

namespace RTTE.UI {
    public partial class SelectScreenAreaForm : Form {
        private static class Constants {
            internal const string SelectableText = "Select an area encompassing the text";
            internal const int InvalidOpacityFactor = 4;
            internal const int SelectionFactor = 2;
            internal const int UpdateIntervalMs = 1000 / 60;
            internal const float SelectableTextFontSize = 36f;
        }

        public delegate void SelectArea(Rectangle area);
        public delegate void CloseAllSelectors();

        public IDisplay Display { get; }
        public bool Selectable { get; }

        private bool Closeable { get; set; }
        private SelectArea OnSelectArea { get; }
        private CloseAllSelectors OnCloseAllSelectors { get; }
        private Font TextFont { get; }
        private Brush TextBrush { get; } = Brushes.LightGray;
        private Brush InvalidOverlay { get; } = new SolidBrush(Color.FromArgb(
            byte.MaxValue / Constants.InvalidOpacityFactor,
            Color.DarkRed
        ));
        private Brush SelectionBrush { get; } = new SolidBrush(Color.FromArgb(
            byte.MaxValue / Constants.SelectionFactor,
            byte.MaxValue,
            byte.MaxValue,
            byte.MaxValue
        ));
        private Pen SelectionPen { get; } = new(Color.FromArgb(
            byte.MaxValue / Constants.SelectionFactor,
            byte.MaxValue / Constants.SelectionFactor,
            byte.MaxValue / Constants.SelectionFactor,
            byte.MaxValue / Constants.SelectionFactor
        ));
        private bool Selecting { get; set; } = false;
        private Point TextPosition { get; } = new(10, 10);
        private Point SelectStart { get; set; } = Point.Empty;
        private Point SelectEnd { get; set; } = Point.Empty;
        private Thread UpdateThread { get; set; }
        private CancellationTokenSource CancellationToken { get; } = new();

        public SelectScreenAreaForm(IDisplay display, bool selectable, SelectArea selectArea, CloseAllSelectors closeAll) {
            InitializeComponent();

            TextFont = new(Font.FontFamily, Constants.SelectableTextFontSize, FontStyle.Bold);

            Display = display;
            Selectable = selectable;
            OnCloseAllSelectors = closeAll;
            OnSelectArea = selectArea;

            DesktopBounds = display.GetArea();
            WindowState = FormWindowState.Maximized;

            Cursor = Selectable ? Cursors.Cross : Cursors.No;
        }

        public void ForceClose() {
            Closeable = true;
            Close();
        }

        private void SelectScreenAreaForm_Load(object sender, EventArgs e) {
            Closeable = false;

            UpdateThread = new(Updater);
            UpdateThread.Start();
        }

        private void SelectScreenAreaForm_FormClosing(object sender, FormClosingEventArgs e) {
            e.Cancel = !Closeable;

            if (Closeable)
                CancellationToken.Cancel();
        }

        private void Updater() {
            while (!CancellationToken.IsCancellationRequested) {
                try {
                    _ = Invoke(new MethodInvoker(() => Refresh()));
                } catch (ObjectDisposedException) {
                }

                Thread.Sleep(Constants.UpdateIntervalMs);
            }
        }

        private void SelectScreenAreaForm_Paint(object sender, PaintEventArgs e) {
            e.Graphics.Clear(Color.White);

            Display.GetImage().Match(image => {
                e.Graphics.DrawImage(
                    image,
                    new Rectangle(0, 0, Size.Width, Size.Height),
                    new Rectangle(0, 0, image.Width, image.Height),
                    GraphicsUnit.Pixel
                );
            }, () => { /* @TODO: do error handling */ });

            if (!Selectable)
                e.Graphics.FillRectangle(InvalidOverlay, new Rectangle(0, 0, Size.Width, Size.Height));
            else
                e.Graphics.DrawString(Constants.SelectableText, TextFont, TextBrush, TextPosition);

            if (Selecting) {
                Point smaller = new(
                    Math.Min(SelectEnd.X, SelectStart.X),
                    Math.Min(SelectEnd.Y, SelectStart.Y)
                );
                Point larger = new(
                    Math.Max(SelectEnd.X, SelectStart.X),
                    Math.Max(SelectEnd.Y, SelectStart.Y)
                );

                e.Graphics.FillRectangle(
                    SelectionBrush,
                    smaller.X,
                    smaller.Y,
                    larger.X - smaller.X,
                    larger.Y - smaller.Y
                );
                e.Graphics.DrawRectangle(
                    SelectionPen,
                    smaller.X,
                    smaller.Y,
                    larger.X - smaller.X,
                    larger.Y - smaller.Y
                );
            }
        }

        private void SelectScreenAreaForm_MouseDown(object sender, MouseEventArgs e) {
            if (Selectable && e.Button == MouseButtons.Left) {
                Selecting = true;
                SelectStart = e.Location;
            } else if (e.Button == MouseButtons.Right) {
                OnCloseAllSelectors();
            }
        }

        private void SelectScreenAreaForm_MouseUp(object sender, MouseEventArgs e) {
            if (Selectable && e.Button == MouseButtons.Left) {
                Selecting = false;

                //Debug.WriteLine($"{DesktopBounds.Bottom}{DesktopBounds.Right}");

                Point min = new(
                    Math.Min(SelectStart.X, SelectEnd.X),
                    Math.Min(SelectStart.Y, SelectEnd.Y)
                );
                Point max = new(
                    Math.Max(SelectStart.X, SelectEnd.X),
                    Math.Max(SelectStart.Y, SelectEnd.Y)
                );
                Debug.WriteLine($"AAAAAAAA {min} {max}");
                Rectangle displayArea = Display.GetArea();
                Rectangle result = new(
                    ConvertRange(min.X + DesktopBounds.X, (DesktopBounds.Left, DesktopBounds.Right), (displayArea.Left, displayArea.Right)),
                    ConvertRange(min.Y + DesktopBounds.Y, (DesktopBounds.Top, DesktopBounds.Bottom), (displayArea.Top, displayArea.Bottom)),
                    ConvertRange(Math.Abs(min.X - max.X), (0, DesktopBounds.Width), (0, displayArea.Width)),
                    ConvertRange(Math.Abs(min.Y - max.Y), (0, DesktopBounds.Height), (0, displayArea.Height))
                );
                //Debug.WriteLine(result);
                OnSelectArea(result);
            }
        }

        /*public static int ConvertRange(
            int value,
            (int Min, int Max) original, // original range
            (int Min, int Max) theNew // desired range
        ) {
            return (int)(((value - original.Min) / (double)(original.Max - original.Min)) * (theNew.Max - theNew.Min) + theNew.Min);
        }*/

        public static int ConvertRange(
            int value,
            (int Min, int Max) original, // original range
            (int Min, int Max) theNew // desired range
        ) {
            double scale = (double)(theNew.Max - theNew.Min) / (original.Max - original.Min);
            return (int)(theNew.Min + ((value - original.Min) * scale));
        }

        private void SelectScreenAreaForm_Deactivate(object sender, EventArgs e) {
            if (Selectable)
                Selecting = false;
        }

        private void SelectScreenAreaForm_MouseMove(object sender, MouseEventArgs e) {
            if (Selectable)
                SelectEnd = e.Location;
        }
    }
}
