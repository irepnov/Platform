using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using GGPlatform.BuiltInApp;
using GGPlatform.DBServer;
using GGPlatform.QueryBuilder;
using GGPlatform.InspectorManagerSP;
using GGPlatform.RFCManag;

namespace plugin_2
{
    public partial class formPlugin2 : Form, GGPlatform.BuiltInApp.IPlugin
    {
        private object _MainApp = null;
        private Form _MainForm = null;
        private int _PluginID;
        private int _WorkplaceID;
        private string _PluginAssembly, _PluginName, _SoftName = "";
        private DBSqlServer _dbsql = null;
        private DelegClosePlugin _ClosePlugin;
        private RFCManager _rfc = null;


        QueryBuilder _QueryBuilder = null;
        string ObjectsSQL = "";
        string ObjectsID = "";
        DataTable _dt = null;
        CInspectorManager oCInspectorManagerlater = null;

        public formPlugin2() 
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
            //LoadDGV();

            this.MdiParent = _MainForm;
            this.Show();
            this.WindowState = FormWindowState.Maximized;
            this.Text = _PluginName;
            _rfc.LoadRFC("rfcOKOGU");
            _rfc.LoadRFC("rfcOKOPF");
            _rfc.LoadRFC("rfcLPU");
            LoadInsp();
        }

        void IPlugin.appListAdd()
        {
            //    MessageBox.Show(" plugin1  refresh ");
        }

        void IPlugin.appListDel()
        {
            //   MessageBox.Show(" plugin1  refresh filter ");
        }

