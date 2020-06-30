using System;
using System.Collections.Generic;
using System.Text;
using GGPlatform.BuiltInApp;
using System.Collections;
using System.Windows.Forms;
using GGPlatform.DBServer;
using GGPlatform.RFCManag;

namespace GGPlatform.MainApp
{
    public class ClassManagerWorkplaces
    {
        private ArrayList _Workplaces = null;
        private IWin32Window _MainWindow = null;
        private DBSqlServer _DBSqlServer = null;
        private RFCManager _rfc = null;
        private Lists _ActiveList = null;

        ~ClassManagerWorkplaces()
        {
            ///уничтожение ArrayList
            ///
            _DBSqlServer = null;
            _MainWindow = null;
            _Workplaces = null;
        }

        public ClassManagerWorkplaces(IWin32Window inMainWindow, DBSqlServer inDBSqlServer, RFCManager inRFCManager, ref Lists inActiveList)
        {
            _MainWindow = inMainWindow;
            _DBSqlServer = inDBSqlServer;
            _rfc = inRFCManager;
            _Workplaces = new ArrayList();
            _ActiveList = inActiveList;
        }

        public DBSqlServer DBSqlServer
        { 
            set {_DBSqlServer = value;}
        }

        public void AddWorkplace(ClassWorkplaces inWorkplace)
        {         
            _Workplaces.Add(inWorkplace);
        }
        public IPlugin SetPlugin(int inWorkplaceID, int inPluginID, string inSoftName)
        {
            IPlugin _CurrentPlugin = null;
            Boolean CreationFlag = false;
            IEnumerator _Enumerator = _Workplaces.GetEnumerator();
            ClassWorkplaces _place = null;
            string _IsAssemblyName = "";
            string _IsNamePlagin = "";
            while (_Enumerator.MoveNext())
            {
                _place = (ClassWorkplaces)_Enumerator.Current;
                if (_place.WorkplaceID == inWorkplaceID)
                {
                    _CurrentPlugin = _place.GetPlugin(inPluginID, ref CreationFlag, ref _IsAssemblyName, ref _IsNamePlagin);
                    _Enumerator = null;
                    break;
                }
            }
            if (CreationFlag)  //новый плагин, загружаю его и строю меню
            {
                _CurrentPlugin.plugPluginID = inPluginID;
                _CurrentPlugin.plugWorkplaceID = inWorkplaceID;
                _CurrentPlugin.plugAssemblyName = _IsAssemblyName;
                _CurrentPlugin.plugSoftName = inSoftName;
                _CurrentPlugin.plugName = _IsNamePlagin;
                _CurrentPlugin.plugInit(_MainWindow, _DBSqlServer, _rfc, ref _ActiveList);               
              //  _CurrentPlugin.plugMakeMenuStrip();
            }
            else
            { 
                ///////вызвать поиск окна
                IPlugin _PluginChild = null;
                foreach (Form _Form in ((Form)_MainWindow).MdiChildren)
                {
                    _PluginChild = (IPlugin)_Form;
                    if ((_PluginChild.plugWorkplaceID == inWorkplaceID) && (_PluginChild.plugPluginID == inPluginID))  //толи это окно???
                    {
                        _Form.Activate();
                        _PluginChild = null;
                        break;                       
                    }
                }               
            }
            return _CurrentPlugin;
        }
        public void DelPlugin(int inWorkplaceID, int inPluginID)
        {
            IEnumerator _Enumerator = _Workplaces.GetEnumerator();
            ClassWorkplaces _place = null;
            while (_Enumerator.MoveNext())
            {
                _place = (ClassWorkplaces)_Enumerator.Current;
                if (_place.WorkplaceID == inWorkplaceID)
                {
                    _place.DelPlugin(inPluginID);
                    _Enumerator = null;
                    break;
                }
            }           
        }

    }
}
