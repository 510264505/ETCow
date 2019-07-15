using ETModel;
using System.Collections.Generic;

namespace ETHotfix
{
    public static class DeskCardCacheComponentSystem
    {
        /// <summary>
        /// 获取剩余牌
        /// </summary>
        public static List<Card> GetRemainderCard(this DeskCardCacheComponent self)
        {
            return self.remainderCard;
        }

        /// <summary>
        /// 根据椅子号返回玩家信息
        /// </summary>
        public static Gamer GetGamer(this DeskCardCacheComponent self, int chairId)
        {
            if (self.gamers.TryGetValue(chairId, out Gamer gamer))
            {
                return gamer;
            }
            return null;
        }

        /// <summary>
        /// 返回玩家数量
        /// </summary>
        public static int GetGamerCount(this DeskCardCacheComponent self)
        {
            return self.GamerCount;
        }
    }
}
