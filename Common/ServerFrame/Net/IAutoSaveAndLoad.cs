using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPC
{
    public class IAutoDBField
    {
        public IAutoDBField()
        {
        }
        public System.Reflection.PropertyInfo Property;
        public string Field;
    }

    public class IAutoDBClassDesc
    {
        public List<IAutoDBField> Fields = new List<IAutoDBField>();
        public void CreateDesc(System.Type klassType)
        {
            System.Type objType = klassType;
            System.Reflection.PropertyInfo[] fields = objType.GetProperties();

            Fields.Clear();
            foreach (System.Reflection.PropertyInfo i in fields)
            {
                System.Object[] attrs = i.GetCustomAttributes(typeof(ServerFrame.DB.DBBindField), true); 
                if (attrs.Length == 0)
                {
                    continue;
                }

                IAutoDBField f = new IAutoDBField();
                f.Property = i;
                f.Field = (attrs[0] as ServerFrame.DB.DBBindField).Field;
                Fields.Add(f);
            }
        }
    }

    public class IAutoSLField
    {
        public IAutoSLField()
        {
            Level = 0;
            IgnoreWhenSingle = false;
        }
        public System.Reflection.PropertyInfo Property;
        public int Level;
        public bool IgnoreWhenSingle;
    }

	public class IAutoSLClassDesc
	{
        public System.Collections.Generic.List<IAutoSLField> Fields = new System.Collections.Generic.List<IAutoSLField>();
		
		public void CreateDesc(System.Type klassType)
	    {
		    System.Type objType = klassType;
		    System.Reflection.PropertyInfo[] fields = objType.GetProperties();

		    Fields.Clear();
		    foreach(System.Reflection.PropertyInfo i in fields)
		    {
			    System.Object[] attrs = i.GetCustomAttributes(typeof(AutoSaveLoadAttribute),true);
			    if(attrs.Length==0)
			    {
				    continue;
			    }

			    IAutoSLField f = new IAutoSLField();
			    f.Property = i;
                var attr = attrs[0] as AutoSaveLoadAttribute;
                f.IgnoreWhenSingle = !attr.IsSingle;


			    attrs = i.GetCustomAttributes(typeof(FieldAutoSaveLoadAttribute),true);
			    if(attrs.Length!=0)
			    {
				    FieldAutoSaveLoadAttribute atb = (FieldAutoSaveLoadAttribute)attrs[0];
				    if(atb!=null)
				    {
					    f.Level = atb.Level;
				    }
			    }

			    Fields.Add(f);
		    }
	    }
	}

	public class IAutoSLClassDescManager
	{
		System.Collections.Generic.Dictionary<System.Type,IAutoSLClassDesc> mKlasses= new System.Collections.Generic.Dictionary<System.Type,IAutoSLClassDesc>();
        Dictionary<System.Type, IAutoDBClassDesc> mDBKlasses = new Dictionary<System.Type, IAutoDBClassDesc>();
        static IAutoSLClassDescManager smInstance = new IAutoSLClassDescManager();
	
		public static IAutoSLClassDescManager Instance
        {
            get
            {
			    return smInstance;
            }
		}

        public IAutoDBClassDesc GetDBClassDesc(System.Type klassType)
        {
            lock (this)
            {
                IAutoDBClassDesc result;
                if (mDBKlasses.TryGetValue(klassType, out result))
                    return result;

                result = new IAutoDBClassDesc();
                result.CreateDesc(klassType);

                mDBKlasses.Add(klassType, result);
                return result;
            }
        }


		public IAutoSLClassDesc GetClassDesc(System.Type klassType)
	    {
            lock (this)
            {
                IAutoSLClassDesc result;
                if (mKlasses.TryGetValue(klassType, out result))
                    return result;

                result = new IAutoSLClassDesc();
                result.CreateDesc(klassType);

                mKlasses.Add(klassType, result);
                return result;
            }
	    }
	}

	public class IAutoSaveAndLoad
	{
        public virtual void PackageWrite(PackageWriter pkg)
	    {
		    if( pkg.IsSinglePkg )
		    {
			    PackageWriteSingle(pkg);
		    }
		    else
		    {
			    PackageWriteFull(pkg);
		    }
	    }
        public virtual void PackageRead(PackageProxy pkg)
	    {
		    if( pkg.IsSinglePkg )
		    {
			    PackageReadSingle(pkg);
		    }
		    else
		    {
			    PackageReadFull(pkg);
		    }
	    }

        #region Read&Write Utility
        protected bool WritePkg(PackageWriter pkg,System.Reflection.PropertyInfo i)
        {
            var propType = i.PropertyType;
            if (propType == typeof(System.SByte))
            {
                pkg.Write( System.Convert.ToSByte(i.GetValue(this,null)));
            }
            else if(propType==typeof(System.Int16))
		    {
		        pkg.Write( System.Convert.ToInt16(i.GetValue(this,null)));
	        }
		    else if(propType==typeof(System.Int32))
		    {
		        pkg.Write( System.Convert.ToInt32(i.GetValue(this,null)));
	        }
		    else if(propType==typeof(System.Int64))
		    {
		        pkg.Write( System.Convert.ToInt64(i.GetValue(this,null)));
	        }
		    else if(propType==typeof(System.Byte))
		    {
		        pkg.Write( System.Convert.ToByte(i.GetValue(this,null)));
	        }
		    else if(propType==typeof(System.UInt16))
		    {
		        pkg.Write( System.Convert.ToUInt16(i.GetValue(this,null)));
	        }
		    else if(propType==typeof(System.UInt32))
		    {
		        pkg.Write( System.Convert.ToUInt32(i.GetValue(this,null)));
	        }
		    else if(propType==typeof(System.UInt64))
		    {
		        pkg.Write( System.Convert.ToUInt64(i.GetValue(this,null)));
	        }
		    else if(propType==typeof(System.Single))
		    {
		        pkg.Write( System.Convert.ToSingle(i.GetValue(this,null)));
	        }
		    else if(propType==typeof(System.Double))
		    {
		        pkg.Write( System.Convert.ToDouble(i.GetValue(this,null)));
	        }
		    else if(propType==typeof(System.Guid))
		    {
		        System.Guid id = (System.Guid)(i.GetValue(this,null));
		        pkg.Write( id );
	        }
            else if (propType == typeof(SlimDX.Vector2))
            {
                SlimDX.Vector2 id = (SlimDX.Vector2)(i.GetValue(this, null));
                pkg.Write(id);
            }
            else if (propType == typeof(SlimDX.Vector3))
            {
                SlimDX.Vector3 id = (SlimDX.Vector3)(i.GetValue(this, null));
                pkg.Write(id);
            }
            else if (propType == typeof(SlimDX.Vector4))
            {
                SlimDX.Vector4 id = (SlimDX.Vector4)(i.GetValue(this, null));
                pkg.Write(id);
            }
            else if (propType == typeof(SlimDX.Quaternion))
            {
                SlimDX.Quaternion id = (SlimDX.Quaternion)(i.GetValue(this, null));
                pkg.Write(id);
            }
            else if (propType == typeof(SlimDX.Matrix))
            {
                SlimDX.Matrix id = (SlimDX.Matrix)(i.GetValue(this, null));
                pkg.Write(id);
            }
            else if (propType == typeof(System.DateTime))
            {
                System.DateTime id = (System.DateTime)(i.GetValue(this, null));
                pkg.Write(id);
            }
            else if (propType == typeof(System.String))
            {
                System.String str = (System.String)(i.GetValue(this, null));
                pkg.Write(str);
            }
            else if (propType == typeof(System.Byte[]))
            {
                System.Byte[] str = (System.Byte[])(i.GetValue(this, null));
                pkg.Write(str);
            }
            else
            {
                return false;
            }
            return true;
        }

        protected bool ReadPkg(PackageProxy pkg, System.Reflection.PropertyInfo i)
        {
            var propType = i.PropertyType;
            if(propType==typeof(System.SByte))
            {
		        System.SByte value;
		        pkg.Read(out value);
		        i.SetValue(this,value,null);
	        }
		    else if(propType==typeof(System.Int16))
		    {
		        System.Int16 value;
		        pkg.Read(out value);
		        i.SetValue(this,value,null);
	        }
		    else if(propType==typeof(System.Int32))
		    {
		        System.Int32 value;
		        pkg.Read(out value);
		        i.SetValue(this,value,null);
	        }
		    else if(propType==typeof(System.Int64))
		    {
		        System.Int64 value;
		        pkg.Read(out value);
		        i.SetValue(this,value,null);
	        }
		    else if(propType==typeof(System.Byte))
		    {
		        System.Byte value;
		        pkg.Read(out value);
		        i.SetValue(this,value,null);
	        }
		    else if(propType==typeof(System.UInt16))
		    {
		        System.UInt16 value;
		        pkg.Read(out value);
		        i.SetValue(this,value,null);
	        }
		    else if(propType==typeof(System.UInt32))
		    {
		        System.UInt32 value;
		        pkg.Read(out value);
		        i.SetValue(this,value,null);
	        }
		    else if(propType==typeof(System.UInt64))
		    {
		        System.UInt64 value;
		        pkg.Read(out value);
		        i.SetValue(this,value,null);
	        }
		    else if(propType==typeof(System.Single))
		    {
		        System.Single value;
		        pkg.Read(out value);
		        i.SetValue(this,value,null);
	        }
		    else if(propType==typeof(System.Double))
		    {
		        System.Double value;
		        pkg.Read(out value);
		        i.SetValue(this,value,null);
	        }
		    else if(propType==typeof(System.Guid))
		    {
		        System.Guid value;
		        pkg.Read(out value);
		        i.SetValue(this,value,null);
	        }
		    else if(propType==typeof(SlimDX.Vector3))
		    {
		        SlimDX.Vector3 value;
		        pkg.Read(out value);
		        i.SetValue(this,value,null);
	        }
		    else if(propType==typeof(SlimDX.Matrix))
		    {
		        SlimDX.Matrix value;
		        pkg.Read(out value);
		        i.SetValue(this,value,null);
	        }
		    else if(propType==typeof(System.DateTime))
		    {
		        System.DateTime value;
		        pkg.Read(out value);
		        i.SetValue(this,value,null);
	        }
		    else if(propType==typeof(System.String))
		    {
		        System.String value="";
		        pkg.Read(out value);
		        i.SetValue(this,value,null);
	        }
            else if (propType == typeof(System.Byte[]))
            {
                System.Byte[] value = new System.Byte[0];
                pkg.Read(out value);
                i.SetValue(this, value, null);
            }
            else
            {
                return false;
            }
            return true;
        }

        protected bool WriteData(DataWriter pkg, System.Reflection.PropertyInfo i)
        {
            var propType = i.PropertyType;
            if (propType == typeof(System.SByte))
            {
                pkg.Write(System.Convert.ToSByte(i.GetValue(this, null)));
            }
            else if (propType == typeof(System.Int16))
            {
                pkg.Write(System.Convert.ToInt16(i.GetValue(this, null)));
            }
            else if (propType == typeof(System.Int32))
            {
                pkg.Write(System.Convert.ToInt32(i.GetValue(this, null)));
            }
            else if (propType == typeof(System.Int64))
            {
                pkg.Write(System.Convert.ToInt64(i.GetValue(this, null)));
            }
            else if (propType == typeof(System.Byte))
            {
                pkg.Write(System.Convert.ToByte(i.GetValue(this, null)));
            }
            else if (propType == typeof(System.UInt16))
            {
                pkg.Write(System.Convert.ToUInt16(i.GetValue(this, null)));
            }
            else if (propType == typeof(System.UInt32))
            {
                pkg.Write(System.Convert.ToUInt32(i.GetValue(this, null)));
            }
            else if (propType == typeof(System.UInt64))
            {
                pkg.Write(System.Convert.ToUInt64(i.GetValue(this, null)));
            }
            else if (propType == typeof(System.Single))
            {
                pkg.Write(System.Convert.ToSingle(i.GetValue(this, null)));
            }
            else if (propType == typeof(System.Double))
            {
                pkg.Write(System.Convert.ToDouble(i.GetValue(this, null)));
            }
            else if (propType == typeof(System.Guid))
            {
                System.Guid id = (System.Guid)(i.GetValue(this, null));
                pkg.Write(id);
            }
            else if (propType == typeof(SlimDX.Vector3))
            {
                SlimDX.Vector3 id = (SlimDX.Vector3)(i.GetValue(this, null));
                pkg.Write(id);
            }
            else if (propType == typeof(SlimDX.Matrix))
            {
                SlimDX.Matrix id = (SlimDX.Matrix)(i.GetValue(this, null));
                pkg.Write(id);
            }
            else if (propType == typeof(System.DateTime))
            {
                System.DateTime id = (System.DateTime)(i.GetValue(this, null));
                pkg.Write(id);
            }
            else if (propType == typeof(System.String))
            {
                System.String str = (System.String)(i.GetValue(this, null));
                pkg.Write(str);
            }
            else if (propType == typeof(System.Byte[]))
            {
                System.Byte[] str = (System.Byte[])(i.GetValue(this, null));
                pkg.Write(str);
            }
            else
            {
                return false;
            }
            return true;
        }

        protected bool ReadData(DataReader pkg, System.Reflection.PropertyInfo i)
        {
            var propType = i.PropertyType;
            if (propType == typeof(System.SByte))
            {
                System.SByte value;
                pkg.Read(out value);
                i.SetValue(this, value, null);
            }
            else if (propType == typeof(System.Int16))
            {
                System.Int16 value;
                pkg.Read(out value);
                i.SetValue(this, value, null);
            }
            else if (propType == typeof(System.Int32))
            {
                System.Int32 value;
                pkg.Read(out value);
                i.SetValue(this, value, null);
            }
            else if (propType == typeof(System.Int64))
            {
                System.Int64 value;
                pkg.Read(out value);
                i.SetValue(this, value, null);
            }
            else if (propType == typeof(System.Byte))
            {
                System.Byte value;
                pkg.Read(out value);
                i.SetValue(this, value, null);
            }
            else if (propType == typeof(System.UInt16))
            {
                System.UInt16 value;
                pkg.Read(out value);
                i.SetValue(this, value, null);
            }
            else if (propType == typeof(System.UInt32))
            {
                System.UInt32 value;
                pkg.Read(out value);
                i.SetValue(this, value, null);
            }
            else if (propType == typeof(System.UInt64))
            {
                System.UInt64 value;
                pkg.Read(out value);
                i.SetValue(this, value, null);
            }
            else if (propType == typeof(System.Single))
            {
                System.Single value;
                pkg.Read(out value);
                i.SetValue(this, value, null);
            }
            else if (propType == typeof(System.Double))
            {
                System.Double value;
                pkg.Read(out value);
                i.SetValue(this, value, null);
            }
            else if (propType == typeof(System.Guid))
            {
                System.Guid value;
                pkg.Read(out value);
                i.SetValue(this, value, null);
            }
            else if (propType == typeof(SlimDX.Vector3))
            {
                SlimDX.Vector3 value;
                pkg.Read(out value);
                i.SetValue(this, value, null);
            }
            else if (propType == typeof(SlimDX.Matrix))
            {
                SlimDX.Matrix value;
                pkg.Read(out value);
                i.SetValue(this, value, null);
            }
            else if (propType == typeof(System.DateTime))
            {
                System.DateTime value;
                pkg.Read(out value);
                i.SetValue(this, value, null);
            }
            else if (propType == typeof(System.String))
            {
                System.String value = "";
                pkg.Read(out value);
                i.SetValue(this, value, null);
            }
            else if (propType == typeof(System.Byte[]))
            {
                System.Byte[] value = new System.Byte[0];
                pkg.Read(out value);
                i.SetValue(this, value, null);
            }
            else
            {
                return false;
            }
            return true;
        }
        #endregion

        public static void PackageWriteList<T>(List<T> list, RPC.PackageWriter pkg)
            where T : RPC.IAutoSaveAndLoad
        {
            UInt16 count = (UInt16)list.Count;
            pkg.Write(count);
            foreach (var i in list)
            {
                i.PackageWrite(pkg);
            }
        }

        public static void PackageReadList<T>(List<T> list, RPC.PackageProxy pkg)
            where T : RPC.IAutoSaveAndLoad, new()
        {
            UInt16 count;
            pkg.Read(out count);
            list.Clear();
            for (int i = 0; i < count; i++)
            {
                T iv = new T();
                iv.PackageRead(pkg);
                list.Add(iv);
            }
        }

        public static void DaraWriteList<T>(List<T> list, RPC.DataWriter pkg, bool bToClient)
            where T : RPC.IAutoSaveAndLoad
        {
            UInt16 count = (UInt16)list.Count;
            pkg.Write(count);
            foreach (var i in list)
            {
                RPC.DataWriter idr = new RPC.DataWriter();
                i.DataWrite(idr, bToClient);
                pkg.Write(idr);
            }
        }

        public static void DaraReadList<T>(List<T> list, RPC.DataReader pkg, bool bToClient)
            where T : RPC.IAutoSaveAndLoad,new()
        {
            UInt16 count;
            pkg.Read(out count);
            list.Clear();
            for (int i = 0; i < count; i++)
            {
                RPC.DataReader idr;
                pkg.Read(out idr);
                T iv = new T();
                iv.DataRead(idr, bToClient);
                list.Add(iv);
            }
        }


        public virtual void PackageWriteFull(PackageWriter pkg)
	    {
		    var desc = IAutoSLClassDescManager.Instance.GetClassDesc( this.GetType() );
		    foreach(IAutoSLField f in desc.Fields)
		    {
			    var i = f.Property;

                if (WritePkg(pkg, i))
                {
                }
                else if(i.PropertyType.IsSubclassOf(typeof(IAutoSaveAndLoad)))
                {
                    var obj = (IAutoSaveAndLoad)(i.GetValue(this,null));
					pkg.Write( obj );
                }
		    }
	    }
        public virtual void PackageReadFull(PackageProxy pkg)
	    {
		    var desc = IAutoSLClassDescManager.Instance.GetClassDesc( this.GetType() );
		    foreach(IAutoSLField f in desc.Fields)
		    {
                var i = f.Property;

                if (ReadPkg(pkg, i))
                {
                }
                else if (i.PropertyType.IsSubclassOf(typeof(IAutoSaveAndLoad)))
                {
                    var value = (RPC.IAutoSaveAndLoad)System.Activator.CreateInstance(i.PropertyType);
					pkg.Read(value);
					i.SetValue(this,value,null);
                }
		    }
	    }

        public virtual void PackageWriteSingle(PackageWriter pkg)
	    {
		    var desc = IAutoSLClassDescManager.Instance.GetClassDesc( this.GetType() );
		    foreach(IAutoSLField f in desc.Fields)
		    {
			    if(f.IgnoreWhenSingle)
				    continue;

			    var i = f.Property;

                if (WritePkg(pkg, i))
                {
                }
                else if (i.PropertyType.IsSubclassOf(typeof(IAutoSaveAndLoad)))
                {
                    var obj = (IAutoSaveAndLoad)(i.GetValue(this, null));
                    pkg.Write(obj);
                }
		    }
	    }
        public virtual void PackageReadSingle(PackageProxy pkg)
	    {
		    var desc = IAutoSLClassDescManager.Instance.GetClassDesc( this.GetType() );
		    foreach(IAutoSLField f in desc.Fields)
		    {
			    if(f.IgnoreWhenSingle)
				    continue;

			    var i = f.Property;

                if (ReadPkg(pkg, i))
                {
                }
                else if (i.PropertyType.IsSubclassOf(typeof(IAutoSaveAndLoad)))
                {
                    var value = (RPC.IAutoSaveAndLoad)System.Activator.CreateInstance(i.PropertyType);
                    pkg.Read(value);
                    i.SetValue(this, value, null);
                }
		    }
	    }
        public virtual void DataWrite(DataWriter pkg, bool bToClient)
	    {
		    if( bToClient )
		    {
			    DataWriteSingle(pkg);
		    }
		    else
		    {
			    DataWriteFull(pkg);
		    }
	    }
        public virtual void DataRead(DataReader pkg, bool bToClient)
	    {
		    if( bToClient )
		    {
			    DataReadSingle(pkg);
		    }
		    else
		    {
			    DataReadFull(pkg);
		    }
	    }

        public virtual void DataWriteFull(DataWriter pkg)
	    {
		    var desc = IAutoSLClassDescManager.Instance.GetClassDesc( this.GetType() );
		    foreach(IAutoSLField f in desc.Fields)
		    {
			    var i = f.Property;

                if (WriteData(pkg, i))
                {
                }
                else if (i.PropertyType.IsSubclassOf(typeof(IAutoSaveAndLoad)))
                {
                    var obj = (IAutoSaveAndLoad)(i.GetValue(this, null));
                    pkg.Write(obj,false);
                }
		    }
	    }
        public virtual void DataReadFull(DataReader pkg)
	    {
		    var desc = IAutoSLClassDescManager.Instance.GetClassDesc( this.GetType() );
		    foreach(IAutoSLField f in desc.Fields)
		    {
			    var i = f.Property;

                if (ReadData(pkg, i))
                {
                }
                else if (i.PropertyType.IsSubclassOf(typeof(IAutoSaveAndLoad)))
                {
                    var value = (RPC.IAutoSaveAndLoad)System.Activator.CreateInstance(i.PropertyType);
                    pkg.Read(value,false);
                    i.SetValue(this, value, null);
                }
		    }
	    }

        public virtual void DataWriteSingle(DataWriter pkg)
	    {
		    var desc = IAutoSLClassDescManager.Instance.GetClassDesc( this.GetType() );
		    foreach(IAutoSLField f in desc.Fields)
		    {
			    if(f.IgnoreWhenSingle)
				    continue;

			    var i = f.Property;

                if (WriteData(pkg, i))
                {
                }
                else if (i.PropertyType.IsSubclassOf(typeof(IAutoSaveAndLoad)))
                {
                    var obj = (IAutoSaveAndLoad)(i.GetValue(this, null));
                    pkg.Write(obj, true);
                }
		    }
	    }
        public virtual void DataReadSingle(DataReader pkg)
	    {
		    var desc = IAutoSLClassDescManager.Instance.GetClassDesc( this.GetType() );
		    foreach(IAutoSLField f in desc.Fields)
		    {
			    if(f.IgnoreWhenSingle)
				    continue;

			    var i = f.Property;

                if (ReadData(pkg, i))
                {
                }
                else if (i.PropertyType.IsSubclassOf(typeof(IAutoSaveAndLoad)))
                {
                    var value = (RPC.IAutoSaveAndLoad)System.Activator.CreateInstance(i.PropertyType);
                    pkg.Read(value, true);
                    i.SetValue(this, value, null);
                }
		    }
	    }

        public virtual void DataWrite(DataWriter pkg, int minLevel, int maxLevel)
	    {
		    var desc = IAutoSLClassDescManager.Instance.GetClassDesc( this.GetType() );
		    foreach(IAutoSLField f in desc.Fields)
		    {
			    if(f.Level>maxLevel || f.Level<minLevel)
				    continue;

			    var i = f.Property;

                WriteData(pkg, i);
		    }
	    }
        public virtual void DataRead(DataReader pkg, int minLevel, int maxLevel)
	    {
		    var desc = IAutoSLClassDescManager.Instance.GetClassDesc( this.GetType() );
		    foreach(IAutoSLField f in desc.Fields)
		    {
			    if(f.Level>maxLevel || f.Level<minLevel)
				    continue;

			    var i = f.Property;

                ReadData(pkg, i);
		    }
	    }

	    public virtual IAutoSaveAndLoad CloneObject()
	    {
		    IAutoSaveAndLoad result = (IAutoSaveAndLoad)System.Activator.CreateInstance(this.GetType());

		    var desc = IAutoSLClassDescManager.Instance.GetClassDesc( this.GetType() );
		    foreach(IAutoSLField f in desc.Fields)
		    {
			    var i = f.Property;

			    System.Type type = i.PropertyType;
			    i.SetValue(result,i.GetValue(this,null),null);
		    }
		    return result;
	    }

        public virtual string GetCppStructureDefine()
        {
            const string TabWord = "	";

            Type objType = GetType();
            System.Reflection.PropertyInfo[] fields = objType.GetProperties();
            String defStr = "";
            String writeStr = "";
            String readStr = "";

            String writeDataStr = "";
            String readDataStr = "";

            for (int i = 0; i < fields.Length; i++)
            {
                System.Reflection.PropertyInfo ri = fields[i];
                //object[] objAttrs = ri.GetCustomAttributes(typeof(FieldDontAutoSaveLoadAttribute), true);
                //object[] objSingleAttrs = ri.GetCustomAttributes(typeof(FieldDontAutoSingleSaveLoadAttribute), true);
                //if (objAttrs == null || objAttrs.Length == 0 || objSingleAttrs == null || objSingleAttrs.Length == 0)

                object[] objAttrs = ri.GetCustomAttributes(typeof(AutoSaveLoadAttribute), true);
                if (objAttrs.Length == 0)
                    continue;
                var attr = (objAttrs[0] as AutoSaveLoadAttribute);
                if (attr.IsSingle)
                {
                    Type type = ri.PropertyType;
                    string TypeName = type.FullName.Replace("SlimDX", "UnityEngine");
                    if (TypeName.Contains("System.Collections.Generic") || (TypeName.Contains("[]") && type != typeof(byte[])))
                        continue;

                    defStr += TabWord + TabWord + "public " + TypeName + " " + ri.Name + ";\r\n";
                    writeStr += TabWord + TabWord + TabWord + "pkg.Write(" + ri.Name + ");\r\n";
                    readStr += TabWord + TabWord + TabWord + "pkg.Read(out " + ri.Name + ");\r\n";
                    writeDataStr += TabWord + TabWord + TabWord + "pkg.Write(" + ri.Name + ");\r\n";
                    readDataStr += TabWord + TabWord + TabWord + "pkg.Read(out " + ri.Name + ");\r\n";
                }
            }

            String strOut = "";
            strOut += "namespace CSCommon.Data \r\n";
            strOut += "{ \r\n";
            strOut += TabWord + "[System.Serializable] \r\n";
            strOut += TabWord + "public class " + objType.Name + ": RPC.IAutoSaveAndLoad \r\n";
            strOut += TabWord + "{ \r\n";
            strOut += defStr + "\r\n";
            strOut += TabWord + TabWord + "public override void PackageWrite(RPC.PackageWriter pkg)\r\n";
            strOut += TabWord + TabWord + "{ \r\n";
            strOut += writeStr;
            strOut += TabWord + TabWord + "}\r\n";
            strOut += TabWord + TabWord + "public override void PackageRead(RPC.PackageProxy pkg)\r\n";
            strOut += TabWord + TabWord + "{ \r\n";
            strOut += readStr;
            strOut += TabWord + TabWord + "}\r\n";
            strOut += TabWord + "}\r\n";
            strOut += "}\n";

            return strOut;
        }

	}
}
