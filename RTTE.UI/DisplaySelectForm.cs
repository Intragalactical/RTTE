using RTTE.Library.Display;
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using RTTE.Library.Common;
using LibraryUtils = RTTE.Library.Common.Utils;
using RTTE.Library.ScreenGrabber;
using RTTE.Library.Native;
using RTTE.Library.Common.Interfaces;

namespace RTTE.UI {
    public partial class DisplaySelectForm : Form {
        private static class Constants {
            internal const uint UpdateIntervalMs = 10;
        }

        private List<IDisplay> Displays { get; } = new();
        private List<SelectScreenAreaForm> ScreenAreaSelectors { get; } = new();
        private ImageList Images { get; } = new();
        private Thread UpdateThread { get; set; }
        private CancellationTokenSource CancellationToken { get; }

        public DisplaySelectForm() {
            InitializeComponent();

            Text = string.Format(Text, LibraryUtils.GetCurrentArchitecture().ToString());

            CancellationToken = new();

            ListViewDisplaysWindows.LargeImageList = Images;
            ListViewDisplaysWindows.LargeImageList.ImageSize = new Size(256, 144);
        }

        private void WindowSelectForm_Load(object sender, EventArgs e) {
            UpdateThread = new(Updater);
            UpdateThread.Start();

            IEnumerable<IDisplay> displayMonitors = DisplayList.Create(API.Create(), ScreenGrabber.Create()).Get();
            Displays.AddRange(displayMonitors);

            for (int i = 0; i < displayMonitors.Count(); i++) {
                IDisplay displayMonitor = displayMonitors.ElementAt(i);
                _ = displayMonitor.GetImage().Match(image => {
                    string displayName = displayMonitor.GetName();
                    int imageIndex = Images.Images.AddStrip(image.ConformToSize(new(256, 144)) ?? null);
                    Images.Images.SetKeyName(imageIndex, displayName);
                    ListViewItem newItem = new() {
                        ImageKey = displayName,
                        Text = displayName,
                        ImageIndex = imageIndex
                    };
                    _ = ListViewDisplaysWindows.Items.Add(newItem);
                }, () => { /* @TODO: do error handling */ });
            }

            GC.Collect();
            GC.Collect();
        }

        private void Updater() {
            while (!CancellationToken.IsCancellationRequested) {
                Thread.Sleep((int)Constants.UpdateIntervalMs);
            }
        }

        private void CloseAllSelectors() {
            ScreenAreaSelectors.ForEach(areaSelector => areaSelector.ForceClose());
            Show();
        }

        private void OnSelectArea(Rectangle area) {
            ScreenAreaSelectors.ForEach(areaSelector => areaSelector.ForceClose());
            //new AksuForm(area).Show();
            Close();
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e) {
            Application.Exit();
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e) {
            _ = new AboutForm().ShowDialog();
        }

        private void ButtonClose_Click(object sender, EventArgs e) {
            Application.Exit();
        }

        private void BackButton_Click(object sender, EventArgs e) {
            new MainForm().Show();
            Close();
        }

        private void ListViewDisplaysWindows_SelectedIndexChanged(object sender, EventArgs e) {
            ButtonSelect.Enabled = ListViewDisplaysWindows.SelectedItems.Count > 0;
        }

        private bool IsSelectedDisplay(IDisplay display)
            => display.GetName().Equals(ListViewDisplaysWindows.SelectedItems[0].Text, StringComparison.InvariantCultureIgnoreCase);

        private void ButtonSelect_Click(object sender, EventArgs e) {
            ScreenAreaSelectors.Clear();

            ScreenAreaSelectors.AddRange(
                Displays.Select(display => new SelectScreenAreaForm(display, IsSelectedDisplay(display), OnSelectArea, CloseAllSelectors))
            );

            if (ScreenAreaSelectors.Any(form => form.Selectable)) {
                Hide();
                ScreenAreaSelectors.ForEach(form => form.Show());
            }
        }
    }
}
