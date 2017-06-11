using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using System.ComponentModel;
using System.Reflection;

namespace ServerFrame.Editor
{
    public class Template : Attribute
    {
        public Template()
        {

        }

    }

    public class CDataEditorAttribute : Attribute
    {
        public string m_strFileExt = "";

        public CDataEditorAttribute(string strFileExt)
        {
            m_strFileExt = strFileExt;
        }
    }

    interface IComboxProList
    {
        List<string> GetdataSource();
    }

    /// <summary>
    /// 下拉式编辑器
    /// </summary>
    public class DropTypeDialogEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context != null && context.Instance != null)
            {
                return UITypeEditorEditStyle.DropDown;//显示下拉箭头
            }
            return base.GetEditStyle(context);
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            System.Windows.Forms.Design.IWindowsFormsEditorService editorService = null;
            if (context != null && context.Instance != null && provider != null)
            {
                editorService = (System.Windows.Forms.Design.IWindowsFormsEditorService)provider.GetService(typeof(System.Windows.Forms.Design.IWindowsFormsEditorService));
                if (editorService != null)
                {
                    var type = context.Instance.GetType();
                    var tMethod = type.GetMethod("GetdataSource");
                    ListBox comboBox = new ListBox();
                    comboBox.FormattingEnabled = true;
                    comboBox.Name = "comboBox";
                    comboBox.Size = new System.Drawing.Size(100, 200);
                    comboBox.DataSource = tMethod.Invoke(context.Instance, null);
                    editorService.DropDownControl(comboBox);
                    value = comboBox.SelectedItem;
                    return value;

                }

            }
            return value;
        }

    }


    public class MaterialStringEditor : System.Drawing.Design.UITypeEditor
    {
        ////UISystem.MaterialParameterEditor.OnEditValue = MaterialStringEditor.OnEditValue;
        //public static MidLayer.IMaterialParameter OnEditValue()
        //{
        //    MaterialForm form = new MaterialForm();
        //    form.ShowDialog();
        //    return form.SelectMaterial;
        //}
        // 选择自定义项风格
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            // 此枚举决定自定义项将显示按钮
            return UITypeEditorEditStyle.Modal;
        }

        // 点击自定义项中的按钮时触发此方法
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            // 自定义点击按钮后触发操作
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (null != edSvc)
            {
                var curPath = System.IO.Directory.GetCurrentDirectory();
                var subPath = curPath.Remove(curPath.IndexOf("Kiang") + 5);

                OpenFileDialog form = new OpenFileDialog();
                form.InitialDirectory = subPath + "\\Release\\ui\\image\\";
                if (form.ShowDialog() == DialogResult.OK)
                {
                    var file = form.FileName.Substring(form.FileName.IndexOf("Release") + 8);
                    file = file.Replace('\\', '/');
                    return file;
                }
            }

            return value;
        }
    }

    public class FileStringEditor : System.Drawing.Design.UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (null != edSvc)
            {
                OpenFileDialog form = new OpenFileDialog();
                form.InitialDirectory = System.IO.Directory.GetCurrentDirectory();
                if (form.ShowDialog() == DialogResult.OK)
                {
                    var file = form.FileName.Substring(form.FileName.IndexOf("Release") + 8);
                    file = file.Replace('\\', '/');
                    return file;
                }
            }

            return value;
        }
    }

    /// <summary>
    /// 枚举转换器
    /// 用此类之前，必须保证在枚举项中定义了Description
    /// </summary>
    public class EnumConverter : ExpandableObjectConverter
    {
        /// <summary>
        /// 枚举项集合
        /// </summary>
        Dictionary<object, string> dic;

        /// <summary>
        /// 构造函数
        /// </summary>
        public EnumConverter()
        {
            dic = new Dictionary<object, string>();
        }
        /// <summary>
        /// 加载枚举项集合
        /// </summary>
        /// <param name="context"></param>
        private void LoadDic(ITypeDescriptorContext context)
        {
            dic = GetEnumValueDesDic(context.PropertyDescriptor.PropertyType);
        }

        /// <summary>
        /// 是否可从来源转换
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }
        /// <summary>
        /// 从来源转换
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
                //如果是枚举
                if (context.PropertyDescriptor.PropertyType.IsEnum)
                {
                    if (dic.Count <= 0)
                        LoadDic(context);
                    if (dic.Values.Contains(value.ToString()))
                    {
                        foreach (object obj in dic.Keys)
                        {
                            if (dic[obj] == value.ToString())
                            {
                                return obj;
                            }
                        }
                    }
                }
            }

            return base.ConvertFrom(context, culture, value);
        }
        /// <summary>
        /// 是否可转换
        /// </summary>
        /// <param name="context"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            //ListAttribute listAttribute = (ListAttribute)context.PropertyDescriptor.Attributes[typeof(ListAttribute)];
            //StandardValuesCollection vals = new TypeConverter.StandardValuesCollection(listAttribute._lst);

            //Dictionary<object, string> dic = GetEnumValueDesDic(typeof(PKGenerator));

            //StandardValuesCollection vals = new TypeConverter.StandardValuesCollection(dic.Keys);

            if (dic == null || dic.Count <= 0)
                LoadDic(context);

            StandardValuesCollection vals = new TypeConverter.StandardValuesCollection(dic.Keys);

            return vals;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            //DescriptionAttribute.GetCustomAttribute(
            //EnumDescription
            //List<KeyValuePair<Enum, string>> mList = UserCombox.ToListForBind(value.GetType());
            //foreach (KeyValuePair<Enum, string> mItem in mList)
            //{
            //    if (mItem.Key.Equals(value))
            //    {
            //        return mItem.Value;
            //    }
            //}
            //return "Error!";

            //绑定控件
            //            FieldInfo fieldinfo = value.GetType().GetField(value.ToString());
            //Object[] objs = fieldinfo.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
            //if (objs == null || objs.Length == 0)
            //{
            //    return value.ToString();
            //}
            //else
            //{
            //    System.ComponentModel.DescriptionAttribute da = (System.ComponentModel.DescriptionAttribute)objs[0];
            //    return da.Description;
            //}

            if (dic.Count <= 0)
                LoadDic(context);

            foreach (object key in dic.Keys)
            {
                if (key.ToString() == value.ToString() || dic[key] == value.ToString())
                {
                    return dic[key].ToString();
                }
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        /// <summary>
        /// 记载枚举的值+描述
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public Dictionary<object, string> GetEnumValueDesDic(Type enumType)
        {
            Dictionary<object, string> dic = new Dictionary<object, string>();
            FieldInfo[] fieldinfos = enumType.GetFields();
            foreach (FieldInfo field in fieldinfos)
            {
                if (field.FieldType.IsEnum)
                {
                    Object[] objs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
                    if (objs.Length > 0)
                    {
                        dic.Add(Enum.Parse(enumType, field.Name), ((DescriptionAttribute)objs[0]).Description);
                    }
                }

            }

            return dic;
        }

    }

    public class PointFConverter : TypeConverter
    {
        // Methods
        public PointFConverter()
        {
        }
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return ((sourceType == typeof(string)) || base.CanConvertFrom(context, sourceType));
        }
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return ((destinationType == typeof(System.ComponentModel.Design.Serialization.InstanceDescriptor)) || base.CanConvertTo(context, destinationType));
        }
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (!(value is string))
            {
                return base.ConvertFrom(context, culture, value);
            }
            string text = ((string)value).Trim();
            if (text.Length == 0)
            {
                return null;
            }
            if (culture == null)
            {
                culture = System.Globalization.CultureInfo.CurrentCulture;
            }
            char ch = culture.TextInfo.ListSeparator[0];
            string[] textArray = text.Split(new char[] { ch });
            float[] numArray = new float[textArray.Length];
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(float));
            for (int i = 0; i < numArray.Length; i++)
            {
                numArray[i] = (float)converter.ConvertFromString(context, culture, textArray[i]);
            }
            if (numArray.Length != 2)
            {
                throw new ArgumentException("格式不正确！");
            }
            return new System.Drawing.PointF(numArray[0], numArray[1]);

        }
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == null)
            {
                throw new ArgumentNullException("destinationType");
            }
            if ((destinationType == typeof(string)) && (value is System.Drawing.PointF))
            {
                System.Drawing.PointF pointf = (System.Drawing.PointF)value;
                if (culture == null)
                {
                    culture = System.Globalization.CultureInfo.CurrentCulture;
                }
                string separator = culture.TextInfo.ListSeparator + " ";
                TypeConverter converter = TypeDescriptor.GetConverter(typeof(float));
                string[] textArray = new string[2];
                int num = 0;
                textArray[num++] = converter.ConvertToString(context, culture, pointf.X);
                textArray[num++] = converter.ConvertToString(context, culture, pointf.Y);
                return string.Join(separator, textArray);
            }
            if ((destinationType == typeof(System.ComponentModel.Design.Serialization.InstanceDescriptor)) && (value is System.Drawing.SizeF))
            {
                System.Drawing.PointF pointf2 = (System.Drawing.PointF)value;
                ConstructorInfo member = typeof(System.Drawing.PointF).GetConstructor(new Type[] { typeof(float), typeof(float) });
                if (member != null)
                {
                    return new System.ComponentModel.Design.Serialization.InstanceDescriptor(member, new object[] { pointf2.X, pointf2.Y });
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);

        }
        public override object CreateInstance(ITypeDescriptorContext context, System.Collections.IDictionary propertyValues)
        {
            return new System.Drawing.PointF((float)propertyValues["X"], (float)propertyValues["Y"]);
        }
        public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            return TypeDescriptor.GetProperties(typeof(System.Drawing.PointF), attributes).Sort(new string[] { "X", "Y" });
        }
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
    }


}
