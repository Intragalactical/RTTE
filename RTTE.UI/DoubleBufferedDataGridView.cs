using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTTE.UI {
    public class DoubleBufferedDataGridView : DataGridView {
        public DoubleBufferedDataGridView() : base() {
            DoubleBuffered = true;
        }
    }
}
