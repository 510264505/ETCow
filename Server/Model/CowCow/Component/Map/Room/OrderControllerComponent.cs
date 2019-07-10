using System.Collections.Generic;

namespace ETModel
{
    public class OrderControllerComponent : Component
    {
        //当前庄家
        public int CurrentBanker { get; set; }
        //当前庄家椅子
        public int CurrentBankerChairID { get; set; }
        //庄家牌
        public readonly List<Card> cards = new List<Card>();
        //牌类型
        public CardGroupType cardsType { get; set; }
        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }
            base.Dispose();

            this.CurrentBanker = 0;
            this.CurrentBankerChairID = 0;
            this.cards.Clear();
            this.cardsType = CardGroupType.None;
        }
    }
}
