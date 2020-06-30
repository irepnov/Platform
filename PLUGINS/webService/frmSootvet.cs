using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GGPlatform.DBServer;
using GGPlatform.ExcelManagers;

namespace webService
{
    public partial class frmSootvet : Form
    {
        private DBSqlServer _dbsql = null;
        private DataTable _dtEtalon = null;
        private DataTable _dtEmpty = null;
        private string _PostavRef = "";
        private bool _check = false;
        private string _proizv = "не задано";

        public frmSootvet(object[] inParams)
        {
            InitializeComponent();
            _dbsql = (DBSqlServer)inParams[0];
            _dtEmpty = (DataTable)inParams[1];

            _PostavRef = inParams[2].ToString();
            _check = (bool)inParams[3];
          //  FillEtalon();
            CreateGrid();

         /*   var graphics = CreateGraphics();
            comboBoxColumn.DropDownWidth = (from width in
                         (from DataRow item in _dtEtalon.Rows
                          select Convert.ToInt32(graphics.MeasureString(item["Name"].ToString(), Font).Width))
                                            select width).Max() + 10;*/
        }

        private void CreateGrid()
        {
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter;

            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ColumnCount = 3;

            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);

            dataGridView1.Columns[0].HeaderText = "ID поля";
            dataGridView1.Columns[0].Width = 10;
            dataGridView1.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridView1.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView1.Columns[0].DefaultCellStyle.BackColor = DataGridView.DefaultBackColor;
            dataGridView1.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView1.Columns[0].Visible = false;

            dataGridView1.Columns[1].HeaderText = "Наименование товара поставщика";
            dataGridView1.Columns[1].Width = 350;
            dataGridView1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridView1.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView1.Columns[1].DefaultCellStyle.BackColor = DataGridView.DefaultBackColor;
            dataGridView1.Columns[1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            dataGridView1.Columns[2].HeaderText = "Соответствующее родное наименование товара";
            dataGridView1.Columns[2].Width = 400;
            dataGridView1.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridView1.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView1.Columns[2].DefaultCellStyle.BackColor = DataGridView.DefaultBackColor;

            foreach (DataRow dr in _dtEmpty.Rows)
            {
                DataGridViewRow row = new DataGridViewRow();
                DataGridViewTextBoxCell cell0 = new DataGridViewTextBoxCell();
                DataGridViewTextBoxCell cell1 = new DataGridViewTextBoxCell();
                DataGridViewComboBoxCell cell2 = new DataGridViewComboBoxCell();

                FillEtalon(dr["Name"].ToString());
                cell2.Items.AddRange(_dtEtalon.Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray());

                cell0.Value = dr["id"].ToString();
                cell1.Value = dr["Name"].ToString();
                row.Cells.Add(cell0);
                row.Cells.Add(cell1);
                row.Cells.Add(cell2);

                cell0.ReadOnly = true;
                cell1.ReadOnly = true;

                dataGridView1.Rows.Add(row);
            }
        }

        private void FillEtalon(string inModel)
        {
            if (!inModel.Contains(_proizv)) //если модель от другого производителя, то выбираю список Эталонных моделей
            {
                //узнаю производителя
                string[] words = inModel.Split(' ');
                string sql = "";
                foreach (string word in words)
                {
                    sql = sql + " or Proizv like '%" + word + "%'";
                }
                if (sql.Substring(0, 3) == " or")
                    sql = sql.Substring(4, sql.Length - 4);  //уберу первый And
                _dbsql.SQLScript = "select top 1 Proizv from uchEtalonNomen where " + sql;
                string thisProizv = _dbsql.ExecuteScalar();

                //выберу все модели по производителю
                _dbsql.SQLScript = "exec uchPostavNomenc @IDOperation = 6, @PostavRef = @post, @isIntellect = @ch, @proizv = @pr";            
                _dbsql.AddParameter("@post", EnumDBDataTypes.EDT_Integer, _PostavRef);
                _dbsql.AddParameter("@ch", EnumDBDataTypes.EDT_Bit, _check.ToString());
                _dbsql.AddParameter("@pr", EnumDBDataTypes.EDT_String, thisProizv);
                _dtEtalon = _dbsql.FillDataSet().Tables[0];

                _proizv = thisProizv; //сохраню производителя
            }
        }

