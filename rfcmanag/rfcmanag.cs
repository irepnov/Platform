using System;
using System.Collections;
using System.Windows.Forms;
using GGPlatform.DBServer;


//ПЕРЕПИСАТЬ ЗАГРУЗКУ НЕ ИЗ XML, А ИЗ бд, ТАК КАК ВЕЗДЕ ОДИНАКОВЫЕ СПРАВОЧНИКИ



namespace GGPlatform.RFCManag
{
    public class RFCManager
    {
        private ArrayList _RFCList = null;
        private IWin32Window _sMainHandle;
        private frmRFCForm _fmRFCForm = null;
        private string _RegistrySoftName = "";
        private GGPlatform.DBServer.DBSqlServer _DBSql = null;

        public RFCManager(IWin32Window inMainHandle, DBSqlServer inDBServer, string inRegistrySoftName)
        {
            _sMainHandle = inMainHandle;
            _DBSql = inDBServer;
            _RegistrySoftName = inRegistrySoftName;
            _RFCList = new ArrayList();
        }

        ~RFCManager()  //Form.Destroy ????
        {
            if ((_RFCList != null) && (_RFCList.Count > 0))
            {
               /* IEnumerator Enumerator = _RFCList.GetEnumerator();
                while (Enumerator.MoveNext())
                {
                    _fmRFCForm = null;
                    _fmRFCForm = (frmRFCForm)Enumerator.Current;
                    _RFCList.Remove(_fmRFCForm);
                    _fmRFCForm.Dispose();
                    _fmRFCForm = null;
                }
                Enumerator = null;*/
                _RFCList.Clear();
            }
            _DBSql = null;
            _RFCList = null;
        }

        public void LoadRFC(string inRFCName) //read xml, call CreateRFC in parameters OR Find to Array and LOAD
        {
            _fmRFCForm = null;

            //find in Array
            IEnumerator Enumerator = _RFCList.GetEnumerator();
            while (Enumerator.MoveNext())
            {
                _fmRFCForm = (frmRFCForm)Enumerator.Current;
                if (_fmRFCForm.RFCName.ToUpper() == inRFCName.ToUpper())   //UpperCase
                {
                    Enumerator = null;
                    return;
                }
            }
            Enumerator = null;

            _fmRFCForm = new frmRFCForm(_sMainHandle, inRFCName, _DBSql, _RegistrySoftName);
            _RFCList.Add(_fmRFCForm);
            _fmRFCForm = null;
        }

        public void UnLoadRFC(string inRFCName) //read xml, call CreateRFC in parameters OR Find to Array and LOAD
        {
            _fmRFCForm = null;

            //find in Array
            IEnumerator Enumerator = _RFCList.GetEnumerator();
            while (Enumerator.MoveNext())
            {
                _fmRFCForm = (frmRFCForm)Enumerator.Current;
                if (_fmRFCForm.RFCName.ToUpper() == inRFCName.ToUpper())   //UpperCase
                {
                    break;
                }
                else _fmRFCForm = null;
            }
            Enumerator = null;

            if (_fmRFCForm == null)
            {
                /*MessageBox.Show(_sMainHandle, "Справочник " + inRFCName + " не загружен",
                                "Ошибка [модуль rfcmanag, класс rfcmanag, метод UnLoadRFC]",
                                MessageBoxButtons.OK, MessageBoxIcon.Error); */
                throw new Exception("Справочник " + inRFCName + " не загружен");
                return;
            }

            try
            {
                _RFCList.Remove(_fmRFCForm);
                _fmRFCForm.Dispose();                
                _fmRFCForm = null;               
            }
            catch (Exception ex)
            {
                /*MessageBox.Show(_sMainHandle, "Ошибка закрытия справочника \n" + ex.Message,
                                "Ошибка [модуль rfcmanag, класс rfcmanag, метод UnLoadRFC]",
                                MessageBoxButtons.OK, MessageBoxIcon.Error); */
                _fmRFCForm = null;
                throw new Exception("Ошибка закрытия справочника \n" + ex.Message);
                
            }
        }

        /*  public DialogResult ShowRFC(string inRFCName)
          {
              _fmRFCForm = null;

              LoadRFC(inRFCName);  ///загрузим его, если он еще не загружен/

              //find in Array
              IEnumerator Enumerator = _RFCList.GetEnumerator();
              while (Enumerator.MoveNext())
              {
                  _fmRFCForm = (frmRFCForm)Enumerator.Current;
                  if (_fmRFCForm.RFCName.ToUpper() == inRFCName.ToUpper())   //UpperCase
                  {
                      break;
                  }
                  else _fmRFCForm = null;
              }
              Enumerator = null;

              if (_fmRFCForm == null)
              {
                  MessageBox.Show(_sMainHandle, "Справочник " + inRFCName + " не загружен",
                                  "Ошибка [модуль rfcmanag, класс rfcmanag, метод ShowRFC]",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error); 
                  ///throw new Exception("Справочник " + inRFCName + " не загружен");
                  return DialogResult.None;
              }

              try
              {
                  /// if _fmRFCForm = null   call LoadRFC  
                  /// 
                  return _fmRFCForm.ShowDialog(_sMainHandle);
              }
              catch (Exception ex)
              {
                  MessageBox.Show(_sMainHandle, "Ошибка просмотра справочника \n" + ex.Message,
                                  "Ошибка [модуль rfcmanag, класс rfcmanag, метод ShowRFC]",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error); 
                  //throw new Exception("Ошибка просмотра справочника \n" + ex.Message);
                  return DialogResult.None;
              }
          }*/

