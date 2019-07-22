using ETModel;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace ETHotfix
{
    /// <summary>
    /// 房间中多个玩家信息
    /// </summary>
    public class RoomGamer
    {
        //椅子号
        public int ChairID { get; set; }
        //昵称
        public string NickName { get; set; }
        //输赢
        public int LoseWinCoin { get; set; }
    }

    /// <summary>
    /// 房间记录
    /// </summary>
    [BsonIgnoreExtraElements]
    public class RoomRecord : Entity
    {
        //房间号
        public string RoomNumber { get; set; }
        //玩家信息
        public List<RoomGamer> Player { get; set; }
        //时间
        public DateTime RegisterTime { get; set; }
    }
}
