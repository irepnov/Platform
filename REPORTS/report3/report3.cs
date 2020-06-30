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

namespace report3
{
    public class Class1 : GGPlatform.BuiltInApp.IReport
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
        private string _templatename = "\\reports\\assembly\\" + "report3.xlt";


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
            /*
            if (inParams["DATEREPORT"] == "")
            {
                MessageBox.Show(_MainForm, "Не указана дата", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                check = true;
            }
            */

            return check;
        }

        void IReport.reportGetDataToExcel(Dictionary<string, string> inParams)
        {
            if (UserCheck(inParams)) //если параметр не указан
                return;

            StatusStrip stat = null;
            ToolStripProgressBar prog = null;
            ToolStripStatusLabel text = null;
            if (_MainForm != null)
            {
                Control[] c = _MainForm.Controls.Find("statusStripContainer", true);
                if (c != null && c.Length > 0)
                {
                    stat = (StatusStrip)c[0];
                    prog = (ToolStripProgressBar)(stat.Items[4]);
                    text = (ToolStripStatusLabel)(stat.Items[3]);
                }
            }

            string[] els = inParams["CODE_MO"].Replace("'", "").Replace(" ", "").Split(',');
            if (els != null && els.Length > 0)
            {
                if (prog != null)
                {
                    prog.Value = 0;
                    prog.Minimum = 0;
                    prog.Maximum = els.Length;
                    prog.Visible = true;
                }
                if (text != null)
                {
                    text.Text = "Выполняется формирование отчётов, ожидайте...";
                    Application.DoEvents();
                }
            }


            _excel = new ExcelManager(Application.StartupPath + _templatename);          
            //на каждое лпу по листу
            foreach (string item in els)
            {
                _excel.CopyWorkSheet(item, "Шаблон");//нумеруются с 1
                _excel.SetValueToRange("B2", item);
                if (prog != null) prog.Value += 1;
            }

            //сводный лист
            _excel.SetWorkSheetByName = "Свод";
            int i = 2;
            foreach (string item in els)
            {
                _excel.SetValueToRange("B"+i.ToString(), "=" + item + "!B2");
                i += 1;
            }

            if (prog != null)
            {
                prog.Value = prog.Maximum;
                prog.Visible = false;
            }
            if (text != null)
            {
                text.Text = "";
            }
            stat = null;
            prog = null;
            text = null;

            //сохраняем выходим           
            if (_excel.SaveDocument("c:\\temp\\report3.xls", true, ESaveFormats.xlNormal))
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
            _oCInspectorManager.Add(EInspectorModes.Reference, EInspectorTypes.TypeOfReferenceInt,
                                    "1.Мед. орг-я", "Поле", "Выберите необходимые медицинские организации", "code_mo", "", "rfclpu", true);

            _propertyGrid.Refresh();
        }
            catch (Exception ex)
            {
                throw new Exception(_reportAssemblyName + " Ошибка инициализации параметров отчета \n" + ex.Message);
    }
}

    }
}
