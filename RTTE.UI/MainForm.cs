using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibraryUtils = RTTE.Library.Common.Utils;

namespace RTTE.UI;

public partial class MainForm : Form {
    private static class Constants {
        internal const string ButtonReadFromMemoryTT = "Select a process, find text address hooks and read from memory. Also supports DLL injection.";
        internal const string ButtonOCRTT = "Select an area of a window or a screen and use Optical Character Recognition to find the text.";
        internal const string ButtonExitTT = "Exit RTTE";
    }

    public MainForm() {
        InitializeComponent();

        Text = string.Format(Text, LibraryUtils.GetCurrentArchitecture().ToString());

        HelpTooltip.SetToolTip(ButtonReadFromMemory, Constants.ButtonReadFromMemoryTT);
        HelpTooltip.SetToolTip(ButtonOCR, Constants.ButtonOCRTT);
        HelpTooltip.SetToolTip(ButtonExit, Constants.ButtonExitTT);
    }

    private void ButtonExit_Click(object sender, EventArgs e) {
        Application.Exit();
    }

    private void ButtonReadFromMemory_Click(object sender, EventArgs e) {
        new ProcessSelectForm().Show();
        Close();
    }

    private void ExitToolStripMenuItem_Click(object sender, EventArgs e) {
        Application.Exit();
    }

    private void AboutToolStripMenuItem_Click(object sender, EventArgs e) {
        _ = new AboutForm().ShowDialog();
    }

    private void ButtonOCR_Click(object sender, EventArgs e) {
        new DisplaySelectForm().Show();
        Close();
    }
}
