namespace SimWorkT
{
    partial class frmSimWorkT
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSimWorkT));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tbEdit = new System.Windows.Forms.ToolStripDropDownButton();
            this.tbEditPhoneNum = new System.Windows.Forms.ToolStripMenuItem();
            this.tbCena_Product = new System.Windows.Forms.ToolStripMenuItem();
            this.btActive = new System.Windows.Forms.ToolStripDropDownButton();
            this.tbActive = new System.Windows.Forms.ToolStripMenuItem();
            this.btactiveChangeData = new System.Windows.Forms.ToolStripMenuItem();
            this.tbRFA = new System.Windows.Forms.ToolStripDropDownButton();
            this.tbInputRFA = new System.Windows.Forms.ToolStripMenuItem();
            this.tbRFA_WD = new System.Windows.Forms.ToolStripMenuItem();
            this.tbRFA_Provider = new System.Windows.Forms.ToolStripMenuItem();
            this.btVoznagr = new System.Windows.Forms.ToolStripButton();
            this.btReports = new System.Windows.Forms.ToolStripDropDownButton();
            this.btTovarReport = new System.Windows.Forms.ToolStripMenuItem();
            this.tbSopost = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this._dgvcontrolM = new DataGridViewGGControl.DataGridViewGGControl();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this._dgvcontrolD = new DataGridViewGGControl.DataGridViewGGControl();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbEdit,
            this.btActive,
            this.tbRFA,
            this.btVoznagr,
            this.btReports});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1180, 25);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.Visible = false;
            // 
            // tbEdit
            // 
            this.tbEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbEditPhoneNum,
            this.tbCena_Product});
            this.tbEdit.Image = ((System.Drawing.Image)(resources.GetObject("tbEdit.Image")));
            this.tbEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbEdit.Name = "tbEdit";
            this.tbEdit.Size = new System.Drawing.Size(98, 22);
            this.tbEdit.Text = "Изменение";
            // 
            // tbEditPhoneNum
            // 
            this.tbEditPhoneNum.Name = "tbEditPhoneNum";
            this.tbEditPhoneNum.Size = new System.Drawing.Size(168, 22);
            this.tbEditPhoneNum.Text = "Номер телефона";
            this.tbEditPhoneNum.Click += new System.EventHandler(this.tbEditPhoneNum_Click);
            // 
            // tbCena_Product
            // 
            this.tbCena_Product.Name = "tbCena_Product";
            this.tbCena_Product.Size = new System.Drawing.Size(168, 22);
            this.tbCena_Product.Text = "Цена / продукт";
            this.tbCena_Product.Click += new System.EventHandler(this.tbCena_Product_Click);
            // 
            // btActive
            // 
            this.btActive.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbActive,
            this.btactiveChangeData});
            this.btActive.Image = ((System.Drawing.Image)(resources.GetObject("btActive.Image")));
            this.btActive.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btActive.Name = "btActive";
            this.btActive.Size = new System.Drawing.Size(134, 22);
            this.btActive.Text = "Активация товара";
            // 
            // tbActive
            // 
            this.tbActive.Name = "tbActive";
            this.tbActive.Size = new System.Drawing.Size(246, 22);
            this.tbActive.Text = "Активация";
            this.tbActive.Click += new System.EventHandler(this.tbActive_Click);
            // 
            // btactiveChangeData
            // 
            this.btactiveChangeData.Name = "btactiveChangeData";
            this.btactiveChangeData.Size = new System.Drawing.Size(246, 22);
            this.btactiveChangeData.Text = "Активация с перезаписью даты";
            this.btactiveChangeData.Click += new System.EventHandler(this.btactiveChangeData_Click);
            // 
            // tbRFA
            // 
            this.tbRFA.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbInputRFA,
            this.tbRFA_WD,
            this.tbRFA_Provider});
            this.tbRFA.Image = ((System.Drawing.Image)(resources.GetObject("tbRFA.Image")));
            this.tbRFA.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbRFA.Name = "tbRFA";
            this.tbRFA.Size = new System.Drawing.Size(187, 22);
            this.tbRFA.Text = "Регистр. формы абонентов";
            // 
            // tbInputRFA
            // 
            this.tbInputRFA.Name = "tbInputRFA";
            this.tbInputRFA.Size = new System.Drawing.Size(242, 22);
            this.tbInputRFA.Text = "Принятые РФА";
            this.tbInputRFA.Click += new System.EventHandler(this.tbInputRFA_Click);
            // 
            // tbRFA_WD
            // 
            this.tbRFA_WD.Name = "tbRFA_WD";
            this.tbRFA_WD.Size = new System.Drawing.Size(242, 22);
            this.tbRFA_WD.Text = "Регистрация РФА у Web Dealer";
            this.tbRFA_WD.Click += new System.EventHandler(this.tbRFA_WD_Click);
            // 
            // tbRFA_Provider
            // 
            this.tbRFA_Provider.Name = "tbRFA_Provider";
            this.tbRFA_Provider.Size = new System.Drawing.Size(242, 22);
            this.tbRFA_Provider.Text = "РФА сданные оператору";
            this.tbRFA_Provider.Click += new System.EventHandler(this.tbRFA_Provider_Click);
            // 
            // btVoznagr
            // 
            this.btVoznagr.Image = ((System.Drawing.Image)(resources.GetObject("btVoznagr.Image")));
            this.btVoznagr.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btVoznagr.Name = "btVoznagr";
            this.btVoznagr.Size = new System.Drawing.Size(118, 22);
            this.btVoznagr.Text = "Вознаграждение";
            this.btVoznagr.Click += new System.EventHandler(this.btVoznagr_Click);
            // 
            // btReports
            // 
            this.btReports.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btTovarReport,
            this.tbSopost});
            this.btReports.Image = ((System.Drawing.Image)(resources.GetObject("btReports.Image")));
            this.btReports.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btReports.Name = "btReports";
            this.btReports.Size = new System.Drawing.Size(77, 22);
            this.btReports.Text = "Отчеты";
            // 
            // btTovarReport
            // 
            this.btTovarReport.Image = ((System.Drawing.Image)(resources.GetObject("btTovarReport.Image")));
            this.btTovarReport.Name = "btTovarReport";
            this.btTovarReport.Size = new System.Drawing.Size(221, 22);
            this.btTovarReport.Text = "Список товара по складам";
            this.btTovarReport.Click += new System.EventHandler(this.btTovarReport_Click);
            // 
            // tbSopost
            // 
            this.tbSopost.Image = ((System.Drawing.Image)(resources.GetObject("tbSopost.Image")));
            this.tbSopost.Name = "tbSopost";
            this.tbSopost.Size = new System.Drawing.Size(221, 22);
            this.tbSopost.Text = "Сопоставление номеров";
            this.tbSopost.Click += new System.EventHandler(this.tbSopost_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.splitter1);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1180, 389);
            this.panel1.TabIndex = 1;
            this.panel1.Resize += new System.EventHandler(this.panel1_Resize);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this._dgvcontrolM);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1180, 229);
            this.panel3.TabIndex = 2;
            // 
            // _dgvcontrolM
            // 
            this._dgvcontrolM.Dock = System.Windows.Forms.DockStyle.Fill;
            this._dgvcontrolM.Location = new System.Drawing.Point(0, 0);
            this._dgvcontrolM.Name = "_dgvcontrolM";
            this._dgvcontrolM.Size = new System.Drawing.Size(1180, 229);
            this._dgvcontrolM.TabIndex = 0;
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter1.Location = new System.Drawing.Point(0, 229);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(1180, 3);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 232);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1180, 157);
            this.panel2.TabIndex = 0;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this._dgvcontrolD);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1180, 157);
            this.panel4.TabIndex = 1;
            // 
            // _dgvcontrolD
            // 
            this._dgvcontrolD.Dock = System.Windows.Forms.DockStyle.Fill;
            this._dgvcontrolD.Location = new System.Drawing.Point(0, 0);
            this._dgvcontrolD.Name = "_dgvcontrolD";
            this._dgvcontrolD.Size = new System.Drawing.Size(1180, 157);
            this._dgvcontrolD.TabIndex = 0;
            // 
            // frmSimWorkT
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1180, 414);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmSimWorkT";
            this.Text = "Работа с товаром";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private DataGridViewGGControl.DataGridViewGGControl _dgvcontrolM;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel4;
        private DataGridViewGGControl.DataGridViewGGControl _dgvcontrolD;
        private System.Windows.Forms.ToolStripDropDownButton btReports;
        private System.Windows.Forms.ToolStripMenuItem btTovarReport;
        private System.Windows.Forms.ToolStripButton btVoznagr;
        private System.Windows.Forms.ToolStripDropDownButton btActive;
        private System.Windows.Forms.ToolStripMenuItem btactiveChangeData;
        private System.Windows.Forms.ToolStripMenuItem tbActive;
        private System.Windows.Forms.ToolStripDropDownButton tbRFA;
        private System.Windows.Forms.ToolStripMenuItem tbInputRFA;
        private System.Windows.Forms.ToolStripMenuItem tbRFA_WD;
        private System.Windows.Forms.ToolStripMenuItem tbRFA_Provider;
        private System.Windows.Forms.ToolStripDropDownButton tbEdit;
        private System.Windows.Forms.ToolStripMenuItem tbEditPhoneNum;
        private System.Windows.Forms.ToolStripMenuItem tbCena_Product;
        private System.Windows.Forms.ToolStripMenuItem tbSopost;
    }
}

