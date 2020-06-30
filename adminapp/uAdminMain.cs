using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using GGPlatform.BuiltInApp;
using GGPlatform.DBServer;
using GGPlatform.RFCManag;

namespace GGPlatform.adminapp
{
    public partial class frmAdminMain : Form, GGPlatform.BuiltInApp.IPlugin
    {
        public frmAdminMain()
        {
            InitializeComponent();
        }


        private object _MainApp = null;
        private int _PluginID, _WorkplaceID = 0;
        private string _PluginAssembly, _PluginName, _SoftName = "";
        private DBSqlServer _dbsql = null;
        private DelegClosePlugin _ClosePlugin;
        private BindingSource bs = new BindingSource();
        private int _ObjectType = -1;

        int IPlugin.plugPluginButtons
        {
            get { return (int)EnumPluginButtons.EPB_Refresh; }
        }
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
        ToolStrip IPlugin.plugToolStrip { get { return null/*toolStrip1*/; } }
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
        void IPlugin.plugDeinit() { }
        void IPlugin.appRefresh() { LoadGrid(); }
        void IPlugin.appRefreshFilter() { }
        void IPlugin.appOption() { }
        void IPlugin.plugInit(object MainApp, DBSqlServer inDBSqlServer, RFCManager inRFCManager, ref Lists inActiveLists)
        {
            _MainApp = MainApp;
            _dbsql = inDBSqlServer;
            this.MdiParent = (Form)_MainApp;
            this.Show();
            this.WindowState = FormWindowState.Maximized;
            this.Text = _PluginName;
            pnTreeView.Size = new Size(250, pnTreeView.Size.Height);
            _dgvcontrol.dataGridViewGG.GGRegistrySoftName = _SoftName;            
        }


        void IPlugin.appListAdd()
        {
        }

        void IPlugin.appListDel()
        {
        }




        private void frmAdminMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            _ClosePlugin(_WorkplaceID, _PluginID);
        }

        private void frmAdminMain_Load(object sender, EventArgs e)
        {
            treeView.ExpandAll();
            _dgvcontrol.dataGridViewGG.DoubleClick += new EventHandler(dataGridViewGG_DoubleClick);
            _dgvcontrol.dataGridViewGG.AllowUserToAddRows = false;
            _dgvcontrol.dataGridViewGG.AllowUserToDeleteRows = false;
            _dgvcontrol.dataGridViewGG.AllowUserToOrderColumns = true;
            _dgvcontrol.dataGridViewGG.AutoGenerateColumns = false;
            _dgvcontrol.dataGridViewGG.MultiSelect = false;
            _dgvcontrol.dataGridViewGG.ReadOnly = true;
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if ((treeView.SelectedNode.Index >= 0) && (treeView.SelectedNode.Parent != null))
            {
                _dgvcontrol.dataGridViewGG.GGSaveGridRegistry(); //сохраняю текущие настройки грида (прошлого)
                _ObjectType = treeView.SelectedNode.Index;
                LoadGrid();                
            }
        }

        private void LoadGrid()
        {
            tbDel.Enabled = true;
            switch (_ObjectType)
                {
                    case 0: { LoadGridSettings(); } break;
                    case 1: { LoadGridUsers(); } break;
                    case 2: { LoadGridWorkplaces(); } break;
                    case 3: { LoadGridAssemblys(); } break;
                    case 4: { LoadGridGroupReports(); } break;
                    case 5: { LoadGridReports(); } break;
                } 
            //загружу настройки грида
            _dgvcontrol.RefreshFilterFieldsGG();
            _dgvcontrol.dataGridViewGG.GGLoadGridRegistry();
        }

        private void dataGridViewGG_DoubleClick(object sender, EventArgs e)
        {
            //DataRow dr = ((DataTable)(bs.DataSource)).Rows[_dgvcontrol.dataGridViewGG.CurrentRow.Index];
            DataRow dr = ((DataRowView)_dgvcontrol.dataGridViewGG.CurrentRow.DataBoundItem).Row;

            frmElement fmElement = new frmElement((int)dr["ID"], dr, _dbsql, _ObjectType);            
            if (fmElement.ShowDialog() == DialogResult.OK)
                LoadGrid();
            fmElement.Dispose();
            fmElement = null;
        }

