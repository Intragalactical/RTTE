using RTTE.Library.Process;
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
using System.Diagnostics;
using RTTE.UI.Common;
using System.Threading;
using RTTE.Library.TextReader;
using LanguageExt;
using RTTE.Library.Native;
using RTTE.Library.Common.Interfaces;

namespace RTTE.UI {
    public partial class FindAddressesForm : Form {
        private readonly List<ProcessAddressHook> AddressList = new();
        private readonly List<ProcessMemoryScanner> MemoryScanners = new();

        private IWinAPI WinAPI { get; }
        private IEnumerable<AdvancedProcess> SelectedProcesses { get; }

        private bool Refining { get; set; } = false;
        private bool Scanning { get; set; } = false;
        private CancellationTokenSource CancellationToken { get; } = new();
        private CancellationTokenSource CancelScan { get; } = new();
        private Thread UpdateThread { get; set; }

        public FindAddressesForm(IEnumerable<AdvancedProcess> selectedProcesses) {
            InitializeComponent();

            WinAPI = API.Create();

            Text = string.Format(Text, LibraryUtils.GetCurrentArchitecture().ToString());

            SelectedProcesses = selectedProcesses;
        }

        private void Updater() {
            while (!CancellationToken.IsCancellationRequested) {
                bool canBeEnabled = TextTextBox.Text.Length > 0 && AddressHookDataGridView.Rows.Count > 1 && !Scanning && !Refining;

                try {
                    if (RefineButton.Enabled != canBeEnabled)
                        _ = Invoke(new MethodInvoker(() =>
                            RefineButton.Enabled = canBeEnabled
                        ));
                    _ = Invoke(new MethodInvoker(() => {
                        SearchButton.Enabled = TextTextBox.Text.Length > 0 && !Refining;
                        ButtonSelect.Enabled = AddressHookDataGridView.SelectedRows.Count > 0 && !Scanning && !Refining;
                    }));
                } catch (ObjectDisposedException) {
                    // Catch this as it is absolutely necessary
                }

                Thread.Sleep(10);
            }
        }

        private void FindAddressesForm_Load(object sender, EventArgs e) {
            EncodingComboBox.Items.AddRange(Encoding.GetEncodings().Select(eI => eI.GetEncoding().WebName).ToArray());
            EncodingComboBox.Items.Insert(0, "All");
            int utf16Index = EncodingComboBox.Items.IndexOf(Encoding.Unicode.WebName);
            EncodingComboBox.SelectedIndex = utf16Index;

            ProcessesDataGridView.ColumnCount = 4;
            DataGridViewColumn firstColumn = ProcessesDataGridView.Columns[0];
            firstColumn.SortMode = DataGridViewColumnSortMode.Automatic;
            firstColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            firstColumn.Width = 100;
            firstColumn.HeaderText = "Name";

            DataGridViewColumn secondColumn = ProcessesDataGridView.Columns[1];
            secondColumn.SortMode = DataGridViewColumnSortMode.Automatic;
            secondColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            secondColumn.Width = 40;
            secondColumn.HeaderText = "PID";

            DataGridViewColumn thirdColumn = ProcessesDataGridView.Columns[2];
            thirdColumn.SortMode = DataGridViewColumnSortMode.Automatic;
            thirdColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            thirdColumn.Width = 30;
            thirdColumn.HeaderText = "Platform";

            DataGridViewColumn fourthColumn = ProcessesDataGridView.Columns[3];
            fourthColumn.SortMode = DataGridViewColumnSortMode.Automatic;
            fourthColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            fourthColumn.Width = 100;
            fourthColumn.HeaderText = "Window Title";

            int newColumnIndex = ProcessesDataGridView.Columns.Add(new DataGridViewProgressColumn());
            DataGridViewColumn newColumn = ProcessesDataGridView.Columns[newColumnIndex];
            newColumn.SortMode = DataGridViewColumnSortMode.Automatic;
            newColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            newColumn.HeaderText = "Progress";

            AddressHookDataGridView.ColumnCount = 3;
            DataGridViewColumn firstAddressColumn = AddressHookDataGridView.Columns[0];
            firstAddressColumn.SortMode = DataGridViewColumnSortMode.Automatic;
            firstAddressColumn.Width = 40;
            firstAddressColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            firstAddressColumn.HeaderText = "PID";

            DataGridViewColumn secondAddressColumn = AddressHookDataGridView.Columns[1];
            secondAddressColumn.SortMode = DataGridViewColumnSortMode.Automatic;
            secondAddressColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            secondAddressColumn.Width = 80;
            secondAddressColumn.HeaderText = "Encoding";

            DataGridViewColumn thirdAddressColumn = AddressHookDataGridView.Columns[2];
            thirdAddressColumn.SortMode = DataGridViewColumnSortMode.Automatic;
            thirdAddressColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            thirdAddressColumn.HeaderText = "Address";

            IEnumerable<ProcessMemoryScanner> scanners = SelectedProcesses
                .Select(advancedProcess => ProcessMemoryScanner.Create(WinAPI, advancedProcess))
                .Somes();
            MemoryScanners.AddRange(scanners);

            foreach (ProcessMemoryScanner memoryScanner in MemoryScanners) {
                memoryScanner.OnProgressChanged += MemoryScanner_OnProgressChanged;
                memoryScanner.OnAddressFound += MemoryScanner_OnAddressFound;

                _ = ProcessesDataGridView.Rows.Add(memoryScanner.AsObjectArray());
            }

            UpdateThread = new(Updater);
            UpdateThread.Start();
        }

