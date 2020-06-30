namespace SimRodnNom
{
    partial class frmSimRodnNom
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSimRodnNom));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tbImport = new System.Windows.Forms.ToolStripButton();
            this.tbEditt = new System.Windows.Forms.ToolStripButton();
            this.btZakaz = new System.Windows.Forms.ToolStripDropDownButton();
            this.btClearZakaz = new System.Windows.Forms.ToolStripMenuItem();
            this.tbExportZakaz = new System.Windows.Forms.ToolStripMenuItem();
            this.tbAnaliz = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this._dgvcontrolM = new DataGridViewGGControl.DataGridViewGGControl();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbImport,
            this.tbEditt,
            this.btZakaz,
            this.tbAnaliz});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1180, 25);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.Visible = false;
            // 
            // tbImport
            // 
            this.tbImport.Image = ((System.Drawing.Image)(resources.GetObject("tbImport.Image")));
            this.tbImport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbImport.Name = "tbImport";
            this.tbImport.Size = new System.Drawing.Size(156, 22);
            this.tbImport.Text = "Импорт номенклатуры";
            this.tbImport.Click += new System.EventHandler(this.tbImport_Click);
            // 
            // tbEditt
            // 
            this.tbEditt.Image = ((System.Drawing.Image)(resources.GetObject("tbEditt.Image")));
            this.tbEditt.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbEditt.Name = "tbEditt";
            this.tbEditt.Size = new System.Drawing.Size(81, 22);
            this.tbEditt.Text = "Изменить";
            this.tbEditt.Click += new System.EventHandler(this.tbEditt_Click);
            // 
            // btZakaz
            // 
            this.btZakaz.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btClearZakaz,
            this.tbExportZakaz});
            this.btZakaz.Image = ((System.Drawing.Image)(resources.GetObject("btZakaz.Image")));
            this.btZakaz.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btZakaz.Name = "btZakaz";
            this.btZakaz.Size = new System.Drawing.Size(66, 22);
            this.btZakaz.Text = "Заказ";
            // 
            // btClearZakaz
            // 
            this.btClearZakaz.Name = "btClearZakaz";
            this.btClearZakaz.Size = new System.Drawing.Size(162, 22);
            this.btClearZakaz.Text = "Очистить";
            this.btClearZakaz.Click += new System.EventHandler(this.btactiveChangeData_Click);
            // 
            // tbExportZakaz
            // 
            this.tbExportZakaz.Name = "tbExportZakaz";
            this.tbExportZakaz.Size = new System.Drawing.Size(162, 22);
            this.tbExportZakaz.Text = "Выгрузить заказ";
            this.tbExportZakaz.Click += new System.EventHandler(this.tbExportZakaz_Click);
            // 
            // tbAnaliz
            // 
            this.tbAnaliz.Image = ((System.Drawing.Image)(resources.GetObject("tbAnaliz.Image")));
            this.tbAnaliz.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbAnaliz.Name = "tbAnaliz";
            this.tbAnaliz.Size = new System.Drawing.Size(144, 22);
            this.tbAnaliz.Text = "Анализ поставщиков";
            this.tbAnaliz.Click += new System.EventHandler(this.tbAnaliz_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1180, 389);
            this.panel1.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this._dgvcontrolM);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1180, 389);
            this.panel3.TabIndex = 2;
            // 
            // _dgvcontrolM
            // 
            this._dgvcontrolM.Dock = System.Windows.Forms.DockStyle.Fill;
            this._dgvcontrolM.Location = new System.Drawing.Point(0, 0);
            this._dgvcontrolM.Name = "_dgvcontrolM";
            this._dgvcontrolM.Size = new System.Drawing.Size(1180, 389);
            this._dgvcontrolM.TabIndex = 0;
            // 
            // frmSimRodnNom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1180, 414);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmSimRodnNom";
            this.Text = "Работа с товаром";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private DataGridViewGGControl.DataGridViewGGControl _dgvcontrolM;
        private System.Windows.Forms.ToolStripDropDownButton btZakaz;
        private System.Windows.Forms.ToolStripMenuItem btClearZakaz;
        private System.Windows.Forms.ToolStripMenuItem tbExportZakaz;
        private System.Windows.Forms.ToolStripButton tbImport;
        private System.Windows.Forms.ToolStripButton tbEditt;
        private System.Windows.Forms.ToolStripButton tbAnaliz;
    }
}

