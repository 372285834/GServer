
namespace Iocp
{
    /// <summary>
    /// 提供给SocketAsyncEventArgs.UserToken使用
    /// </summary>
    class AsyncUserToken
    {
        internal byte[] messageBytes;
        internal byte[] prefixBytes;
        internal int messageBytesDone;
        internal int prefixBytesDone;
        internal int messageLength;
        internal int bufferOffset;
        internal int bufferSkip;
        internal TcpConnect tcpConn;
        public long SendTime;

        internal bool IsPrefixReady
        {
            get { return prefixBytesDone == NetPacketParser.PREFIX_SIZE; }
        }

        internal int DataOffset
        {
            get { return bufferOffset + bufferSkip; }
        }
        internal int RemainByte
        {
            get { return messageLength - messageBytesDone; }
        }
        internal bool IsMessageReady
        {
            get { return messageBytesDone == messageLength; }
        }
        internal AsyncUserToken()
        {
            prefixBytes = new byte[NetPacketParser.PREFIX_SIZE];
        }

        internal void Reset(bool skip)
        {
            this.messageBytes = null;
            for (int i = 0; i < NetPacketParser.PREFIX_SIZE; i++)
                prefixBytes[i] = 0;

            this.messageBytesDone = 0;
            this.prefixBytesDone = 0;
            this.messageLength = 0;
            if (skip)
                this.bufferSkip = 0;
        }
    }
}