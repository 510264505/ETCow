using ETModel;
using System;
using System.Collections.Generic;

namespace ETHotfix
{
    [ObjectSystem]
    public class DeskComponentAwake : AwakeSystem<DeskComponent>
    {
        public override void Awake(DeskComponent self)
        {
            self.Awake();
        }
    }
    public static class DeskComponentSystem
    {
        public static void Awake(this DeskComponent self)
        {
            self.CreateHandCard();
        }

        /// <summary>
        /// 洗牌
        /// </summary>
        public static void Shuffle(this DeskComponent self)
        {
            Random random = new Random();
            List<Card> newCards = new List<Card>();
            foreach (var card in self.library)
            {
                newCards.Insert(random.Next(newCards.Count + 1), card);
            }
            self.library.Clear();
            self.library.AddRange(newCards);
        }

        /// <summary>
        /// 发牌
        /// </summary>
        public static Card Deal(this DeskComponent self)
        {
            Card card = self.library[self.library.Count - 1];
            self.library.Remove(card);
            return card;
        }

        /// <summary>
        /// 添加牌
        /// </summary>
        public static void AddCard(this DeskComponent self, Card card)
        {
            self.library.Add(card);
        }

        /// <summary>
        /// 创建一副牌
        /// </summary>
        private static void CreateHandCard(this DeskComponent self)
        {
            int suitsLen = 4;
            int weightLen = 13;
            //创建普通扑克
            for (int i = 0; i < suitsLen; i++)
            {
                for (int j = 0; j < weightLen; j++)
                {
                    Weight w = (Weight)j;
                    Suits s = (Suits)i;
                    Card card = new Card() { CardSuits = s, CardWeight = w };
                    self.library.Add(card);
                }
            }
            //大小王
            //self.library.Add(new Card() { CardWeight = Weight.Sjoker, CardSuits = Suits.None });
            //self.library.Add(new Card() { CardWeight = Weight.Ljoker, CardSuits = Suits.None });
        }
    }
}
