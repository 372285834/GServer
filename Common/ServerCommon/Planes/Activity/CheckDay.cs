using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes.Activity
{
    public class CheckDay
    {
        public const int MaxCheckDay = 32;
        public static bool IsCheckedDay(int day,Int64 CheckData)
        {
            Int64 dvalue = ((Int64)1 << day);
            return ((CheckData & dvalue)!=0) ? true : false;
        }
        public static Int64 SetDay(int day, Int64 CheckData)
        {
            CheckData |= ((Int64)1 << day);
            return CheckData;
        }
        public static Int64 UnSetDay(int day, Int64 CheckData)
        {
            CheckData = CheckData & (~((Int64)1 << day));
            return CheckData;
        }
        public static byte GetMonth(Int64 CheckData)
        {
            return (byte)(CheckData >> MaxCheckDay);
        }
        public static Int64 SetMonth(byte month,Int64 CheckData) 
        {
            CheckData = (CheckData << 8) >> 8;
            CheckData |= (((Int64)month) << MaxCheckDay);
            return CheckData;
        }

        public static bool IsKeepCheck(int keepDay,Int64 CheckData)
        {
            bool isKeppCheck = false;
            for (int i = 0; i < keepDay; i++)
            {
                if (IsCheckedDay(i,CheckData)==false)
                {
                    isKeppCheck = false;
                }
            }
            return isKeppCheck;
        }

        
            //CheckData = (CheckData << 8) >> 8;
            //CheckData |= (((Int64)month) << MaxCheckDay);
            //return CheckData;

        public static Int64 SetIsGetContinueGift(int flag, Int64 CheckData)
        {
            CheckData |= ((Int64)1 << flag);
            return CheckData;
        }
        public static bool GetIsGetGift(int flag, Int64 CheckData)
        {
            Int64 dvalue = ((Int64)1 << flag);
            return ((CheckData & dvalue) != 0) ? true : false;
        }
        public static Int64 ResetISGetContinueGift(int flag, Int64 CheckData)
        {
            Int64 dvalue = ((Int64)0 << flag);
            CheckData = CheckData & dvalue;
            return CheckData;
        }

    }
    
}
