using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ServerCommon
{
    [Serializable]
    public class ServerConfig
    {
        static ServerConfig mInstance;
        public static ServerConfig Instance 
        {
            get 
            {
                if (mInstance == null)
                {
                    Reload();
                }
                return mInstance;
            } 
        }

        public IRegisterServerParameter RegParam = new IRegisterServerParameter();
        public IGateServerParameter GateParam = new IGateServerParameter();
        public IDataServerParameter DataParam = new IDataServerParameter();
        public IPlanesServerParameter PlaneParam = new IPlanesServerParameter();
        public IComServerParameter ComParam = new IComServerParameter();

        public string TablePath = @"../../Client/Assets/StreamingAssets/TableData";
        public string TemplatePath = @"../../Client/Assets/StreamingAssets/Template/";
        public string MapInfoPath = @"../../Client/Assets/StreamingAssets/Mapinfo/";
        public string MapNavPath = @"Data/MapNavs/";
        public string NamePath = @"Data/Name";

        public static ServerConfig Load()
        {
            var path = @"Bin\srvcfg\config.cfg";
            if (!File.Exists(path))
            {
                var cf = new ServerConfig();
                cf.Save(path);
                Log.Log.Common.Print(Log.LogLevel.Warning, "not find ServerConfig!");
                return cf;
            }
            using (System.IO.FileStream sr = new System.IO.FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                System.Xml.Serialization.XmlSerializer xml = new System.Xml.Serialization.XmlSerializer(typeof(ServerConfig));
                return xml.Deserialize(sr) as ServerConfig;
            }            
        }

        public static void Reload()
        {
            mInstance = Load();
        }

        public void Save(string path)
        {
            using (System.IO.FileStream sr = new System.IO.FileStream(path, FileMode.OpenOrCreate))
            {
                System.Xml.Serialization.XmlSerializer xml = new System.Xml.Serialization.XmlSerializer(typeof(ServerConfig));
                xml.Serialize(sr, this);
            }
        }
    }
}
