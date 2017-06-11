using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerFrame
{
    public enum GameObjectType : ushort
    {
        None,
        Account,
        Player, //角色
        NPC, //NPC
        Item, //物品
        ConsignGird,//寄售格子
        Quest, //任务
        Horse, //坐骑
        Email, //邮件
        Buffer, //BUFF
        Message,
        Guild,
        Trigger,// 触发器
        WayPoint,// 路点
        Map,
        Server,
        PlayerImage, //玩家镜像
        MAX   // 最大不能超过32（uuid的生成type占5位）
    }
}