        public DialogResult ShowRFC(string inRFCName, bool inMultiSelect = true)
        {
            _fmRFCForm = null;

            LoadRFC(inRFCName);  ///загрузим его, если он еще не загружен/

            //find in Array
            IEnumerator Enumerator = _RFCList.GetEnumerator();
            while (Enumerator.MoveNext())
            {
                _fmRFCForm = (frmRFCForm)Enumerator.Current;
                if (_fmRFCForm.RFCName.ToUpper() == inRFCName.ToUpper())   //UpperCase
                {
                    _fmRFCForm._RFCMultiSelect = inMultiSelect;
                    break;
                }
                else _fmRFCForm = null;
            }
            Enumerator = null;

            if (_fmRFCForm == null)
            {
                MessageBox.Show(_sMainHandle, "Справочник " + inRFCName + " не загружен",
                                "Ошибка [модуль rfcmanag, класс rfcmanag, метод ShowRFC]",
                                MessageBoxButtons.OK, MessageBoxIcon.Error); 
                //throw new Exception("Справочник " + inRFCName + " не загружен");
                return DialogResult.None;
            }

            try
            {
                /// if _fmRFCForm = null   call LoadRFC  
                /// 
                return _fmRFCForm.ShowDialog(_sMainHandle);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_sMainHandle, "Ошибка просмотра справочника \n" + ex.Message,
                                "Ошибка [модуль rfcmanag, класс rfcmanag, метод ShowRFC]",
                                MessageBoxButtons.OK, MessageBoxIcon.Error); 
                //throw new Exception("Ошибка просмотра справочника \n" + ex.Message);
                return DialogResult.None;
            }
        }

        public void RefreshRFC(string inRFCName)
        {
            //////////////////проблемы
            //после Рефреша, галочка в заголовке столбце Двоится (появяляется еще одна галочка)
            //если у спраовничка режим Одиночный выбор, то все равно программно получается постаить несколько галочек методом SetRFCValue

            _fmRFCForm = null;

            //find in Array
            IEnumerator Enumerator = _RFCList.GetEnumerator();
            while (Enumerator.MoveNext())
            {
                _fmRFCForm = (frmRFCForm)Enumerator.Current;
                if (_fmRFCForm.RFCName.ToUpper() == inRFCName.ToUpper())   //UpperCase
                {                    
                    break;
                }
                else _fmRFCForm = null;
            }
            Enumerator = null;

            if (_fmRFCForm == null)
            {
                /*MessageBox.Show(_sMainHandle, "Справочник " + inRFCName + " не загружен",
                                "Ошибка [модуль rfcmanag, класс rfcmanag, метод RefreshRFC]",
                                MessageBoxButtons.OK, MessageBoxIcon.Error); */
                throw new Exception("Справочник " + inRFCName + " не загружен");
                return;
            }

            try
            {
                _fmRFCForm.RefreshRFC();
                _fmRFCForm = null;
            }
            catch (Exception ex)
            {
                /*MessageBox.Show(_sMainHandle, "Ошибка обновления справочника \n" + ex.Message,
                                "Ошибка [модуль rfcmanag, класс rfcmanag, метод RefreshRFC]",
                                MessageBoxButtons.OK, MessageBoxIcon.Error); */
                _fmRFCForm = null;
                throw new Exception("Ошибка обновления справочника \n" + ex.Message);
            }
        }

        public string GetRFCValueOne(string inRFCName, string inFieldName)
        {
            _fmRFCForm = null;
            string _str = String.Empty;

            //find in Array
            IEnumerator Enumerator = _RFCList.GetEnumerator();
            while (Enumerator.MoveNext())
            {
                _fmRFCForm = (frmRFCForm)Enumerator.Current;
                if (_fmRFCForm.RFCName.ToUpper() == inRFCName.ToUpper())   //UpperCase
                {
                    break;
                }
                else _fmRFCForm = null;
            }
            Enumerator = null;

            if (_fmRFCForm == null)
            {
                /*MessageBox.Show(_sMainHandle, "Справочник " + inRFCName + " не загружен",
                                "Ошибка [модуль rfcmanag, класс rfcmanag, метод GetRFCValueOne]",
                                MessageBoxButtons.OK, MessageBoxIcon.Error); */
                throw new Exception("Справочник " + inRFCName + " не загружен");
                return null;
            }

            try
            {
                _str = _fmRFCForm.GetRFCValueOne(inFieldName);
                _fmRFCForm = null;
                return _str;
            }
            catch (Exception ex)
            {
                /*MessageBox.Show(_sMainHandle, "Ошибка получения значения из справочника \n" + ex.Message,
                                "Ошибка [модуль rfcmanag, класс rfcmanag, метод GetRFCValueOne]",
                                MessageBoxButtons.OK, MessageBoxIcon.Error); */
                _fmRFCForm = null;
                throw new Exception("Ошибка получения значения из справочника \n" + ex.Message);
                return null;
            }
        }

