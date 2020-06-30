using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Globalization;
using System.Windows.Forms;
using GGPlatform.BuiltInApp;
using GGPlatform.DBServer;
using GGPlatform.RFCManag;
using GGPlatform.RegManager;

namespace GGPlatform.MainApp
{
    public partial class frmMainApp : Form
    {
        public frmMainApp()
        {
            InitializeComponent();
        }


        /*****************************   Кнопки, автивация, деактивация   *****************************/
        private void ManagerActions(Boolean isEnabled)
        {
            actionConnect.Enabled      = !isEnabled;
            actionDisconnect.Enabled   = isEnabled;
            toolMenuWorkplace.Enabled  = isEnabled;
        }
        private void ManagerToolButtons()
        {
            int MaskButtons = 0;           
            if (_CurrentPlugin != null)
            {
                MaskButtons = _CurrentPlugin.plugPluginButtons;
                actionPlugOption.Enabled = ((MaskButtons & (int)EnumPluginButtons.EPB_Option) != 0);

                toolPlugRefresh.Enabled = ((MaskButtons & (int)EnumPluginButtons.EPB_Refresh) != 0);
                toolPlugRefreshFilter.Enabled = ((MaskButtons & (int)EnumPluginButtons.EPB_RefreshFilter) != 0);
                toolPlugRefreshFilter.Visible = ((MaskButtons & (int)EnumPluginButtons.EPB_RefreshFilter) != 0);

                toolPlugListShow.Enabled = ((MaskButtons & (int)EnumPluginButtons.EPB_ListShow) != 0);
                toolPlugListAdd.Enabled = ((MaskButtons & (int)EnumPluginButtons.EPB_ListShow) != 0);
                toolPlugListAdd.Visible = ((MaskButtons & (int)EnumPluginButtons.EPB_ListShow) != 0);
                toolPlugListDel.Enabled = ((MaskButtons & (int)EnumPluginButtons.EPB_ListShow) != 0);
                toolPlugListDel.Visible = ((MaskButtons & (int)EnumPluginButtons.EPB_ListShow) != 0);             

                toolStripPlugin.Enabled = true;
                toolStripPlugin.Visible = true;
                ToolStrip _tmpToolStrip = null;
                _tmpToolStrip = _CurrentPlugin.plugToolStrip;
                if (_tmpToolStrip != null)
                {
                    ToolStripManager.RevertMerge(toolStripPlugin);
                    ToolStripManager.Merge(_tmpToolStrip, toolStripPlugin);
                }
                else
                {
                    toolStripPlugin.Enabled = false;
                    toolStripPlugin.Visible = false;
                }
                _tmpToolStrip = null;
            }
            else
            {
                actionPlugOption.Enabled = false;

                toolPlugRefresh.Enabled = false;
                toolPlugRefreshFilter.Enabled = false;
                toolPlugRefreshFilter.Visible = false;

                toolPlugListShow.Enabled = false;
                toolPlugListAdd.Enabled = false;
                toolPlugListAdd.Visible = false;
                toolPlugListDel.Enabled = false;
                toolPlugListDel.Visible = false;
                
                toolStripPlugin.Enabled = false;
                toolStripPlugin.Visible = false;
            }
        }



