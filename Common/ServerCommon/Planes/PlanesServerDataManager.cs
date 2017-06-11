using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{
    [System.ComponentModel.TypeConverterAttribute("System.ComponentModel.ExpandableObjectConverter")]
    public class PlanesServerDataManager
    {
        static PlanesServerDataManager smInstance = new PlanesServerDataManager();
        public static PlanesServerDataManager Instance
        {
            get { return smInstance; }
        }
        //目前这里面只放了全部本服务器内玩家，以后可能还要放帮会等统一管理对象
        Dictionary<ulong, PlayerInstance> mPlayers = new Dictionary<ulong, PlayerInstance>();
        public Dictionary<ulong, PlayerInstance> Players
        {
            get { return mPlayers; }
        }

        public PlayerInstance FindPlayerInstance(ulong roleId)
        {
            lock (this)
            {
                PlayerInstance player;
                if (false == mPlayers.TryGetValue(roleId, out player))
                    return null;
                return player;
            }
        }

        public void AddPlayerInstance(PlayerInstance player)
        {
            lock (this)
            {
                mPlayers[player.Id] = player;
            }
        }

        public void RemovePlayerInstance(PlayerInstance player)
        {
            lock (this)
            {
                mPlayers.Remove(player.Id);
            }
        }

        public void OnLogoutPlane(PlayerInstance player)
        {
            var lastTime = player.PlayerData.RoleDetail.LastLoginDate;
            int today = System.DateTime.Now.Day;
            player.PlayerData.RoleDetail.TodayPassSecond += (int)((System.DateTime.Now - lastTime).TotalSeconds);
        }

        public void RemovePlayerInstance(ulong roleId)
        {
            lock (this)
            {
                mPlayers.Remove(roleId);
            }
        }
    }
}
