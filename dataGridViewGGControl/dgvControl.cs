using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;
//using ;
using GGPlatform.DBServer;

namespace DataGridViewGGControl
{
    public partial class DataGridViewGGControl : UserControl
    {
        private class ComboboxItem
        {
            public string FieldName { get; set; }
            public string FieldHeader { get; set; }

            public ComboboxItem(string inFieldName, string inFieldHeader)
            {
                FieldName = inFieldName;
                FieldHeader = inFieldHeader;
            }
        }

        private DBSqlServer _dbsql = null;
        public DataGridViewGG.DataGridViewGG dataGridViewGG { get; set; }
        public ToolStripTextBox tbTextGG { get; set; }
        public ToolStripComboBox cbFieldsGG { get; set; }
        private BindingSource bs = null;
        
        private Dictionary<string, string> arrColorColumns = null;
        public DBSqlServer DBSqlServerGG { set { _dbsql = value; } }
        private Int32 FreezeRow, FreezeCol = 0;


        public void AddColorColumnsGG(Dictionary<string, string> inarrColorColumns)
        {
            arrColorColumns = inarrColorColumns;
            btColorInfo.Enabled = true;//показываем, не показываем подсказки
        }

        public void RefreshFilterFieldsGG()
        {
            cbFields.Items.Clear();
            tbText.Text = "";
            foreach (DataGridViewColumn Column in dataGridViewGG1.Columns)
            {
                ///ошибка попадают поля типа ДАТА, т.к. они тоже стринг
                if (Column.ValueType.ToString() == "System.String")
                    cbFields.Items.Add(new ComboboxItem(Column.DataPropertyName, Column.HeaderText));
            }
            cbFieldsGG = cbFields;
        }

        public DataGridViewGGControl()
        {
            InitializeComponent();
            dataGridViewGG = dataGridViewGG1;
            tbTextGG = tbText;
            cbFieldsGG = cbFields;
            tbFreeze.Tag = "unfreezed";
            tbWrap.Tag = "notwrap";
            tbFreeze.ToolTipText = "Закрепить область";
         //   cbFieldsGG.
          //  cbFields.BackColor = Color.WhiteSmoke;
            tbText.BackColor = Color.WhiteSmoke;

            //if (bs != null)
            //{
            //    bs.ListChanged += (ee, s) => { toolStripStatusLabel1.Text = "listchanged " + bs.Count.ToString(); };
            //    bs.BindingComplete += (ee, s) => { toolStripStatusLabel1.Text = "complete " + bs.Count.ToString(); };
            //}
                

           //////// btColorInfo.Enabled = (arrColorColumns != null);  //показываем, не показываем подсказки
        }

        public void SetEnabledButtonExcel(bool inEnabled)
        {
            btExcel.Enabled = inEnabled;
        }

        public void SetEnabledButtonWord(bool inEnabled)
        {
            btWord.Enabled = inEnabled;
        }

        public void SetEnabledButtonHTML(bool inEnabled)
        {
            btHTML.Enabled = inEnabled;
        }

        public void SetEnabledButtonXML(bool inEnabled)
        {
            btXML.Enabled = inEnabled;
        }

        public void SetEnabledButtonWrap(bool inEnabled)
        {
            tbWrap.Enabled = inEnabled;
        }

        public void SetEnabledButtonFreeze(bool inEnabled)
        {
            tbFreeze.Enabled = inEnabled;
        }

        private void toolStrip1_Resize(object sender, EventArgs e)
        {
            int wid = toolStrip1.Width / 15;

            cbFields.Size = new Size((wid * 4), cbFields.Size.Height);
            
            tbText.Size = new Size(/*toolStrip1.Width - 340 - (wid * 3)*/  (wid * 9) - 150, tbText.Size.Height);
        }

        private void RowsCountTSL()
        {
            if (bs != null)
                tslRowsCount.Text = "записей " + bs.Count.ToString();
            else
                tslRowsCount.Text = "";
        }

        private void dataGridViewGG1_DataSourceChanged(object sender, EventArgs e)
        {
            bs = ((BindingSource)dataGridViewGG1.DataSource);
           /* if (bs != null)
            {
                bs.ListChanged += (ee, s) => 
                {
                    RowsCountTSL();
                };
            }*/
        }

        private void tbText_TextChanged(object sender, EventArgs e)
        {
            if (bs == null)
                return;

            if ((tbText.Text != "") && (cbFields.SelectedIndex > -1))
            {                
                bs.Filter = string.Format("{0} like '%{1}%'", (cbFields.SelectedItem as ComboboxItem).FieldName, tbText.Text);
            }
            else
            {
                bs.Filter = "";
            }
        }

