using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace CbrConverter
{
    public partial class MainForm : Form
    {
        bool _fileSelected = false;
        bool _outputFolderSelected = false;

        // last locations picked in each dialog, remembered for this session only
        string _lastSourcePath = string.Empty;
        string _lastOutputPath = string.Empty;

        public MainForm()
        {
            InitializeComponent();
            this.MinimumSize = new Size(this.Width, this.Height - 120);
        }

       

        private void btn_StartStop_Click(object sender, EventArgs e)
        {

            if (DataAccess.Instance.g_Processing)
            {
                DataAccess.Instance.g_Processing = false;
                btn_StartStop.Text = "START";
                lbl_ProcessingFile.Text = string.Empty;
            }
            else
            {
                bool cbr2pdf = chk_cbr2pdf.Checked && chk_cbr2pdf.Enabled;
                bool pdf2cbr = chk_pdf2cbr.Checked && chk_pdf2cbr.Enabled;
                if ((cbr2pdf || pdf2cbr) && (_fileSelected))
                {
                    btn_StartStop.Text = "STOP";
                    DataAccess.Instance.g_ReduceSize = chk_ReduceSize.Checked;
                    DataAccess.Instance.g_Processing = true;
                    Extract ex = new Extract();
                    this.Subscribe(ex);
                    ex.BeginExtraction(cbr2pdf, pdf2cbr, chk_ReduceSize.Checked, chk_NbPages.Checked, chk_JoinImages.Checked, chk_NbPages.Checked);
                }
            }
        }

        

        public void Subscribe(Extract m)
        {
            m.evnt_UpdateCurBar += new Extract.UpdateCurrentBar(UpdateCurrBar);
            m.evnt_UpdatTotBar += new Extract.UpdateTotalBar(UpdateTotaBar);
            m.evnt_UpdateFileName += new Extract.UpdateFileName(UpdateFileName);
            m.evnt_ErrorNotify += new Extract.ErrorNotify(ErrorShowPopup);
            PdfFunctions.evnt_UpdateCurBar += new PdfFunctions.UpdateCurrentBar(UpdateCurrBar);
        }
        private void UpdateCurrBar()
        {           
            this.Invoke((MethodInvoker)delegate
            {
                if (DataAccess.Instance.g_curProgress > 100)
                    DataAccess.Instance.g_curProgress = 100;
                pbar_ActualFile.Value = (int)DataAccess.Instance.g_curProgress;
            });
        }

        private void UpdateFileName(Extract m, EventArgs e)
        {

            this.Invoke((MethodInvoker)delegate
            {
                lbl_ProcessingFile.Text = Path.GetFileName(DataAccess.Instance.g_WorkingFile);
                if (lbl_ProcessingFile.Text == string.Empty)//finished
                {
                    btn_StartStop.Text = "START";
                   
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

        private void ErrorShowPopup(Extract m, string e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                //btn_StartStop.Text = "START";
                //pbar_TotalProgress.Value = 0;
                //pbar_ActualFile.Value = 0;
                //lbl_ProcessingFile.Text =string.Empty;
                var item = new ListViewItem(new List<string>{ (listViewLog.Items.Count + 1).ToString(), e}.ToArray());
                item.ToolTipText = e;
                listViewLog.Items.Add(item);
                item.EnsureVisible();
                //this.textBoxLog.Text += e + Environment.NewLine;
                ResizeLogColumn();
            });
            //MessageBox.Show(e, "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void btn_SelectFile_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Title = "Select a file to convert";
                dlg.Filter = "Comics and PDFs (*.cbr;*.cbz;*.rar;*.zip;*.pdf)|*.cbr;*.cbz;*.rar;*.zip;*.pdf|All files (*.*)|*.*";
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
                dlg.Description = "Select a folder to convert (subfolders are processed too)";
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
            _lastSourcePath = path;
            DataAccess.Instance.g_WorkingDir = path;
            tbox_SourceFile.Text = path;

            // only enable the conversions that apply to the selection, but keep what the user ticked
            string ext = File.Exists(path) ? Path.GetExtension(path).ToLower() : string.Empty;
            bool isFolder = Directory.Exists(path);
            bool isArchive = ext == ".cbr" || ext == ".cbz" || ext == ".rar" || ext == ".zip";
            chk_cbr2pdf.Enabled = isFolder || isArchive;
            chk_pdf2cbr.Enabled = isFolder || ext == ".pdf";
            chk_JoinImages.Enabled = chk_pdf2cbr.Enabled;
            chk_NbPages.Enabled = chk_pdf2cbr.Enabled;

            _fileSelected = true;

            if (this.chk_SourceFolder.Checked)
            {
                _outputFolderSelected = true;
                DataAccess.Instance.g_Output_dir = Path.GetDirectoryName(path);
                this.tbox_OuputFolder.Text = this.tbox_SourceFile.Text;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tbox_SourceFile.SelectionStart = 0;
        }

        private void lbl_about_Click(object sender, EventArgs e)
        {
            AboutorForm aboutform = new AboutorForm();
            aboutform.ShowDialog();
        }


        /// <summary>
        /// Widen the message column to fit the longest message (the list scrolls sideways if needed),
        /// but never narrower than the visible area.
        /// </summary>
        private void ResizeLogColumn()
        {
            listViewLog.AutoResizeColumn(1, ColumnHeaderAutoResizeStyle.ColumnContent);
            int available = listViewLog.ClientSize.Width - columnHeaderNb.Width;
            if (columnHeaderInfo.Width < available)
                columnHeaderInfo.Width = available;
        }

        private void listViewLog_Resize(object sender, EventArgs e)
        {
            ResizeLogColumn();
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
                var lines = listViewLog.SelectedItems.Cast<ListViewItem>().Select(i => i.SubItems[1].Text);
                Clipboard.SetText(string.Join(Environment.NewLine, lines));
                e.Handled = true;
            }
        }

        private void tbox_OuputFolder_Click(object sender, EventArgs e)
        {
            using (var dlg = new FolderBrowserDialog())
            {
                dlg.Description = "Select an output folder";
                dlg.UseDescriptionForTitle = true;
                dlg.SelectedPath = _lastOutputPath != string.Empty ? _lastOutputPath : LastSourceFolder(); //reopen where we were last

                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    _lastOutputPath = dlg.SelectedPath;
                    DataAccess.Instance.g_Output_dir = dlg.SelectedPath;
                    tbox_OuputFolder.Text = dlg.SelectedPath;

                    _outputFolderSelected = true;
                }
            }
        }

        private void chk_SourceFolder_CheckedChanged(object sender, EventArgs e)
        {           
            this.tbox_OuputFolder.Enabled = !this.chk_SourceFolder.Checked;
            if (this.chk_SourceFolder.Checked)
            {
                DataAccess.Instance.g_Output_dir = Path.GetDirectoryName(DataAccess.Instance.g_WorkingDir);
                this.tbox_OuputFolder.Text = DataAccess.Instance.g_Output_dir;
            }
        }
    }
}
