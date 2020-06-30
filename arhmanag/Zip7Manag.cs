using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;

namespace GGPlatform.Zip7Manag
{
    public class Zip7Manager
    {
        private string _7zEXE = "";
        private ProcessWindowStyle _WindowStyle;

        public string File7Zip
        {
            get { return _7zEXE; }
            set
            {
                if (!File.Exists(value)) throw new Exception("Архиватор " + value + " не найден");
                if (!File.Exists(value.Replace("7z.exe", "7-zip.dll"))) throw new Exception("Библиотека " + value.Replace("7z.exe", "7-zip.dll") + " не найдена");

                _7zEXE = value;
            }
        }
        public ProcessWindowStyle WindowStyle
        {
            get { return _WindowStyle; }
            set { _WindowStyle = value; }
        }

        public Zip7Manager()
        {
            File7Zip = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\7z.exe";
            WindowStyle = ProcessWindowStyle.Hidden;
        }
        public Zip7Manager(string inFile7Zip, ProcessWindowStyle inWindowStyle)
        {
            File7Zip = inFile7Zip;
            _WindowStyle = inWindowStyle;
        }

        private void AddToArchive(string inArhiveFileName, string inFileToArhive)
        {
            if (!Directory.Exists(Path.GetDirectoryName(inArhiveFileName)))
                Directory.CreateDirectory(inArhiveFileName);

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = _7zEXE;
            // добавить в архив с максимальным сжатием
            startInfo.Arguments = " a -r -y -mx9 ";
            // имя архива
            startInfo.Arguments += "\"" + inArhiveFileName + "\"";
            // файлы для запаковки
            startInfo.Arguments += " \"" + inFileToArhive + "\"";
            startInfo.WindowStyle = _WindowStyle;
            int sevenZipExitCode = 0;
            using (Process sevenZip = Process.Start(startInfo))
            {
                sevenZip.WaitForExit();
                sevenZipExitCode = sevenZip.ExitCode;
            }
            // Если с первого раза не получилось,
            //пробуем еще раз через 1 секунду
            if (sevenZipExitCode != 0 && sevenZipExitCode != 1)
            {
                using (Process sevenZip = Process.Start(startInfo))
                {
                    Thread.Sleep(2000);
                    sevenZip.WaitForExit();
                    switch (sevenZip.ExitCode)
                    {
                        case 0: return; // Без ошибок и предупреждений
                        case 1: return; // Есть некритичные предупреждения
                        case 2: throw new Exception("Фатальная ошибка");
                        case 7: throw new Exception("Ошибка в командной строке");
                        case 8: throw new Exception("Недостаточно памяти для выполнения операции");
                        case 225: throw new Exception("Пользователь отменил выполнение операции");
                        default: throw new Exception("Архиватор 7z вернул недокументированный код ошибки: " + sevenZip.ExitCode.ToString());
                    }
                }
            }
            startInfo = null;
        }

        public void ExtractFromArchive(string inArhiveFileName, string inExtractFolder = "")
        {
            // Предварительные проверки
            if (inExtractFolder != "")
                if (!Directory.Exists(inExtractFolder))
                    Directory.CreateDirectory(inExtractFolder);
 
            // Формируем параметры вызова 7z
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = _7zEXE;
            // Распаковать (для полных путей - x)
            startInfo.Arguments = " e -y ";
            // Файл, который нужно распаковать
            startInfo.Arguments += "\"" + inArhiveFileName + "\"";
            // Папка распаковки
            if (inExtractFolder != "")
                startInfo.Arguments += " -o" + "\"" + inExtractFolder + "\"";

            startInfo.WindowStyle = _WindowStyle;
            int sevenZipExitCode = 0;
            using (Process sevenZip = Process.Start(startInfo))
            {
                sevenZip.WaitForExit();
                sevenZipExitCode = sevenZip.ExitCode;
            }
            // Если с первого раза не получилось,
            //пробуем еще раз через 1 секунду
            if (sevenZipExitCode != 0 && sevenZipExitCode != 1)
            {
                using (Process sevenZip = Process.Start(startInfo))
                {
                    Thread.Sleep(2000);
                    sevenZip.WaitForExit();
                    switch (sevenZip.ExitCode)
                    {
                        case 0: return; // Без ошибок и предупреждений
                        case 1: return; // Есть некритичные предупреждения
                        case 2: throw new Exception("Фатальная ошибка");
                        case 7: throw new Exception("Ошибка в командной строке");
                        case 8: throw new Exception("Недостаточно памяти для выполнения операции");
                        case 225: throw new Exception("Пользователь отменил выполнение операции");
                        default: throw new Exception("Архиватор 7z вернул недокументированный код ошибки: " + sevenZip.ExitCode.ToString());
                    }
                }
            }
            startInfo = null;
        }
        

