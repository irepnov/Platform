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

namespace SimPrihodT
{
    public partial class frmSimPrihodT : Form, GGPlatform.BuiltInApp.IPlugin
    {

        public class Field
        {
            private string _FieldName = "";
            private string _FieldCaption = "";
            private string _FieldType = "";
            private bool _FieldVision = true;

            public Field(string inFieldName, string inFieldCaption, string inFieldType, bool inFieldVision)
            {
                _FieldName = inFieldName;
                _FieldCaption = inFieldCaption;
                _FieldType = inFieldType;
                _FieldVision = inFieldVision;
            }

            public string FieldName { get { return _FieldName; } }
            public string FieldCaption { get { return _FieldCaption; } }
            public string FieldType { get { return _FieldType; } }
            public bool FieldVision { get { return _FieldVision; } }
        }

        private object _MainApp = null;
        private Form _MainForm = null;
        private int _PluginID;
        private int _WorkplaceID;
        private string _PluginAssembly, _PluginName, _SoftName = "";
        private DBSqlServer _dbsql = null;
        private DelegClosePlugin _ClosePlugin;
        private RFCManager _rfc = null;
        private QueryBuilder _QueryBuilderM = null;
        private string ObjectsSQLM = "";
        private string ObjectsCaptionM = "";
        private int ObjectsIDM;
        private ArrayList ObjectsFieldsM = null;
        private BindingSource bsM = new BindingSource();
        private Lists _ActiveList = null;

        private string ObjectsSQLD = "";
        private string ObjectsCaptionD = "";
        private int ObjectsIDD;
        private ArrayList ObjectsFieldsD = null;
        private BindingSource bsD = new BindingSource();

        public frmSimPrihodT() 
        {      
            InitializeComponent();
        }

        int IPlugin.plugPluginButtons
        {
            get
            {
                return (int)EnumPluginButtons.EPB_Refresh +
                       (int)EnumPluginButtons.EPB_RefreshFilter;
            }
        }
        ToolStrip IPlugin.plugToolStrip { get { return toolStrip1; } }

