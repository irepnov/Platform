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

namespace report1
{
    public class Class1: GGPlatform.BuiltInApp.IReport
    {
        private Form _MainForm = null;
        private DBSqlServer _dbsql = null;
        private DataGridViewGGControl.DataGridViewGGControl _dgvcontrol = null;
        private RFCManager _rfc = null;

        private string _reportAssemblyName;
        private int _reportID;
        private PropertyGridEx.PropertyGridEx _propertyGrid = new PropertyGridEx.PropertyGridEx();
        private CInspectorManager _oCInspectorManager = null;
        private ExcelManager _excel = null;
        private string _templatename = "\\reports\\assembly\\" + "report1.xlt";


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
            get { return (int)EnumReportButtons.ERB_Excel; } 
        }

        void IReport.reportInit(object inMainApp, DataGridViewGGControl.DataGridViewGGControl inMainGridControl, DBSqlServer inDBSqlServer, RFCManager inRFCManager)
        {
            _MainForm = (Form)inMainApp;
            _dbsql = inDBSqlServer;
            _dgvcontrol = inMainGridControl;
            _rfc = inRFCManager;
            InitPropertyGrid();
        }

        void IReport.reportDeinit() { }

        private Boolean UserCheck(Dictionary<string, string> inParams)
        {
            Boolean check = false;
            if (inParams["DATEREPORT"] == "")
            {
                MessageBox.Show(_MainForm, "Не указана дата", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                check = true;
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
            _dbsql.SQLScript = "exec report1 @date = @d";
            _dbsql.AddParameter("@d", EnumDBDataTypes.EDT_DateTime, inParams["DATEREPORT"]);
            DataTable dt = _dbsql.FillDataSetVisual("ggggggdf dfgdfgdfg").Tables[0];
            bs.DataSource = dt;
            _dgvcontrol.dataGridViewGG.DataSource = bs;

            _dgvcontrol.dataGridViewGG.GGAddColumn("ID", "ИД", typeof(Int32));
            _dgvcontrol.dataGridViewGG.GGAddColumn("Code", "Код", typeof(String));
            _dgvcontrol.dataGridViewGG.GGAddColumn("NameShort", "Наименование сокращенное", typeof(String));
            _dgvcontrol.dataGridViewGG.GGAddColumn("OGRN", "ОГРН", typeof(String));
            _dgvcontrol.dataGridViewGG.GGAddColumn("INN", "ИНН", typeof(String));
            _dgvcontrol.dataGridViewGG.GGAddColumn("OKOPFCode", "ОКОПФ Код", typeof(String));
            _dgvcontrol.dataGridViewGG.GGAddColumn("OKOPFName", "ОКОПФ наименование", typeof(String));
            _dgvcontrol.dataGridViewGG.GGAddColumn("DateBegin", "Дата начала", typeof(DateTime));

            _dgvcontrol.RefreshFilterFieldsGG();
           
            //создаем ексел
            _excel = new ExcelManager(Application.StartupPath + _templatename);
            _excel.SetCalculation(ECalculation.xlCalculationManual);
            _excel.SetScreenUpdating(false);
            _excel.SetValueToRange("C3", inParams["DATEREPORT"]);            
            //массив данных, который начинается с столбца №2, т.е. указав номер столбца, можно исключить первые стоблцы
            int indexStartColumn = 1; //если например из результирующего набора не выводить Первый столбец ID
            object[,] DataArray = _excel.GetDataArrayFromDataTable(dt, indexStartColumn, _MainForm);
            //перенести массива в Excel
            //стартовая ячейка
            int indexFirstExcelRow = 7; //    7
            int indexFirstExcelColumn = 1; // A            
            string _Range = CExcelRanges.Ranges[indexFirstExcelColumn - 1] + indexFirstExcelRow.ToString() +   //начало диапазона  J10   -1 т.к.массив с 0
                            ":" +
                            CExcelRanges.Ranges[(indexFirstExcelColumn - 1 + dt.Columns.Count - indexStartColumn) - 1] + ((dt.Rows.Count + indexFirstExcelRow) - 1).ToString();  //конец диапазона
            _excel.SetValueToRange(_Range, DataArray);            
            _excel.SetFormatToFindInRange("A" + indexFirstExcelRow.ToString() + 
                                          ":" + 
                                          "A" + ((dt.Rows.Count + indexFirstExcelRow) - 1).ToString(), 
                                          "77", 
                                          "A:H", 15, false, 
                                          _MainForm);
            _excel.SetBorderStyle(_Range, BorderLineStyle.xlContinuous);
            _excel.SetScreenUpdating(true);
            DataArray = null;
            //сохраняем выходим           
            if (_excel.SaveDocument("c:\\temp\\report1.xls", true, ESaveFormats.xlNormal)) 
                MessageBox.Show(_MainForm, "Отчет сформирован и сохранён в каталоге c:\\temp\\", "Информация", 
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
            _excel.Visible = true;
        }

        void IReport.reportGetDataToWord(Dictionary<string, string> inParams)
        {
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
            _oCInspectorManager.Add(EInspectorModes.Simple, EInspectorTypes.TypeOfDate,
                                    "1. Дата", "Параметры формирования отчета", "Дата запроса", "datereport", "", "");

            _propertyGrid.Refresh();
        }
            catch (Exception ex)
            {
                throw new Exception(_reportAssemblyName + " Ошибка инициализации параметров отчета \n" + ex.Message);
    }
}

    }
}
