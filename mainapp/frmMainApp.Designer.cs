namespace GGPlatform.MainApp
{
    partial class frmMainApp
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMainApp));
            this.MainAppToolMenu = new System.Windows.Forms.MenuStrip();
            this.toolMenuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.toolMenuItemConnect = new System.Windows.Forms.ToolStripMenuItem();
            this.toolMenuItemDisconnectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolMenuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolMenuWorkplace = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolMenuWindow = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolWindowItemCascade = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolWindowItemArrange = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolWindowItemArrangeHor = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolWindowItemArrangeVer = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolWindowItemArrangeIcons = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolMenuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolMenuItemHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolMenuItemAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.MainAppToolStrip = new System.Windows.Forms.ToolStrip();
            this.toolConnectDB = new System.Windows.Forms.ToolStripButton();
            this.toolDisconnectDB = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolPlugOption = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolPlugListShow = new System.Windows.Forms.ToolStripSplitButton();
            this.toolPlugListAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.toolPlugListDel = new System.Windows.Forms.ToolStripMenuItem();
            this.toolPlugRefresh = new System.Windows.Forms.ToolStripSplitButton();
            this.toolPlugRefreshFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.actionList = new Crad.Windows.Forms.Actions.ActionList();
            this.actionConnect = new Crad.Windows.Forms.Actions.Action();
            this.actionDisconnect = new Crad.Windows.Forms.Actions.Action();
            this.actionExit = new Crad.Windows.Forms.Actions.Action();
            this.actionPlugOption = new Crad.Windows.Forms.Actions.Action();
            this.actionPlugListAdd = new Crad.Windows.Forms.Actions.Action();
            this.actionPlugListDel = new Crad.Windows.Forms.Actions.Action();
            this.actionPlugRefreshFilter = new Crad.Windows.Forms.Actions.Action();
            this.actionPlugRefresh = new Crad.Windows.Forms.Actions.Action();
            this.actionPlugListShow = new Crad.Windows.Forms.Actions.Action();
            this.statusStripContainer = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusDB = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusProperty = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusRows = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripPlugin = new System.Windows.Forms.ToolStrip();
            this.exportReestrToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importReestrToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.MainAppToolMenu.SuspendLayout();
            this.MainAppToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.actionList)).BeginInit();
            this.statusStripContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainAppToolMenu
            // 
            this.MainAppToolMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolMenuFile,
            this.toolMenuWorkplace,
            this.ToolMenuWindow,
            this.ToolMenuHelp});
            this.MainAppToolMenu.Location = new System.Drawing.Point(0, 0);
            this.MainAppToolMenu.MdiWindowListItem = this.ToolMenuWindow;
            this.MainAppToolMenu.Name = "MainAppToolMenu";
            this.MainAppToolMenu.Size = new System.Drawing.Size(1235, 24);
            this.MainAppToolMenu.TabIndex = 0;
            this.MainAppToolMenu.Text = "Действия";
            // 
            // toolMenuFile
            // 
            this.toolMenuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolMenuItemConnect,
            this.toolMenuItemDisconnectToolStripMenuItem,
            this.toolStripSeparator5,
            this.exportReestrToolStripMenuItem,
            this.importReestrToolStripMenuItem,
            this.toolStripSeparator2,
            this.toolMenuItemExit});
            this.toolMenuFile.MergeIndex = 0;
            this.toolMenuFile.Name = "toolMenuFile";
            this.toolMenuFile.Size = new System.Drawing.Size(48, 20);
            this.toolMenuFile.Text = "&Файл";
            // 
            // toolMenuItemConnect
            // 
            this.actionList.SetAction(this.toolMenuItemConnect, this.actionConnect);
            this.toolMenuItemConnect.Image = ((System.Drawing.Image)(resources.GetObject("toolMenuItemConnect.Image")));
            this.toolMenuItemConnect.Name = "toolMenuItemConnect";
            this.toolMenuItemConnect.ShortcutKeyDisplayString = "F9";
            this.toolMenuItemConnect.Size = new System.Drawing.Size(241, 22);
            this.toolMenuItemConnect.Text = "&Подключиться к БД";
            // 
            // toolMenuItemDisconnectToolStripMenuItem
            // 
            this.actionList.SetAction(this.toolMenuItemDisconnectToolStripMenuItem, this.actionDisconnect);
            this.toolMenuItemDisconnectToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("toolMenuItemDisconnectToolStripMenuItem.Image")));
            this.toolMenuItemDisconnectToolStripMenuItem.Name = "toolMenuItemDisconnectToolStripMenuItem";
            this.toolMenuItemDisconnectToolStripMenuItem.ShortcutKeyDisplayString = "F10";
            this.toolMenuItemDisconnectToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
            this.toolMenuItemDisconnectToolStripMenuItem.Text = "&Отключиться от БД";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(238, 6);
            // 
            // toolMenuItemExit
            // 
            this.actionList.SetAction(this.toolMenuItemExit, this.actionExit);
            this.toolMenuItemExit.Name = "toolMenuItemExit";
            this.toolMenuItemExit.ShortcutKeyDisplayString = "Alt+F4";
            this.toolMenuItemExit.Size = new System.Drawing.Size(241, 22);
            this.toolMenuItemExit.Text = "Выход";
            // 
            // toolMenuWorkplace
            // 
            this.toolMenuWorkplace.Enabled = false;
            this.toolMenuWorkplace.Image = ((System.Drawing.Image)(resources.GetObject("toolMenuWorkplace.Image")));
            this.toolMenuWorkplace.MergeIndex = 1;
            this.toolMenuWorkplace.Name = "toolMenuWorkplace";
            this.toolMenuWorkplace.Size = new System.Drawing.Size(117, 20);
            this.toolMenuWorkplace.Text = "&Рабочие места";
            // 
            // ToolMenuWindow
            // 
            this.ToolMenuWindow.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolWindowItemCascade,
            this.ToolWindowItemArrange,
            this.ToolWindowItemArrangeIcons,
            this.toolStripMenuItem1});
            this.ToolMenuWindow.MergeIndex = 99;
            this.ToolMenuWindow.Name = "ToolMenuWindow";
            this.ToolMenuWindow.Size = new System.Drawing.Size(48, 20);
            this.ToolMenuWindow.Text = "О&кно";
            // 
            // ToolWindowItemCascade
            // 
            this.ToolWindowItemCascade.Image = global::GGPlatform.MainApp.Properties.Resources.application_cascade_5825;
            this.ToolWindowItemCascade.Name = "ToolWindowItemCascade";
            this.ToolWindowItemCascade.Size = new System.Drawing.Size(187, 22);
            this.ToolWindowItemCascade.Text = "&Каскад";
            this.ToolWindowItemCascade.Click += new System.EventHandler(this.ToolMenuItem_Click);
            // 
            // ToolWindowItemArrange
            // 
            this.ToolWindowItemArrange.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolWindowItemArrangeHor,
            this.ToolWindowItemArrangeVer});
            this.ToolWindowItemArrange.Name = "ToolWindowItemArrange";
            this.ToolWindowItemArrange.Size = new System.Drawing.Size(187, 22);
            this.ToolWindowItemArrange.Text = "&Упорядочить";
            // 
            // ToolWindowItemArrangeHor
            // 
            this.ToolWindowItemArrangeHor.Image = global::GGPlatform.MainApp.Properties.Resources.application_tile_horizontal_2349;
            this.ToolWindowItemArrangeHor.Name = "ToolWindowItemArrangeHor";
            this.ToolWindowItemArrangeHor.Size = new System.Drawing.Size(161, 22);
            this.ToolWindowItemArrangeHor.Text = "по горизонтали";
            this.ToolWindowItemArrangeHor.Click += new System.EventHandler(this.ToolMenuItem_Click);
            // 
            // ToolWindowItemArrangeVer
            // 
            this.ToolWindowItemArrangeVer.Image = global::GGPlatform.MainApp.Properties.Resources.application_tile_vertical_8886;
            this.ToolWindowItemArrangeVer.Name = "ToolWindowItemArrangeVer";
            this.ToolWindowItemArrangeVer.Size = new System.Drawing.Size(161, 22);
            this.ToolWindowItemArrangeVer.Text = "по вертикали";
            this.ToolWindowItemArrangeVer.Click += new System.EventHandler(this.ToolMenuItem_Click);
            // 
            // ToolWindowItemArrangeIcons
            // 
            this.ToolWindowItemArrangeIcons.Name = "ToolWindowItemArrangeIcons";
            this.ToolWindowItemArrangeIcons.Size = new System.Drawing.Size(187, 22);
            this.ToolWindowItemArrangeIcons.Text = "Упорядочить &значки";
            this.ToolWindowItemArrangeIcons.Click += new System.EventHandler(this.ToolMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(184, 6);
            // 
            // ToolMenuHelp
            // 
            this.ToolMenuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolMenuItemHelp,
            this.toolStripSeparator3,
            this.ToolMenuItemAbout});
            this.ToolMenuHelp.MergeIndex = 100;
            this.ToolMenuHelp.Name = "ToolMenuHelp";
            this.ToolMenuHelp.Size = new System.Drawing.Size(65, 20);
            this.ToolMenuHelp.Text = "&Справка";
            // 
            // ToolMenuItemHelp
            // 
            this.ToolMenuItemHelp.Name = "ToolMenuItemHelp";
            this.ToolMenuItemHelp.Size = new System.Drawing.Size(149, 22);
            this.ToolMenuItemHelp.Text = "Справка";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(146, 6);
            // 
            // ToolMenuItemAbout
            // 
            this.ToolMenuItemAbout.Name = "ToolMenuItemAbout";
            this.ToolMenuItemAbout.Size = new System.Drawing.Size(149, 22);
            this.ToolMenuItemAbout.Text = "О программе";
            this.ToolMenuItemAbout.Click += new System.EventHandler(this.ToolMenuItem_Click);
            // 
            // MainAppToolStrip
            // 
            this.MainAppToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolConnectDB,
            this.toolDisconnectDB,
            this.toolStripSeparator1,
            this.toolPlugOption,
            this.toolStripSeparator4,
            this.toolPlugListShow,
            this.toolPlugRefresh});
            this.MainAppToolStrip.Location = new System.Drawing.Point(0, 24);
            this.MainAppToolStrip.Name = "MainAppToolStrip";
            this.MainAppToolStrip.Size = new System.Drawing.Size(1235, 25);
            this.MainAppToolStrip.TabIndex = 1;
            this.MainAppToolStrip.Text = "Plugin2";
            // 
            // toolConnectDB
            // 
            this.actionList.SetAction(this.toolConnectDB, this.actionConnect);
            this.toolConnectDB.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolConnectDB.Image = ((System.Drawing.Image)(resources.GetObject("toolConnectDB.Image")));
            this.toolConnectDB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolConnectDB.Name = "toolConnectDB";
            this.toolConnectDB.Size = new System.Drawing.Size(23, 22);
            this.toolConnectDB.Text = "&Подключиться к БД";
            // 
            // toolDisconnectDB
            // 
            this.actionList.SetAction(this.toolDisconnectDB, this.actionDisconnect);
            this.toolDisconnectDB.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolDisconnectDB.Image = ((System.Drawing.Image)(resources.GetObject("toolDisconnectDB.Image")));
            this.toolDisconnectDB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolDisconnectDB.Name = "toolDisconnectDB";
            this.toolDisconnectDB.Size = new System.Drawing.Size(23, 22);
            this.toolDisconnectDB.Text = "&Отключиться от БД";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolPlugOption
            // 
            this.actionList.SetAction(this.toolPlugOption, this.actionPlugOption);
            this.toolPlugOption.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolPlugOption.Enabled = false;
            this.toolPlugOption.Image = ((System.Drawing.Image)(resources.GetObject("toolPlugOption.Image")));
            this.toolPlugOption.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolPlugOption.Name = "toolPlugOption";
            this.toolPlugOption.Size = new System.Drawing.Size(23, 22);
            this.toolPlugOption.Text = "Настройки рабочего места";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // toolPlugListShow
            // 
            this.toolPlugListShow.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolPlugListAdd,
            this.toolPlugListDel});
            this.toolPlugListShow.Enabled = false;
            this.toolPlugListShow.Image = ((System.Drawing.Image)(resources.GetObject("toolPlugListShow.Image")));
            this.toolPlugListShow.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolPlugListShow.Name = "toolPlugListShow";
            this.toolPlugListShow.Size = new System.Drawing.Size(80, 22);
            this.toolPlugListShow.Text = "Списки";
            this.toolPlugListShow.ToolTipText = "Выбрать текущие списки";
            this.toolPlugListShow.ButtonClick += new System.EventHandler(this.actionPlugButton_Execute);
            // 
            // toolPlugListAdd
            // 
            this.actionList.SetAction(this.toolPlugListAdd, this.actionPlugListAdd);
            this.toolPlugListAdd.Enabled = false;
            this.toolPlugListAdd.Image = ((System.Drawing.Image)(resources.GetObject("toolPlugListAdd.Image")));
            this.toolPlugListAdd.Name = "toolPlugListAdd";
            this.toolPlugListAdd.Size = new System.Drawing.Size(137, 22);
            this.toolPlugListAdd.Text = "&Добавить";
            this.toolPlugListAdd.ToolTipText = "Добавить (обновить) текущий список";
            this.toolPlugListAdd.Click += new System.EventHandler(this.actionPlugButton_Execute);
            // 
            // toolPlugListDel
            // 
            this.actionList.SetAction(this.toolPlugListDel, this.actionPlugListDel);
            this.toolPlugListDel.Enabled = false;
            this.toolPlugListDel.Image = ((System.Drawing.Image)(resources.GetObject("toolPlugListDel.Image")));
            this.toolPlugListDel.Name = "toolPlugListDel";
            this.toolPlugListDel.Size = new System.Drawing.Size(137, 22);
            this.toolPlugListDel.Text = "&Исключить";
            this.toolPlugListDel.ToolTipText = "Исключить текущий список из системы";
            this.toolPlugListDel.Click += new System.EventHandler(this.actionPlugButton_Execute);
            // 
            // toolPlugRefresh
            // 
            this.toolPlugRefresh.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolPlugRefreshFilter});
            this.toolPlugRefresh.Enabled = false;
            this.toolPlugRefresh.Image = ((System.Drawing.Image)(resources.GetObject("toolPlugRefresh.Image")));
            this.toolPlugRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolPlugRefresh.Name = "toolPlugRefresh";
            this.toolPlugRefresh.Size = new System.Drawing.Size(137, 22);
            this.toolPlugRefresh.Text = "Обновить &данные";
            this.toolPlugRefresh.ButtonClick += new System.EventHandler(this.actionPlugButton_Execute);
            // 
            // toolPlugRefreshFilter
            // 
            this.actionList.SetAction(this.toolPlugRefreshFilter, this.actionPlugRefreshFilter);
            this.toolPlugRefreshFilter.Enabled = false;
            this.toolPlugRefreshFilter.Image = global::GGPlatform.MainApp.Properties.Resources._00109;
            this.toolPlugRefreshFilter.Name = "toolPlugRefreshFilter";
            this.toolPlugRefreshFilter.ShortcutKeys = System.Windows.Forms.Keys.F6;
            this.toolPlugRefreshFilter.Size = new System.Drawing.Size(214, 22);
            this.toolPlugRefreshFilter.Text = "&Использовать фильтр";
            // 
            // actionList
            // 
            this.actionList.Actions.Add(this.actionConnect);
            this.actionList.Actions.Add(this.actionDisconnect);
            this.actionList.Actions.Add(this.actionExit);
            this.actionList.Actions.Add(this.actionPlugRefresh);
            this.actionList.Actions.Add(this.actionPlugOption);
            this.actionList.Actions.Add(this.actionPlugRefreshFilter);
            this.actionList.Actions.Add(this.actionPlugListShow);
            this.actionList.Actions.Add(this.actionPlugListAdd);
            this.actionList.Actions.Add(this.actionPlugListDel);
            this.actionList.ContainerControl = this;
            // 
            // actionConnect
            // 
            this.actionConnect.Image = ((System.Drawing.Image)(resources.GetObject("actionConnect.Image")));
            this.actionConnect.ShortcutKeys = System.Windows.Forms.Keys.F9;
            this.actionConnect.Text = "&Подключиться к БД";
            this.actionConnect.ToolTipText = "Подключиться к базе данных (F9)";
            this.actionConnect.Execute += new System.EventHandler(this.actionConnect_Execute);
            // 
            // actionDisconnect
            // 
            this.actionDisconnect.Image = ((System.Drawing.Image)(resources.GetObject("actionDisconnect.Image")));
            this.actionDisconnect.ShortcutKeys = System.Windows.Forms.Keys.F10;
            this.actionDisconnect.Text = "&Отключиться от БД";
            this.actionDisconnect.ToolTipText = "Отключиться от базы данных (F10)";
            this.actionDisconnect.Execute += new System.EventHandler(this.actionDisconnect_Execute);
            // 
            // actionExit
            // 
            this.actionExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.actionExit.Text = "Выход";
            this.actionExit.ToolTipText = "Выйти из программы (Alt+F4)";
            this.actionExit.Execute += new System.EventHandler(this.actionExit_Execute);
            // 
            // actionPlugOption
            // 
            this.actionPlugOption.Enabled = false;
            this.actionPlugOption.Image = ((System.Drawing.Image)(resources.GetObject("actionPlugOption.Image")));
            this.actionPlugOption.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.actionPlugOption.Text = "Настройки рабочего места";
            this.actionPlugOption.ToolTipText = "Настройки режима";
            this.actionPlugOption.Execute += new System.EventHandler(this.actionPlugButton_Execute);
            // 
            // actionPlugListAdd
            // 
            this.actionPlugListAdd.Image = ((System.Drawing.Image)(resources.GetObject("actionPlugListAdd.Image")));
            this.actionPlugListAdd.Text = "&Добавить";
            this.actionPlugListAdd.ToolTipText = "Добавить (обновить) текущий список";
            this.actionPlugListAdd.Execute += new System.EventHandler(this.actionPlugButton_Execute);
            // 
            // actionPlugListDel
            // 
            this.actionPlugListDel.Image = ((System.Drawing.Image)(resources.GetObject("actionPlugListDel.Image")));
            this.actionPlugListDel.Text = "Исключить";
            this.actionPlugListDel.ToolTipText = "Исключить список из текущих списков";
            // 
            // actionPlugRefreshFilter
            // 
            this.actionPlugRefreshFilter.Image = global::GGPlatform.MainApp.Properties.Resources._00109;
            this.actionPlugRefreshFilter.ShortcutKeys = System.Windows.Forms.Keys.F6;
            this.actionPlugRefreshFilter.Text = "&Использовать фильтр";
            this.actionPlugRefreshFilter.ToolTipText = "Фильтровать данные";
            this.actionPlugRefreshFilter.Execute += new System.EventHandler(this.actionPlugButton_Execute);
            // 
            // actionPlugRefresh
            // 
            this.actionPlugRefresh.Image = ((System.Drawing.Image)(resources.GetObject("actionPlugRefresh.Image")));
            this.actionPlugRefresh.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.actionPlugRefresh.Text = "Обновить &данные";
            this.actionPlugRefresh.ToolTipText = "Обновить данные (F5)";
            this.actionPlugRefresh.Execute += new System.EventHandler(this.actionPlugButton_Execute);
            // 
            // actionPlugListShow
            // 
            this.actionPlugListShow.Image = ((System.Drawing.Image)(resources.GetObject("actionPlugListShow.Image")));
            this.actionPlugListShow.Text = "&Списки";
            this.actionPlugListShow.ToolTipText = "Выбрать текущие списки";
            this.actionPlugListShow.Execute += new System.EventHandler(this.actionPlugButton_Execute);
            // 
            // statusStripContainer
            // 
            this.statusStripContainer.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusDB,
            this.toolStripStatusProperty,
            this.toolStripStatusRows,
            this.toolStripStatusInfo,
            this.toolStripProgressBar});
            this.statusStripContainer.Location = new System.Drawing.Point(0, 491);
            this.statusStripContainer.Name = "statusStripContainer";
            this.statusStripContainer.Size = new System.Drawing.Size(1235, 22);
            this.statusStripContainer.TabIndex = 3;
            this.statusStripContainer.Resize += new System.EventHandler(this.statusStrip_Resize);
            // 
            // toolStripStatusDB
            // 
            this.toolStripStatusDB.AutoSize = false;
            this.toolStripStatusDB.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.toolStripStatusDB.BorderStyle = System.Windows.Forms.Border3DStyle.RaisedOuter;
            this.toolStripStatusDB.Name = "toolStripStatusDB";
            this.toolStripStatusDB.Size = new System.Drawing.Size(190, 17);
            this.toolStripStatusDB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusProperty
            // 
            this.toolStripStatusProperty.AutoSize = false;
            this.toolStripStatusProperty.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.toolStripStatusProperty.Name = "toolStripStatusProperty";
            this.toolStripStatusProperty.Size = new System.Drawing.Size(300, 17);
            this.toolStripStatusProperty.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusRows
            // 
            this.toolStripStatusRows.AutoSize = false;
            this.toolStripStatusRows.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.toolStripStatusRows.Name = "toolStripStatusRows";
            this.toolStripStatusRows.Size = new System.Drawing.Size(50, 17);
            // 
            // toolStripStatusInfo
            // 
            this.toolStripStatusInfo.AutoSize = false;
            this.toolStripStatusInfo.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.toolStripStatusInfo.Name = "toolStripStatusInfo";
            this.toolStripStatusInfo.Size = new System.Drawing.Size(150, 17);
            this.toolStripStatusInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripProgressBar
            // 
            this.toolStripProgressBar.Name = "toolStripProgressBar";
            this.toolStripProgressBar.Size = new System.Drawing.Size(200, 16);
            this.toolStripProgressBar.Visible = false;
            // 
            // toolStripPlugin
            // 
            this.toolStripPlugin.Location = new System.Drawing.Point(0, 49);
            this.toolStripPlugin.Name = "toolStripPlugin";
            this.toolStripPlugin.Size = new System.Drawing.Size(1235, 25);
            this.toolStripPlugin.TabIndex = 5;
            this.toolStripPlugin.Text = "toolStrip1";
            this.toolStripPlugin.Visible = false;
            // 
            // exportReestrToolStripMenuItem
            // 
            this.exportReestrToolStripMenuItem.Name = "exportReestrToolStripMenuItem";
            this.exportReestrToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
            this.exportReestrToolStripMenuItem.Text = "&Экспорт настроек интерфейса";
            this.exportReestrToolStripMenuItem.Click += new System.EventHandler(this.exportReestrToolStripMenuItem_Click);
            // 
            // importReestrToolStripMenuItem
            // 
            this.importReestrToolStripMenuItem.Name = "importReestrToolStripMenuItem";
            this.importReestrToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
            this.importReestrToolStripMenuItem.Text = "&Импорт настроек интерфейса";
            this.importReestrToolStripMenuItem.Click += new System.EventHandler(this.importReestrToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(238, 6);
            // 
            // frmMainApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1235, 513);
            this.Controls.Add(this.toolStripPlugin);
            this.Controls.Add(this.MainAppToolStrip);
            this.Controls.Add(this.statusStripContainer);
            this.Controls.Add(this.MainAppToolMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.MainAppToolMenu;
            this.Name = "frmMainApp";
            this.Text = "GGPlatform";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMainApp_FormClosing);
            this.MdiChildActivate += new System.EventHandler(this.frmMainApp_MdiChildActivate);
            this.Shown += new System.EventHandler(this.frmMainApp_Shown);
            this.MainAppToolMenu.ResumeLayout(false);
            this.MainAppToolMenu.PerformLayout();
            this.MainAppToolStrip.ResumeLayout(false);
            this.MainAppToolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.actionList)).EndInit();
            this.statusStripContainer.ResumeLayout(false);
            this.statusStripContainer.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MainAppToolMenu;
        private System.Windows.Forms.ToolStripMenuItem toolMenuFile;
        private System.Windows.Forms.ToolStrip MainAppToolStrip;
        private System.Windows.Forms.ToolStripMenuItem toolMenuItemConnect;
        private System.Windows.Forms.ToolStripMenuItem toolMenuWorkplace;
        private System.Windows.Forms.ToolStripButton toolConnectDB;
        private System.Windows.Forms.ToolStripButton toolDisconnectDB;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private Crad.Windows.Forms.Actions.ActionList actionList;
        private Crad.Windows.Forms.Actions.Action actionConnect;
        private Crad.Windows.Forms.Actions.Action actionDisconnect;
        private Crad.Windows.Forms.Actions.Action actionExit;
        private System.Windows.Forms.ToolStripMenuItem toolMenuItemDisconnectToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem toolMenuItemExit;
        private System.Windows.Forms.ToolStripMenuItem ToolMenuHelp;
        private System.Windows.Forms.ToolStripMenuItem ToolMenuItemHelp;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem ToolMenuItemAbout;
        private Crad.Windows.Forms.Actions.Action actionPlugRefresh;
        private Crad.Windows.Forms.Actions.Action actionPlugOption;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton toolPlugOption;
        private System.Windows.Forms.StatusStrip statusStripContainer;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusDB;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusInfo;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusProperty;
        private System.Windows.Forms.ToolStripMenuItem ToolMenuWindow;
        private System.Windows.Forms.ToolStripMenuItem ToolWindowItemCascade;
        private System.Windows.Forms.ToolStripMenuItem ToolWindowItemArrange;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem ToolWindowItemArrangeIcons;
        private System.Windows.Forms.ToolStripMenuItem ToolWindowItemArrangeHor;
        private System.Windows.Forms.ToolStripMenuItem ToolWindowItemArrangeVer;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusRows;
        private System.Windows.Forms.ToolStrip toolStripPlugin;
        private System.Windows.Forms.ToolStripSplitButton toolPlugRefresh;
        private Crad.Windows.Forms.Actions.Action actionPlugRefreshFilter;
        private Crad.Windows.Forms.Actions.Action actionPlugListShow;
        private Crad.Windows.Forms.Actions.Action actionPlugListAdd;
        private System.Windows.Forms.ToolStripSplitButton toolPlugListShow;
        private System.Windows.Forms.ToolStripMenuItem toolPlugListAdd;
        private Crad.Windows.Forms.Actions.Action actionPlugListDel;
        private System.Windows.Forms.ToolStripMenuItem toolPlugListDel;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar;
        private System.Windows.Forms.ToolStripMenuItem toolPlugRefreshFilter;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem exportReestrToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importReestrToolStripMenuItem;
    }
}

