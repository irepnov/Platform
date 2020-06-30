namespace WSProf
{
    partial class frmWSProf
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmWSProf));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tbImport = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.btActive = new System.Windows.Forms.ToolStripDropDownButton();
            this.tbExportMonit = new System.Windows.Forms.ToolStripMenuItem();
            this.обработатьПротоколToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.обработкаРезультатовИнформированияИзВнешнегоИсточникаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ручноИнформированиеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.экпортВТФОМСToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.непринятыеЗаписиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel5 = new System.Windows.Forms.Panel();
            this._dgvcontrolM = new DataGridViewGGControl.DataGridViewGGControl();
            this.panel2 = new System.Windows.Forms.Panel();
            this._dgvcontrolD = new DataGridViewGGControl.DataGridViewGGControl();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbImport,
            this.toolStripButton2,
            this.btActive});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1028, 25);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.Visible = false;
            // 
            // tbImport
            // 
            this.tbImport.Image = ((System.Drawing.Image)(resources.GetObject("tbImport.Image")));
            this.tbImport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbImport.Name = "tbImport";
            this.tbImport.Size = new System.Drawing.Size(131, 22);
            this.tbImport.Text = "Пополнение списков";
            this.tbImport.ToolTipText = "Пополнение списков граждан, подлежащих информированию";
            this.tbImport.Click += new System.EventHandler(this.tbImport_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(180, 22);
            this.toolStripButton2.Text = "Обновить адреса и телефоны";
            this.toolStripButton2.ToolTipText = "Обновить адреса и телефоны на основании сведений о прикрепленном населении";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // btActive
            // 
            this.btActive.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbExportMonit,
            this.обработатьПротоколToolStripMenuItem,
            this.обработкаРезультатовИнформированияИзВнешнегоИсточникаToolStripMenuItem,
            this.ручноИнформированиеToolStripMenuItem,
            this.toolStripSeparator1,
            this.экпортВТФОМСToolStripMenuItem,
            this.непринятыеЗаписиToolStripMenuItem});
            this.btActive.Image = ((System.Drawing.Image)(resources.GetObject("btActive.Image")));
            this.btActive.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btActive.Name = "btActive";
            this.btActive.Size = new System.Drawing.Size(123, 22);
            this.btActive.Text = "Информирование";
            // 
            // tbExportMonit
            // 
            this.tbExportMonit.Name = "tbExportMonit";
            this.tbExportMonit.Size = new System.Drawing.Size(409, 22);
            this.tbExportMonit.Text = "Сформировать рассылку";
            this.tbExportMonit.Click += new System.EventHandler(this.tbExportMonit_Click);
            // 
            // обработатьПротоколToolStripMenuItem
            // 
            this.обработатьПротоколToolStripMenuItem.Name = "обработатьПротоколToolStripMenuItem";
            this.обработатьПротоколToolStripMenuItem.Size = new System.Drawing.Size(409, 22);
            this.обработатьПротоколToolStripMenuItem.Text = "Обработать протокол рассылки";
            this.обработатьПротоколToolStripMenuItem.Click += new System.EventHandler(this.обработатьПротоколToolStripMenuItem_Click);
            // 
            // обработкаРезультатовИнформированияИзВнешнегоИсточникаToolStripMenuItem
            // 
            this.обработкаРезультатовИнформированияИзВнешнегоИсточникаToolStripMenuItem.Name = "обработкаРезультатовИнформированияИзВнешнегоИсточникаToolStripMenuItem";
            this.обработкаРезультатовИнформированияИзВнешнегоИсточникаToolStripMenuItem.Size = new System.Drawing.Size(409, 22);
            this.обработкаРезультатовИнформированияИзВнешнегоИсточникаToolStripMenuItem.Text = "Обработка результатов информирования из внешнего источника";
            this.обработкаРезультатовИнформированияИзВнешнегоИсточникаToolStripMenuItem.Click += new System.EventHandler(this.обработкаРезультатовИнформированияИзВнешнегоИсточникаToolStripMenuItem_Click);
            // 
            // ручноИнформированиеToolStripMenuItem
            // 
            this.ручноИнформированиеToolStripMenuItem.Name = "ручноИнформированиеToolStripMenuItem";
            this.ручноИнформированиеToolStripMenuItem.Size = new System.Drawing.Size(409, 22);
            this.ручноИнформированиеToolStripMenuItem.Text = "Ручное информирование";
            this.ручноИнформированиеToolStripMenuItem.Click += new System.EventHandler(this.ручноИнформированиеToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(406, 6);
            // 
            // экпортВТФОМСToolStripMenuItem
            // 
            this.экпортВТФОМСToolStripMenuItem.Name = "экпортВТФОМСToolStripMenuItem";
            this.экпортВТФОМСToolStripMenuItem.Size = new System.Drawing.Size(409, 22);
            this.экпортВТФОМСToolStripMenuItem.Text = "Обмен с ТФОМС";
            this.экпортВТФОМСToolStripMenuItem.Click += new System.EventHandler(this.экпортВТФОМСToolStripMenuItem_Click);
            // 
            // непринятыеЗаписиToolStripMenuItem
            // 
            this.непринятыеЗаписиToolStripMenuItem.Name = "непринятыеЗаписиToolStripMenuItem";
            this.непринятыеЗаписиToolStripMenuItem.Size = new System.Drawing.Size(409, 22);
            this.непринятыеЗаписиToolStripMenuItem.Text = "Непринятые записи";
            this.непринятыеЗаписиToolStripMenuItem.Click += new System.EventHandler(this.непринятыеЗаписиToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1028, 389);
            this.panel1.TabIndex = 1;
            this.panel1.Resize += new System.EventHandler(this.panel1_Resize);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.splitter1);
            this.panel3.Controls.Add(this.panel5);
            this.panel3.Controls.Add(this.panel2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1028, 389);
            this.panel3.TabIndex = 2;
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter1.Location = new System.Drawing.Point(0, 218);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(1028, 3);
            this.splitter1.TabIndex = 3;
            this.splitter1.TabStop = false;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this._dgvcontrolM);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1028, 221);
            this.panel5.TabIndex = 2;
            // 
            // _dgvcontrolM
            // 
            this._dgvcontrolM.Dock = System.Windows.Forms.DockStyle.Fill;
            this._dgvcontrolM.Location = new System.Drawing.Point(0, 0);
            this._dgvcontrolM.Name = "_dgvcontrolM";
            this._dgvcontrolM.Size = new System.Drawing.Size(1028, 221);
            this._dgvcontrolM.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this._dgvcontrolD);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 221);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1028, 168);
            this.panel2.TabIndex = 1;
            // 
            // _dgvcontrolD
            // 
            this._dgvcontrolD.Dock = System.Windows.Forms.DockStyle.Fill;
            this._dgvcontrolD.Location = new System.Drawing.Point(0, 0);
            this._dgvcontrolD.Name = "_dgvcontrolD";
            this._dgvcontrolD.Size = new System.Drawing.Size(1028, 168);
            this._dgvcontrolD.TabIndex = 1;
            // 
            // frmWSProf
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1028, 414);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmWSProf";
            this.Text = "Работа с товаром";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.frmWSProf_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ToolStripDropDownButton btActive;
        private System.Windows.Forms.ToolStripMenuItem tbExportMonit;
        private System.Windows.Forms.ToolStripButton tbImport;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripMenuItem экпортВТФОМСToolStripMenuItem;
        private System.Windows.Forms.Panel panel2;
        private DataGridViewGGControl.DataGridViewGGControl _dgvcontrolD;
        private System.Windows.Forms.Panel panel5;
        private DataGridViewGGControl.DataGridViewGGControl _dgvcontrolM;
        private System.Windows.Forms.ToolStripMenuItem обработатьПротоколToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem ручноИнформированиеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem непринятыеЗаписиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem обработкаРезультатовИнформированияИзВнешнегоИсточникаToolStripMenuItem;
        private System.Windows.Forms.Splitter splitter1;
    }
}

