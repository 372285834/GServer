using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace CSCommon.Component
{
    // 对象层管理器，用于世界浏览器对象分层管理
    public class IActorLayerManger : INotifyPropertyChanged
    {
        #region INotifyPropertyChangedMembers
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        static IActorLayerManger smInstance = new IActorLayerManger();
        public static IActorLayerManger Instance
        {
            get { return smInstance; }
        }

        List<string> mLayers = new List<string>();
        [ServerFrame.Config.DataValueAttribute("Layers")]
        public List<string> Layers
        {
            get { return mLayers; }
            set { mLayers = value; }
        }

        public IActorLayerManger()
        {
            Load();
        }

        public bool AddLayer(string layerName)
        {
            if (Layers.Contains(layerName))
                return false;

            Layers.Add(layerName);

            return true;
        }

        public bool RemoveLayer(string layerName)
        {
            return Layers.Remove(layerName);
        }

        public string GetFileName()
        {
            return ServerFrame.Support.IFileConfig.Instance.EditorSourceDirectory + "\\LayerConfig.xml";
        }

        public void Load()
        {
            ServerFrame.Config.IConfigurator.FillProperty(this, GetFileName());
        }

        public void Save()
        {
            //ServerFrame.Support.IConfigurator.SaveProperty(this, "Layers", GetFileName());

            //var layerConfigFile = ServerFrame.Support.IFileManager.Instance.Root + GetFileName();
            //var svnState = SvnInterface.Commander.Instance.CheckStatusEx(layerConfigFile);
            //switch (svnState)
            //{
            //    case SvnInterface.SvnStatus.NotControl:
            //        SvnInterface.Commander.Instance.Add(layerConfigFile);
            //        break;
            //}

            //SvnInterface.Commander.Instance.Commit(layerConfigFile);
        }
    }
}
