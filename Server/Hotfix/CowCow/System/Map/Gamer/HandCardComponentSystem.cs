using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    public static class HandCardComponentSystem
    {
        /// <summary>
        /// 获取所有手牌
        /// </summary>
        public static Card[] GetAll(this HandCardComponent self)
        {
            return self.cards.ToArray();
        }
        /// <summary>
        /// 添加手牌
        /// </summary>
        public static void AddCard(this HandCardComponent self, Card card)
        {
            self.cards.Add(card);
        }
        /// <summary>
        /// 开牌
        /// </summary>
        public static void OpenCard(this HandCardComponent self)
        {

        }
        /// <summary>
        /// 排序
        /// </summary>
        public static void Sort(this HandCardComponent self)
        {

        }
    }
}
