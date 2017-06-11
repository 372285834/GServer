using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerFrame.Support
{
    public class BitSet
    {
        int mBitCount;
        public int BitCount
        {
            get { return mBitCount; }
        }
        System.Byte[] mData;

        public BitSet()
        {

        }
        ~BitSet()
        {
            Cleanup();
        }
        public void Cleanup()
        {
            if(mData != null)
                mData = null;
            mBitCount = 0;
        }

        public void Init(int bitCount)
        {
            Cleanup();
            int buffSize = bitCount/8 + 1;
            mData = new byte[buffSize];
            mBitCount = bitCount;
        }

        public void SetBit(int iBit, bool value)
        {
            if (iBit < 0 || iBit >= mBitCount)
            {
                Log.Log.Common.Print("Error! SetBit {0}/{1}", iBit, mBitCount);
                return;
            }

            int master = iBit/8;
            int slave = iBit%8;
            if(value)
                mData[master] = (byte)(mData[master] | (1<<slave));
            else
                mData[master] = (byte)(mData[master] & (~(1<<slave)));
        }

        public bool IsBit(int iBit)
        {
            if (iBit < 0 || iBit >= mBitCount)
            {
                Log.Log.Common.Print("Error! IsBit {0}/{1}", iBit, mBitCount);
                return true;
            }

            int master = iBit/8;
            int slave = iBit%8;

            return ((mData[master] & (1<<slave)) != 0) ? true : false;
        }

        public byte[] ToBytes()
        {
            return mData;
        }

        public void FromBytes(byte[] buffer)
        {
            mData = new System.Byte[buffer.Length];
            Buffer.BlockCopy(buffer,0,mData,0,buffer.Length);
        }

        //public string ToBase64String()
        //{
        //    return System.Convert.ToBase64String(mData);
        //}

        //public bool FromBase64String(string str, int bitCount)
        //{
        //    int buffSize = bitCount / 8 + 1;
        //    byte[] temp = System.Convert.FromBase64String(str);
        //    if (buffSize < temp.Length)
        //        return false;

        //    mData = new byte[buffSize];
        //    temp.CopyTo(mData, 0);
        //    mBitCount = bitCount;
        //    return true;
        //}
    }
}
