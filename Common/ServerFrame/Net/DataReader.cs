using SlimDX;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RPC
{
    public class DataReader
    {
        public byte[] mHandle;
        public int mPos;
        protected int mSize;

        public DataReader(Byte[] addr)
        {
            mHandle = addr;
            mPos = 0;
            mSize = addr.Length;
        }

        public DataReader(Byte[] addr,int startIndex, int size, int remain)
        {
            Proxy(addr, startIndex, size, remain);
        }

        public void Cleanup()
        {
            mHandle = null;
            mPos = 0;
        }

        void OnReadError()
        {

        }


        int DataPtr()
        {
            return 0;
        }
        int CurPtr()
        {
            return mPos;
        }

        public int Length
        {
            get { return mSize; }
        }

        public void Proxy(byte[] addr,int startIndex,int size,int remain)
        {
            Cleanup();
            if (size > remain)
            {
                return;
            }            
            mHandle = new byte[size];
            Buffer.BlockCopy(addr, startIndex, mHandle, 0, size);
            mPos = 0;
            mSize = size;
        }

        //void DeepCopy(byte[] addr)
        //{
        //    mHandle = new byte[addr.Length + 4];
        //    Buffer.BlockCopy(addr, 0, mHandle, 0, addr.Length);
        //    mPos = 0;
        //    mIsDeepCopy = true;
        //}

        #region Data Reader
        public void Read( IAutoSaveAndLoad result ,bool bToClient )
	    {
		    result.DataRead(this,bToClient);
	    }

	    public void Read(out DataReader data)
	    {
		    System.UInt16 size;
		    Read(out size);

            if (CurPtr() + size > mSize)
            {
                data = null;
                OnReadError();
                return;
            }

            data = new DataReader(mHandle,mPos, size, mSize - mPos);
            mPos += size;
            //unsafe
            //{
            //    int pos = IDllImportAPI.Net_StreamReader_Tell(mHandle.ToPointer());
            //    IntPtr ptr = (IntPtr)IDllImportAPI.Net_StreamReader_CurPtr(mHandle.ToPointer(), (int)size);
            //    if (ptr!=(IntPtr)0)
            //        data = new DataReader(ptr, (int)size, mSize - pos);
            //    else
            //        data = new DataReader(ptr, 0, 0);
            //}

	    }
        public void Read(out string data)
        {
            ushort length;
            Read(out length);
            if (length == 0)
            {
                data = "";
                return;
            }



            int buffSize = sizeof(char) * length;
            if (CurPtr() + buffSize > mSize)
            {
                data = "";
                OnReadError();
                return;
            }
            char[] chars = new char[length];
            unsafe
            {
                fixed (char* pValue = chars)
                {
                    Marshal.Copy(mHandle, mPos, (IntPtr)pValue, buffSize);
                }
            }
            mPos += buffSize;
            data = new string(chars);
            
        }

        public void Read(out System.Char data)
        {
            int length = Marshal.SizeOf(typeof(System.Char));

            if (CurPtr() + length > mSize)
            {
                data = '0';
                return;
            }

            unsafe
            {
                fixed (System.Char* pValue = &data)
                {
                    Marshal.Copy(mHandle, mPos, (IntPtr)pValue, length);
                }
            }
            mPos += length;
        }

        public void Read(out System.Single data)
        {
            int length = Marshal.SizeOf(typeof(System.Single));
            if (CurPtr() + length > mSize)
            {
                data = 0;
                OnReadError();
                return;
            }
            unsafe
            {
                fixed (System.Single* pValue = &data)
                {
                    Marshal.Copy(mHandle, mPos, (IntPtr)pValue, length);
                }
            }
            mPos += length;
        }

        public void Read(out System.Double data)
        {
            int length = Marshal.SizeOf(typeof(System.Double));
            if (CurPtr() + length > mSize)
            {
                data = 0;
                OnReadError();
                return;
            }
            unsafe
            {
                fixed (System.Double* pValue = &data)
                {
                    Marshal.Copy(mHandle, mPos, (IntPtr)pValue, length);
                }
            }
            mPos += length;
        }

        public void Read(out System.SByte data)
        {
            int length = Marshal.SizeOf(typeof(System.SByte));
            if (CurPtr() + length > mSize)
            {
                data = 0;
                OnReadError();
                return;
            }
            unsafe
            {
                fixed (System.SByte* pValue = &data)
                {
                    Marshal.Copy(mHandle, mPos, (IntPtr)pValue, length);
                }
            }
            mPos += length;
        }

        public void Read(out System.Int16 data)
        {
            int length = Marshal.SizeOf(typeof(System.Int16));
            if (CurPtr() + length > mSize)
            {
                data = 0;
                OnReadError();
                return;
            }
            unsafe
            {
                fixed (System.Int16* pValue = &data)
                {
                    Marshal.Copy(mHandle, mPos, (IntPtr)pValue, length);
                }
            }
            mPos += length;
        }

        public void Read(out System.Int32 data)
        {
            int length = Marshal.SizeOf(typeof(System.Int32));
            if (CurPtr() + length > mSize)
            {
                data = 0;
                OnReadError();
                return;
            }
            unsafe
            {
                fixed (System.Int32* pValue = &data)
                {
                    Marshal.Copy(mHandle, mPos, (IntPtr)pValue, length);
                }
            }
            mPos += length;
        }

        public void Read(out System.Int64 data)
        {
            int length = Marshal.SizeOf(typeof(System.Int64));
            if (CurPtr() + length > mSize)
            {
                data = 0;
                OnReadError();
                return;
            }
            unsafe
            {
                fixed (System.Int64* pValue = &data)
                {
                    Marshal.Copy(mHandle, mPos, (IntPtr)pValue, length);
                }
            }
            mPos += length;
        }

        public void Read(out System.Byte data)
        {
            int length = Marshal.SizeOf(typeof(System.Byte));
            if (CurPtr() + length > mSize)
            {
                data = 0;
                OnReadError();
                return;
            }
            unsafe
            {
                fixed (System.Byte* pValue = &data)
                {
                    Marshal.Copy(mHandle, mPos, (IntPtr)pValue, length);
                }
            }
            mPos += length;
        }

        public void Read(out System.UInt16 data)
        {
            int length = Marshal.SizeOf(typeof(System.UInt16));
            if (CurPtr() + length > mSize)
            {
                data = 0;
                OnReadError();
                return;
            }
            unsafe
            {
                fixed (System.UInt16* pValue = &data)
                {
                    Marshal.Copy(mHandle, mPos, (IntPtr)pValue, length);
                }
            }
            mPos += length;
        }

        public void Read(out System.UInt32 data)
        {
            int length = Marshal.SizeOf(typeof(System.UInt32));
            if (CurPtr() + length > mSize)
            {
                data = 0;
                OnReadError();
                return;
            }
            unsafe
            {
                fixed (System.UInt32* pValue = &data)
                {
                    Marshal.Copy(mHandle, mPos, (IntPtr)pValue, length);
                }
            }
            mPos += length;
        }

        public void Read(out System.UInt64 data)
        {
            int length = Marshal.SizeOf(typeof(System.UInt64));
            if (CurPtr() + length > mSize)
            {
                data = 0;
                OnReadError();
                return;
            }
            unsafe
            {
                fixed (System.UInt64* pValue = &data)
                {
                    Marshal.Copy(mHandle, mPos, (IntPtr)pValue, length);
                }
            }
            mPos += length;
        }

        public void Read(out System.Guid data)
        {
            int length = Marshal.SizeOf(typeof(System.Guid));
            if (CurPtr() + length > mSize)
            {
                data = Guid.Empty;
                OnReadError();
                return;
            }

            unsafe
            {
                fixed (Guid* pValue = &data)
                {
                    Marshal.Copy(mHandle, mPos, (IntPtr)pValue, length);
                }
            }
            mPos += length;
        }

        public void Read(out System.DateTime data)
        {
            long bin = BitConverter.ToInt64(mHandle, CurPtr());
            if (CurPtr() + sizeof(long) > mSize)
            {
                data = System.DateTime.MinValue;
                OnReadError();
                return;
            }

            mPos += sizeof(long);
            data = System.DateTime.FromBinary(bin);
        }

        public void Read(out SlimDX.Vector2 data)
        {
            int length = Marshal.SizeOf(typeof(SlimDX.Vector2));
            if (CurPtr() + length > mSize)
            {
                data = SlimDX.Vector2.Zero;
                OnReadError();
                return;
            }

            unsafe
            {
                fixed (SlimDX.Vector2* pValue = &data)
                {
                    Marshal.Copy(mHandle, mPos, (IntPtr)pValue, length);
                }
            }
            mPos += length;
        }

        public void Read(out SlimDX.Vector3 data)
        {
            int length = Marshal.SizeOf(typeof(SlimDX.Vector3));
            if (CurPtr() + length > mSize)
            {
                data = SlimDX.Vector3.Zero;
                OnReadError();
                return;
            }

            unsafe
            {
                fixed (SlimDX.Vector3* pValue = &data)
                {
                    Marshal.Copy(mHandle, mPos, (IntPtr)pValue, length);
                }
            }
            mPos += length;
        }

        public void Read(out SlimDX.Vector4 data)
        {
            int length = Marshal.SizeOf(typeof(SlimDX.Vector4));
            if (CurPtr() + length > mSize)
            {
                data = new SlimDX.Vector4();
                OnReadError();
                return;
            }

            unsafe
            {
                fixed (SlimDX.Vector4* pValue = &data)
                {
                    Marshal.Copy(mHandle, mPos, (IntPtr)pValue, length);
                }
            }
            mPos += length;
        }

        public void Read(out SlimDX.Quaternion data)
        {
            int length = Marshal.SizeOf(typeof(SlimDX.Quaternion));
            if (CurPtr() + length > mSize)
            {
                data = SlimDX.Quaternion.Identity;
                OnReadError();
                return;
            }

            unsafe
            {
                fixed (SlimDX.Quaternion* pValue = &data)
                {
                    Marshal.Copy(mHandle, mPos, (IntPtr)pValue, length);
                }
            }
            mPos += length;
        }

        public void Read(out SlimDX.Matrix data)
        {
            int length = Marshal.SizeOf(typeof(SlimDX.Matrix));
            if (CurPtr() + length > mSize)
            {
                data = SlimDX.Matrix.Identity;
                OnReadError();
                return;
            }

            unsafe
            {
                fixed (SlimDX.Matrix* pValue = &data)
                {
                    Marshal.Copy(mHandle, mPos, (IntPtr)pValue, length);
                }
            }
            mPos += length;

        }

        public void Read(out byte[] data)
        {
            ushort length;
            Read(out length);
            data = new byte[length];
            if (length == 0)
                return;
            Buffer.BlockCopy(mHandle, CurPtr(), data, 0, length);
            mPos += length;
        }

        public DataReader ReadDataReader()
        {
            DataReader dr;
            Read(out dr);
            return dr;
        }
        public string ReadString()
        {
            string ret;
            Read(out ret);
            return ret;
        }
        public System.Char ReadChar()
        {
            System.Char ret;
            Read(out ret);
            return ret;
        }
        public System.Single ReadSingle()
        {
            System.Single ret;
            Read(out ret);
            return ret;
        }
        public Double ReadDouble()
        {
            System.Double ret;
            Read(out ret);
            return ret;
        }
        public SByte ReadSByte()
        {
            System.SByte ret;
            Read(out ret);
            return ret;
        }
        public Int16 ReadInt16()
        {
            System.Int16 ret;
            Read(out ret);
            return ret;
        }
        public Int32 ReadInt32()
        {
            System.Int32 ret;
            Read(out ret);
            return ret;
        }
        public Int64 ReadInt64()
        {
            System.Int64 ret;
            Read(out ret);
            return ret;
        }
        public Byte ReadByte()
        {
            System.Byte ret;
            Read(out ret);
            return ret;
        }
        public UInt16 ReadUInt16()
        {
            System.UInt16 ret;
            Read(out ret);
            return ret;
        }
        public UInt32 ReadUInt32()
        {
            System.UInt32 ret;
            Read(out ret);
            return ret;
        }
        public UInt64 ReadUInt64()
        {
            System.UInt64 ret;
            Read(out ret);
            return ret;
        }
        public Guid ReadGuid()
        {
            System.Guid ret;
            Read(out ret);
            return ret;
        }
        public DateTime ReadDateTime()
        {
            System.DateTime ret;
            Read(out ret);
            return ret;
        }
        public Vector2 ReadVector2()
        {
            Vector2 ret;
            Read(out ret);
            return ret;
        }
        public Vector3 ReadVector3()
        {
            Vector3 ret;
            Read(out ret);
            return ret;
        }
        public Vector4 ReadVector4()
        {
            Vector4 ret;
            Read(out ret);
            return ret;
        }
        public Quaternion ReadQuaternion()
        {
            Quaternion ret;
            Read(out ret);
            return ret;
        }
        public Matrix ReadMatrix4x4()
        {
            Matrix ret;
            Read(out ret);
            return ret;
        }
        public byte[] ReadByteArray()
        {
            byte[] ret;
            Read(out ret);
            return ret;
        }

        #endregion
    }
}
