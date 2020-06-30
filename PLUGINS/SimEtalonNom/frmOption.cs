using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GGPlatform.RegManager;
using GGPlatform.DBServer;

namespace SimEtalonNom
{
    public partial class frmOption : Form
    {
        private RegManag _reg = null;
        private string _p = "";
        private string _typenacenca = "";
        private DBSqlServer _dbsql = null;
        public frmOption(object[] inParams)
        {
            InitializeComponent();
            _reg = (RegManag)inParams[1];
            _p = inParams[2].ToString();
            _typenacenca = inParams[3].ToString();
            _dbsql = (DBSqlServer)inParams[4];

            if (_typenacenca == "PRICEPOSTAV")
                rbPricePostav.Checked = true;

            if (_typenacenca == "SEBESTFACT")
                rbSebest.Checked = true;

            LoadCon();          
        }

        private void LoadCon()
        {
            dataGridViewGG1.AllowUserToAddRows = false;
            dataGridViewGG1.AllowUserToDeleteRows = false;
            dataGridViewGG1.AllowUserToOrderColumns = true;
            dataGridViewGG1.AutoGenerateColumns = false;
            dataGridViewGG1.MultiSelect = false;

            dataGridViewGG1.CellEndEdit += (s, e) =>
            {
                string _newval = "";
                string _idrow = "";

                DataRow dr = ((DataRowView)dataGridViewGG1.CurrentRow.DataBoundItem).Row;

                _newval = dataGridViewGG1[e.ColumnIndex, e.RowIndex].Value.ToString();
                _idrow = dr["ID"].ToString();

                dr = null;

                Int32 _id;

                if (Int32.TryParse(_idrow, out _id))
                {
                    _dbsql.SQLScript = "update GGPlatform.ObjectsDescription set FieldCaption = @newv where id = @id";
                    _dbsql.AddParameter("@newv", EnumDBDataTypes.EDT_String, _newval.ToString());
                    _dbsql.AddParameter("@id", EnumDBDataTypes.EDT_Integer, _id.ToString());
                    _dbsql.ExecuteNonQuery();
                }
            };

            BindingSource bs = new BindingSource();

            _dbsql.SQLScript = @"select ID, FieldAlias, FieldCaption 
                                from GGPlatform.ObjectsDescription 
                                where ObjectsRef = (select ID from GGPlatform.Objects where ObjectName = 'EtalonNomen')
                                  and FieldName like 'e.Con%'";
            bs.DataSource = _dbsql.FillDataSet().Tables[0];
            dataGridViewGG1.Columns.Clear();
            dataGridViewGG1.DataSource = bs;

            DataGridViewColumn _col = null;
            _col = dataGridViewGG1.GGAddColumn("FieldAlias", "Константа", typeof(String));
            _col.ReadOnly = true;
            _col.Width = 80;

            _col = dataGridViewGG1.GGAddColumn("FieldCaption", "Наименование конкурента", typeof(String));
            _col.ReadOnly = false;
            _col.Width = 430;
        }

        private bool WriteOption()
        {
            bool _result = false;
            try
            {
                if (rbPricePostav.Checked)
                    _typenacenca = "PRICEPOSTAV";

                if (rbSebest.Checked)
                    _typenacenca = "SEBESTFACT";

                _reg.GGRegistrySetValue("TypeNacenca", _typenacenca, _p);

                _result = true;

                return _result;
            }
            catch
            {
                return _result;
            }
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            if ((!rbPricePostav.Checked) && (!rbSebest.Checked))
            {
                MessageBox.Show("Не заданы настройки автоматического расчета наценки", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if ((rbPricePostav.Checked) && (rbSebest.Checked))
            {
                MessageBox.Show("Некорректно заданы настройки автоматического расчета наценки", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!WriteOption())
            {
                MessageBox.Show("Ошибка сохранения настроек в реестре Windows", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.DialogResult = DialogResult.OK;
        }
    }
}
