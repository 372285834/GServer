using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RPCCodeBuilder
{
    class MethodDesc
    {
        public System.Reflection.MethodInfo mi = null;
        public RPC.RPCMethodAttribute rpcAttr = null;
        public System.Type hostType = null;
        public Byte MethodIndex = 0;
        public UInt32 HashcodeOfMethod = 0;
    }
    class ChildObjectDesc
    {
        public System.Reflection.PropertyInfo pi = null;
        public RPC.RPCChildObjectAttribute rpcAttr = null;
        public System.Type hostType = null;
    }
    class IndexerDesc
    {
        public System.Reflection.PropertyInfo pi = null;
        public RPC.RPCIndexObjectAttribute rpcAttr = null;
        public System.Type hostType = null;
    }
    class RPCClassBuilder
    {
        const string EndLine = "\r\n";
        const string TabStr = "    ";

        public static string MakeClientClassCode(TreeNode node, bool bIsClient)
        {
            string temp = "";
            return MakeClientClassCode(node, bIsClient, out temp);
        }

        public static string MakeClientClassCode(TreeNode node, bool bIsClient, out string strClientOut)
        {
            object obj = node.Tag;
            strClientOut = "";

            if (obj == null)
                return "";

            string[] full = obj.ToString().Split('.');
            string strOut = "";

            if (full.Length <= 0)
                return "";

            do
            {
                if (!bIsClient)
                {
                    RPC.RPCClassAttribute attr = obj as RPC.RPCClassAttribute;
                    full = attr.RPCType.ToString().Split('.');
                }

                if (full.Length == 1)
                {
                    strOut += "public class H_" + full[0] + EndLine;
                    strOut += "{" + EndLine;
                    strOut += TabStr + "public static " + "H_" + full[0] + " smInstance = new H_" + full[0] + "();" + EndLine;
                    break;
                }
                else if (full.Length > 2)
                {
                    string[] objNew = new string[2];
                    string strNameSpace = "";
                    for (int i = 0; i < full.Length - 1; i++)
                    {
                        strNameSpace += full[i];

                        if (i < full.Length - 2)
                            strNameSpace += '.';
                    }

                    objNew[0] = strNameSpace;
                    objNew[1] = full[full.Length - 1];
                    full = objNew;
                }

                strOut += "namespace " + full[0] + "{" + EndLine;
                strOut += "public class H_" + full[1] + EndLine;
                strOut += "{" + EndLine;
                strOut += TabStr + "public static " + full[0] + ".H_" + full[1] + " smInstance = new " + full[0] + ".H_" + full[1] + "();" + EndLine;

            } while (false);

            strClientOut += strOut;

            foreach( TreeNode childNode in node.Nodes )
            {
                MethodDesc ma = childNode.Tag as MethodDesc;
                if (ma!=null)
                {
                    string code = MakeCallCode(ma);
                    strOut += code;

                    if (!bIsClient)
                        if (ma.rpcAttr.LimitLevel < 400)
                            strClientOut += code;
                }

                System.Reflection.PropertyInfo pi = childNode.Tag as System.Reflection.PropertyInfo;
                if (pi != null)
                {
                    if (pi.Name == "Item")
                    {
                        string strIndex = MakeIndexObjectCode(pi, System.Convert.ToInt32(childNode.Name));
                        strOut += strIndex;
                        strClientOut += strIndex;
                    }
                    else
                    {
                        string strChild = MakeChildObjectCode(pi, System.Convert.ToInt32(childNode.Name));
                        strOut += strChild;
                        strClientOut += strChild;
                    }
                }
            }
            strOut += "}" + EndLine;
            strOut += "}" + EndLine;
            strClientOut += "}" + EndLine;
            strClientOut += "}" + EndLine;

            return strOut;
        }

        public static string MakeCallCode(MethodDesc desc)
        {
            System.Reflection.MethodInfo ma = desc.mi;
            int Index = desc.MethodIndex;
            string strOut="";
            strOut += TabStr + "public void " + ma.Name + "(" + "RPC.PackageWriter pkg" ;
            System.Reflection.ParameterInfo[] parameters = ma.GetParameters();
            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].ParameterType.FullName == "Iocp.NetConnection")
                    continue;
                if (parameters[i].ParameterType.FullName == "RPC.RPCForwardInfo")
                    continue;
                if (parameters[i].ParameterType.FullName == "RPC.DataReader")
                    strOut += "," + "RPC.DataWriter " + parameters[i].Name;
                else
                {
                    strOut += "," + parameters[i].ParameterType.FullName + " " + parameters[i].Name;

                    //if (isClient&&parameters[i].ParameterType.FullName.Contains("SlimDX"))
                    //    strOut += "," + parameters[i].ParameterType.FullName.Replace("SlimDX", "UnityEngine") + " " + parameters[i].Name;
                    //else
                    //    strOut += "," + parameters[i].ParameterType.FullName + " " + parameters[i].Name;
                }
            }
            strOut += ")" + EndLine;
            strOut += TabStr + "{" + EndLine;
            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].ParameterType.FullName == "Iocp.NetConnection")
                    continue;
                if (parameters[i].ParameterType.FullName == "RPC.RPCForwardInfo")
                    continue;
                strOut += TabStr + TabStr + "pkg.Write(" + parameters[i].Name + ");" + EndLine;
            }
            strOut += TabStr + TabStr + "pkg.SetMethod(" + Index + ");" + EndLine;
            strOut += TabStr + "}" + EndLine;
            return strOut;
        }

        public static string MakeChildObjectCode(System.Reflection.PropertyInfo pi,int index)
        {
            //FRPC_TestRPC ^ RPCGet_Child0(PackageWriter ^ pkg);
            //{
            //    pkg->PushStack(0);
            //    return mChild0;
            //}
            string strOut = "";
            string strTypeFullName = pi.PropertyType.Namespace + ".H_" + pi.PropertyType.Name;
            strOut += TabStr + "public " + strTypeFullName + " HGet_" + pi.Name + "(RPC.PackageWriter pkg)" + EndLine;
            strOut += TabStr + "{" + EndLine;
            strOut += TabStr + TabStr + "pkg.PushStack(" + index + ");" + EndLine;
            strOut += TabStr + TabStr + "return " + strTypeFullName + ".smInstance;" + EndLine;
            strOut += TabStr + "}" + EndLine;
            return strOut;
        }

        public static string MakeIndexObjectCode(System.Reflection.PropertyInfo pi, int index)
        {
            //FRPC_TestRPC ^ RPCGet_Child0(PackageWriter ^ pkg);
            //{
            //    pkg->PushStack(0);
            //    return mChild0;
            //}
            string strCall = "";
            string strArgSerial = "";
            System.Reflection.ParameterInfo[] parameters = pi.GetIndexParameters();
            for (int i = 0; i < parameters.Length; i++)
            {
                strCall += "," + parameters[i].ParameterType.FullName + " " + parameters[i].Name;
                strArgSerial += TabStr + TabStr + "pkg.Write(" + parameters[i].Name + ");" + EndLine;
            }
            string strOut = "";
            string strTypeFullName = pi.PropertyType.Namespace + ".H_" + pi.PropertyType.Name;
            strOut += TabStr + "public " + strTypeFullName + " HIndex(RPC.PackageWriter pkg" + strCall + ")" + EndLine;
            strOut += TabStr + "{" + EndLine;
            strOut += TabStr + TabStr + "pkg.PushStack(11+" + index + ");" + EndLine;
            strOut += strArgSerial;
            strOut += TabStr + TabStr + "return " + strTypeFullName + ".smInstance;" + EndLine;
            strOut += TabStr + "}" + EndLine;
            return strOut;
        }

        public static string MakeIndexerExecuterCode(System.Reflection.PropertyInfo pi, TreeNode node, bool bIsClient)
        {
            string fullname = "";
            int LimitLevel = 0;

            if (bIsClient)
            {
                fullname = node.Name;
            }
            else
            {
                RPC.RPCClassAttribute ClassAttr = node.Tag as RPC.RPCClassAttribute;
                fullname = ClassAttr.RPCType.FullName;
                object[] propAttrs = pi.GetCustomAttributes(typeof(RPC.RPCIndexObjectAttribute), false);
                if (propAttrs != null && propAttrs.Length == 1)
                {
                    RPC.RPCIndexObjectAttribute propAtt = propAttrs[0] as RPC.RPCIndexObjectAttribute;
                    if (propAtt != null)
                        LimitLevel = propAtt.LimitLevel;
                }
            }

            string strOut = "";
            strOut += "[RPC.RPCIndexExecuterTypeAttribute(" + RPC.RPCEntrance.GetIndexerHashCode(pi, fullname) + ",\"" + pi.Name + "\",typeof(HIndex_" + RPC.RPCEntrance.GetIndexerHashCode(pi, fullname) + "))]" + EndLine;
            strOut += "public class HIndex_" + RPC.RPCEntrance.GetIndexerHashCode(pi, fullname) + ": RPC.RPCIndexerExecuter" + EndLine;
            strOut += "{" + EndLine;
            strOut += TabStr + "public override int LimitLevel" + EndLine;
            strOut += TabStr + "{" + EndLine;
            strOut += TabStr + TabStr + "get { return " + LimitLevel + "; }" + EndLine;
            strOut += TabStr + "}" + EndLine;
            strOut += TabStr + "public override RPC.RPCObject Execute(RPC.RPCObject obj,RPC.PackageProxy pkg)" + EndLine;
            strOut += TabStr + "{" + EndLine;
            strOut += TabStr + TabStr + fullname + " host = obj as " + fullname + ";" + EndLine;
            strOut += TabStr + TabStr + "if (host == null) return null;" + EndLine;

            string strCall = "";
            System.Reflection.ParameterInfo[] parameters = pi.GetIndexParameters();
            for (int i = 0; i < parameters.Length; i++)
            {
                bool isNew = false;
                Type baseType = parameters[i].ParameterType;
                while (!isNew)
                {
                    baseType = baseType.BaseType;
                    if (baseType == null)
                        break;
                    if (baseType.Name == "IAutoSaveAndLoad")
                    {
                        isNew = true;
                        break;
                    }
                }
                if (isNew)
                {
                    strOut += TabStr + TabStr + parameters[i].ParameterType.FullName + " " + parameters[i].Name + " = new " + parameters[i].ParameterType.FullName + "();" + EndLine;
                    strOut += TabStr + TabStr + "pkg.Read( " + parameters[i].Name + ");" + EndLine;
                }
                else
                {
                    strOut += TabStr + TabStr + parameters[i].ParameterType.FullName + " " + parameters[i].Name + ";" + EndLine;
                    strOut += TabStr + TabStr + "pkg.Read(out " + parameters[i].Name + ");" + EndLine;
                }

                if (i == parameters.Length - 1)
                    strCall += parameters[i].Name;
                else
                    strCall += parameters[i].Name + ",";
            }

            strOut += TabStr + TabStr + "return host[" + strCall + "];" + EndLine;

            strOut += TabStr + "}" + EndLine;
            strOut += "}" + EndLine;
            return strOut;
        }

        public static string MakeMethodExecuterCode(MethodDesc desc, bool bIsClient)
        {
            System.Reflection.MethodInfo mi = desc.mi;
            System.Type ctype = desc.hostType;

            int LimitLevel = 0;

            if (!bIsClient)
            {
                object[] propAttrs = mi.GetCustomAttributes(typeof(RPC.RPCMethodAttribute), false);
                if (propAttrs != null && propAttrs.Length == 1)
                {
                    RPC.RPCMethodAttribute propAtt = propAttrs[0] as RPC.RPCMethodAttribute;
                    if (propAtt != null)
                        LimitLevel = propAtt.LimitLevel;
                }
            }

            string strOut = "";
            strOut += "[RPC.RPCMethordExecuterTypeAttribute(" + RPC.RPCEntrance.GetMethodHashCode(mi, ctype.FullName) + ",\"" + mi.Name + "\",typeof(HExe_" + RPC.RPCEntrance.GetMethodHashCode(mi, ctype.FullName) + "))]" + EndLine;
            strOut += "public class HExe_" + RPC.RPCEntrance.GetMethodHashCode(mi, ctype.FullName) + ": RPC.RPCMethodExecuter" + EndLine;
            strOut += "{" + EndLine;
            strOut += TabStr + "public override int LimitLevel" + EndLine;
            strOut += TabStr + "{" + EndLine;
            strOut += TabStr + TabStr + "get { return " + LimitLevel + "; }" + EndLine;
            strOut += TabStr + "}" + EndLine;

            if (bIsClient)
                strOut += TabStr + "public override object Execute(RPC.RPCObject obj,RPC.PackageProxy pkg)" + EndLine;
            else
                strOut += TabStr + "public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)" + EndLine;
            strOut += TabStr + "{" + EndLine;
            strOut += TabStr + TabStr + ctype.FullName + " host = obj as " + ctype.FullName + ";" + EndLine;
            strOut += TabStr + TabStr + "if (host == null) return null;" + EndLine;
            
            string strCall = "";
            System.Reflection.ParameterInfo[] parameters = mi.GetParameters();
            for (int i = 0; i < parameters.Length; i++)
            {
                string finalName = parameters[i].Name;

                bool isNew = false;
                Type baseType = parameters[i].ParameterType;
                while (!isNew)
                {
                    baseType = baseType.BaseType;
                    if (baseType == null)
                        break;
                    if (baseType.Name == "IAutoSaveAndLoad")
                    {
                        isNew = true;
                        break;
                    }
                }

                if (parameters[i].ParameterType.FullName == "Iocp.NetConnection")
                {
                    finalName = "connect";
                }
                else if (parameters[i].ParameterType.FullName == "RPC.RPCForwardInfo")
                {
                    finalName = "fwd";
                }
                else if (isNew)
                {
                    strOut += TabStr + TabStr + parameters[i].ParameterType.FullName + " " + parameters[i].Name + " = new " + parameters[i].ParameterType.FullName + "();" + EndLine;
                    strOut += TabStr + TabStr + "pkg.Read( " + parameters[i].Name + ");" + EndLine;
                }
                else
                {
                    strOut += TabStr + TabStr + parameters[i].ParameterType.FullName + " " + parameters[i].Name + ";" + EndLine;
                    strOut += TabStr + TabStr + "pkg.Read(out " + parameters[i].Name + ");" + EndLine;
                }

                if (i == parameters.Length - 1)
                    strCall += finalName;
                else
                    strCall += finalName + ",";
            }

            if(mi.ReturnType==typeof(void))
                strOut += TabStr + TabStr + "host." + mi.Name + "(" + strCall + ");" + EndLine + "return null;" + EndLine;
            else
                strOut += TabStr + TabStr + "return host." + mi.Name + "(" + strCall + ");" + EndLine;

            strOut += TabStr + "}" + EndLine;
            strOut += "}" + EndLine;
            return strOut;
        }

