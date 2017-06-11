using System;
using System.Collections;
using System.Reflection;
using System.Xml;
namespace ServerFrame.Config
{
    public class DataValueAttribute : Attribute
    {
        public System.String Name;

        public DataValueAttribute(System.String name)
        {
            Name = name;
        }

    }

    public class IConfigurator
    {

        public static bool FillProperty(object obj, string file)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(file);

            XmlElement root = xml.DocumentElement;
            return FillProperty(obj, root);
        }

        public static bool FillProperty(object obj, XmlElement node)
        {
            if (node == null)
                return false;

            Type type = obj.GetType();
            System.Reflection.PropertyInfo[] properties = type.GetProperties();

            foreach (System.Reflection.PropertyInfo prop in properties)
            {
                if (prop.CanWrite == false)
                    continue;

                System.Object[] attrs = prop.GetCustomAttributes(typeof(ServerFrame.Config.DataValueAttribute), true);
                if (attrs == null || attrs.Length <= 0)
                    continue;

                ServerFrame.Config.DataValueAttribute dv = (ServerFrame.Config.DataValueAttribute)attrs[0];
                if (dv == null)
                    continue;

                XmlElement cNode = (XmlElement)node.GetElementsByTagName(dv.Name)[0];
                if (cNode == null)
                    continue;

                XmlAttribute att = cNode.GetAttributeNode("Type");
                XmlAttribute attV = cNode.GetAttributeNode("Value");

                if (att == null)
                    continue;

                Type ValueType = Type.GetType(att.Value);

                if (ValueType == typeof(Guid))
                {
                    prop.SetValue(obj, new Guid(attV.Value),null);
                }
                //else if (ValueType == typeof(Vector3))
                //{
                //    string[] strs = attV.Value.Split(',');
                //    if (strs.Length != 3)
                //        continue;

                //    Vector3 vec3 = new Vector3();
                //    vec3.X = Convert.ToSingle(strs[0]);
                //    vec3.Y = Convert.ToSingle(strs[1]);
                //    vec3.Z = Convert.ToSingle(strs[2]);
                //}
                //else if (ValueType == typeof(Vector2))
                //{
                //    System.String clrStr = attV.Value;
                //    System.String[] strs = clrStr.Split(',');
                //    if (strs.Length != 2)
                //        continue;
                //    Vector2 point;
                //    point.X = System.Convert.ToSingle(strs[0]);
                //    point.Y = System.Convert.ToSingle(strs[1]);
                //    prop.SetValue(obj, point, null);
                //}
                else if (ValueType == typeof(Enum))
                {
                    prop.SetValue(obj, Convert.ChangeType(attV.Value, typeof(System.Int32)), null);
                }
                else if (att.Value.Contains("@"))
                {
                    Assembly assembly = null;
                    string strClassName = "";
                    System.String[] substrs = att.Value.Split('@');
                    if (substrs.Length < 2)
                        continue;
                    string dll = substrs[0];
                    strClassName = substrs[1];

                    System.String fullDllName = System.AppDomain.CurrentDomain.BaseDirectory + dll;
                    fullDllName = fullDllName.Replace('/', '\\');
                    assembly = System.Reflection.Assembly.LoadFile(fullDllName);
                    if (assembly == null)
                        continue;

                    System.Object subObj = assembly.CreateInstance(strClassName);

                    if (subObj == null)
                        continue;

                    if (subObj.GetType() != prop.PropertyType)
                        continue;
                    if (false == FillProperty(subObj, cNode))
                        continue;
                    prop.SetValue(obj, subObj, null);
                }
                else if (att.Value == "List")
                {
                    var dType = cNode.GetAttributeNode("DataType");
                    if (dType == null)
                        continue;

                    Assembly assembly = null;
                    string strClassName = "";
                    if (dType.Value.Contains("@"))
                    {
                        System.String[] substrs = dType.Value.Split('@');
                        if (substrs.Length < 2)
                            continue;
                        string dll = substrs[0];
                        strClassName = substrs[1];

                        System.String fullDllName = System.AppDomain.CurrentDomain.BaseDirectory + dll;
                        fullDllName = fullDllName.Replace('/', '\\');
                        assembly = System.Reflection.Assembly.LoadFile(fullDllName);
                        if (assembly == null)
                            continue;

                    }
                    else
                    {
                        strClassName = dType.Value;
                    }

                    System.Reflection.MethodInfo mi = prop.PropertyType.GetMethod("Add");
                    var p = mi.GetParameters();

                    System.Object list = System.Activator.CreateInstance(prop.PropertyType);

                    object[] args = new object[1];

                    XmlElement nodes = (XmlElement)cNode.FirstChild;
                    for (; nodes != null; nodes = (XmlElement)nodes.NextSibling)
                    {
                        System.Object subObj = assembly != null ? assembly.CreateInstance(strClassName) : Activator.CreateInstance(Type.GetType(strClassName));
                        if (subObj == null)
                            continue;
                        var vType = subObj.GetType();

                        var nodeValue = nodes.GetAttribute("Value");

                        if (vType == typeof(Guid))
                        {
                            subObj = new Guid(nodeValue);
                        }
                        else if (vType.IsEnum)
                        {
                            subObj = Convert.ToInt32(nodeValue);
                        }
                        else if (vType.IsValueType)
                        {
                            if (nodeValue != null)
                                subObj = Convert.ChangeType(nodeValue, vType);
                        }
                        else if (false == FillProperty(subObj, nodes))
                        {
                            continue;
                        }
                        args[0] = subObj;
                        mi.Invoke(list, args);
                    }
                    prop.SetValue(obj, list, null);
                }
                else
                {
                    prop.SetValue(obj, Convert.ChangeType(attV.Value, ValueType), null);
                }
            }

            return true;
        }

