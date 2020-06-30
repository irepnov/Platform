namespace WSAttach
{
    partial class frmActs
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmActs));
            this.panel1 = new System.Windows.Forms.Panel();
            this.propertyGridEx1 = new PropertyGridEx.PropertyGridEx();
            this.panel3 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.dgControl = new DataGridViewGGControl.DataGridViewGGControl();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.дляМедОрганизацийToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.дляПКМЭКToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.propertyGridEx1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(260, 622);
            this.panel1.TabIndex = 0;
            // 
            // propertyGridEx1
            // 
            // 
            // 
            // 
            this.propertyGridEx1.DocCommentDescription.AutoEllipsis = true;
            this.propertyGridEx1.DocCommentDescription.Cursor = System.Windows.Forms.Cursors.Default;
            this.propertyGridEx1.DocCommentDescription.Location = new System.Drawing.Point(3, 18);
            this.propertyGridEx1.DocCommentDescription.Name = "";
            this.propertyGridEx1.DocCommentDescription.Size = new System.Drawing.Size(254, 37);
            this.propertyGridEx1.DocCommentDescription.TabIndex = 1;
            this.propertyGridEx1.DocCommentImage = null;
            // 
            // 
            // 
            this.propertyGridEx1.DocCommentTitle.Cursor = System.Windows.Forms.Cursors.Default;
            this.propertyGridEx1.DocCommentTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.propertyGridEx1.DocCommentTitle.Location = new System.Drawing.Point(3, 3);
            this.propertyGridEx1.DocCommentTitle.Name = "";
            this.propertyGridEx1.DocCommentTitle.Size = new System.Drawing.Size(254, 15);
            this.propertyGridEx1.DocCommentTitle.TabIndex = 0;
            this.propertyGridEx1.DocCommentTitle.UseMnemonic = false;
            this.propertyGridEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGridEx1.Location = new System.Drawing.Point(0, 0);
            this.propertyGridEx1.Name = "propertyGridEx1";
            this.propertyGridEx1.Size = new System.Drawing.Size(260, 622);
            this.propertyGridEx1.TabIndex = 9;
            // 
            // 
            // 
            this.propertyGridEx1.ToolStrip.AccessibleName = "ToolBar";
            this.propertyGridEx1.ToolStrip.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
            this.propertyGridEx1.ToolStrip.AllowMerge = false;
            this.propertyGridEx1.ToolStrip.AutoSize = false;
            this.propertyGridEx1.ToolStrip.CanOverflow = false;
            this.propertyGridEx1.ToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.propertyGridEx1.ToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.propertyGridEx1.ToolStrip.Location = new System.Drawing.Point(0, 1);
            this.propertyGridEx1.ToolStrip.Name = "";
            this.propertyGridEx1.ToolStrip.Padding = new System.Windows.Forms.Padding(2, 0, 1, 0);
            this.propertyGridEx1.ToolStrip.Size = new System.Drawing.Size(260, 25);
            this.propertyGridEx1.ToolStrip.TabIndex = 1;
            this.propertyGridEx1.ToolStrip.TabStop = true;
            this.propertyGridEx1.ToolStrip.Text = "PropertyGridToolBar";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.button1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(260, 569);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1037, 53);
            this.panel3.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(950, 18);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Закрыть";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dgControl
            // 
            this.dgControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgControl.Location = new System.Drawing.Point(260, 0);
            this.dgControl.Name = "dgControl";
            this.dgControl.Size = new System.Drawing.Size(1037, 569);
            this.dgControl.TabIndex = 6;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripDropDownButton1});
            this.toolStrip1.Location = new System.Drawing.Point(260, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1037, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(77, 22);
            this.toolStripButton1.Text = "Обновить";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(114, 22);
            this.toolStripButton2.Text = "Рассчитать акты";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.дляМедОрганизацийToolStripMenuItem,
            this.дляПКМЭКToolStripMenuItem});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(119, 22);
            this.toolStripDropDownButton1.Text = "Выгрузить акты";
            // 
            // дляМедОрганизацийToolStripMenuItem
            // 
            this.дляМедОрганизацийToolStripMenuItem.Name = "дляМедОрганизацийToolStripMenuItem";
            this.дляМедОрганизацийToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.дляМедОрганизацийToolStripMenuItem.Text = "для мед. организаций";
            this.дляМедОрганизацийToolStripMenuItem.Click += new System.EventHandler(this.дляМедОрганизацийToolStripMenuItem_Click);
            // 
            // дляПКМЭКToolStripMenuItem
            // 
            this.дляПКМЭКToolStripMenuItem.Name = "дляПКМЭКToolStripMenuItem";
            this.дляПКМЭКToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.дляПКМЭКToolStripMenuItem.Text = "для ПК МЭК";
            this.дляПКМЭКToolStripMenuItem.Click += new System.EventHandler(this.дляПКМЭКToolStripMenuItem_Click);
            // 
            // frmActs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1297, 622);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.dgControl);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Name = "frmActs";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Акты сверки численности прикрепленного населения";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmInfosError_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button button1;
        private DataGridViewGGControl.DataGridViewGGControl dgControl;
        private PropertyGridEx.PropertyGridEx propertyGridEx1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem дляМедОрганизацийToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem дляПКМЭКToolStripMenuItem;
    }
}