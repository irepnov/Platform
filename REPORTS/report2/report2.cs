using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Sql;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

using GGPlatform.DBServer;
using GGPlatform.RFCManag;
using GGPlatform.BuiltInApp;
using PropertyGridEx;
using DataGridViewGGControl;
using GGPlatform.InspectorManagerSP;
using GGPlatform.ExcelManagers;

namespace report2
{
    public class Class1 : GGPlatform.BuiltInApp.IReport
    {

        private object _MainApp = null;
        private Form _MainForm = null;
        private DBSqlServer _dbsql = null;
        private DataGridViewGGControl.DataGridViewGGControl _dgvcontrol = null;
        private RFCManager _rfc = null;

        private string _reportAssemblyName;
        private int _reportID;
        private PropertyGridEx.PropertyGridEx _propertyGrid = new PropertyGridEx.PropertyGridEx();
        private CInspectorManager _oCInspectorManager = null;
        private ExcelManager _excel = null;
        private string _templatename = "\\reports\\assembly\\" + "report2.xlt";


        string IReport.reportAssemblyName
        {
            get { return _reportAssemblyName; }
            set { _reportAssemblyName = value; }
        }
        int IReport.reportID
        {
            get { return _reportID; }
            set { _reportID = value; }
        }
        int IReport.reportButtons
        {
            get { return (int)EnumReportButtons.ERB_Excel + (int)EnumReportButtons.ERB_Word; }
        }

        void IReport.reportInit(object inMainApp, DataGridViewGGControl.DataGridViewGGControl inMainGridControl, DBSqlServer inDBSqlServer, RFCManager inRFCManager)
        {
            _MainApp = inMainApp;
            _MainForm = (Form)_MainApp;
            _dbsql = inDBSqlServer;
            _dgvcontrol = inMainGridControl;
            _rfc = inRFCManager;
            InitPropertyGrid();
        }

        void IReport.reportDeinit() { }

