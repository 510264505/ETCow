using System.Collections.Generic;

namespace ETModel
{
    public class RoomPlayer
    {
        //椅子号
        public int ChairID { get; set; }
        //昵称
        public string NickName { get; set; }
        //输赢
        public int LoseWinCoin { get; set; }
    }
    public class RoomRecord
    {
        //房间号
        public string RoomNumber { get; set; }
        public List<RoomPlayer> Player { get; set; }
    }
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
        //金额
        public long Money { get; set; }
        public List<RoomRecord> Records { get; set; }
    }
}
