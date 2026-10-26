namespace CbrConverter
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.layoutRoot = new System.Windows.Forms.TableLayoutPanel();
            this.lbl_SourceHeading = new System.Windows.Forms.Label();
            this.layoutSource = new System.Windows.Forms.TableLayoutPanel();
            this.tbox_SourceFile = new System.Windows.Forms.TextBox();
            this.btn_SelectFolder = new System.Windows.Forms.Button();
            this.lbl_SourceHint = new System.Windows.Forms.Label();
            this.lbl_OutputHeading = new System.Windows.Forms.Label();
            this.layoutOutput = new System.Windows.Forms.TableLayoutPanel();
            this.rb_SameFolder = new System.Windows.Forms.RadioButton();
            this.rb_CustomFolder = new System.Windows.Forms.RadioButton();
            this.tbox_OutputFolder = new System.Windows.Forms.TextBox();
            this.btn_SelectOutput = new System.Windows.Forms.Button();
            this.lbl_ConvertHeading = new System.Windows.Forms.Label();
            this.layoutOptions = new System.Windows.Forms.TableLayoutPanel();
            this.cmb_From = new System.Windows.Forms.ComboBox();
            this.lbl_To = new System.Windows.Forms.Label();
            this.cmb_To = new System.Windows.Forms.ComboBox();
            this.chk_JoinImages = new System.Windows.Forms.CheckBox();
            this.lbl_ProgressHeading = new System.Windows.Forms.Label();
            this.layoutProgress = new System.Windows.Forms.TableLayoutPanel();
            this.lbl_ProcessingFile = new System.Windows.Forms.Label();
            this.lbl_FileCounter = new System.Windows.Forms.Label();
            this.pbar_ActualFile = new System.Windows.Forms.ProgressBar();
            this.lbl_Overall = new System.Windows.Forms.Label();
            this.pbar_TotalProgress = new System.Windows.Forms.ProgressBar();
            this.layoutActions = new System.Windows.Forms.TableLayoutPanel();
            this.lnk_About = new System.Windows.Forms.LinkLabel();
            this.btn_StartStop = new System.Windows.Forms.Button();
            this.layoutLogHeader = new System.Windows.Forms.TableLayoutPanel();
            this.lbl_LogHeading = new System.Windows.Forms.Label();
            this.btn_ClearLog = new System.Windows.Forms.Button();
            this.listViewLog = new System.Windows.Forms.ListView();
            this.columnHeaderTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderInfo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.layoutRoot.SuspendLayout();
            this.layoutSource.SuspendLayout();
            this.layoutOutput.SuspendLayout();
            this.layoutOptions.SuspendLayout();
            this.layoutProgress.SuspendLayout();
            this.layoutActions.SuspendLayout();
            this.layoutLogHeader.SuspendLayout();
            this.SuspendLayout();
            //
            // layoutRoot
            //
            this.layoutRoot.ColumnCount = 1;
            this.layoutRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutRoot.Controls.Add(this.lbl_SourceHeading, 0, 0);
            this.layoutRoot.Controls.Add(this.layoutSource, 0, 1);
            this.layoutRoot.Controls.Add(this.lbl_SourceHint, 0, 2);
            this.layoutRoot.Controls.Add(this.lbl_OutputHeading, 0, 3);
            this.layoutRoot.Controls.Add(this.layoutOutput, 0, 4);
            this.layoutRoot.Controls.Add(this.lbl_ConvertHeading, 0, 5);
            this.layoutRoot.Controls.Add(this.layoutOptions, 0, 6);
            this.layoutRoot.Controls.Add(this.lbl_ProgressHeading, 0, 7);
            this.layoutRoot.Controls.Add(this.layoutProgress, 0, 8);
            this.layoutRoot.Controls.Add(this.layoutActions, 0, 9);
            this.layoutRoot.Controls.Add(this.layoutLogHeader, 0, 10);
            this.layoutRoot.Controls.Add(this.listViewLog, 0, 11);
            this.layoutRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutRoot.Location = new System.Drawing.Point(0, 0);
            this.layoutRoot.Name = "layoutRoot";
            this.layoutRoot.Padding = new System.Windows.Forms.Padding(12, 6, 12, 12);
            this.layoutRoot.RowCount = 12;
            this.layoutRoot.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutRoot.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutRoot.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutRoot.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutRoot.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutRoot.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutRoot.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutRoot.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutRoot.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutRoot.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutRoot.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutRoot.Size = new System.Drawing.Size(624, 681);
            this.layoutRoot.TabIndex = 0;
            //
            // lbl_SourceHeading
            //
            this.lbl_SourceHeading.AutoSize = true;
            this.lbl_SourceHeading.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lbl_SourceHeading.Margin = new System.Windows.Forms.Padding(0, 6, 0, 3);
            this.lbl_SourceHeading.Name = "lbl_SourceHeading";
            this.lbl_SourceHeading.Text = "Source";
            //
            // layoutSource
            //
            this.layoutSource.AutoSize = true;
            this.layoutSource.ColumnCount = 2;
            this.layoutSource.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutSource.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.layoutSource.Controls.Add(this.tbox_SourceFile, 0, 0);
            this.layoutSource.Controls.Add(this.btn_SelectFolder, 1, 0);
            this.layoutSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutSource.Margin = new System.Windows.Forms.Padding(0);
            this.layoutSource.Name = "layoutSource";
            this.layoutSource.RowCount = 1;
            this.layoutSource.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutSource.TabIndex = 1;
            //
            // tbox_SourceFile
            //
            this.tbox_SourceFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbox_SourceFile.Margin = new System.Windows.Forms.Padding(0, 3, 6, 3);
            this.tbox_SourceFile.Name = "tbox_SourceFile";
            this.tbox_SourceFile.PlaceholderText = "No folder selected";
            this.tbox_SourceFile.ReadOnly = true;
            this.tbox_SourceFile.TabStop = false;
            //
            // btn_SelectFolder
            //
            this.btn_SelectFolder.AutoSize = true;
            this.btn_SelectFolder.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.btn_SelectFolder.MinimumSize = new System.Drawing.Size(80, 27);
            this.btn_SelectFolder.Name = "btn_SelectFolder";
            this.btn_SelectFolder.TabIndex = 1;
            this.btn_SelectFolder.Text = "Browse...";
            this.btn_SelectFolder.UseVisualStyleBackColor = true;
            this.btn_SelectFolder.Click += new System.EventHandler(this.btn_SelectFolder_Click);
            //
            // lbl_SourceHint
            //
            this.lbl_SourceHint.AutoSize = true;
            this.lbl_SourceHint.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lbl_SourceHint.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.lbl_SourceHint.Name = "lbl_SourceHint";
            this.lbl_SourceHint.Text = "To convert a single file, drag it onto this window.";
            //
            // lbl_OutputHeading
            //
            this.lbl_OutputHeading.AutoSize = true;
            this.lbl_OutputHeading.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lbl_OutputHeading.Margin = new System.Windows.Forms.Padding(0, 10, 0, 3);
            this.lbl_OutputHeading.Name = "lbl_OutputHeading";
            this.lbl_OutputHeading.Text = "Output";
            //
            // layoutOutput
            //
            this.layoutOutput.AutoSize = true;
            this.layoutOutput.ColumnCount = 4;
            this.layoutOutput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.layoutOutput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.layoutOutput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutOutput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.layoutOutput.Controls.Add(this.rb_SameFolder, 0, 0);
            this.layoutOutput.Controls.Add(this.rb_CustomFolder, 1, 0);
            this.layoutOutput.Controls.Add(this.tbox_OutputFolder, 2, 0);
            this.layoutOutput.Controls.Add(this.btn_SelectOutput, 3, 0);
            this.layoutOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutOutput.Margin = new System.Windows.Forms.Padding(0);
            this.layoutOutput.Name = "layoutOutput";
            this.layoutOutput.RowCount = 1;
            this.layoutOutput.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutOutput.TabIndex = 2;
            //
            // rb_SameFolder
            //
            this.rb_SameFolder.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.rb_SameFolder.AutoSize = true;
            this.rb_SameFolder.Checked = true;
            this.rb_SameFolder.Margin = new System.Windows.Forms.Padding(0, 3, 12, 3);
            this.rb_SameFolder.Name = "rb_SameFolder";
            this.rb_SameFolder.TabIndex = 0;
            this.rb_SameFolder.TabStop = true;
            this.rb_SameFolder.Text = "Same folder as source";
            this.rb_SameFolder.UseVisualStyleBackColor = true;
            this.rb_SameFolder.CheckedChanged += new System.EventHandler(this.rb_OutputFolder_CheckedChanged);
            //
            // rb_CustomFolder
            //
            this.rb_CustomFolder.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.rb_CustomFolder.AutoSize = true;
            this.rb_CustomFolder.Margin = new System.Windows.Forms.Padding(0, 3, 6, 3);
            this.rb_CustomFolder.Name = "rb_CustomFolder";
            this.rb_CustomFolder.TabIndex = 1;
            this.rb_CustomFolder.Text = "Other folder:";
            this.rb_CustomFolder.UseVisualStyleBackColor = true;
            this.rb_CustomFolder.CheckedChanged += new System.EventHandler(this.rb_OutputFolder_CheckedChanged);
            //
            // tbox_OutputFolder
            //
            this.tbox_OutputFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbox_OutputFolder.Enabled = false;
            this.tbox_OutputFolder.Margin = new System.Windows.Forms.Padding(0, 3, 6, 3);
            this.tbox_OutputFolder.Name = "tbox_OutputFolder";
            this.tbox_OutputFolder.PlaceholderText = "No output folder selected";
            this.tbox_OutputFolder.ReadOnly = true;
            this.tbox_OutputFolder.TabStop = false;
            //
            // btn_SelectOutput
            //
            this.btn_SelectOutput.AutoSize = true;
            this.btn_SelectOutput.Enabled = false;
            this.btn_SelectOutput.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.btn_SelectOutput.MinimumSize = new System.Drawing.Size(80, 27);
            this.btn_SelectOutput.Name = "btn_SelectOutput";
            this.btn_SelectOutput.TabIndex = 2;
            this.btn_SelectOutput.Text = "Browse...";
            this.btn_SelectOutput.UseVisualStyleBackColor = true;
            this.btn_SelectOutput.Click += new System.EventHandler(this.btn_SelectOutput_Click);
            //
            // lbl_ConvertHeading
            //
            this.lbl_ConvertHeading.AutoSize = true;
            this.lbl_ConvertHeading.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lbl_ConvertHeading.Margin = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.lbl_ConvertHeading.Name = "lbl_ConvertHeading";
            this.lbl_ConvertHeading.Text = "Convert";
            //
            // layoutOptions
            //
            this.layoutOptions.AutoSize = true;
            this.layoutOptions.ColumnCount = 4;
            this.layoutOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.layoutOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.layoutOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.layoutOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutOptions.Controls.Add(this.cmb_From, 0, 0);
            this.layoutOptions.Controls.Add(this.lbl_To, 1, 0);
            this.layoutOptions.Controls.Add(this.cmb_To, 2, 0);
            this.layoutOptions.Controls.Add(this.chk_JoinImages, 0, 1);
            this.layoutOptions.SetColumnSpan(this.chk_JoinImages, 4);
            this.layoutOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutOptions.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.layoutOptions.Name = "layoutOptions";
            this.layoutOptions.RowCount = 2;
            this.layoutOptions.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutOptions.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutOptions.TabIndex = 3;
            //
            // cmb_From
            //
            this.cmb_From.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_From.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.cmb_From.Name = "cmb_From";
            this.cmb_From.Size = new System.Drawing.Size(150, 23);
            this.cmb_From.TabIndex = 0;
            this.cmb_From.SelectedIndexChanged += new System.EventHandler(this.cmb_From_SelectedIndexChanged);
            //
            // lbl_To
            //
            this.lbl_To.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lbl_To.AutoSize = true;
            this.lbl_To.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.lbl_To.Name = "lbl_To";
            this.lbl_To.Text = "to";
            //
            // cmb_To
            //
            this.cmb_To.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_To.Margin = new System.Windows.Forms.Padding(0);
            this.cmb_To.Name = "cmb_To";
            this.cmb_To.Size = new System.Drawing.Size(150, 23);
            this.cmb_To.TabIndex = 1;
            this.cmb_To.SelectionChangeCommitted += new System.EventHandler(this.cmb_To_SelectionChangeCommitted);
            //
            // chk_JoinImages
            //
            this.chk_JoinImages.AutoSize = true;
            this.chk_JoinImages.Margin = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.chk_JoinImages.Name = "chk_JoinImages";
            this.chk_JoinImages.TabIndex = 2;
            this.chk_JoinImages.Text = "Merge sliced images back into whole pages";
            this.chk_JoinImages.UseVisualStyleBackColor = true;
            //
            // lbl_ProgressHeading
            //
            this.lbl_ProgressHeading.AutoSize = true;
            this.lbl_ProgressHeading.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lbl_ProgressHeading.Margin = new System.Windows.Forms.Padding(0, 10, 0, 3);
            this.lbl_ProgressHeading.Name = "lbl_ProgressHeading";
            this.lbl_ProgressHeading.Text = "Progress";
            //
            // layoutProgress
            //
            this.layoutProgress.AutoSize = true;
            this.layoutProgress.ColumnCount = 2;
            this.layoutProgress.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutProgress.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.layoutProgress.Controls.Add(this.lbl_ProcessingFile, 0, 0);
            this.layoutProgress.Controls.Add(this.lbl_FileCounter, 1, 0);
            this.layoutProgress.Controls.Add(this.pbar_ActualFile, 0, 1);
            this.layoutProgress.Controls.Add(this.lbl_Overall, 0, 2);
            this.layoutProgress.Controls.Add(this.pbar_TotalProgress, 0, 3);
            this.layoutProgress.SetColumnSpan(this.pbar_ActualFile, 2);
            this.layoutProgress.SetColumnSpan(this.lbl_Overall, 2);
            this.layoutProgress.SetColumnSpan(this.pbar_TotalProgress, 2);
            this.layoutProgress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutProgress.Margin = new System.Windows.Forms.Padding(0);
            this.layoutProgress.Name = "layoutProgress";
            this.layoutProgress.RowCount = 4;
            this.layoutProgress.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutProgress.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutProgress.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutProgress.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutProgress.TabIndex = 4;
            //
            // lbl_ProcessingFile
            //
            this.lbl_ProcessingFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_ProcessingFile.AutoEllipsis = true;
            this.lbl_ProcessingFile.Margin = new System.Windows.Forms.Padding(0, 0, 6, 3);
            this.lbl_ProcessingFile.Name = "lbl_ProcessingFile";
            this.lbl_ProcessingFile.Size = new System.Drawing.Size(100, 20);
            this.lbl_ProcessingFile.Text = "Ready";
            this.lbl_ProcessingFile.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            //
            // lbl_FileCounter
            //
            this.lbl_FileCounter.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lbl_FileCounter.AutoSize = true;
            this.lbl_FileCounter.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lbl_FileCounter.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.lbl_FileCounter.Name = "lbl_FileCounter";
            //
            // pbar_ActualFile
            //
            this.pbar_ActualFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.pbar_ActualFile.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.pbar_ActualFile.Name = "pbar_ActualFile";
            this.pbar_ActualFile.Size = new System.Drawing.Size(100, 16);
            //
            // lbl_Overall
            //
            this.lbl_Overall.AutoSize = true;
            this.lbl_Overall.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.lbl_Overall.Name = "lbl_Overall";
            this.lbl_Overall.Text = "Overall";
            //
            // pbar_TotalProgress
            //
            this.pbar_TotalProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.pbar_TotalProgress.Margin = new System.Windows.Forms.Padding(0);
            this.pbar_TotalProgress.Name = "pbar_TotalProgress";
            this.pbar_TotalProgress.Size = new System.Drawing.Size(100, 16);
            //
            // layoutActions
            //
            this.layoutActions.AutoSize = true;
            this.layoutActions.ColumnCount = 2;
            this.layoutActions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutActions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.layoutActions.Controls.Add(this.lnk_About, 0, 0);
            this.layoutActions.Controls.Add(this.btn_StartStop, 1, 0);
            this.layoutActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutActions.Margin = new System.Windows.Forms.Padding(0, 12, 0, 0);
            this.layoutActions.Name = "layoutActions";
            this.layoutActions.RowCount = 1;
            this.layoutActions.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutActions.TabIndex = 5;
            //
            // lnk_About
            //
            this.lnk_About.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lnk_About.AutoSize = true;
            this.lnk_About.Margin = new System.Windows.Forms.Padding(0);
            this.lnk_About.Name = "lnk_About";
            this.lnk_About.TabIndex = 1;
            this.lnk_About.TabStop = true;
            this.lnk_About.Text = "About";
            this.lnk_About.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnk_About_LinkClicked);
            //
            // btn_StartStop
            //
            this.btn_StartStop.Enabled = false;
            this.btn_StartStop.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btn_StartStop.Margin = new System.Windows.Forms.Padding(0);
            this.btn_StartStop.Name = "btn_StartStop";
            this.btn_StartStop.Size = new System.Drawing.Size(130, 36);
            this.btn_StartStop.TabIndex = 0;
            this.btn_StartStop.Text = "Start";
            this.btn_StartStop.UseVisualStyleBackColor = true;
            this.btn_StartStop.Click += new System.EventHandler(this.btn_StartStop_Click);
            //
            // layoutLogHeader
            //
            this.layoutLogHeader.AutoSize = true;
            this.layoutLogHeader.ColumnCount = 2;
            this.layoutLogHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutLogHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.layoutLogHeader.Controls.Add(this.lbl_LogHeading, 0, 0);
            this.layoutLogHeader.Controls.Add(this.btn_ClearLog, 1, 0);
            this.layoutLogHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutLogHeader.Margin = new System.Windows.Forms.Padding(0, 10, 0, 3);
            this.layoutLogHeader.Name = "layoutLogHeader";
            this.layoutLogHeader.RowCount = 1;
            this.layoutLogHeader.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutLogHeader.TabIndex = 6;
            //
            // lbl_LogHeading
            //
            this.lbl_LogHeading.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lbl_LogHeading.AutoSize = true;
            this.lbl_LogHeading.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lbl_LogHeading.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_LogHeading.Name = "lbl_LogHeading";
            this.lbl_LogHeading.Text = "Warnings and errors";
            //
            // btn_ClearLog
            //
            this.btn_ClearLog.AutoSize = true;
            this.btn_ClearLog.Margin = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.btn_ClearLog.Name = "btn_ClearLog";
            this.btn_ClearLog.MinimumSize = new System.Drawing.Size(80, 0);
            this.btn_ClearLog.TabIndex = 0;
            this.btn_ClearLog.Text = "Clear";
            this.btn_ClearLog.UseVisualStyleBackColor = true;
            this.btn_ClearLog.Click += new System.EventHandler(this.btn_ClearLog_Click);
            //
            // listViewLog
            //
            this.listViewLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderTime,
            this.columnHeaderType,
            this.columnHeaderInfo});
            this.listViewLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewLog.FullRowSelect = true;
            this.listViewLog.Margin = new System.Windows.Forms.Padding(0);
            this.listViewLog.Name = "listViewLog";
            this.listViewLog.OwnerDraw = true;
            this.listViewLog.ShowItemToolTips = true;
            this.listViewLog.TabIndex = 7;
            this.listViewLog.UseCompatibleStateImageBehavior = false;
            this.listViewLog.View = System.Windows.Forms.View.Details;
            this.listViewLog.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.listViewLog_DrawColumnHeader);
            this.listViewLog.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.listViewLog_DrawItem);
            this.listViewLog.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.listViewLog_DrawSubItem);
            this.listViewLog.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listViewLog_KeyDown);
            this.listViewLog.Resize += new System.EventHandler(this.listViewLog_Resize);
            //
            // columnHeaderTime
            //
            this.columnHeaderTime.Text = "Time";
            this.columnHeaderTime.Width = 70;
            //
            // columnHeaderType
            //
            this.columnHeaderType.Text = "Type";
            this.columnHeaderType.Width = 70;
            //
            // columnHeaderInfo
            //
            this.columnHeaderInfo.Text = "Message";
            this.columnHeaderInfo.Width = 460;
            //
            // MainForm
            //
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 681);
            this.Controls.Add(this.layoutRoot);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CBR Converter";
            this.layoutRoot.ResumeLayout(false);
            this.layoutRoot.PerformLayout();
            this.layoutSource.ResumeLayout(false);
            this.layoutSource.PerformLayout();
            this.layoutOutput.ResumeLayout(false);
            this.layoutOutput.PerformLayout();
            this.layoutOptions.ResumeLayout(false);
            this.layoutOptions.PerformLayout();
            this.layoutProgress.ResumeLayout(false);
            this.layoutProgress.PerformLayout();
            this.layoutActions.ResumeLayout(false);
            this.layoutActions.PerformLayout();
            this.layoutLogHeader.ResumeLayout(false);
            this.layoutLogHeader.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel layoutRoot;
        private System.Windows.Forms.Label lbl_SourceHeading;
        private System.Windows.Forms.TableLayoutPanel layoutSource;
        private System.Windows.Forms.TextBox tbox_SourceFile;
        private System.Windows.Forms.Button btn_SelectFolder;
        private System.Windows.Forms.Label lbl_SourceHint;
        private System.Windows.Forms.Label lbl_OutputHeading;
        private System.Windows.Forms.TableLayoutPanel layoutOutput;
        private System.Windows.Forms.RadioButton rb_SameFolder;
        private System.Windows.Forms.RadioButton rb_CustomFolder;
        private System.Windows.Forms.TextBox tbox_OutputFolder;
        private System.Windows.Forms.Button btn_SelectOutput;
        private System.Windows.Forms.Label lbl_ConvertHeading;
        private System.Windows.Forms.TableLayoutPanel layoutOptions;
        private System.Windows.Forms.ComboBox cmb_From;
        private System.Windows.Forms.Label lbl_To;
        private System.Windows.Forms.ComboBox cmb_To;
        private System.Windows.Forms.CheckBox chk_JoinImages;
        private System.Windows.Forms.Label lbl_ProgressHeading;
        private System.Windows.Forms.TableLayoutPanel layoutProgress;
        private System.Windows.Forms.Label lbl_ProcessingFile;
        private System.Windows.Forms.Label lbl_FileCounter;
        private System.Windows.Forms.ProgressBar pbar_ActualFile;
        private System.Windows.Forms.Label lbl_Overall;
        private System.Windows.Forms.ProgressBar pbar_TotalProgress;
        private System.Windows.Forms.TableLayoutPanel layoutActions;
        private System.Windows.Forms.LinkLabel lnk_About;
        private System.Windows.Forms.Button btn_StartStop;
        private System.Windows.Forms.TableLayoutPanel layoutLogHeader;
        private System.Windows.Forms.Label lbl_LogHeading;
        private System.Windows.Forms.Button btn_ClearLog;
        private System.Windows.Forms.ListView listViewLog;
        private System.Windows.Forms.ColumnHeader columnHeaderTime;
        private System.Windows.Forms.ColumnHeader columnHeaderType;
        private System.Windows.Forms.ColumnHeader columnHeaderInfo;
    }
}
