namespace GGPlatform.adminapp
{
    partial class frmAdminMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAdminMain));
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Настройки", 7, 7);
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Пользователи", 2, 2);
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Рабочие места", 1, 1);
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Интерфейсы", 6, 6);
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Группы отчетов", 5, 5);
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Отчетные формы", 3, 3);
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Объекты системы", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4,
            treeNode5,
            treeNode6});
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.pnTreeView = new System.Windows.Forms.Panel();
            this.treeView = new System.Windows.Forms.TreeView();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel1 = new System.Windows.Forms.Panel();
            this._dgvcontrol = new DataGridViewGGControl.DataGridViewGGControl();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tbNew = new System.Windows.Forms.ToolStripButton();
            this.tbDel = new System.Windows.Forms.ToolStripButton();
            this.tbProp = new System.Windows.Forms.ToolStripButton();
            this.pnTreeView.SuspendLayout();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "regedit.ico");
            this.imageList1.Images.SetKeyName(1, "map.ico");
            this.imageList1.Images.SetKeyName(2, "users.ico");
            this.imageList1.Images.SetKeyName(3, "02936.ico");
            this.imageList1.Images.SetKeyName(4, "objects.ico");
            this.imageList1.Images.SetKeyName(5, "tumbs.ico");
            this.imageList1.Images.SetKeyName(6, "exec.ico");
            this.imageList1.Images.SetKeyName(7, "settings.ico");
            // 
            // pnTreeView
            // 
            this.pnTreeView.Controls.Add(this.treeView);
            this.pnTreeView.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnTreeView.Location = new System.Drawing.Point(0, 0);
            this.pnTreeView.Name = "pnTreeView";
            this.pnTreeView.Size = new System.Drawing.Size(200, 646);
            this.pnTreeView.TabIndex = 7;
            // 
            // treeView
            // 
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.ImageIndex = 0;
            this.treeView.ImageList = this.imageList1;
            this.treeView.Location = new System.Drawing.Point(0, 0);
            this.treeView.Name = "treeView";
            treeNode1.ImageIndex = 7;
            treeNode1.Name = "itemTreeSettings";
            treeNode1.SelectedImageIndex = 7;
            treeNode1.Text = "Настройки";
            treeNode2.ImageIndex = 2;
            treeNode2.Name = "itemTreeUser";
            treeNode2.SelectedImageIndex = 2;
            treeNode2.Text = "Пользователи";
            treeNode3.ImageIndex = 1;
            treeNode3.Name = "itemTreeWorkplace";
            treeNode3.SelectedImageIndex = 1;
            treeNode3.Text = "Рабочие места";
            treeNode4.ImageIndex = 6;
            treeNode4.Name = "itemTreeInterface";
            treeNode4.SelectedImageIndex = 6;
            treeNode4.Text = "Интерфейсы";
            treeNode5.ImageIndex = 5;
            treeNode5.Name = "itemTreeReportGroup";
            treeNode5.SelectedImageIndex = 5;
            treeNode5.Text = "Группы отчетов";
            treeNode6.ImageIndex = 3;
            treeNode6.Name = "itemTreeReport";
            treeNode6.SelectedImageIndex = 3;
            treeNode6.Text = "Отчетные формы";
            treeNode7.Name = "itemTree";
            treeNode7.Text = "Объекты системы";
            this.treeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode7});
            this.treeView.SelectedImageIndex = 0;
            this.treeView.Size = new System.Drawing.Size(200, 646);
            this.treeView.TabIndex = 7;
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(200, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 646);
            this.splitter1.TabIndex = 8;
            this.splitter1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._dgvcontrol);
            this.panel1.Controls.Add(this.toolStrip1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(203, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(931, 646);
            this.panel1.TabIndex = 9;
            // 
            // _dgvcontrol
            // 
            this._dgvcontrol.Dock = System.Windows.Forms.DockStyle.Fill;
            this._dgvcontrol.Location = new System.Drawing.Point(0, 25);
            this._dgvcontrol.Name = "_dgvcontrol";
            this._dgvcontrol.Size = new System.Drawing.Size(931, 621);
            this._dgvcontrol.TabIndex = 8;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbNew,
            this.tbDel,
            this.tbProp});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(931, 25);
            this.toolStrip1.TabIndex = 7;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tbNew
            // 
            this.tbNew.Image = ((System.Drawing.Image)(resources.GetObject("tbNew.Image")));
            this.tbNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbNew.Name = "tbNew";
            this.tbNew.Size = new System.Drawing.Size(79, 22);
            this.tbNew.Text = "Добавить";
            this.tbNew.ToolTipText = "Добавить запись";
            this.tbNew.Click += new System.EventHandler(this.tbNew_Click);
            // 
            // tbDel
            // 
            this.tbDel.Image = ((System.Drawing.Image)(resources.GetObject("tbDel.Image")));
            this.tbDel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbDel.Name = "tbDel";
            this.tbDel.Size = new System.Drawing.Size(71, 22);
            this.tbDel.Text = "Удалить";
            this.tbDel.ToolTipText = "Удалить запись";
            this.tbDel.Click += new System.EventHandler(this.tbDel_Click);
            // 
            // tbProp
            // 
            this.tbProp.Image = ((System.Drawing.Image)(resources.GetObject("tbProp.Image")));
            this.tbProp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbProp.Name = "tbProp";
            this.tbProp.Size = new System.Drawing.Size(81, 22);
            this.tbProp.Text = "Изменить";
            this.tbProp.ToolTipText = "Изменить запись";
            this.tbProp.Click += new System.EventHandler(this.tbProp_Click);
            // 
            // frmAdminMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1134, 646);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.pnTreeView);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmAdminMain";
            this.Text = "Администрирование";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmAdminMain_FormClosed);
            this.Load += new System.EventHandler(this.frmAdminMain_Load);
            this.pnTreeView.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Panel pnTreeView;
        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tbNew;
        private System.Windows.Forms.ToolStripButton tbDel;
        private System.Windows.Forms.ToolStripButton tbProp;
        private DataGridViewGGControl.DataGridViewGGControl _dgvcontrol;


    }
}

