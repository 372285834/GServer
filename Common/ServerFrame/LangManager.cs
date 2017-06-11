using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerFrame
{
    public class LangManager
    {
        Dictionary<string, int> mIdMap = new Dictionary<string, int>();
        List<string> mStrings = new List<string>();
        string mErrorString;
        string mLangPackageFile;

        static LangManager mInstance = new LangManager();
        public static LangManager Instance
        {
            get { return mInstance; }
        }

        public LangManager()
        {
            mErrorString = "error string id";
        }

        public bool LoadLanguage(string file)
        {
            if (!System.IO.File.Exists(file))
                return false;

            mLangPackageFile = file;
            mStrings.Clear();

            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.Load(file);

            mStrings.Clear();
            mIdMap.Clear();
            foreach (System.Xml.XmlElement node in doc.DocumentElement.FirstChild.ChildNodes)
            {
                var idNodes = node.GetElementsByTagName("StringId");
                var contentNodes = node.GetElementsByTagName("Content");

                if (idNodes.Count > 0 && contentNodes.Count > 0)
                {
                    System.Xml.XmlElement idElement = idNodes[0] as System.Xml.XmlElement;
                    System.Xml.XmlElement contentElement = contentNodes[0] as System.Xml.XmlElement;

                    mStrings.Add(contentElement.GetAttribute("Value"));
                    int id = (int)mStrings.Count - 1;
                    mIdMap[idElement.GetAttribute("Value")] = id;
                }
            }

            return true;
        }

        public string GetLangStr(string str)
        {
            return GetString(RegString(str));
        }

        public int GetStringId(string str)
        {
            int retValue = -1;
            mIdMap.TryGetValue(str, out retValue);
            return retValue;
        }

        public string GetString(int idx)
        {
            if (idx < 0 || idx >= mStrings.Count)
                return mErrorString;

            return mStrings[idx];
        }

        public string GetStringSlow(string str)
        {
            return GetString(GetStringId(str));
        }

        public int RegString(string str)
        {
            int id = -1;
            if (mIdMap.TryGetValue(str, out id))
            {
                return id;
            }

            mStrings.Add(str);
            id = mStrings.Count - 1;
            mIdMap[str] = id;
            
            return id;
        }

        public void SaveLangPackage()
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml("<ServerCommon.Data.Tools.LangPackage></ServerCommon.Data.Tools.LangPackage>");

            var element = doc.CreateElement("Strings");
            var att = doc.CreateAttribute("Type");
            att.Value = "List";
            element.Attributes.InsertAfter(null, att);

            att = doc.CreateAttribute("DataType");
            att.Value = "ServerCommon.dll@ServerCommon.Data.Tools.LangString";
            element.Attributes.InsertAfter(null, att);
            doc.DocumentElement.AppendChild(element);

            foreach (var id in mIdMap.Keys)
            {
                var idElement = doc.CreateElement("StringId");
                idElement.SetAttribute("Type", "String");
                idElement.SetAttribute("Value", id);
                element.AppendChild(idElement);

                int contentIdx = -1;
                mIdMap.TryGetValue(id, out contentIdx);

                var contentElement = doc.CreateElement("Content");
                contentElement.SetAttribute("Type", "String");
                contentElement.SetAttribute("Value", GetString(contentIdx));
                element.AppendChild(contentElement);
            }

            doc.Save(mLangPackageFile);
        }
    }
}