        void IPlugin.plugInit(object MainApp, DBSqlServer inDBSqlServer, RFCManager inRFCManager, ref Lists inActiveLists)
        {
            _MainApp = MainApp;
            _MainForm = (Form)_MainApp;
            _dbsql = inDBSqlServer;
            _rfc = inRFCManager;
            _ActiveList = inActiveLists;

            ReadObjectM();
            ReadObjectD();
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
            LoadDataM(ObjectsSQLM);
        }
        void IPlugin.appRefreshFilter()
        {
            /*if (_dgvcontrolM.dataGridViewGG.Focused)
            {*/
                if (_QueryBuilderM == null)
                    _QueryBuilderM = new QueryBuilder(ObjectsIDM, (IWin32Window)_MainApp, _dbsql, TypeReturnScript.trsScript);
                string sqlstr = _QueryBuilderM.GetQuery();
                if (sqlstr != null)
                    if (ObjectsSQLM.Replace(" ", "") != sqlstr.Replace(" ", ""))
                    {
                        ObjectsSQLM = sqlstr;
                        LoadDataM(ObjectsSQLM);
                    }
           /* }*/

            //if (_dgvcontrolD.dataGridViewGG.Focused)
            //{
            //    if (_QueryBuilderD == null)
            //        _QueryBuilderD = new QueryBuilder(ObjectsIDD, (IWin32Window)_MainApp, _dbsql, TypeReturnScript.trsScript);
            //    string sqlstr = _QueryBuilderD.GetQuery();
            //    MessageBox.Show(sqlstr);

            //    if (sqlstr != null)
            //        if (ObjectsSQLD.Replace(" ", "") != sqlstr.Replace(" ", ""))
            //        {
            //            ObjectsSQLD = sqlstr;
            //            LoadDataD(ObjectsSQLD, -1);
            //        }
            //}            
        }
        void IPlugin.appOption() { }
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
            Int32 _id = -1;
            try
            {
                _id = (int)(((BindingSource)sender).Current as DataRowView)["ID"];
                LoadDataD(ObjectsSQLD, _id);                
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
                _dgvcontrolM.dataGridViewGG.AllowUserToAddRows = false;
                _dgvcontrolM.dataGridViewGG.AllowUserToDeleteRows = false;
                _dgvcontrolM.dataGridViewGG.AllowUserToOrderColumns = true;
                _dgvcontrolM.dataGridViewGG.AutoGenerateColumns = false;
                _dgvcontrolM.dataGridViewGG.MultiSelect = false;
                _dgvcontrolM.dataGridViewGG.GGRegistryAssemblyName = _PluginAssembly + "_" + "BillIncome";
                _dgvcontrolM.dataGridViewGG.Columns.Clear();
                _dgvcontrolM.dataGridViewGG.DoubleClick += DataGridViewGG_DoubleClick;
                bsM.PositionChanged += new EventHandler(Position_Changed);  //отслеживает переход между строками
                _dgvcontrolM.DBSqlServerGG = _dbsql;

                LoadDataM(ObjectsSQLM);

                Field _field = null;
                string tmpFieldName = "";
                string tmpFieldCaption = "";
                string tmpFieldType = "";
                bool tmpFieldVisible = true;
                IEnumerator Enumerator = ObjectsFieldsM.GetEnumerator();
                while (Enumerator.MoveNext())
                {
                    _field = (Field)Enumerator.Current;
                    tmpFieldName = _field.FieldName;
                    tmpFieldCaption = _field.FieldCaption;
                    tmpFieldType = _field.FieldType;
                    tmpFieldVisible = _field.FieldVision;

                    if ((!tmpFieldVisible) && (!_dbsql.InfoAboutConnection.UserIsAdmin)) continue;  //если он невидимый в настройкахи ты не Админ, то проскакиваю его

                    DataGridViewColumn colu = null;

                    if (tmpFieldType == "L")
                    {
                        colu = _dgvcontrolM.dataGridViewGG.GGAddColumn(tmpFieldName, tmpFieldCaption, typeof(Boolean));
                    }
                    else
                        if (tmpFieldType == "N")
                        {
                            colu = _dgvcontrolM.dataGridViewGG.GGAddColumn(tmpFieldName, tmpFieldCaption, typeof(Int32));
                        }
                        else
                            if (tmpFieldType == "D")
                            {
                                colu = _dgvcontrolM.dataGridViewGG.GGAddColumn(tmpFieldName, tmpFieldCaption, typeof(DateTime));
                            }
                            else
                                {
                                    colu = _dgvcontrolM.dataGridViewGG.GGAddColumn(tmpFieldName, tmpFieldCaption, typeof(String));
                                }

                    if (tmpFieldName.ToUpper() == "ID")
                        tmpFieldVisible = false; //поле ИД сразу невидимо

                    colu.Visible = tmpFieldVisible;
                    colu.ReadOnly = true;
                }
                Enumerator = null;
                _field = null;

                _dgvcontrolM.RefreshFilterFieldsGG();
                _dgvcontrolM.dataGridViewGG.GGLoadGridRegistry();
            }
            catch (Exception exx)
            {
                MessageBox.Show(exx.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DataGridViewGG_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                DataRow dr = ((DataRowView)_dgvcontrolM.dataGridViewGG.CurrentRow.DataBoundItem).Row;

                frmBillIncome fmIncome = new frmBillIncome((int)dr["ID"], dr, _dbsql);
                if (fmIncome.ShowDialog() == DialogResult.OK)
                    LoadDataM(ObjectsSQLM);
                fmIncome.Dispose();
                fmIncome = null;
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
                _dgvcontrolD.dataGridViewGG.AllowUserToAddRows = false;
                _dgvcontrolD.dataGridViewGG.AllowUserToDeleteRows = false;
                _dgvcontrolD.dataGridViewGG.AllowUserToOrderColumns = true;
                _dgvcontrolD.dataGridViewGG.AutoGenerateColumns = false;
                _dgvcontrolD.dataGridViewGG.MultiSelect = false;
                _dgvcontrolD.dataGridViewGG.GGRegistryAssemblyName = _PluginAssembly + "_" + "uchProductBillIncome";
                _dgvcontrolD.dataGridViewGG.Columns.Clear();

                Field _field = null;
                string tmpFieldName = "";
                string tmpFieldCaption = "";
                string tmpFieldType = "";
                bool tmpFieldVisible = true;
                IEnumerator Enumerator = ObjectsFieldsD.GetEnumerator();
                while (Enumerator.MoveNext())
                {
                    _field = (Field)Enumerator.Current;
                    tmpFieldName = _field.FieldName;
                    tmpFieldCaption = _field.FieldCaption;
                    tmpFieldType = _field.FieldType;
                    tmpFieldVisible = _field.FieldVision;

                    if ((!tmpFieldVisible) && (!_dbsql.InfoAboutConnection.UserIsAdmin)) continue;  //если он невидимый в настройкахи ты не Админ, то проскакиваю его

                    DataGridViewColumn colu = null;

                    if (tmpFieldType == "L")
                    {
                        colu = _dgvcontrolD.dataGridViewGG.GGAddColumn(tmpFieldName, tmpFieldCaption, typeof(Boolean));
                    }
                    else
                        if (tmpFieldType == "N")
                        {
                            colu = _dgvcontrolD.dataGridViewGG.GGAddColumn(tmpFieldName, tmpFieldCaption, typeof(Int32));
                        }
                        else
                            if (tmpFieldType == "D")
                            {
                                colu = _dgvcontrolD.dataGridViewGG.GGAddColumn(tmpFieldName, tmpFieldCaption, typeof(DateTime));
                            }
                            else
                            {
                                colu = _dgvcontrolD.dataGridViewGG.GGAddColumn(tmpFieldName, tmpFieldCaption, typeof(String));
                            }

                    if (tmpFieldName.ToUpper() == "ID")
                        tmpFieldVisible = false; //поле ИД сразу невидимо

                    colu.Visible = tmpFieldVisible;
                    colu.ReadOnly = true;
                }
                Enumerator = null;
                _field = null;

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
                _dbsql.SQLScript = sql;
                DataTable _dt = null;
                _dt = _dbsql.FillDataSet().Tables[0];
                bsM.DataSource = _dt;

                _dgvcontrolM.dataGridViewGG.DataSource = bsM;
            }
            catch (Exception exx)
            {
                MessageBox.Show(exx.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDataD(string sql, int inBillID)
        {
            try
            {
                if (sql == "")
                return;

                _dbsql.SQLScript = sql;
                if (inBillID > -1)
                    _dbsql.SQLScript = sql + " where b.BillIncomeRef = " + inBillID.ToString();
                bsD.DataSource = _dbsql.FillDataSet().Tables[0]; ;
                _dgvcontrolD.dataGridViewGG.DataSource = bsD;
            }
            catch (Exception exx)
            {
                MessageBox.Show(exx.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ReadObjectM()
        {
            _dgvcontrolM.dataGridViewGG.GGRegistrySoftName = _SoftName;
            try
            {
                _dbsql.SQLScript = "select ID, ObjectCaption, ObjectExpression from [GGPlatform].[Objects] where Upper(ObjectName) = @obj";
                _dbsql.AddParameter("@obj", EnumDBDataTypes.EDT_String, "BillIncome".ToUpper());
                SqlDataReader Objects = _dbsql.ExecuteReader();
                while (Objects.Read())
                {
                    ObjectsIDM = (int)Objects["ID"];
                    ObjectsCaptionM = Objects["ObjectCaption"].ToString();
                    ObjectsSQLM = Objects["ObjectExpression"].ToString();
                }
                Objects.Close();

                _dbsql.SQLScript = "select ID, isnull(FieldAlias, FieldName) as FieldName, FieldCaption, FieldType, FieldVisible from GGPlatform.ObjectsDescription where ObjectsRef = @obj and FieldType <> 'Q'";
                _dbsql.AddParameter("@obj", EnumDBDataTypes.EDT_Integer, ObjectsIDM.ToString());
                SqlDataReader ObjectsDescription = _dbsql.ExecuteReader();
                ObjectsFieldsM = new ArrayList();
                Field tmpfield = null;
                while (ObjectsDescription.Read())
                {
                    tmpfield = new Field(ObjectsDescription["FieldName"].ToString(),
                                         ObjectsDescription["FieldCaption"].ToString(),
                                         ObjectsDescription["FieldType"].ToString(),
                                         (bool)ObjectsDescription["FieldVisible"]);
                    ObjectsFieldsM.Add(tmpfield);
                    tmpfield = null;
                }
                ObjectsDescription.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка настройки объекта BillIncome /n " + ex.Message,
                                "Ошибка [модуль SimPrihodT, класс SimPrihodT, метод ReadObjectM]",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }            
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                frmBillIncome fmBillIncome = new frmBillIncome(-1, null, _dbsql);
                if (fmBillIncome.ShowDialog() == DialogResult.OK)
                    LoadDataM(ObjectsSQLM);
                fmBillIncome.Dispose();
                fmBillIncome = null;
            }
            catch (Exception exx)
            {
                MessageBox.Show(exx.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            DataGridViewGG_DoubleClick(null, null);
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Удалить выбранную накладную?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;

                DataRow dr = ((DataRowView)_dgvcontrolM.dataGridViewGG.CurrentRow.DataBoundItem).Row;
                _dbsql.SQLScript = "exec uchIncomeBill @IDOperation = 4, @ID = @idd";
                _dbsql.AddParameter("@idd", EnumDBDataTypes.EDT_Integer, ((int)dr["ID"]).ToString());
                _dbsql.ExecuteNonQuery();
                dr = null;

                LoadDataM(ObjectsSQLM);
            }
            catch (Exception exx)
            {
                MessageBox.Show(exx.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {


        }

        private void ReadObjectD()
        {
            _dgvcontrolD.dataGridViewGG.GGRegistrySoftName = _SoftName;

            try
            {
                _dbsql.SQLScript = "select ID, ObjectCaption, ObjectExpression from [GGPlatform].[Objects] where Upper(ObjectName) = @obj";
                _dbsql.AddParameter("@obj", EnumDBDataTypes.EDT_String, "uchProductBillIncome".ToUpper());
                SqlDataReader Objects = _dbsql.ExecuteReader();
                while (Objects.Read())
                {
                    ObjectsIDD = (int)Objects["ID"];
                    ObjectsCaptionD = Objects["ObjectCaption"].ToString();
                    ObjectsSQLD = Objects["ObjectExpression"].ToString();
                }
                Objects.Close();

                _dbsql.SQLScript = "select ID, isnull(FieldAlias, FieldName) as FieldName, FieldCaption, FieldType, FieldVisible from GGPlatform.ObjectsDescription where ObjectsRef = @obj and FieldType <> 'Q'";
                _dbsql.AddParameter("@obj", EnumDBDataTypes.EDT_Integer, ObjectsIDD.ToString());
                SqlDataReader ObjectsDescription = _dbsql.ExecuteReader();
                ObjectsFieldsD = new ArrayList();
                Field tmpfield = null;
                while (ObjectsDescription.Read())
                {
                    tmpfield = new Field(ObjectsDescription["FieldName"].ToString(),
                                         ObjectsDescription["FieldCaption"].ToString(),
                                         ObjectsDescription["FieldType"].ToString(),
                                         (bool)ObjectsDescription["FieldVisible"]);
                    ObjectsFieldsD.Add(tmpfield);
                    tmpfield = null;
                }
                ObjectsDescription.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка настройки объекта uchProductBillIncome /n " + ex.Message,
                                "Ошибка [модуль SimPrihodT, класс SimPrihodT, метод ReadObjectD]",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void panel1_Resize(object sender, EventArgs e)
        {
            int r = panel1.Height / 3;
            panel3.Height = r * 2;
            panel2.Height = r;
        }
       

    }
}