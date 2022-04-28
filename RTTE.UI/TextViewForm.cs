using RTTE.Library.Common.Interfaces;
using RTTE.Library.TextReader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTTE.UI {
    public partial class TextViewForm : Form {
        private ITextReader TextReader { get; }
        private CancellationTokenSource CancellationToken { get; }

        private Thread UpdateThread { get; set; }

        public TextViewForm(ITextReader textReader) {
            InitializeComponent();

            TextReader = textReader;
            CancellationToken = new();
        }

        private void TextViewForm_Load(object sender, EventArgs e) {
            UpdateThread = new(Updater);
            UpdateThread.Start();
        }

        private void Updater() {
            while (!CancellationToken.IsCancellationRequested) {
                string readFromMemory = TextReader.Read();
                _ = Invoke(new MethodInvoker(() => textView1.Text = readFromMemory));
                Thread.Sleep(1000 / 60);
            }
        }
    }
}
