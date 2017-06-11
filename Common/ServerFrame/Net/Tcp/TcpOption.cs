
using System.Net;

namespace Iocp
{
    public enum NetState
    {
        Invalid,
        Accept,
        Connect,
        Disconnect,
    };

    public class TcpOption
    {
        public TcpOption(int maxConnections, int maxAcceptOps, int bufferSize)
        {
            this.MaxConnections = maxConnections;
            this.NumOfSaeaForRecSend = 2 * maxConnections;
            this.MaxAcceptOps = maxAcceptOps;
            this.BufferSize = bufferSize;
        }

        /// <summary>
        /// 最大链接数
        /// </summary>
        public int MaxConnections { get; private set; }
        
        /// <summary>
        /// 并发的IO数量
        /// </summary>
		public int NumOfSaeaForRecSend { get; private set; }
        
        /// <summary>
        /// 并发的Accept数量
        /// </summary>
		public int MaxAcceptOps { get; private set; }
        
        /// <summary>
        /// 缓冲区大小
        /// </summary>
		public int BufferSize { get; private set; }

        //public static TcpOption ForServer = new TcpOption(10000, 1024, 8192);
        public static TcpOption ForRegServer = new TcpOption(3000, 1024, 1024*64);
        public static TcpOption ForGateServer = new TcpOption(3000, 1024, 1024 * 64);
        public static TcpOption ForDataServer = new TcpOption(100, 1024, 1024 * 64);
        public static TcpOption ForPlanesServer = new TcpOption(100, 1024, 1024 * 64);
        public static TcpOption ForComServer = new TcpOption(100, 1024, 1024 * 64);
        public static TcpOption ForPathServer = new TcpOption(100, 1024, 1024 * 64);
    }
}