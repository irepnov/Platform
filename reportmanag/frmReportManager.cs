using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Reflection;

using GGPlatform.BuiltInApp;
using GGPlatform.DBServer;
using GGPlatform.RFCManag;
using GGPlatform.InspectorManagerSP;
using GGPlatform.RegManager;

namespace GGPlatform.reportmanag
{
    public partial class frmReportManager : Form, GGPlatform.BuiltInApp.IPlugin
    {
        internal class Report
        {
            private IReport _report = null;
            private PropertyGridEx.PropertyGridEx _params = null;
            private CInspectorManager _inspector = null;

            public Report(IReport inReport, PropertyGridEx.PropertyGridEx inParams, CInspectorManager inInspector)
            {
                _report = inReport;
                _params = inParams;
                _inspector = inInspector;
            }

            public IReport ReportItem { set { _report = value; } get { return _report; } }
            public PropertyGridEx.PropertyGridEx ParamItem { set { _params = value; } get { return _params; } }
            public CInspectorManager InspectorItem { set { _inspector = value; } get { return _inspector; } }
        }
        internal class ReportDescription
        {
            public int ReportID = -1;
            public int ReportGroupID = -1;
            public string ReportAssembly = "";

            public ReportDescription(int inReportID, int inReportGroupID, string inReportAssembly)
            {
                ReportID = inReportID;
                ReportGroupID = inReportGroupID;
                ReportAssembly = inReportAssembly;
            }
        }

        public frmReportManager()
        {
            InitializeComponent();
        }
        
        private object _MainApp = null;
        private Form _MainForm = null;
        private int _PluginID;
        private int _WorkplaceID;
        private string _PluginAssembly, _PluginName, _SoftName = "";
        private DBSqlServer _dbsql = null;
        private DelegClosePlugin _ClosePlugin;
        private RFCManager _rfc = null;
        private Int32 _AuditID;

        private IReport _CurrentReport = null;
        private CInspectorManager _CurrentInspector = null;
        private List<Report> _Reports = new List<Report>();
        private PropertyGridEx.PropertyGridEx _CurrentParams = null;
        private TreeNode _parentNode = null;

