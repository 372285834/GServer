using System;
using System.Net.Sockets;

namespace Iocp
{
    class NetPacketParser
    {
        static public int PREFIX_SIZE = 2;

        static public int HandlePrefix(SocketAsyncEventArgs saea, AsyncUserToken dataToken, int remainingBytesToProcess)
        {
            if (remainingBytesToProcess >= PREFIX_SIZE - dataToken.prefixBytesDone)
            {
                for (int i = 0; i < PREFIX_SIZE - dataToken.prefixBytesDone; i++)
                {
                    dataToken.prefixBytes[dataToken.prefixBytesDone + i] = saea.Buffer[dataToken.DataOffset + i];
                }
                remainingBytesToProcess = remainingBytesToProcess - PREFIX_SIZE + dataToken.prefixBytesDone;
                dataToken.bufferSkip += PREFIX_SIZE - dataToken.prefixBytesDone;
                dataToken.prefixBytesDone = PREFIX_SIZE;
                dataToken.messageLength = BitConverter.ToUInt16(dataToken.prefixBytes, 0);
            }
            else
            {
                for (int i = 0; i < remainingBytesToProcess; i++)
                {
                    dataToken.prefixBytes[dataToken.prefixBytesDone + i] = saea.Buffer[dataToken.DataOffset + i];
                }
                dataToken.prefixBytesDone += remainingBytesToProcess;
                remainingBytesToProcess = 0;
            }

            return remainingBytesToProcess;
        }

        static public int HandleMessage(SocketAsyncEventArgs saea, AsyncUserToken dataToken, int remainingBytesToProcess)
        {
            if (dataToken.messageBytesDone == 0)
            {
                dataToken.messageBytes = new byte[dataToken.messageLength];
            }

            var nonCopiedBytes = 0;
            if (remainingBytesToProcess + dataToken.messageBytesDone >= dataToken.messageLength)
            {
                var copyedBytes = dataToken.RemainByte;
                nonCopiedBytes = remainingBytesToProcess - copyedBytes;
                Buffer.BlockCopy(saea.Buffer, dataToken.DataOffset, dataToken.messageBytes, dataToken.messageBytesDone, copyedBytes);
                dataToken.messageBytesDone = dataToken.messageLength;
                dataToken.bufferSkip += copyedBytes;
            }
            else
            {
                Buffer.BlockCopy(saea.Buffer, dataToken.DataOffset, dataToken.messageBytes, dataToken.messageBytesDone, remainingBytesToProcess);
                dataToken.messageBytesDone += remainingBytesToProcess;
            }

            return nonCopiedBytes;
        }
    }
}
