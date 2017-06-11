using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCommon.Data
{

    [ServerFrame.DB.DBBindTable("ItemData")]//定义物品实例数据库表
    [System.ComponentModel.TypeConverterAttribute("System.ComponentModel.ExpandableObjectConverter")]
    public class ItemData : RPC.IAutoSaveAndLoad
    {
        #region 随机数
        [System.Runtime.InteropServices.DllImport("winmm.dll")]
        extern static System.Int32 timeGetTime();
        static System.Random mRand = new System.Random(timeGetTime());
        [RPC.AutoSaveLoad]
        public static System.Random Rand
        {
            get { return mRand; }
        }

        #endregion

        CSTable.IItemBase mTemplate;
        [System.ComponentModel.Browsable(false)]
        public CSTable.IItemBase Template
        {
            get { return mTemplate; }
        }

        System.DateTime mCreateTime;
        [ServerFrame.DB.DBBindField("CreateTime")]
        [RPC.AutoSaveLoad]
        public System.DateTime CreateTime
        {
            get { return mCreateTime; }
            set { mCreateTime = value; }
        }

        #region 核心数据 物品一般属性
        //------------------------------------------------------------------------------对应物品模板ID
        int mItemTemlateId;
        [ServerFrame.DB.DBBindField("ItemTemlateId")]
        [RPC.AutoSaveLoad(true)]
        public int ItemTemlateId
        {
            get
            {
                if (mTemplate == null)
                    return mItemTemlateId;
                return mTemplate.id;
            }
            set
            {
                mItemTemlateId = value;
                mTemplate = CSTable.ItemUtil.GetItem(value);
            }
        }

        //---------------------------------------------------------------------------------物品实例ID
        ulong mItemId;
        [ServerFrame.DB.DBBindField("ItemId")]
        [RPC.AutoSaveLoad(true)]
        public ulong ItemId
        {
            get { return mItemId; }
            set { mItemId = value; }
        }

        //-----------------------------------------------------------------------------------所有者实例ID，可能是Role,也可能是佣兵团，家族，位面
        ulong mOwnerId;
        [ServerFrame.DB.DBBindField("OwnerId")]
        [RPC.AutoSaveLoad(true)]
        public ulong OwnerId
        {
            get { return mOwnerId; }
            set { mOwnerId = value; }
        }

        //---------------------------------------------------------------------------------物品当前保质期 
        int mItemShelflife = 0;
        [ServerFrame.DB.DBBindField("ItemShelflife")]
        [RPC.AutoSaveLoad(true)]
        public int ItemShelflife
        {
            get { return mItemShelflife; }
            set { mItemShelflife = value; }
        }

        //-----------------------------------------------------------------------------物品强化等级
        Int16 mItemLv = 1;
        [ServerFrame.DB.DBBindField("ItemLv")]
        [RPC.AutoSaveLoad(true)]
        public Int16 ItemLv
        {
            get { return mItemLv; }
            set { mItemLv = value; }
        }
        //----------------------------------------------------------------------------------------当前经验
        int mCurExp = 0;
        [ServerFrame.DB.DBBindField("CurExp")]
        [RPC.AutoSaveLoad(true)]
        public int CurExp
        {
            get { return mCurExp; }
            set { mCurExp = value; }
        }
        //-----------------------------------------------------------------------------物品绑定状态
        byte mItemBindState;
        [ServerFrame.DB.DBBindField("ItemBindState")]
        [RPC.AutoSaveLoad(true)]
        public byte ItemBindState
        {
            get { return mItemBindState; }
            set { mItemBindState = value; }
        }

        //--------------------------------------------------------------------------------------所在背包
        byte mInventory;
        [ServerFrame.DB.DBBindField("Inventory")]
        [RPC.AutoSaveLoad(true)]
        public byte Inventory
        {
            get { return mInventory; }
            set { mInventory = value; }
        }

        //----------------------------------------------------------------------------------------
        int mStackNum = 1;
        [ServerFrame.DB.DBBindField("StackNum")]
        [RPC.AutoSaveLoad(true)]
        public int StackNum
        {
            get { return mStackNum; }
            set { mStackNum = value; }
        }
        //----------------------------------------------------------------------------------------是否装备
        byte mWearState;
        [ServerFrame.DB.DBBindField("WearState")]
        [RPC.AutoSaveLoad(true)]
        public byte WearState
        {
            get { return mWearState; }
            set { mWearState = value; }
        }
        //-----------------------------------------------------------------------------物品精炼等级
        Int16 mItemRefineLv;
        [ServerFrame.DB.DBBindField("ItemRefineLv")]
        [RPC.AutoSaveLoad(true)]
        public Int16 ItemRefineLv
        {
            get { return mItemRefineLv; }
            set { mItemRefineLv = value; }
        }
        //----------------------------------------------------------------------------------------
        UInt16 mPosition;
        [ServerFrame.DB.DBBindField("Position")]
        [RPC.AutoSaveLoad(true)]
        public UInt16 Position
        {
            get { return mPosition; }
            set { mPosition = value; }
        }
        #endregion
    }
}
