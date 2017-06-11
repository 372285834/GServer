using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCommon
{

    [ServerFrame.Editor.CDataEditorAttribute(".ip")]
    public class ClientIPConfig : ServerFrame.CommonTemplate<ClientIPConfig>
    {
        string mIpAddress = "127.0.0.1";
        [System.ComponentModel.DisplayName("IP地址")]
        [ServerFrame.Config.DataValueAttribute("IpAddress")]
        public string IpAddress
        {
            get { return mIpAddress; }
            set { mIpAddress = value; }
        }


        public override string GetFilePath()
        {
            return "ip.ip";
        }

    }
}
