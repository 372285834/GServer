using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ServerFrame
{
    public abstract class TemplateManager<T, ItemT> : Singleton<T>
        where ItemT : new()
        where T : new()
    {
        public abstract string GetFilePath();
        public void LoadAllTemplate(string tPath)
        {
            try
            {
                mAllItems.Clear();
                var files = System.IO.Directory.GetFiles(System.IO.Path.Combine(tPath, GetFilePath()));

                var cType = typeof(ItemT);
                var atts = cType.GetCustomAttributes(typeof(ServerFrame.Editor.CDataEditorAttribute), true);
                string suffix = "";
                if (atts.Length > 0)
                {
                    ServerFrame.Editor.CDataEditorAttribute dea = atts[0] as ServerFrame.Editor.CDataEditorAttribute;
                    suffix = dea.m_strFileExt.Substring(1);
                }

                foreach (var i in files)
                {
                    if (i.Contains("meta"))
                        continue;

                    string[] strs = i.Substring(i.LastIndexOf("\\") + 1).Split('.');
                    if (strs.Length < 2)
                        continue;
                    if (strs[1] == suffix)
                    {
                        string fullPathname = i;
                        ItemT item = new ItemT();
                        if (ServerFrame.Config.IConfigurator.FillProperty(item, fullPathname))
                        {
                            var templateid = int.Parse(strs[0]);
                            mAllItems.Add(templateid, item);
                            OnLoadedItem(item, templateid);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
            	
            }
            
        }

        public void ReloadItem(Int32 tid)
        {
            mAllItems.Remove(tid);
            var path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "");
            string tempPath = System.IO.Path.Combine(path, GetFilePath());

            var cType = typeof(ItemT);
            var atts = cType.GetCustomAttributes(typeof(ServerFrame.Editor.CDataEditorAttribute), true);
            string suffix = "";
            if (atts.Length > 0)
            {
                ServerFrame.Editor.CDataEditorAttribute dea = atts[0] as ServerFrame.Editor.CDataEditorAttribute;
                suffix = dea.m_strFileExt.Substring(1);
            }

            string itemname = tid.ToString() + "." + suffix;
            string fullPathname = System.IO.Path.Combine(tempPath, itemname);

            ItemT item = new ItemT();
            if (ServerFrame.Config.IConfigurator.FillProperty(item, fullPathname))
            {
                var templateid = tid;
                mAllItems.Add(templateid, item);
                OnLoadedItem(item, templateid);
            }
        }

        Dictionary<int, ItemT> mAllItems = new Dictionary<int, ItemT>();
        public ItemT GetTemplate(Int32 tid)
        {
            ItemT item;
            if (mAllItems.TryGetValue(tid, out item))
            {
                return item;
            }
            return default(ItemT);
        }

        public virtual void OnLoadedItem(ItemT item, int templateid)
        {

        }

    }

    public abstract class CommonTemplate<T> : Singleton<T>
        where T : new()
    {

        public abstract string GetFilePath();
        public void LoadCommonTemplate(string tPath)
        {
            var filePath = System.IO.Path.Combine(tPath, GetFilePath());
            if (System.IO.File.Exists(filePath))
                Config.IConfigurator.FillProperty(this, filePath);
            OnInitIndex();
        }
        public virtual void OnInitIndex()
        {

        }
    }
}