        private void LoadGridSettings()
        {
            tbDel.Enabled = false;
            _dgvcontrol.dataGridViewGG.GGRegistryAssemblyName = _PluginAssembly + "_settings";
            _dbsql.SQLScript = "select * from GGPlatform.Settings";

            bs.DataSource = _dbsql.FillDataSet().Tables[0];
            _dgvcontrol.dataGridViewGG.Columns.Clear();
            _dgvcontrol.dataGridViewGG.DataSource = bs;

            _dgvcontrol.dataGridViewGG.GGAddColumn("Name", "Наименование показателя", typeof(String));
            _dgvcontrol.dataGridViewGG.GGAddColumn("Description", "Описание", typeof(String));
            _dgvcontrol.dataGridViewGG.GGAddColumn("Key", "Ключ показателя", typeof(String));
            _dgvcontrol.dataGridViewGG.GGAddColumn("Value", "Значение показателя", typeof(String));
        }

        private void LoadGridUsers()
        {
            _dgvcontrol.dataGridViewGG.GGRegistryAssemblyName = _PluginAssembly + "_users";
            _dbsql.SQLScript = "select * from GGPlatform.Users";
            bs.DataSource = _dbsql.FillDataSet().Tables[0];
            _dgvcontrol.dataGridViewGG.Columns.Clear();
            _dgvcontrol.dataGridViewGG.DataSource = bs;

            _dgvcontrol.dataGridViewGG.GGAddColumn("Login", "Имя пользователя", typeof(String));
            _dgvcontrol.dataGridViewGG.GGAddColumn("Fam", "Фамилия", typeof(String));
            _dgvcontrol.dataGridViewGG.GGAddColumn("Im", "Имя", typeof(String));
            _dgvcontrol.dataGridViewGG.GGAddColumn("Otch", "Отчество", typeof(String));
            _dgvcontrol.dataGridViewGG.GGAddColumn("PostRef", "Должность", typeof(String));
            _dgvcontrol.dataGridViewGG.GGAddColumn("SectionRef", "Отдел", typeof(String));
            _dgvcontrol.dataGridViewGG.GGAddColumn("isAdmin", "Админ", typeof(Boolean));
            _dgvcontrol.dataGridViewGG.GGAddColumn("isDropped", "Удален", typeof(Boolean));
            _dgvcontrol.dataGridViewGG.GGAddColumn("isWindowsUser", "WindowsUser", typeof(Boolean));
            _dgvcontrol.dataGridViewGG.GGAddColumn("CreateDate", "Создан", typeof(String));
            _dgvcontrol.dataGridViewGG.GGAddColumn("UpdateDate", "Изменен", typeof(String));
        }

        private void LoadGridWorkplaces()
        {
            _dgvcontrol.dataGridViewGG.GGRegistryAssemblyName = _PluginAssembly + "_workplaces";
            _dbsql.SQLScript = "select * from GGPlatform.Workplaces";
            bs.DataSource = _dbsql.FillDataSet().Tables[0];
            _dgvcontrol.dataGridViewGG.Columns.Clear();
            _dgvcontrol.dataGridViewGG.DataSource = bs;

            _dgvcontrol.dataGridViewGG.GGAddColumn("OrderID", "№ п/п", typeof(Int32));
            _dgvcontrol.dataGridViewGG.GGAddColumn("Name", "Наименование рабочего места", typeof(String));
            _dgvcontrol.dataGridViewGG.GGAddColumn("Description", "Описание", typeof(String));
        }

        private void LoadGridAssemblys()
        {
            _dgvcontrol.dataGridViewGG.GGRegistryAssemblyName = _PluginAssembly + "_assemblys";
            _dbsql.SQLScript = "select * from GGPlatform.Assemblys";
            bs.DataSource = _dbsql.FillDataSet().Tables[0];
            _dgvcontrol.dataGridViewGG.Columns.Clear();
            _dgvcontrol.dataGridViewGG.DataSource = bs;
         
            _dgvcontrol.dataGridViewGG.GGAddColumn("Name", "Наименование интерфейса", typeof(String));
            _dgvcontrol.dataGridViewGG.GGAddColumn("AssemblyName", "Наименование сборки интерфейса", typeof(String));
            _dgvcontrol.dataGridViewGG.GGAddColumn("ShortCut", "Горячая клавиша", typeof(String));
        }

