using ServerFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{

    public class BuildOutPut
    {
        public byte mType;//产出物品类型 CSCommon.eMartialItemType SQL
        public System.DateTime mStartTime;//本次开始时间 SQL
        public byte mOutCount = 0;//产出次数 SQL
        public sbyte mUpOutIndex = -1; //哪种加成 SQL
        public System.DateTime mUpStartTime;//本次加成开始时间 SQL
        public int mOutPutTotalNum = 0;//总数
        public object mTimerHandler = null;
        public BuildOutPut(byte _type)
        {
            mType = _type;
        }
        public float mRemainUpHour = 0;
        public float mPer = 0;
        public bool mIsInit = false;
        int mTimeArg = 3600;
        //获取本次剩余时间
        public float GetPer()
        {
            if (mIsInit)
            {
                return mPer * mTimeArg;
            }
            return CSCommon.MartialClubCommon.Instance.OutPutPer * mTimeArg;
        }
        public byte GetPerCount(float hour)
        {
            byte count = (byte)(hour / CSCommon.MartialClubCommon.Instance.OutPutPer);
            return count;
        }

        //获取累计最大次数
        public byte GetMaxTime()
        {
            return CSCommon.MartialClubCommon.Instance.OutPutMaxTime;
        }
        //获取本次提升收益倍数
        public float GetUpRate(System.DateTime _time)
        {
            System.DateTime mUpEndTime = System.DateTime.MinValue;
            float uprate = 1.0f;
            if (mUpOutIndex >= 0)
            {
                var up = CSCommon.MartialClubCommon.Instance.UpGains;
                if (mUpOutIndex < up.Count)
                {
                    var upgain = up[mUpOutIndex];
                    mUpEndTime = mUpStartTime + new TimeSpan(upgain.Validity, 0, 0, 0);
                    uprate = upgain.UpRate;
                }
                //结算时间比较
                if (mUpEndTime > _time)
                {
                    mRemainUpHour = (float)(mUpEndTime - _time).TotalHours;
                    return uprate;
                }
            }
            return 1.0f;
        }
        //获取本次奖励数量
        public int GetRewardCount()
        {
            int getcount = 0;
            if (mOutCount <= 0)
            {
                return getcount;
            }
            getcount = mOutPutTotalNum / mOutCount;
            return getcount;
        }
        //获取本次奖励
        public void GetReward(int count)
        {
            mOutCount -= 1;
            mOutPutTotalNum -= count;
        }
        //初始化
        public void Initdata(System.DateTime now, int tplOutPutNum)
        {
            float totalh = (float)(now - mStartTime).TotalHours;
            var max = GetMaxTime();
            byte count = GetPerCount(totalh);
            bool isOver = false;
            for (byte i = 0; i < count;i++ )
            {
                if (mOutCount >= max)
                {
                    isOver = true;
                    break;
                }
                AddOutPutNum(tplOutPutNum);
            }
            //还有时间没走完
            if (!isOver)
            {
                mPer = (float)(1-(now - mStartTime).TotalHours);
                mIsInit = true;
            }
        }
        //增加本次收益
        public void AddOutPutNum(int tplOutPutNum)
        {
            var pre = GetPer();
            mOutCount++;

            mStartTime = mStartTime + new TimeSpan(0, 0, (int)pre);
            var uprate = GetUpRate(mStartTime);
            mOutPutTotalNum += (int)(tplOutPutNum * uprate);
        }
    }

    public class BuildOutPutManager
    {
        protected PlayerInstance mRole;

        Dictionary<byte, BuildOutPut> mOutDict = new Dictionary<byte, BuildOutPut>();

        public BuildOutPut GetBuildOutPut(byte typed)
        {
            BuildOutPut result = null;
            mOutDict.TryGetValue(typed, out result);
            return result;
        }

        public int GetCurTplOutCount(byte _type)
        {
            var type = (CSCommon.eMartialItemType)_type;
            if (type > CSCommon.eMartialItemType.MartialStart && type < CSCommon.eMartialItemType.FruitStart)
            {
                return mRole.MartialBuild.GetOutPutCount(_type);
            }
            else if (type > CSCommon.eMartialItemType.FruitStart && type < CSCommon.eMartialItemType.MineralStart)
            {
                return mRole.PlantBuild.GetOutPutCount(_type);
            }
            else if (type > CSCommon.eMartialItemType.MineralStart)
            {
                return mRole.SmeltBuild.GetOutPutCount(_type);
            }
            return 0;
        }


        public void CreateOutPutToAdd(byte type)
        {
            if (GetBuildOutPut(type) != null)
            {
                return;
            }
            BuildOutPut output = new BuildOutPut((byte)type);
            output.mStartTime = System.DateTime.Now;
            AddOutPut(output);
        }

        public void AddOutPut(BuildOutPut output, bool addtime = true)
        {
            mOutDict[output.mType] = output;
            if(addtime)
            {
                output.mTimerHandler = TimerManager.doLoop(output.GetPer(), TimeOver, output);
            }
        }

        public void TimeOver(TimerEvent evt)
        {
            var output = (BuildOutPut)evt.param;
            output.AddOutPutNum(GetCurTplOutCount(output.mType));
            if (output.mOutCount >= output.GetMaxTime())
            {
                TimerManager.clearTimer(output.mTimerHandler);
            }
        }

        private void ClearTimeHandlers()
        {
            foreach (var i in mOutDict.Values)
            {
                if (i.mTimerHandler != null)
                {
                    TimerManager.clearTimer(i.mTimerHandler);
                }
            }
        }


        //--------------------------------------数据初始化--------------------------------------------------------
        public void InitOutData()
        {
            //计算离线数据
            var now = System.DateTime.Now;
            foreach(var i in mOutDict.Values)
            {
                i.Initdata(now, GetCurTplOutCount(i.mType));
                if (i.mIsInit)
                {
                    i.mTimerHandler = TimerManager.doOnce(i.GetPer(), InitTimeOver, i);
                }
            }
        }

        public void InitTimeOver(TimerEvent evt)
        {
            var output = (BuildOutPut)evt.param;
            output.AddOutPutNum(GetCurTplOutCount(output.mType));
            output.mPer = 0;
            output.mIsInit = false;
            if (output.mOutCount >= output.GetMaxTime())
            {
                TimerManager.clearTimer(output.mTimerHandler);
            }
            else
            {
                output.mTimerHandler = TimerManager.doLoop(output.GetPer(), TimeOver, output);
            }
        }

        //--------------------------------------数据存取---------------------------------------------------------

        //初始化数据库数据
        public void MartialUnSerialize(PlayerInstance role, byte[] buffer)
        {
            mRole = role;
            if (buffer == null || buffer.Length <= 0)
            {
                return;
            }
            RPC.DataReader dr = new RPC.DataReader(buffer, 0, buffer.Length, buffer.Length);
            byte count = dr.ReadByte();
            for (byte i = 0; i < count; i++)
            {
                byte type = dr.ReadByte();
                BuildOutPut output = new BuildOutPut(type);
                output.mStartTime = dr.ReadDateTime();
                output.mOutCount = dr.ReadByte();
                output.mUpOutIndex = dr.ReadSByte();
                output.mUpStartTime = dr.ReadDateTime();
                output.mOutPutTotalNum = dr.ReadInt32();
                AddOutPut(output, false);
            }
            InitOutData();
        }

        //保存数据库调用
        public void MartialSerialize()
        {
            ClearTimeHandlers();
            RPC.DataWriter dw = new RPC.DataWriter();
            byte itemCount = (byte)mOutDict.Count;
            dw.Write(itemCount);
            foreach (var i in mOutDict.Values)
            {
                dw.Write(i.mType);
                dw.Write(i.mStartTime);
                dw.Write(i.mOutCount);
                dw.Write(i.mUpOutIndex);
                dw.Write(i.mUpStartTime);
                dw.Write(i.mOutPutTotalNum);
            }
            mRole.PlayerData.MartialData.OutPutInfo =  dw.Trim();
        }
    }
}
