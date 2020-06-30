namespace GGPlatform.reportmanag
{
    partial class frmReportManager
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmReportManager));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageList = new System.Windows.Forms.TabPage();
            this.treeViewList = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.tabPageData = new System.Windows.Forms.TabPage();
            this._dgvcontrol = new DataGridViewGGControl.DataGridViewGGControl();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnExcel = new System.Windows.Forms.ToolStripSplitButton();
            this.btnWord = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPageList.SuspendLayout();
            this.tabPageData.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Panel1MinSize = 240;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl);
            this.splitContainer1.Panel2.Controls.Add(this.toolStrip1);
            this.splitContainer1.Size = new System.Drawing.Size(921, 465);
            this.splitContainer1.SplitterDistance = 240;
            this.splitContainer1.SplitterWidth = 3;
            this.splitContainer1.TabIndex = 0;
            // 
            // tabControl
            // 
            this.tabControl.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabControl.Controls.Add(this.tabPageList);
            this.tabControl.Controls.Add(this.tabPageData);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.ImageList = this.imageList1;
            this.tabControl.Location = new System.Drawing.Point(0, 25);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(678, 440);
            this.tabControl.TabIndex = 1;
            // 
            // tabPageList
            // 
            this.tabPageList.Controls.Add(this.treeViewList);
            this.tabPageList.ImageIndex = 8;
            this.tabPageList.Location = new System.Drawing.Point(4, 4);
            this.tabPageList.Name = "tabPageList";
            this.tabPageList.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageList.Size = new System.Drawing.Size(670, 413);
            this.tabPageList.TabIndex = 0;
            this.tabPageList.Text = "Список отчётных форм";
            this.tabPageList.UseVisualStyleBackColor = true;
            // 
            // treeViewList
            // 
            this.treeViewList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewList.ImageIndex = 0;
            this.treeViewList.ImageList = this.imageList1;
            this.treeViewList.Location = new System.Drawing.Point(3, 3);
            this.treeViewList.Name = "treeViewList";
            this.treeViewList.SelectedImageIndex = 0;
            this.treeViewList.Size = new System.Drawing.Size(664, 407);
            this.treeViewList.TabIndex = 0;
            this.treeViewList.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewList_AfterSelect);
            this.treeViewList.KeyUp += new System.Windows.Forms.KeyEventHandler(this.treeViewList_KeyUp);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "02936.ico");
            this.imageList1.Images.SetKeyName(1, "db.ico");
            this.imageList1.Images.SetKeyName(2, "documents.ico");
            this.imageList1.Images.SetKeyName(3, "move.ico");
            this.imageList1.Images.SetKeyName(4, "MS-Office-2003-Excel-icon.png");
            this.imageList1.Images.SetKeyName(5, "MS-Office-2003-Word-icon.png");
            this.imageList1.Images.SetKeyName(6, "open.ico");
            this.imageList1.Images.SetKeyName(7, "prndoc.ico");
            this.imageList1.Images.SetKeyName(8, "tumbs.ico");
            this.imageList1.Images.SetKeyName(9, "folder.ico");
            // 
            // tabPageData
            // 
            this.tabPageData.Controls.Add(this._dgvcontrol);
            this.tabPageData.ImageIndex = 1;
            this.tabPageData.Location = new System.Drawing.Point(4, 4);
            this.tabPageData.Name = "tabPageData";
            this.tabPageData.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageData.Size = new System.Drawing.Size(670, 413);
            this.tabPageData.TabIndex = 1;
            this.tabPageData.Text = "Данные";
            this.tabPageData.UseVisualStyleBackColor = true;
            // 
            // _dgvcontrol
            // 
            this._dgvcontrol.Dock = System.Windows.Forms.DockStyle.Fill;
            this._dgvcontrol.Location = new System.Drawing.Point(3, 3);
            this._dgvcontrol.Name = "_dgvcontrol";
            this._dgvcontrol.Size = new System.Drawing.Size(664, 407);
            this._dgvcontrol.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnExcel});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(678, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnExcel
            // 
            this.btnExcel.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnWord});
            this.btnExcel.Image = ((System.Drawing.Image)(resources.GetObject("btnExcel.Image")));
            this.btnExcel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(156, 22);
            this.btnExcel.Text = "Сформировать отчет";
            this.btnExcel.ButtonClick += new System.EventHandler(this.btnExcel_ButtonClick);
            // 
            // btnWord
            // 
            this.btnWord.Image = global::GGPlatform.reportmanag.Properties.Resources.MS_Office_2003_Word_icon;
            this.btnWord.Name = "btnWord";
            this.btnWord.Size = new System.Drawing.Size(219, 22);
            this.btnWord.Text = "Сформировать в MS Word";
            this.btnWord.Click += new System.EventHandler(this.btnWord_Click);
            // 
            // frmReportManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(921, 465);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmReportManager";
            this.Text = "Менеджер отчётов";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.frmReportManager_Load);
            this.Shown += new System.EventHandler(this.frmReportManager_Shown);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabPageList.ResumeLayout(false);
            this.tabPageData.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageList;
        private System.Windows.Forms.TabPage tabPageData;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripSplitButton btnExcel;
        private System.Windows.Forms.ToolStripMenuItem btnWord;
        private System.Windows.Forms.TreeView treeViewList;
        private DataGridViewGGControl.DataGridViewGGControl _dgvcontrol;
    }
}

