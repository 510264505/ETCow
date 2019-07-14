using System;
using System.Collections.Generic;
using ETModel;

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
    public class RoomRecord
    {
        //房间号
        public string RoomNumber { get; set; }
        //玩家信息
        public List<RoomGamer> Player { get; set; }
        //时间
        public DateTime RegisterTime { get; set; }
    }

    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfo : Entity
    {
        //账号
        public string Account { get; set; }
        //密码
        public string Password { get; set; }
        //昵称
        public string NickName { get; set; }
        //头像
        public string HeadIcon { get; set; }
        //性别
        public int Sex { get; set; }
        //钻石
        public long Diamond { get; set; }
        //注册时间
        public DateTime RegisterTime { get; set; }
    }
}
