namespace SimMoveT
{
    partial class frmTovar
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTovar));
            this.panel1 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpFiles = new System.Windows.Forms.TabPage();
            this.lbProtocol = new System.Windows.Forms.ListBox();
            this.btFiles = new System.Windows.Forms.Button();
            this.tbFiles = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tpNumbers = new System.Windows.Forms.TabPage();
            this.tbLast = new System.Windows.Forms.TextBox();
            this.tbFirst = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tpBetween = new System.Windows.Forms.TabPage();
            this.tbLastICC_kol = new System.Windows.Forms.TextBox();
            this.tbFirstICC_kol = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tpTovarChecked = new System.Windows.Forms.TabPage();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.btDel = new System.Windows.Forms.Button();
            this.btAdd = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this._dgSelected = new DataGridViewGGControl.DataGridViewGGControl();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this._dgCheck = new DataGridViewGGControl.DataGridViewGGControl();
            this.pnInspector = new System.Windows.Forms.Panel();
            this.propertyGridEx1 = new PropertyGridEx.PropertyGridEx();
            this.btRefresh = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tpFiles.SuspendLayout();
            this.tpNumbers.SuspendLayout();
            this.tpBetween.SuspendLayout();
            this.tpTovarChecked.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.pnInspector.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 379);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(658, 49);
            this.panel1.TabIndex = 1;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(571, 16);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Отмена";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(496, 16);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(69, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "ОК";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tabControl1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(658, 379);
            this.panel2.TabIndex = 2;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpFiles);
            this.tabControl1.Controls.Add(this.tpNumbers);
            this.tabControl1.Controls.Add(this.tpBetween);
            this.tabControl1.Controls.Add(this.tpTovarChecked);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(658, 379);
            this.tabControl1.TabIndex = 1;
            this.tabControl1.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControl1_Selected);
            // 
            // tpFiles
            // 
            this.tpFiles.Controls.Add(this.lbProtocol);
            this.tpFiles.Controls.Add(this.btFiles);
            this.tpFiles.Controls.Add(this.tbFiles);
            this.tpFiles.Controls.Add(this.label1);
            this.tpFiles.Location = new System.Drawing.Point(4, 22);
            this.tpFiles.Name = "tpFiles";
            this.tpFiles.Padding = new System.Windows.Forms.Padding(3);
            this.tpFiles.Size = new System.Drawing.Size(650, 353);
            this.tpFiles.TabIndex = 0;
            this.tpFiles.Text = "Товар из файла";
            this.tpFiles.UseVisualStyleBackColor = true;
            // 
            // lbProtocol
            // 
            this.lbProtocol.FormattingEnabled = true;
            this.lbProtocol.Location = new System.Drawing.Point(8, 32);
            this.lbProtocol.Name = "lbProtocol";
            this.lbProtocol.Size = new System.Drawing.Size(634, 316);
            this.lbProtocol.TabIndex = 3;
            // 
            // btFiles
            // 
            this.btFiles.Location = new System.Drawing.Point(616, 4);
            this.btFiles.Name = "btFiles";
            this.btFiles.Size = new System.Drawing.Size(26, 23);
            this.btFiles.TabIndex = 2;
            this.btFiles.Text = "...";
            this.btFiles.UseVisualStyleBackColor = true;
            this.btFiles.Click += new System.EventHandler(this.btFiles_Click);
            // 
            // tbFiles
            // 
            this.tbFiles.Location = new System.Drawing.Point(53, 6);
            this.tbFiles.Name = "tbFiles";
            this.tbFiles.Size = new System.Drawing.Size(557, 20);
            this.tbFiles.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Файл:";
            // 
            // tpNumbers
            // 
            this.tpNumbers.Controls.Add(this.tbLast);
            this.tpNumbers.Controls.Add(this.tbFirst);
            this.tpNumbers.Controls.Add(this.label3);
            this.tpNumbers.Controls.Add(this.label2);
            this.tpNumbers.Location = new System.Drawing.Point(4, 22);
            this.tpNumbers.Name = "tpNumbers";
            this.tpNumbers.Padding = new System.Windows.Forms.Padding(3);
            this.tpNumbers.Size = new System.Drawing.Size(650, 353);
            this.tpNumbers.TabIndex = 1;
            this.tpNumbers.Text = "Товар по кол-ву";
            this.tpNumbers.UseVisualStyleBackColor = true;
            // 
            // tbLast
            // 
            this.tbLast.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbLast.Location = new System.Drawing.Point(293, 165);
            this.tbLast.MaxLength = 20;
            this.tbLast.Name = "tbLast";
            this.tbLast.ReadOnly = true;
            this.tbLast.Size = new System.Drawing.Size(214, 20);
            this.tbLast.TabIndex = 3;
            // 
            // tbFirst
            // 
            this.tbFirst.Location = new System.Drawing.Point(293, 138);
            this.tbFirst.MaxLength = 20;
            this.tbFirst.Name = "tbFirst";
            this.tbFirst.ReadOnly = true;
            this.tbFirst.Size = new System.Drawing.Size(214, 20);
            this.tbFirst.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(123, 172);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(121, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Последний номер ICC:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(123, 145);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Первый номер ICC:";
            // 
            // tpBetween
            // 
            this.tpBetween.Controls.Add(this.tbLastICC_kol);
            this.tpBetween.Controls.Add(this.tbFirstICC_kol);
            this.tpBetween.Controls.Add(this.label4);
            this.tpBetween.Controls.Add(this.label5);
            this.tpBetween.Location = new System.Drawing.Point(4, 22);
            this.tpBetween.Name = "tpBetween";
            this.tpBetween.Size = new System.Drawing.Size(650, 353);
            this.tpBetween.TabIndex = 2;
            this.tpBetween.Text = "Товар по диапазону";
            this.tpBetween.UseVisualStyleBackColor = true;
            // 
            // tbLastICC_kol
            // 
            this.tbLastICC_kol.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbLastICC_kol.Location = new System.Drawing.Point(293, 165);
            this.tbLastICC_kol.MaxLength = 20;
            this.tbLastICC_kol.Name = "tbLastICC_kol";
            this.tbLastICC_kol.Size = new System.Drawing.Size(214, 20);
            this.tbLastICC_kol.TabIndex = 7;
            // 
            // tbFirstICC_kol
            // 
            this.tbFirstICC_kol.Location = new System.Drawing.Point(293, 138);
            this.tbFirstICC_kol.MaxLength = 20;
            this.tbFirstICC_kol.Name = "tbFirstICC_kol";
            this.tbFirstICC_kol.Size = new System.Drawing.Size(214, 20);
            this.tbFirstICC_kol.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(123, 172);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(167, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Укажите последний номер ICC:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(123, 145);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(151, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Укажите первый номер ICC:";
            // 
            // tpTovarChecked
            // 
            this.tpTovarChecked.Controls.Add(this.panel4);
            this.tpTovarChecked.Controls.Add(this.pnInspector);
            this.tpTovarChecked.Location = new System.Drawing.Point(4, 22);
            this.tpTovarChecked.Name = "tpTovarChecked";
            this.tpTovarChecked.Size = new System.Drawing.Size(650, 353);
            this.tpTovarChecked.TabIndex = 3;
            this.tpTovarChecked.Text = "Произвольный товар";
            this.tpTovarChecked.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.panel5);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(252, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(398, 353);
            this.panel4.TabIndex = 1;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.panel6);
            this.panel5.Controls.Add(this.groupBox2);
            this.panel5.Controls.Add(this.groupBox1);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(398, 353);
            this.panel5.TabIndex = 1;
            this.panel5.Resize += new System.EventHandler(this.panel5_Resize);
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.btDel);
            this.panel6.Controls.Add(this.btAdd);
            this.panel6.Location = new System.Drawing.Point(21, 167);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(373, 30);
            this.panel6.TabIndex = 3;
            // 
            // btDel
            // 
            this.btDel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btDel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btDel.Location = new System.Drawing.Point(202, 0);
            this.btDel.Name = "btDel";
            this.btDel.Size = new System.Drawing.Size(171, 30);
            this.btDel.TabIndex = 1;
            this.btDel.Text = "-";
            this.btDel.UseVisualStyleBackColor = true;
            this.btDel.Click += new System.EventHandler(this.btDel_Click);
            // 
            // btAdd
            // 
            this.btAdd.Dock = System.Windows.Forms.DockStyle.Left;
            this.btAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btAdd.Location = new System.Drawing.Point(0, 0);
            this.btAdd.Name = "btAdd";
            this.btAdd.Size = new System.Drawing.Size(202, 30);
            this.btAdd.TabIndex = 0;
            this.btAdd.Text = "+";
            this.btAdd.UseVisualStyleBackColor = true;
            this.btAdd.Click += new System.EventHandler(this.btAdd_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this._dgSelected);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(0, 203);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(398, 150);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Выбранный товар";
            // 
            // _dgSelected
            // 
            this._dgSelected.Dock = System.Windows.Forms.DockStyle.Fill;
            this._dgSelected.Location = new System.Drawing.Point(3, 16);
            this._dgSelected.Name = "_dgSelected";
            this._dgSelected.Size = new System.Drawing.Size(392, 131);
            this._dgSelected.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this._dgCheck);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(398, 161);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Найденный товар";
            // 
            // _dgCheck
            // 
            this._dgCheck.Dock = System.Windows.Forms.DockStyle.Fill;
            this._dgCheck.Location = new System.Drawing.Point(3, 16);
            this._dgCheck.Name = "_dgCheck";
            this._dgCheck.Size = new System.Drawing.Size(392, 142);
            this._dgCheck.TabIndex = 0;
            // 
            // pnInspector
            // 
            this.pnInspector.Controls.Add(this.propertyGridEx1);
            this.pnInspector.Controls.Add(this.btRefresh);
            this.pnInspector.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnInspector.Location = new System.Drawing.Point(0, 0);
            this.pnInspector.Name = "pnInspector";
            this.pnInspector.Size = new System.Drawing.Size(252, 353);
            this.pnInspector.TabIndex = 0;
            // 
            // propertyGridEx1
            // 
            // 
            // 
            // 
            this.propertyGridEx1.DocCommentDescription.AccessibleName = "";
            this.propertyGridEx1.DocCommentDescription.AutoEllipsis = true;
            this.propertyGridEx1.DocCommentDescription.Cursor = System.Windows.Forms.Cursors.Default;
            this.propertyGridEx1.DocCommentDescription.Location = new System.Drawing.Point(3, 18);
            this.propertyGridEx1.DocCommentDescription.Name = "";
            this.propertyGridEx1.DocCommentDescription.Size = new System.Drawing.Size(246, 37);
            this.propertyGridEx1.DocCommentDescription.TabIndex = 1;
            this.propertyGridEx1.DocCommentImage = null;
            // 
            // 
            // 
            this.propertyGridEx1.DocCommentTitle.Cursor = System.Windows.Forms.Cursors.Default;
            this.propertyGridEx1.DocCommentTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.propertyGridEx1.DocCommentTitle.Location = new System.Drawing.Point(3, 3);
            this.propertyGridEx1.DocCommentTitle.Name = "";
            this.propertyGridEx1.DocCommentTitle.Size = new System.Drawing.Size(246, 15);
            this.propertyGridEx1.DocCommentTitle.TabIndex = 0;
            this.propertyGridEx1.DocCommentTitle.UseMnemonic = false;
            this.propertyGridEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGridEx1.Location = new System.Drawing.Point(0, 0);
            this.propertyGridEx1.Name = "propertyGridEx1";
            this.propertyGridEx1.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            this.propertyGridEx1.SelectedObject = ((object)(resources.GetObject("propertyGridEx1.SelectedObject")));
            this.propertyGridEx1.Size = new System.Drawing.Size(252, 330);
            this.propertyGridEx1.TabIndex = 1;
            // 
            // 
            // 
            this.propertyGridEx1.ToolStrip.AccessibleName = "Панель инструментов";
            this.propertyGridEx1.ToolStrip.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
            this.propertyGridEx1.ToolStrip.AllowMerge = false;
            this.propertyGridEx1.ToolStrip.AutoSize = false;
            this.propertyGridEx1.ToolStrip.CanOverflow = false;
            this.propertyGridEx1.ToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.propertyGridEx1.ToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.propertyGridEx1.ToolStrip.Location = new System.Drawing.Point(0, 1);
            this.propertyGridEx1.ToolStrip.Name = "";
            this.propertyGridEx1.ToolStrip.Padding = new System.Windows.Forms.Padding(2, 0, 1, 0);
            this.propertyGridEx1.ToolStrip.Size = new System.Drawing.Size(252, 25);
            this.propertyGridEx1.ToolStrip.TabIndex = 1;
            this.propertyGridEx1.ToolStrip.TabStop = true;
            this.propertyGridEx1.ToolStrip.Text = "PropertyGridToolBar";
            // 
            // btRefresh
            // 
            this.btRefresh.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btRefresh.Location = new System.Drawing.Point(0, 330);
            this.btRefresh.Name = "btRefresh";
            this.btRefresh.Size = new System.Drawing.Size(252, 23);
            this.btRefresh.TabIndex = 0;
            this.btRefresh.Text = "Найти";
            this.btRefresh.UseVisualStyleBackColor = true;
            this.btRefresh.Click += new System.EventHandler(this.btRefresh_Click);
            // 
            // frmTovar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(658, 428);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmTovar";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Приход товара";
            this.Shown += new System.EventHandler(this.frmTovar_Shown);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tpFiles.ResumeLayout(false);
            this.tpFiles.PerformLayout();
            this.tpNumbers.ResumeLayout(false);
            this.tpNumbers.PerformLayout();
            this.tpBetween.ResumeLayout(false);
            this.tpBetween.PerformLayout();
            this.tpTovarChecked.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.pnInspector.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpFiles;
        private System.Windows.Forms.ListBox lbProtocol;
        private System.Windows.Forms.Button btFiles;
        private System.Windows.Forms.TextBox tbFiles;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tpNumbers;
        private System.Windows.Forms.TextBox tbLast;
        private System.Windows.Forms.TextBox tbFirst;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabPage tpBetween;
        private System.Windows.Forms.TextBox tbLastICC_kol;
        private System.Windows.Forms.TextBox tbFirstICC_kol;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabPage tpTovarChecked;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel pnInspector;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btDel;
        private System.Windows.Forms.Button btAdd;
        private DataGridViewGGControl.DataGridViewGGControl _dgSelected;
        private DataGridViewGGControl.DataGridViewGGControl _dgCheck;
        private PropertyGridEx.PropertyGridEx propertyGridEx1;
        private System.Windows.Forms.Button btRefresh;
    }
}