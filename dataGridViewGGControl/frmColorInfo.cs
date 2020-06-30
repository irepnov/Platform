using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GGPlatform.DBServer;

namespace DataGridViewGGControl
{
    public partial class frmColorInfo : Form
    {
        private DBSqlServer _dbsql = null;
        private Dictionary<string, string> _dict = null;

        public frmColorInfo(DBSqlServer inDBSql, Dictionary<string, string> inDict)
        {
            _dbsql = inDBSql;
            _dict = inDict;
            InitializeComponent();
        }

        private void dgv_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                string tmp = ((DataGridView)sender).Columns[e.ColumnIndex].DataPropertyName;
                if (tmp.ToUpper().Contains("COLOR"))
                {
                    DataRowView dr = (DataRowView)((DataGridView)sender).Rows[e.RowIndex].DataBoundItem;
                    e.CellStyle.BackColor = Color.White;

                    if (dr == null)
                        return;

                    switch (dr[tmp].ToString())
                    {
                        case "A": { e.CellStyle.BackColor = Color.AntiqueWhite; } break;
                        case "M": { e.CellStyle.BackColor = Color.Aquamarine; } break;
                        case "L": { e.CellStyle.BackColor = Color.Lavender; } break;
                        case "V": { e.CellStyle.BackColor = Color.Violet; } break;
                        case "W": { e.CellStyle.BackColor = Color.BurlyWood; } break;
                        case "P": { e.CellStyle.BackColor = Color.Pink; } break;
                        case "K": { e.CellStyle.BackColor = Color.DarkKhaki; } break;
                        case "O": { e.CellStyle.BackColor = Color.DarkOrange; } break;
                        case "S": { e.CellStyle.BackColor = Color.DarkSalmon; } break;
                        case "G": { e.CellStyle.BackColor = Color.DarkSeaGreen; } break;
                        case "T": { e.CellStyle.BackColor = Color.DarkTurquoise; } break;
                        case "Y": { e.CellStyle.BackColor = Color.Yellow; } break;
                        case "R": { e.CellStyle.BackColor = Color.OrangeRed; } break;
                        default: { e.CellStyle.BackColor = Color.White; } break;
                    }
                }
            }
        }

        private void frmColorInfo_Load(object sender, EventArgs e)
        {
            tabControl1.TabPages.Clear();

            if (_dict == null)
                return;

            foreach (KeyValuePair<string, string> kvp in _dict)
            {
                TabPage newTabPage = new TabPage(kvp.Key);
                newTabPage.Name = kvp.Key;

                DataGridViewGG.DataGridViewGG dgv = dgv = new DataGridViewGG.DataGridViewGG();
                dgv.Name = "ll";
                dgv.ReadOnly = true;
                dgv.AllowUserToAddRows = false;
                dgv.AllowUserToDeleteRows = false;
                dgv.AllowUserToOrderColumns = true;
                dgv.AutoGenerateColumns = false;
                dgv.MultiSelect = false;
                dgv.AutoGenerateColumns = false;
                //dgv.RowHeadersVisible = false;
                dgv.CellPainting += dgv_CellPainting;

                string sql = "";
                string capt = "";
                int objID = 0;

                _dbsql.SQLScript = "select * from GGPlatform.Objects where objectname = '" + kvp.Value + "'";
                SqlDataReader _dr = _dbsql.ExecuteReader();
                while (_dr.Read())
                {
                    sql = _dr["ObjectExpression"].ToString();
                    capt = _dr["ObjectCaption"].ToString();
                    objID = (int)_dr["ID"];
                }
                _dr.Close();
                newTabPage.Text = capt;

                _dbsql.SQLScript = sql;
                dgv.DataSource = _dbsql.FillDataSet().Tables[0];



                _dbsql.SQLScript = "select ID, isnull(FieldAlias, FieldName) as FieldName, FieldCaption, FieldType, FieldVisible from GGPlatform.ObjectsDescription where ObjectsRef = @obj";
                _dbsql.AddParameter("@obj", EnumDBDataTypes.EDT_Integer, objID.ToString());
                SqlDataReader ObjectsDescription = _dbsql.ExecuteReader();
                while (ObjectsDescription.Read())
                {
                    Type t = null;

                    if (ObjectsDescription["FieldType"].ToString() == "L")
                    {
                        t = typeof(Boolean);
                    }
                    else
                        if (ObjectsDescription["FieldType"].ToString() == "N")
                        {
                            t = typeof(Int32);
                        }
                        else
                        {
                            t = typeof(String);
                        }

                    dgv.GGAddColumn(ObjectsDescription["FieldName"].ToString(),
                                    ObjectsDescription["FieldCaption"].ToString(),
                                    t).Visible = (bool)ObjectsDescription["FieldVisible"];
                }
                ObjectsDescription.Close();

                dgv.Dock = DockStyle.Fill;
                newTabPage.Controls.Add(dgv);

                tabControl1.Controls.Add(newTabPage);
            }


        }
    }
}
