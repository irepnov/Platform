using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

using GGPlatform.DBServer;
using GGPlatform.RFCManag;
using PropertyGridEx;
using GGPlatform.InspectorManagerSP;
using DataGridViewGGControl;
//using GGPlatform.NSPropertyGridAddClasses;

namespace GGPlatform.BuiltInApp
{

    /// <summary>
    /// подключение плагинов к платформе
    /// </summary>
    /// <param name="inWorkplaceID"></param>
    /// <param name="inPluginID"></param>
    public delegate void DelegClosePlugin(int inWorkplaceID, int inPluginID);

    public enum EnumPluginButtons
    {
        EPB_Option = 1,
        EPB_Refresh = 2,
        EPB_RefreshFilter = 4,
        EPB_ListShow = 8
    } //типы 

    public interface IPlugin
    {
        string plugAssemblyName { set; get; }
        string plugSoftName { set; get; }
        string plugName { get; set; }
        int plugWorkplaceID { set; get; }
        int plugPluginID { set; get; }
        int plugPluginButtons { get; }
        ToolStrip plugToolStrip { get; }       
        DelegClosePlugin plugEventClosed { set; }
        void plugInit(object inMainApp, DBSqlServer inDBSqlServer, RFCManager inRFCManager, ref Lists inActiveLists);      
        void plugDeinit();      
        void appRefresh();
        void appRefreshFilter();
        void appOption();

        void appListAdd();
        void appListDel();
    }


    /// <summary>
    /// //текущие списки программы
    /// </summary>
    public class ListItem
    {
        private string _listPlugAssembly = "";
        private string _listCode = "";
        private string _listName = "";
        private string _listValue = "";
        private Boolean _listChecked = false;

        public ListItem(string inListPlugAssembly, string inListCode, string inListName, string inListValue, Boolean inListChecked)
        {
            _listPlugAssembly = inListPlugAssembly;
            _listCode = inListCode;
            _listName = inListName;
            _listValue = inListValue;
            _listChecked = inListChecked;
        }

        public string ListPlugAssembly { set { _listPlugAssembly = value; } get { return _listPlugAssembly; } }
        public string ListCode { set { _listCode = value; } get { return _listCode; } }
        public string ListName { set { _listName = value; } get { return _listName; } }
        public string ListValue { set { _listValue = value; } get { return _listValue; } }
        public Boolean ListIsChecked { set { _listChecked = value; } get { return _listChecked; } }
    } // элемент текущего списка

    public class Lists
    {
        private List<ListItem> _lists = null;
        private frmShowLists _fmShowList = null;

        public Lists() 
        {
            _lists = new List<ListItem>();
        }

        ~Lists()
        {
            try
            {
                _lists.Clear();
                _lists = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка деструктора: \n" + ex.Message,
                                "Ошибка [модуль BuiltInApp, класс Lists, метод ~Lists]",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

      //  public List<ListItem> GetLists { get { return _lists; } }

        //добавление рефреш
        public void AddList(string inListPluginAssembly, string inListCode, string inListName, string inListValue)
        {
            Boolean fl = false;
            Int32 index = _lists.Count;

            for (int i = 0; i < _lists.Count; i++) // Loop through List with for
            {
                if ((_lists[i].ListPlugAssembly == inListPluginAssembly) &&
                    (_lists[i].ListCode == inListCode))
                {
                    fl = _lists[i].ListIsChecked;
                    index = i;
                    _lists.RemoveAt(i);
                }
            }
            
            _lists.Insert(index, new ListItem(inListPluginAssembly, inListCode, inListName, inListValue, fl));
        }

        //исключение из списка
        public void DelList(string inListPluginAssembly, string inListCode)
        {
            for (int i = 0; i < _lists.Count; i++) // Loop through List with for
            {
                if ((_lists[i].ListPlugAssembly == inListPluginAssembly) &&
                    (_lists[i].ListCode == inListCode))
                    _lists.RemoveAt(i);
            }
        }

        //вернуть выбранные списки
        public Dictionary<string, string> GetSelectedLists()
        {
            Dictionary<string, string> _selectedlists = new Dictionary<string,string>();
            foreach (ListItem item in _lists)
            {
                if (item.ListIsChecked)
                    _selectedlists.Add(item.ListCode, item.ListValue);
            }
            return _selectedlists;
        }

        /*public void SetChecked(Boolean inCheck)
        {
            foreach (ListItem item in _lists)
            {
                item.ListIsChecked = inCheck;
            } 
        }*/

        public void ShowLists()
        {
            if (_fmShowList == null)
                _fmShowList = new frmShowLists(ref _lists);
            _fmShowList.ShowDialog();
            _fmShowList.Dispose();
            _fmShowList = null;
            return;
        }

    }


    /// <summary>
    /// менеджер отчяетов
    /// </summary>

    public enum EnumReportButtons
    {
        ERB_Excel = 1,
        ERB_Word = 2
    } //типы 

    public interface IReport
    {
        string reportAssemblyName { set; get; }
        int reportID { set; get; }
        int reportButtons { get; }
        void reportInit(object inMainApp, DataGridViewGGControl.DataGridViewGGControl inMainGridControl, DBSqlServer inDBSqlServer, RFCManager inRFCManager);
        void reportDeinit();
        void reportGetDataToExcel(Dictionary<string, string> inParams);
        void reportGetDataToWord(Dictionary<string, string> inParams);
        PropertyGridEx.PropertyGridEx reportGetParameters { get; }
        CInspectorManager reportGetInspector { get; } 
    }
}
