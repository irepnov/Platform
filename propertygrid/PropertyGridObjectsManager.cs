using System;
using System.Windows.Forms;
using System.Collections;
using System.Drawing;
using System.Data;
using System.Drawing.Design;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace GGPlatform.NSPropertyGridAddClasses
{
    #region AdditionalClases
    public class PropertyOrderPair : IComparable
    {
        private int _order;
        private string _name;
        public string Name
        {
            get { return _name; }
        }

        public PropertyOrderPair(string name, int order)
        {
            _order = order;
            _name = name;
        }

        /// <summary>
        /// Собственно метод сравнения
        /// </summary>
        public int CompareTo(object obj)
        {
            int otherOrder = ((PropertyOrderPair)obj)._order;
            if (otherOrder == _order)
            {
                // если Order одинаковый - сортируем по именам
                string otherName = ((PropertyOrderPair)obj)._name;
                return string.Compare(_name, otherName);
            }
            else if (otherOrder > _order)
            {
                return -1;
            }
            return 1;
        }
    }

    public class PropertySorter : ExpandableObjectConverter
    {
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        /// <summary>
        /// Возвращает упорядоченный список свойств
        /// </summary>
        public override PropertyDescriptorCollection GetProperties(
           ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(value, attributes);
            ArrayList orderedProperties = new ArrayList();
            foreach (PropertyDescriptor pd in pdc)
            {
                Attribute attribute = pd.Attributes[typeof(PropertyOrderAttribute)];
                if (attribute != null)
                {
                    // атрибут есть - используем номер п/п из него
                    PropertyOrderAttribute poa = (PropertyOrderAttribute)attribute;
                    orderedProperties.Add(new PropertyOrderPair(pd.Name, poa.Order));
                }
                else
                {
                    // атрибута нет - считаем что 0
                    orderedProperties.Add(new PropertyOrderPair(pd.Name, 0));
                }
            }

            // сортируем по Order-у
            orderedProperties.Sort();

            // формируем список имен свойств
            ArrayList propertyNames = new ArrayList();
            foreach (PropertyOrderPair pop in orderedProperties)
            {
                propertyNames.Add(pop.Name);
            }

            // возвращаем
            return pdc.Sort((string[])propertyNames.ToArray(typeof(string)));
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyOrderAttribute : Attribute
    {
        private int _order;
        public PropertyOrderAttribute(int order)
        {
            _order = order;
        }

        public int Order
        {
            get { return _order; }
        }
    }

    public enum MyEnum
	{
		FirstEntry,
		SecondEntry,
		ThirdEntry
    }
    #endregion

    #region AdditionalExpandTypeClasses
    [Serializable]
    [TypeConverter(typeof(PropertySorter))]
    [ComVisible(true)]
    public class CExpandTypeInt
    { 
        private int x, y;
        [DisplayName("от")]
        [PropertyOrder(10)]
        public int FirstElement
        {
            get { return x; }
            set { x = value; }
        }        
        [DisplayName("до")]
        [PropertyOrder(20)]
        public int LasElement
        {
            get { return y; }
            set { y = value; }
        }        
        public CExpandTypeInt(int x1, int y1)
        {
            x = x1;
            y = y1;
        }
        public override string ToString()
        {
            return Convert.ToString(x) + ";" + Convert.ToString(y);
        }
    }
    [Serializable]
    [TypeConverter(typeof(PropertySorter))]
    [ComVisible(true)]
    public class CExpandTypeDouble
    {
        private double x, y;
        [DisplayName("от")]
        [PropertyOrder(10)]
        public double FirstElement
        {
            get { return x; }
            set { x = value; }
        }
        [DisplayName("до")]
        [PropertyOrder(20)]
        public double LasElement
        {
            get { return y; }
            set { y = value; }
        }
        public CExpandTypeDouble(double x1, double y1)
        {
            x = x1;
            y = y1;
        }
        public override string ToString()
        {
            return Convert.ToString(x) + ";" + Convert.ToString(y);
        }
    }

    #endregion
    //--------------------------------------------------------------------------------------------------------------
    [Serializable]
    [TypeConverter(typeof(PropertySorter))]
    [ComVisible(true)]
    public class CExpandCustomType<T>
    {
        private T x, y;
        //--------------------------------------------------------------------------------------------------------------
        [DisplayName("от")]
        [PropertyOrder(10)]
        public T FirstElement
        {
            get { return x; }
            set { x = value; }
        }
        //--------------------------------------------------------------------------------------------------------------
        [DisplayName("до")]
        [PropertyOrder(20)]
        public T LasElement
        {
            get { return y; }
            set { y = value; }
        }
        //--------------------------------------------------------------------------------------------------------------
        public CExpandCustomType(T x1, T y1)
        {
            x = x1;
            y = y1;
        }
        //--------------------------------------------------------------------------------------------------------------
        public CExpandCustomType()
        {     
        }
        //--------------------------------------------------------------------------------------------------------------
        public override string ToString()
        {
           
            Type ControlType = typeof(T);
            if (ControlType == typeof(DateTime))
            {
                if (Convert.ToDateTime(x) == DateTime.MinValue && Convert.ToDateTime(y) == DateTime.MinValue)
                    return "";
                else if (Convert.ToDateTime(x) != DateTime.MinValue && Convert.ToDateTime(y) != DateTime.MinValue)
                    return Convert.ToDateTime(x).ToShortDateString() + ";" +
                            Convert.ToDateTime(y).ToShortDateString();
                else if (Convert.ToDateTime(x) == DateTime.MinValue && Convert.ToDateTime(y) != DateTime.MinValue)
                    return ";" + Convert.ToDateTime(y).ToShortDateString();
                else if (Convert.ToDateTime(x) != DateTime.MinValue && Convert.ToDateTime(y) == DateTime.MinValue)
                    return Convert.ToDateTime(x).ToShortDateString() + ";";
            }
            else
            {
                if (Convert.ToString(x) == "" && Convert.ToString(y) == "")
                    return "";
                else return Convert.ToString(x) + ";" + Convert.ToString(y);
            }
            return "";
        }
        //--------------------------------------------------------------------------------------------------------------
    }
    //--------------------------------------------------------------------------------------------------------------
    [Serializable()]
    public class CListKeyValue<Type1, Type2>
	{
        private Type2 iKey;
        private Type1 sValue;
        //--------------------------------------------------------------------------------------------------------------
        public CListKeyValue(Type1 Text, Type2 Value)
		{
            sValue = Text;
            iKey = Value;
		}
        //--------------------------------------------------------------------------------------------------------------
        public Type2 PKey
		{
			get
			{
                return iKey;
			}
			set
			{
                iKey = value;
			}
		}
        //--------------------------------------------------------------------------------------------------------------
        public Type1 PValue
		{
			get
			{
                return sValue;
			}
			set
			{
                sValue = value;
			}
		}
        //--------------------------------------------------------------------------------------------------------------
    }
    //--------------------------------------------------------------------------------------------------------------
    public class StringManager
    {
        public static List<string> ParsingString(string Separator_, string ResourceStr_)
        {
            List<string> ListStr = new List<string>();

            int CurPossition_ = 0;
            ResourceStr_ = ResourceStr_.Trim();

            string TmpStr_ = ResourceStr_;

            if (ResourceStr_ == "" || Separator_ == "")
                return null;

            CurPossition_ = ResourceStr_.IndexOf(Separator_);
            if (CurPossition_ == -1)
            {
                ListStr.Add(ResourceStr_);
                return ListStr;
            }
            else
            {
                while (CurPossition_ != -1)
                {
                    ListStr.Add(TmpStr_.Trim().Substring(0, CurPossition_));

                    TmpStr_ = TmpStr_.Trim().Substring(CurPossition_ + Separator_.Length, TmpStr_.Length - CurPossition_ - Separator_.Length);

                    CurPossition_ = TmpStr_.IndexOf(Separator_);
                }
                if (TmpStr_.Length > 0)
                {
                    ListStr.Add(TmpStr_.Trim());
                }
            }
            return ListStr;
        }
    }
    //--------------------------------------------------------------------------------------------------------------
    #region ConvertorRegions
    //--------------------------------------------------------------------------------------------------------------
    public class MyStringConverter : MultilineStringConverter
	{
		
		public override object ConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, System.Type destinationType)
		{
			return "(My multiline string editor)";
		}
	}
    //--------------------------------------------------------------------------------------------------------------
	public class MyEditor : System.Drawing.Design.UITypeEditor
	{
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			if (context != null&& context.Instance != null)
			{
				if (! context.PropertyDescriptor.IsReadOnly)
				{
					return UITypeEditorEditStyle.Modal;
				}
			}
			return UITypeEditorEditStyle.None;
		}
        //--------------------------------------------------------------------------------------------------------------
		[RefreshProperties(RefreshProperties.All)]public override object EditValue(ITypeDescriptorContext context, System.IServiceProvider provider, object value)
		{
			if (context == null || provider == null || context.Instance == null)
			{
				return base.EditValue(provider, value);
			}
			if (MessageBox.Show("Ответьте мне", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Information)==DialogResult.Yes) 
			{
				value = true;
			}
			else
			{
				value = false;
			}
			return value;
		}
	}
    //--------------------------------------------------------------------------------------------------------------
    public class CTypeConverterInt32 : Int32Converter
    {
        private bool bMsgboxIsVisible = false;
        public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            try
            {
                if (Convert.ToString(value) == "") return ""; 
                return base.ConvertFrom(context, culture, value);
            }
            catch (Exception ex)
            {
                if (!bMsgboxIsVisible)
                {
                    bMsgboxIsVisible = true;

                    MessageBox.Show("Сообщение: " + ex.Message, "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                bMsgboxIsVisible = false;
                return context.PropertyDescriptor.GetValue(this);
            }
        }
    }
    //--------------------------------------------------------------------------------------------------------------
    public class CTypeConverterDouble : DoubleConverter
    {
        private bool bMsgboxIsVisible = false;
        public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            try
            {
                if (Convert.ToString(value) == "") return "";
                return base.ConvertFrom(context, culture, value);
            }
            catch (Exception ex)
            {
                if (!bMsgboxIsVisible)
                {
                    bMsgboxIsVisible = true;
                    MessageBox.Show("Сообщение: " + ex.Message, "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                bMsgboxIsVisible = false;
                return context.PropertyDescriptor.GetValue(this);
            }
        }
    }
    //--------------------------------------------------------------------------------------------------------------
    public class CTypeConverterDateTime : DateTimeConverter
    {
        private bool bMsgboxIsVisible = false;
        public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            try
            {
                return base.ConvertFrom(context, culture, value);
            }
            catch (Exception ex)
            {
                if (!bMsgboxIsVisible)
                {
                    bMsgboxIsVisible = true;
                    MessageBox.Show("Сообщение: " + ex.Message, "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                bMsgboxIsVisible = false;
                return context.PropertyDescriptor.GetValue(this);
            }
        }
    }
    //--------------------------------------------------------------------------------------------------------------
    #endregion
}
