using LanguageExt;
using RTTE.Library.Native;
using RTTE.Library.Process;
using RTTE.UI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibraryUtils = RTTE.Library.Common.Utils;
using RTTE.Library.PeFile;
using PeFileClass = PeNet.PeFile;
using RTTE.Library.File;
using RTTE.Library.Common.Interfaces;
using RTTE.UI.Entities;

namespace RTTE.UI {
    public partial class ProcessSelectForm : Form {
        private static class Constants {
            internal const uint UpdateIntervalMs = 100;
            internal const string HideInvalidTT = "Hides all processes with an invalid or unmatching processor architecture.";
            internal const string HideWindowlessTT = "Hides all processes which do not have a main window.";
            internal const string FilterTextBoxTT = "Filter processes by name, id, architecture or main window title.";
            internal const string RedIndicatorTT = "These processes are not running on the same processor architecture as RTTE is.";
            internal const string DarkGrayIndicatorTT = "These processes do not have a specified processor architecture. They might be system processes.";
            internal const string GreenIndicatorTT = "These processes are valid, have address hooks already pre-configured in a settings file, and are ready to be injected.";
            internal const string WhiteIndicatorTT = "These processes are valid, but are not pre-configured. They can be injected.";
            internal const string ButtonCloseTT = "Close the current window.";
            internal const string ButtonSelectAndContinueTT = "Continue with the selected processes.";
        }

        public delegate void ProcessesSelected(IEnumerable<IAdvancedProcess> processes);

        private readonly SortableBindingList<ProcessEntity> Processes = new();

        private CancellationTokenSource CancellationToken { get; set; }

        private Thread UpdateThread { get; set; }

        private ProcessList ProcessLister { get; }

        public ProcessSelectForm() {
            InitializeComponent();

            Text = string.Format(Text, LibraryUtils.GetCurrentArchitecture().ToString());

            ProcessLister = ProcessList.Create(API.Create(), PeFileParser.Create(BaseFile.Create(), PeFileParser.DefaultParser));

            HelpTooltip.SetToolTip(HideInvalidCheckBox, Constants.HideInvalidTT);
            HelpTooltip.SetToolTip(HideWindowlessCheckBox, Constants.HideWindowlessTT);
            HelpTooltip.SetToolTip(FilterTextBox, Constants.FilterTextBoxTT);
            HelpTooltip.SetToolTip(PanelRedIndicator, Constants.RedIndicatorTT);
            HelpTooltip.SetToolTip(LabelRedIndicator, Constants.RedIndicatorTT);
            HelpTooltip.SetToolTip(PanelDarkGrayIndicator, Constants.DarkGrayIndicatorTT);
            HelpTooltip.SetToolTip(LabelDarkGrayIndicator, Constants.DarkGrayIndicatorTT);
            HelpTooltip.SetToolTip(PanelGreenIndicator, Constants.GreenIndicatorTT);
            HelpTooltip.SetToolTip(LabelGreenIndicator, Constants.GreenIndicatorTT);
            HelpTooltip.SetToolTip(PanelWhiteIndicator, Constants.WhiteIndicatorTT);
            HelpTooltip.SetToolTip(LabelWhiteIndicator, Constants.WhiteIndicatorTT);
            HelpTooltip.SetToolTip(ButtonClose, Constants.ButtonCloseTT);
            HelpTooltip.SetToolTip(ButtonSelectAndContinue, Constants.ButtonSelectAndContinueTT);

            ProcessDataGrid.DataSource = Processes;

            DataGridViewColumn firstColumn = ProcessDataGrid.Columns[0];
            firstColumn.SortMode = DataGridViewColumnSortMode.Automatic;
            firstColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            firstColumn.Width = 60;
            firstColumn.HeaderText = "";

            DataGridViewColumn secondColumn = ProcessDataGrid.Columns[1];
            secondColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            secondColumn.Width = 160;
            secondColumn.HeaderText = "Name";

            DataGridViewColumn thirdColumn = ProcessDataGrid.Columns[2];
            thirdColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            thirdColumn.Width = 40;
            thirdColumn.HeaderText = "PID";

            DataGridViewColumn fourthColumn = ProcessDataGrid.Columns[3];
            fourthColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            fourthColumn.Width = 60;
            fourthColumn.HeaderText = "Platform";
            fourthColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewColumn fifthColumn = ProcessDataGrid.Columns[4];
            fifthColumn.HeaderText = "Window Text";
        }

        private void Resort() {
            Option<DataGridViewColumn> column = ProcessDataGrid.SortedColumn;
            ListSortDirection order = ProcessDataGrid.SortOrder == SortOrder.Ascending ?
                ListSortDirection.Ascending :
                ListSortDirection.Descending;

            _ = column.Match(
                column => Invoke(new MethodInvoker(() => ProcessDataGrid.Sort(column, order))),
                () => { }
            );
        }