        private Boolean UserCheck(Dictionary<string, string> inParams)
        {
            Boolean check = false;
            if (inParams["MONTHS1"] == "")
            {
                MessageBox.Show("Не указана месяц с", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                check = true;
                return check;
            }
            if (inParams["MONTHS2"] == "")
            {
                MessageBox.Show("Не указана месяц по", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                check = true;
                return check;
            }

            return check;
        }

        void IReport.reportGetDataToExcel(Dictionary<string, string> inParams)
        {
            if (UserCheck(inParams)) //если параметр не указан
                return;

            // MessageBox.Show(inParams["userfio"]);

            //  _dgvcontrol.dataGridViewGG.AutoGenerateColumns = false;
            _dgvcontrol.dataGridViewGG.Columns.Clear();

            BindingSource bs = new BindingSource();
           // _dbsql.SQLScript = "select top 1000 p.DATS, p.CODE_MO, p.FIO, p.IMA, p.OTCH, p.DATR, p.ISTI, p.SUMMA_I, p.PV from P p where p.months between 1 and 1";
             _dbsql.SQLScript = "exec report2 @code_mo = @code_mo_, @fio = @fio_, @ima = @ima_, @datrf = @datrf_, @datrl = @datrl_, @months1 = @months1_, @months2 = @months2_";

           _dbsql.AddParameter("@code_mo_", EnumDBDataTypes.EDT_String, inParams["CODE_MO"].Replace("'", ""));
            _dbsql.AddParameter("@fio_", EnumDBDataTypes.EDT_String, inParams["FAM"]);
            _dbsql.AddParameter("@ima_", EnumDBDataTypes.EDT_String, inParams["IMA"]);

            _dbsql.AddParameter("@datrf_", EnumDBDataTypes.EDT_DateTime, inParams["DATRF"]);
            _dbsql.AddParameter("@datrl_", EnumDBDataTypes.EDT_DateTime, inParams["DATRL"]);

            _dbsql.AddParameter("@months1_", EnumDBDataTypes.EDT_Integer, inParams["MONTHS1"]);
            _dbsql.AddParameter("@months2_", EnumDBDataTypes.EDT_Integer, inParams["MONTHS2"]);

            DataTable dt = _dbsql.FillDataSetVisual("выборка персональных счетов").Tables[0];
            bs.DataSource = dt;
            _dgvcontrol.dataGridViewGG.DataSource = bs;

            _dgvcontrol.dataGridViewGG.GGAddColumn("DATS", "Дата реестра", typeof(DateTime));
            _dgvcontrol.dataGridViewGG.GGAddColumn("CODE_MO", "ЛПУ", typeof(String));
            _dgvcontrol.dataGridViewGG.GGAddColumn("FIO", "Фамилия", typeof(String));
            _dgvcontrol.dataGridViewGG.GGAddColumn("IMA", "Имя", typeof(String));
            _dgvcontrol.dataGridViewGG.GGAddColumn("OTCH", "Отчество", typeof(String));
            _dgvcontrol.dataGridViewGG.GGAddColumn("DATR", "Дата рождения", typeof(DateTime));
            _dgvcontrol.dataGridViewGG.GGAddColumn("ISTI", "История", typeof(String));
            _dgvcontrol.dataGridViewGG.GGAddColumn("SUMMA_I", "Сумма", typeof(String));
            _dgvcontrol.dataGridViewGG.GGAddColumn("PV", "прична", typeof(String));

            _dgvcontrol.RefreshFilterFieldsGG();

            //создаем ексел
            _excel = new ExcelManager(Application.StartupPath + _templatename);
            //_excel.AddValueToWorkSheet("C3", inParams["datereport"]);
            //массив данных, который начинается с столбца №2, т.е. указав номер столбца, можно исключить первые стоблцы
            int indexStartColumn = 1; //если например из результирующего набора не выводить Первый столбец ID
            object[,] DataArray = null;
            DataArray = new object[dt.Rows.Count, dt.Columns.Count - indexStartColumn];
            int i = 0;
            foreach (DataRow rw in dt.Rows)
            {
                for (int k = 0; k < dt.Columns.Count - indexStartColumn; ++k)
                {
                    DataArray[i, k] = rw[dt.Columns[k + indexStartColumn].ColumnName].ToString();
                }
                ++i; //в массиве начинаю заполнять новую строку
            }
            //перенести массива в Excel
            //стартовая ячейка
            int indexFirstExcelRow = 7; //    7
            int indexFirstExcelColumn = 1; // A
            string Range__ = CExcelRanges.Ranges[indexFirstExcelColumn - 1] + indexFirstExcelRow.ToString() +   //начало диапазона  J10   -1 т.к.массив с 0
                             ":" +
                             CExcelRanges.Ranges[(indexFirstExcelColumn - 1 + dt.Columns.Count - indexStartColumn) - 1] + ((dt.Rows.Count + indexFirstExcelRow) - 1).ToString();  //конец диапазона
            _excel.SetValueToRange(Range__, DataArray);
            _excel.SetBorderStyle(Range__, BorderLineStyle.xlContinuous);
            //сохраняем выходим
            _excel.SaveDocument("c:\\temp\\report2.xls", true, ESaveFormats.xlNormal);
            MessageBox.Show("Отчет сформирован и сохранён в каталоге c:\\temp\\", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            _excel.Visible = true;
        }

        void IReport.reportGetDataToWord(Dictionary<string, string> inParams)
        {
            string rr = "";
            foreach (var pair in inParams)
            {
                rr = rr + " \n " + pair.Key + "   " + pair.Value.ToString();
            }
            MessageBox.Show(rr);

        }

        PropertyGridEx.PropertyGridEx IReport.reportGetParameters
        {
            get { return _propertyGrid; }
        }

        CInspectorManager IReport.reportGetInspector
        {
            get { return _oCInspectorManager; }
        }

        private void InitPropertyGrid()
        {
            try
            {
                _propertyGrid.ShowCustomProperties = true;
                _propertyGrid.ToolbarVisible = false;
                _propertyGrid.Item.Clear();
                _propertyGrid.Tag = _reportAssemblyName;

                _oCInspectorManager = new CInspectorManager(ref _propertyGrid, ref _dbsql, ref _rfc);

                _oCInspectorManager.Add(EInspectorModes.Reference, EInspectorTypes.TypeOfReferenceInt,
                                            "1.ЛПУ", "Поле", "Справочник ЛПУ (множественный выбор)", "code_mo", "", "rfclpu", false);

                _oCInspectorManager.Add(EInspectorModes.ListBox, EInspectorTypes.TypeOfListInt,
                                            "2.Месяц с", "Поле", "Сгенерированный список", "months1",
                                            " select 1 as id, 'январь' as name " +
                                            " union select 2 as id, 'февраль' as name " +
                                            " union select 3 as id, 'март' as name " +
                                            " union select 4 as id, 'апрель' as name " +
                                            " union select 5 as id, 'май' as name " +
                                            " union select 6 as id, 'июнь' as name " +
                                            " union select 7 as id, 'июль' as name " +
                                            " union select 8 as id, 'август' as name " +
                                            " union select 9 as id, 'сентябрь' as name " +
                                            " union select 10 as id, 'октябрь' as name " +
                                            " union select 11 as id, 'ноябрь' as name " +
                                            " union select 12 as id, 'декабрь' as name ",
                                            "");

                _oCInspectorManager.Add(EInspectorModes.ListBox, EInspectorTypes.TypeOfListInt,
                                            "3.Месяц по", "Поле", "Сгенерированный список", "months2",
                                            " select * from viewMonths ",
                                            "");

                _oCInspectorManager.Add(EInspectorModes.Simple, EInspectorTypes.TypeOfStringLike,
                                            "4.Фамилия", "Поле", "Фамилия пациента", "FAM", "", "");

                _oCInspectorManager.Add(EInspectorModes.Simple, EInspectorTypes.TypeOfStringLike,
                                            "5.Имя", "Поле", "Имя пациента", "IMA", "", "");

                _oCInspectorManager.Add(EInspectorModes.ExpandType, EInspectorTypes.TypeOfExpandDate,
                                            "6.Диапазон ДР", "Поле", "Диапазон дат рождения", "Datr", "", "");

                _propertyGrid.Refresh();
            }
            catch (Exception ex)
            {
                throw new Exception(_reportAssemblyName + " Ошибка инициализации параметров отчета \n" + ex.Message);
            }
        }

    }
}
