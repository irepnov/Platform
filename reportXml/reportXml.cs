using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Windows.Forms;
using System.IO;

using GGPlatform.DBServer;
using GGPlatform.RFCManag;
using GGPlatform.BuiltInApp;
using GGPlatform.InspectorManagerSP;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace GGPlatform.reportXml
{
    internal class param
    {
        public string DisplayMode { get; set; }
        public string ValueType { get; set; }
        public string DisplayName { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string SQLScript { get; set; }
        public string RFCName { get; set; }
        public string RFCMultiSelect { get; set; }
    }
    internal class reportConfig
    {
        public string Language { get; set; }
        public string Template { get; set; }
        public IEnumerable<param> Parameters { get; set; }
        public string ExecuteScript { get; set; }
    }

    public class reportXml: GGPlatform.BuiltInApp.IReport
    {
        private Form _MainForm = null;
        private DBSqlServer _dbsql = null;
        private RFCManager _rfc = null;

        private string _reportAssemblyName;
        private int _reportID;
        private PropertyGridEx.PropertyGridEx _propertyGrid = new PropertyGridEx.PropertyGridEx();
        private CInspectorManager _oCInspectorManager = null;
        private reportConfig _reportConfig = null;
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
            _rfc = inRFCManager;            
            if (String.IsNullOrEmpty(_reportAssemblyName))
                throw new Exception("Не указан конфигурационный файл отчета");
            ReadReportConfig();         
            InitPropertyGrid();
        }
        void IReport.reportDeinit() { }
        void IReport.reportGetDataToExcel(Dictionary<string, string> inParams)
        {
            ExecuteReport(inParams);            
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
                //проверю уникальность имен параметров
                var _doubleparam = _reportConfig.Parameters.GroupBy(r => r.Name)
                                                           .Select(s => new { nameparam = s.Key, countparam = s.Distinct().Count() })
                                                           .Where(h => h.countparam > 1).ToList();
                string _r = string.Join(",", _doubleparam.Select(i => i.nameparam).ToArray());
                if (!String.IsNullOrEmpty(_r))
                    throw new Exception("В конфигурационном файле отчета указаны дублирующиеся параметры " + _r);

                _propertyGrid.ShowCustomProperties = true;
                _propertyGrid.ToolbarVisible = false;
                _propertyGrid.Item.Clear();
                _propertyGrid.Tag = _reportAssemblyName;

                _oCInspectorManager = new CInspectorManager(ref _propertyGrid, ref _dbsql, ref _rfc);
                foreach(param p in _reportConfig.Parameters)
                {
                    EInspectorModes _modes;
                    EInspectorTypes _types;
                    bool _select = false;
                    #region Setting TypesModes
                      switch (p.DisplayMode.ToUpper())
                      {
                          case "SIMPLE": _modes = EInspectorModes.Simple;
                              break;
                          case "LISTBOX": _modes = EInspectorModes.ListBox;
                              break;
                          case "REFERENCE": _modes = EInspectorModes.Reference;
                              break;
                          case "EXPANDTYPE": _modes = EInspectorModes.ExpandType;
                              break;
                          default: _modes = EInspectorModes.Simple;
                              break;
                      }

                      switch (p.ValueType.ToUpper())
                      {
                          case "TYPEOFINT": _types = EInspectorTypes.TypeOfInt;
                              break;
                          case "TYPEOFDOUBLE": _types = EInspectorTypes.TypeOfDouble;
                              break;
                          case "TYPEOFSTRING": _types = EInspectorTypes.TypeOfString;
                              break;
                          case "TYPEOFDATE": _types = EInspectorTypes.TypeOfDate;
                              break;
                          case "TYPEOFLISTINT": _types = EInspectorTypes.TypeOfListInt;
                              break;
                          case "TYPEOFLISTSTRING": _types = EInspectorTypes.TypeOfListString;
                              break;
                          case "TYPEOFEXPANDINT": _types = EInspectorTypes.TypeOfExpandInt;
                              break;
                          case "TYPEOFEXPANDDATE": _types = EInspectorTypes.TypeOfExpandDate;
                              break;
                          case "TYPEOFREFERENCEINT": _types = EInspectorTypes.TypeOfReferenceInt;
                              break;
                          case "TYPEOFREFERENCESTRING": _types = EInspectorTypes.TypeOfReferenceString;
                              break;
                          case "TYPEOFSTRINGLIKE": _types = EInspectorTypes.TypeOfStringLike;
                              break;
                          default: _types = EInspectorTypes.TypeOfString;
                              break;
                      }

                      if (p.RFCMultiSelect.ToUpper().Contains("TRUE"))
                          _select = true;
                      #endregion

                    _oCInspectorManager.Add(_modes, _types, 
                                            p.DisplayName, p.Category, 
                                            p.Description, p.Name,
                                            p.SQLScript, p.RFCName,                                              
                                            _select);
                }
                _propertyGrid.Refresh();
            }
            catch
            {
                if (_oCInspectorManager != null)
                    _oCInspectorManager = null;

                throw;
            }
        }
        private Boolean ReadReportConfig()
        {
            Boolean _result = false;
            XDocument doc = null;
            _reportConfig = new reportConfig();
            
            try
            {
                if (!File.Exists(Application.StartupPath + "\\reports\\xml\\" + _reportAssemblyName))
                    throw new Exception("Конфигурационный файл отчета " + _reportAssemblyName + " не найден");

                doc = XDocument.Load(Application.StartupPath + "\\reports\\xml\\" + _reportAssemblyName);
                _reportConfig.Parameters = from c in doc.Descendants("param")
                                              select new param()
                                              {
                                                  DisplayMode = (string)c.Attribute("DisplayMode"),
                                                  ValueType = (string)c.Attribute("ValueType"),
                                                  DisplayName = (string)c.Attribute("DisplayName"),
                                                  Category = (string)c.Attribute("Category"),
                                                  Description = (string)c.Attribute("Description"),
                                                  Name = (string)c.Attribute("Name"),
                                                  SQLScript = (string)c.Attribute("SQLScript"),
                                                  RFCName = (string)c.Attribute("RFCName"),
                                                  RFCMultiSelect = (string)c.Attribute("RFCMultiSelect")
                                              };

                _reportConfig.Language = doc.Descendants().Where(n => n.Name == "language").Attributes("name").FirstOrDefault().Value;
                _reportConfig.Template = doc.Descendants().Where(n => n.Name == "template").Attributes("filename").FirstOrDefault().Value;
                _reportConfig.ExecuteScript = doc.DescendantNodes().Where(n => n.NodeType == System.Xml.XmlNodeType.CDATA).FirstOrDefault().Parent.Value;
               // doc = null;

                if ((_reportConfig.Language.ToUpper() != "VBSCRIPT") /*&& (_reportConfig.Language.ToUpper() != "JSCRIPT")*/)
                    throw new Exception("Неподдерживаемый язык отчета " + _reportConfig.Language);

                if (String.IsNullOrEmpty(_reportConfig.Template))
                    throw new Exception("Не указан шаблон отчета");

                if (!File.Exists(Application.StartupPath + "\\reports\\xml\\" + _reportConfig.Template))
                    throw new Exception("Шаблон отчета " + _reportConfig.Template + " не найден");

                if (String.IsNullOrEmpty(_reportConfig.ExecuteScript.Replace(" ", "")))
                    throw new Exception("Не указан сценарий отчета");

                if (!_reportConfig.ExecuteScript.Contains("ReportEngine"))
                    throw new Exception("В сценарии отчета отсутствует процедура ReportEngine");

                _result = true;
            }
            catch
            {
                if (doc != null)
                    doc = null;

                _result = false;
                throw; 
            }
            finally
            {
                if (doc != null)
                    doc = null;
            }
            return _result;
        }
        private Boolean ExecuteReport(Dictionary<string, string> _inParams)
        {
            ////////////////////СБОРКА ДОЛЖНА БЫТЬ обязательно х86  иначе MSScriptControl не работает
			////////////////http://www.excelworld.ru/forum/2-2227-1
            Boolean _result = false;
            MSScriptControl.ScriptControlClass script = new MSScriptControl.ScriptControlClass();
            int _paramsColRow = 0;

            try
            {                  
                script.AllowUI = true;
                script.Language = _reportConfig.Language;
                script.SitehWnd = (Int32)_MainForm.Handle;

                #region компаную переменные
                //скомпаную переменные отчета и подставлю их внутрь скрипта
                string _params = null;
                foreach(KeyValuePair<string, string> k in _inParams)
                {
                    if (_reportConfig.Language.ToUpper() == "VBSCRIPT")
                    {
                        if (k.Value == "")
                        {
                            _params = _params + "dim " + k.Key + "\n";
                            _paramsColRow = _paramsColRow + 1;
                        }
                        else
                        {
                            _params = _params + "dim " + k.Key + "\n";

                            int amount = 0;
                            amount = new Regex("[']").Matches(k.Value).Count;
                            if (amount >= 2)   //если выбрали из справочника, то там будут значения в одинарных кавычках
                                _params = _params + k.Key + " = \"(" + k.Value + ")\"\n"; //mont = "555"        lpu = "('jjj', 'hhhh')"                            
                            else
                                _params = _params + k.Key + " = \"" + k.Value + "\"\n"; //mont = "555"        lpu = "('jjj', 'hhhh')"
                            _paramsColRow = _paramsColRow + 2;
                        }
                    }                       

                    if (_reportConfig.Language.ToUpper() == "JSCRIPT")
                    {
                        if (k.Value == "")
                        {
                            _params = _params + "var " + k.Key + ";\n";
                            _paramsColRow = _paramsColRow + 1;
                        }
                        else
                        {
                            _params = _params + "var " + k.Key + ";\n";

                            int amount = 0;
                            amount = new Regex("[']").Matches(k.Value).Count;
                            if (amount >= 2)   //если выбрали из справочника, то там будут значения в одинарных кавычках
                                _params = _params + k.Key + " = \"(" + k.Value + ")\";\n"; //mont = "555"        lpu = "('jjj', 'hhhh')"                            
                            else
                                _params = _params + k.Key + " = \"" + k.Value + "\";\n"; //mont = "555"        lpu = "('jjj', 'hhhh')"
                            _paramsColRow = _paramsColRow + 2;
                        }
                    }
                }
                //системная перемененная
                _params = _params + "dim TEMPLATE \n";
                _params = _params + "TEMPLATE = \"" + Application.StartupPath + "\\reports\\xml\\" + _reportConfig.Template + "\"\n";
                _paramsColRow = _paramsColRow + 2;              

                #endregion
                string _exec = null;
                _exec = _params + _reportConfig.ExecuteScript;

                /*  script.AddCode(
                                  @"function test
                                    if (1=1) then 
                                      test = true
                                    else 
                                      test = false
                                    end if
                                  end function

                                  sub exeScript
                                  test
                                  MsgBox 1
                                  end sub
                                  "
                                );
                  object[] myParams = { };
                  script.Run("exeScript", ref myParams);*/

                script.AddObject("rfcmanag", _rfc, false);  //добавлем оюъекты, у них обязательно должны стоять опция СДЕЛАТЬ ВИДИМОЙ ДЛЯ СОМ
                script.AddObject("dbserver", _dbsql, false);  //добавлем оюъекты, у них обязательно должны стоять опция СДЕЛАТЬ ВИДИМОЙ ДЛЯ СОМ
                script.AddCode(_exec);  //добавлем код и он сразу выполняется
                
                script = null;
                _result = true;
            }
            catch (Exception tt)
            { //
                string err = script.Error.Source + "\n" +
                             "строка № " + (script.Error.Line - _paramsColRow).ToString() + "\n" + 
                             "текст: " + script.Error.Text.Trim() + "\n" +
                             "исключение: " + tt.Message;
                script = null;         
                _result = false;
                throw new Exception(err);
            }
            finally
            {
                script = null;                                
            }          
            return _result;
        }
    }
}