        private bool UserCheck()
        {
            /* bool _result = false;
             try
             {
                 _result = true;

                 foreach(DataGridViewRow drv in dataGridView1.Rows)
                 {
                     if ((drv.Cells[2].Value == null) || (drv.Cells[2].Value.ToString() == ""))
                         throw new Exception("Не весь товар сопоставлен");
                 }                
             }
             catch (Exception ex)
             {
                 foreach (DataGridViewRow drv in dataGridView1.Rows)
                 {
                     _dbsql.SQLScript = @"update tmpPostavNomen 
                                          set RodnName = null,
                                              RodnNomenRef = null
                                          where id = @id";
                     _dbsql.AddParameter("@id", EnumDBDataTypes.EDT_Integer, drv.Cells[0].Value.ToString());
                     _dbsql.ExecuteNonQuery();
                 }

                 _result = false;
                 MessageBox.Show(ex.Message, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
             }
             return _result;*/
            return true;
        }

        private void ExportXlsNotSoot()
        {
            _dbsql.SQLScript = "select distinct name from tmpPostavNomen where RodnName is null";
            DataTable _dt = _dbsql.FillDataSet().Tables[0];

            if ((_dt == null) || (_dt.Rows.Count < 1))
                return;

            ExcelManager _excel = new ExcelManager();
            try
            {
                _excel.SetWorkSheetByIndex = 1;
                _excel.SetValueToRange("A1", "Список несопоставленной номенклатуры поставщика");
                _excel.SetFontBold("A1");
                _excel.SetColumnWidth("A:A", 100);
                _excel.SetCalculation(ECalculation.xlCalculationManual);
                _excel.SetScreenUpdating(false);
                //массив данных, который начинается с столбца №2, т.е. указав номер столбца, можно исключить первые стоблцы
                int indexStartColumn = 0; //если например из результирующего набора не выводить Первый столбец ID
                object[,] DataArray = _excel.GetDataArrayFromDataTable(_dt, indexStartColumn, null);
                //перенести массива в Excel
                //стартовая ячейка
                int indexFirstExcelRow = 2; //    3
                int indexFirstExcelColumn = 1; // A               

                string _Range = CExcelRanges.Ranges[indexFirstExcelColumn - 1] + indexFirstExcelRow.ToString() +   //начало диапазона  J10   -1 т.к.массив с 0
                                ":" +
                                CExcelRanges.Ranges[(indexFirstExcelColumn - 1 + _dt.Columns.Count - indexStartColumn) - 1] + ((_dt.Rows.Count + indexFirstExcelRow) - 1).ToString();  //конец диапазона
                _excel.SetValueToRange(_Range, DataArray);
                _excel.SetBorderStyle(_Range, BorderLineStyle.xlContinuous);
                _excel.SetScreenUpdating(true);
                DataArray = null;
                _dt = null;
                //сохраняем выходим           
                if (_excel.SaveDocument("c:\\temp\\uchNotSopostPostNom__" + DateTime.Now.Date.ToShortDateString().Replace(".", "_") + "_" + _PostavRef + ".xls", true, ESaveFormats.xlNormal))
                    MessageBox.Show("Отчет сформирован и сохранён в каталоге c:\\temp\\", "Информация",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                _excel.CloseExcelDocumentAndShow();
            }
            catch
            {
                _excel.CloseDocument();
                _excel = null;
                _dt = null;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (!UserCheck())
                return;

            foreach (DataGridViewRow drv in dataGridView1.Rows)
            {
                if ((drv.Cells[2].Value != null) && (drv.Cells[2].Value.ToString() != ""))
                {
                    _dbsql.SQLScript = @"update tmpPostavNomen 
                                         set RodnName = e.name,
                                             RodnNomenRef = e.id
                                         from tmpPostavNomen t
                                          inner join uchRodnNomen e on e.Name = @et
                                         where t.id = @id";
                    _dbsql.AddParameter("@et", EnumDBDataTypes.EDT_String, drv.Cells[2].Value.ToString());
                    _dbsql.AddParameter("@id", EnumDBDataTypes.EDT_Integer, drv.Cells[0].Value.ToString());
                    _dbsql.ExecuteNonQuery();
                }
            }

            ExportXlsNotSoot();
            this.DialogResult = DialogResult.OK;
        }
    }
}
