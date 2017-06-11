using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Com
{
    public class GuildInstance
    {
        Dictionary<ulong, UserRole> mMembers = new Dictionary<ulong, UserRole>();
        public Dictionary<ulong, UserRole> Members
        {
            get { return mMembers; }
        }

        public byte GetNumByPost(byte post)
        {
            byte num = 0;
            foreach (var i in mMembers)
            {
                if (i.Value.RoleData.GuildPost == post)
                {
                    num++;
                }
            }
            return num;
        }

        CSCommon.Data.GuildCom mGuildData = new CSCommon.Data.GuildCom();
        public CSCommon.Data.GuildCom GuildData
        {
            get { return mGuildData; }
            set { mGuildData = value; }
        }

        Dictionary<ulong, CSCommon.Data.Message> mMessages = new Dictionary<ulong, CSCommon.Data.Message>();//帮会消息
        public Dictionary<ulong, CSCommon.Data.Message> Messages
        {
            get { return mMessages; }
        }

        public void UpdateMember(UserRole role)
        {
            if (role == null)
            {
                return;
            }
            if (role.RoleData == null)
            {
                return;
            }
            if (mMembers.ContainsKey(role.RoleData.RoleId))
            {
                mMembers[role.RoleData.RoleId] = role;
            }
            
        }

    }
    public class GuildManager
    {
        static GuildManager smInstance = new GuildManager();
        static public GuildManager Instance
        {
            get {return smInstance;}
        }

        Dictionary<ulong, GuildInstance> mGuilds = new Dictionary<ulong, GuildInstance>();
        public Dictionary<ulong, GuildInstance> Guilds
        {
            get { return mGuilds; }
        }


        public void Init()
        {
            ServerFrame.TimerManager.RegTodayTimerEvent(CSCommon.GuildCommon.Instance.PayTaxTime, GuildPayTax);
        }

        public GuildInstance GetGuild(ulong guildId)
        {
            GuildInstance guild;
            if (mGuilds.TryGetValue(guildId, out guild)==true)
            {
                return guild;
            }
            guild = DB_LoadGuild(guildId);
            if(guild==null)
                return null;

            mGuilds.Add(guildId, guild);
            DB_InitAllGuildMember(guild);
            return guild;
        }

        private GuildInstance DB_LoadGuild(ulong guildId)
        {
            GuildInstance guild = new GuildInstance();

            var condition = "GuildId=" + guildId;
            var dbOp = ServerFrame.DB.DBConnect.SelectData(condition, guild.GuildData, "");
            var tab = UserRoleManager.Instance.DBConnect._ExecuteSelect(dbOp, "GuildCom");
            if (tab == null || tab.Rows.Count != 1)
                return null;

            if (false == ServerFrame.DB.DBConnect.FillObject(guild.GuildData, tab.Rows[0]))
                return null;

            return guild;
        }

        public void DB_InitAllGuildMember(GuildInstance guild)
        {
            ServerFrame.DB.DBOperator dbOp = new ServerFrame.DB.DBOperator();
            dbOp.SqlCode = "select RoleId from RoleCom where GuildId=\'" + guild.GuildData.GuildId + "\'";
            var tab = UserRoleManager.Instance.DBConnect._ExecuteSelect(dbOp, "RoleCom");
            if (tab == null || tab.Rows.Count < 1)
                return;

            for (int i = 0; i < tab.Rows.Count; i++)
            {
                ulong roleId = (ulong)tab.Rows[i]["RoleId"];
                if (roleId != 0)
                {
                    var role = UserRoleManager.Instance.GetRole(roleId);
                    if(role != null)
                    {
                        guild.Members.Add(roleId, role);
                    }
                }
            }
        }

        public bool AddGuildInstance(GuildInstance guild)
        {
            if (mGuilds.ContainsValue(guild))
                return false;

            mGuilds.Add(guild.GuildData.GuildId, guild);
            return true;
        }

        public bool RemoveGuildInstance(ulong guildId)
        {
            if (mGuilds.ContainsKey(guildId))
            {
                mGuilds.Remove(guildId);
                return true;
            }
            return false;
        }


        public void GuildPayTax(ServerFrame.TimerEvent timerEvent)
        {
            foreach(var i in mGuilds)
            {
                if (i.Value.GuildData.GuildGold < (ulong)CSCommon.GuildCommon.Instance.PayTax)
                {
                    //解散帮会
                    DissolveGuild(i.Key);
                }
                else
                {
                    //交税
                    i.Value.GuildData.GuildGold -= (ulong)CSCommon.GuildCommon.Instance.PayTax;
                    if (i.Value.GuildData.GuildGold < (ulong)CSCommon.GuildCommon.Instance.LessPayNum)
                    {
                        //发邮件提醒管理
                        foreach (var member in i.Value.Members)
                        {
                            if (member.Value.RoleData.GuildPost > (byte)CSCommon.eGuildPost.JingYing)
                            {
                                UserRoleManager.Instance.CreateMailAndSend(member.Key, CSCommon.eMailFromType.GuildGoldLessInfo);
                            }

                        }

                    }
                }
            }
        }

        public void DissolveGuild(ulong guildId)
        {
            GuildInstance guild = null;
            mGuilds.TryGetValue(guildId, out guild);
            if (guild == null)
                return;

            foreach(var i in guild.Members)
            {
                UserRoleManager.Instance.DissolveGuildMember(guild.GuildData.GuildId, i.Key);
            }
            guild.Members.Clear();
            RemoveGuildInstance(guildId);
            UserRoleManager.Instance.DB_DelGuildCom(guild.GuildData);
        }
    }
}