        private void cbFields_SelectedIndexChanged(object sender, EventArgs e)
        {
            bs.Filter = "";
        }

        private void DataGridViewGGControl_Load(object sender, EventArgs e)
        {
            cbFields.ComboBox.DisplayMember = "FieldHeader";
            cbFields.ComboBox.ValueMember = "FieldName";           
        }

        private void btWord_Click(object sender, EventArgs e)
        {
            dataGridViewGG1.GGExportToWord();
        }

        private void btHTML_Click(object sender, EventArgs e)
        {
            dataGridViewGG1.GGExportToHTML();
        }

        private void btXML_Click(object sender, EventArgs e)
        {
            dataGridViewGG1.GGExportToXML();
        }

        private void btExcel_Click(object sender, EventArgs e)
        {
            dataGridViewGG1.GGExportToExcel();
        }

        private void tbWrap_Click(object sender, EventArgs e)
        {
            if (tbWrap.Tag.ToString() == "wrap")
            {
                dataGridViewGG1.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
                dataGridViewGG1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
                tbWrap.Tag = "notwrap";
                tbWrap.Image = Properties.Resources.tbWrap_Image;
                tbWrap.ToolTipText = "Переносить строки";
            }
            else
            {
                dataGridViewGG1.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dataGridViewGG1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
                tbWrap.Tag = "wrap";
                tbWrap.Image = Properties.Resources.tbWrapNot_Image;
                tbWrap.ToolTipText = "Отменить перенос строк";
            }
        }

        private void btColorInfo_Click(object sender, EventArgs e)
        {
            if (arrColorColumns == null)
                return;

            frmColorInfo fmColor = new frmColorInfo(_dbsql, arrColorColumns);
            fmColor.ShowDialog();
            fmColor.Dispose();
        }

        private void dataGridViewGG1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (arrColorColumns == null)  //закомментил, т.к. в Менеджере справочников не задаются ссылки на цветовые подсказки и соответственно Цветом не выделяется
                return;

            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
              {
                  string tmp = dataGridViewGG1.Columns[e.ColumnIndex].DataPropertyName;
                  if (arrColorColumns.ContainsKey(tmp))
                  {
                      DataRowView dr = (DataRowView)((DataGridView)sender).Rows[e.RowIndex].DataBoundItem;
                      e.CellStyle.BackColor = Color.White;

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

        private void tbFreeze_Click(object sender, EventArgs e)
        {
            if (tbFreeze.Tag.ToString() == "freezed")
            {
                dataGridViewGG1.Rows[0].Frozen = false;
                dataGridViewGG1.Columns[1].Frozen = false;

                FreezeBand(dataGridViewGG1.Rows[FreezeRow], false);
                FreezeBand(dataGridViewGG1.Columns[FreezeCol], false);

                tbFreeze.Image = Properties.Resources.tbFreeze_Image;
                tbFreeze.Tag = "unfreezed";
                tbFreeze.ToolTipText = "Закрепить область";
            } else 
            {
                if (dataGridViewGG1.Columns[dataGridViewGG1.CurrentCell.ColumnIndex].DataPropertyName.ToUpper() != "ISCHECKEDFIELD")
                {
                    FreezeRow = dataGridViewGG1.CurrentCell.RowIndex;
                    FreezeCol = dataGridViewGG1.CurrentCell.ColumnIndex;

                    tbFreeze.Image = Properties.Resources.btNotFreeze;
                    FreezeBand(dataGridViewGG1.Rows[FreezeRow], true);
                    FreezeBand(dataGridViewGG1.Columns[FreezeCol], true);

                    tbFreeze.Tag = "freezed";
                    tbFreeze.ToolTipText = "Снять закрепление области";
                }            
            }
        }

        private void dataGridViewGG1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (bs != null)
            {
                RowsCountTSL();
            }
        }

        private void statusStrip1_DoubleClick(object sender, EventArgs e)
        {
            MessageBox.Show("Имя табличного представления: " + dataGridViewGG1.GGObjectName);
        }

        private void FreezeBand(DataGridViewBand band, Boolean st)
        {            
            if (st)
            {
                band.Frozen = st;
                dataGridViewGG1.Rows[FreezeRow].Cells[FreezeCol].Style.BackColor = Color.LightSlateGray;
                band.DefaultCellStyle.BackColor = Color.LightSteelBlue;
            }
            else
            {
                dataGridViewGG1.Rows[FreezeRow].Cells[FreezeCol].Style.BackColor = Color.Empty;
                band.DefaultCellStyle.BackColor = Color.Empty;
            }
        }

    }
}
