using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Collections;
using GGPlatform.BuiltInApp;
using GGPlatform.DBServer;
using GGPlatform.QueryBuilder;
using GGPlatform.RFCManag;
using GGPlatform.ExcelManagers;
using System.Runtime.InteropServices;
using System.IO;
using GGPlatform.RegManager;

namespace ConfGrid
{
    public partial class frmConfGrid : Form, GGPlatform.BuiltInApp.IPlugin
    {
        private object _MainApp = null;
        private Form _MainForm = null;
        private int _PluginID;
        private int _WorkplaceID;
        private string _PluginAssembly, _PluginName, _SoftName = "";
        private DBSqlServer _dbsql = null;
        private DelegClosePlugin _ClosePlugin;
        private RFCManager _rfc = null;
        private BindingSource bsM = new BindingSource();
        private Lists _ActiveList = null;

        private BindingSource bsD = new BindingSource();

        public frmConfGrid() 
        {      
            InitializeComponent();
        }

        int IPlugin.plugPluginButtons
        {
            get
            {
                return (int)EnumPluginButtons.EPB_Refresh;
            }
        }
        ToolStrip IPlugin.plugToolStrip { get { return /*toolStrip1*/null; } }

        void IPlugin.plugInit(object MainApp, DBSqlServer inDBSqlServer, RFCManager inRFCManager, ref Lists inActiveLists)
        {
            _MainApp = MainApp;
            _MainForm = (Form)_MainApp;
            _dbsql = inDBSqlServer;
            _rfc = inRFCManager;
            _ActiveList = inActiveLists;

            GridLoadM();
            GridLoadD();

            this.MdiParent = _MainForm;
            this.Show();
            this.WindowState = FormWindowState.Maximized;
            this.Text = _PluginName;          
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
            _dgvcontrolM.dataGridViewGG.GGSaveGridRegistry();
            _dgvcontrolD.dataGridViewGG.GGSaveGridRegistry();
        }
        void IPlugin.appRefresh()
        {
            LoadDataM("select * from [GGPlatform].[Objects] order by ObjectName");
        }
        void IPlugin.appRefreshFilter()
        {
        }
        void IPlugin.appOption()
        {
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

        void IPlugin.appListAdd()
        { 
        }

        void IPlugin.appListDel()
        {
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            _ClosePlugin(_WorkplaceID, _PluginID);
        }

        private void Position_Changed(object sender, EventArgs e)
        {
            Int32 _idObject = -1;
            try
            {
                _idObject = (int)(((BindingSource)sender).Current as DataRowView)["ID"];

                rtbObjectScript.Text = Convert.ToString((((BindingSource)sender).Current as DataRowView)["ObjectExpression"]);
                rtbObjectSubScript.Text = Convert.ToString((((BindingSource)sender).Current as DataRowView)["ObjectExpressionSubQuery"]);

                LoadDataD(@"Select d.*, m.ObjectName 
                            from[GGPlatform].[ObjectsDescription] d
                                left join[GGPlatform].[Objects] m on m.id = d.ObjectsRefOrSubRef
                            where d.ObjectsRef = {0}
                            order by d.id", _idObject);                
            }
            catch
            {
                _dgvcontrolD.dataGridViewGG.DataSource = null;
            }
        }

        private void GridLoadM()
        {
            try
            {
                //при повторном открытии не сохраняет галочки!!!!!
                _dgvcontrolM.dataGridViewGG.Columns.Clear();
                _dgvcontrolM.dataGridViewGG.AllowUserToAddRows = false;
                _dgvcontrolM.dataGridViewGG.AllowUserToDeleteRows = false;
                _dgvcontrolM.dataGridViewGG.AllowUserToOrderColumns = true;
                _dgvcontrolM.dataGridViewGG.AutoGenerateColumns = false;
                _dgvcontrolM.dataGridViewGG.MultiSelect = false;
                _dgvcontrolM.dataGridViewGG.GGRegistryAssemblyName = _PluginAssembly + "_" + "Objects";
                _dgvcontrolM.dataGridViewGG.GGRegistrySoftName = _SoftName;
                _dgvcontrolM.dataGridViewGG.GGObjectName = "Objects";

                //  _dgvcontrolM.dataGridViewGG.DoubleClick += tbEditPhoneNum_Click;
                bsM.PositionChanged += new EventHandler(Position_Changed);  //отслеживает переход между строками
                _dgvcontrolM.DBSqlServerGG = _dbsql;

                LoadDataM("select * from[GGPlatform].[Objects] order by ObjectName");

                _dgvcontrolM.dataGridViewGG.ReadOnly = true;

                _dgvcontrolM.dataGridViewGG.GGAddColumn("ID", "ИД", typeof(Int32)).Visible = false; 
                _dgvcontrolM.dataGridViewGG.GGAddColumn("ObjectName", "Объект", typeof(String));
                _dgvcontrolM.dataGridViewGG.GGAddColumn("ObjectCaption", "Наименование объекта", typeof(String));
                _dgvcontrolM.dataGridViewGG.GGAddColumn("ObjectHeight", "Высота", typeof(Int32));
                _dgvcontrolM.dataGridViewGG.GGAddColumn("ObjectWidht", "Ширина", typeof(Int32));

                _dgvcontrolM.RefreshFilterFieldsGG();
                _dgvcontrolM.dataGridViewGG.GGLoadGridRegistry();
            }
            catch (Exception exx)
            {
                MessageBox.Show(exx.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GridLoadD()
        {
            try
            {
                //при повторном открытии не сохраняет галочки!!!!!
                _dgvcontrolD.dataGridViewGG.Columns.Clear();
                _dgvcontrolD.dataGridViewGG.AllowUserToAddRows = false;
                _dgvcontrolD.dataGridViewGG.AllowUserToDeleteRows = false;
                _dgvcontrolD.dataGridViewGG.AllowUserToOrderColumns = true;
                _dgvcontrolD.dataGridViewGG.AutoGenerateColumns = false;
                _dgvcontrolD.dataGridViewGG.MultiSelect = false;
                _dgvcontrolD.dataGridViewGG.GGRegistryAssemblyName = _PluginAssembly + "_" + "ObjectsDescription";
                _dgvcontrolD.dataGridViewGG.GGRegistrySoftName = _SoftName;
                _dgvcontrolD.dataGridViewGG.GGObjectName = "ObjectsDescription";

                _dgvcontrolD.DBSqlServerGG = _dbsql;

                _dgvcontrolD.dataGridViewGG.ReadOnly = true;

                _dgvcontrolD.dataGridViewGG.GGAddColumn("ID", "ИД", typeof(Int32)).Visible = false;
                _dgvcontrolD.dataGridViewGG.GGAddColumn("FieldName", "Поле таблицы", typeof(String));
                _dgvcontrolD.dataGridViewGG.GGAddColumn("FieldAlias", "Псевдоним поля", typeof(String));
                _dgvcontrolD.dataGridViewGG.GGAddColumn("FieldCaption", "Наименование поля", typeof(String));
                _dgvcontrolD.dataGridViewGG.GGAddColumn("FieldType", "Тип", typeof(String)).Width = 30;
                _dgvcontrolD.dataGridViewGG.GGAddColumn("FieldVisible", "Вид.", typeof(Boolean)).Width = 30;
                _dgvcontrolD.dataGridViewGG.GGAddColumn("ObjectName", "Объект-подзапрос", typeof(String));

                _dgvcontrolD.RefreshFilterFieldsGG();
                _dgvcontrolD.dataGridViewGG.GGLoadGridRegistry();
            }
            catch (Exception exx)
            {
                MessageBox.Show(exx.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDataM(string sql)
        {
            try
            {
                if (sql == "")
                return;

                _MainForm.Cursor = System.Windows.Forms.Cursors.WaitCursor;

                _dbsql.SQLScript = sql;
                DataTable _dt = null;
                _dt = _dbsql.FillDataSet().Tables[0];
                bsM.DataSource = _dt;

                _dgvcontrolM.dataGridViewGG.DataSource = bsM;
            }
            catch (Exception exx)
            {
                _MainForm.Cursor = System.Windows.Forms.Cursors.Default;
                MessageBox.Show(exx.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _MainForm.Cursor = System.Windows.Forms.Cursors.Default;
            }
        }

        private void LoadDataD(string sql, int inObjectID)
        {
            try
            {
                if (sql == "")
                return;

                _MainForm.Cursor = System.Windows.Forms.Cursors.WaitCursor;

                _dbsql.SQLScript = String.Format(sql, inObjectID);

                bsD.DataSource = _dbsql.FillDataSet().Tables[0]; ;
                _dgvcontrolD.dataGridViewGG.DataSource = bsD;
            }
            catch (Exception exx)
            {
                _MainForm.Cursor = System.Windows.Forms.Cursors.Default;
                MessageBox.Show(exx.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _MainForm.Cursor = System.Windows.Forms.Cursors.Default;
            }
        }

        private void groupBox1_Resize(object sender, EventArgs e)
        {
            int t = groupBox1.Height / 5;
            splitContainer3.SplitterDistance = t * 3;
        }

        private void panel2_Resize(object sender, EventArgs e)
        {
            int t = panel2.Height / 5;
            splitContainer2.SplitterDistance = t * 3;
        }

 
        private void panel1_Resize(object sender, EventArgs e)
        {
            int r = panel1.Width / 7;
            splitContainer1.SplitterDistance = r * 3;
        }
       

    }
}