        private void DeleteFile(string inFileNameOrMask)
        {
            if ((!inFileNameOrMask.Contains("*")) && (!inFileNameOrMask.Contains("?")))
            {
                if (File.Exists(inFileNameOrMask)) File.Delete(inFileNameOrMask);  //удаление конечного файла
            }
            else
            {
                //удаление файлов по маске
                string _mask = inFileNameOrMask.Replace(Path.GetDirectoryName(inFileNameOrMask), "").Replace("\\", "");
                string[] allFoundFiles = Directory.GetFiles(Path.GetDirectoryName(inFileNameOrMask), _mask, SearchOption.TopDirectoryOnly);
                foreach (string file in allFoundFiles)
                    if (File.Exists(file)) File.Delete(file);
            }
        }

        public void ToArhiveFileList(string inArhiveFileName, List<string> inFilesToArhiveOrMask, Boolean isDeleteFiles = false)
        {            
            if (inFilesToArhiveOrMask.Count <= 0) throw new Exception("Не указан список файлов для архивирования");

            foreach (string file in inFilesToArhiveOrMask)
            {
                if ((!file.Contains("*")) && (!file.Contains("?")))
                    if (!File.Exists(file)) throw new Exception("Файл " + file + " не найден");
            }

            foreach (string file in inFilesToArhiveOrMask)
            {
                AddToArchive(inArhiveFileName, file);
                if (isDeleteFiles) DeleteFile(file);
            }
        }

        public void ToArhiveFile(string inArhiveFileName, string inFileToArhiveOrMask, Boolean isDeleteFiles = false)
        {
            if ((!inFileToArhiveOrMask.Contains("*")) && (!inFileToArhiveOrMask.Contains("?"))) //если указана маска файла, то не проверять на наличие файла
                if (!File.Exists(inFileToArhiveOrMask)) throw new Exception("Файл " + inFileToArhiveOrMask + " не найден");

            AddToArchive(inArhiveFileName, inFileToArhiveOrMask);
            if (isDeleteFiles) DeleteFile(inFileToArhiveOrMask);
        }



      /*  public void ToArhiveMaskFiles(string inArhiveFileName, string inMaskFileToArhive, Boolean isDeleteFiles = false)
        {
            if (!File.Exists(_7zEXE)) throw new Exception("Архиватор " + _7zEXE + " не найден");
            if (!File.Exists(_7zEXE.Replace("7z.exe", "7-zip.dll"))) throw new Exception("Библиотека " + _7zEXE.Replace("7z.exe", "7-zip.dll") + " не найдена");
            if (!Directory.Exists(Path.GetDirectoryName(inArhiveFileName))) throw new Exception("Директория конечного архива " + inArhiveFileName + " не найдена");

            string _mask = inMaskFileToArhive.Replace(Path.GetDirectoryName(inArhiveFileName), "");
            string[] allFoundFiles = Directory.GetFiles(Path.GetDirectoryName(inMaskFileToArhive), _mask, SearchOption.TopDirectoryOnly);

            foreach (string file in allFoundFiles)
                if (!File.Exists(file)) throw new Exception("Файл " + file + " не найден");

            foreach (string file in allFoundFiles)
            {
                AddToArchive(inArhiveFileName, file);
                if (isDeleteFiles) File.Delete(file);
            }
        }*/
    }
}
