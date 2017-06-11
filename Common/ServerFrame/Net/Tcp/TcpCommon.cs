using System;

namespace Iocp
{
    public class NetConnection
	{
	    public System.Object m_BindData;
		public int mLimitLevel;

        public virtual void SendBuffer(byte[] data, int offset, int count)
		{
			
		}

        public virtual void Disconnect()
		{
			
		}
	};
}