        private void LoadGridGroupReports()
        {
            _dgvcontrol.dataGridViewGG.GGRegistryAssemblyName = _PluginAssembly + "_groupreports";
            _dbsql.SQLScript = "select * from GGPlatform.ReportGroups";
            bs.DataSource = _dbsql.FillDataSet().Tables[0];
            _dgvcontrol.dataGridViewGG.Columns.Clear();
            _dgvcontrol.dataGridViewGG.DataSource = bs;

            _dgvcontrol.dataGridViewGG.GGAddColumn("OrderID", "№ п/п", typeof(Int32));
            _dgvcontrol.dataGridViewGG.GGAddColumn("Name", "Наименование группы", typeof(String));
        }

        private void LoadGridReports()
        {
            _dgvcontrol.dataGridViewGG.GGRegistryAssemblyName = _PluginAssembly + "_reports";
            _dbsql.SQLScript = "select * from GGPlatform.Reports";
            bs.DataSource = _dbsql.FillDataSet().Tables[0];
            _dgvcontrol.dataGridViewGG.Columns.Clear();
            _dgvcontrol.dataGridViewGG.DataSource = bs;
    
            _dgvcontrol.dataGridViewGG.GGAddColumn("Name", "Наименование отчета", typeof(String));
            _dgvcontrol.dataGridViewGG.GGAddColumn("AssemblyName", "Наименование сборки отчета", typeof(String));
        }

        private void tbNew_Click(object sender, EventArgs e)
        {
            frmElement fmElement = new frmElement(-1, null, _dbsql, _ObjectType);
            if (fmElement.ShowDialog() == DialogResult.OK)
                LoadGrid();
            fmElement.Dispose();
            fmElement = null;
        }

        private void tbProp_Click(object sender, EventArgs e)
        {
            dataGridViewGG_DoubleClick(null, null);
        }

        private void tbDel_Click(object sender, EventArgs e)
        {
            // DataRow dr = ((DataTable)(bs.DataSource)).Rows[_dgvcontrol.dataGridViewGG.CurrentRow.Index];           
            DataRow dr = ((DataRowView)_dgvcontrol.dataGridViewGG.CurrentRow.DataBoundItem).Row;
            Int32 _id = -1;
            _id = (int)dr["ID"];
            dr = null;

            if (_id > -1)
            {
                switch (_ObjectType)
                {
                    case 0: {  } break;
                    case 1:
                        {
                            _dbsql.SQLScript = "exec [GGPlatform].[usp_UsersManager] @TypeQuery = 5, @ObjectID = @objID";
                            _dbsql.AddParameter("@objID", EnumDBDataTypes.EDT_Integer, _id.ToString());
                            _dbsql.ExecuteScalar();
                        } break;
                    case 2:
                        {
                            _dbsql.SQLScript = "exec [GGPlatform].[usp_WorkplacesManager] @TypeQuery = 5, @ObjectID = @objID";
                            _dbsql.AddParameter("@objID", EnumDBDataTypes.EDT_Integer, _id.ToString());
                            _dbsql.ExecuteScalar();
                        } break;
                    case 3:
                        {
                            _dbsql.SQLScript = "exec [GGPlatform].[usp_AssemblysManager] @TypeQuery = 5, @ObjectID = @objID";
                            _dbsql.AddParameter("@objID", EnumDBDataTypes.EDT_Integer, _id.ToString());
                            _dbsql.ExecuteScalar();
                        } break;
                    case 4:
                        {
                            _dbsql.SQLScript = "exec [GGPlatform].[usp_ReportGroupsManager] @TypeQuery = 5, @ObjectID = @objID";
                            _dbsql.AddParameter("@objID", EnumDBDataTypes.EDT_Integer, _id.ToString());
                            _dbsql.ExecuteScalar();
                        } break;
                    case 5:
                        {
                            _dbsql.SQLScript = "exec [GGPlatform].[usp_ReportsManager] @TypeQuery = 5, @ObjectID = @objID";
                            _dbsql.AddParameter("@objID", EnumDBDataTypes.EDT_Integer, _id.ToString());
                            _dbsql.ExecuteScalar();
                        } break;
                }
            }
            LoadGrid();
        }

    }
}