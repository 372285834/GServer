using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSCommon;

namespace ServerCommon.Planes
{
    public class PlayerStatus
    {
        //所中buff状态列表
        bool[] mBuffStatus = new bool[(int)eBuffStatusType.MAX];
        //免疫buff列表
        bool[] mImmunityList = new bool[(int)eImmunityType.MAX];

        public void CleanUp()
        {
            for (var i = eBuffStatusType.None; i < eBuffStatusType.MAX; i++)
            {
                mBuffStatus[(int)i] = false;
            }
            for (var i = eImmunityType.All; i < eImmunityType.MAX; i++)
            {
                mImmunityList[(int)i] = false;
            }
        }

	    public bool IsHasBuffStatus( eBuffStatusType type )
        {
            if ( type <= eBuffStatusType.None || type >= eBuffStatusType.MAX )
	        {
		        return false;
	        }
	        return mBuffStatus[(int)type];
        }

        public bool AddStatusType( int type )
        {
            if (type <= (int)eBuffStatusType.None || type >= (int)eBuffStatusType.MAX)
            {
                return false;
            }

	        mBuffStatus[type] = true;

            return true;
        }

        public bool BuffAddStatusType(CSTable.BuffData data, RoleActor owner)
        {
            if (null == data || null == owner)
                return false;

            if (!AddStatusType(data.type))
                return false;

            //对特殊buff状态的处理

            return true;
        }

        public bool DelStatusType( int type )
        {
            if (type <= (int)eBuffStatusType.None || type >= (int)eBuffStatusType.MAX)
            {
                return false;
            }

            mBuffStatus[type] = false;

            return true;
        }

	    public bool IsCanJump()
        {
            if (mBuffStatus[(int)eBuffStatusType.眩晕])
                return false;

            return true;
        }

	    public bool IsCanMove()
        {
            if (mBuffStatus[(int)eBuffStatusType.眩晕])
                return false;

            return true;
        }

        public bool IsImmunityBuffStatus( int status )
        {
	        if ( status < (int)eImmunityType.All || status >= (int)eImmunityType.MAX )
		        return false;
	        
            if ( mImmunityList[(int)eImmunityType.All] )
		        return true;
	        
            return mImmunityList[(int)status];
        }
        
        public bool IsAllImmunityBuff()
        {
	        return mImmunityList[(int)eImmunityType.All];
        }

        public bool IsHasImmunityType( int type )
        {
            if (type < (int)eImmunityType.All || type >= (int)eImmunityType.MAX)
		        return false;
	        
            return mImmunityList[type];
        }

        public bool AddImmunityType( int type )
        {
	        if (type < (int)eImmunityType.All || type >= (int)eImmunityType.MAX)
		        return false;
	        
            mImmunityList[type] = true;
	        
            return true;
        }

        public bool DelmmunityType( int type )
        {
            if (type < (int)eImmunityType.All || type >= (int)eImmunityType.MAX)
                return false;

	        mImmunityList[type] = false;

	        return true;
        }
    }
}
