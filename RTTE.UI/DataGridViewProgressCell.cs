using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTTE.UI {
    public class DataGridViewProgressCell : DataGridViewImageCell {
        private static readonly Image EmptyImage;
        static DataGridViewProgressCell() {
            EmptyImage = new Bitmap(1, 1, PixelFormat.Format32bppArgb);
        }

        public DataGridViewProgressCell() {
            ValueType = typeof(int);
        }

        protected override object GetFormattedValue(
            object value,
            int rowIndex,
            ref DataGridViewCellStyle cellStyle,
            TypeConverter valueTypeConverter,
            TypeConverter formattedValueTypeConverter,
            DataGridViewDataErrorContexts context
        )
            => EmptyImage;

        protected override void Paint(
            Graphics graphics,
            Rectangle clipBounds,
            Rectangle cellBounds,
            int rowIndex,
            DataGridViewElementStates cellState,
            object value,
            object formattedValue,
            string errorText,
            DataGridViewCellStyle cellStyle,
            DataGridViewAdvancedBorderStyle advancedBorderStyle,
            DataGridViewPaintParts paintParts
        ) {
            base.Paint(
                graphics,
                clipBounds,
                cellBounds,
                rowIndex,
                cellState,
                value,
                formattedValue,
                errorText,
                cellStyle,
                advancedBorderStyle,
                paintParts & ~DataGridViewPaintParts.ContentForeground
            );

            int progressValue = (int)(value ?? 0);
            float percentage = progressValue / 100.0f;
            Brush foreColorBrush = new SolidBrush(cellStyle.ForeColor);

            string percentageText = $"{progressValue}%";
            SizeF measurement = graphics.MeasureString(percentageText, cellStyle.Font);

            if (percentage > 0) {
                Brush fillRectBrush = new SolidBrush(Color.FromArgb(203, 235, 108));
                graphics.FillRectangle(fillRectBrush, cellBounds.X + 2, cellBounds.Y + 2, Convert.ToInt32((percentage * cellBounds.Width) - 4), cellBounds.Height - 4);
                graphics.DrawString(percentageText, cellStyle.Font, foreColorBrush, cellBounds.X + (cellBounds.Width / 2) - (measurement.Width / 2), cellBounds.Y + (cellBounds.Height / 2) - (measurement.Height / 2));
            } else {
                graphics.DrawString(
                    percentageText,
                    cellStyle.Font,
                    DataGridView.CurrentRow.Index == rowIndex ?
                        new SolidBrush(cellStyle.SelectionForeColor) :
                        foreColorBrush,
                    cellBounds.X + (cellBounds.Width / 2) - (measurement.Width / 2),
                    cellBounds.Y + (cellBounds.Height / 2) - (measurement.Height / 2)
                );
            }
        }
    }
}
