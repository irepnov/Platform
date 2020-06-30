namespace prip_pl
{
    partial class formPrip
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(formPrip));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsb_ExecuteMIAC = new System.Windows.Forms.ToolStripButton();
            this.tsddb_Reports = new System.Windows.Forms.ToolStripDropDownButton();
            this.tb_reportMIAC = new System.Windows.Forms.ToolStripMenuItem();
            this.результатыСверкиССРЗToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbMRF_SRZ = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbSRZ_mun = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbNoprik = new System.Windows.Forms.ToolStripMenuItem();
            this.tsb_DoctorPower = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbUchReport = new System.Windows.Forms.ToolStripMenuItem();
            this.tsb_polAge = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbError = new System.Windows.Forms.ToolStripMenuItem();
            this.tsb_prikLPU_SMO = new System.Windows.Forms.ToolStripMenuItem();
            this.tsb_notPrik = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbdd_CSV = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsb_PrikToMO = new System.Windows.Forms.ToolStripMenuItem();
            this.tsb_PrikToMEDR = new System.Windows.Forms.ToolStripMenuItem();
            this.tsb_prikSMO = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this._dgvcontrol = new DataGridViewGGControl.DataGridViewGGControl();
            this.toolStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsb_ExecuteMIAC,
            this.tsddb_Reports,
            this.tsb_notPrik,
            this.toolStripSeparator1,
            this.tsbdd_CSV,
            this.tsb_prikSMO,
            this.toolStripButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1312, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.Visible = false;
            // 
            // tsb_ExecuteMIAC
            // 
            this.tsb_ExecuteMIAC.Image = ((System.Drawing.Image)(resources.GetObject("tsb_ExecuteMIAC.Image")));
            this.tsb_ExecuteMIAC.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_ExecuteMIAC.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.tsb_ExecuteMIAC.MergeIndex = 16;
            this.tsb_ExecuteMIAC.Name = "tsb_ExecuteMIAC";
            this.tsb_ExecuteMIAC.Size = new System.Drawing.Size(127, 22);
            this.tsb_ExecuteMIAC.Text = "Обработка МИАЦ";
            this.tsb_ExecuteMIAC.ToolTipText = "Импорт и сверка файла PRIK из МИАЦ";
            this.tsb_ExecuteMIAC.Click += new System.EventHandler(this.tsb_Import_Click);
            // 
            // tsddb_Reports
            // 
            this.tsddb_Reports.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tb_reportMIAC,
            this.результатыСверкиССРЗToolStripMenuItem,
            this.tsbMRF_SRZ,
            this.tsbSRZ_mun,
            this.tsbNoprik,
            this.tsb_DoctorPower,
            this.tsbUchReport,
            this.tsb_polAge,
            this.tsbError,
            this.tsb_prikLPU_SMO});
            this.tsddb_Reports.Image = ((System.Drawing.Image)(resources.GetObject("tsddb_Reports.Image")));
            this.tsddb_Reports.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsddb_Reports.Name = "tsddb_Reports";
            this.tsddb_Reports.Size = new System.Drawing.Size(77, 22);
            this.tsddb_Reports.Text = "Отчеты";
            // 
            // tb_reportMIAC
            // 
            this.tb_reportMIAC.Name = "tb_reportMIAC";
            this.tb_reportMIAC.Size = new System.Drawing.Size(390, 22);
            this.tb_reportMIAC.Text = "Анализ данных от МО (МИАЦ)";
            this.tb_reportMIAC.Click += new System.EventHandler(this.tb_reportMIAC_Click);
            // 
            // результатыСверкиССРЗToolStripMenuItem
            // 
            this.результатыСверкиССРЗToolStripMenuItem.Name = "результатыСверкиССРЗToolStripMenuItem";
            this.результатыСверкиССРЗToolStripMenuItem.Size = new System.Drawing.Size(390, 22);
            this.результатыСверкиССРЗToolStripMenuItem.Text = "Результаты сверки с СРЗ";
            this.результатыСверкиССРЗToolStripMenuItem.Click += new System.EventHandler(this.результатыСверкиССРЗToolStripMenuItem_Click);
            // 
            // tsbMRF_SRZ
            // 
            this.tsbMRF_SRZ.Name = "tsbMRF_SRZ";
            this.tsbMRF_SRZ.Size = new System.Drawing.Size(390, 22);
            this.tsbMRF_SRZ.Text = "Результаты сверки с СРЗ по МРФ";
            this.tsbMRF_SRZ.Click += new System.EventHandler(this.tsbMRF_SRZ_Click);
            // 
            // tsbSRZ_mun
            // 
            this.tsbSRZ_mun.Name = "tsbSRZ_mun";
            this.tsbSRZ_mun.Size = new System.Drawing.Size(390, 22);
            this.tsbSRZ_mun.Text = "Результаты сверки с СРЗ по мун. образованиям";
            this.tsbSRZ_mun.Click += new System.EventHandler(this.tsbSRZ_mun_Click);
            // 
            // tsbNoprik
            // 
            this.tsbNoprik.Name = "tsbNoprik";
            this.tsbNoprik.Size = new System.Drawing.Size(390, 22);
            this.tsbNoprik.Text = "Неприкрепленные граждане в разрезе мун. образований";
            this.tsbNoprik.Click += new System.EventHandler(this.tsbNoprik_Click);
            // 
            // tsb_DoctorPower
            // 
            this.tsb_DoctorPower.Name = "tsb_DoctorPower";
            this.tsb_DoctorPower.Size = new System.Drawing.Size(390, 22);
            this.tsb_DoctorPower.Text = "Распределение нагрузки по врачам";
            this.tsb_DoctorPower.Click += new System.EventHandler(this.tsb_DoctorPower_Click);
            // 
            // tsbUchReport
            // 
            this.tsbUchReport.Name = "tsbUchReport";
            this.tsbUchReport.Size = new System.Drawing.Size(390, 22);
            this.tsbUchReport.Text = "Распределение нагрузки по участкам";
            this.tsbUchReport.Click += new System.EventHandler(this.tsbUchReport_Click);
            // 
            // tsb_polAge
            // 
            this.tsb_polAge.Name = "tsb_polAge";
            this.tsb_polAge.Size = new System.Drawing.Size(390, 22);
            this.tsb_polAge.Text = "Половозрастной состав по мун. образованиям";
            this.tsb_polAge.Click += new System.EventHandler(this.tsb_polAge_Click);
            // 
            // tsbError
            // 
            this.tsbError.Name = "tsbError";
            this.tsbError.Size = new System.Drawing.Size(390, 22);
            this.tsbError.Text = "Анализ причин отклонения записей";
            this.tsbError.Click += new System.EventHandler(this.tsbError_Click);
            // 
            // tsb_prikLPU_SMO
            // 
            this.tsb_prikLPU_SMO.Name = "tsb_prikLPU_SMO";
            this.tsb_prikLPU_SMO.Size = new System.Drawing.Size(390, 22);
            this.tsb_prikLPU_SMO.Text = "Прикрепленное население в разрезе ЛПУ и СМО";
            this.tsb_prikLPU_SMO.Click += new System.EventHandler(this.прикрепленноеНаселениеВРазрезеЛПУИСМОToolStripMenuItem_Click);
            // 
            // tsb_notPrik
            // 
            this.tsb_notPrik.Image = ((System.Drawing.Image)(resources.GetObject("tsb_notPrik.Image")));
            this.tsb_notPrik.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_notPrik.Name = "tsb_notPrik";
            this.tsb_notPrik.Size = new System.Drawing.Size(131, 22);
            this.tsb_notPrik.Text = "Неприкрепленные";
            this.tsb_notPrik.ToolTipText = "Выгрузить список неприкрепленных";
            this.tsb_notPrik.Click += new System.EventHandler(this.tsb_notPrik_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbdd_CSV
            // 
            this.tsbdd_CSV.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsb_PrikToMO,
            this.tsb_PrikToMEDR});
            this.tsbdd_CSV.Image = ((System.Drawing.Image)(resources.GetObject("tsbdd_CSV.Image")));
            this.tsbdd_CSV.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbdd_CSV.Name = "tsbdd_CSV";
            this.tsbdd_CSV.Size = new System.Drawing.Size(145, 22);
            this.tsbdd_CSV.Text = "Файлы CSV для СРЗ";
            // 
            // tsb_PrikToMO
            // 
            this.tsb_PrikToMO.Name = "tsb_PrikToMO";
            this.tsb_PrikToMO.Size = new System.Drawing.Size(253, 22);
            this.tsb_PrikToMO.Text = "Прикрепление к МО";
            this.tsb_PrikToMO.Click += new System.EventHandler(this.tsb_PrikToMO_Click);
            // 
            // tsb_PrikToMEDR
            // 
            this.tsb_PrikToMEDR.Name = "tsb_PrikToMEDR";
            this.tsb_PrikToMEDR.Size = new System.Drawing.Size(253, 22);
            this.tsb_PrikToMEDR.Text = "Прикрепление к мед. работнику";
            this.tsb_PrikToMEDR.Click += new System.EventHandler(this.tsb_PrikToMEDR_Click);
            // 
            // tsb_prikSMO
            // 
            this.tsb_prikSMO.Image = ((System.Drawing.Image)(resources.GetObject("tsb_prikSMO.Image")));
            this.tsb_prikSMO.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_prikSMO.Name = "tsb_prikSMO";
            this.tsb_prikSMO.Size = new System.Drawing.Size(171, 22);
            this.tsb_prikSMO.Text = "Прикрепленные для СМО";
            this.tsb_prikSMO.Click += new System.EventHandler(this.tsb_prikSMO_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(165, 22);
            this.toolStripButton1.Text = "Регистр мед. работников";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this._dgvcontrol);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 25);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1312, 287);
            this.panel2.TabIndex = 5;
            // 
            // _dgvcontrol
            // 
            this._dgvcontrol.Dock = System.Windows.Forms.DockStyle.Fill;
            this._dgvcontrol.Location = new System.Drawing.Point(0, 0);
            this._dgvcontrol.Name = "_dgvcontrol";
            this._dgvcontrol.Size = new System.Drawing.Size(1312, 287);
            this._dgvcontrol.TabIndex = 0;
            // 
            // formPrip
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1312, 312);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.toolStrip1);
            this.Name = "formPrip";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsb_ExecuteMIAC;
        private System.Windows.Forms.Panel panel2;
        private DataGridViewGGControl.DataGridViewGGControl _dgvcontrol;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripDropDownButton tsddb_Reports;
        private System.Windows.Forms.ToolStripMenuItem tb_reportMIAC;
        private System.Windows.Forms.ToolStripMenuItem результатыСверкиССРЗToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton tsb_notPrik;
        private System.Windows.Forms.ToolStripDropDownButton tsbdd_CSV;
        private System.Windows.Forms.ToolStripMenuItem tsb_PrikToMO;
        private System.Windows.Forms.ToolStripMenuItem tsb_PrikToMEDR;
        private System.Windows.Forms.ToolStripMenuItem tsb_DoctorPower;
        private System.Windows.Forms.ToolStripMenuItem tsbUchReport;
        private System.Windows.Forms.ToolStripMenuItem tsbSRZ_mun;
        private System.Windows.Forms.ToolStripMenuItem tsbNoprik;
        private System.Windows.Forms.ToolStripMenuItem tsb_polAge;
        private System.Windows.Forms.ToolStripMenuItem tsbError;
        private System.Windows.Forms.ToolStripMenuItem tsbMRF_SRZ;
        private System.Windows.Forms.ToolStripButton tsb_prikSMO;
        private System.Windows.Forms.ToolStripMenuItem tsb_prikLPU_SMO;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
    }
}

