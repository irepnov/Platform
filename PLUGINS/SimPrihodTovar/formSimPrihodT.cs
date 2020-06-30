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
    public partial class formSimPrihodT : Form, GGPlatform.BuiltInApp.IPlugin
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


        private QueryBuilder _QueryBuilderD = null;
        private string ObjectsSQLD = "";
        private string ObjectsCaptionD = "";
        private int ObjectsIDD;
        private ArrayList ObjectsFieldsD = null;
        private BindingSource bsD = new BindingSource();

        public formPlugin3() 
        {      
            InitializeComponent();
        }

        int IPlugin.plugPluginButtons
        {
            get
            {
                return (int)EnumPluginButtons.EPB_Refresh +
                       (int)EnumPluginButtons.EPB_RefreshFilter +
                       (int)EnumPluginButtons.EPB_ListShow;
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

          //  _rfc.LoadRFC("rfcOKOGU");
            _rfc.LoadRFC("rfcStreetType");
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
            if (_dgvcontrolM.dataGridViewGG.Focused)
            {
                if (_QueryBuilderM == null)
                    _QueryBuilderM = new QueryBuilder(ObjectsIDM, (IWin32Window)_MainApp, _dbsql, TypeReturnScript.trsScript);
                string sqlstr = _QueryBuilderM.GetQuery();
                if (sqlstr != null)
                    if (ObjectsSQLM.Replace(" ", "") != sqlstr.Replace(" ", ""))
                    {
                        ObjectsSQLM = sqlstr;
                        LoadDataM(ObjectsSQLM);
                    }
            }

            if (_dgvcontrolD.dataGridViewGG.Focused)
            {
                if (_QueryBuilderD == null)
                    _QueryBuilderD = new QueryBuilder(ObjectsIDD, (IWin32Window)_MainApp, _dbsql, TypeReturnScript.trsScript);
                string sqlstr = _QueryBuilderD.GetQuery();
                MessageBox.Show(sqlstr);

                if (sqlstr != null)
                    if (ObjectsSQLD.Replace(" ", "") != sqlstr.Replace(" ", ""))
                    {
                        ObjectsSQLD = sqlstr;
                        LoadDataD(ObjectsSQLD, -1);
                    }
            }            
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
            string sel = _dgvcontrolM.dataGridViewGG.GGGetFieldValuesCheckedRows("code");
            if (sel != "")
                _ActiveList.AddList(_PluginAssembly, "LPU_SELECT_USER", "Выбранные пользователем медицинские организации", "(" + sel + ")");  //может быть пусто
            _ActiveList.AddList(_PluginAssembly, "LPU_ACTIVE_URID", "Действующие медицинские юридические организации", "(select code from rfcLPU where DateEnd is null and ParentCode is null)");
            _ActiveList.AddList(_PluginAssembly, "LPU_NOTACTIVE_URID", "Не действующие медицинские юридические организации", "(select code from rfcLPU where DateEnd is not null and ParentCode is null)");
            _ActiveList.AddList(_PluginAssembly, "LPU_ACTIVE", "Действующие медицинские юрид.(стр.) организации", "(select code from rfcLPU where DateEnd is null)");
            _ActiveList.AddList(_PluginAssembly, "LPU_NOTACTIVE", "Не действующие медицинские юрид.(стр.) организации", "(select code from rfcLPU where DateEnd is not null)");

            if (_dgvcontrolD.dataGridViewGG.RowCount > 0)
                _ActiveList.AddList(_PluginAssembly, "LPU_SELECT_LICENSE", "Отобранные пользователем лицензии", "(select lpu.code " + ObjectsSQLD.Substring(ObjectsSQLD.IndexOf("from")) + ")");
        }

        void IPlugin.appListDel()
        {
            _ActiveList.DelList(_PluginAssembly, "LPU_SELECT_USER");
        }


        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            _ClosePlugin(_WorkplaceID, _PluginID);
        }

        private void dataGridViewGGM_SelectionChanged(object sender, EventArgs e)
        {
            MessageBox.Show("dd");
        }

        private void Position_Changed(object sender, EventArgs e)
        {
            int LPUID = -1;

            try
            {
                LPUID = (int)(((BindingSource)sender).Current as DataRowView)["ID"];
                LoadDataD(ObjectsSQLD, LPUID);
            }
            catch
            { 
            
            }
        }

        private void GridLoadM()
        {
            //при повторном открытии не сохраняет галочки!!!!!
            _dgvcontrolM.dataGridViewGG.AllowUserToAddRows = false;
            _dgvcontrolM.dataGridViewGG.AllowUserToDeleteRows = false;
            _dgvcontrolM.dataGridViewGG.AllowUserToOrderColumns = true;
            _dgvcontrolM.dataGridViewGG.AutoGenerateColumns = false;
            _dgvcontrolM.dataGridViewGG.MultiSelect = false;
            _dgvcontrolM.dataGridViewGG.GGRegistryAssemblyName = _PluginAssembly + "_" + "rfcLPU";
            _dgvcontrolM.dataGridViewGG.Columns.Clear();
            bsM.PositionChanged += new EventHandler(Position_Changed);  //отслеживает переход между строками

            _dgvcontrolM.DBSqlServerGG = _dbsql;
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("LPUStatusColors", "rfcLPUStatus");
            dict.Add("OKOPFColors", "rfcOKOPF");
            _dgvcontrolM.AddColorColumnsGG(dict);

            LoadDataM(ObjectsSQLM);

            _dgvcontrolM.dataGridViewGG.GGAddColumn("IsCheckedField", "check", typeof(Boolean));
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

          //  lbRecords.Text = _dt.Rows.Count.ToString() + " записей";
        }

        private void GridLoadD()
        {
            //при повторном открытии не сохраняет галочки!!!!!
            _dgvcontrolD.dataGridViewGG.AllowUserToAddRows = false;
            _dgvcontrolD.dataGridViewGG.AllowUserToDeleteRows = false;
            _dgvcontrolD.dataGridViewGG.AllowUserToOrderColumns = true;
            _dgvcontrolD.dataGridViewGG.AutoGenerateColumns = false;
            _dgvcontrolD.dataGridViewGG.MultiSelect = false;
            _dgvcontrolD.dataGridViewGG.GGRegistryAssemblyName = _PluginAssembly + "_" + "LPULicense";
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

            //  lbRecords.Text = _dt.Rows.Count.ToString() + " записей";
        }


        private void LoadDataM(string sql)
        {
            if (sql == "")
                return;

            //подключение списков к фильтру
            string activelist = "";
            if (_ActiveList != null)
            {
                Dictionary<string, string> gg = _ActiveList.GetSelectedLists();
                if (gg.Count > 0)
                {
                    foreach (var pair in gg)
                    {
                        if (pair.Key.Contains("LPU"))  //ищем списки с кактгорией LPU
                        {
                            if (activelist == "")
                                activelist += " (l.Code in " + pair.Value.ToString() + ") \n";
                            else activelist += " and (l.Code in " + pair.Value.ToString() + ") \n";
                        }
                    }
                    
                    if (sql.Contains(" where "))
                        sql = sql.Replace(" where ", " \n where " + activelist + " and "); //условие уже было
                    else
                        if (sql.Contains(" order by "))
                            sql = sql.Replace(" order by ", " \n where " + activelist + " \n order by "); //условия не было, но было сортировка
                        else
                            sql += " \n where " + activelist; //нет ни условия ни сортировки
                }
                gg = null;
            }


            MessageBox.Show(sql);

            _dbsql.SQLScript = sql;
            DataTable _dt = null;
            DataColumn _col = null;
            _dt = _dbsql.FillDataSet().Tables[0];
            _col = _dt.Columns.Add("IsCheckedField", System.Type.GetType("System.Boolean"));   ///добавлю Чекед поле
            _col.ReadOnly = false;
            _col.DefaultValue = "false";
            bsM.DataSource = _dt;            

            _dgvcontrolM.dataGridViewGG.DataSource = bsM;            
        }

        private void LoadDataD(string sql, int inLPUID)
        {
            if (sql == "")
                return;

            _dbsql.SQLScript = sql;
            if (inLPUID > -1)
                _dbsql.SQLScript = sql + " where lpu.ID = " + inLPUID.ToString();
            bsD.DataSource = _dbsql.FillDataSet().Tables[0]; ;
            _dgvcontrolD.dataGridViewGG.DataSource = bsD;
        }


        private void ReadObjectM()
        {
            _dgvcontrolM.dataGridViewGG.GGRegistrySoftName = _SoftName;

            try
            {
                _dbsql.SQLScript = "select ID, ObjectCaption, ObjectExpression from [GGPlatform].[Objects] where Upper(ObjectName) = @obj";
                _dbsql.AddParameter("@obj", EnumDBDataTypes.EDT_String, "rfclpu".ToUpper());
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
                MessageBox.Show("Ошибка настройки объекта rfcLPU /n " + ex.Message,
                                "Ошибка [модуль plugin_3, класс plugin_3, метод ReadObjectM]",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            
        }

        private void ReadObjectD()
        {
            _dgvcontrolD.dataGridViewGG.GGRegistrySoftName = _SoftName;

            try
            {
                _dbsql.SQLScript = "select ID, ObjectCaption, ObjectExpression from [GGPlatform].[Objects] where Upper(ObjectName) = @obj";
                _dbsql.AddParameter("@obj", EnumDBDataTypes.EDT_String, "lpulicense".ToUpper());
                SqlDataReader Objects = _dbsql.ExecuteReader();
                //int _obj = 0;
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
                MessageBox.Show("Ошибка настройки объекта LPULicense /n " + ex.Message,
                                "Ошибка [модуль plugin_3, класс plugin_3, метод ReadObjectD]",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

         //   GridLoadD();
        }


        private void panel1_Resize(object sender, EventArgs e)
        {
            int r = panel1.Height / 3;
            panel3.Height = r * 2;
            panel2.Height = r;
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> gg = _ActiveList.GetSelectedLists();

            string rr = "";
            foreach (var pair in gg)
            {
                rr = rr + " \n " + pair.Key + "   " + pair.Value.ToString();
            }
            MessageBox.Show(rr);
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            int yy = -100;
            _dbsql.SQLScript = "exec testError";
try
            {
                yy = _dbsql.ExecuteNonQueryVisual("executed");
            }
catch (Exception err)
{
    MessageBox.Show(err.Message, "hhhhhh");
}
MessageBox.Show(yy.ToString());       
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            _dbsql.SQLScript = "exec testError";
            int yy = -100;
            try
            {
                yy = _dbsql.ExecuteNonQuery();
            }
            catch(Exception err)
            {
                MessageBox.Show(err.Message, "hhhhhh");
            }
            
            MessageBox.Show(yy.ToString());
        }
       

    }
}