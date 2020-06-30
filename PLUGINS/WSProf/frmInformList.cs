using GGPlatform.DBServer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WSProf
{
    public partial class frmInformList : Form
    {

        private DBSqlServer _dbsql = null;
        private string _templatename = "\\reports\\template\\";
        private string _metod { get; set; }
        private string _step { get; set; }

        private string _rslt { get; set; }
        public DateTime _date { get; set; }
        private List<DataRow> _list;

        public frmInformList(object[] inParams)
        {
            InitializeComponent();

            _dbsql = (DBSqlServer)inParams[0];
            _list = (List<DataRow>)inParams[1];
        }

        public bool UserCheck()
        {
            bool _result = false;
            try
            {
                _result = true;

                if (cbMetod.Text == "")
                {
                    ActiveControl = cbMetod;
                    throw new Exception("Необходимо указать метод информирования");
                }

                if (cbStep.Text == "")
                {
                    ActiveControl = cbStep;
                    throw new Exception("Необходимо указать этап информирования");
                }

                if (cbRslt.Text == "")
                {
                    ActiveControl = cbRslt;
                    throw new Exception("Необходимо указать результат информирования");
                }

                if (dtDate.Text == "")
                {
                    ActiveControl = dtDate;
                    throw new Exception("Необходимо указать дату информирования");
                }

                _metod = Convert.ToInt16(cbMetod.Text.Substring(0, 2)).ToString();
                _step = Convert.ToInt16(cbStep.Text.Substring(0, 3)).ToString();
                _rslt = Convert.ToInt16(cbRslt.Text.Substring(0, 4)).ToString();
                _date = dtDate.Value;
            }
            catch (Exception ex)
            {
                _result = false;
                MessageBox.Show(ex.Message, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return _result;
            //проверки
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!UserCheck())
                return;

            Cursor = Cursors.WaitCursor;
            button1.Enabled = false;

            foreach(DataRow _r in _list)
            {
                _dbsql.SQLScript = "exec [WSProf].[spFactInfos] @IDOper = 7, @kind = @k, @PeopleID = @pid, @infodate = @d, @infometh = @m, @infostep = @s, @code_mo = @mo, @InfoRslt = @rs";
                _dbsql.AddParameter("@k", EnumDBDataTypes.EDT_Integer, _r["kindID"].ToString());
                _dbsql.AddParameter("@pid", EnumDBDataTypes.EDT_Integer, _r["peopleID"].ToString());
                _dbsql.AddParameter("@d", EnumDBDataTypes.EDT_DateTime, _date.ToShortDateString());
                _dbsql.AddParameter("@m", EnumDBDataTypes.EDT_String, _metod);
                _dbsql.AddParameter("@s", EnumDBDataTypes.EDT_String, _step);
                _dbsql.AddParameter("@mo", EnumDBDataTypes.EDT_String, _r["code_mo"].ToString());
                _dbsql.AddParameter("@rs", EnumDBDataTypes.EDT_String, _rslt);
                _dbsql.ExecuteNonQuery();
            }

            Cursor = Cursors.Default;
            button1.Enabled = true;
            MessageBox.Show("Ручное информирование выбранных записей проведено", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void cbMetod_SelectionChangeCommitted(object sender, EventArgs e)
        {
            // select right('0' + cast(code as varchar(5)), 4) + ' - '+ name as name, def, infometh from wsprof.inforslt
            cbRslt.Items.Clear();
            _dbsql.SQLScript = "select right('0' + cast(code as varchar(5)), 4) + ' - '+ name as name, def, infometh from wsprof.inforslt where InfoMeth = @met";
            _dbsql.AddParameter("@met", EnumDBDataTypes.EDT_Integer, cbMetod.Text.Substring(0, 2));
            DataTable _dt = _dbsql.FillDataSet().Tables[0];
            string _def = "";
            foreach (DataRow _r in _dt.Rows)
            {
                cbRslt.Items.Add(_r["name"].ToString());
                if ((bool)_r["def"]) _def = _r["name"].ToString();
            }
            cbRslt.Text = _def;
        }
    }
}
