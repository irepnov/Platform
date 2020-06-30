using System;
using System.Windows.Forms;
using GGPlatform.BuiltInApp;
using GGPlatform.DBServer;
using GGPlatform.RFCManag;

namespace plugin_1
{
    public partial class formPlugin1 : Form, GGPlatform.BuiltInApp.IPlugin
    {
        object _MainApp = null;
        Form _MainForm = null;
        int _PluginID;
        int _WorkplaceID;
        string _PluginAssembly, _PluginName, _SoftName = "";
        DelegClosePlugin _ClosePlugin;
        DBSqlServer _dbsql = null;
        RFCManager _rfc = null;
        private BindingSource bs = new BindingSource();

        public formPlugin1() 
        {      
            InitializeComponent();
        }

        int IPlugin.plugPluginButtons { get { return (int)EnumPluginButtons.EPB_Refresh; } }

        ToolStrip IPlugin.plugToolStrip { get { return toolStrip1; } }

        void IPlugin.plugInit(object MainApp, DBSqlServer inDBSqlServer, RFCManager inRFCManager, ref Lists inActiveLists)
        {
            _MainApp = MainApp;
            _MainForm = (Form)_MainApp;
            _dbsql = inDBSqlServer;
            _rfc = inRFCManager;

          //  MessageBox.Show(" plugin1  init " + _dbsql.Connection.State.ToString());

            this.MdiParent = (Form)_MainApp;
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
         //   MessageBox.Show(" plugin1  deinit ");
        }

        void IPlugin.appListAdd()
        {
            //    MessageBox.Show(" plugin1  refresh ");
        }

        void IPlugin.appListDel()
        {
            //   MessageBox.Show(" plugin1  refresh filter ");
        }


        void IPlugin.appRefresh()
        {
        //    MessageBox.Show(" plugin1  refresh ");
        }

        void IPlugin.appRefreshFilter()
        {
         //   MessageBox.Show(" plugin1  refresh filter ");
        }

        void IPlugin.appOption() { MessageBox.Show("плагин 1 Option"); }

        private void mySubMenu1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("мое меню1, плагин 1");
        }

        private void mySubMenu2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("мое меню2, плагин 1");
        }

        int IPlugin.plugWorkplaceID { get { return _WorkplaceID; } 
                                      set { _WorkplaceID = value; } }
        int IPlugin.plugPluginID    { get { return _PluginID; }
                                      set { _PluginID = value; } }
        DelegClosePlugin IPlugin.plugEventClosed { set { _ClosePlugin = value; } }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            _ClosePlugin(_WorkplaceID, _PluginID);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*
                DataTable _dt = null;
                string SQLBase = "select top 10 * from rfcokogu";
                _dbsql.SQLScript = SQLBase;
                _dt = _dbsql.FillDataSet().Tables[0];
                dataGridView1.AutoGenerateColumns = true;
                dataGridView1.DataSource = _dt;
            */
            _dgvcontrol.dataGridViewGG.GGRegistryAssemblyName = _PluginAssembly + "_settings_";            
            _dbsql.SQLScript = "exec testSelect";
           // _dbsql.SQLScript = "select top 0 * from LPULicense";
            //_dbsql.SQLScript = "select top 10000 * from P where fio like 'е%' or fio not like 'е%'";

            bs.DataSource = _dbsql.FillDataSetVisual("Тестовый визуальный запрос данных kld dfh ksldhl kjhskdfshg hdh dh").Tables[0];
            //bs.DataSource = _dbsql.FillDataSet().Tables[0];
            _dgvcontrol.dataGridViewGG.Columns.Clear();
            _dgvcontrol.dataGridViewGG.DataSource = bs;

            _dgvcontrol.dataGridViewGG.GGAddColumn("ns", "Наименование показателя", typeof(String));
            _dgvcontrol.dataGridViewGG.GGAddColumn("sn", "Описание", typeof(String));
            _dgvcontrol.dataGridViewGG.GGAddColumn("dats", "Ключ показателя", typeof(String));
            _dgvcontrol.dataGridViewGG.GGAddColumn("fio", "Значение показателя", typeof(String));
            _dgvcontrol.dataGridViewGG.GGAddColumn("ima", "Значение показателя", typeof(String));
            _dgvcontrol.dataGridViewGG.GGAddColumn("otch", "Значение показателя", typeof(String));
            _dgvcontrol.RefreshFilterFieldsGG();

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            button1_Click(null, null);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
           // _rfc = new RFCManager(_MainForm, _dbsql);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _rfc.LoadRFC("rfcOKOGU");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            _rfc.ShowRFC("rfcOKOGU");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            _rfc.UnLoadRFC("rfcOKOGU");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            _rfc.RefreshRFC("rfcOKOGU");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            MessageBox.Show(_rfc.GetRFCValueOne("rfcokogu", "code"));
        }

        private void button9_Click(object sender, EventArgs e)
        {
            MessageBox.Show(_rfc.GetRFCValueMulti("rfcokogu", "code"));
        }

        private void button10_Click(object sender, EventArgs e)
        {
            MessageBox.Show(_rfc.GetRFCValueArrayList("rfcokogu", "code").Count.ToString());
        }

        private void button11_Click(object sender, EventArgs e)
        {
            _rfc.SetRFCValue("rfcokogu", "code", "11000");
        }

        private void button12_Click(object sender, EventArgs e)
        {
            _rfc.SetRFCValue("rfcokogu", "code", "'11000','12000','13000'");
        }

        private void button13_Click(object sender, EventArgs e)
        {
          //  dataGridView1.Rows[3].Selected = true;
          //  dataGridView1.CurrentCell = dataGridView1[0, 3];
        }

        private void button16_Click(object sender, EventArgs e)
        {
            //_dbsql.SQLScript = "select top 1000 id_p, ns, sn, dats, fio, pol, summa_i from [dbo].[P]";
            //ADODB.Recordset rs;
            //rs = _dbsql.ConvertToRecordset(_dbsql.FillDataSet().Tables[0]);
            //int col = 0;
            //col = rs.RecordCount;
        }

        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void button15_Click(object sender, EventArgs e)
        {
            _dbsql.SQLScript = "exec testExec";
            _dbsql.ExecuteNonQueryVisual("gggg gfhfgh gfhfgh fgh");
        }

       
    }
}