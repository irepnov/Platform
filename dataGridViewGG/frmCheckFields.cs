using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Office.Interop;
using Excel = Microsoft.Office.Interop.Excel;
using Word = Microsoft.Office.Interop.Word;
using System.Runtime.InteropServices;

namespace DataGridViewGG
{
    public partial class frmCheckFields : Form
    {
      //  private Excel.Application ExcelApp = null;
      //  private Excel.Workbook ExcelWorkBook = null;
      //  private Excel.Worksheet ExcelWorkSheet = null;
        private DataGridView _dgv = null;
        private int _AppType = -1;  //1-Eksel   2-Word

        public frmCheckFields(DataGridView inDataGridView, int inOfficeAppType)
        {
            InitializeComponent();
            _dgv = inDataGridView;
            _AppType = inOfficeAppType;
            loadListBox();
        }

        public void loadListBox()
        {
            clb.Items.Clear();
            foreach (DataGridViewColumn column in _dgv.Columns)
            {
                if ((column.Visible) & (column.Name != "") & (column.Name.ToUpper() != "ISCHECKEDFIELD"))
                    clb.Items.Add(column.HeaderText, true);
            }
        }

        private void setStyle(Excel.Range range)
        {
            range.EntireColumn.AutoFit();
            range.EntireRow.AutoFit();
            object[] border = new object[] { Excel.XlBordersIndex.xlEdgeLeft, 
                                             Excel.XlBordersIndex.xlEdgeTop, 
                                             Excel.XlBordersIndex.xlEdgeBottom, 
                                             Excel.XlBordersIndex.xlEdgeRight,
                                             Excel.XlBordersIndex.xlInsideVertical,
                                             Excel.XlBordersIndex.xlInsideHorizontal };

            for (int i = 0; i < border.Length; i++)
            {
                range.Borders[(Excel.XlBordersIndex)border[i]].LineStyle = Excel.XlLineStyle.xlContinuous;
                range.Borders[(Excel.XlBordersIndex)border[i]].Weight = Excel.XlBorderWeight.xlThin;
                range.Borders[(Excel.XlBordersIndex)border[i]].ColorIndex = Excel.XlColorIndex.xlColorIndexAutomatic;
            }
        }

