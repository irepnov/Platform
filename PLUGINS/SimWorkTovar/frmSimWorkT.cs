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

namespace SimWorkT
{
    public partial class frmSimWorkT : Form, GGPlatform.BuiltInApp.IPlugin
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
        private DateTime _minRFADate = DateTime.MinValue;
        private RegManag _reg = new RegManag();

        public frmSimWorkT() 
        {      
            InitializeComponent();
        }

        int IPlugin.plugPluginButtons
        {
            get
            {
                return (int)EnumPluginButtons.EPB_Refresh +
                       (int)EnumPluginButtons.EPB_RefreshFilter + 
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

            ReadMinDateRFA();

            ReadObjectM();
            ReadObjectD();
            GridLoadM();
            GridLoadD();

            this.MdiParent = _MainForm;
            this.Show();
            this.WindowState = FormWindowState.Maximized;
            this.Text = _PluginName;          
        }

        private void ReadMinDateRFA()
        {
            string _p = "Software\\GGPlatform\\" + _SoftName + "\\" + _PluginAssembly + "\\Options";
            object _min = null;
            _min = _reg.GGRegistryGetValue("MinDateRFA", _p);
            if (_min != null)
                _minRFADate = Convert.ToDateTime(_min);
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
            _reg = null;
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
            frmOptionInter fmOption = new frmOptionInter(new object[] { _SoftName, _PluginAssembly });
            if (fmOption.ShowDialog() == DialogResult.OK)
                ReadMinDateRFA();
            fmOption.Dispose();
            fmOption = null;
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
            Int32 _idIncome = -1;
            Int32 _idMove = -1;
            try
            {
                _idIncome = (int)(((BindingSource)sender).Current as DataRowView)["BillIncomeRef"];
                string _id = ((((BindingSource)sender).Current as DataRowView)["BillMoveRef"]).ToString();

                if (!String.IsNullOrEmpty(_id))
                    _idMove = Convert.ToInt32(_id);

                LoadDataD(ObjectsSQLD, _idIncome, _idMove);                
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
                _dgvcontrolM.dataGridViewGG.GGRegistryAssemblyName = _PluginAssembly + "_" + "Product";
                
                _dgvcontrolM.dataGridViewGG.DoubleClick += tbEditPhoneNum_Click; 
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

                    if ((!_dbsql.InfoAboutConnection.UserIsAdmin) &&
                       (
                            (tmpFieldName == "SumI") || (tmpFieldName == "SumAll") || (tmpFieldName == "SumPeriod")
                       ))
                        continue; //пропускаю поля Вознаграждений, оставляю их только для Админа                        

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
                _dgvcontrolD.dataGridViewGG.GGRegistryAssemblyName = _PluginAssembly + "_" + "ProductBills";
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

                _MainForm.Cursor = System.Windows.Forms.Cursors.WaitCursor;

                //добавлю учет минимальной даты РФА
                if ((_minRFADate != null) && (_minRFADate != DateTime.MinValue))
                {
                    if (sql.Contains(" where ")) //если уже есть какието условия поиска, то добавим наше
                        sql = sql.Replace(" where ", " where (u.DateInputRFA is null or u.DateInputRFA >= '" + _minRFADate.ToShortDateString() + "') and ");
                    else
                        if (sql.Contains(" order by ")) //если не ыбло ограничений, но была сортировка
                            sql = sql.Replace(" order by", " where (u.DateInputRFA is null or u.DateInputRFA >= '" + _minRFADate.ToShortDateString() + "') order by ");
                        else //не было ограничений и не ыбло сортировки
                            sql = sql + " where (u.DateInputRFA is null or u.DateInputRFA >= '" + _minRFADate.ToShortDateString() + "') ";
                }

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

        private void LoadDataD(string sql, int inBillIncomeID, int inBillMoveID)
        {
            try
            {
                if (sql == "")
                return;

                _MainForm.Cursor = System.Windows.Forms.Cursors.WaitCursor;

                _dbsql.SQLScript = sql;
                _dbsql.SQLScript = String.Format(sql, inBillIncomeID, inBillMoveID);

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

        private void ReadObjectM()
        {
            _dgvcontrolM.dataGridViewGG.GGRegistrySoftName = _SoftName;
            try
            {
                _dbsql.SQLScript = "select ID, ObjectCaption, ObjectExpression from [GGPlatform].[Objects] where Upper(ObjectName) = @obj";
                _dbsql.AddParameter("@obj", EnumDBDataTypes.EDT_String, "Product".ToUpper());
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
                MessageBox.Show("Ошибка настройки объекта BillWork \n " + ex.Message,
                                "Ошибка [модуль SimWorkT, класс SimWorkT, метод ReadObjectM]",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }            
        }

        private void tbEditPhone_Click(object sender, EventArgs e)
        {
            
        }

        private void ClearTmpProducts()
        {
            _dbsql.SQLScript = "exec uchProducts @IDOperation = 1";
            _dbsql.ExecuteNonQuery();
        }

        private void LoadActive(int inOperation)
        {
            frmLoadFromExcel fmLoad = new frmLoadFromExcel(new object[] { _dbsql, 1 });
            if (inOperation == 4)
                fmLoad.Text = "Прием реестра вновь активированных номеров";
            if (inOperation == 11)
                fmLoad.Text = "Прием реестра ранее активированных номеров";
            if (fmLoad.ShowDialog() == DialogResult.OK)
            {
                _dbsql.SQLScript = "exec uchProducts @IDOperation = " + inOperation.ToString();
                DataTable dt = _dbsql.FillDataSet().Tables[0];
                if (dt.Rows.Count > 0)
                    ExportExcelNotActive(dt);
                else
                    MessageBox.Show("Товар активирован", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            ClearTmpProducts();
            fmLoad.Dispose();
            fmLoad = null;
        }

        private void tbActive_Click(object sender, EventArgs e)
        {
            LoadActive(4);
        }

        private void ExportExcelNotActive(DataTable inDT)
        {
            //создаем ексел
            _excel = new ExcelManager(Application.StartupPath + _templatename + "uchNotFound.xlt");
            _excel.SetCalculation(ECalculation.xlCalculationManual);
            _excel.SetScreenUpdating(false);
            //массив данных, который начинается с столбца №2, т.е. указав номер столбца, можно исключить первые стоблцы
            int indexStartColumn = 0; //если например из результирующего набора не выводить Первый столбец ID
            object[,] DataArray = _excel.GetDataArrayFromDataTable(inDT, indexStartColumn, _MainForm);
            //перенести массива в Excel
            //стартовая ячейка
            int indexFirstExcelRow = 3; //    3
            int indexFirstExcelColumn = 1; // A            
            string _Range = CExcelRanges.Ranges[indexFirstExcelColumn - 1] + indexFirstExcelRow.ToString() +   //начало диапазона  J10   -1 т.к.массив с 0
                            ":" +
                            CExcelRanges.Ranges[(indexFirstExcelColumn - 1 + inDT.Columns.Count - indexStartColumn) - 1] + ((inDT.Rows.Count + indexFirstExcelRow) - 1).ToString();  //конец диапазона
            _excel.SetValueToRange(_Range, DataArray);
            _excel.SetBorderStyle(_Range, BorderLineStyle.xlContinuous);
            _excel.SetScreenUpdating(true);
            DataArray = null;
            //сохраняем выходим           
            if (_excel.SaveDocument("c:\\temp\\uchNotFound.xls", true, ESaveFormats.xlNormal))
                MessageBox.Show(_MainForm, "Отчет сформирован и сохранён в каталоге c:\\temp\\", "Информация",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
            _excel.CloseExcelDocumentAndShow();
        }

        private void btactiveChangeData_Click(object sender, EventArgs e)
        {
            LoadActive(11);
        }

        private void btVoznagr_Click(object sender, EventArgs e)
        {
            frmLoadFromExcel fmLoad = new frmLoadFromExcel(new object[] { _dbsql, 2 });
            fmLoad.Text = "Прием реестра вознаграждений";
            if (fmLoad.ShowDialog() == DialogResult.OK)
            {
                _dbsql.SQLScript = "exec uchProducts @IDOperation = 6, @isMTS = 0";
                _dbsql.ExecuteNonQuery();

                MessageBox.Show("Информация о вознаграждении обработана", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            ClearTmpProducts();
            fmLoad.Dispose();
            fmLoad = null;
        }

        private void btEqualPhone_Click(object sender, EventArgs e)
        {

        }

        private void tbEditPhoneNum_Click(object sender, EventArgs e)
        {
            DataRow dr = null;
            try
            {
                dr = ((DataRowView)_dgvcontrolM.dataGridViewGG.CurrentRow.DataBoundItem).Row;
                if (dr == null)
                    return;

                frmChangePhone fmChange = new frmChangePhone(dr["NumberPhone"].ToString(), Convert.ToInt32(dr["Id"]), _dbsql);

                if (fmChange.ShowDialog() == DialogResult.OK)
                {
                    LoadDataM(ObjectsSQLM);
                }
                fmChange.Dispose();
                fmChange = null;
                dr = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка изменение номера телефона \n " + ex.Message,
                                "Ошибка",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                dr = null;
            }
        }

        private void tbCena_Product_Click(object sender, EventArgs e)
        {
            frmLoadFromExcel fmLoad = new frmLoadFromExcel(new object[] { _dbsql, 4 });
            fmLoad.Text = "Прием реестра товара, подлежащего изменению";
            if (fmLoad.ShowDialog() == DialogResult.OK)
            {
                _dbsql.SQLScript = "exec uchProducts @IDOperation = 13";
                _dbsql.ExecuteNonQuery();

                MessageBox.Show("Изменение информации о товаре произведено", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            ClearTmpProducts();
            fmLoad.Dispose();
            fmLoad = null;
        }

        private void btTovarReport_Click(object sender, EventArgs e)
        {
            frmLoadFromExcel fmLoad = new frmLoadFromExcel(new object[] { _dbsql, 5 });
            fmLoad.Text = "Список товара по складу";
            fmLoad.ShowDialog();
            ClearTmpProducts();
            fmLoad.Dispose();
            fmLoad = null;
        }

        private void tbSopost_Click(object sender, EventArgs e)
        {
            frmLoadFromExcel fmLoad = new frmLoadFromExcel(new object[] { _dbsql, 3 });
            fmLoad.Text = "Сопоставление номеров";
            fmLoad.ShowDialog();
            ClearTmpProducts();
            fmLoad.Dispose();
            fmLoad = null;
        }

        private void tbRFA_WD_Click(object sender, EventArgs e)
        {
            frmLoadFromExcel fmLoad = new frmLoadFromExcel(new object[] { _dbsql, 6 });
            fmLoad.Text = "Прием реестра товара, зарегистрированного у Web Dealera";
            if (fmLoad.ShowDialog() == DialogResult.OK)
            {
                _dbsql.SQLScript = "exec uchProducts @IDOperation = 5";
                _dbsql.ExecuteNonQuery();

                MessageBox.Show("Обработка информации о товаре завершена", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            ClearTmpProducts();
            fmLoad.Dispose();
            fmLoad = null;
        }

        private void tbRFA_Provider_Click(object sender, EventArgs e)
        {
            frmLoadFromExcel fmLoad = new frmLoadFromExcel(new object[] { _dbsql, 7 });
            fmLoad.Text = "Прием реестра РФА, сданных оператору";
            if (fmLoad.ShowDialog() == DialogResult.OK)
            {
                _dbsql.SQLScript = "exec uchProducts @IDOperation = 12";
                _dbsql.ExecuteNonQuery();

                MessageBox.Show("Обработка информации о РФА, сданных оператору завершена", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            ClearTmpProducts();
            fmLoad.Dispose();
            fmLoad = null;
        }

        private void tbInputRFA_Click(object sender, EventArgs e)
        {
            frmLoadFromExcel fmLoad = new frmLoadFromExcel(new object[] { _dbsql, 8 });
            fmLoad.Text = "Загрузка принятых РФА";
            if (fmLoad.ShowDialog() == DialogResult.OK)
            {
               /* _dbsql.SQLScript = "exec uchProducts @IDOperation = 5";
                _dbsql.ExecuteNonQuery();*/

                MessageBox.Show("Обработка информации о товаре завершена", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            ClearTmpProducts();
            fmLoad.Dispose();
            fmLoad = null;
        }

        private void ReadObjectD()
        {
            _dgvcontrolD.dataGridViewGG.GGRegistrySoftName = _SoftName;

            try
            {
                _dbsql.SQLScript = "select ID, ObjectCaption, ObjectExpression from [GGPlatform].[Objects] where Upper(ObjectName) = @obj";
                _dbsql.AddParameter("@obj", EnumDBDataTypes.EDT_String, "ProductBills".ToUpper());
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
                MessageBox.Show("Ошибка настройки объекта uchProductBillWork \n " + ex.Message,
                                "Ошибка [модуль SimWorkT, класс SimWorkT, метод ReadObjectD]",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void panel1_Resize(object sender, EventArgs e)
        {
            int r = panel1.Height / 4;
            panel3.Height = r * 3;
            panel2.Height = r;
        }
       

    }
}