        private void AddAddressHookRow(ProcessAddressHook addressHook) {
            _ = Invoke(new MethodInvoker(() =>
                AddressHookDataGridView.Rows.Add(new object[] {
                    addressHook.PID,
                    addressHook.TextEncoding.WebName,
                    "0x" + addressHook.Address.ToString("X8")
                })
            ));
        }

        private void AddAddressHookRows(IEnumerable<ProcessAddressHook> addressHooks) {
            foreach (ProcessAddressHook addressHook in addressHooks)
                AddAddressHookRow(addressHook);
        }

        private void MemoryScanner_OnAddressFound(ProcessAddressHook addressHook) {
            AddressList.Add(addressHook);
            AddAddressHookRow(addressHook);
        }

        private void MemoryScanner_OnProgressChanged(int processId, int value) {
            foreach (DataGridViewRow row in ProcessesDataGridView.Rows) {
                if ((int)row.Cells[1].Value == processId) {
                    row.Cells[4].Value = value;
                }
            }
        }

        private async void SearchButton_Click(object sender, EventArgs e) {
            TextTextBox.Enabled = false;
            ButtonSelect.Enabled = false;
            EncodingComboBox.Enabled = false;
            BackButton.Enabled = false;
            Scanning = true;

            string selectedEncodingName = (string)EncodingComboBox.SelectedItem;

            bool searchAllEncodings = selectedEncodingName.Equals("All", StringComparison.InvariantCultureIgnoreCase);
            IEnumerable<Encoding> encodings = Encoding
                .GetEncodings()
                .Select(e => e.GetEncoding())
                .Where(e => searchAllEncodings || e.WebName == selectedEncodingName);

            if (SearchButton.Text.Equals("Reset", StringComparison.InvariantCultureIgnoreCase)) {
                AddressList.Clear();
                AddressHookDataGridView.Rows.Clear();
                TextTextBox.Clear();
                SearchButton.Text = "Search";
                LabelResults.Text = "";
                TextTextBox.Enabled = true;
                EncodingComboBox.Enabled = true;
                ButtonSelect.Enabled = true;
                BackButton.Enabled = true;
                Scanning = false;
                _ = TextTextBox.Focus();
            } else if (SearchButton.Text.Equals("Stop", StringComparison.InvariantCultureIgnoreCase)) {
                CancelScan.Cancel();
            } else {
                SearchButton.Text = "Stop";
                IWinAPI winAPI = API.Create();
                IEnumerable<ProcessAddressHook> addressHooks = (await Task.WhenAll(MemoryScanners.Select(memoryScanner => memoryScanner.Scan(
                    winAPI,
                    TextTextBox.Text,
                    encodings,
                    CancellationTokenSource.CreateLinkedTokenSource(CancellationToken.Token, CancelScan.Token)
                )))).SelectMany(a => a);
                int count = addressHooks.Count();
                _ = Invoke(new MethodInvoker(() => {
                    LabelResults.Text = count > 0 ?
                        $"Found {count} matching addresses!" :
                        "No matching addresses found! Please try again.";
                    LabelResults.ForeColor = count > 0 ?
                        Color.ForestGreen :
                        Color.Red;
                    TextTextBox.Enabled = true;
                    EncodingComboBox.Enabled = true;
                    ButtonSelect.Enabled = true;
                    BackButton.Enabled = true;
                    SearchButton.Text = count > 0 ?
                        "Reset" :
                        SearchButton.Text;
                    Scanning = false;
                }));
            }
        }

