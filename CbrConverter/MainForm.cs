using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CbrConverter
{
    public partial class MainForm : Form
    {
        bool _fileSelected = false;

        // last locations picked in each dialog
        string _lastSourcePath = string.Empty;
        string _lastOutputPath = string.Empty;

        static readonly Color WarningColor = Color.FromArgb(150, 85, 0);
        static readonly Color ErrorColor = Color.Firebrick;

        /// <param name="startPath">file or folder passed on the command line (e.g. dropped onto the exe)</param>
        public MainForm(string startPath = null)
        {
            InitializeComponent();
            PdfFunctions.evnt_UpdateCurBar += new PdfFunctions.UpdateCurrentBar(UpdateCurrBar);
            EnableDragDrop(this);

            if (File.Exists(startPath) || Directory.Exists(startPath))
                SetSource(Path.GetFullPath(startPath));
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            // keep everything above the log visible, plus a few log rows
            this.MinimumSize = new Size(560, this.Height - listViewLog.Height + 100);
            ResizeLogColumn();
        }

        #region Start / stop and progress

        private void btn_StartStop_Click(object sender, EventArgs e)
        {
            if (DataAccess.Instance.g_Processing)
            {
                // the conversion thread stops after the current step and then reports it has finished
                DataAccess.Instance.g_Processing = false;
                btn_StartStop.Text = "Stopping...";
                btn_StartStop.Enabled = false;
                return;
            }

            // only what is both ticked and applicable to the source
            bool comicToPdf = chk_cbr2pdf.Checked && chk_cbr2pdf.Enabled;
            bool comicToFolder = chk_ComicToFolder.Checked && chk_ComicToFolder.Enabled;
            bool pdfToCbz = chk_pdf2cbr.Checked && chk_pdf2cbr.Enabled;
            bool pdfToFolder = chk_PdfToFolder.Checked && chk_PdfToFolder.Enabled;
            if (!_fileSelected)
                return;
            if (!comicToPdf && !comicToFolder && !pdfToCbz && !pdfToFolder)
            {
                MessageBox.Show(this, "Tick at least one conversion to run.", "CBR Converter", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (rb_CustomFolder.Checked && !Directory.Exists(_lastOutputPath))
            {
                MessageBox.Show(this, "Choose an output folder first, or select \"Same folder as source\".", "CBR Converter", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataAccess.Instance.g_Output_dir = rb_SameFolder.Checked ? SourceFolder() : _lastOutputPath;

            DataAccess.Instance.g_curProgress = 0;
            DataAccess.Instance.g_totProgress = 0;
            pbar_ActualFile.Value = 0;
            pbar_TotalProgress.Value = 0;

            SetRunning(true);
            DataAccess.Instance.g_Processing = true;
            Extract ex = new Extract();
            this.Subscribe(ex);
            ex.BeginExtraction(comicToPdf, comicToFolder, pdfToCbz, pdfToFolder, chk_JoinImages.Checked);
        }

        /// <summary>
        /// Lock the settings while converting
        /// </summary>
        private void SetRunning(bool running)
        {
            btn_SelectFile.Enabled = !running;
            btn_SelectFolder.Enabled = !running;
            layoutOutput.Enabled = !running;
            layoutOptions.Enabled = !running;
            btn_StartStop.Text = running ? "Stop" : "Start";
            btn_StartStop.Enabled = running || _fileSelected;
            if (running)
            {
                lbl_ProcessingFile.Text = string.Empty;
                lbl_FileCounter.Text = string.Empty;
            }
        }

        public void Subscribe(Extract m)
        {
            m.evnt_UpdateCurBar += new Extract.UpdateCurrentBar(UpdateCurrBar);
            m.evnt_UpdatTotBar += new Extract.UpdateTotalBar(UpdateTotaBar);
            m.evnt_UpdateFileName += new Extract.UpdateFileName(UpdateFileName);
            m.evnt_ErrorNotify += new Extract.ErrorNotify(LogError);
            m.evnt_WarningNotify += new Extract.ErrorNotify(LogWarning);
        }

        private void UpdateCurrBar()
        {
            this.Invoke((MethodInvoker)delegate
            {
                if (DataAccess.Instance.g_curProgress > 100)
                    DataAccess.Instance.g_curProgress = 100;
                pbar_ActualFile.Value = (int)DataAccess.Instance.g_curProgress;
                int count = DataAccess.Instance.g_FileCount;
                if (count > 0 && DataAccess.Instance.g_FileIndex > 0)
                    pbar_TotalProgress.Value = (int)Math.Min(100, ((DataAccess.Instance.g_FileIndex - 1) * 100 + DataAccess.Instance.g_curProgress) / count);
            });
        }

        private void UpdateFileName(Extract m, EventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                string file = Path.GetFileName(DataAccess.Instance.g_WorkingFile);
                if (string.IsNullOrEmpty(file)) //finished
                {
                    SetRunning(false);
                    lbl_ProcessingFile.Text = "Finished";
                    lbl_FileCounter.Text = string.Empty;
                    pbar_ActualFile.Value = 0;
                }
                else
                {
                    lbl_ProcessingFile.Text = file;
                    if (DataAccess.Instance.g_FileCount > 0)
                        lbl_FileCounter.Text = string.Format("File {0} of {1}", DataAccess.Instance.g_FileIndex, DataAccess.Instance.g_FileCount);
                }
            });
        }

        private void UpdateTotaBar(Extract m, EventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                if (DataAccess.Instance.g_totProgress > 100)
                    DataAccess.Instance.g_totProgress = 100;
                pbar_TotalProgress.Value = (int)DataAccess.Instance.g_totProgress;
            });
        }

        #endregion

        #region Source and output selection

        private void btn_SelectFile_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Title = "Select a file to convert";
                dlg.Filter = "Comics and PDFs (*.cbr;*.cbz;*.pdf)|*.cbr;*.cbz;*.pdf";
                dlg.InitialDirectory = LastSourceFolder(); //reopen where we were last
                if (File.Exists(_lastSourcePath))
                    dlg.FileName = Path.GetFileName(_lastSourcePath);

                if (dlg.ShowDialog(this) == DialogResult.OK)
                    SetSource(dlg.FileName);
            }
        }

        private void btn_SelectFolder_Click(object sender, EventArgs e)
        {
            using (var dlg = new FolderBrowserDialog())
            {
                dlg.Description = "Select a folder to convert (subfolders are converted too)";
                dlg.UseDescriptionForTitle = true;
                dlg.SelectedPath = LastSourceFolder(); //reopen where we were last

                if (dlg.ShowDialog(this) == DialogResult.OK)
                    SetSource(dlg.SelectedPath);
            }
        }

        /// <summary>
        /// Folder of the last selected source (the file's folder if a file was selected)
        /// </summary>
        private string LastSourceFolder()
        {
            if (File.Exists(_lastSourcePath))
                return Path.GetDirectoryName(_lastSourcePath);
            return _lastSourcePath;
        }

        private void SetSource(string path)
        {
            string type = Path.GetExtension(path).ToLower();
            if (File.Exists(path) && type != ".cbr" && type != ".cbz" && type != ".pdf")
            {
                LogWarning(null, path + " : not a CBR, CBZ or PDF file, so it can't be converted");
                return;
            }

            _lastSourcePath = path;
            DataAccess.Instance.g_WorkingDir = path;
            tbox_SourceFile.Text = path;
            tbox_SourceFile.SelectionStart = tbox_SourceFile.Text.Length; //show the end of long paths

            // only enable the conversions that apply to the selection, but keep what the user ticked
            string ext = File.Exists(path) ? Path.GetExtension(path).ToLower() : string.Empty;
            bool isFolder = Directory.Exists(path);
            bool isArchive = ext == ".cbr" || ext == ".cbz";
            lbl_ComicTo.Enabled = chk_cbr2pdf.Enabled = chk_ComicToFolder.Enabled = isFolder || isArchive;
            lbl_PdfTo.Enabled = chk_pdf2cbr.Enabled = chk_PdfToFolder.Enabled = isFolder || ext == ".pdf";
            UpdateMergeOption();

            _fileSelected = true;
            btn_StartStop.Enabled = true;
            lbl_ProcessingFile.Text = "Ready";
            lbl_FileCounter.Text = string.Empty;
            UpdateOutputDisplay();
        }

        private void btn_SelectOutput_Click(object sender, EventArgs e)
        {
            using (var dlg = new FolderBrowserDialog())
            {
                dlg.Description = "Select an output folder";
                dlg.UseDescriptionForTitle = true;
                dlg.SelectedPath = _lastOutputPath != string.Empty ? _lastOutputPath : LastSourceFolder(); //reopen where we were last

                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    _lastOutputPath = dlg.SelectedPath;
                    UpdateOutputDisplay();
                }
            }
        }

        private void rb_OutputFolder_CheckedChanged(object sender, EventArgs e)
        {
            tbox_OutputFolder.Enabled = rb_CustomFolder.Checked;
            btn_SelectOutput.Enabled = rb_CustomFolder.Checked;
            UpdateOutputDisplay();
        }

        private void UpdateOutputDisplay()
        {
            if (rb_SameFolder.Checked)
                tbox_OutputFolder.Text = _fileSelected ? SourceFolder() : string.Empty;
            else
                tbox_OutputFolder.Text = _lastOutputPath;
            tbox_OutputFolder.SelectionStart = tbox_OutputFolder.Text.Length;
        }

        /// <summary>
        /// The selected folder, or the folder of the selected file
        /// </summary>
        private string SourceFolder()
        {
            string source = DataAccess.Instance.g_WorkingDir;
            return Directory.Exists(source) ? source : Path.GetDirectoryName(source);
        }

        private void chk_PdfOutput_CheckedChanged(object sender, EventArgs e)
        {
            UpdateMergeOption();
        }

        /// <summary>
        /// Merging only applies when converting PDFs
        /// </summary>
        private void UpdateMergeOption()
        {
            chk_JoinImages.Enabled = chk_pdf2cbr.Enabled && (chk_pdf2cbr.Checked || chk_PdfToFolder.Checked);
        }

        #endregion

        #region Drag and drop

        /// <summary>
        /// Accept a dropped file or folder anywhere on the window
        /// </summary>
        private void EnableDragDrop(Control control)
        {
            control.AllowDrop = true;
            control.DragEnter += Control_DragEnter;
            control.DragDrop += Control_DragDrop;
            foreach (Control child in control.Controls)
                EnableDragDrop(child);
        }

        private void Control_DragEnter(object sender, DragEventArgs e)
        {
            bool canDrop = !DataAccess.Instance.g_Processing && e.Data.GetDataPresent(DataFormats.FileDrop);
            e.Effect = canDrop ? DragDropEffects.Copy : DragDropEffects.None;
        }

        private void Control_DragDrop(object sender, DragEventArgs e)
        {
            var paths = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (paths == null || paths.Length == 0 || DataAccess.Instance.g_Processing)
                return;

            SetSource(paths[0]);
            if (paths.Length > 1)
                LogWarning(null, string.Format("{0} items were dropped; only {1} will be converted. Drop a folder to convert several files.", paths.Length, paths[0]));
        }

        #endregion

        #region Log

        private void LogError(Extract m, string message)
        {
            AddLogEntry("Error", message, ErrorColor);
        }

        private void LogWarning(Extract m, string message)
        {
            AddLogEntry("Warning", message, WarningColor);
        }

        private void AddLogEntry(string type, string message, Color color)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate { AddLogEntry(type, message, color); });
                return;
            }

            var item = new ListViewItem(new[] { DateTime.Now.ToString("HH:mm:ss"), type, message });
            item.ForeColor = color;
            item.ToolTipText = message;
            listViewLog.Items.Add(item);
            item.EnsureVisible();
            ResizeLogColumn();
        }

        /// <summary>
        /// Widen the message column to fit the longest message (the list scrolls sideways if needed),
        /// but never narrower than the visible area.
        /// </summary>
        private void ResizeLogColumn()
        {
            if (listViewLog.Items.Count > 0)
                listViewLog.AutoResizeColumn(2, ColumnHeaderAutoResizeStyle.ColumnContent);
            // a few pixels short of the full width, or the list shows a needless horizontal scrollbar
            int available = listViewLog.ClientSize.Width - columnHeaderTime.Width - columnHeaderType.Width - 4;
            if (columnHeaderInfo.Width < available)
                columnHeaderInfo.Width = available;
        }

        private void listViewLog_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.Graphics.FillRectangle(SystemBrushes.Window, e.Bounds);
            var textBounds = new Rectangle(e.Bounds.X + 6, e.Bounds.Y, e.Bounds.Width - 6, e.Bounds.Height - 1);
            TextRenderer.DrawText(e.Graphics, e.Header.Text, listViewLog.Font, textBounds, SystemColors.ControlText,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis);
            // separator between columns and a line under the whole header
            e.Graphics.DrawLine(SystemPens.ControlLight, e.Bounds.Right - 1, e.Bounds.Top + 4, e.Bounds.Right - 1, e.Bounds.Bottom - 5);
            e.Graphics.DrawLine(SystemPens.ControlDark, e.Bounds.Left, e.Bounds.Bottom - 1, e.Bounds.Right, e.Bounds.Bottom - 1);
        }

        private void listViewLog_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void listViewLog_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void listViewLog_Resize(object sender, EventArgs e)
        {
            ResizeLogColumn();
        }

        private string LogText(IEnumerable<ListViewItem> items)
        {
            return string.Join(Environment.NewLine, items.Select(i => string.Join("\t", i.SubItems.Cast<ListViewItem.ListViewSubItem>().Select(s => s.Text))));
        }

        /// <summary>
        /// Ctrl+C copies the selected log messages, Ctrl+A selects them all
        /// </summary>
        private void listViewLog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                foreach (ListViewItem item in listViewLog.Items)
                    item.Selected = true;
                e.Handled = true;
            }
            else if (e.Control && e.KeyCode == Keys.C && listViewLog.SelectedItems.Count > 0)
            {
                Clipboard.SetText(LogText(listViewLog.SelectedItems.Cast<ListViewItem>()));
                e.Handled = true;
            }
        }

        private void btn_ClearLog_Click(object sender, EventArgs e)
        {
            listViewLog.Items.Clear();
            ResizeLogColumn();
        }

        #endregion

        private void lnk_About_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (var aboutform = new AboutorForm())
                aboutform.ShowDialog(this);
        }
    }
}
