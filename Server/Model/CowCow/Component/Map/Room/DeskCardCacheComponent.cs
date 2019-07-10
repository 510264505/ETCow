using System.Collections.Generic;

namespace ETModel
{
    public class DeskCardCacheComponent : Component
    {
        //剩余牌
        public readonly List<Card> remainderCard = new List<Card>();
        //玩家信息，包括牌
        public readonly Dictionary<int, Gamer> gamers = new Dictionary<int, Gamer>();
        //玩家数量
        public int GamerCount { get { return gamers.Values.Count; } }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }
            base.Dispose();

            remainderCard.Clear();
            foreach (var gamer in gamers.Values)
            {
                gamer.cards.Clear();
            }
        }
    }
}
