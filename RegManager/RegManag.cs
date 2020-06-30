using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace GGPlatform.RegManager
{
    public class RegManag
    {
        public void GGRegistrySetValue(string Key, object Value, string RegistryPath)
        {
            RegistryKey currRegistryKey = Registry.CurrentUser.CreateSubKey(RegistryPath);
            if (currRegistryKey != null)
            {
                currRegistryKey.SetValue(Key, Value);
                currRegistryKey.Close();
            }
        }
        // возвращает значение из реестра
        public object GGRegistryGetValue(string Key, string RegistryPath)
        {
            object val = null;
            RegistryKey currRegistryKey = Registry.CurrentUser.CreateSubKey(RegistryPath);
            if (currRegistryKey != null)
            {
                val = currRegistryKey.GetValue(Key);
                currRegistryKey.Close();
            }
            return val;
        }
        public bool GGRegistryExport(string Filename, string RegistryPath)
        {
            if (!Directory.Exists(Path.GetDirectoryName(Filename)))
                Directory.CreateDirectory(Path.GetDirectoryName(Filename));

            using (Process regpr = Process.Start(
                                                    new ProcessStartInfo("reg", string.Format("export " + RegistryPath + " \"{0}\" /y", Filename))
                                                    {
                                                        WindowStyle = ProcessWindowStyle.Hidden
                                                    }
                                                ))
            {
                regpr.WaitForExit();
                if (regpr.ExitCode == 1)
                    throw new Exception("Ошибка экспорта ветви реестра \n" + RegistryPath);
            }

         /*   Process.Start(new ProcessStartInfo("reg", string.Format("export " + RegistryPath + " \"{0}\" /y", Filename))
            {
                WindowStyle = ProcessWindowStyle.Hidden
            }
                ).WaitForExit();*/
            return true;
         }
        public bool GGRegistryImport(string Filename)
        {
            if (!File.Exists(Filename))
                throw new Exception("Файл настроек интерфейса не найден \n" + Filename);

            using (Process regpr = Process.Start(
                                                    new ProcessStartInfo("reg", string.Format("import \"{0}\" ", Filename))
                                                    {
                                                        WindowStyle = ProcessWindowStyle.Hidden
                                                    }
                                                ))
            {
                Thread.Sleep(1000);
                regpr.WaitForExit();
                if (regpr.ExitCode == 1)
                    throw new Exception("Ошибка импорта ветви реестра");
            }
            return true;
        }

    }
}
