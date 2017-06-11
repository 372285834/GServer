using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCommon.Data
{
    public enum eGameAttributeOp
    {
        NONE,
        BASE,
        BASEADD,
        BASEPER,
        BASEDELTA,
        PER,
        DELTA,
        FINALPER
    };

    /// <summary>
    /// 通用属性结构
    /// </summary>
    public class GameAttribute
    {
        int mBase;
        int mBaseAdd;
        float mBasePer;
        int mBaseDelta;
        float mPer;
        int mDelta;
        float mFinalPer;
        int m_value;
        int mOld;
        bool mDirty;
        
        /// <summary>
        /// 取得最终属性
        /// </summary>
        public int Value
        {
            get
            {
                if (mDirty)
                {
                    mDirty = false;
                    m_value = (int)((((mBase + mBaseAdd) * (1 + mBasePer) + mBaseDelta) * (1 + mPer) + mDelta) * (1 + mFinalPer));
                }
                return m_value;
            }
        }

        public void DoAttributeOp(eGameAttributeOp op, int value)
        {
            switch (op)
            {
                case eGameAttributeOp.BASE:
                    Base = value;
                    break;
                case eGameAttributeOp.BASEADD:
                    AddBaseAdd(value);
                    break;
                case eGameAttributeOp.BASEDELTA:
                    AddBaseDelta(value);
                    break;
                case eGameAttributeOp.DELTA:
                    AddDelta(value);
                    break;
            }
        }

	    /// <summary>
	    /// 设置属性的基础数值
	    /// </summary>
	    public int Base
	    {
            get
            {
                return mBase;
            }
            set
            {
                mBase = value;
                mDirty = true;
            }
	    }

	    public void AddBase( int Add ) 
	    {
		    mBase += Add;
		    if (mBase < 0 )
		    {
			    mBase = 0 ;
		    }
		    mDirty = true;
	    }

	    /// <summary>
        /// 增加属性的基础数值增量
	    /// </summary>
	    public void AddBaseAdd( int Add ) 
	    {
            mBaseAdd += Add;
            mDirty = true;
        }

	    public void ReSetBaseAdd() 
	    {
		    mBaseAdd = 0;
		    mDirty = true;
	    }

	    /// <summary>
        /// 增加属性的基础数值百分比
	    /// </summary>
        public void AddBasePer( float per ) 
	    {
            mBasePer += per;
            mDirty = true;
        }

        public void SetBasePer( float per )
        {
            mBasePer = per;
            mDirty = true;
        }

	    /// <summary>
        /// 增加属性的基础数值附加值
	    /// </summary>
        public void AddBaseDelta( int delta )
	    {
            mBaseDelta += delta;
            mDirty = true;
        }

	    public void SetBaseDelta( int delta )
	    {
		    mBaseDelta = delta;
		    mDirty = true;
	    }

	    /// <summary>
        /// 增加属性的百分比
	    /// </summary>
        public void AddPer( float per )
	    {
            mPer += per;
            mDirty = true;
        }

	    /// <summary>
        /// 增加属性的附加值
	    /// </summary>
        public void AddDelta( int delta )
	    {
            mDelta += delta;
            mDirty = true;
        }

	    /// <summary>
        /// 设置属性的最终百分比
	    /// </summary>
        public void SetFinalPercent( float per )
	    {
            mFinalPer = per;
            mDirty = true;
        }

	    public bool isChanged() 
	    {
		    return mOld != Value;
	    }

        public void reset()
        {
            mOld = Value;
            mBase = 0;
            mBaseAdd = 0;
            mBasePer = 0;
            mBaseDelta = 0;
            mPer = 0;
            mDelta = 0;
            mFinalPer = 0;
            mDirty = true;
        }
    }
}
