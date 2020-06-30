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

namespace WSProf
{
    public partial class frmWSProf : Form, GGPlatform.BuiltInApp.IPlugin
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
        private string _templatename = "\\reports\\template\\";
        private ExcelManager _excel = null;
        private RegManag _reg = new RegManag();

        private string _WSSender;
        private string _WSUsername;
        private string _WSPassword;
        private string _WSUrl;
        private string _MaxRowCount = 10000.ToString();

        public frmWSProf() 
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

            ReadObjectD();
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

        private void Position_Changed(object sender, EventArgs e)
        {
            Int32 _id = -1;
            try
            {
                _id = (int)(((BindingSource)sender).Current as DataRowView)["PeopleID"];
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
                _dgvcontrolM.dataGridViewGG.GGRegistryAssemblyName = _PluginAssembly + "_" + "wsprof_Planlist";
                _dgvcontrolM.dataGridViewGG.Columns.Clear();
                bsM.PositionChanged += new EventHandler(Position_Changed);  //отслеживает переход между строками

                _dgvcontrolM.DBSqlServerGG = _dbsql;

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
                _dgvcontrolD.dataGridViewGG.GGRegistryAssemblyName = _PluginAssembly + "_" + "infos";
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

        private void LoadDataD(string sql, int inPeopleID)
        {
            try
            {
                if (sql == "")
                    return;

                _dbsql.SQLScript = sql;
                if (inPeopleID > -1)
                    _dbsql.SQLScript = sql + " where p.PeopleID = " + inPeopleID.ToString();
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
                _dbsql.AddParameter("@obj", EnumDBDataTypes.EDT_String, "wsprof_Planlist".ToUpper());
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
                                "Ошибка [модуль WSProf, класс WSProf, метод ReadObjectM]",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }            
        }

        private void ReadObjectD()
        {
            _dgvcontrolD.dataGridViewGG.GGRegistrySoftName = _SoftName;

            try
            {
                _dbsql.SQLScript = "select ID, ObjectCaption, ObjectExpression from [GGPlatform].[Objects] where Upper(ObjectName) = @obj";
                _dbsql.AddParameter("@obj", EnumDBDataTypes.EDT_String, "wsprof_FactsInfo".ToUpper());
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
                MessageBox.Show("Ошибка настройки объекта uchProductBillMove \n " + ex.Message,
                                "Ошибка [модуль SimMoveT, класс SimMoveT, метод ReadObjectD]",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void panel1_Resize(object sender, EventArgs e)
        {
            int r = panel1.Height / 7;
            panel5.Height = r * 5;
            panel2.Height = r * 2;
        }

        private void tbExportMonit_Click(object sender, EventArgs e)
        {
            frmRassilka fmLoad = new frmRassilka(new object[] { _dbsql, 1 });
            fmLoad.ShowDialog();
            fmLoad.Dispose();
            fmLoad = null;
        }

        private void tbAnaliz_Click(object sender, EventArgs e)
        {

        }

        public static string SerializeObjectToXml(Object data)
        {
            System.IO.StringWriter output = new System.IO.StringWriter();
            XmlSerializer xs = new XmlSerializer(data.GetType());
            xs.Serialize(output, data);
            return output.ToString();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            _dbsql.SQLScript = "exec [WSProf].[spPlanInfos] @IDOper = 6";
            _dbsql.ExecuteNonQueryVisual("Обновление адресов и номеров телефонов");
        }

        private void обработатьПротоколToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmRassilka fmLoad = new frmRassilka(new object[] { _dbsql, 2 });
            fmLoad.ShowDialog();
            fmLoad.Dispose();
            fmLoad = null;
        }

        private void ручноИнформированиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<DataRow> _rows = _dgvcontrolM.dataGridViewGG.GGGetCheckedDataRows();
            if (_rows.Count < 1)
            {
                MessageBox.Show("Необходимо отметить записи, подлежащие ручному информированию", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            frmInformList fmLoad = new frmInformList(new object[] { _dbsql, _rows });
            fmLoad.ShowDialog();
            fmLoad.Dispose();
            fmLoad = null;
        }

        private void непринятыеЗаписиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmInfosError fmLoad = new frmInfosError(new object[] { _dbsql, _rfc, _SoftName, _PluginAssembly });
            fmLoad.ShowDialog();
            fmLoad.Dispose();
            fmLoad = null;
        }

        private void экпортВТФОМСToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string _mnth1_inf = "";
            string _mnth2_inf = "";
            string _mnth1_pln = "";
            string _mnth2_pln = "";
            string _year = "";
            string _failed = "0";

            frmExportPeriod testDialog = new frmExportPeriod();
            if (testDialog.ShowDialog(this) == DialogResult.OK)
            {
                _mnth1_inf = (testDialog.cbMonth.SelectedIndex + 1).ToString();
                _mnth2_inf = (testDialog.cbMonth2.SelectedIndex + 1).ToString();
                _mnth1_pln = (testDialog.cbMonthPlan_1.SelectedIndex + 1).ToString();
                _mnth2_pln = (testDialog.cbMonthPlan_2.SelectedIndex + 1).ToString();
                _year = Convert.ToInt16(testDialog.cbYear.Value).ToString();
                if (testDialog.cbcFialed.Checked) _failed = "1";
            }
            else
            {
                //MessageBox.Show("Необходимо указать период, подлежащий передаче в ТФОМС", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                testDialog.Dispose();
                return;
            }
            testDialog.Dispose();

            String url = _WSUrl;
            EndpointAddress address = new EndpointAddress(url);
            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
            binding.OpenTimeout = new TimeSpan(0, 2, 30);
            binding.CloseTimeout = new TimeSpan(0, 1, 30);
            binding.SendTimeout = new TimeSpan(0, 3, 0);
            binding.ReceiveTimeout = new TimeSpan(0, 3, 0);
            binding.MaxReceivedMessageSize = 2147483647;
            binding.MaxBufferPoolSize = 2147483647;
            XmlDictionaryReaderQuotas readerQuotas = new XmlDictionaryReaderQuotas();
            readerQuotas.MaxArrayLength = 2147483647;
            readerQuotas.MaxStringContentLength = 2147483647;
            binding.ReaderQuotas = readerQuotas;

            _MainForm.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            WS.DCExchangeSrv ws = new WS.DCExchangeSrvClient(binding, address);
            WS.putEvFactInfosRequest request = null;
            WS.putEvFactInfosResponse response = null;
            _dbsql.SQLScript = "exec [WSProf].[spFactInfos] @IDOper = 1, @Mnth1 = @mn1, @Mnth2 = @mn2, @Year = @y, @isFailedInform = @ff, @mnth1_pln = @mp1, @mnth2_pln = @mp2";
            _dbsql.AddParameter("@mn1", EnumDBDataTypes.EDT_String, (_mnth1_inf == "0") ? null : _mnth1_inf); 
            _dbsql.AddParameter("@mn2", EnumDBDataTypes.EDT_String, (_mnth2_inf == "0") ? null : _mnth2_inf);
            _dbsql.AddParameter("@y", EnumDBDataTypes.EDT_String, _year);
            _dbsql.AddParameter("@ff", EnumDBDataTypes.EDT_Bit, _failed);
            _dbsql.AddParameter("@mp1", EnumDBDataTypes.EDT_String, (_mnth1_pln == "0") ? null : _mnth1_pln);
            _dbsql.AddParameter("@mp2", EnumDBDataTypes.EDT_String, (_mnth2_pln == "0") ? null : _mnth2_pln);
            string requestXML = _dbsql.ExecuteScalar();
            request = DeserializeXmlToObject<WS.putEvFactInfosRequest>(requestXML);
            response = ws.putEvFactInfos(request);
            string _IDPackage = "";
            _dbsql.SQLScript = @"insert into [WSProf].Packages (packagesTypeID, pdate_send, perr_response, pbody_send, perrmes_response, pbody_response) 
                                  values(3, @pdate_send, @perr_resp, @pbody_send, @perrmes_resp, @pbody_resp);
                                  SELECT SCOPE_IDENTITY() AS SCOPE_IDENTITY;";
            _dbsql.AddParameter("@pdate_send", EnumDBDataTypes.EDT_DateTime, request.orderpack.p10_packinf.p10_pakagedate.ToString());
            _dbsql.AddParameter("@perr_resp", EnumDBDataTypes.EDT_Integer, response.responcepack.r10_packinf.p13_zerrpkg.ToString());
            _dbsql.AddParameter("@pbody_send", EnumDBDataTypes.EDT_String, requestXML);
            _dbsql.AddParameter("@perrmes_resp", EnumDBDataTypes.EDT_String, response.responcepack.r10_packinf.p14_errmsg == null ? String.Empty : response.responcepack.r10_packinf.p14_errmsg);
            _dbsql.AddParameter("@pbody_resp", EnumDBDataTypes.EDT_String, response == null ? String.Empty : SerializeObjectToXml(response));
            _IDPackage = _dbsql.ExecuteScalar();
            if (response.responcepack.r10_packinf.p14_errmsg != null)
            {
                _MainForm.Cursor = System.Windows.Forms.Cursors.Default;
                MessageBox.Show(@"В пакете с идентификатором ID = " + _IDPackage + " обнаружены критические ошибки:\n"
                                 + "[" + response.responcepack.r10_packinf.p14_errmsg + "]\n"
                                 + "Устраните ошибки и повторите операцию",
                                 "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                response = null;
                request = null;
                return;
            }

            _dbsql.SQLScript = "exec [WSProf].[spFactInfos] @IDOper = 2, @IDObject = @obj";
            _dbsql.AddParameter("@obj", EnumDBDataTypes.EDT_String, _IDPackage);
            _dbsql.ExecuteNonQuery();
            _MainForm.Cursor = System.Windows.Forms.Cursors.Default;
            if ((response.responcepack.r12_orerl != null) && (response.responcepack.r12_orerl.Length > 0))
            {
                MessageBox.Show(@"В пакете с идентификатором ID = " + _IDPackage + " обнаружены ошибки в передаваемых сведениях\n"
                                 + "Исправьте ошибки в сведениях и повторите передачу данных",
                                 "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show("Информация о проведенном информировании передана в ТФОМС без ошибок",
                                "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            response = null;
            request = null;
            binding = null;
            address = null;
            ws = null;
        }

        private void обработкаРезультатовИнформированияИзВнешнегоИсточникаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmInformExcel fmLoad = new frmInformExcel(new object[] { _dbsql });
            fmLoad.ShowDialog();
            fmLoad.Dispose();
            fmLoad = null;
        }

        private void frmWSProf_Load(object sender, EventArgs e)
        {

        }

        public static T DeserializeXmlToObject<T>(string data) where T : class
        {
            if ((data == null) || (data.Trim().Length == 0))
                return null;

            var xs = new XmlSerializer(typeof(T));
            using (var sr = new StringReader(data))
                return (T)xs.Deserialize(sr);
        }

        private void tbImport_Click(object sender, EventArgs e)
        {
            frmPlanInfosImport fmLoad = new frmPlanInfosImport(new object[] { _dbsql, _WSSender, _WSUsername, _WSPassword, _WSUrl });
            if (fmLoad.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("Списки обработаны", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDataM(ObjectsSQLM);
            };
            fmLoad.Dispose();
            fmLoad = null;
        }







    }
}