        int IPlugin.plugPluginButtons
        {
            get
            {
                return (int)EnumPluginButtons.EPB_Option;
            }
        }
        ToolStrip IPlugin.plugToolStrip { get { return null; } }
        void IPlugin.plugInit(object inMainApp, DBSqlServer inDBSqlServer, RFCManager inRFCManager, ref Lists inActiveLists)
        {
            _MainApp = inMainApp;
            _MainForm = (Form)_MainApp;
            _dbsql = inDBSqlServer;
            _rfc = inRFCManager;

            this.MdiParent = _MainForm;
            this.Show();
            this.WindowState = FormWindowState.Maximized;
            this.Text = _PluginName;
            this._dgvcontrol.dataGridViewGG.GGRegistrySoftName = _SoftName;
            this._dgvcontrol.dataGridViewGG.GGRegistryAssemblyName = _PluginAssembly;

            _dgvcontrol.dataGridViewGG.AllowUserToAddRows = false;
            _dgvcontrol.dataGridViewGG.AllowUserToDeleteRows = false;
            _dgvcontrol.dataGridViewGG.AllowUserToOrderColumns = true;
            _dgvcontrol.dataGridViewGG.AutoGenerateColumns = false;
            _dgvcontrol.dataGridViewGG.MultiSelect = false;
            _dgvcontrol.dataGridViewGG.ReadOnly = true;
        }
        string IPlugin.plugAssemblyName
        {
            get { return _PluginAssembly; }
            set { _PluginAssembly = value; }
        }
        string IPlugin.plugSoftName
        {
            get { return _SoftName; }
            set { _SoftName = value; }
        }
        string IPlugin.plugName
        {
            get { return _PluginName; }
            set { _PluginName = value; }
        }       
        void IPlugin.plugDeinit() 
        {
            GGPlatform.RegManager.RegManag _reg = new RegManag();
            string _p = "Software\\GGPlatform\\" + _SoftName + "\\" + _PluginAssembly;
            _reg.GGRegistrySetValue("SplitterWidth", splitContainer1.SplitterDistance, _p);
            _reg = null;

            ///////////уничтожени отчетов из массива
            if (_parentNode != null)
                _parentNode = null;
            if (_CurrentParams != null)
            {
                _CurrentParams.Dispose();
                _CurrentParams = null;
            }
            if (_CurrentInspector != null)
                _CurrentInspector = null;
            if (_CurrentReport != null)
            {
                _CurrentReport.reportDeinit();
                _CurrentReport = null;
            }
            for (int i = 0; i <= _Reports.Count - 1; i++)
            {
                _Reports[i].ReportItem.reportDeinit();
                _Reports[i].InspectorItem.ClearPropertyGrid();
                _Reports[i].ParamItem.Dispose();
                _Reports[i] = null;
                _Reports[i] = null;
            }
            _Reports.Clear();
            _Reports = null;
        }
        void IPlugin.appRefresh() { }
        void IPlugin.appRefreshFilter() { }
        void IPlugin.appOption() { }
        void IPlugin.appListAdd() { }
        void IPlugin.appListDel() { }
        int IPlugin.plugWorkplaceID
        {
            get { return _WorkplaceID; }
            set { _WorkplaceID = value; }
        }       
        int IPlugin.plugPluginID
        {
            get { return _PluginID; }
            set { _PluginID = value; }
        }
        DelegClosePlugin IPlugin.plugEventClosed { set { _ClosePlugin = value; } }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            _ClosePlugin(_WorkplaceID, _PluginID);
        }
        private bool GetReportByName(ReportDescription inReportDescription, ref IReport inReport, ref PropertyGridEx.PropertyGridEx inProp, ref CInspectorManager inInspector)
        {
             string ReportAssembly = inReportDescription.ReportAssembly;
             int ReportID = inReportDescription.ReportID;
             int ReportGroupID = inReportDescription.ReportGroupID;
             Boolean _result = false;

             for (int i = 0; i <= _Reports.Count - 1; i++)
             {
                 if (_Reports[i].ReportItem.reportAssemblyName == ReportAssembly)
                 {
                     inReport = _Reports[i].ReportItem;
                     inProp = _Reports[i].ParamItem;
                     inInspector = _Reports[i].InspectorItem;
                     return true;
                 }
             }

             Assembly _Assembly = null;
             try
             {
                 if (ReportAssembly.ToUpper().Contains(".DLL"))
                     _Assembly = Assembly.LoadFrom(Application.StartupPath + "\\reports\\assembly\\" + ReportAssembly );  // отчет это самостоятельная сборка
                 else
                     if (ReportAssembly.ToUpper().Contains(".XML"))
                         _Assembly = Assembly.LoadFrom(Application.StartupPath + "\\reportxml.dll"); // отчет это XML файл , который обрабатывается единой сборкой 
                     else throw new Exception("Тип отчета не поддерживается платформой");

                 foreach (Type type in _Assembly.GetTypes())
                 {
                     Type iface = type.GetInterface("IReport", true);
                     if (iface != null)
                     {
                         _CurrentReport = (IReport)Activator.CreateInstance(type);                       
                         _CurrentReport.reportAssemblyName = ReportAssembly;
                         _CurrentReport.reportID = ReportID;
                         _CurrentReport.reportInit(_MainForm, _dgvcontrol, _dbsql, _rfc);
                         _Reports.Add(new Report(_CurrentReport, _CurrentReport.reportGetParameters, _CurrentReport.reportGetInspector));
                         iface = null;
                     }
                 }
                 inReport = _CurrentReport;
                 inProp = _CurrentReport.reportGetParameters;
                 inInspector = _CurrentReport.reportGetInspector;
                 PropertyGridEx.PropertyGridEx pg = inProp;
                 pg.Parent = splitContainer1.Panel1;
                 pg.Dock = DockStyle.Fill;
                 pg.Visible = false;
                 _result = true;
             }
             catch (Exception ex)
             {
                 _result = false;
                 MessageBox.Show(_MainForm,
                                 "Ошибка загрузки отчета \n" + ex.Message,
                                 "Ошибка [модуль ReportManager, класс frmReportManager, метод GetReportByName]",
                                 MessageBoxButtons.OK, MessageBoxIcon.Error);
             }
             finally
             {

             }
             return _result;
        }
        private void SetVisiblePropertyGrid(PropertyGrid inParams)
        {
            foreach (PropertyGridEx.PropertyGridEx pg in splitContainer1.Panel1.Controls)
                pg.Visible = (pg.Tag.ToString() == inParams.Tag.ToString());
        }
        private void frmReportManager_Load(object sender, EventArgs e)
        {
            try
            {
                if (_dbsql == null)
                    return;

                treeViewList.Nodes.Clear();
                _dbsql.SQLScript = "exec [GGPlatform].[usp_GetReportsGroups] @TypeQuery = 1";
                DataTable dt = _dbsql.FillDataSet().Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    _parentNode = treeViewList.Nodes.Add(dr["Name"].ToString());
                    PopulateTreeView(Convert.ToInt32(dr["ID"].ToString()), _parentNode);
                    _parentNode.ImageIndex = 9;
                    _parentNode.SelectedImageIndex = 6;
                }
                treeViewList.ExpandAll();
                dt.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_MainForm,
                                "Ошибка загрузки дерева отчетов \n" + ex.Message,
                                "Ошибка [модуль ReportManager, класс frmReportManager, метод frmReportManager_Load]",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void PopulateTreeView(int parentId, TreeNode parentNode)
        {
            try
            {
                _dbsql.SQLScript = "exec [GGPlatform].[usp_GetReportsGroups] @TypeQuery = 2, @GroupReportID = @pGroupReportID";
                _dbsql.AddParameter("@pGroupReportID", EnumDBDataTypes.EDT_Integer, parentId.ToString());
                DataTable dtchildc = _dbsql.FillDataSet().Tables[0];
                TreeNode childNode;
                foreach (DataRow dr in dtchildc.Rows)
                {
                    if (parentNode == null)
                        childNode = treeViewList.Nodes.Add(dr["Name"].ToString());
                    else
                        childNode = parentNode.Nodes.Add(dr["Name"].ToString());
                    childNode.Text = dr["Name"].ToString();
                    childNode.Tag = new ReportDescription((Int32)dr["ID"],
                                                          parentId,
                                                          dr["AssemblyName"].ToString());
                    childNode.ImageIndex = 0;
                    childNode.SelectedImageIndex = 2;
                }
                dtchildc.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_MainForm,
                                "Ошибка чтения из БД информации об отчете \n" + ex.Message,
                                "Ошибка [модуль ReportManager, класс frmReportManager, метод PopulateTreeView]",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnExcel_ButtonClick(object sender, EventArgs e)
        {
            if (_CurrentReport != null)
                try
                {
                    _MainForm.Cursor = System.Windows.Forms.Cursors.WaitCursor;

                    _dbsql.SQLScript = "exec [GGPlatform].[usp_AuditManager] @TypeQuery = 1, @Login = @pLogin, @ReportRef = @pReportRef";
                    _dbsql.AddParameter("@pLogin", EnumDBDataTypes.EDT_String, _dbsql.InfoAboutConnection.UserLogin);
                    _dbsql.AddParameter("@pReportRef", EnumDBDataTypes.EDT_Integer, _CurrentReport.reportID.ToString());
                    _AuditID = Convert.ToInt32(_dbsql.ExecuteScalar());

                    _CurrentReport.reportGetDataToExcel(_CurrentInspector.GetDictionaryValues()); 
                     //добавить параметры   - константы
                    _dbsql.SQLScript = "exec [GGPlatform].[usp_AuditManager] @TypeQuery = 2, @ObjectID = @pObjectID";
                    _dbsql.AddParameter("@pObjectID", EnumDBDataTypes.EDT_Integer, _AuditID.ToString());
                    _dbsql.ExecuteNonQuery();

                    _MainForm.Cursor = System.Windows.Forms.Cursors.Default;
                }
                catch (Exception ex)
                {
                    _dbsql.SQLScript = "exec [GGPlatform].[usp_AuditManager] @TypeQuery = 3, @ObjectID = @pObjectID";
                    _dbsql.AddParameter("@pObjectID", EnumDBDataTypes.EDT_Integer, _AuditID.ToString());
                    _dbsql.ExecuteNonQuery();

                    _MainForm.Cursor = System.Windows.Forms.Cursors.Default;

                    MessageBox.Show(_MainForm,
                                    "Ошибка выполнения отчета \n" + ex.Message,
                                    "Ошибка [модуль ReportManager, класс frmReportManager, метод btnExcel_ButtonClick]",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    _MainForm.Cursor = System.Windows.Forms.Cursors.Default;
                }
        }
        private void btnWord_Click(object sender, EventArgs e)
        {
            if (_CurrentReport != null)
                try
                {
                    _MainForm.Cursor = System.Windows.Forms.Cursors.WaitCursor;

                    _dbsql.SQLScript = "exec [GGPlatform].[usp_AuditManager] @TypeQuery = 1, @Login = @pLogin, @ReportRef = @pReportRef";
                    _dbsql.AddParameter("@pLogin", EnumDBDataTypes.EDT_String, _dbsql.InfoAboutConnection.UserLogin);
                    _dbsql.AddParameter("@pReportRef", EnumDBDataTypes.EDT_Integer, _CurrentReport.reportID.ToString());
                    _AuditID = Convert.ToInt32(_dbsql.ExecuteScalar());

                    _CurrentReport.reportGetDataToWord(_CurrentInspector.GetDictionaryValues());
                    //добавить параметры   - константы
                    _dbsql.SQLScript = "exec [GGPlatform].[usp_AuditManager] @TypeQuery = 2, @ObjectID = @pObjectID";
                    _dbsql.AddParameter("@pObjectID", EnumDBDataTypes.EDT_Integer, _AuditID.ToString());
                    _dbsql.ExecuteNonQuery();

                    _MainForm.Cursor = System.Windows.Forms.Cursors.Default;
                }
                catch (Exception ex)
                {
                    _dbsql.SQLScript = "exec [GGPlatform].[usp_AuditManager] @TypeQuery = 3, @ObjectID = @pObjectID";
                    _dbsql.AddParameter("@pObjectID", EnumDBDataTypes.EDT_Integer, _AuditID.ToString());
                    _dbsql.ExecuteNonQuery();

                    _MainForm.Cursor = System.Windows.Forms.Cursors.Default;

                    MessageBox.Show(_MainForm,
                                    "Ошибка выполнения отчета \n" + ex.Message,
                                    "Ошибка [модуль ReportManager, класс frmReportManager, метод btnWord_Click]",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    _MainForm.Cursor = System.Windows.Forms.Cursors.Default;
                }
        }
        private bool RefreshReportByName(ReportDescription inReportDescription)
        {
            string ReportAssembly = inReportDescription.ReportAssembly;
            Boolean _result = false;

            try
            {
                foreach (PropertyGridEx.PropertyGridEx pg in splitContainer1.Panel1.Controls)
                    if (pg.Visible)
                        pg.Enabled = false;

                btnExcel.Enabled = false;

                for (int i = 0; i <= _Reports.Count - 1; i++)
                {
                    if (_Reports[i].ReportItem.reportAssemblyName == ReportAssembly)
                    {
                        _Reports[i].ReportItem.reportDeinit();
                        _Reports[i].InspectorItem.ClearPropertyGrid();
                        _Reports[i].ParamItem.Dispose();
                        _Reports[i] = null;
                        _Reports.RemoveAt(i);
                        break;
                    }
                }

                _result = true;
            }
            catch (Exception ex)
            {
                _result = false;
                MessageBox.Show(_MainForm,
                                "Ошибка перезагрузки отчета \n" + ex.Message,
                                "Ошибка [модуль ReportManager, класс frmReportManager, метод RefreshReportByName]",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {

            }
            return _result;
        }
        private void treeViewList_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F12)
            {
                if ((treeViewList.SelectedNode == null) || (treeViewList.SelectedNode.ImageIndex != 0))
                    return;

                if (RefreshReportByName((ReportDescription)treeViewList.SelectedNode.Tag))
                    treeViewList_AfterSelect(null, null);
            }
        }
        private void treeViewList_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeViewList.SelectedNode == null)
                return;

            if (treeViewList.SelectedNode.ImageIndex != 0)
            {
                foreach (PropertyGridEx.PropertyGridEx pg in splitContainer1.Panel1.Controls)
                    if (pg.Visible)
                        pg.Enabled = false;

                btnExcel.Enabled = false;
            }
            else
                if (GetReportByName((ReportDescription)treeViewList.SelectedNode.Tag, ref _CurrentReport, ref _CurrentParams, ref _CurrentInspector))
                {
                    int MaskButtons = 0;
                    MaskButtons = _CurrentReport.reportButtons;
                    btnExcel.Enabled = ((MaskButtons & (int)EnumReportButtons.ERB_Excel) != 0);
                    btnWord.Enabled = ((MaskButtons & (int)EnumReportButtons.ERB_Word) != 0);

                    foreach (PropertyGridEx.PropertyGridEx pg in splitContainer1.Panel1.Controls)
                        if (pg.Visible)
                            pg.Enabled = true;

                    btnExcel.Enabled = true;
                    SetVisiblePropertyGrid(_CurrentParams);
                }
        }
        private void frmReportManager_Shown(object sender, EventArgs e)
        {
            if (!_dbsql.InfoAboutConnection.UserIsAdmin)
                tabControl.TabPages.Remove(tabPageData);

            GGPlatform.RegManager.RegManag _reg = new RegManag();
            string _p = "Software\\GGPlatform\\" + _SoftName + "\\" + _PluginAssembly;
            splitContainer1.SplitterDistance = Convert.ToInt32(_reg.GGRegistryGetValue("SplitterWidth", _p));
            _reg = null;
        }
    }
}
