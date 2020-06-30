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

namespace SimRodnNom
{
    public partial class frmSimRodnNom : Form, GGPlatform.BuiltInApp.IPlugin
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

        public frmSimRodnNom() 
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
                _dgvcontrolM.dataGridViewGG.GGRegistryAssemblyName = _PluginAssembly + "_" + "RodnNomen";
                _dgvcontrolM.dataGridViewGG.Columns.Clear();
                _dgvcontrolM.dataGridViewGG.DoubleClick += tbEditt_Click;
                _dgvcontrolM.dataGridViewGG.CellEndEdit += (s, e) =>
                {
                    string _newval = "";
                    string _idrow = "";

                    _newval = _dgvcontrolM.dataGridViewGG[e.ColumnIndex, e.RowIndex].Value.ToString();
                    _idrow = _dgvcontrolM.dataGridViewGG.Rows[e.RowIndex].Cells["ID"].Value.ToString();

                    Int32 _val;
                    Int32 _id;

                    if (Int32.TryParse(_newval, out _val) && Int32.TryParse(_idrow, out _id))
                    {
                        _dbsql.SQLScript = "update uchRodnNomen set CountOrder = @newv where id = @id";
                        _dbsql.AddParameter("@newv", EnumDBDataTypes.EDT_Integer, _val.ToString());
                        _dbsql.AddParameter("@id", EnumDBDataTypes.EDT_Integer, _id.ToString());
                        _dbsql.ExecuteNonQuery();
                    }

                    /*string str = "ляляляля";
                    bool IsDigit = str.Length == str.Where(c => char.IsDigit(c)).Count();*/
                };
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

                    if (tmpFieldName == "CountOrder")//ручное редактирование поля
                        colu.ReadOnly = false;
                    else colu.ReadOnly = true;
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

        

        private void ReadObjectM()
        {
            _dgvcontrolM.dataGridViewGG.GGRegistrySoftName = _SoftName;
            try
            {
                _dbsql.SQLScript = "select ID, ObjectCaption, ObjectExpression from [GGPlatform].[Objects] where Upper(ObjectName) = @obj";
                _dbsql.AddParameter("@obj", EnumDBDataTypes.EDT_String, "RodnNomen".ToUpper());
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
                                "Ошибка [модуль SimRodnNom, класс SimRodnNom, метод ReadObjectM]",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }            
        }

        private void btactiveChangeData_Click(object sender, EventArgs e)
        {
            _dbsql.SQLScript = "exec uchRodnNomenc @IDOperation = 8";
            _dbsql.ExecuteNonQueryVisual("Обнуление информации о кол-ве заказываемого товара");
            LoadDataM(ObjectsSQLM);
            MessageBox.Show("Обнуление информации о кол-ве заказываемого товара завершено", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ClearTmpProduct()//очиу вренменную таблицу
        {
            _dbsql.SQLScript = @"exec uchRodnNomenc @IDOperation = 3";
            _dbsql.ExecuteNonQuery();
        }

        private void tbAnaliz_Click(object sender, EventArgs e)
        {
            _dbsql.SQLScript = "exec uchRodnNomenc @IDOperation = 9";
            _dbsql.ExecuteNonQueryVisual("Выполняется анализ ценовых предложений поставщиков");
            LoadDataM(ObjectsSQLM);
            MessageBox.Show("Анализ ценовых предложений поставщиков завершен", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void tbExportZakaz_Click(object sender, EventArgs e)
        {
            ExcelManager _excel = null;
            try
            {
                _dbsql.SQLScript = "exec uchRodnNomenc @IDOperation = 10";
                DataTable inDT = _dbsql.FillDataSet().Tables[0];

                //создаем ексел
                _excel = new ExcelManager(Application.StartupPath + _templatename + "uchOrderPhone.xlt");
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
                if (_excel.SaveDocument("c:\\temp\\uchOrderPhone__" + DateTime.Now.Date.ToShortDateString().Replace(".", "_") + ".xls", true, ESaveFormats.xlNormal))
                    MessageBox.Show(_MainForm, "Отчет сформирован и сохранён в каталоге c:\\temp\\", "Информация",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                _excel.CloseExcelDocumentAndShow();
            }
            catch
            {
                _excel.CloseDocument();
                _excel = null;
            }
        }

        private void tbEditt_Click(object sender, EventArgs e)
        {
            DataRow dr = null;
            try
            {
                dr = ((DataRowView)_dgvcontrolM.dataGridViewGG.CurrentRow.DataBoundItem).Row;
                if (dr == null)
                    return;

                frmEditRodn fmChange = new frmEditRodn(dr, Convert.ToInt32(dr["Id"]), _dbsql);

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
                MessageBox.Show("Ошибка изменение информации о товаре \n " + ex.Message,
                                "Ошибка",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                dr = null;
            }
        }

        private void tbImport_Click(object sender, EventArgs e)
        {
            frmLoadFromExcel fmLoad = new frmLoadFromExcel(new object[] { _dbsql, 1 });
            fmLoad.Text = "Прием родной номенклатуры";
            if (fmLoad.ShowDialog() == DialogResult.OK)
            {                
                MessageBox.Show("Родная номенклатура обработана", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDataM(ObjectsSQLM);
            }
            fmLoad.Dispose();
            fmLoad = null;
            ClearTmpProduct();
        }

        


       

    }
}