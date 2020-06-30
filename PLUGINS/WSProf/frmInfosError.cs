using GGPlatform.DBServer;
using GGPlatform.InspectorManagerSP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using GGPlatform.RFCManag;
using System.Windows.Forms;

namespace WSProf
{
    public partial class frmInfosError : Form
    {
        private DBSqlServer _dbsql = null;
        private RFCManager _rfc = null;
        private CInspectorManager _insp = null;
        private string _SoftName = "";
        private string _PluginAssembly = "";
        public frmInfosError(object[] inParams)
        {
            InitializeComponent();

            _dbsql = (DBSqlServer)inParams[0];
            _rfc = (RFCManager)inParams[1];
            _SoftName = inParams[2].ToString();
            _PluginAssembly = inParams[3].ToString();
            LoadInsp();
        }

        private void LoadInsp()
        {
            propertyGridEx1.ShowCustomProperties = true;
            propertyGridEx1.Item.Clear();
            _insp = new CInspectorManager(ref propertyGridEx1, ref _dbsql, ref _rfc);
            
            _insp.Add(EInspectorModes.Simple, EInspectorTypes.TypeOfDate,
                                        "Дата информирования", "Поле", "Дата информирования", "i.InfoDate", "", "");
            _insp.Add(EInspectorModes.ListBox, EInspectorTypes.TypeOfListInt,
                                        "Метод информирования", "Поле", "Используемый метод информирования", "i.InfoMeth",
                                        "select id, code + ' ' + name as name from WSProf.InfoMeth", "");
            _insp.Add(EInspectorModes.ListBox, EInspectorTypes.TypeOfListInt,
                                        "Этап информирования", "Поле", "Проведенный этап информирования", "i.InfoStep",
                                        "select id, code + ' ' + name as name from WSProf.InfoStep", "");
            _insp.Add(EInspectorModes.Simple, EInspectorTypes.TypeOfDate,
                                        "Дата отправки", "Поле", "Дата отправки сведений в ТФОМС", "i.ExportDate", "", "");
            _insp.Add(EInspectorModes.ListBox, EInspectorTypes.TypeOfListInt,
                                        "Код ошибки", "Поле", "Код ошибки, возвращенный ТФОМС", "r.ID",
                                        "select id, cast(code as varchar(3)) + ' ' + name as name from WSProf.ReasonReturns", "");
            /*   _insp.Add(EInspectorModes.Reference, EInspectorTypes.TypeOfReferenceInt,
                                           "4.ОКОГУ", "Поле", "Справочник ОКОГУ (множественный выбор)", "RFCOKOGU", "", "rfcOKOGU", true);
               _insp.Add(EInspectorModes.Reference, EInspectorTypes.TypeOfReferenceInt,
                                           "5.ОКОПФ", "Поле", "Справочник ОКОПФ (разовый выбор)", "RFCOKOPF", "", "rfcOKOPF");
               _insp.Add(EInspectorModes.Simple, EInspectorTypes.TypeOfStringLike,
                                           "6.Фамилия", "Поле", "Фамилия пациента", "FAM", "", "");
               _insp.Add(EInspectorModes.ExpandType, EInspectorTypes.TypeOfExpandDate,
                                           "7.Диапазон ДР", "Поле", "Диапазон дат рождения", "Datr", "", "");
               _insp.Add(EInspectorModes.Simple, EInspectorTypes.TypeOfDate,
                                           "8.Дата загрузки", "Поле", "Дата загрузки", "CreateDate", "", "");
               _insp.Add(EInspectorModes.Simple, EInspectorTypes.TypeOfInt,
                                           "9.№ записи", "Поле", "№ записи", "RecordNumber", "", "");
               _insp.Add(EInspectorModes.ExpandType, EInspectorTypes.TypeOfExpandInt,
                                           "9.Диапазон № случая", "Поле", "Диапазон № случая", "CaseID", "", "");
               _insp.Add(EInspectorModes.Reference, EInspectorTypes.TypeOfReferenceInt,
                                           "10.ЛПУ", "Поле", "Справочник ЛПУ (множественный выбор)", "RFClpu", "", "rfclpu", true);*/
            propertyGridEx1.Refresh();
        }


