using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTTE.UI {
    public class DataGridViewProgressColumn : DataGridViewImageColumn {
        public DataGridViewProgressColumn() {
            CellTemplate = new DataGridViewProgressCell();
        }
    }
}
