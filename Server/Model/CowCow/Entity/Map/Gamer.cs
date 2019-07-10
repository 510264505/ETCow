using System.Collections.Generic;

namespace ETModel
{
    [ObjectSystem]
    public class GamerAwakeSystem : AwakeSystem<Gamer, long>
    {
        public override void Awake(Gamer self, long id)
        {
            self.Awake(id);
        }
    }
    public sealed class Gamer : Entity
    {
        //用户唯一ID
        public long UserID { get; set; }
        //玩家Gate   ActorID
        public long ActorID { get; set; }
        //玩家所在房间号ID
        public long RoomID { get; set; }
        //玩家所在房间的椅子号
        public int ChairID { get; set; }
        //玩家身份
        public Identity Identity { get; set; }
        //是否准备
        public bool IsReady { get; set; }
        //是否离线
        public bool IsOffline { get; set; }
        //玩家牌
        public List<Card> cards { get; set; }
        //牌类型
        public CardGroupType cardsType { get; set; }
        public void Awake(long id)
        {
            this.UserID = id;
        }
        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }
            base.Dispose();

            this.UserID = 0;
            this.ActorID = 0;
            this.RoomID = 0;
            this.ChairID = 0;
            this.Identity = Identity.None;
            this.IsReady = false;
            this.IsOffline = false;
            this.cards.Clear();
            this.cardsType = CardGroupType.None;
        }
    }
}
