//----------------------------------------------------------------------------------------------------------
using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using DocumentFormat.OpenXml.Spreadsheet;
using SpreadsheetLight;
//----------------------------------------------------------------------------------------------------------
namespace GGPlatform.ExcelManagers
{
    //----------------------------------------------------------------------------------------------------------
    public class ExcelOpenXMLManager
    {
        private SLDocument sl;
        public ExcelOpenXMLManager()
        {
            try
            {
                sl = new SLDocument();
            }
            catch
            {
                throw new ExceptionManeger(ExceptionStrings.ExceptionMessages[4]);
            }
        }
        public ExcelOpenXMLManager(string inFileName, string inSheatName, bool isStreamRead = false)
        {
            if (inFileName != "")
            {
                if (!File.Exists(inFileName))
                    throw new ExceptionManeger(ExceptionStrings.ExceptionMessages[0]);
            } else
                throw new ExceptionManeger(ExceptionStrings.ExceptionMessages[0]);
                
            if (!inFileName.Contains(".xlsx"))
                throw new ExceptionManeger(ExceptionStrings.ExceptionMessages[5]);

            if (inSheatName == "")
                throw new ExceptionManeger(ExceptionStrings.ExceptionMessages[2]);

            try
            {
                if (isStreamRead)
                {
                    FileStream fs = new FileStream(inFileName, FileMode.Open);//читаем фаил в потоке
                    sl = new SLDocument(fs, inSheatName);
                } else
                    sl = new SLDocument(inFileName, inSheatName);//читаем файл
            }
            catch
            {
                throw new ExceptionManeger(ExceptionStrings.ExceptionMessages[4]);
            }
        }

