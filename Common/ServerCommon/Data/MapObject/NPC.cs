using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using ServerCommon.Planes;

namespace CSCommon.Data
{
    public class RoleDataBase : RPC.IAutoSaveAndLoad
    {
        #region 属性改变通知
        protected void OnPropertyChanged(string propertyName)
        {
            SendPackage(propertyName);
        }

        IStateHost mHostNpc;

        public void _SetHostNpc(IStateHost npc)
        {
            mHostNpc = npc;
        }

        protected void SendPackage(string proName)
        {
            if (mHostNpc == null)
                return;
            var proInfo = this.GetType().GetProperty(proName);

            RPC.PackageWriter pkg = new RPC.PackageWriter();

            RPC.DataWriter dw = new RPC.DataWriter();
            if (proInfo.PropertyType == typeof(Byte))
                dw.Write(System.Convert.ToByte(proInfo.GetValue(this, null)));
            else if (proInfo.PropertyType == typeof(UInt16))
                dw.Write(System.Convert.ToUInt16(proInfo.GetValue(this, null)));
            else if (proInfo.PropertyType == typeof(UInt32))
                dw.Write(System.Convert.ToUInt32(proInfo.GetValue(this, null)));
            else if (proInfo.PropertyType == typeof(UInt64))
                dw.Write(System.Convert.ToUInt64(proInfo.GetValue(this, null)));
            else if (proInfo.PropertyType == typeof(SByte))
                dw.Write(System.Convert.ToSByte(proInfo.GetValue(this, null)));
            else if (proInfo.PropertyType == typeof(Int16))
                dw.Write(System.Convert.ToInt16(proInfo.GetValue(this, null)));
            else if (proInfo.PropertyType == typeof(Int32))
                dw.Write(System.Convert.ToInt32(proInfo.GetValue(this, null)));
            else if (proInfo.PropertyType == typeof(Int64))
                dw.Write(System.Convert.ToInt64(proInfo.GetValue(this, null)));
            else if (proInfo.PropertyType == typeof(float))
                dw.Write(System.Convert.ToSingle(proInfo.GetValue(this, null)));
            else if (proInfo.PropertyType == typeof(double))
                dw.Write(System.Convert.ToDouble(proInfo.GetValue(this, null)));
            else if (proInfo.PropertyType == typeof(System.Guid))
                dw.Write(System.Guid.Parse(proInfo.GetValue(this, null).ToString()));
            else
                return;

            mHostNpc.OnValueChanged(proName, dw);
        }
        #endregion

        CSTable.NPCTplData mTemplate = null;
        public CSTable.NPCTplData Template
        {
            get { return mTemplate; }
        }

        int mTemplateId;
        [RPC.AutoSaveLoad(true)]
        public int TemplateId
        {
            get { return mTemplateId; } 
            set
            {
                mTemplateId = value;
                mTemplate = CSTable.StaticDataManager.NPCTpl[value];
            }
        }

        [RPC.AutoSaveLoad(true)]
        public string RoleName
        {
            get;
            set;
        }


        ulong mRoleId;
        [RPC.AutoSaveLoad(true)]
        public ulong RoleId
        {
            get { return mRoleId; }
            set { mRoleId = value; }
        }

        byte mActionId;
        [RPC.AutoSaveLoad(true)]
        public byte ActionId
        {
            get { return mActionId; }
            set { mActionId = value; }
        }

        SlimDX.Vector3 mPosition;
        [RPC.AutoSaveLoad(true)]
        public virtual SlimDX.Vector3 Position
        {
            get { return mPosition; }
            set
            {
                mPosition = value;
            }
        }

        // NPC朝向(弧度)
        float mDirection = 0;
        [RPC.AutoSaveLoad(true)]
        public virtual float Direction
        {
            get { return mDirection; }
            set
            {
                if (System.Math.Abs(mDirection - value) < 0.00001f)
                    return;

                mDirection = value;

                //mDirectionAngle = (float)(value / System.Math.PI * 180.0f);

                //if (OnDirectionChanged != null)
                //    OnDirectionChanged(mDirection);

            }
        }

        float mScale = 1;
        [RPC.AutoSaveLoad(true)]
        public virtual float Scale
        {
            get { return mScale; }
            set
            {
                if (System.Math.Abs(mScale - value) < 0.00001f)
                    return;

                mScale = value;

                //if (OnScaleChanged != null)
                //    OnScaleChanged(mScale);

            }
        }

        int mRoleHP = 1000;
        /// <summary>
        /// 当前血量
        /// </summary>
        [RPC.AutoSaveLoad(true)]
        public int RoleHp
        {
            get { return mRoleHP; }
            set
            {
                mRoleHP = value;
                OnPropertyChanged("RoleHp");
            }
        }

        int mRoleMaxHP = 1000;
        /// <summary>
        /// 当前血量
        /// </summary>
        [RPC.AutoSaveLoad(true)]
        public int RoleMaxHp
        {
            get { return mRoleMaxHP; }
            set
            {
                mRoleMaxHP = value;
                OnPropertyChanged("RoleMaxHp");
            }
        }

        byte mCamp = 0;
        /// <summary>
        /// 阵营
        /// </summary>
        [RPC.AutoSaveLoad(true)]
        public byte Camp
        {
            get { return mCamp; }
            set{mCamp = value;}
        }

    }

    public class NPCData : RoleDataBase
    {
        float mSpeed = 0;
        /// <summary>
        /// 移动速度
        /// </summary>
        [RPC.AutoSaveLoad(true)]
        public float Speed
        {
            get { return mSpeed; }
            set { mSpeed = value; }
        }
    }

    public class GatherData : RoleDataBase
    {
        UInt16 mDropper;
        [ServerFrame.Support.AutoSaveLoadAttribute]
        public UInt16 Dropper
        {
            get { return mDropper; }
            set { mDropper = value; }
        }

        int mRebornTime = 10000;
        [ServerFrame.Support.AutoCopyAttribute]
        [ServerFrame.Support.AutoSaveLoadAttribute]
        public int RebornTime
        {
            get { return mRebornTime; }
            set { mRebornTime = value; }
        }
    }
}