        public static bool FillProperty(object obj, string path, Guid guid, string postFix)
        {
            System.String file = path + guid.ToString() + "." + postFix;
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(file);

            return FillProperty(obj, xml.DocumentElement);
        }

        public static bool SaveProperty(object obj, XmlElement node)
        {
            System.Type type = obj.GetType();
            System.Reflection.PropertyInfo[] properties = type.GetProperties();
            foreach (System.Reflection.PropertyInfo prop in properties)
            {
                if (prop.CanRead == false)
                    continue;
                System.Object[] attrs = prop.GetCustomAttributes(typeof(ServerFrame.Config.DataValueAttribute), true);
                if (attrs == null || attrs.Length == 0)
                    continue;
                ServerFrame.Config.DataValueAttribute dv = (ServerFrame.Config.DataValueAttribute)attrs[0];
                if (dv == null)
                    continue;

                XmlElement cNode = null;
                if (prop.PropertyType.IsGenericType)
                {
                    int nGenericArgCount = prop.PropertyType.GetGenericArguments().Length;
                    Type gnType = prop.PropertyType.GetGenericArguments()[0];
                    object objValue = prop.GetValue(obj, null);

                    cNode = node.OwnerDocument.CreateElement(dv.Name);
                    cNode.SetAttribute("Type", "List");

                    cNode.SetAttribute("DataType", gnType.FullName.Contains("System.") ? gnType.FullName : gnType.Module.Name + "@" + gnType.FullName);
                    node.AppendChild(cNode);

                    if (objValue.GetType().FullName.Contains("System.Collections.Generic.List"))
                    {
                        IEnumerable list = objValue as IEnumerable;
                        foreach (var item in list)
                        {
                            XmlElement cData = node.OwnerDocument.CreateElement("Data");
                            cNode.AppendChild(cData);
                            if (item.GetType().IsEnum)
                            {
                                cData.SetAttribute("Type", "System.Enum" );
                                SetAttr(cData, item);
                            }
                            else if (item.GetType().IsGenericType || !item.GetType().IsPrimitive)
                            {
                                SaveProperty(item, cData);
                            }
                            else
                            {
                                SetAttr(cData, item);
                            }
                        }

                        continue;
                    }
                }
                else
                {
                    cNode = node.OwnerDocument.CreateElement(dv.Name);

                }

                if (prop.PropertyType.IsEnum)
                {
                    cNode.SetAttribute("Type", "System.Enum");
                    object objGet = prop.GetValue(obj, null);
                    SetAttr(cNode, objGet);

                }
                else if (!prop.PropertyType.FullName.Contains("System."))
                {
                    cNode.SetAttribute("Type", prop.PropertyType.Module.Name + "@" + prop.PropertyType.FullName);
                    var objGet = prop.GetValue(obj, null);
                    if (objGet == null)
					{
                        objGet = System.Activator.CreateInstance(prop.PropertyType);
					}
                    SaveProperty(objGet, cNode);
                }
                else
                {
                    cNode.SetAttribute("Type", prop.PropertyType.FullName);
                    object objGet = prop.GetValue(obj, null);
                    SetAttr(cNode, objGet);
                }

                node.AppendChild(cNode);

            }

            return true;
        }

        static void SetAttr(XmlElement owner, object value)
        {
            if (value == null)
                owner.SetAttribute("Value", "");

            string strValue = "";
            //if (value.GetType() == typeof(Vector3))
            //{
            //    Vector3 vec3 = (Vector3)value;
            //    strValue = string.Format("{0},{1},{2}", vec3.X, vec3.Y, vec3.Z);
            //}
            //else if (value.GetType() == typeof(Vector2))
            //{
            //    Vector2 vec2 = (Vector2)value;
            //    strValue = string.Format("{0},{1}", vec2.X, vec2.Y);
            //}
            //else 
            if (value != null)
            {
                if (value.GetType().IsEnum)
                {
                    strValue = ((int)value).ToString();
                }
                else
                {
                    strValue = value.ToString();
                }
            }

            owner.SetAttribute("Value", strValue);
        }

        public static bool SaveProperty(object obj, string name, string file)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement xmlElement = xmlDoc.CreateElement(obj.GetType().FullName);
            xmlDoc.AppendChild(xmlElement);

            if (!SaveProperty(obj, xmlElement))
                return false;

            xmlDoc.Save(file);
            return true;
        }
    }
}
