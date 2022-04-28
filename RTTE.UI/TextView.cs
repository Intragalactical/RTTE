using RTTE.Library.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTTE.UI {
    public partial class TextView : UserControl {
        private string text = "";
        public new string Text {
            get => text;
            set {
                if (text != value) {
                    text = value;
                    Refresh();
                }
            }
        }
        public ITranslator Translator { get; set; }

        public TextView() : base() {
            InitializeComponent();

            DoubleBuffered = true;
        }

        private void TextView_Paint(object sender, PaintEventArgs e) {
            CharacterRange[] ranges = Text.Select(c => new CharacterRange(Text.IndexOf(c), 1)).ToArray();
            StringFormat stringFormat = new();
            //stringFormat.SetMeasurableCharacterRanges(ranges);

            Region[] regions = e.Graphics.MeasureCharacterRanges(Text, Font, new RectangleF(0, 0, 400, Font.Height * 2), stringFormat);
            e.Graphics.DrawString(Text, Font, new SolidBrush(ForeColor), Point.Empty);

            foreach (Region region in regions) {
                e.Graphics.DrawRectangle(Pens.Red, Rectangle.Round(region.GetBounds(e.Graphics)));
            }
        }

        private void TextView_MouseMove(object sender, MouseEventArgs e) {
            Refresh();
        }
    }
}
