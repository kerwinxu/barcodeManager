using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Io.Github.Kerwinxu.LibShapes.Core.Shape
{
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
}
