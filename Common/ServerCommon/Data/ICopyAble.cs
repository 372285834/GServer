using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCommon.Data
{
    public class DoNotCopyAttribute : Attribute { }

    public class ICopyable
    {//慎用，这个速度快不了
        public virtual void CopyFrom(ICopyable src)
        {
            Type type = GetType();
            if (src.GetType() != type)
                return;

            System.Reflection.PropertyInfo[] props = type.GetProperties();
            foreach (System.Reflection.PropertyInfo i in props)
            {
                var attrs = i.GetCustomAttributes(typeof(DoNotCopyAttribute), true);
                if (attrs != null && attrs.Length > 0)
                    continue;

                attrs = i.GetCustomAttributes(typeof(System.ComponentModel.ReadOnlyAttribute), true);
                if (attrs == null || attrs.Length != 0)
                    continue;

                if (!i.CanWrite)
                    continue;

                if (i.PropertyType.IsSubclassOf(typeof(ICopyable)))
                {
                    ICopyable value = System.Activator.CreateInstance(i.PropertyType) as ICopyable;
                    ICopyable subObject = i.GetValue(src, null) as ICopyable;
                    value.CopyFrom(subObject);
                    i.SetValue(this, value, null);
                }
                else if (i.PropertyType.IsGenericType)
                {
                    var lst = System.Activator.CreateInstance(i.PropertyType);
                    var srclst = i.GetValue(src, null);
                    System.Reflection.MethodInfo mi = i.PropertyType.GetMethod("Add");
                    if (mi == null)
                        continue;
                    System.Reflection.PropertyInfo pi = i.PropertyType.GetProperty("Count");
                    if (pi == null)
                        continue;
                    System.Reflection.PropertyInfo tisPI2 = i.PropertyType.GetProperty("Item");
                    if (tisPI2 == null)
                        continue;

                    Type gnType = i.PropertyType.GetGenericArguments()[0];
                    object[] args = new object[1];
                    int Count = (int)pi.GetValue(srclst, null);
                    for (int j = 0; j < Count; j++)
                    {
                        if (gnType.IsValueType || gnType.IsEnum)
                        {
                            object elem = System.Activator.CreateInstance(gnType);
                            object elemSrc = tisPI2.GetValue(srclst, new object[] { j });
                            elem = elemSrc;
                            args[0] = elem;
                            mi.Invoke(lst, args);
                        }
                        else if (gnType.IsSubclassOf(typeof(ICopyable)))
                        {
                            ICopyable elem = System.Activator.CreateInstance(gnType) as ICopyable;
                            ICopyable elemSrc = tisPI2.GetValue(srclst, new object[] { j }) as ICopyable;
                            elem.CopyFrom(elemSrc);
                            args[0] = elem;
                            mi.Invoke(lst, args);
                        }
                    }
                    i.SetValue(this, lst, null);
                }
                else
                {

                    i.SetValue(this, i.GetValue(src, null), null);
                }
            }
        }
    }
}
