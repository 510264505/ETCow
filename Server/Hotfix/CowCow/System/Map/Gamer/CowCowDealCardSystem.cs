using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    public static class CowCowDealCardSystem
    {
        /// <summary>
        /// 发所有牌
        /// </summary>
        public static List<int> DealAllCards(int count)
        {
            List<int> allCards = new List<int>();
            while(allCards.Count < count)
            { 
                int n = RandomHelper.RandomNumber(1, 53); //除去大小鬼，52张牌
                if (!allCards.Contains(n))
                {
                    allCards.Insert(RandomHelper.RandomNumber(allCards.Count), n);
                }
            }
            return allCards;
        }
    }
}
