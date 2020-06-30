using System;
using System.Collections.Generic;
using System.Text;
using GGPlatform.BuiltInApp;
using System.Collections;
using System.Windows.Forms;
using System.Reflection;

namespace GGPlatform.MainApp
{
    public class ClassPlugins
    {
        private int _PluginID = 0;
        private string _AssemblyName = "";
        private string _PluginName = "";
        private IPlugin _PluginObject = null;
        private Assembly _Assembly = null;

        ~ClassPlugins()
        {
            if (_PluginObject != null)
            {
                _PluginObject.plugDeinit();
                _PluginObject = null;
            }
        }
        public ClassPlugins(int inPluginID, string inAssemblyName, string inPlaginName)
        {
            _PluginID = inPluginID;
            _AssemblyName = inAssemblyName;
            _PluginName = inPlaginName;
        }      
        public string AssemblyName { get { return _AssemblyName; } }
        public string PluginName { get { return _PluginName; } }
        public int PluginID { get { return _PluginID; } }
        public IPlugin PluginObject
        {
            get { return _PluginObject; }
            set { _PluginObject = value; }
        }       
        public void CreatePlugin(IWin32Window inParentWindow)
        {          
            _PluginObject = null;
            try
            {
                _Assembly = Assembly.Load(_AssemblyName);
                foreach (Type type in _Assembly.GetTypes())
                {
                    Type iface = type.GetInterface("IPlugin", true);
                    if (iface != null)
                    {
                        _PluginObject = (IPlugin)Activator.CreateInstance(type);
                        iface = null;
                    }
                }
            }
            catch (Exception ex)
            { 
                MessageBox.Show(inParentWindow,
                                "Ошибка загрузки сборки (" + _AssemblyName + ") \n" + ex.Message,
                                "Ошибка [модуль appmain, класс ClassPlugins, метод CreatePlugin]",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                _PluginObject = null;
            }

        }
    }
}
