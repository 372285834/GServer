using ServerCommon.Planes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCommon.Data
{
    [ServerFrame.DB.DBBindTable("RankData")]
    public class RankData : RPC.IAutoSaveAndLoad
    {
        PlayerInstance mHostPlayer;
        public void _SetHostPlayer(PlayerInstance player)
        {
            mHostPlayer = player;
        }

        protected void OnPropertyChanged(string proName)
        {
            if (mHostPlayer == null)
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

            mHostPlayer.OnRankValueChanged(proName, dw);
        }

        [ServerFrame.DB.DBBindField("RoleId")]
        [RPC.AutoSaveLoad]
        public ulong RoleId { get; set; }

        [ServerFrame.DB.DBBindField("RoleName")]
        [RPC.AutoSaveLoad]
        public string RoleName { get; set; }

        [ServerFrame.DB.DBBindField("PlanesId")]
        [RPC.AutoSaveLoad]
        public ushort PlanesId { get; set; }

        UInt16 mLevel;
        [ServerFrame.DB.DBBindField("Level")]
        [RPC.AutoSaveLoad]
        public UInt16 Level
        {
            get { return mLevel; }
            set 
            {
                mLevel = value;
                OnPropertyChanged("Level");
            }
        }

        int mArenaPoint;
        [ServerFrame.DB.DBBindField("ArenaPoint")]//竞技场积分
        [RPC.AutoSaveLoad]
        public int ArenaPoint
        {
            get { return mArenaPoint; }
            set
            {
                mArenaPoint = value;
                OnPropertyChanged("ArenaPoint");
            }
        }

        int mFighting;
        [ServerFrame.DB.DBBindField("Fighting")]//战力
        [RPC.AutoSaveLoad]
        public int Fighting
        {
            get { return mFighting; }
            set 
            {
                mFighting = value;
                OnPropertyChanged("Fighting");
            }
        }

        #region 战功
        int mExploit;
        [ServerFrame.DB.DBBindField("Exploit")]//战功
        [RPC.AutoSaveLoad]
        public int Exploit
        {
            get { return mExploit; }
            set
            {
                mExploit = value;
                OnPropertyChanged("Exploit");
            }
        }

        [ServerFrame.DB.DBBindField("ExploitRank")]//战功名次
        [RPC.AutoSaveLoad]
        public int ExploitRank { get; set; }

        byte mExploitBox;
        [ServerFrame.DB.DBBindField("ExploitBox")]//战功奖励箱子数量
        [RPC.AutoSaveLoad]
        public byte ExploitBox
        {
            get { return mExploitBox; }
            set
            {
                mExploitBox = value;
                OnPropertyChanged("ExploitBox");
            }
        }
        #endregion

        #region 威望
        int mPrestige;
        [ServerFrame.DB.DBBindField("Prestige")]//威望
        [RPC.AutoSaveLoad]
        public int Prestige
        {
            get { return mPrestige; }
            set
            {
                mPrestige = value;
                OnPropertyChanged("Prestige");
            }
        }

        [ServerFrame.DB.DBBindField("PrestigeRank")]//威望名次
        [RPC.AutoSaveLoad]
        public int PrestigeRank { get; set; }

        byte mPrestigeBox;
        [ServerFrame.DB.DBBindField("PrestigeBox")]//威望奖励箱子数量
        [RPC.AutoSaveLoad]
        public byte PrestigeBox
        {
            get { return mPrestigeBox; }
            set
            {
                mPrestigeBox = value;
                OnPropertyChanged("PrestigeBox");
            }
        }
        #endregion

        #region 挑战
        int mChallenge;
        [ServerFrame.DB.DBBindField("Challenge")]//挑战
        [RPC.AutoSaveLoad]
        public int Challenge
        {
            get { return mChallenge; }
            set
            {
                mChallenge = value;
                OnPropertyChanged("Challenge");
            }
        }
        #endregion       

        #region 抢夺龙脉
        #endregion       

        #region 击杀敌人
        int mKillEnemy;
        [ServerFrame.DB.DBBindField("KillEnemy")]//杀敌
        [RPC.AutoSaveLoad]
        public int KillEnemy
        {
            get { return mKillEnemy; }
            set
            {
                mKillEnemy = value;
                OnPropertyChanged("KillEnemy");
            }
        }       
        #endregion

        #region 击杀士兵

        #endregion

        #region 击杀禁卫军

        #endregion

        #region 运送粮草
        
        #endregion

        #region 运送军饷

        #endregion

        #region 运送攻城车

        #endregion

        #region 击倒城旗

        #endregion

        #region 攻击城池

        #endregion

    }


}
