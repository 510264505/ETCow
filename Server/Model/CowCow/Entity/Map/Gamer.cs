using System.Collections.Generic;

namespace ETModel
{
    [ObjectSystem]
    public class GamerAwakeSystem : AwakeSystem<Gamer>
    {
        public override void Awake(Gamer self)
        {
            self.Awake();
        }
    }
    public sealed class Gamer : Entity
    {
        //用户唯一ID        DB数据库的 _id
        public long UserID { get; set; }
        //玩家Gate   ActorID     component 的 InstanceId
        //public long ActorID { get; set; }
        //玩家所在房间号ID
        public string RoomID { get; set; }
        //玩家名
        public string Name { get; set; }
        //玩家所在房间的椅子号
        public int SeatID { get; set; }
        //玩家身份
        public Identity Identity { get; set; }
        //是否准备
        public GamerStatus Status { get; set; }
        //是否离线
        public bool IsOffline { get; set; }
        //玩家金币
        public int Coin { get; set; }
        //玩家性别
        public int Sex { get; set; }
        //玩家头像
        public string HeadIcon { get; set; }
        //是否提交手牌
        public bool IsSubmitHandCards { get; set; } = false;
        //玩家牌
        public List<int> cards { get; set; }
        //牌类型
        public int cardType { get; set; }
        //牌中最大的那张
        public int MaxValue { get; set; }
        //最大牌的花色
        public int FloweColor { get; set; }
        //牛几
        public int CowNumber { get; set; }
        //倍数
        public int Multiple { get; set; }
        //抢庄
        public int GrabBanker { get; set; }
        //做庄次数
        public int BankerCount { get; set; }
        //五小牛次数
        public int FiveSmallCowCount { get; set; }
        //五花牛次数
        public int FiveFlowerCowCount { get; set; }
        //炸弹牛次数
        public int BombCowCount { get; set; }
        //牛牛次数
        public int DoubleCowCount { get; set; }
        //有牛次数
        public int HaveCowCount { get; set; }
        //无牛次数
        public int NotCowCount { get; set; }
        public void Awake()
        {
            this.UserID = this.Id;
            this.cards = new List<int>();
            this.Status = GamerStatus.None;
            this.BankerCount = 0;
            this.FiveSmallCowCount = 0;
            this.FiveFlowerCowCount = 0;
            this.BombCowCount = 0;
            this.DoubleCowCount = 0;
            this.HaveCowCount = 0;
            this.NotCowCount = 0;
        }
        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }
            base.Dispose();

            this.UserID = 0;
            this.RoomID = string.Empty;
            this.Name = string.Empty;
            this.SeatID = 0;
            this.Identity = Identity.None;
            this.Status = 0;
            this.IsOffline = false;
            this.Coin = 0;
            this.Sex = 0;
            this.HeadIcon = string.Empty;
            this.cards.Clear();
            this.cardType = 0;
            this.MaxValue = 0;
            this.FloweColor = 0;
            this.CowNumber = 0;
            this.Multiple = 0;
            this.GrabBanker = 0;
            this.BankerCount = 0;
            this.FiveSmallCowCount = 0;
            this.FiveFlowerCowCount = 0;
            this.BombCowCount = 0;
            this.DoubleCowCount = 0;
            this.HaveCowCount = 0;
            this.NotCowCount = 0;
        }
    }
}