        private void RefreshData()
        {
            Dictionary<string, string> _dict = null;

            if (_insp != null)
                _dict = _insp.GetDictionaryValues();

            string _sql;
            string _infodate;
            string _infostep;
            string _infometh;
            string _infoexport;
            string _err;
            string _where;

            _sql = _infodate = _infostep = _infometh = _infoexport = _where = _err = "";

            _dict.TryGetValue("I.INFODATE", out _infodate);
            _dict.TryGetValue("I.INFOMETH", out _infometh);
            _dict.TryGetValue("I.INFOSTEP", out _infostep);
            _dict.TryGetValue("I.EXPORTDATE", out _infoexport);
            _dict.TryGetValue("R.ID", out _err);

            _sql = @"set dateformat dmy select i.id, i.InfoDate, i.Infometh, i.infostep, i.exportdate, i.smo_code, p.enp, p.fam, p.im, p.otch, p.datr, m.name as mname, s.name as sname, r.id as rid, r.code as rcode, r.name as rname, r.description as rdesc
                    from [WSProf].factinfos i
                     inner join [WSProf].[People] p on p.id = i.PeopleID
                     inner join [WSProf].[InfoMeth] m on m.code = i.infometh
                     inner join [WSProf].[InfoStep] s on s.code = i.infostep
                     inner join [WSProf].[FactInfosErrors] e on e.FactInfoID = i.id
                     inner join [WSProf].[ReasonReturns] r on r.id = e.ReasonReturnID ";
            _where = " where ";

            if (_infodate != null && _infodate != "") _where = _where + String.Format(" and i.InfoDate = '{0}'", _infodate);
            if (_infometh != null && _infometh != "") _where = _where + String.Format(" and i.Infometh = '{0}'", _infometh);
            if (_infostep != null && _infostep != "") _where = _where + String.Format(" and i.infostep = '{0}'", _infostep);
            if (_infoexport != null && _infoexport != "") _where = _where + String.Format(" and i.exportdate = '{0}'", _infoexport);
            if (_err != null && _err != "") _where = _where + String.Format(" and r.id = '{0}'", _err);

            _where = _where.Replace("where  and ", "where ");

            if (_where.Trim() != "where") _sql = _sql + _where;
            _sql = _sql + " order by p.fam, p.im, p.otch, p.datr, i.infodate";

            dgControl.dataGridViewGG.AllowUserToAddRows = false;
            dgControl.dataGridViewGG.AllowUserToDeleteRows = false;
            dgControl.dataGridViewGG.AllowUserToOrderColumns = true;
            dgControl.dataGridViewGG.AutoGenerateColumns = false;
            dgControl.dataGridViewGG.MultiSelect = false;
            dgControl.dataGridViewGG.GGRegistrySoftName = _SoftName;
            dgControl.dataGridViewGG.GGRegistryAssemblyName = _PluginAssembly + "_" + "Errors";
            dgControl.dataGridViewGG.Columns.Clear();
            dgControl.dataGridViewGG.GGAddColumn("enp", "ЕНП", typeof(string)).ReadOnly = true;
            dgControl.dataGridViewGG.GGAddColumn("fam", "Фамилия", typeof(string)).ReadOnly = true;
            dgControl.dataGridViewGG.GGAddColumn("im", "Имя", typeof(string)).ReadOnly = true;
            dgControl.dataGridViewGG.GGAddColumn("otch", "Отчество", typeof(string)).ReadOnly = true;
            dgControl.dataGridViewGG.GGAddColumn("datr", "Дата рождения", typeof(DateTime)).ReadOnly = true;

            dgControl.dataGridViewGG.GGAddColumn("mname", "Метод информирования", typeof(string)).ReadOnly = true;
            dgControl.dataGridViewGG.GGAddColumn("sname", "Этап информирования", typeof(string)).ReadOnly = true;
            dgControl.dataGridViewGG.GGAddColumn("InfoDate", "Дата информирования", typeof(DateTime)).ReadOnly = true;
            dgControl.dataGridViewGG.GGAddColumn("smo_code", "Код СМО", typeof(string)).ReadOnly = true;
            dgControl.dataGridViewGG.GGAddColumn("exportdate", "Дата отправки к ТФОМС", typeof(DateTime)).ReadOnly = true;
            dgControl.dataGridViewGG.GGAddColumn("rcode", "Код ошибки", typeof(string)).ReadOnly = true;
            dgControl.dataGridViewGG.GGAddColumn("rname", "Наименование ошибки", typeof(string)).ReadOnly = true;
            dgControl.dataGridViewGG.GGAddColumn("rdesc", "Описание ошибки", typeof(string)).ReadOnly = true;
            dgControl.RefreshFilterFieldsGG();
            dgControl.dataGridViewGG.GGLoadGridRegistry();

            _dbsql.SQLScript = _sql;
            DataTable _dt = null;
            _dt = _dbsql.FillDataSet().Tables[0];
            BindingSource _bs = new BindingSource();
            _bs.DataSource = _dt;
            dgControl.dataGridViewGG.DataSource = _bs;
        }
        private void frmInfosError_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                RefreshData();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            RefreshData();
        }
    }
}