        /*****************************   AppSettingsReader   *****************************/
        private AppSettingsReader _ConfigReader = null;
        private string _CaptionPlatform = "";
        private void CreateConfigReader()
        {
            _ConfigReader = new AppSettingsReader();
            if (_ConfigReader == null)
            {
                MessageBox.Show(this, "Не найден файл конфигурации приложения: \n" + Application.StartupPath + "\\mainapp.exe.config",
                                "Ошибка [модуль MainApp, метод CreateConfigReader]", 
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();               
            }

            _Servername = (string)(_ConfigReader.GetValue("Servername", typeof(string)));
            _Basename = (string)(_ConfigReader.GetValue("Basename", typeof(string)));
            _ConnectionString = (string)(_ConfigReader.GetValue("ConnectionString", typeof(string)));
            _CaptionPlatform = (string)(_ConfigReader.GetValue("CaptionPlatform", typeof(string)));

            if (_Servername == "" && _Basename == "")
            {
                MessageBox.Show(this, "В файле конфигурации приложения не указан сервер или база данных: \n" + "Проверьте конфигурационный файл",
                                "Внимание",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            if (_Servername == "" && _Basename != "")
            {
                MessageBox.Show(this, "В файле конфигурации приложения не указан сервер баз данных: \n" + "Проверьте конфигурационный файл",
                                "Внимание",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            if (_Servername != "" && _Basename == "")
            {
                MessageBox.Show(this, "В файле конфигурации приложения не указано название базы данных: \n" + "Проверьте конфигурационный файл",
                                "Внимание",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            if (_CaptionPlatform == "")
            {
                MessageBox.Show(this, "В файле конфигурации приложения не указано название программы: \n" + "Проверьте конфигурационный файл",
                                "Внимание",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }



        /*****************************   DBServer   *****************************/
        private DBSqlServer _DBSql = null;
        private string _Servername = "";
        private string _Basename = "";
        private Int32 _AuditID = 0;
        private string _ConnectionString = "";

        private void CreateDBSqlServer()
        {
            try
            {
                if (_DBSql == null)
                    _DBSql = new DBSqlServer(_Servername, _Basename, this, _CaptionPlatform);
            }
            catch (Exception err)
            {
                MessageBox.Show(this, err.Message, "Ошибка",  MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cultureNew()
        {
            //изменю региональные настройки
            CultureInfo culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            culture.DateTimeFormat.DateSeparator = ".";
            culture.DateTimeFormat.ShortDatePattern = "dd.MM.yyyy";
            
            culture.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        private void actionConnect_Execute(object sender, EventArgs e)
        {
            CreateDBSqlServer();
            try 
            {
                if (_DBSql != null)
                    _DBSql.ConnectVisual();
            }
            catch (Exception err)
            {
                MessageBox.Show(this, err.Message, "Ошибка",  MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (_DBSql.Connection != null && _DBSql.Connection.State == ConnectionState.Open)
            {
                cultureNew();
                InitRFC();
                CreateCMP();
                ManagerActions(true);
                MakeMenuWorkplace();
                toolStripStatusDB.Text = "Соединение с БД установлено";
                toolStripStatusProperty.Text = "сервер [" + _DBSql.InfoAboutConnection.ServerName +
                                               "] база [" + _DBSql.InfoAboutConnection.BaseName +
                                               "] пользователь [" + _DBSql.InfoAboutConnection.UserLogin + "]";
                                                
            }
            else actionDisconnect_Execute(null, null);
        }
        private void actionDisconnect_Execute(object sender, EventArgs e)
        {
            try 
            {
                if (_DBSql != null && _DBSql.Connection != null)
                    _DBSql.Disconnect();
                _DBSql = null;
                _Workplace = null;
                _CMP = null; 

                //  ????   закрыть все справочники  ???

                CloseMDIChild(); //закрываю Чилд и соответсвенно плагины
                ManagerActions(false);
                ManagerToolButtons();
                toolStripStatusDB.Text = "Соединение с БД отсутствует";
                toolStripStatusProperty.Text = "";
            }
            catch (Exception err)
            {
                MessageBox.Show(this, err.Message, "Ошибка",  MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /********************** RFCManager   ***********************/
        private RFCManager _rfc = null;
        private void InitRFC()
        {
            try 
            {
                if (_rfc == null)
                    _rfc = new RFCManager(this, _DBSql, _CaptionPlatform);
            }
            catch (Exception err)
            {
                MessageBox.Show(this, err.Message, "Ошибка",  MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /*****************************   ClassManagerWorkplaces   *****************************/
        private ClassManagerWorkplaces _CMP = null;
        private DelegClosePlugin _DelegClosePlugin = null;
        private ClassWorkplaces _Workplace = null;
        private Lists _ActiveList = null;


        private void CreateCMP()
        {
            if (_ActiveList == null)
                _ActiveList = new Lists();
            if (_CMP == null)
                _CMP = new ClassManagerWorkplaces(this, _DBSql, _rfc, ref _ActiveList);
            _DelegClosePlugin = new DelegClosePlugin(_CMP.DelPlugin);
        }


        /*****************************   работа с текущими списками  *****************************/
        private void ActiveListsShow() ///показать списки
        {
            if (_ActiveList != null)
                _ActiveList.ShowLists();
        }


        /*****************************   загрузка плагинов и создание меню  *****************************/
        private IPlugin _CurrentPlugin = null;
        
        private void MakeMenuWorkplace()
        {
            try 
            { 
               // _CMP.DBSqlServer = _DBSql; ///во время дисконнекта в экземпляре класса _CMP остается закрытый сервер БД и он передается в плагины..., поэтому повтрно его обновляю
                toolMenuWorkplace.DropDownItems.Clear(); //очищаю список ранее загруженных рабочих мест
                int _WorkplaceID, _PluginID = 0;
                //читаю рабочие места
                _DBSql.SQLScript = "execute GGPlatform.usp_GetWorkplacesAssemblys @TypeQuery = 1";
                DataSet _dsWorkplace = _DBSql.FillDataSet();
                for (int i = 0; i < _dsWorkplace.Tables[0].Rows.Count; i++)
                {
                    _WorkplaceID = Convert.ToInt16(_dsWorkplace.Tables[0].Rows[i]["ID"].ToString());
                    ToolStripMenuItem _tmpWorkplaceItem = new ToolStripMenuItem();
                    _tmpWorkplaceItem.Text = _dsWorkplace.Tables[0].Rows[i]["Name"].ToString();
                    _tmpWorkplaceItem.Tag = _WorkplaceID;
                    toolMenuWorkplace.DropDownItems.Add(_tmpWorkplaceItem);
                    //добавляю рабочее место
                    _Workplace = new ClassWorkplaces(_WorkplaceID, this);  ///первое рабочее место

                    //читаю сборки рабочего места
                    _DBSql.SQLScript = "execute GGPlatform.usp_GetWorkplacesAssemblys @TypeQuery = 2, @WorkplaceID = @pWorkplaceID";               
                    _DBSql.AddParameter("@pWorkplaceID", EnumDBDataTypes.EDT_Integer, _WorkplaceID.ToString());
                    DataSet _dsAssemblys = _DBSql.FillDataSet();
                    for (int j = 0; j < _dsAssemblys.Tables[0].Rows.Count; j++)
                    {
                        _PluginID = Convert.ToInt16(_dsAssemblys.Tables[0].Rows[j]["ID"].ToString());
                        ToolStripMenuItem _tmpAssemblyItem = new ToolStripMenuItem();
                        _tmpAssemblyItem.Text = _dsAssemblys.Tables[0].Rows[j]["Name"].ToString();
                        //_tmpAssemblyItem.ShortcutKeys = 
                        _tmpAssemblyItem.Tag = _PluginID;
                        _tmpAssemblyItem.Click += new EventHandler(myToolStripPlugin_Click);
                        _tmpWorkplaceItem.DropDownItems.Add(_tmpAssemblyItem);

                        //наполняю рабочее место сборками
                        _Workplace.AddPlugin(_PluginID, _dsAssemblys.Tables[0].Rows[j]["AssemblyName"].ToString().Replace(".dll", ""), _tmpAssemblyItem.Text);
                    }
                    _dsAssemblys = null;
                    _CMP.AddWorkplace(_Workplace);
                }
                _dsWorkplace = null;
                toolMenuWorkplace.Enabled = true;
                toolMenuWorkplace.Visible = true;
            }
            catch (Exception err)
            {
                MessageBox.Show(this, err.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }  
        }
        private void CloseMDIChild()
        {
            try
            { 
                foreach (Form _Form in this.MdiChildren)
                    _Form.Close();
            }
            catch (Exception err)
            {
                MessageBox.Show(this, err.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void myToolStripPlugin_Click(object sender, EventArgs e)
        {
            try
            { 
                ToolStripMenuItem _workplace, _plugin = null;
                _plugin = (ToolStripMenuItem)sender;
                _workplace = (ToolStripMenuItem)_plugin.OwnerItem;

                _CurrentPlugin = null;
                _CurrentPlugin = _CMP.SetPlugin( (int)_workplace.Tag, (int)_plugin.Tag, _CaptionPlatform);

                if (_CurrentPlugin != null)
                {
                    ManagerToolButtons();
                    _CurrentPlugin.plugEventClosed = _DelegClosePlugin;

                    if (_DBSql.InfoAboutConnection.UserIsAudit)
                    {
                        _DBSql.SQLScript = "exec [GGPlatform].[usp_AuditManager] @TypeQuery = 11, @Login = @pLogin, @CommandText = @pCommandText, @Status = 1, @SessionID = @pSessionID, @DateEnd = @pDateEnd";
                        _DBSql.AddParameter("@pLogin", EnumDBDataTypes.EDT_String, _DBSql.InfoAboutConnection.UserLogin);
                        _DBSql.AddParameter("@pCommandText", EnumDBDataTypes.EDT_String, "Запуск плагина " + _CurrentPlugin.plugName);
                        _DBSql.AddParameter("@pSessionID", EnumDBDataTypes.EDT_Integer, _DBSql.InfoAboutConnection.SessionID.ToString());
                        _DBSql.AddParameter("@pDateEnd", EnumDBDataTypes.EDT_DateTime, DateTime.Now.ToString());
                        _AuditID = Convert.ToInt32(_DBSql.ExecuteScalar());
                    }
                }

                _plugin = null;
                _workplace = null;
            }
            catch (Exception err)
            {
                MessageBox.Show(this, err.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void frmMainApp_MdiChildActivate(object sender, EventArgs e)
        {
            _CurrentPlugin = null;
            _CurrentPlugin = (IPlugin)this.ActiveMdiChild;
            if (_CurrentPlugin != null)
                this.ActiveMdiChild.Text = _CurrentPlugin.plugName;   /////////Как-нибудь унифицировать!!!
            ManagerToolButtons();           
        }
        private void actionExit_Execute(object sender, EventArgs e)
        {
            actionDisconnect_Execute(null, null);
            Close();
        }
        private void actionPlugButton_Execute(object sender, EventArgs e)
        {
            if (_CurrentPlugin != null)
            {
                if (sender == toolPlugRefresh)  ///actionPlugRefresh
                    _CurrentPlugin.appRefresh();
                if (sender == actionPlugRefreshFilter)
                    _CurrentPlugin.appRefreshFilter();
                if (sender == actionPlugOption)
                    _CurrentPlugin.appOption();

                if (sender == toolPlugListShow)
                    ActiveListsShow();
                if (sender == toolPlugListAdd)
                    _CurrentPlugin.appListAdd();
                if (sender == toolPlugListDel)
                    _CurrentPlugin.appListDel();
            }
        }
        private void frmMainApp_Shown(object sender, EventArgs e)
        {
            actionDisconnect_Execute(null, null);
            CreateConfigReader();
            this.Text = _CaptionPlatform;
            actionConnect_Execute(null, null);
        }
        private void statusStrip_Resize(object sender, EventArgs e)
        {
            toolStripStatusDB.Width = 180;
            toolStripStatusRows.Width = 70;
            toolStripStatusProperty.Width = 500;
            toolStripProgressBar.Width = 200;

            toolStripStatusInfo.Width = statusStripContainer.Width - toolStripProgressBar.Width - toolStripStatusDB.Width - toolStripStatusRows.Width - toolStripStatusProperty.Width - 15;           
        }     
        private void frmMainApp_FormClosing(object sender, FormClosingEventArgs e)
        {
            actionDisconnect_Execute(null, null);
        }
        private void toolPlugClose_Click(object sender, EventArgs e)
        {
            this.Text = Application.UserAppDataPath.ToString();
        }
        private void ToolMenuItem_Click(object sender, EventArgs e)
        {
            if (sender == ToolWindowItemCascade)
                this.LayoutMdi(MdiLayout.Cascade);            
            if (sender == ToolWindowItemArrangeHor)
                this.LayoutMdi(MdiLayout.TileHorizontal); 
            if (sender == ToolWindowItemArrangeVer)
                this.LayoutMdi(MdiLayout.TileVertical); 
            if (sender == ToolWindowItemArrangeIcons)
                this.LayoutMdi(MdiLayout.ArrangeIcons); 
            if (sender == ToolMenuItemAbout)
            {
                frmAbout fmAbout = new frmAbout();
                fmAbout.ShowDialog();
                fmAbout = null;
            }
        }

        private void exportReestrToolStripMenuItem_Click(object sender, EventArgs e)
        {//\\" + _CaptionPlatform
            GGPlatform.RegManager.RegManag _reg = new RegManag();
            try
            {
                if (_reg.GGRegistryExport("C:\\Temp\\interface.reg", "HKEY_CURRENT_USER\\Software\\GGPlatform"))
                    MessageBox.Show(this, "Настройки интерфейса сохранены в файле \n" + "C:\\Temp1\\ExportInter.reg", _CaptionPlatform, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception err)
            {
                _reg = null;
                MessageBox.Show(this, err.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _reg = null;
            }
        }

        private void importReestrToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GGPlatform.RegManager.RegManag _reg = new RegManag();
            try
            {               
                if (_reg.GGRegistryImport("C:\\Temp\\interface.reg"))
                    MessageBox.Show(this, "Настройки интерфейса импортированы", _CaptionPlatform, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception err)
            {
                _reg = null;
                MessageBox.Show(this, err.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _reg = null;
            }
        }
    }
}