#region CppCodeBuilder
        public static string CSharp2CppType(string strType)
        {
            return strType.Replace(".", "::");
        }

        public static string BeginCppNamespaceDeclare(string strNamespace)
        {
            if (strNamespace == "")
                return "";
            string strOut = "";
            int start = 0;
            int pos = strNamespace.IndexOf('.', start);
            while (pos >= 0)
            {
                int endPos = pos;
                strOut += "namespace " + strNamespace.Substring(start, endPos - start) + "{ ";
                start = pos + 1;
                pos = strNamespace.IndexOf('.', start);
            }
            strOut += "namespace " + strNamespace.Substring(start, strNamespace.Length - start) + "{";

            strOut += EndLine;
            return strOut;
        }

        public static string EndCppNamespaceDeclare(string strNamespace)
        {
            if (strNamespace == "")
                return "";
            string strOut = "";
            int start = 0;
            int pos = strNamespace.IndexOf('.', start);
            while (pos >= 0)
            {
                strOut += "} ";
                start = pos + 1;
                pos = strNamespace.IndexOf('.', start);
            }
            strOut += "} ";
            
            strOut += EndLine;
            return strOut;
        }

        public static string MakeClientCppClassDeclareCodePure(TreeNode node)
        {
            RPC.RPCClassAttribute ca = node.Tag as RPC.RPCClassAttribute;
            if (ca == null)
                return "";

            string strOut = BeginCppNamespaceDeclare(ca.RPCType.Namespace);
            strOut += "struct H_" + ca.RPCType.Name + ";" + EndLine;
            strOut += EndCppNamespaceDeclare(ca.RPCType.Namespace);
            return strOut;
        }

        public static string MakeClientCppClassDeclareCode(TreeNode node)
        {
            RPC.RPCClassAttribute ca = node.Tag as RPC.RPCClassAttribute;
            if (ca == null)
                return "";

            string strOut = BeginCppNamespaceDeclare(ca.RPCType.Namespace);
            strOut += "struct H_" + ca.RPCType.Name + EndLine;
            strOut += "{" + EndLine;
            strOut += TabStr + "static " + CSharp2CppType(ca.RPCType.Namespace) + "::H_" + ca.RPCType.Name + "* smInstance();" + EndLine;
            foreach (TreeNode childNode in node.Nodes)
            {
                MethodDesc ma = childNode.Tag as MethodDesc;
                if (ma != null)
                {
                    strOut += MakeCppCallCodeDeclare(ma);
                }
                System.Reflection.PropertyInfo pi = childNode.Tag as System.Reflection.PropertyInfo;
                if (pi != null)
                {
                    if (pi.Name == "Item")
                    {
                        strOut += MakeCppIndexObjectCodeDeclare(pi, System.Convert.ToInt32(childNode.Name));
                    }
                    else
                    {
                        strOut += MakeCppChildObjectCodeDeclare(pi, System.Convert.ToInt32(childNode.Name));
                    }
                }
            }
            strOut += "};" + EndLine;
            strOut += EndCppNamespaceDeclare(ca.RPCType.Namespace);
            return strOut;
        }

        public static string MakeClientCppClassCode(TreeNode node)
        {
            RPC.RPCClassAttribute ca = node.Tag as RPC.RPCClassAttribute;
            if (ca == null)
                return "";

            string strOut = "";//= BeginCppNamespaceDeclare(ca.RPCType.Namespace);
            string strClassName = CSharp2CppType(ca.RPCType.Namespace) + "::H_" + ca.RPCType.Name;
            strOut += TabStr + strClassName + "* " + strClassName + "::smInstance(){ static " +
                                            strClassName + " gInst; return &gInst;}" + EndLine;
            foreach (TreeNode childNode in node.Nodes)
            {
                MethodDesc ma = childNode.Tag as MethodDesc;
                if (ma != null)
                {
                    strOut += MakeCppCallCode(ma,strClassName);
                }
                System.Reflection.PropertyInfo pi = childNode.Tag as System.Reflection.PropertyInfo;
                if (pi != null)
                {
                    if (pi.Name == "Item")
                    {
                        strOut += MakeCppIndexObjectCode(pi, System.Convert.ToInt32(childNode.Name), strClassName);
                    }
                    else
                    {
                        strOut += MakeCppChildObjectCode(pi, System.Convert.ToInt32(childNode.Name), strClassName);
                    }
                }
            }
            strOut += "";//EndCppNamespaceDeclare(ca.RPCType.Namespace);

            return strOut;
        }

        public static string MakeCppCallCodeDeclare(MethodDesc desc)
        {
            if (desc.rpcAttr.NoClientCall)
                return "";
            System.Reflection.MethodInfo ma = desc.mi;
            int Index = desc.MethodIndex;
            string strOut = "";
            strOut += TabStr + "void " + ma.Name + "(" + "CppRPC::PackageWriter& pkg";
            System.Reflection.ParameterInfo[] parameters = ma.GetParameters();
            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].ParameterType.FullName == "RPC.DataReader")
                    strOut += "," + "CppRPC::DataWriter& " + parameters[i].Name;
                else if (parameters[i].ParameterType.FullName == "Iocp.NetConnection")
                    strOut += "";
                else if (parameters[i].ParameterType.FullName == "RPC.RPCForwardInfo")
                    strOut += "";
                else
                    strOut += "," + CSharp2CppType(parameters[i].ParameterType.FullName) + " " + parameters[i].Name;
            }
            strOut += ");" + EndLine;
            return strOut;
        }

        public static string MakeCppCallCode(MethodDesc desc, string strCName)
        {
            if (desc.rpcAttr.NoClientCall)
                return "";
            System.Reflection.MethodInfo ma = desc.mi;
            int Index = desc.MethodIndex;
            string strOut = "";
            strOut += TabStr + "void " + strCName + "::" + ma.Name + "(" + "CppRPC::PackageWriter& pkg";
            System.Reflection.ParameterInfo[] parameters = ma.GetParameters();
            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].ParameterType.FullName == "RPC.DataReader")
                    strOut += "," + "CppRPC::DataWriter& " + parameters[i].Name;
                else if (parameters[i].ParameterType.FullName == "Iocp.NetConnection")
                    strOut += "";
                else if (parameters[i].ParameterType.FullName == "RPC.RPCForwardInfo")
                    strOut += "";
                else
                    strOut += "," + CSharp2CppType(parameters[i].ParameterType.FullName) + " " + parameters[i].Name;
            }
            strOut += ")" + EndLine;
            strOut += TabStr + "{" + EndLine;
            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].ParameterType.FullName != "Iocp.NetConnection" && parameters[i].ParameterType.FullName != "RPC.RPCForwardInfo")
                    strOut += TabStr + TabStr + "pkg.Write(" + parameters[i].Name + ");" + EndLine;
            }
            strOut += TabStr + TabStr + "pkg.SetMethod(" + Index + ");" + EndLine;
            strOut += TabStr + "};" + EndLine;
            return strOut;
        }

        public static string MakeCppChildObjectCodeDeclare(System.Reflection.PropertyInfo pi, int index)
        {
            //FRPC_TestRPC ^ RPCGet_Child0(PackageWriter ^ pkg);
            //{
            //    pkg->PushStack(0);
            //    return mChild0;
            //}
            string strOut = "";
            string strFullTypeName = pi.PropertyType.Namespace + ".H_" + pi.PropertyType.Name;
            strOut += TabStr + CSharp2CppType(strFullTypeName) + "* HGet_" + pi.Name + "(CppRPC::PackageWriter& pkg);" + EndLine;
            return strOut;
        }

        public static string MakeCppChildObjectCode(System.Reflection.PropertyInfo pi, int index, string strCName)
        {
            //FRPC_TestRPC ^ RPCGet_Child0(PackageWriter ^ pkg);
            //{
            //    pkg->PushStack(0);
            //    return mChild0;
            //}
            string strOut = "";
            string strFullTypeName = pi.PropertyType.Namespace + ".H_" + pi.PropertyType.Name;
            strOut += TabStr + CSharp2CppType(strFullTypeName) + "* " + strCName + "::HGet_" + pi.Name + "(CppRPC::PackageWriter& pkg)" + EndLine;
            strOut += TabStr + "{" + EndLine;
            strOut += TabStr + TabStr + "pkg.PushStack(" + index + ");" + EndLine;
            strOut += TabStr + TabStr + "return " + CSharp2CppType(strFullTypeName) + "::smInstance();" + EndLine;
            strOut += TabStr + "}" + EndLine;
            return strOut;
        }

        public static string MakeCppIndexObjectCodeDeclare(System.Reflection.PropertyInfo pi, int index)
        {
            //FRPC_TestRPC ^ RPCGet_Child0(PackageWriter ^ pkg);
            //{
            //    pkg->PushStack(0);
            //    return mChild0;
            //}
            string strCall = "";
            System.Reflection.ParameterInfo[] parameters = pi.GetIndexParameters();
            for (int i = 0; i < parameters.Length; i++)
            {
                strCall += "," + CSharp2CppType(parameters[i].ParameterType.FullName) + " " + parameters[i].Name;
            }
            string strOut = "";
            string strFullTypeName = pi.PropertyType.Namespace + ".H_" + pi.PropertyType.Name;
            strOut += TabStr + CSharp2CppType(strFullTypeName) + "* HIndex_" + pi.Name + "(CppRPC::PackageWriter& pkg" + strCall + ");" + EndLine;
            return strOut;
        }

        public static string MakeCppIndexObjectCode(System.Reflection.PropertyInfo pi, int index, string strCName)
        {
            //FRPC_TestRPC ^ RPCGet_Child0(PackageWriter ^ pkg);
            //{
            //    pkg->PushStack(0);
            //    return mChild0;
            //}
            string strCall = "";
            string strArgSerial = "";
            string strFullTypeName = pi.PropertyType.Namespace + ".H_" + pi.PropertyType.Name;
            System.Reflection.ParameterInfo[] parameters = pi.GetIndexParameters();
            for (int i = 0; i < parameters.Length; i++)
            {
                strCall += "," + CSharp2CppType(parameters[i].ParameterType.FullName) + " " + parameters[i].Name;
                strArgSerial += TabStr + TabStr + "pkg.Write(" + parameters[i].Name + ");" + EndLine;
            }
            string strOut = "";
            strOut += TabStr + CSharp2CppType(strFullTypeName) + "* " + strCName + "::HIndex_" + pi.Name + "(CppRPC::PackageWriter& pkg" + strCall + ")" + EndLine;
            strOut += TabStr + "{" + EndLine;
            strOut += TabStr + TabStr + "pkg.PushStack(11+" + index + ");" + EndLine;
            strOut += strArgSerial;
            strOut += TabStr + TabStr + "return " + CSharp2CppType(strFullTypeName) + "::smInstance();" + EndLine;
            strOut += TabStr + "}" + EndLine;
            return strOut;
        }
#endregion
    }
}
