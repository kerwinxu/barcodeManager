﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Design;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms.Design;

namespace VestShapes
{
    //这个文件时跟属性中的弹出菜单相关的。

    /// <summary>
    /// BaseObject类是一个用来继承的抽象类。 
    /// 每一个由此类继承而来的类将自动支持克隆方法。
    /// 该类实现了Icloneable接口，并且每个从该对象继承而来的对象都将同样地
    /// 支持Icloneable接口。 
    /// </summary> 
    public abstract class BaseObject : ICloneable
    {
        /// <summary>    
        /// 克隆对象，并返回一个已克隆对象的引用    
        /// </summary>    
        /// <returns>引用新的克隆对象</returns>     
        public object Clone()
        {
            //首先我们建立指定类型的一个实例         
            object newObject = Activator.CreateInstance(this.GetType());
            //我们取得新的类型实例的字段数组。         
            FieldInfo[] fields = newObject.GetType().GetFields();
            int i = 0;
            foreach (FieldInfo fi in this.GetType().GetFields())
            {
                //我们判断字段是否支持ICloneable接口。             
                Type ICloneType = fi.FieldType.GetInterface("ICloneable", true);
                if (ICloneType != null)
                {
                    //取得对象的Icloneable接口。                 
                    ICloneable IClone = (ICloneable)fi.GetValue(this);
                    //我们使用克隆方法给字段设定新值。                
                    fields[i].SetValue(newObject, IClone.Clone());
                }
                else
                {
                    // 如果该字段部支持Icloneable接口，直接设置即可。                 
                    fields[i].SetValue(newObject, fi.GetValue(this));
                }
                //现在我们检查该对象是否支持IEnumerable接口，如果支持，             
                //我们还需要枚举其所有项并检查他们是否支持IList 或 IDictionary 接口。            
                Type IEnumerableType = fi.FieldType.GetInterface("IEnumerable", true);
                if (IEnumerableType != null)
                {
                    //取得该字段的IEnumerable接口                
                    IEnumerable IEnum = (IEnumerable)fi.GetValue(this);
                    Type IListType = fields[i].FieldType.GetInterface("IList", true);
                    Type IDicType = fields[i].FieldType.GetInterface("IDictionary", true);
                    int j = 0;
                    if (IListType != null)
                    {
                        //取得IList接口。                     
                        IList list = (IList)fields[i].GetValue(newObject);
                        foreach (object obj in IEnum)
                        {
                            //查看当前项是否支持支持ICloneable 接口。                         
                            ICloneType = obj.GetType().GetInterface("ICloneable", true);
                            if (ICloneType != null)
                            {
                                //如果支持ICloneable 接口，			 
                                //我们用它李设置列表中的对象的克隆			 
                                ICloneable clone = (ICloneable)obj;
                                list[j] = clone.Clone();
                            }
                            //注意：如果列表中的项不支持ICloneable接口，那么                      
                            //在克隆列表的项将与原列表对应项相同                      
                            //（只要该类型是引用类型）                        
                            j++;
                        }
                    }
                    else if (IDicType != null)
                    {
                        //取得IDictionary 接口                    
                        IDictionary dic = (IDictionary)fields[i].GetValue(newObject);
                        j = 0;
                        foreach (DictionaryEntry de in IEnum)
                        {
                            //查看当前项是否支持支持ICloneable 接口。                         
                            ICloneType = de.Value.GetType().
                                GetInterface("ICloneable", true);
                            if (ICloneType != null)
                            {
                                ICloneable clone = (ICloneable)de.Value;
                                dic[de.Key] = clone.Clone();
                            }
                            j++;
                        }
                    }
                }
                i++;
            }
            return newObject;
        }
    }

    /// <summary>
    /// 这个是弹出转换的窗体来设置转换的
    /// </summary>
    internal class ClsPropertyGridTransfroms : UITypeEditor
    {
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
        /// <summary>
        /// 编辑属性
        /// </summary>
        /// <param name="context">可用于获取附加上下文信息的 ITypeDescriptorContext。 </param>
        /// <param name="provider">IServiceProvider ，此编辑器可用其来获取服务。</param>
        /// <param name="value">要编辑的对象。 </param>
        /// <returns>新的对象值。 如果对象的值尚未更改，则它返回的对象应与传递给它的对象相同。 </returns>
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                FrmTransforms f = new FrmTransforms();
                // your setting here
                edSvc.ShowDialog(f);
            }
            return value;//返回原先的值
        }
        public override bool GetPaintValueSupported(System.ComponentModel.ITypeDescriptorContext context)
        {
            return false;
        }
    }
}
