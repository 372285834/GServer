using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerFrame
{
    [System.ComponentModel.TypeConverterAttribute("System.ComponentModel.ExpandableObjectConverter")]
    public class NetEndPoint
    {
        public NetEndPoint()
        {

        }
        public NetEndPoint(string ip, UInt16 p)
        {
            IpAddress = ip; Port = p;
        }
        string mIpAddress;
        public string IpAddress
        {
            get { return mIpAddress; }
            set { mIpAddress = value; }
        }
        UInt16 mPort;
        public UInt16 Port
        {
            get { return mPort; }
            set { mPort = value; }
        }

        Iocp.NetConnection mConnect;
        public Iocp.NetConnection Connect
        {
            get { return mConnect; }
            set { mConnect = value; }
        }
        ulong mId;
        public ulong Id
        {
            get { return mId; }
            set { mId = value; }
        }
    }
}
