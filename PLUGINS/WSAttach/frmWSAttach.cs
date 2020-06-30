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
using System.ServiceModel;
using System.Xml;
using System.Xml.Serialization;
using System.Text;
using System.Threading;

namespace WSAttach
{
    public partial class frmWSAttach : Form, GGPlatform.BuiltInApp.IPlugin
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

        private string _templatename = "\\reports\\template\\";
        private ExcelManager _excel = null;
        private RegManag _reg = new RegManag();

        private string _WSSender;
        private string _WSUsername;
        private string _WSPassword;
        private string _WSUrl;
        private string _MaxRowCount = 10000.ToString();

        public frmWSAttach() 
        {      
            InitializeComponent();
        }

        int IPlugin.plugPluginButtons
        {
            get
            {
                return (int)EnumPluginButtons.EPB_Refresh +
                       (int)EnumPluginButtons.EPB_RefreshFilter+
                       (int)EnumPluginButtons.EPB_Option;
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

            _rfc.LoadRFC("Settings");
            _rfc.SetRFCValue("Settings", "key", "WSSender");
            _WSSender = _rfc.GetRFCValueOne("Settings", "value");
            _rfc.SetRFCValue("Settings", "key", "WSUsername");
            _WSUsername = _rfc.GetRFCValueOne("Settings", "value");
            _rfc.SetRFCValue("Settings", "key", "WSPassword");
            _WSPassword = _rfc.GetRFCValueOne("Settings", "value");
            _rfc.SetRFCValue("Settings", "key", "WSUrl");
            _WSUrl = _rfc.GetRFCValueOne("Settings", "value");
            _rfc.SetRFCValue("Settings", "key", "MaxRowCount");
            _MaxRowCount = _rfc.GetRFCValueOne("Settings", "value");


            ReadObjectM();
            GridLoadM();

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
        }
        void IPlugin.appRefresh()
        {
            LoadDataM(ObjectsSQLM);
        }
        void IPlugin.appRefreshFilter()
        {
            if (_QueryBuilderM == null)
                _QueryBuilderM = new QueryBuilder(ObjectsIDM, (IWin32Window)_MainApp, _dbsql, TypeReturnScript.trsScript);

            _QueryBuilderM.GGRegistrySoftName = _SoftName;
            _QueryBuilderM.GGRegistryAssemblyName = _PluginAssembly;

            string sqlstr = _QueryBuilderM.GetQuery();
            if (sqlstr != null)
                if (ObjectsSQLM.Replace(" ", "") != sqlstr.Replace(" ", ""))
                {
                    ObjectsSQLM = sqlstr;
                    LoadDataM(ObjectsSQLM);
                }        
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
                _dgvcontrolM.dataGridViewGG.GGRegistryAssemblyName = _PluginAssembly + "_" + "WSAttach_PeopleActs";
                _dgvcontrolM.dataGridViewGG.Columns.Clear();
              //  bsM.PositionChanged += new EventHandler(Position_Changed);  //отслеживает переход между строками

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

                  /*  if (tmpFieldName == "NewRozn")//ручное редактирование поля
                        colu.ReadOnly = false;
                    else colu.ReadOnly = true;*/

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

        private void LoadDataM(string sql)
        {
            try
            {
                if (sql == "")
                    return;
                _MainForm.Cursor = System.Windows.Forms.Cursors.WaitCursor;

                // сохраняю сортировку
                DataGridViewColumn _colSort = null;
                System.ComponentModel.ListSortDirection _dirSort = System.ComponentModel.ListSortDirection.Ascending; 
                if (_dgvcontrolM.dataGridViewGG.DataSource != null)
                {
                    _colSort = _dgvcontrolM.dataGridViewGG.SortedColumn;
                    if (_colSort != null)
                    {
                        if (_dgvcontrolM.dataGridViewGG.SortOrder == System.Windows.Forms.SortOrder.Descending)
                            _dirSort = System.ComponentModel.ListSortDirection.Descending;
                        else
                            _dirSort = System.ComponentModel.ListSortDirection.Ascending;
                    }
                }

                _dbsql.SQLScript = sql;
                DataTable _dt = null;
                _dt = _dbsql.FillDataSet().Tables[0];
                bsM.DataSource = _dt;

                _dgvcontrolM.dataGridViewGG.DataSource = bsM;

                if ((_colSort != null) && (_dgvcontrolM.dataGridViewGG.Columns.Contains(_colSort))) //возвращаю сортировку                
                {
                    _dgvcontrolM.dataGridViewGG.Sort(_colSort, _dirSort);
                }
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

        private void ReadObjectM()
        {
            _dgvcontrolM.dataGridViewGG.GGRegistrySoftName = _SoftName;
            try
            {
                _dbsql.SQLScript = "select ID, ObjectCaption, ObjectExpression from [GGPlatform].[Objects] where Upper(ObjectName) = @obj";
                _dbsql.AddParameter("@obj", EnumDBDataTypes.EDT_String, "WSAttach_PeopleActs".ToUpper());
                SqlDataReader Objects = _dbsql.ExecuteReader();
                while (Objects.Read())
                {
                    ObjectsIDM = (int)Objects["ID"];
                    ObjectsCaptionM = Objects["ObjectCaption"].ToString();
                    ObjectsSQLM = Objects["ObjectExpression"].ToString();
                }
                ObjectsSQLM = ObjectsSQLM.Replace("MaxRowCount", _MaxRowCount);
                Objects.Close();

                _dbsql.SQLScript = "select ID, isnull(FieldAlias, FieldName) as FieldName, FieldCaption, FieldType, FieldVisible from GGPlatform.ObjectsDescription where ObjectsRef = @obj and ((FieldType <> 'Q' and FieldVisible = 0) or (FieldVisible = 1))";
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
                MessageBox.Show("Ошибка настройки объекта BillWork \n " + ex.Message,
                                "Ошибка [модуль WSAttach, класс WSAttach, метод ReadObjectM]",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }            
        }


        private string _mnth = "";
        private string _year = "";
        private int _hour = 20;
        private ShowProgressTask myWait;
        private Thread myProcess;
        private void tbAnaliz_Click(object sender, EventArgs e)
        {            
            frmPeriod testDialog = new frmPeriod();
            if (testDialog.ShowDialog(this) == DialogResult.OK)
            {
                _mnth = (testDialog.cbMonth.SelectedIndex + 1).ToString();
                _year = Convert.ToInt16(testDialog.cbYear.Value).ToString();
                _hour = Convert.ToInt16(testDialog.numericUpDown1.Value);
            }
            else
            {
              //  MessageBox.Show("Необходимо указать период, подлежащий загрузке из ТФОМС", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                testDialog.Dispose();
                return;
            }
            testDialog.Dispose();

            _MainForm.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            Application.DoEvents();

            timer1.Interval = 60000 * 1; //опрос каждые минуты
            timer1.Tick += Timer1_Tick;
            timer1.Enabled = true; //запускаяю таимер
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            if ((DateTime.Now.Hour == _hour)  //время стрта
                && (DateTime.Now.Minute >= 00 && DateTime.Now.Minute <= 40)
                    && (timer1.Enabled))  //еслиеще не старотовал
            {
                timer1.Enabled = false;  //отключу таимер после первого запуска
                //Индикация длительного процесса
                myWait = new ShowProgressTask("Загрузка прикрепленного населения");
                myProcess = new Thread(doStuffOnThread);
                myProcess.Start();
                myWait.ShowDialog(this);
            }                        
        }

        private void closeWaitForm()
        {
            myWait.Close();
          //  MessageBox.Show("Your Process Is Complete");
        }
        private void doStuffOnThread()
        {
            try
            {
                //....
                //What ever process you want to do here ....
                loadPrick();
                //....
                if (myWait.InvokeRequired)
                {
                    myWait.BeginInvoke((MethodInvoker)delegate () { closeWaitForm(); });
                }
                else
                {
                    myWait.Close();//Fault tolerance this code should never be executed
                }
            }
            catch (Exception ex)
            {
                string exc = ex.Message;//Fault tolerance this code should never be executed
            }
        }
        private void loadPrick()
        {
            string session = Guid.NewGuid().ToString();

            _MainForm.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            Application.DoEvents();

            _dbsql.SQLScript = "exec [WSAttach].[spAttach] @IDOper = 2, @mnth = @m, @year = @y";
            _dbsql.AddParameter("@m", EnumDBDataTypes.EDT_Integer, _mnth.ToString());
            _dbsql.AddParameter("@y", EnumDBDataTypes.EDT_Integer, _year.ToString());
            _dbsql.ExecuteNonQuery();//удалю ранее загруженные сведения

            String url = _WSUrl;
            EndpointAddress address = new EndpointAddress(url);
            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
            binding.OpenTimeout = new TimeSpan(0, 1, 30);
            binding.CloseTimeout = new TimeSpan(0, 1, 30);
            binding.SendTimeout = new TimeSpan(0, 4, 0);
            binding.ReceiveTimeout = new TimeSpan(0, 5, 0);
            binding.MaxReceivedMessageSize = 2147483647;
            binding.MaxBufferPoolSize = 2147483647;
            XmlDictionaryReaderQuotas readerQuotas = new XmlDictionaryReaderQuotas();
            readerQuotas.MaxArrayLength = 2147483647;
            readerQuotas.MaxStringContentLength = 2147483647;
            binding.ReaderQuotas = readerQuotas;

            WS.DCExchangeSrv ws = new WS.DCExchangeSrvClient(binding, address);
            WS.GetAttachListByRangeActRequest request = new WS.GetAttachListByRangeActRequest();
            WS.GetAttachListByRangeActResponse response = null;

            request.username = _WSUsername;
            request.password = _WSPassword;
            request.sendercode = _WSSender;
            request.monthact = Convert.ToInt16(_mnth);
            request.yearact = Convert.ToInt16(_year);
            request.startid = 0;

            _dbsql.SQLScript = @"delete from [WSAttach].[transferAttach] where sessionGuid = @guid;
                                delete from [WSAttach].[Packages] where packagesTypeID = 1";
            _dbsql.AddParameter("@guid", EnumDBDataTypes.EDT_String, session);
            _dbsql.ExecuteNonQuery();

            do // в первом пакете общее кол-во
            {
                IAsyncResult res = ws.BeginGetAttachListByRangeAct(request, null, null);
                Application.DoEvents();
                response = ws.EndGetAttachListByRangeAct(res);

                string returnXml = SerializeObjectToXml(response);
                if (response.orderpack.p12_alist.Length > 0)
                {
                    string _IDPackage = "";
                    _dbsql.SQLScript = @"insert into [WSAttach].[Packages](packagesTypeID, pdate_send, pbody_send, perr_response, perrmes_response, pbody_response, sessionGuid) 
                                        values (1, @pdate, @pbody_send, @perr, @perrmes, @pbody_resp, @guid); 
                                        SELECT SCOPE_IDENTITY() AS SCOPE_IDENTITY;";
                    _dbsql.AddParameter("@pdate", EnumDBDataTypes.EDT_DateTime, response.orderpack.p10_packinf.p10_pakagedate.ToString());
                    _dbsql.AddParameter("@pbody_send", EnumDBDataTypes.EDT_String, SerializeObjectToXml(request));
                    _dbsql.AddParameter("@perr", EnumDBDataTypes.EDT_String, response.orderpack.p10_packinf.p13_zerrpkg.ToString());
                    _dbsql.AddParameter("@perrmes", EnumDBDataTypes.EDT_String, response.orderpack.p10_packinf.p14_errmsg == null ? String.Empty : response.orderpack.p10_packinf.p14_errmsg.ToString());
                    _dbsql.AddParameter("@pbody_resp", EnumDBDataTypes.EDT_String, returnXml);
                    _dbsql.AddParameter("@guid", EnumDBDataTypes.EDT_String, session);
                    _IDPackage = _dbsql.ExecuteScalar();

                    _dbsql.SQLScript = "exec [WSAttach].[spAttach] @IDOper = 1, @sessionGuid = @guid, @IDObject = @idobj, @mnth = @m, @year = @y";
                    _dbsql.AddParameter("@guid", EnumDBDataTypes.EDT_String, session);
                    _dbsql.AddParameter("@idobj", EnumDBDataTypes.EDT_Integer, _IDPackage);
                    _dbsql.AddParameter("@m", EnumDBDataTypes.EDT_Integer, _mnth);
                    _dbsql.AddParameter("@y", EnumDBDataTypes.EDT_Integer, _year);
                    _dbsql.ExecuteNonQuery();
                    request.startid = response.orderpack.p11_nid;
                }
              //  response.orderpack.p11_nid = -1;
            } while (response.orderpack.p11_nid != -1);

            if (response.orderpack.p10_packinf.p14_errmsg != null)
            {
                _MainForm.Cursor = System.Windows.Forms.Cursors.Default;
                Application.DoEvents();
                MessageBox.Show(_MainForm, "Информация о прикрепленном населении, согласно актов сверки загружена с ошибками\n" +
                                 response.orderpack.p10_packinf.p14_errmsg,
                                "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                response = null;
                request = null;

                ws = null;
                binding = null;
                address = null;

                myWait.Close();

                return;
            }
            response = null;
            request = null;

            ws = null;
            binding = null;
            address = null;

            _dbsql.SQLScript = "exec [WSAttach].[spAttach] @IDOper = 4, @sessionGuid = @guid";
            _dbsql.AddParameter("@guid", EnumDBDataTypes.EDT_String, session);
            _dbsql.ExecuteNonQueryVisual("Обновление регистра прикрепленного населения");

            _dbsql.SQLScript = "exec [WSAttach].[spAttach] @IDOper = 5, @sessionGuid = @guid, @mnth = @m, @year = @y";
            _dbsql.AddParameter("@guid", EnumDBDataTypes.EDT_String, session);
            _dbsql.AddParameter("@m", EnumDBDataTypes.EDT_Integer, _mnth);
            _dbsql.AddParameter("@y", EnumDBDataTypes.EDT_Integer, _year);
            string _rr = _dbsql.ExecuteScalar();

            _MainForm.Cursor = System.Windows.Forms.Cursors.Default;
            Application.DoEvents();

            if (_rr == null || _rr == "1")
            {
                myWait.Close();

                MessageBox.Show(_MainForm, "Информация о прикрепленном населении, согласно актов сверки загружена с ошибками\nВыявлены ошибки в численности населения",
                                "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                myWait.Close();

                MessageBox.Show(_MainForm, "Информация о прикрепленном населении, согласно актов сверки загружена\nСформируйте акты сверки чисоленности",
                                "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            frmActs fmActs = new frmActs(new object[] { _dbsql, _rfc, _SoftName, _PluginAssembly });
            fmActs.ShowDialog();
            fmActs.Dispose();
            fmActs = null;
        }







        public static string SerializeObjectToXml(Object data)
        {
            System.IO.StringWriter output = new System.IO.StringWriter();
            XmlSerializer xs = new XmlSerializer(data.GetType());
            xs.Serialize(output, data);
            return output.ToString();
        }



        public static T DeserializeXmlToObject<T>(string data) where T : class
        {
            if ((data == null) || (data.Trim().Length == 0))
                return null;

            var xs = new XmlSerializer(typeof(T));
            using (var sr = new StringReader(data))
                return (T)xs.Deserialize(sr);
        }


    }
}