        private async void RefineButton_Click(object sender, EventArgs e) {
            TextTextBox.Enabled = false;
            ButtonSelect.Enabled = false;
            EncodingComboBox.Enabled = false;
            BackButton.Enabled = false;
            Refining = true;

            string selectedEncodingName = (string)EncodingComboBox.SelectedItem;

            bool searchAllEncodings = selectedEncodingName.Equals("All", StringComparison.InvariantCultureIgnoreCase);
            IEnumerable<Encoding> encodings = Encoding
                .GetEncodings()
                .Select(e => e.GetEncoding())
                .Where(e => searchAllEncodings || e.WebName == selectedEncodingName);

            IEnumerable<ProcessAddressHook> remaining = await ProcessMemoryScanner.Refine(
                WinAPI,
                AddressList,
                TextTextBox.Text,
                encodings
            );
            AddressList.Clear();
            AddressList.AddRange(remaining);
            _ = Invoke(new MethodInvoker(() => AddressHookDataGridView.Rows.Clear()));
            AddAddressHookRows(remaining);

            int count = remaining.Count();

            _ = Invoke(new MethodInvoker(() => {
                LabelResults.Text = count > 0 ?
                    $"Found {count} matching addresses!" :
                    "No matching addresses found! Please try again.";
                LabelResults.ForeColor = count > 0 ?
                    Color.ForestGreen :
                    Color.Red;
                TextTextBox.Enabled = true;
                EncodingComboBox.Enabled = true;
                ButtonSelect.Enabled = true;
                BackButton.Enabled = true;
                Refining = false;
            }));
        }

        private void ButtonClose_Click(object sender, EventArgs e) {
            if (MessageBox.Show(
                "Are you sure you want to exit RTTE?",
                "Exit RTTE?",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            ) == DialogResult.Yes)
                Application.Exit();
        }

        private void BackButton_Click(object sender, EventArgs e) {
            new ProcessSelectForm().Show();
            Close();
        }

        private void FindAddressesForm_FormClosing(object sender, FormClosingEventArgs e) {
            CancellationToken.Cancel();
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e) {
            _ = new AboutForm().ShowDialog();
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e) {
            Application.Exit();
        }

        private void ButtonSelect_Click(object sender, EventArgs e) {
            int PID = (int)AddressHookDataGridView.SelectedRows[0].Cells[0].Value;

            ProcessAddressHook addressHook = AddressList.First(a => a.PID == PID);
            new TextViewForm(new MemoryTextReader(
                API.Create(),
                addressHook.ProcessHandle,
                addressHook.Address,
                addressHook.TextEncoding
            )).Show();

            Close();
        }
    }
}
