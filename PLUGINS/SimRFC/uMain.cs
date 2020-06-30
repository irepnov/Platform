using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using GGPlatform.BuiltInApp;
using GGPlatform.DBServer;
using GGPlatform.RFCManag;

namespace SimRFC
{
    public partial class frmMain : Form, GGPlatform.BuiltInApp.IPlugin
    {
        public frmMain()
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




        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            _ClosePlugin(_WorkplaceID, _PluginID);
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            treeView.ExpandAll();
            _dgvcontrol.dataGridViewGG.DoubleClick += new EventHandler(dataGridViewGG_DoubleClick);
            _dgvcontrol.dataGridViewGG.AllowUserToAddRows = false;
            _dgvcontrol.dataGridViewGG.AllowUserToDeleteRows = false;
            _dgvcontrol.dataGridViewGG.AllowUserToOrderColumns = true;
            _dgvcontrol.dataGridViewGG.AutoGenerateColumns = false;
            _dgvcontrol.dataGridViewGG.MultiSelect = false;
            _dgvcontrol.dataGridViewGG.ReadOnly = true;

            tbNew.Enabled = tbProp.Enabled = false;
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            _ObjectType = -1;
            if ((treeView.SelectedNode.Index >= 0) && (treeView.SelectedNode.Parent != null))
            {
                _dgvcontrol.dataGridViewGG.GGSaveGridRegistry(); //сохраняю текущие настройки грида (прошлого)
                _ObjectType = treeView.SelectedNode.Index + 1;
                LoadGrid();

                tbNew.Enabled = tbProp.Enabled = true;
            } else
            {
                _dgvcontrol.dataGridViewGG.Columns.Clear();
                _dgvcontrol.dataGridViewGG.DataSource = null;

                tbNew.Enabled = tbProp.Enabled = false;
            }
        }

        private void LoadGrid()
        {
            _dgvcontrol.dataGridViewGG.ReadOnly = true;
            // tbDel.Enabled = true;
            _dgvcontrol.dataGridViewGG.CellEndEdit -= null;
            switch (_ObjectType)
                {
                    case 1: { LoadGridTovar(); } break;
                    case 2: { LoadGridProvider(); } break;
                    case 3: { LoadGridSklad(); } break;
                    case 4: { LoadGridOtvLico(); } break;
                    case 5: { LoadGridTypeSklad(); } break;
                case 6: { LoadGridPostav(); } break;
                default: break; 
                } 
            //загружу настройки грида
            _dgvcontrol.RefreshFilterFieldsGG();
            _dgvcontrol.dataGridViewGG.GGLoadGridRegistry();
        }