      /*  void IPlugin.plugMakeMenuStrip()
        {
            MenuStrip mnu = null;
            if (_MainForm.Controls.Find("menuStrip1", false) != null && _MainForm.Controls.Find("menuStrip1", false).Length > 0)
            {
                mnu = (MenuStrip)_MainForm.Controls.Find("menuStrip1", false)[0];
            }
            if (mnu == null) throw new Exception("Меню не найдено");
            
            //создаем новые элементы меню

            ToolStripMenuItem pluginMenu = new ToolStripMenuItem();
            pluginMenu.Text = "Плагин";

            ToolStripMenuItem mySubMenu1 = new ToolStripMenuItem();
            ToolStripMenuItem mySubMenu2 = new ToolStripMenuItem();

            mySubMenu1.Click += new EventHandler(mySubMenu1_Click);
            mySubMenu2.Click += new EventHandler(mySubMenu2_Click);

            mySubMenu1.Text = "plugin 2, submit1";
            mySubMenu2.Text = "plugin 2, submit2";

            pluginMenu.DropDownItems.Add(mySubMenu1);
            pluginMenu.DropDownItems.Add(new ToolStripSeparator());
            pluginMenu.DropDownItems.Add(mySubMenu2);
            //добавляем новые элементы меню на форму
            mnu.Items.Add(pluginMenu);
        }*/

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

        }

        void IPlugin.appRefresh()
        {
           // LoadData(ObjectsSQL);
        }

        void IPlugin.appRefreshFilter()
        {
           /*  if (_QueryBuilder == null)
                _QueryBuilder = new QueryBuilder(Convert.ToInt32(ObjectsID), (IWin32Window)_MainApp, _dbsql);
            string sqlstr = _QueryBuilder.GetQuery();
            if (sqlstr != null)
                if (ObjectsSQL.Replace(" ", "") != sqlstr.Replace(" ", ""))
                {
                    ObjectsSQL = sqlstr;
                    LoadData(ObjectsSQL);
                }*/
        }


        void IPlugin.appOption() { }

        private void mySubMenu1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("мое меню, плагин 2, подпункт 1");
        }

        private void mySubMenu2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("мое меню, плагин 2, подпункт 2");
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

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            _ClosePlugin(_WorkplaceID, _PluginID);
        }

        private void LoadData(string sql)
        {
            //MessageBox.Show(" plugin2  loaddata ");
            _dbsql.SQLScript = sql;
            _dt = _dbsql.FillDataSet().Tables[0];
            dataGridView1.DataSource = _dt;    
        }

        private void LoadDGV()
        {
            dataGridView1.AutoGenerateColumns = false;

            _dbsql.SQLScript = "select id, ObjectExpression from GGPlatform.Objects where id = 1";  ///имя плагина
            SqlDataReader Objects = _dbsql.ExecuteReader();
            while (Objects.Read())
            {
                ObjectsSQL = Objects["ObjectExpression"].ToString();
                ObjectsID = Objects["id"].ToString();
            }
            Objects.Close();
            Objects.Dispose();

            LoadData(ObjectsSQL);
            
            //_dbsql.SQLScript = "select * from GGPlatform.ObjectsDescription where ObjectsRef = @ObjectsRef order by id";
            _dbsql.SQLScript = "select ID, FieldName, isnull(FieldAlias, FieldName) as FieldAlias, FieldCaption, FieldType, FieldVisible, ObjectsRef, ReferenceRef from GGPlatform.ObjectsDescription where ObjectsRef = @ObjectsRef order by id";


            _dbsql.AddParameter("@ObjectsRef", EnumDBDataTypes.EDT_Integer, ObjectsID);
            SqlDataReader ObjectsDescription = _dbsql.ExecuteReader();

            string tmpFieldName = "";
            string tmpFieldCaption = "";           
            DataGridViewColumn column;
            DataGridViewCell cell;
            //галочка
            column = new DataGridViewCheckBoxColumn();
            column.HeaderText = "V";
            column.Name = "V";
            dataGridView1.Columns.Add(column);
            
            //остальные поля
            while (ObjectsDescription.Read())
            {
                tmpFieldName = ObjectsDescription["FieldAlias"].ToString();
                tmpFieldCaption = ObjectsDescription["FieldCaption"].ToString();
                _dt.Columns[tmpFieldName].Caption = tmpFieldCaption;
                column = new DataGridViewColumn();
                column.Name = tmpFieldName;
                column.DataPropertyName = tmpFieldName;
                column.HeaderText = tmpFieldCaption;

                if ( (ObjectsDescription["FieldType"].ToString() == "C") ||
                     (ObjectsDescription["FieldType"].ToString() == "D") ||
                     (ObjectsDescription["FieldType"].ToString() == "N") ||
                     (ObjectsDescription["FieldType"].ToString() == "R") )
                { 
                    cell = new DataGridViewTextBoxCell();
                } else
                    if (ObjectsDescription["FieldType"].ToString() == "L")
                    {
                        cell = new DataGridViewCheckBoxCell();
                    }
                    else
                    {
                        cell = new DataGridViewTextBoxCell();
                    }
                column.CellTemplate = cell;
                dataGridView1.Columns.Add(column);
            }
            ObjectsDescription.Close();



        }

        private void LoadInsp()
        {
            propertyGridEx1.ShowCustomProperties = true;
            propertyGridEx1.Item.Clear();
            oCInspectorManagerlater = new CInspectorManager(ref propertyGridEx1, ref _dbsql, ref _rfc);
            oCInspectorManagerlater.Add(EInspectorModes.ListBox, EInspectorTypes.TypeOfListInt,
                                        "1.Список из таблицы", "Поле", "Справочник ОКОПФ (select)", "OKOPFRef",
                                        "select id, name from rfcOKOPF", "");
            oCInspectorManagerlater.Add(EInspectorModes.ListBox, EInspectorTypes.TypeOfListInt,
                                        "2.Список", "Поле", "Сгенерированный список", "Months",
                                        " select 0 as id, '' as name " +
                                        " union select 1 as id, 'январь' as name " +
                                        " union select 2 as id, 'февраль' as name " +
                                        " union select 3 as id, 'март' as name " +
                                        " union select 4 as id, 'апрель' as name ", "");
            oCInspectorManagerlater.Add(EInspectorModes.ListBox, EInspectorTypes.TypeOfListString,
                                        "3.Список", "Поле", "Сгенерированный список строк", "names",
                                        " select '' as code, '' names " +
                                        " union select 'А' as code, 'буква А' names " +
                                        " union select 'Б' as code, 'буква Б' names " +
                                        " union select 'В' as code, 'буква В' names " +
                                        " union select 'Г' as code, 'буква Г' names ", "");
            oCInspectorManagerlater.Add(EInspectorModes.Reference, EInspectorTypes.TypeOfReferenceInt,
                                        "4.ОКОГУ", "Поле", "Справочник ОКОГУ (множественный выбор)", "RFCOKOGU", "", "rfcOKOGU", true);
            oCInspectorManagerlater.Add(EInspectorModes.Reference, EInspectorTypes.TypeOfReferenceInt,
                                        "5.ОКОПФ", "Поле", "Справочник ОКОПФ (разовый выбор)", "RFCOKOPF", "", "rfcOKOPF");
            oCInspectorManagerlater.Add(EInspectorModes.Simple, EInspectorTypes.TypeOfStringLike,
                                        "6.Фамилия", "Поле", "Фамилия пациента", "FAM", "", "");
            oCInspectorManagerlater.Add(EInspectorModes.ExpandType, EInspectorTypes.TypeOfExpandDate,
                                        "7.Диапазон ДР", "Поле", "Диапазон дат рождения", "Datr", "", "");
            oCInspectorManagerlater.Add(EInspectorModes.Simple, EInspectorTypes.TypeOfDate,
                                        "8.Дата загрузки", "Поле", "Дата загрузки", "CreateDate", "", "");
            oCInspectorManagerlater.Add(EInspectorModes.Simple, EInspectorTypes.TypeOfInt,
                                        "9.№ записи", "Поле", "№ записи", "RecordNumber", "", "");
            oCInspectorManagerlater.Add(EInspectorModes.ExpandType, EInspectorTypes.TypeOfExpandInt,
                                        "9.Диапазон № случая", "Поле", "Диапазон № случая", "CaseID", "", "");
            oCInspectorManagerlater.Add(EInspectorModes.Reference, EInspectorTypes.TypeOfReferenceInt,
                                        "10.ЛПУ", "Поле", "Справочник ЛПУ (множественный выбор)", "RFClpu", "", "rfclpu", true);
            propertyGridEx1.Refresh();                
        }

        private void button2_Click(object sender, EventArgs e)
        {
            oCInspectorManagerlater.ClearPropertyGrid();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> gg = oCInspectorManagerlater.GetDictionaryValues();

            string rr = "";
            foreach (var pair in gg)
            {
                rr = rr + " \n " + pair.Key + "   " + pair.Value.ToString();                
            }
            MessageBox.Show(rr);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            object t = oCInspectorManagerlater.GetValueByIndex(Convert.ToInt32(textBox1.Text));
            if (t != null)
                MessageBox.Show(t.ToString());
        }
       

    }
}