        private void ExcelExports()
        {
            ///заполним массив Заголовками
            var strFieldsHeader = new List<string>();
            foreach (object itemChecked in clb.CheckedItems)
                strFieldsHeader.Add(itemChecked.ToString());

            ///заполним массив Именами полей
            var strFieldsName = new List<string>();
            foreach (DataGridViewColumn column in _dgv.Columns)
            {
                if ((column.Visible) & (column.Name != "") & (column.Name.ToUpper() != "ISCHECKEDFIELD")) ///потому что в Лист боксе только видимые столбцы
                    foreach (string str in strFieldsHeader)
                        if (str.Equals(column.HeaderText))
                        {
                            strFieldsName.Add(column.Name);
                            break;
                        }
            }

            ////двумерный массив, заголовок + строки
            object[,] d = new object[_dgv.RowCount + 1, strFieldsName.Count];   //+1

            ///заголовки
            int c = 0;
            foreach (string str in strFieldsHeader)
            {
                d[0, c] = str;
                c++;
            }

            //строки
            for (int i = 0; i <= _dgv.RowCount - 1; i++)  //-1  <
            {
                for (int j = 0; j < strFieldsName.Count; j++)
                {
                    if ((_dgv.Columns[strFieldsName[j].ToString()].ValueType == typeof(Int32)) ||
                        (_dgv.Columns[strFieldsName[j].ToString()].ValueType == typeof(Int16)) ||
                        (_dgv.Columns[strFieldsName[j].ToString()].ValueType == typeof(Int64)))
                        try
                        { d[i + 1, j] = Convert.ToInt32(_dgv.Rows[i].Cells[strFieldsName[j].ToString()].Value.ToString()); }
                        catch
                        { d[i + 1, j] = 0; }
                    else
                        if (_dgv.Columns[strFieldsName[j].ToString()].ValueType == typeof(Decimal))
                            try
                            { d[i + 1, j] = Convert.ToDecimal(_dgv.Rows[i].Cells[strFieldsName[j].ToString()].Value.ToString()); }
                            catch
                            { d[i + 1, j] = 0; }
                        else
                            if (_dgv.Columns[strFieldsName[j].ToString()].ValueType == typeof(DateTime))
                                try
                                { d[i + 1, j] = "'" + _dgv.Rows[i].Cells[strFieldsName[j].ToString()].Value.ToString().Substring(0, 10); }
                                catch
                                { d[i + 1, j] = null; }
                            else
                                if (_dgv.Columns[strFieldsName[j].ToString()].ValueType == typeof(Boolean))
                                {
                                    string t = _dgv.Rows[i].Cells[strFieldsName[j].ToString()].Value.ToString();
                                    if (t != "")
                                        t = (t == "True") ? "Да" : "Нет";
                                    d[i + 1, j] = t;
                                }
                                else
                                    try
                                    { d[i + 1, j] = "'" + _dgv.Rows[i].Cells[strFieldsName[j].ToString()].Value.ToString(); }
                                    catch
                                    { d[i + 1, j] = null; }
                }
            }

            /* for (int i = 0; i <= _dgv.RowCount - 1; i++)  //-1  <
             {
                 for (int j = 0; j < strFieldsName.Count; j++)
                 {
                     if (_dgv.Columns[strFieldsName[j].ToString()].ValueType == typeof(String))
                         d[i + 1, j] = _dgv.Rows[i].Cells[strFieldsName[j].ToString()].Value.ToString();
                     else
                         if ((_dgv.Columns[strFieldsName[j].ToString()].ValueType == typeof(Int32)) ||
                             (_dgv.Columns[strFieldsName[j].ToString()].ValueType == typeof(Int16)) ||
                             (_dgv.Columns[strFieldsName[j].ToString()].ValueType == typeof(Int64)))
                             try
                             { d[i + 1, j] = Convert.ToInt32(_dgv.Rows[i].Cells[strFieldsName[j].ToString()].Value.ToString()); }
                             catch
                             { d[i + 1, j] = 0; }
                         else
                             if (_dgv.Columns[strFieldsName[j].ToString()].ValueType == typeof(Decimal))
                                 try
                                 { d[i + 1, j] = Convert.ToDecimal(_dgv.Rows[i].Cells[strFieldsName[j].ToString()].Value.ToString()); }
                                 catch
                                 { d[i + 1, j] = 0; }
                             else
                                 if (_dgv.Columns[strFieldsName[j].ToString()].ValueType == typeof(DateTime))
                                     try
                                     { d[i + 1, j] = Convert.ToDateTime((_dgv.Rows[i].Cells[strFieldsName[j].ToString()].Value.ToString().Substring(0, 10))); }
                                     catch
                                     { d[i + 1, j] = null; }
                                 else
                                     if (_dgv.Columns[strFieldsName[j].ToString()].ValueType == typeof(Boolean))
                                     {
                                         string t = _dgv.Rows[i].Cells[strFieldsName[j].ToString()].Value.ToString();
                                         if (t != "")
                                             t = (t == "True") ? "Да" : "Нет";
                                         d[i + 1, j] = t;
                                     }
                                     else d[i + 1, j] = _dgv.Rows[i].Cells[strFieldsName[j].ToString()].Value.ToString();
                 }
             }*/

            Excel.Application ExcelApp = new Excel.Application();  // создание нового Excel
            Excel.Workbook ExcelWorkBook = ExcelApp.Workbooks.Add(Type.Missing);  // новая книга
            Excel.Worksheet ExcelWorkSheet = (Excel.Worksheet)ExcelWorkBook.Worksheets.get_Item(1); // выбираем именно первый лист

            ///вставляю в Ексел
            int topRow = 2;
            int leftCol = 1;
            int rows = d.GetUpperBound(0) + 1;
            int cols = d.GetUpperBound(1) + 1;
            object leftTop = ExcelWorkSheet.Cells[topRow, leftCol];
            object rightBottom = ExcelWorkSheet.Cells[topRow + _dgv.RowCount, leftCol + strFieldsName.Count - 1];
            Excel.Range range = ExcelWorkSheet.get_Range(leftTop, rightBottom);
            range.Value2 = d;
            setStyle(range);
            
            Array.Clear(d, 0, d.Length);
            d = null;
            strFieldsHeader.Clear();
            strFieldsHeader = null;
            strFieldsName.Clear();
            strFieldsName = null;

            ExcelApp.Visible = true;  // отображение окна Excel
        }

