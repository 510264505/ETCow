using System.Collections.Generic;

namespace ETModel
{
    public class HandCardComponent : Component
    {
        //所有手牌
        public readonly List<Card> cards = new List<Card>();
        //身份
        public Identity AccessIdentity { get; set; }
        //是否托管
        public bool IsTrusteeship { get; set; }
        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }
            base.Dispose();

            this.cards.Clear();
            this.AccessIdentity = Identity.None;
            this.IsTrusteeship = false;
        }
    }
}
