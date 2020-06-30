using System;
using System.Collections.Generic;
using System.Text;
using GGPlatform.BuiltInApp;
using System.Collections;
using System.Windows.Forms;

namespace GGPlatform.MainApp
{
    public class ClassWorkplaces
    {
        private int _WorkplaceID = 0;
        private IWin32Window _ParentWindow;
        private ArrayList _Plugins = null;

        public ClassWorkplaces(int inWorkplaceID, IWin32Window inParentWindow)
        {
            _WorkplaceID = inWorkplaceID;
            _ParentWindow = inParentWindow;
            _Plugins = new ArrayList();
        }
        ~ClassWorkplaces()
        {           
            while (_Plugins.Count > 0)
            {
                _Plugins.RemoveAt(_Plugins.Count - 1);
            }    
            _Plugins = null;
        }

        public void AddPlugin(int inPluginID, string inAssemblyName, string inPluginName)
        {
            ClassPlugins _Plugin = new ClassPlugins(inPluginID, inAssemblyName, inPluginName);
            _Plugins.Add(_Plugin);
        }
        public void DelPlugin(int inPluginID)
        {
            IEnumerator Enumerator = _Plugins.GetEnumerator();
            ClassPlugins Plugin = null;
            while (Enumerator.MoveNext())
            {
                Plugin = (ClassPlugins)Enumerator.Current;
                if (Plugin.PluginID == inPluginID)
                {
                    if (Plugin.PluginObject != null)
                        Plugin.PluginObject.plugDeinit();
                    Plugin.PluginObject = null;
                    Enumerator = null;
                    Plugin = null;
                    break;
                }
            }
            Plugin = null;
            Enumerator = null;
        }
        public IPlugin GetPlugin(int inPluginID, ref Boolean IsNewPlugin, ref string IsAssemblyName, ref string IsNamePlagin)
        {
            IEnumerator Enumerator = _Plugins.GetEnumerator();
            ClassPlugins Plugin = null;

            while (Enumerator.MoveNext())
            {
                Plugin = (ClassPlugins)Enumerator.Current;
                
                if (Plugin.PluginID == inPluginID)
                {
                    IsAssemblyName = Plugin.AssemblyName;
                    IsNamePlagin = Plugin.PluginName;
                    IsNewPlugin = false;
                    if (Plugin.PluginObject == null)
                    {
                        try
                        {
                            IsNewPlugin = true;
                            Plugin.CreatePlugin(_ParentWindow);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(_ParentWindow,
                                            "Ошибка загрузки плагина /n" + ex.Message,
                                            "Ошибка [модуль AppMain, класс ClassWorkplaces, метод GetPlugin]",
                                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return null;
                        }
                    }                  
                    return Plugin.PluginObject;              
                }
            }
            Enumerator = null;
            Plugin = null;
            return null;
        }
        public int WorkplaceID { get { return _WorkplaceID; } }
    }
}