        public string GetRFCValueMulti(string inRFCName, string inFieldName)
        {
            _fmRFCForm = null;
            string _str = String.Empty;

            //find in Array
            IEnumerator Enumerator = _RFCList.GetEnumerator();
            while (Enumerator.MoveNext())
            {
                _fmRFCForm = (frmRFCForm)Enumerator.Current;
                if (_fmRFCForm.RFCName.ToUpper() == inRFCName.ToUpper())   //UpperCase
                {
                    break;
                }
                else _fmRFCForm = null;
            }
            Enumerator = null;

            if (_fmRFCForm == null)
            {
                /*MessageBox.Show(_sMainHandle, "Справочник " + inRFCName + " не загружен",
                                "Ошибка [модуль rfcmanag, класс rfcmanag, метод GetRFCValueMulti]",
                                MessageBoxButtons.OK, MessageBoxIcon.Error); */
                throw new Exception("Справочник " + inRFCName + " не загружен");
                return null;
            }

            try
            {
                _str = _fmRFCForm.GetRFCValueMulti(inFieldName);
                _fmRFCForm = null;
                return _str;
            }
            catch (Exception ex)
            {
                /*MessageBox.Show(_sMainHandle, "Ошибка получения значения из справочника \n" + ex.Message,
                                "Ошибка [модуль rfcmanag, класс rfcmanag, метод GetRFCValueMulti]",
                                MessageBoxButtons.OK, MessageBoxIcon.Error); */
                _fmRFCForm = null;
                throw new Exception("Ошибка получения значения из справочника \n" + ex.Message);
                return null;
            }
        }

        public ArrayList GetRFCValueArrayList(string inRFCName, string inFieldName)
        {
            _fmRFCForm = null;
            ArrayList _str = null;

            //find in Array
            IEnumerator Enumerator = _RFCList.GetEnumerator();
            while (Enumerator.MoveNext())
            {
                _fmRFCForm = (frmRFCForm)Enumerator.Current;
                if (_fmRFCForm.RFCName.ToUpper() == inRFCName.ToUpper())   //UpperCase
                {
                    break;
                }
                else _fmRFCForm = null;
            }
            Enumerator = null;

            if (_fmRFCForm == null)
            {
                /*MessageBox.Show(_sMainHandle, "Справочник " + inRFCName + " не загружен",
                                "Ошибка [модуль rfcmanag, класс rfcmanag, метод GetRFCValueArrayList]",
                                MessageBoxButtons.OK, MessageBoxIcon.Error); */
                throw new Exception("Справочник " + inRFCName + " не загружен");
                return null;
            }

            try
            {
                _str = _fmRFCForm.GetRFCValueArrayList(inFieldName);
                _fmRFCForm = null;
                return _str;
            }
            catch (Exception ex)
            {
                /*MessageBox.Show(_sMainHandle, "Ошибка получения значения из справочника \n" + ex.Message,
                                "Ошибка [модуль rfcmanag, класс rfcmanag, метод GetRFCValueArrayList]",
                                MessageBoxButtons.OK, MessageBoxIcon.Error); */
                _fmRFCForm = null;
                throw new Exception("Ошибка получения значения из справочника \n" + ex.Message);
                return null;
            }
        }

        public void SetRFCValue(string inRFCName, string inFieldName, string inFieldValue)
        {
            _fmRFCForm = null;
 
            //find in Array
            IEnumerator Enumerator = _RFCList.GetEnumerator();
            while (Enumerator.MoveNext())
            {
                _fmRFCForm = (frmRFCForm)Enumerator.Current;
                if (_fmRFCForm.RFCName.ToUpper() == inRFCName.ToUpper())   //UpperCase
                {
                    break;
                }
                else _fmRFCForm = null;
            }
            Enumerator = null;

            if (_fmRFCForm == null)
            {
                /*MessageBox.Show(_sMainHandle, "Справочник " + inRFCName + " не загружен",
                                "Ошибка [модуль rfcmanag, класс rfcmanag, метод SetRFCValue]",
                                MessageBoxButtons.OK, MessageBoxIcon.Error); */
                throw new Exception("Справочник " + inRFCName + " не загружен");
                return;
            }

            try
            {
                _fmRFCForm.SetRFCValue(inFieldName, inFieldValue);
                _fmRFCForm = null;
                return;
            }
            catch (Exception ex)
            {
                /*MessageBox.Show(_sMainHandle, "Ошибка получения значения из справочника \n" + ex.Message,
                                "Ошибка [модуль rfcmanag, класс rfcmanag, метод SetRFCValue]",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);*/
                _fmRFCForm = null;
                throw new Exception("Ошибка получения значения из справочника \n" + ex.Message);
                return;
            }
        }
    }
}