        private void WordExports()
        {
            ///заполним массив Заголовками
            var strFieldsHeader = new List<string>();
            foreach (object itemChecked in clb.CheckedItems)
                strFieldsHeader.Add(itemChecked.ToString());

            ///заполним массив Именами полей
            var strFieldsName = new List<string>();
            foreach (DataGridViewColumn column in _dgv.Columns)
            {
                if ((column.Visible) & (column.Name != "") & (column.Name.ToUpper() != "ISCHECKEDFIELD")) ///потому что в Лист боксе только видимые столбцы
                    foreach (string str in strFieldsHeader)
                        if (str.Equals(column.HeaderText))
                        {
                            strFieldsName.Add(column.Name);
                            break;
                        }
            }

            ////двумерный массив, заголовок + строки
            object[,] d = new object[_dgv.RowCount + 1, strFieldsName.Count];

            ///заголовки
            int c = 0;
            foreach (string str in strFieldsHeader)
            {
                d[0, c] = str;
                c++;
            }

            //строки
            for (int i = 0; i <= _dgv.RowCount - 1; i++)
            {
                for (int j = 0; j < strFieldsName.Count; j++)
                {
                    d[i + 1, j] = _dgv.Rows[i].Cells[strFieldsName[j].ToString()].Value.ToString();
                }
            }

            ///вставляю в Ексел
            int columns = strFieldsName.Count;
            int rows = _dgv.RowCount;

            Word.Application application = new Word.Application();
            Object missing = Type.Missing;
            application.Documents.Add(ref missing, ref missing, ref missing, ref missing);
            Word.Document document = application.ActiveDocument;
            Word.Range range = application.Selection.Range;
            Object behiavor = Word.WdDefaultTableBehavior.wdWord9TableBehavior;
            Object autoFitBehiavor = Word.WdAutoFitBehavior.wdAutoFitFixed;
            document.Tables.Add(range, rows + 1, columns, ref behiavor, ref autoFitBehiavor);
            for (int i = 0; i <= rows; i++)
                for (int j = 0; j < columns; j++)
                {
                    document.Tables[1].Cell(i + 1, j + 1).Range.Text = d[i, j].ToString();
                }

            Array.Clear(d, 0, d.Length);
            d = null;
            strFieldsHeader.Clear();
            strFieldsHeader = null;
            strFieldsName.Clear();
            strFieldsName = null;

            application.Visible = true;
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            Cursor = System.Windows.Forms.Cursors.WaitCursor;
            btOK.Enabled = false;
            Application.DoEvents();            
            if (_AppType == 1) ExcelExports();
            if (_AppType == 2) WordExports();
            Cursor = System.Windows.Forms.Cursors.Default;
            btOK.Enabled = true;
            Application.DoEvents();
        }

        private void cbCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                cbCheck.Text = "снять отметки";
            }
            else
            {
                cbCheck.Text = "отметить всё";
            }

            for (int i = 0; i <= clb.Items.Count - 1; i++ )
                clb.SetItemChecked(i, ((CheckBox)sender).Checked);
        }
    }
}