        private void dataGridViewGG_DoubleClick(object sender, EventArgs e)
        {
            if (_ObjectType == -1)
                return;
            //DataRow dr = ((DataTable)(bs.DataSource)).Rows[_dgvcontrol.dataGridViewGG.CurrentRow.Index];
            DataRow dr = ((DataRowView)_dgvcontrol.dataGridViewGG.CurrentRow.DataBoundItem).Row;

            frmElement fmElement = new frmElement((int)dr["ID"], dr, _dbsql, _ObjectType);            
            if (fmElement.ShowDialog() == DialogResult.OK)
                LoadGrid();
            fmElement.Dispose();
            fmElement = null;
        }
        private void LoadGridTovar()//1
        {
            _dgvcontrol.dataGridViewGG.GGRegistryAssemblyName = _PluginAssembly + "_tovar";
            _dbsql.SQLScript = "exec uchReference @IDOperation = " + _ObjectType.ToString();
            bs.DataSource = _dbsql.FillDataSet().Tables[0];
            _dgvcontrol.dataGridViewGG.Columns.Clear();
            _dgvcontrol.dataGridViewGG.DataSource = bs;

            _dgvcontrol.dataGridViewGG.GGAddColumn("Name", "Наименование", typeof(String));
        }
        private void LoadGridProvider()//2
        {
            _dgvcontrol.dataGridViewGG.GGRegistryAssemblyName = _PluginAssembly + "_provider";
            _dbsql.SQLScript = "exec uchReference @IDOperation = " + _ObjectType.ToString();
            bs.DataSource = _dbsql.FillDataSet().Tables[0];
            _dgvcontrol.dataGridViewGG.Columns.Clear();
            _dgvcontrol.dataGridViewGG.DataSource = bs;

            _dgvcontrol.dataGridViewGG.GGAddColumn("Name", "Наименование", typeof(String));
            _dgvcontrol.dataGridViewGG.GGAddColumn("Comment", "Описание", typeof(String));
        }
        private void LoadGridSklad()//3
        {
            _dgvcontrol.dataGridViewGG.GGRegistryAssemblyName = _PluginAssembly + "_sklad";
            _dbsql.SQLScript = "exec uchReference @IDOperation = " + _ObjectType.ToString();
            bs.DataSource = _dbsql.FillDataSet().Tables[0];
            _dgvcontrol.dataGridViewGG.Columns.Clear();
            _dgvcontrol.dataGridViewGG.DataSource = bs;

            _dgvcontrol.dataGridViewGG.GGAddColumn("Name", "Наименование", typeof(String));
            _dgvcontrol.dataGridViewGG.GGAddColumn("TypeR", "Тип склада", typeof(String));
            _dgvcontrol.dataGridViewGG.GGAddColumn("CodePoint", "Код точки", typeof(String));
            _dgvcontrol.dataGridViewGG.GGAddColumn("isMainRepository", "Основной", typeof(bool));
            _dgvcontrol.dataGridViewGG.GGAddColumn("FIO", "Ответственное лицо", typeof(String));
            _dgvcontrol.dataGridViewGG.GGAddColumn("Comment", "Описание", typeof(String));
        }       
        private void LoadGridOtvLico()//4
        {
            _dgvcontrol.dataGridViewGG.GGRegistryAssemblyName = _PluginAssembly + "_otvlico";
            _dbsql.SQLScript = "exec uchReference @IDOperation = " + _ObjectType.ToString();
            bs.DataSource = _dbsql.FillDataSet().Tables[0];
            _dgvcontrol.dataGridViewGG.Columns.Clear();
            _dgvcontrol.dataGridViewGG.DataSource = bs;

            _dgvcontrol.dataGridViewGG.GGAddColumn("RepositoryName", "Склад", typeof(String));
            _dgvcontrol.dataGridViewGG.GGAddColumn("Fam", "Фамилия", typeof(String));
            _dgvcontrol.dataGridViewGG.GGAddColumn("Im", "Имя", typeof(String));
            _dgvcontrol.dataGridViewGG.GGAddColumn("Otch", "Отчество", typeof(String));

            _dgvcontrol.dataGridViewGG.GGAddColumn("Phone_1", "Телефон 1", typeof(String));
            _dgvcontrol.dataGridViewGG.GGAddColumn("Phone_2", "Телефон 2", typeof(String));
            _dgvcontrol.dataGridViewGG.GGAddColumn("Phone_3", "Телефон 3", typeof(String));

            _dgvcontrol.dataGridViewGG.GGAddColumn("DocSeria", "Серия УДЛ", typeof(String));
            _dgvcontrol.dataGridViewGG.GGAddColumn("DocNumber", "Номер УДЛ", typeof(String));

            _dgvcontrol.dataGridViewGG.GGAddColumn("AdresPR", "Адрес проживания", typeof(String));
            _dgvcontrol.dataGridViewGG.GGAddColumn("AdresREG", "Адрес регистрации", typeof(String));

            _dgvcontrol.dataGridViewGG.GGAddColumn("CreateDate", "Создан", typeof(DateTime));
            _dgvcontrol.dataGridViewGG.GGAddColumn("CreateUser", "Создал", typeof(String));
            _dgvcontrol.dataGridViewGG.GGAddColumn("UpdateDate", "Изменен", typeof(DateTime));
            _dgvcontrol.dataGridViewGG.GGAddColumn("UpdateUser", "Изменил", typeof(String));
        }      
        private void LoadGridTypeSklad()//5
        {
            _dgvcontrol.dataGridViewGG.GGRegistryAssemblyName = _PluginAssembly + "_typesklad";
            _dbsql.SQLScript = "exec uchReference @IDOperation = " + _ObjectType.ToString();
            bs.DataSource = _dbsql.FillDataSet().Tables[0];
            _dgvcontrol.dataGridViewGG.Columns.Clear();
            _dgvcontrol.dataGridViewGG.DataSource = bs;

            _dgvcontrol.dataGridViewGG.GGAddColumn("Name", "Наименование", typeof(String));
        }

        private void LoadGridPostav()//6
        {
            _dgvcontrol.dataGridViewGG.ReadOnly = false;
                _dgvcontrol.dataGridViewGG.GGRegistryAssemblyName = _PluginAssembly + "postav";
            _dbsql.SQLScript = "exec uchReference @IDOperation = " + _ObjectType.ToString();
            bs.DataSource = _dbsql.FillDataSet().Tables[0];
            _dgvcontrol.dataGridViewGG.Columns.Clear();
            _dgvcontrol.dataGridViewGG.DataSource = bs;

            _dgvcontrol.dataGridViewGG.GGAddColumn("Name", "Наименование", typeof(String)).ReadOnly = true; 
            _dgvcontrol.dataGridViewGG.GGAddColumn("isActive", "Актуальный", typeof(bool)).ReadOnly = true;
            _dgvcontrol.dataGridViewGG.GGAddColumn("DateLastPrice", "Дата последнего прайса", typeof(DateTime)).ReadOnly = true;

            _dgvcontrol.dataGridViewGG.GGAddColumn("PriorityID", "Приоритет", typeof(Int32)).ReadOnly = false;

            _dgvcontrol.dataGridViewGG.CellEndEdit += (s, e) =>
            {
                 string _newval = "";
                 string _idrow = "";

                 DataRow dr = ((DataRowView)_dgvcontrol.dataGridViewGG.CurrentRow.DataBoundItem).Row;
                
                 _newval = _dgvcontrol.dataGridViewGG[e.ColumnIndex, e.RowIndex].Value.ToString();
                 _idrow = dr["ID"].ToString();

                 dr = null;

                 Int32 _val;
                 Int32 _id;

                 if (Int32.TryParse(_newval, out _val) && Int32.TryParse(_idrow, out _id))
                 {
                     _dbsql.SQLScript = "update rfcPostav set PriorityID = @newv where id = @id";
                     _dbsql.AddParameter("@newv", EnumDBDataTypes.EDT_Integer, _val.ToString());
                     _dbsql.AddParameter("@id", EnumDBDataTypes.EDT_Integer, _id.ToString());
                     _dbsql.ExecuteNonQuery();
                 }
            };
        }


        private void tbNew_Click(object sender, EventArgs e)
        {
            if (_ObjectType == -1)
                return;
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

        

    }
}