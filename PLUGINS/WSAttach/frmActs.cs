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

namespace WSAttach
{
    public partial class frmActs : Form
    {
        private DBSqlServer _dbsql = null;
        private RFCManager _rfc = null;
        private CInspectorManager _insp = null;
        private string _SoftName = "";
        private string _PluginAssembly = "";
        public frmActs(object[] inParams)
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
                                        "Дата актов сверки", "Поле", "Дата актов сверки", "a.dateact", "", "");
            _insp.Add(EInspectorModes.ListBox, EInspectorTypes.TypeOfListInt,
                                        "Тип актов сверки", "Поле", "Тип актов сверки", "a.typeact",
                                        "select 1 as id, 'акты сверки с АПУ' as name union select 2 as id, 'акты сверки с СМП' as name", "");
            propertyGridEx1.Refresh();
        }


        private void RefreshData()
        {
            Dictionary<string, string> _dict = null;

            if (_insp != null)
                _dict = _insp.GetDictionaryValues();

            string _sql;
            string _dateact;
            string _typeact;

            _sql = _dateact = _typeact = "";

            _dict.TryGetValue("A.DATEACT", out _dateact);
            _dict.TryGetValue("A.TYPEACT", out _typeact);

            if (_typeact == "" || _typeact == null)
            {
                MessageBox.Show("Необходимо указать тип актов сверки численности",
                                 "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (_typeact == "1")
            {
                _sql = @"select a.id, a.dateact, a.dateactattach, a.typeact, 
                           lur.code as urcode, lur.nameshort as urname, l.code as strcode, l.nameshort as strname,
	                       a.all_count, a.c0_1G, a.c0_1M, a.c1_4G, a.c1_4M, a.c5_17G, a.c5_17M, a.c18_54G, a.c18_59M, a.c55G, a.c60M,
	                       a.t0_1G, a.t0_1M, a.t1_4G, a.t1_4M, a.t5_17G, a.t5_17M, a.t18_54G, a.t18_59M, a.t55G, a.t60M,
	                       a.s0_1G, a.s0_1M, a.s1_4G, a.s1_4M, a.s5_17G, a.s5_17M, a.s18_54G, a.s18_59M, a.s55G, a.s60M
                    from [WSAttach].[AttachConsolidateAct] a
                     inner join sprLPU l on l.code = a.code_mo
                     inner join sprLPU lur on lur.code = l.urcode
                    where a.typeact = 1";
            }

            if (_typeact == "2")
            {
                _sql = @"select a.id, a.dateact, a.dateactattach, a.typeact, 
                           l.code as urcode, l.nameshort as urname, lstr.code as strcode, lstr.nameshort as strname,
	                       a.all_count, a.c0_1G, a.c0_1M, a.c1_4G, a.c1_4M, a.c5_17G, a.c5_17M, a.c18_54G, a.c18_59M, a.c55G, a.c60M,
	                       a.t0_1G, a.t0_1M, a.t1_4G, a.t1_4M, a.t5_17G, a.t5_17M, a.t18_54G, a.t18_59M, a.t55G, a.t60M,
	                       a.s0_1G, a.s0_1M, a.s1_4G, a.s1_4M, a.s5_17G, a.s5_17M, a.s18_54G, a.s18_59M, a.s55G, a.s60M
                    from [WSAttach].[AttachConsolidateAct] a
                     inner join sprLPU l on l.code = a.code_mo_smp
                     inner join sprLPU lstr on lstr.code = a.code_mo
                    where a.typeact = 2";
            }


            if (_dateact != null && _dateact != "") _sql = _sql + String.Format(" and a.Dateact = '{0}'", _dateact);
            _sql = _sql + " order by 2, 5, 7";

            dgControl.dataGridViewGG.AllowUserToAddRows = false;
            dgControl.dataGridViewGG.AllowUserToDeleteRows = false;
            dgControl.dataGridViewGG.AllowUserToOrderColumns = true;
            dgControl.dataGridViewGG.AutoGenerateColumns = false;
            dgControl.dataGridViewGG.MultiSelect = false;
            dgControl.dataGridViewGG.GGRegistrySoftName = _SoftName;
            dgControl.dataGridViewGG.GGRegistryAssemblyName = _PluginAssembly + "_" + "acts";
            dgControl.dataGridViewGG.Columns.Clear();
            dgControl.dataGridViewGG.GGAddColumn("dateact", "Дата актов сверки", typeof(string)).ReadOnly = true;
            dgControl.dataGridViewGG.GGAddColumn("dateactattach", "Дата прикрепленного населения", typeof(string)).ReadOnly = true;
            dgControl.dataGridViewGG.GGAddColumn("urcode", "Код МО", typeof(string)).ReadOnly = true;
            dgControl.dataGridViewGG.GGAddColumn("urname", "Наименование МО", typeof(string)).ReadOnly = true;
            dgControl.dataGridViewGG.GGAddColumn("strcode", "Код МО [прикрепления]", typeof(string)).ReadOnly = true;
            dgControl.dataGridViewGG.GGAddColumn("strname", "Наименование МО [прикрепления]", typeof(string)).ReadOnly = true;
            dgControl.dataGridViewGG.GGAddColumn("all_count", "Всего прикреплено", typeof(string)).ReadOnly = true;

            dgControl.dataGridViewGG.GGAddColumn("c0_1G", "Кол-во Ж 0-1", typeof(int)).ReadOnly = true;
            dgControl.dataGridViewGG.GGAddColumn("c0_1M", "Кол-во М 0-1", typeof(int)).ReadOnly = true;
            dgControl.dataGridViewGG.GGAddColumn("c1_4G", "Кол-во Ж 1-4", typeof(int)).ReadOnly = true;
            dgControl.dataGridViewGG.GGAddColumn("c1_4M", "Кол-во М 1-4", typeof(int)).ReadOnly = true;
            dgControl.dataGridViewGG.GGAddColumn("c5_17G", "Кол-во Ж 5-17", typeof(int)).ReadOnly = true;
            dgControl.dataGridViewGG.GGAddColumn("c5_17M", "Кол-во М 5-17", typeof(int)).ReadOnly = true;
            dgControl.dataGridViewGG.GGAddColumn("c18_54G", "Кол-во Ж 18-54", typeof(int)).ReadOnly = true;
            dgControl.dataGridViewGG.GGAddColumn("c18_59M", "Кол-во М 18-59", typeof(int)).ReadOnly = true;
            dgControl.dataGridViewGG.GGAddColumn("c55G", "Кол-во Ж 55>", typeof(int)).ReadOnly = true;
            dgControl.dataGridViewGG.GGAddColumn("c60M", "Кол-во М 60>", typeof(int)).ReadOnly = true;

            dgControl.dataGridViewGG.GGAddColumn("t0_1G", "Тариф Ж 0-1", typeof(int)).ReadOnly = true;
            dgControl.dataGridViewGG.GGAddColumn("t0_1M", "Тариф М 0-1", typeof(int)).ReadOnly = true;
            dgControl.dataGridViewGG.GGAddColumn("t1_4G", "Тариф Ж 1-4", typeof(int)).ReadOnly = true;
            dgControl.dataGridViewGG.GGAddColumn("t1_4M", "Тариф М 1-4", typeof(int)).ReadOnly = true;
            dgControl.dataGridViewGG.GGAddColumn("t5_17G", "Тариф Ж 5-17", typeof(int)).ReadOnly = true;
            dgControl.dataGridViewGG.GGAddColumn("t5_17M", "Тариф М 5-17", typeof(int)).ReadOnly = true;
            dgControl.dataGridViewGG.GGAddColumn("t18_54G", "Тариф Ж 18-54", typeof(int)).ReadOnly = true;
            dgControl.dataGridViewGG.GGAddColumn("t18_59M", "Тариф М 18-59", typeof(int)).ReadOnly = true;
            dgControl.dataGridViewGG.GGAddColumn("t55G", "Тариф Ж 55>", typeof(int)).ReadOnly = true;
            dgControl.dataGridViewGG.GGAddColumn("t60M", "Тариф М 60>", typeof(int)).ReadOnly = true;

            dgControl.dataGridViewGG.GGAddColumn("s0_1G", "Стоим Ж 0-1", typeof(int)).ReadOnly = true;
            dgControl.dataGridViewGG.GGAddColumn("s0_1M", "Стоим М 0-1", typeof(int)).ReadOnly = true;
            dgControl.dataGridViewGG.GGAddColumn("s1_4G", "Стоим Ж 1-4", typeof(int)).ReadOnly = true;
            dgControl.dataGridViewGG.GGAddColumn("s1_4M", "Стоим М 1-4", typeof(int)).ReadOnly = true;
            dgControl.dataGridViewGG.GGAddColumn("s5_17G", "Стоим Ж 5-17", typeof(int)).ReadOnly = true;
            dgControl.dataGridViewGG.GGAddColumn("s5_17M", "Стоим М 5-17", typeof(int)).ReadOnly = true;
            dgControl.dataGridViewGG.GGAddColumn("s18_54G", "Стоим Ж 18-54", typeof(int)).ReadOnly = true;
            dgControl.dataGridViewGG.GGAddColumn("s18_59M", "Стоим М 18-59", typeof(int)).ReadOnly = true;
            dgControl.dataGridViewGG.GGAddColumn("s55G", "Стоим Ж 55>", typeof(int)).ReadOnly = true;
            dgControl.dataGridViewGG.GGAddColumn("s60M", "Стоим М 60>", typeof(int)).ReadOnly = true;

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

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            frmActsOption fmOption = new frmActsOption(new object[] { _dbsql, 1 });
            fmOption.ShowDialog();
            fmOption.Dispose();
            fmOption = null;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void дляМедОрганизацийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmActsOption fmOption = new frmActsOption(new object[] { _dbsql, 2 });
            fmOption.ShowDialog();
            fmOption.Dispose();
            fmOption = null;
        }

        private void дляПКМЭКToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmActsOption fmOption = new frmActsOption(new object[] { _dbsql, 3 });
            fmOption.ShowDialog();
            fmOption.Dispose();
            fmOption = null;
        }
    }
}
