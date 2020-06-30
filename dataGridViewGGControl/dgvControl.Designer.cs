namespace DataGridViewGGControl
{
    partial class DataGridViewGGControl
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте 
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataGridViewGGControl));
            this.panel3 = new System.Windows.Forms.Panel();
            this.dataGridViewGG1 = new DataGridViewGG.DataGridViewGG();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btColorInfo = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btExcel = new System.Windows.Forms.ToolStripButton();
            this.btWord = new System.Windows.Forms.ToolStripButton();
            this.btHTML = new System.Windows.Forms.ToolStripButton();
            this.btXML = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tbWrap = new System.Windows.Forms.ToolStripButton();
            this.tbFreeze = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.cbFields = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.tbText = new System.Windows.Forms.ToolStripTextBox();
            this.imageList1 = new System.Windows.Forms.ImageList();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tslRowsCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGG1)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.AutoSize = true;
            this.panel3.Controls.Add(this.dataGridViewGG1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 25);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(643, 153);
            this.panel3.TabIndex = 4;
            // 
            // dataGridViewGG1
            // 
            this.dataGridViewGG1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewGG1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewGG1.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewGG1.Name = "dataGridViewGG1";
            this.dataGridViewGG1.Size = new System.Drawing.Size(643, 153);
            this.dataGridViewGG1.TabIndex = 2;
            this.dataGridViewGG1.DataSourceChanged += new System.EventHandler(this.dataGridViewGG1_DataSourceChanged);
            this.dataGridViewGG1.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dataGridViewGG1_CellPainting);
            this.dataGridViewGG1.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridViewGG1_DataBindingComplete);
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btColorInfo,
            this.toolStripSeparator3,
            this.btExcel,
            this.btWord,
            this.btHTML,
            this.btXML,
            this.toolStripSeparator2,
            this.tbWrap,
            this.tbFreeze,
            this.toolStripSeparator4,
            this.toolStripLabel1,
            this.cbFields,
            this.toolStripLabel2,
            this.tbText});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(643, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.Resize += new System.EventHandler(this.toolStrip1_Resize);
            // 
            // btColorInfo
            // 
            this.btColorInfo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btColorInfo.Enabled = false;
            this.btColorInfo.Image = ((System.Drawing.Image)(resources.GetObject("btColorInfo.Image")));
            this.btColorInfo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btColorInfo.Name = "btColorInfo";
            this.btColorInfo.Size = new System.Drawing.Size(23, 22);
            this.btColorInfo.Text = "toolStripButton1";
            this.btColorInfo.ToolTipText = "Цветовая подсказка";
            this.btColorInfo.Click += new System.EventHandler(this.btColorInfo_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // btExcel
            // 
            this.btExcel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btExcel.Image = ((System.Drawing.Image)(resources.GetObject("btExcel.Image")));
            this.btExcel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btExcel.Name = "btExcel";
            this.btExcel.Size = new System.Drawing.Size(23, 22);
            this.btExcel.ToolTipText = "Выгрузить сведения в Microsoft Excel";
            this.btExcel.Click += new System.EventHandler(this.btExcel_Click);
            // 
            // btWord
            // 
            this.btWord.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btWord.Image = ((System.Drawing.Image)(resources.GetObject("btWord.Image")));
            this.btWord.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btWord.Name = "btWord";
            this.btWord.Size = new System.Drawing.Size(23, 22);
            this.btWord.Text = "toolStripButton1";
            this.btWord.ToolTipText = "Выгрузить сведения в Microsoft Word";
            this.btWord.Click += new System.EventHandler(this.btWord_Click);
            // 
            // btHTML
            // 
            this.btHTML.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btHTML.Image = ((System.Drawing.Image)(resources.GetObject("btHTML.Image")));
            this.btHTML.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btHTML.Name = "btHTML";
            this.btHTML.Size = new System.Drawing.Size(23, 22);
            this.btHTML.Text = "toolStripButton1";
            this.btHTML.ToolTipText = "Выгрузить данные в HTML";
            this.btHTML.Click += new System.EventHandler(this.btHTML_Click);
            // 
            // btXML
            // 
            this.btXML.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btXML.Image = ((System.Drawing.Image)(resources.GetObject("btXML.Image")));
            this.btXML.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btXML.Name = "btXML";
            this.btXML.Size = new System.Drawing.Size(23, 22);
            this.btXML.Text = "toolStripButton1";
            this.btXML.ToolTipText = "Выгрузить данные в XML";
            this.btXML.Click += new System.EventHandler(this.btXML_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tbWrap
            // 
            this.tbWrap.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbWrap.Image = ((System.Drawing.Image)(resources.GetObject("tbWrap.Image")));
            this.tbWrap.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbWrap.Name = "tbWrap";
            this.tbWrap.Size = new System.Drawing.Size(23, 22);
            this.tbWrap.Text = "Переносить строки";
            this.tbWrap.Click += new System.EventHandler(this.tbWrap_Click);
            // 
            // tbFreeze
            // 
            this.tbFreeze.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbFreeze.Image = ((System.Drawing.Image)(resources.GetObject("tbFreeze.Image")));
            this.tbFreeze.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbFreeze.Name = "tbFreeze";
            this.tbFreeze.Size = new System.Drawing.Size(23, 22);
            this.tbFreeze.Text = "Закрепить";
            this.tbFreeze.ToolTipText = "Закрепить область";
            this.tbFreeze.Click += new System.EventHandler(this.tbFreeze_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(73, 22);
            this.toolStripLabel1.Text = "Ключ поиска";
            // 
            // cbFields
            // 
            this.cbFields.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFields.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.cbFields.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.cbFields.Name = "cbFields";
            this.cbFields.Size = new System.Drawing.Size(90, 25);
            this.cbFields.ToolTipText = "Выберите критерий поиска";
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(54, 22);
            this.toolStripLabel2.Text = "значение";
            // 
            // tbText
            // 
            this.tbText.BackColor = System.Drawing.SystemColors.Window;
            this.tbText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbText.Name = "tbText";
            this.tbText.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.tbText.Size = new System.Drawing.Size(40, 25);
            this.tbText.ToolTipText = "Искомое значение";
            this.tbText.TextChanged += new System.EventHandler(this.tbText_TextChanged);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "00800.ico");
            this.imageList1.Images.SetKeyName(1, "01657.ico");
            this.imageList1.Images.SetKeyName(2, "tbWrap.Image.png");
            this.imageList1.Images.SetKeyName(3, "tbWrapNot.Image.png");
            // 
            // panel2
            // 
            this.panel2.AutoSize = true;
            this.panel2.Controls.Add(this.toolStrip1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(643, 25);
            this.panel2.TabIndex = 5;
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.Controls.Add(this.statusStrip1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 178);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(643, 22);
            this.panel1.TabIndex = 6;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslRowsCount});
            this.statusStrip1.Location = new System.Drawing.Point(0, 0);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(643, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            this.statusStrip1.DoubleClick += new System.EventHandler(this.statusStrip1_DoubleClick);
            // 
            // tslRowsCount
            // 
            this.tslRowsCount.Name = "tslRowsCount";
            this.tslRowsCount.Size = new System.Drawing.Size(0, 17);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.panel3);
            this.panel4.Controls.Add(this.panel2);
            this.panel4.Controls.Add(this.panel1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(643, 200);
            this.panel4.TabIndex = 7;
            // 
            // DataGridViewGGControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel4);
            this.Name = "DataGridViewGGControl";
            this.Size = new System.Drawing.Size(643, 200);
            this.Load += new System.EventHandler(this.DataGridViewGGControl_Load);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGG1)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel3;
        private DataGridViewGG.DataGridViewGG dataGridViewGG1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btExcel;
        private System.Windows.Forms.ToolStripButton btWord;
        private System.Windows.Forms.ToolStripButton btHTML;
        private System.Windows.Forms.ToolStripButton btXML;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tbWrap;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox cbFields;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripTextBox tbText;
        private System.Windows.Forms.ToolStripButton btColorInfo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton tbFreeze;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.ToolStripStatusLabel tslRowsCount;
    }
}