        private void Continue(IEnumerable<AdvancedProcess> selectedProcesses) {
            bool differingProcessNames = selectedProcesses
                .Any(sp =>
                    selectedProcesses.Any(sp2 =>
                        !sp2.GetName().Equals(sp.GetName(), StringComparison.InvariantCultureIgnoreCase)
                    )
                );
            if (
                differingProcessNames &&
                MessageBox.Show(
                    "You have selected multiple processes with different names. Are you sure you want to continue?",
                    "Multiple processes with differing names selected",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                ) == DialogResult.No
            )
                return;

            IEnumerable<AdvancedProcess> invalidProcesses = selectedProcesses
                .Where(process => process.GetArchitecture() != LibraryUtils.GetCurrentArchitecture());
            IEnumerable<AdvancedProcess> validProcesses = selectedProcesses
                .Where(process => process.GetArchitecture() == LibraryUtils.GetCurrentArchitecture());

            foreach (AdvancedProcess invalidProcess in invalidProcesses)
                _ = MessageBox.Show(
                    $"Process {invalidProcess.GetName()} ({invalidProcess.GetId()}) cannot be injected!",
                    "Process cannot be injected",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

            if (invalidProcesses.Any() && selectedProcesses.Count() == 1)
                return;

            if (!validProcesses.Any()) {
                _ = MessageBox.Show(
                    "No valid processes found to inject! Please select the processes you want to inject again.",
                    "No valid processes to inject",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }

            bool doContinue = !invalidProcesses.Any() || MessageBox.Show(
                $"Some processes could not be injected. Would you like to inject the remaining {validProcesses.Count()} valid processes?",
                "Continue with valid processes?",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            ) == DialogResult.Yes;

            if (doContinue) {
                new FindAddressesForm(validProcesses).Show();
                Close();
            }
        }

        private void ProcessSelectForm_Load(object sender, EventArgs e) {
            CancellationToken = new();
        }

        private void ProcessSelectForm_Shown(object sender, EventArgs e) {
            Refresh();

            ProcessDataGrid.Sort(ProcessDataGrid.Columns[1], ListSortDirection.Ascending);

            UpdateThread = new Thread(Updater);
            UpdateThread.Start();
        }

        private async void Updater() {
            async Task<IAdvancedProcess> add(IAdvancedProcess process) {
                TaskCompletionSource<IAdvancedProcess> tcs = new(TaskCreationOptions.RunContinuationsAsynchronously);
                try {
                    _ = Invoke(new MethodInvoker(() => {
                        Processes.Add(ProcessEntity.Create(process));
                        tcs.SetResult(process);
                    }));
                } catch (InvalidOperationException) {
                }
                return await tcs.Task;
            }

            async Task<IAdvancedProcess> update(IAdvancedProcess process, int index) {
                TaskCompletionSource<IAdvancedProcess> tcs = new(TaskCreationOptions.RunContinuationsAsynchronously);
                try {
                    _ = Invoke(new MethodInvoker(() => {
                        Processes[index] = ProcessEntity.Create(process);
                        tcs.SetResult(process);
                    }));
                } catch (InvalidOperationException) {
                }
                return await tcs.Task;
            }

            async Task<IAdvancedProcess> delete(IAdvancedProcess process) {
                TaskCompletionSource<IAdvancedProcess> tcs = new(TaskCreationOptions.RunContinuationsAsynchronously);
                try {
                    _ = Invoke(new MethodInvoker(() => {
                        _ = Processes.Remove(ProcessEntity.Create(process));
                        tcs.SetResult(process);
                    }));
                } catch (InvalidOperationException) {
                }
                return await tcs.Task;
            }

            while (!CancellationToken.IsCancellationRequested) {
                ProcessFilterOptions filterOptions = new(
                    FilterTextBox.Text,
                    HideInvalidCheckBox.Checked,
                    HideWindowlessCheckBox.Checked,
                    LibraryUtils.GetCurrentArchitecture()
                );
                IEnumerable<Option<IAdvancedProcess>> addedUpdatedRemoved = await ProcessLister.AddDeleteUpdate(
                    Process.GetProcesses().Select(BaseProcess.Create),
                    Processes.Select(p => p.Process).ToArray(),
                    filterOptions,
                    add,
                    update,
                    delete
                );
                if (addedUpdatedRemoved.Somes().Any())
                    Resort();

                Thread.Sleep((int)Constants.UpdateIntervalMs);
            }
        }

        private void ProcessSelectForm_FormClosing(object sender, FormClosingEventArgs e) {
            CancellationToken.Cancel();
        }

        private void ProcessDataGrid_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e) {
            for (int index = e.RowIndex; index <= e.RowIndex + e.RowCount - 1; index++) {
                DataGridViewRow row = ProcessDataGrid.Rows[index];
                ProcessEntity underlyingData = (ProcessEntity)row.DataBoundItem;

                row.MinimumHeight = 32;
                row.DefaultCellStyle.BackColor = underlyingData.Process.GetArchitecture()
                    .Match(architecture =>
                        LibraryUtils.GetCurrentArchitecture() != architecture ?
                            Color.Red :
                            Color.White,
                        Color.LightGray
                    );
            }
        }

        private void ProcessDataGrid_SelectionChanged(object sender, EventArgs e) {
            IEnumerable<DataGridViewRow> rows = ProcessDataGrid.SelectedRows.Cast<DataGridViewRow>();
            _ = ButtonSelectAndContinue.Invoke(new MethodInvoker(() =>
                ButtonSelectAndContinue.Enabled =
                    rows.Any() &&
                    rows.All(row => ((ProcessEntity)row.DataBoundItem).Process.GetArchitecture() == LibraryUtils.GetCurrentArchitecture())
            ));
        }

        private void ProcessDataGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e) {
            Continue(ProcessDataGrid.SelectedRows.Cast<DataGridViewRow>().Select(dgvr => (AdvancedProcess)dgvr.DataBoundItem));
        }

        private void ButtonSelectAndContinue_Click(object sender, EventArgs e) {
            Continue(ProcessDataGrid.SelectedRows.Cast<DataGridViewRow>().Select(dgvr => (AdvancedProcess)dgvr.DataBoundItem));
        }

        private void ButtonClose_Click(object sender, EventArgs e) {
            new MainForm().Show();
            Close();
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e) {
            _ = new AboutForm().ShowDialog();
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e) {
            Application.Exit();
        }
    }
}
