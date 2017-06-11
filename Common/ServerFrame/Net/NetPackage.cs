using System;
using System.Collections.Generic;

using System.Text;
using System.Runtime.InteropServices;

namespace RPC
{
    public enum PackageType
    {
        PKGT_Send,
        PKGT_SendAndWait,
        PKGT_Return,
        PKGT_C2P_Send,
        PKGT_C2P_SendAndWait,
        PKGT_C2P_Return,
        PKGT_P2C_Send,
        PKGT_C2P_Player_Send,
        PKGT_C2P_Player_SendAndWait,
    };


    [System.Serializable]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 4)]
    unsafe public struct RPCHeader
    {
        enum eHeadParam
        {
            MaxStack = 4,
            MaxStackSize = (MaxStack % 2) == 0 ? (MaxStack / 2) : (MaxStack / 2 + 1),
            StackUnused = 0x0F,//没用到的
            StackIndexBegin = 11,//大于11，小于16的属于索引器
            SinglePkg = (1 << 7),
            PackageSendMask = 0x0F,
        };

        public void ToDefault()
        {
            PKGType = (byte)PackageType.PKGT_Send;
            SerialId = 0xFFFF;
            Methord = 0xFF;
            unsafe
            {
                fixed (byte* pPtr = Stacks)
                {
                    for (int i = 0; i < (int)eHeadParam.MaxStackSize; i++)
                        pPtr[i] = 0xFF;
                }
            }
        }

        //public ushort head_length;//小兰的通讯层需要，标志整个Package的大小

        byte PKGType;
        byte Methord;
        ushort SerialId;

        fixed byte Stacks[(int)eHeadParam.MaxStackSize];
        //byte Stack0;
        //byte Stack1;

        public static int SizeOf()
        {
            return Marshal.SizeOf(typeof(RPCHeader));            
        }


        public void SetSinglePkg()
        {
            PKGType |= (byte)eHeadParam.SinglePkg;//比如给客户端的时候，有些AutoSave对象传递的数据可以不完整，设置这个后，只传没有被标志FieldDontAutoSingleSaveLoadAttribute的域
        }
        public void UnSetSinglePkg()
        {//这个不是需要reset包的时候不要掉
            unchecked
            {
                PKGType &= (byte)(~eHeadParam.SinglePkg);
            }
        }
        public bool IsSinglePkg()
        {
            if ((PKGType & (byte)eHeadParam.SinglePkg) > 0)
                return true;
            else
                return false;
        }
        public PackageType GetPackageType()
        {
            return (PackageType)(PKGType & (byte)eHeadParam.PackageSendMask);
        }
        public void SetPackageType(PackageType t)
        {
            unchecked
            {
                PKGType = (byte)((PKGType & (byte)(~eHeadParam.PackageSendMask)) | ((byte)t & (byte)eHeadParam.PackageSendMask));
            }
        }
        public byte GetMethod()
        {
            return Methord;
        }
        public void SetMethod(byte v)
        {
            Methord = v;
        }
        public ushort GetSerialId()
        {
            return SerialId;
        }
        public void SetSerialId(ushort v)
        {
            SerialId = v;
        }
        public byte GetStack(short Index)
        {
            if (Index >= (short)eHeadParam.MaxStack)
                return (byte)eHeadParam.StackUnused;
            short remainder = (short)(Index % 2);

            unsafe
            {
                fixed (byte* pPtr = Stacks)
                {
                    if (remainder == 0)
                        return (byte)(pPtr[Index / 2] & 0x0F);
                    else
                        return (byte)((pPtr[Index / 2] >> 4) & 0xF);
                }
            }
        }
        public void SetStack(short Index, byte v)
        {
            if ((v & 0xF0)>0)
                return;//这里系统应该崩溃
            short remainder = (short)(Index % 2);

            unsafe
            {
                fixed (byte* pPtr = Stacks)
                {
                    if (remainder == 0)
                        pPtr[Index / 2] = (byte)(v | (pPtr[Index / 2] & 0xF0));
                    else
                        pPtr[Index / 2] = (byte)((v << 4) | (pPtr[Index / 2] & 0x0F));
                }
            }
        }
    };
}