        ~ExcelOpenXMLManager()
        {
            if (sl != null)
                sl.Dispose();
        }
        //сохранение
        public void SaveAs(string inFileName)
        {           
            if (!Directory.Exists("C:\\temp\\")) Directory.CreateDirectory("C:\\temp\\");
            if (inFileName == "") inFileName = "C:\\temp\\tmp.xlsx";
            if (File.Exists(inFileName))
                try
                {
                    File.Delete(inFileName);
                }
                catch
                {
                    MessageBox.Show("Доступ к файлу " + inFileName + " заблокирован.\nВозможно, что файл открыт в другой программе или отсутствует.",
                                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            sl.SaveAs(inFileName);
        }
        public void SaveAs(/*ref*/ MemoryStream inMemoryStream)
        {
            sl.SaveAs(inMemoryStream);
            inMemoryStream.Position = 0;
        }
        //вернуть сам объект
        public SLDocument SLDocument
        {
            get { return sl; }
        }
        //вернуть стиль ячейки
        public SLStyle GetCellStyle(string inCellName)
        {
            return sl.GetCellStyle(inCellName);
        }
        public SLStyle GetCellStyle(int inRowIndex, int inColIndex)
        {
            return sl.GetCellStyle(inRowIndex, inColIndex);
        }
        //выравнивание
        private SLStyle CreateAlignmentStyle(string inCellName, EHorizontalAlignment inHAlignment = EHorizontalAlignment.xlLeft,
            EVerticalAlignment inVAlignment = EVerticalAlignment.xlBottom)
        {     
            SLStyle st = GetCellStyle(inCellName);

                switch ((int)inHAlignment)
                {
                    case -4131:
                        st.Alignment.Horizontal = HorizontalAlignmentValues.Left;
                        break;
                    case -4108:
                        st.Alignment.Horizontal = HorizontalAlignmentValues.Center;
                        break;
                    case -4152:
                        st.Alignment.Horizontal = HorizontalAlignmentValues.Right;
                        break;
                    case -4130:
                        st.Alignment.Horizontal = HorizontalAlignmentValues.Justify;
                        break;
                    default:
                        st.Alignment.Horizontal = HorizontalAlignmentValues.Left;
                        break;
                }

                switch ((int)inVAlignment)
                {
                    case -4160:
                        st.Alignment.Vertical = VerticalAlignmentValues.Top;
                        break;
                    case -4108:
                        st.Alignment.Vertical = VerticalAlignmentValues.Center;
                        break;
                    case -4107:
                        st.Alignment.Vertical = VerticalAlignmentValues.Bottom;
                        break;
                    default:
                        st.Alignment.Vertical = VerticalAlignmentValues.Bottom;
                        break;
                }
            return st;
        }
        private SLStyle CreateAlignmentStyle(int inRowIndex, int inColIndex, EHorizontalAlignment inHAlignment = EHorizontalAlignment.xlLeft,
            EVerticalAlignment inVAlignment = EVerticalAlignment.xlBottom)
        {      
            SLStyle st = GetCellStyle(inRowIndex, inColIndex);

            switch ((int)inHAlignment)
            {
                case -4131:
                    st.Alignment.Horizontal = HorizontalAlignmentValues.Left;
                    break;
                case -4108:
                    st.Alignment.Horizontal = HorizontalAlignmentValues.Center;
                    break;
                case -4152:
                    st.Alignment.Horizontal = HorizontalAlignmentValues.Right;
                    break;
                case -4130:
                    st.Alignment.Horizontal = HorizontalAlignmentValues.Justify;
                    break;
                default:
                    st.Alignment.Horizontal = HorizontalAlignmentValues.Left;
                    break;
            }

            switch ((int)inVAlignment)
            {
                case -4160:
                    st.Alignment.Vertical = VerticalAlignmentValues.Top;
                    break;
                case -4108:
                    st.Alignment.Vertical = VerticalAlignmentValues.Center;
                    break;
                case -4107:
                    st.Alignment.Vertical = VerticalAlignmentValues.Bottom;
                    break;
                default:
                    st.Alignment.Vertical = VerticalAlignmentValues.Bottom;
                    break;
            }
            return st;
        }
        public bool SetAlignment(string inCellName, EHorizontalAlignment inHAlignment, EVerticalAlignment inVAlignment)
        {
            return sl.SetCellStyle(inCellName, CreateAlignmentStyle(inCellName, inHAlignment, inVAlignment));
        }
        public bool SetAlignment(string inCellNameStart, string inCellNameEnd, EHorizontalAlignment inHAlignment, EVerticalAlignment inVAlignment)
        {
            return sl.SetCellStyle(inCellNameStart, inCellNameEnd, CreateAlignmentStyle(inCellNameStart, inHAlignment, inVAlignment));
        }
        public bool SetAlignment(int inRowIndex, int inColIndex, EHorizontalAlignment inHAlignment, EVerticalAlignment inVAlignment)
        {
            return sl.SetCellStyle(inRowIndex, inColIndex, CreateAlignmentStyle(inRowIndex, inColIndex, inHAlignment, inVAlignment));
        }
        public bool SetAlignment(int inRowIndexStart, int inColIndexStart, int inRowIndexEnd, int inColIndexEnd, EHorizontalAlignment inHAlignment, EVerticalAlignment inVAlignment)
        {
            return sl.SetCellStyle(inRowIndexStart, inColIndexStart, inRowIndexEnd, inColIndexEnd, CreateAlignmentStyle(inRowIndexStart, inColIndexStart, inHAlignment, inVAlignment));
        }
        //размер шрифта
        private SLStyle CreateFontSizeStyle(string inCellName, int inFontSize)
        {
            SLStyle st = GetCellStyle(inCellName);
            st.Font.FontSize = inFontSize;
            return st;
        }
        private SLStyle CreateFontSizeStyle(int inRowIndex, int inColIndex, int inFontSize)
        {
            SLStyle st = GetCellStyle(inRowIndex, inColIndex);
            st.Font.FontSize = inFontSize;
            return st;
        }
        public bool SetFontSize(string inCellName, int inFontSize)
        {
            return sl.SetCellStyle(inCellName, CreateFontSizeStyle(inCellName, inFontSize));
        }
        public bool SetFontSize(string inCellNameStart, string inCellNameEnd, int inFontSize)
        {
            return sl.SetCellStyle(inCellNameStart, inCellNameEnd, CreateFontSizeStyle(inCellNameStart, inFontSize));
        }
        public bool SetFontSize(int inRowIndex, int inColIndex, int inFontSize)
        {
            return sl.SetCellStyle(inRowIndex, inColIndex, CreateFontSizeStyle(inRowIndex, inColIndex, inFontSize));
        }
        public bool SetFontSize(int inRowIndexStart, int inColIndexStart, int inRowIndexEnd, int inColIndexEnd, int inFontSize)
        {
            return sl.SetCellStyle(inRowIndexStart, inColIndexStart, inRowIndexEnd, inColIndexEnd, CreateFontSizeStyle(inRowIndexStart, inColIndexStart, inFontSize));
        }
        //стиль шрифта
        private SLStyle CreateFontStyle(string inCellName, bool inBold, bool inStrike, bool inItalic)
        {
            SLStyle st = GetCellStyle(inCellName);
            st.Font.Bold = inBold;
            st.Font.Strike = inStrike;
            st.Font.Italic = inItalic;
            return st;
        }
        private SLStyle CreateFontStyle(int inRowIndex, int inColIndex, bool inBold, bool inStrike, bool inItalic)
        {
            SLStyle st = GetCellStyle(inRowIndex, inColIndex);
            st.Font.Bold = inBold;
            st.Font.Strike = inStrike;
            st.Font.Italic = inItalic;
            return st;
        }
        public bool SetFontStyle(string inCellName, bool inBold, bool inStrike, bool inItalic)
        {
            return sl.SetCellStyle(inCellName, CreateFontStyle(inCellName, inBold, inStrike, inItalic));
        }
        public bool SetFontStyle(string inCellNameStart, string inCellNameEnd, bool inBold, bool inStrike, bool inItalic)
        {
            return sl.SetCellStyle(inCellNameStart, inCellNameEnd, CreateFontStyle(inCellNameStart, inBold, inStrike, inItalic));
        }
        public bool SetFontStyle(int inRowIndex, int inColIndex, bool inBold, bool inStrike, bool inItalic)
        {
            return sl.SetCellStyle(inRowIndex, inColIndex, CreateFontStyle(inRowIndex, inColIndex, inBold, inStrike, inItalic));
        }
        public bool SetFontStyle(int inRowIndexStart, int inColIndexStart, int inRowIndexEnd, int inColIndexEnd, bool inBold, bool inStrike, bool inItalic)
        {
            return sl.SetCellStyle(inRowIndexStart, inColIndexStart, inRowIndexEnd, inColIndexEnd, CreateFontStyle(inRowIndexStart, inColIndexStart, inBold, inStrike, inItalic));
        }
        //рамка
        private SLStyle CreateBorderStyle(string inCellName)
        {
            SLStyle st = sl.GetCellStyle(inCellName);
            st.Border.BottomBorder.BorderStyle = BorderStyleValues.Thin;
            st.Border.LeftBorder.BorderStyle = BorderStyleValues.Thin;
            st.Border.RightBorder.BorderStyle = BorderStyleValues.Thin;
            st.Border.TopBorder.BorderStyle = BorderStyleValues.Thin;
            return st;
        }
        private SLStyle CreateBorderStyle(int inRowIndexStart, int inColIndexStart)
        {
            SLStyle st = sl.GetCellStyle(inRowIndexStart, inColIndexStart);
            st.Border.BottomBorder.BorderStyle = BorderStyleValues.Thin;
            st.Border.LeftBorder.BorderStyle = BorderStyleValues.Thin;
            st.Border.RightBorder.BorderStyle = BorderStyleValues.Thin;
            st.Border.TopBorder.BorderStyle = BorderStyleValues.Thin;
            return st;
        }
        public bool SetBorder(string inCellName)
        {
            return sl.SetCellStyle(inCellName, CreateBorderStyle(inCellName));
        }
        public bool SetBorder(string inCellNameStart, string inCellNameEnd)
        {
            return sl.SetCellStyle(inCellNameStart, inCellNameEnd, CreateBorderStyle(inCellNameStart));
        }
        public bool SetBorder(int inRowIndex, int inColIndex)
        {
            return sl.SetCellStyle(inRowIndex, inColIndex, CreateBorderStyle(inRowIndex, inColIndex));
        }
        public bool SetBorder(int inRowIndexStart, int inColIndexStart, int inRowIndexEnd, int inColIndexEnd)
        {
            return sl.SetCellStyle(inRowIndexStart, inColIndexStart, inRowIndexEnd, inColIndexEnd, CreateBorderStyle(inRowIndexStart, inColIndexStart));
        }
        // установить произвольный стиль для ячейки
        public bool SetCellStyle(string inCellName, SLStyle inStyle)
        {
            return sl.SetCellStyle(inCellName, inStyle);
        }
        public bool SetCellStyle(string inCellNameStart, string inCellNameEnd, SLStyle inStyle)
        {
            return sl.SetCellStyle(inCellNameStart, inCellNameEnd, inStyle);
        }
        public bool SetCellStyle(int inRowIndex, int inColIndex, SLStyle inStyle)
        {
            return sl.SetCellStyle(inRowIndex, inColIndex, inStyle);
        }
        public bool SetCellStyle(int inRowIndexStart, int inColIndexStart, int inRowIndexEnd, int inColIndexEnd, SLStyle inStyle)
        {
            return sl.SetCellStyle(inRowIndexStart, inColIndexStart, inRowIndexEnd, inColIndexEnd, inStyle);
        }
        // worksheats
        public bool AddWorksheat(string inSheatName)
        {
            if (!sl.SelectWorksheet(inSheatName))
                return sl.AddWorksheet(inSheatName);
            else return false;
        }
        public bool DelWorksheat(string inSheatName)
        {
            if (sl.SelectWorksheet(inSheatName))
                return sl.DeleteWorksheet(inSheatName);
            else return false;
        }
        public bool RenameWorksheat(string inOldSheatName, string inNewSheatName)
        {
            if (sl.SelectWorksheet(inOldSheatName))
                return sl.RenameWorksheet(inOldSheatName, inNewSheatName);
            else return false;
        }
        public bool CopyWorksheat(string inExistSheatName, string inNewSheatName)
        {
            if (sl.SelectWorksheet(inExistSheatName))
                return sl.CopyWorksheet(inExistSheatName, inNewSheatName);
            else return false;
        }
        // set value to cell        
        public bool SetValueToCell(string inCellName, DateTime inValue)
        {
            return sl.SetCellValue(inCellName, inValue);
        }
        public bool SetValueToCell(string inCellName, DateTime inValue, string inDateFormat)
        {
            return sl.SetCellValue(inCellName, inValue, inDateFormat);
        }
        public bool SetValueToCell(string inCellName, int inValue)
        {
            return sl.SetCellValue(inCellName, inValue);
        }
        public bool SetValueToCell(string inCellName, string inValue)
        {
            return sl.SetCellValue(inCellName, inValue);
        }
        public bool SetValueToCell(string inCellName, decimal inValue)
        {
            return sl.SetCellValue(inCellName, inValue);
        }
        public bool SetValueToCell(string inCellName, bool inValue)
        {
            return sl.SetCellValue(inCellName, inValue);
        }
        public bool SetValueToCell(int inRowIndex, int inColIndex, DateTime inValue)
        {
            return sl.SetCellValue(inRowIndex, inColIndex, inValue);
        }
        public bool SetValueToCell(int inRowIndex, int inColIndex, DateTime inValue, string inDateFormat)
        {
            return sl.SetCellValue(inRowIndex, inColIndex, inValue, inDateFormat);
        }
        public bool SetValueToCell(int inRowIndex, int inColIndex, int inValue)
        {
            return sl.SetCellValue(inRowIndex, inColIndex, inValue);
        }
        public bool SetValueToCell(int inRowIndex, int inColIndex, string inValue)
        {
            return sl.SetCellValue(inRowIndex, inColIndex, inValue);
        }
        public bool SetValueToCell(int inRowIndex, int inColIndex, decimal inValue)
        {
            return sl.SetCellValue(inRowIndex, inColIndex, inValue);
        }
        public bool SetValueToCell(int inRowIndex, int inColIndex, bool inValue)
        {
            return sl.SetCellValue(inRowIndex, inColIndex, inValue);
        }
        //get value
        public DateTime GetValueFromCellAsDateTime(string inCellName)
        {
            return sl.GetCellValueAsDateTime(inCellName);
        }
        public Int32 GetValueFromCellAsInt32(string inCellName)
        {
            return sl.GetCellValueAsInt32(inCellName);
        }
        public double GetValueFromCellAsDouble(string inCellName)
        {
            return sl.GetCellValueAsDouble(inCellName);
        }
        public string GetValueFromCellAsString(string inCellName)
        {
            return sl.GetCellValueAsString(inCellName);
        }
        public DateTime GetValueFromCellAsDateTime(int inRowIndex, int inColIndex)
        {
            return sl.GetCellValueAsDateTime(inRowIndex, inColIndex);
        }
        public Int32 GetValueFromCellAsInt32(int inRowIndex, int inColIndex)
        {
            return sl.GetCellValueAsInt32(inRowIndex, inColIndex);
        }
        public double GetValueFromCellAsDouble(int inRowIndex, int inColIndex)
        {
            return sl.GetCellValueAsDouble(inRowIndex, inColIndex);
        }
        public string GetValueFromCellAsString(int inRowIndex, int inColIndex)
        {
            return sl.GetCellValueAsString(inRowIndex, inColIndex);
        }
        //высота
        public bool SetRowHeight(int inRowIndexStart, int inRowIndexEnd, double inRowHeight)
        {
            return sl.SetRowHeight(inRowIndexStart, inRowIndexEnd, inRowHeight);
        }
        public bool SetColumnWidth(int inColIndexStart, int inColIndexEnd, double inColumnWidth)
        {
            return sl.SetColumnWidth(inColIndexStart, inColIndexEnd, inColumnWidth);
        }
        public bool SetColumnWidth(string inColNameStart, string inColNameEnd, double inColumnWidth)
        {
            return sl.SetColumnWidth(inColNameStart, inColNameEnd, inColumnWidth);
        }
        //объединнеие
        public bool MergeWorksheetCells(string inCellNameStart, string inCellNameEnd)
        {
            return sl.MergeWorksheetCells(inCellNameStart, inCellNameEnd);
        }
        public bool MergeWorksheetCells(int inRowIndexStart, int inColIndexStart, int inRowIndexEnd, int inColIndexEnd)
        {
            return sl.MergeWorksheetCells(inRowIndexStart, inColIndexStart, inRowIndexEnd, inColIndexEnd);
        }
        public bool UnmergeWorksheetCells(string inCellNameStart, string inCellNameEnd)
        {
            return sl.UnmergeWorksheetCells(inCellNameStart, inCellNameEnd);
        }
        public bool UnmergeWorksheetCells(int inRowIndexStart, int inColIndexStart, int inRowIndexEnd, int inColIndexEnd)
        {
            return sl.UnmergeWorksheetCells(inRowIndexStart, inColIndexStart, inRowIndexEnd, inColIndexEnd);
        }
        //закрепить строки
        public void Freeze(int inNumberOfTopRows, int inNumberOfLeftColumns)
        {
            sl.FreezePanes(inNumberOfTopRows, inNumberOfLeftColumns);
        }
    }

    public class ExcelManager
    {
        private const string UID = "Excel.Application";
        private object oExcel;
        private object WorkBooks, WorkBook, WorkSheets, WorkSheet, Range, Borders, Font, Interior;
        string oPrintTitleRows;
        private string SaveFileNames = "";
        //-------------------------------------------------------------------------------------------------------------
        private void InitExcelApplication()
        {
            try
            {
                oExcel = Activator.CreateInstance(Type.GetTypeFromProgID(UID));
            }
            catch
            {
                throw new ExceptionManeger(ExceptionStrings.ExceptionMessages[1]);
            } 
        }
        //-------------------------------------------------------------------------------------------------------------
        public ExcelManager()
        {
            InitExcelApplication();
            CreateNewDocument();

            oPrintTitleRows = "";
        }
        //-------------------------------------------------------------------------------------------------------------
        public ExcelManager(string inFileName)
        {
            InitExcelApplication();
            if (inFileName != "") OpenDocument(inFileName);
               // else CreateNewDocument();

            oPrintTitleRows = "";
        }
        //-------------------------------------------------------------------------------------------------------------
        public string PPrintTitleRows
        {
            get { return oPrintTitleRows; }
            set { oPrintTitleRows = value; }
        }
        //-------------------------------------------------------------------------------------------------------------
        public object[,] GetRangeValue(string inRange)
        {
            GetRange(inRange);
            return (object[,])Range.GetType().InvokeMember("Value", BindingFlags.GetProperty, null, Range, null);
        }
        public string GetCellValue(int inRow, int inCol)
        {
            object Tmp = WorkSheet.GetType().InvokeMember("Cells", BindingFlags.GetProperty, null, WorkSheet, new object[] { inRow, inCol });
            return Tmp.GetType().InvokeMember("Value", BindingFlags.GetProperty, null, Tmp, null).ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        // inIndexStartColumn = 0 вывести все столбцы, = 1 - кроме первого столбца, inMainApp - ссылка на главную форму 
        public object[,] GetDataArrayFromDataTable(DataTable inDataTable, int inIndexStartColumn, object inMainApp)
        {
            StatusStrip stat = null;
            ToolStripProgressBar prog = null;
            ToolStripStatusLabel text = null;
            if (inMainApp != null)
            {
                Form _MainForm = null;
                _MainForm = (Form)inMainApp;
                System.Windows.Forms.Control[] c = _MainForm.Controls.Find("statusStripContainer", true);
                if (c != null && c.Length > 0)
                {
                    stat = (StatusStrip)c[0];
                    prog = (ToolStripProgressBar)(stat.Items[4]);
                    text = (ToolStripStatusLabel)(stat.Items[3]);
                }
                if (prog != null)
                {
                    prog.Value = 0;
                    prog.Minimum = 0;
                    prog.Maximum = inDataTable.Rows.Count;                    
                    prog.Visible = true;
                }
                if (text != null)
                {
                    text.Text = "Выполняется выгрузка набора данных в отчёт, ожидайте...";
                    Application.DoEvents();
                }
            }

            object[,] DataArray = new object[inDataTable.Rows.Count, inDataTable.Columns.Count - inIndexStartColumn];
            int i = 0;
            foreach (DataRow rw in inDataTable.Rows)
            {
                for (int k = 0; k < inDataTable.Columns.Count - inIndexStartColumn; ++k)
                {
                    if (inDataTable.Columns[k + inIndexStartColumn].DataType == typeof(String))
                        DataArray[i, k] = rw[inDataTable.Columns[k + inIndexStartColumn].ColumnName].ToString();
                    else
                        if ((inDataTable.Columns[k + inIndexStartColumn].DataType == typeof(Int32)) ||
                            (inDataTable.Columns[k + inIndexStartColumn].DataType == typeof(Int16)) ||
                            (inDataTable.Columns[k + inIndexStartColumn].DataType == typeof(Int64)))
                            try
                            { DataArray[i, k] = Convert.ToInt32(rw[inDataTable.Columns[k + inIndexStartColumn].ColumnName].ToString()); }
                            catch
                            { DataArray[i, k] = 0; }
                        else
                            if (inDataTable.Columns[k + inIndexStartColumn].DataType == typeof(Decimal))
                                try
                                { DataArray[i, k] = Convert.ToDecimal(rw[inDataTable.Columns[k + inIndexStartColumn].ColumnName].ToString()); }
                                catch
                                { DataArray[i, k] = 0; }
                            else
                                if (inDataTable.Columns[k + inIndexStartColumn].DataType == typeof(DateTime))
                                    try
                                    { DataArray[i, k] = Convert.ToDateTime((rw[inDataTable.Columns[k + inIndexStartColumn].ColumnName].ToString().Substring(0, 10))); }
                                    catch
                                    { DataArray[i, k] = null; }
                                else
                                    if (inDataTable.Columns[k + inIndexStartColumn].DataType == typeof(Boolean))
                                    {
                                        string t = Convert.ToString(rw[inDataTable.Columns[k + inIndexStartColumn].ColumnName]);
                                        if (t != "")
                                            t = (t == "True") ? "Да":"Нет";
                                        DataArray[i, k] = t;
                                    }
                                    else DataArray[i, k] = Convert.ToString(rw[inDataTable.Columns[k + inIndexStartColumn].ColumnName]);
                }
                ++i; //в массиве начинаю заполнять новую строку
                if (prog != null) prog.Value += 1;
            }
            if (prog != null)
            {
                prog.Value = prog.Maximum;
                prog.Visible = false;
            }
            if (text != null)
            {
                text.Text = "";
            }
            stat = null;
            prog = null;
            text = null;

            return DataArray;
        }
        //-------------------------------------------------------------------------------------------------------------
        public object GetRange(string inRange)
        {
            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty, null, WorkSheet, new object[] { inRange });
            return Range;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool Visible
        {
            set
            {
                oExcel.GetType().InvokeMember("Visible", BindingFlags.SetProperty, null, oExcel, new object[] { value });
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        //Create new Excel Document
        public void OpenDocument(string inFileName)
        {
            if (!File.Exists(inFileName))
            {
                oExcel.GetType().InvokeMember("Quit", BindingFlags.InvokeMethod, null, oExcel, null);
                Marshal.ReleaseComObject(oExcel);
                oExcel = null;
                GC.GetTotalMemory(true);                
                throw new ExceptionManeger(ExceptionStrings.ExceptionMessages[0]);
            }
            WorkBooks = oExcel.GetType().InvokeMember("Workbooks", BindingFlags.GetProperty, null, oExcel, null);
            WorkBook = WorkBooks.GetType().InvokeMember("Open", BindingFlags.InvokeMethod, null, WorkBooks, new object[] { inFileName, true });
            WorkSheets = WorkBook.GetType().InvokeMember("Worksheets", BindingFlags.GetProperty, null, WorkBook, null);
            WorkSheet = WorkSheets.GetType().InvokeMember("Item", BindingFlags.GetProperty, null, WorkSheets, new object[] { 1 });
        }
        //-------------------------------------------------------------------------------------------------------------
        public int SetWorkSheetByIndex
        {
            set 
            { 
                WorkSheet = WorkSheets.GetType().InvokeMember("Item", BindingFlags.GetProperty, null, WorkSheets, new object[] { value });
                WorkSheet.GetType().InvokeMember("Activate", BindingFlags.GetProperty, null, WorkSheet, null);
            }
        }
        public string SetWorkSheetByName
        {
            set
            {
                WorkSheet = WorkSheets.GetType().InvokeMember("Item", BindingFlags.GetProperty, null, WorkSheets, new object[] { value });
                WorkSheet.GetType().InvokeMember("Activate", BindingFlags.GetProperty, null, WorkSheet, null);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        // Open existing Excel Document
        public void CreateNewDocument()
        {
            WorkBooks = oExcel.GetType().InvokeMember("Workbooks", BindingFlags.GetProperty, null, oExcel, null);
            WorkBook = WorkBooks.GetType().InvokeMember("Add", BindingFlags.InvokeMethod, null, WorkBooks, null);
            WorkSheets = WorkBook.GetType().InvokeMember("Worksheets", BindingFlags.GetProperty, null, WorkBook, null);
            WorkSheet = WorkSheets.GetType().InvokeMember("Item", BindingFlags.GetProperty, null, WorkSheets, new object[] { 1 });
            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty, null, WorkSheet, new object[1] { "A1" });
        }
        //-------------------------------------------------------------------------------------------------------------
        //Close Excel Document
        public void CloseDocument()
        {
            WorkBook.GetType().InvokeMember("Close", BindingFlags.InvokeMethod, null, WorkBook, new object[] { true });
            oExcel.GetType().InvokeMember("Quit", BindingFlags.InvokeMethod, null, oExcel, null);
            Marshal.ReleaseComObject(WorkSheets);
            Marshal.ReleaseComObject(WorkSheet);
            Marshal.ReleaseComObject(WorkBooks);
            Marshal.ReleaseComObject(WorkBook);
            Marshal.ReleaseComObject(oExcel);
            WorkSheets = null;
            WorkSheet = null;
            WorkBooks = null;
            WorkBook = null;
            oExcel = null;
            Range = null;
            Borders = null;
            Font = null;
            Interior = null;
            GC.GetTotalMemory(true);
        }

        public void CloseExcelDocumentAndShow()
        {
            CloseDocument(); //уничтожаю объект Ексел
            if (File.Exists(SaveFileNames))  //если существует сохраненный Экселевский файл, то показать его на экран
                System.Diagnostics.Process.Start(SaveFileNames);
        }

        //-------------------------------------------------------------------------------------------------------------
        //Save Excel Document
        public Boolean SaveDocument(string inFileName)
        {
            SaveFileNames = "";
            if (!Directory.Exists("C:\\temp\\")) Directory.CreateDirectory("C:\\temp\\");
            if (inFileName == "") inFileName = "C:\\temp\\tmp.xls";
            if (File.Exists(inFileName))
                try
                {
                    File.Delete(inFileName);
                }
                catch
                {
                    MessageBox.Show("Доступ к файлу " + inFileName + " заблокирован.\nВозможно, что файл открыт в другой программе или отсутствует.",
                                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            WorkBook.GetType().InvokeMember("SaveAs", BindingFlags.InvokeMethod, null, WorkBook, new object[] { inFileName });
            SaveFileNames = inFileName;
            return true;
        }
        //-------------------------------------------------------------------------------------------------------------
        public Boolean SaveDocument(string inFileName, bool isDeleteFile, ESaveFormats inESaveFormats)
        {
            SaveFileNames = "";
            if (inFileName == "")
                if (!Directory.Exists("C:\\temp\\")) Directory.CreateDirectory("C:\\temp\\");
            if (inFileName != "")
                if (!Directory.Exists(Path.GetDirectoryName(inFileName))) Directory.CreateDirectory(Path.GetDirectoryName(inFileName));

            if (inESaveFormats == ESaveFormats.xlNormal)
            {
                if (inFileName == "") inFileName = "C:\\temp\\tmp.xls";
                if (isDeleteFile && File.Exists(inFileName))
                    try
                    {
                        File.Delete(inFileName);
                    }
                    catch (Exception E)
                    {
                        MessageBox.Show("Доступ к файлу " + inFileName + " заблокирован.\nВозможно, что файл открыт в другой программе или отсутствует.\n" + E.Message, 
                                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                WorkBook.GetType().InvokeMember("SaveAs", BindingFlags.InvokeMethod, null, WorkBook, new object[] { inFileName, inESaveFormats });
            }
            if (inESaveFormats == ESaveFormats.XlFixedFormatTypePDF)
            {
                if (inFileName == "") inFileName = "C:\\temp\\tmp.pdf";
                if (isDeleteFile && File.Exists(inFileName))
                    try
                    {
                        File.Delete(inFileName);
                    }
                    catch (Exception E)
                    {
                        MessageBox.Show("Доступ к файлу " + inFileName + " заблокирован.\nВозможно, что файл открыт в другой программе или отсутствует.\n" + E.Message,
                                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                WorkBook.GetType().InvokeMember("ExportAsFixedFormat", BindingFlags.InvokeMethod, null, WorkBook, new object[] { inESaveFormats, inFileName });
            }
            SaveFileNames = inFileName;
            return true;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetPrintTitleRows(string inPrintTitleRows)
        {
            //  "$2:$2"  вторая строка
            object Tmp = WorkSheet.GetType().InvokeMember("PageSetup", BindingFlags.GetProperty, null, WorkSheet, null);
            Tmp.GetType().InvokeMember("PrintTitleRows", BindingFlags.SetProperty, null, Tmp, new object[] { inPrintTitleRows });
            Tmp = null;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetPrintTitleColumns(string inPrintTitleColumns)
        {
            //  "$B:$B"  столбец B
            object Tmp = WorkSheet.GetType().InvokeMember("PageSetup", BindingFlags.GetProperty, null, WorkSheet, null);
            Tmp.GetType().InvokeMember("PrintTitleColumns", BindingFlags.SetProperty, null, Tmp, new object[] { inPrintTitleColumns });
            Tmp = null;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetFitToPagesTall()
        {
            object Tmp = WorkSheet.GetType().InvokeMember("PageSetup", BindingFlags.GetProperty, null, WorkSheet, null);
            Tmp.GetType().InvokeMember("FitToPagesTall", BindingFlags.SetProperty, null, Tmp, new object[] { 1000 });
            Tmp = null;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetFitToPagesWide()
        {
            object Tmp = WorkSheet.GetType().InvokeMember("PageSetup", BindingFlags.GetProperty, null, WorkSheet, null);
            Tmp.GetType().InvokeMember("FitToPagesWide", BindingFlags.SetProperty, null, Tmp, new object[] { 1 });
            Tmp = null;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetZoomWS()
        {
            object Tmp = WorkSheet.GetType().InvokeMember("PageSetup", BindingFlags.GetProperty, null, WorkSheet, null);
            Tmp.GetType().InvokeMember("Zoom", BindingFlags.SetProperty, null, Tmp, new object[] { false });
            Tmp = null;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetColontitul(string inFormat)
        {
            object Tmp = WorkSheet.GetType().InvokeMember("PageSetup", BindingFlags.GetProperty, null, WorkSheet, null);
            Tmp.GetType().InvokeMember("RightFooter", BindingFlags.SetProperty, null, Tmp, new object[] { inFormat });
            Tmp = null;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetOrder()
        {
            object Tmp = WorkSheet.GetType().InvokeMember("PageSetup", BindingFlags.GetProperty, null, WorkSheet, null);
            Tmp.GetType().InvokeMember("Order", BindingFlags.SetProperty, null, Tmp, new object[] { 1 });
            Tmp = null;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetZoomApp()
        {
            object Tmp = oExcel.GetType().InvokeMember("ActiveWindow", BindingFlags.GetProperty, null, oExcel, null);
            Tmp.GetType().InvokeMember("Zoom", BindingFlags.SetProperty, null, Tmp, new object[] { false });
            Tmp = null;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetDisplayGridLines(Boolean inDisplay)
        {
            object Tmp = oExcel.GetType().InvokeMember("ActiveWindow", BindingFlags.GetProperty, null, oExcel, null);
            Tmp.GetType().InvokeMember("DisplayGridlines", BindingFlags.SetProperty, null, Tmp, new object[] { inDisplay });
            Tmp = null;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetView()
        {
            object Tmp = oExcel.GetType().InvokeMember("ActiveWindow", BindingFlags.GetProperty, null, oExcel, null);
            Tmp.GetType().InvokeMember("View", BindingFlags.SetProperty, null, Tmp, new object[] { 2 });
            Tmp = null;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetValueToRange(string inRange, object inValue)
        {
            GetRange(inRange);
            Range.GetType().InvokeMember("Value", BindingFlags.SetProperty, null, Range, new object[] { inValue });
        }

        public static string ParseColNum(int ColNum)
        {
            StringBuilder sb = new StringBuilder();
            if (ColNum <= 0) return "A";

            while (ColNum != 0)
            {
                sb.Append((char)('A' + (ColNum % 26)));
                ColNum /= 26;
            }
            return sb.ToString();
        }

        //-------------------------------------------------------------------------------------------------------------
        //public void SetValueToCell(int inRow, int inCol, object inValue)
        //{
        //    object Tmp = WorkSheet.GetType().InvokeMember("Cells", BindingFlags.GetProperty, null, WorkSheet, new object[] { inRow, inCol });
        //    Tmp.GetType().InvokeMember("Item", BindingFlags.SetProperty, null, Tmp, new object[] { inRow, inCol, inValue });
        //    //Tmp = null;
        //}
        public void SetValueToCell(int inRow, int inCol, string value)
        {
            string range = ParseColNum(inCol - 1) + inRow.ToString();
            GetRange(range);
            Range.GetType().InvokeMember("Value", BindingFlags.SetProperty, null, Range, new object[] { value });
        }
        /*//-------------------------------------------------------------------------------------------------------------
        public void SetPrintPreview()
        {
            object Tmp = oExcel.GetType().InvokeMember("ActiveWindow", BindingFlags.GetProperty, null, oExcel, null);
            Tmp.GetType().InvokeMember("SelectedSheets", BindingFlags.SetProperty, null, Tmp, new object[] { false });

            Tmp.GetType().InvokeMember("PrintPreview", BindingFlags.InvokeMethod, null, Tmp, null);

            //ActiveWindow.SelectedSheets.PrintPreview
        }*/
        //-------------------------------------------------------------------------------------------------------------
        public void AddAutoFilter(string inRange)
        {
            GetRange(inRange);
            Range.GetType().InvokeMember("AutoFilter", BindingFlags.SetProperty, null, Range, new object[] { true });
        }
        //-------------------------------------------------------------------------------------------------------------
        public int GetPageCount()
        {
            object Tmp = WorkBook.GetType().InvokeMember("HPageBreaks", BindingFlags.GetProperty, null, WorkSheet, null);
            return (int)Tmp.GetType().InvokeMember("Count", BindingFlags.GetProperty, null, Tmp, null);
        }
        //-------------------------------------------------------------------------------------------------------------*/
        public void MergeCells(string inRange)
        {
            GetRange(inRange);
            Range.GetType().InvokeMember("MergeCells", BindingFlags.SetProperty, null, Range, new object[] { true });
        }
        //-------------------------------------------------------------------------------------------------------------
        public string GetValue(string inRange)
        {
            GetRange(inRange);
            object Tmp = Range.GetType().InvokeMember("Value", BindingFlags.GetProperty, null, Range, null);
            if (Tmp != null)
                return Tmp.ToString();
            else
                return String.Empty;
        }
        //-------------------------------------------------------------------------------------------------------------       
        public void SetHorizontalAlignment(string inRange, EHorizontalAlignment inAlignment)
        {
            GetRange(inRange);
            Range.GetType().InvokeMember("HorizontalAlignment", BindingFlags.SetProperty, null, Range, new object[] { (int)inAlignment });
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetVerticalAlignment(string inRange, EVerticalAlignment inAlignment)
        {
            GetRange(inRange);
            Range.GetType().InvokeMember("VerticalAlignment", BindingFlags.SetProperty, null, Range, new object[] { (int)inAlignment });
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetWorkSheetOrientation(WorkSheetOrientation inOrientation)
        {
            object Tmp = WorkSheet.GetType().InvokeMember("PageSetup", BindingFlags.GetProperty, null, WorkSheet, null);
            Tmp.GetType().InvokeMember("Orientation", BindingFlags.SetProperty, null, Tmp, new object[] { (int)inOrientation });
            Tmp = null;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetColumnWidth(string inRange, double inWidth)
        {
            GetRange(inRange);
            Range.GetType().InvokeMember("ColumnWidth", BindingFlags.SetProperty, null, Range, new object[] { inWidth });
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetRowHeight(string inRange, double inHeight)
        {
            GetRange(inRange);
            Range.GetType().InvokeMember("RowHeight", BindingFlags.SetProperty, null, Range, new object[] { inHeight });
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetBorderStyle(string inRange, BorderLineStyle inStyle)
        {
            GetRange(inRange);
            Borders = Range.GetType().InvokeMember("Borders", BindingFlags.GetProperty, null, Range, null);
            Range.GetType().InvokeMember("LineStyle", BindingFlags.SetProperty, null, Borders, new object[] { (int)inStyle });
            Borders = null;
        } 
        //-------------------------------------------------------------------------------------------------------------
        public void SetFontSize(string inRange, int inSize)
        {
            GetRange(inRange);
            Font = Range.GetType().InvokeMember("Font", BindingFlags.GetProperty, null, Range, null);
            Range.GetType().InvokeMember("Size", BindingFlags.SetProperty, null, Font, new object[] { inSize });
            Font = null;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetFontName(string inRange, string inFontName)
        {
            GetRange(inRange);
            Font = Range.GetType().InvokeMember("Font", BindingFlags.GetProperty, null, Range, null);
            Range.GetType().InvokeMember("Name", BindingFlags.SetProperty, null, Font, new object[] { inFontName });
            Font = null;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetFontBold(string inRange)
        {
            GetRange(inRange);
            Font = Range.GetType().InvokeMember("Font", BindingFlags.GetProperty, null, Range, null);
            Range.GetType().InvokeMember("Bold", BindingFlags.SetProperty, null, Font, new object[] { true });
            Font = null;
        }
        //УСТАНОВКА ЦВЕТА ФОНА ЯЧЕЙКИ
        public void SetColor(string inRange, int inColor)
        {
            GetRange(inRange);
            Interior = Range.GetType().InvokeMember("Interior", BindingFlags.GetProperty, null, Range, null);
            Range.GetType().InvokeMember("ColorIndex", BindingFlags.SetProperty, null, Interior, new object[] { inColor });
            Interior = null;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetFormat(string inRange, RangeFormat inFormatIndex)
        {
            GetRange(inRange);
            Range.GetType().InvokeMember("NumberFormat", BindingFlags.SetProperty, null, Range, new object[] { CFormatType.Formats[(int)inFormatIndex] });
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetFormat(string inRange, string inFormat)
        {
            GetRange(inRange);
            Range.GetType().InvokeMember("NumberFormat", BindingFlags.SetProperty, null, Range, new object[] { inFormat });
        }

        public void MergeEqualCellsInColumn(string inColumn, int inFirstRow, int inLastRow)
        {
            string _tmpValueOld = GetValue(inColumn + inFirstRow.ToString());
            string _tmpValueNew = ""; 

            int _first = 0;
            int _last = 0;

            SetDisplayAlerts(false);
            _first = inFirstRow;
            _last = inFirstRow;
            for (int i = inFirstRow; i <= inLastRow + 1; i++)
            {
                try
                {
                    _tmpValueNew = GetValue(inColumn + i.ToString());
                }
                catch
                {
                    _tmpValueNew = "";
                }

                if (_tmpValueOld == _tmpValueNew) 
                {
                    _last = i;
                } else
                {
                    if (_first != _last) //не имеет смысла объединять одну ячейку в строке
                    {
                        MergeCells(inColumn + _first.ToString() + ":" + inColumn + _last.ToString());
                        SetHorizontalAlignment(inColumn + _first.ToString() + ":" + inColumn + _last.ToString(), EHorizontalAlignment.xlLeft);
                        SetVerticalAlignment(inColumn + _first.ToString() + ":" + inColumn + _last.ToString(), EVerticalAlignment.xlTop);
                        SetWrapText(inColumn + _first.ToString() + ":" + inColumn + _last.ToString(), true);
                    }
                    _first = i;
                    _last = i;
                    _tmpValueOld = _tmpValueNew;
                }
            }           
        }

        //-------------------------------------------------------------------------------------------------------------
        //inSearchRange A19:A100 - диапазон в котором ищем, inSearchValue - что ищем, inSetRange A:G - колонки в которых применяем формат,   формат, inMainApp - ссылка на главную форму
        public void SetFormatToFindInRange(string inSearchRange, string inSearchValue, string inSetRange, int inSetSize, Boolean inSetBold, object inMainApp)
        {
            StatusStrip stat = null;
            ToolStripProgressBar prog = null;
            ToolStripStatusLabel text = null;
            if (inMainApp != null)
            {
                Form _MainForm = null;
                _MainForm = (Form)inMainApp;
                System.Windows.Forms.Control[] c = _MainForm.Controls.Find("statusStripContainer", true);
                if (c != null && c.Length > 0)
                {
                    stat = (StatusStrip)c[0];
                    prog = (ToolStripProgressBar)(stat.Items[4]);
                    text = (ToolStripStatusLabel)(stat.Items[3]);
                }
            }
            int rowStart = 0;
            int rowFinish = 0;
            string _rowStart = "";
            string _rowFinish = "";

            string colStart = "";
            string colFinish = "";

            //разбираю диапазон, на два диапазона С по ПО
            string start = inSearchRange.Substring(0, inSearchRange.IndexOf(":"));
            string finish = inSearchRange.Substring(inSearchRange.IndexOf(":") + 1, inSearchRange.Length - inSearchRange.IndexOf(":") - 1);

            //вычленяю имя столбца и номер строки
            foreach (char ch in start)
                if (ch >= '0' && ch <= '9')
                    _rowStart += ch;
                else
                    colStart += ch;

            foreach (char ch in finish)
                if (ch >= '0' && ch <= '9')
                    _rowFinish += ch;
                else
                    colFinish += ch;

            //преобразую номер строки в Число
            if (
                (!(Int32.TryParse(_rowStart, out rowStart)))
               ||
               (!(Int32.TryParse(_rowFinish, out rowFinish)))
                )
            {
                stat = null;
                prog = null;
                text = null;
                return;
            }

            if (prog != null)
            {
                prog.Value = 0;
                prog.Minimum = 0;
                prog.Maximum = rowFinish - rowStart + 1;
                prog.Visible = true;
            }
            if (text != null)
            {
                text.Text = "Выполняется форматирование отчёта, ожидайте...";
                Application.DoEvents();
            }

            string rangeFind = "";
            string val = "";

            //цикл по строкам С - ПО и по стоблцу, сравниваю значение в ячейке
            for (int i = rowStart; i <= rowFinish; i++)
            {
                rangeFind = colStart + i.ToString();
                val = "";
                try
                {
                    val = GetValue(colStart + i.ToString()).ToString();
                }
                catch
                { }
                //если содержит искомое значение, то Жирным
                if (val.Contains(inSearchValue))
                {
                    GetRange(inSetRange.Replace(":", i.ToString() + ":") + i.ToString());
                    Font = Range.GetType().InvokeMember("Font", BindingFlags.GetProperty, null, Range, null);
                    Range.GetType().InvokeMember("Size", BindingFlags.SetProperty, null, Font, new object[] { inSetSize });
                    Range.GetType().InvokeMember("Bold", BindingFlags.SetProperty, null, Font, new object[] { inSetBold });
                    Font = null;
                }
                if (prog != null) prog.Value += 1;
            }
            if (prog != null)
            {
                prog.Value = prog.Maximum;
                prog.Visible = false;
            }
            if (text != null)
            {
                text.Text = "";
            }
            stat = null;
            prog = null;
            text = null;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetWrapText(string inRange, bool isWrap)
        {
            GetRange(inRange);
            Range.GetType().InvokeMember("WrapText", BindingFlags.SetProperty, null, Range, new object[] { isWrap });
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetDisplayAlerts(bool inDisplay)
        {
            oExcel.GetType().InvokeMember("DisplayAlerts", BindingFlags.SetProperty, null, oExcel, new object[] { inDisplay });
        }
        public void SetScreenUpdating(bool inUpdate)
        {
            oExcel.GetType().InvokeMember("ScreenUpdating", BindingFlags.SetProperty, null, oExcel, new object[] { inUpdate });
        }
        public void SetCalculation(ECalculation inType)
        {
            oExcel.GetType().InvokeMember("Calculation", BindingFlags.SetProperty, null, oExcel, new object[] { inType });
        }
        //-------------------------------------------------------------------------------------------------------------
        //Количество строк в WorkSeet
        public int GetRowsCount()
        {
            object UsedRange = WorkSheet.GetType().InvokeMember("UsedRange", BindingFlags.GetProperty, null, WorkSheet, null);
            object Rows = UsedRange.GetType().InvokeMember("Rows", BindingFlags.GetProperty, null, UsedRange, null);
            object RowsCount = Rows.GetType().InvokeMember("Count", BindingFlags.GetProperty, null, Rows, null);
            return (int)RowsCount;
        }
        //-------------------------------------------------------------------------------------------------------------
        /*==== Get Document Properties  ========*/
        //-------------------------------------------------------------------------------------------------------------
        public object GetFontSize(string inRange)
        {
            GetRange(inRange);
            Font = Range.GetType().InvokeMember("Font", BindingFlags.GetProperty, null, Range, null);
            return Range.GetType().InvokeMember("Size", BindingFlags.GetProperty, null, Font, null);
        }
        //-------------------------------------------------------------------------------------------------------------
        public object GetFontStyle(string inRange)
        {
            GetRange(inRange);
            Font = Range.GetType().InvokeMember("Font", BindingFlags.GetProperty, null, Range, null);
            return Range.GetType().InvokeMember("FontStyle", BindingFlags.GetProperty, null, Font, null);
        }
        //-------------------------------------------------------------------------------------------------------------
        public object GetColumnWidth(string inRange)
        {
            GetRange(inRange);
            return Range.GetType().InvokeMember("ColumnWidth", BindingFlags.GetProperty, null, Range, null);
        }
        //-------------------------------------------------------------------------------------------------------------
        public object GetHorizontalAlignment(string inRange)
        {
            GetRange(inRange);
            return Range.GetType().InvokeMember("HorizontalAlignment", BindingFlags.GetProperty, null, Range, null);
        }
        ////защита листа
        //public void SetProtectActiveWorkSheet(string inPassword,
        //                                      bool inDrawingObjects = false,
        //                                      bool inContents = false,
        //                                      bool inScenarios = false,
        //                                      bool inUserInterfaceOnly = false,
        //                                      bool inAllowFormattingCells = false,
        //                                      bool inAllowFormattingColumns = false,
        //                                      bool inAllowFormattingRows = false,
        //                                      bool inAllowInsertingColumns = false,
        //                                      bool inAllowInsertingRows = false,
        //                                      bool inAllowInsertingHyperlinks = false,
        //                                      bool inAllowDeletingColumns = false,
        //                                      bool inAllowDeletingRows = false,
        //                                      bool inAllowSorting = false,
        //                                      bool inAllowFiltering = false,
        //                                      bool inAllowUsingPivotTables = false)                                                
        //{
        //    WorkSheet.GetType().InvokeMember("Protect", BindingFlags.InvokeMethod, null, WorkSheet, new object[] 
        //        { inPassword, 
        //          inDrawingObjects, inContents, inScenarios,
        //          inUserInterfaceOnly, inAllowFormattingCells, 
        //          inAllowFormattingColumns, inAllowFormattingRows, inAllowInsertingColumns, 
        //          inAllowInsertingRows, inAllowInsertingHyperlinks, inAllowDeletingColumns, 
        //          inAllowDeletingRows, inAllowSorting, inAllowFiltering, inAllowUsingPivotTables }
        //          );
        //}
        //public void SetUnprotectActiveWorkSheet(string inPassword)
        //{
        //    WorkSheet.GetType().InvokeMember("Unprotect", BindingFlags.InvokeMethod, null, WorkSheet, new object[] { inPassword });
        //}
        //ПЕРЕИМЕНОВАТЬ ЛИСТ
        public void SetNameActiveWorkSheet(string inNameNewWorkSheet)
        {
            WorkSheet.GetType().InvokeMember("Name", BindingFlags.SetProperty, null, WorkSheet, new object[] { inNameNewWorkSheet });
        }
        public void SetNameWorkSheet(int inIndexSheet, string inNameNewWorkSheet)
        {
            WorkSheet = WorkSheets.GetType().InvokeMember("Item", BindingFlags.GetProperty, null, WorkSheets, new object[] { inIndexSheet });
            WorkSheet.GetType().InvokeMember("Name", BindingFlags.SetProperty, null, WorkSheet, new object[] { inNameNewWorkSheet });
        }
        public void SetNameWorkSheet(string inNameSheet, string inNameNewWorkSheet)
        {
            WorkSheet = WorkSheets.GetType().InvokeMember("Item", BindingFlags.GetProperty, null, WorkSheets, new object[] { inNameSheet });
            WorkSheet.GetType().InvokeMember("Name", BindingFlags.SetProperty, null, WorkSheet, new object[] { inNameNewWorkSheet });
        }
        //ДОБАВЛЕНИЕ ЛИСТА
        public void AddNewWorkSheet(string inNameNewWorkSheet)
        {
            WorkSheet = WorkSheets.GetType().InvokeMember("Add", BindingFlags.GetProperty, null, WorkSheets, null);
            WorkSheet.GetType().InvokeMember("Name", BindingFlags.SetProperty, null, WorkSheet, new object[] { inNameNewWorkSheet });
        }
        //-------------------------------------------------------------------------------------------------------------
        public void CopyWorkSheet(string inNameNewWorkSheet, int inIndexCopyWorkSheet)
        {
            WorkSheet = WorkSheets.GetType().InvokeMember("Item", BindingFlags.GetProperty, null, WorkSheets, new object[] { inIndexCopyWorkSheet });
            WorkSheet.GetType().InvokeMember("Copy", BindingFlags.GetProperty, null, WorkSheet, new object[] { WorkSheet });
            WorkSheet = WorkBook.GetType().InvokeMember("ActiveSheet", BindingFlags.GetProperty, null, WorkBook, null); //скопированный лист будет Активным
            WorkSheet.GetType().InvokeMember("Name", BindingFlags.SetProperty, null, WorkSheet, new object[] { inNameNewWorkSheet });
        }
        public void CopyWorkSheet(string inNameNewWorkSheet, string inNameCopyWorkSheet)
        {
            WorkSheet = WorkSheets.GetType().InvokeMember("Item", BindingFlags.GetProperty, null, WorkSheets, new object[] { inNameCopyWorkSheet });
            WorkSheet.GetType().InvokeMember("Copy", BindingFlags.GetProperty, null, WorkSheet, new object[] { WorkSheet });
            WorkSheet = WorkBook.GetType().InvokeMember("ActiveSheet", BindingFlags.GetProperty, null, WorkBook, null); //скопированный лист будет Активным
            WorkSheet.GetType().InvokeMember("Name", BindingFlags.SetProperty, null, WorkSheet, new object[] { inNameNewWorkSheet });
        }

        //-------------------------------------------------------------------------------------------------------------
    }
    //---------------------------------------------------------------------------------------------------------- 
}
//----------------------------------------------------------------------------------------------------------
