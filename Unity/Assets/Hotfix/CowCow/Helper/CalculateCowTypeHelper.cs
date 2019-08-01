using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETHotfix
{
    class CalculateCowTypeHelper
    {
        /// <summary>
        /// 取最大的牛牌型，并取出同牌型最大的那张比拼
        /// </summary>
        public static int[] MostMaxCowType(List<int> card)
        {
            int oneColumn = 13; //一列，一种花色
            int bigTan = 11; //大于十
            int fiveSmallSum = 0;
            int bombCount = 0;
            int fiveFlowerCount = 0;
            int haveSum = 0;
            int haveNumber = 0;
            int notMax = 0;
            int notIndex = 0;
            int oneMax = card[card.Count - 1]; //直接取最后一张
            int twoMax = 0;
            Dictionary<int, bool> dic = new Dictionary<int, bool>();
            for (int i = 0; i < card.Count; i++)
            {
                if (card[i] % oneColumn != 0) //五小牛
                {
                    fiveSmallSum += card[i] % oneColumn;
                }
                else
                {
                    fiveSmallSum += card[i] % oneColumn + oneColumn;
                }
                if (!dic.ContainsKey(card[i] % oneColumn)) //炸弹牛
                {
                    dic.Add(card[i] % oneColumn, true);
                }
                else
                {
                    bombCount++;
                }
                if (card[i] % oneColumn >= bigTan || card[i] % oneColumn == 0) //五花牛
                {
                    fiveFlowerCount++;
                }
                if (i <= 2) //普通牛
                {
                    haveSum += card[i];
                }
                else
                {
                    if (haveNumber < card[i] % 13) //取最大的，第二次比不过时，不用从新赋值
                    {
                        twoMax = card[i];
                    }
                    haveNumber += card[i] % oneColumn;
                }
                if (card[i] % oneColumn > notMax) //无牛
                {
                    notIndex = i;
                    notMax = card[i];
                }
            }
            if (fiveSmallSum < 10)
            {
                return new int[3] { (int)CowType.FiveSmallCow, fiveSmallSum, notIndex };
            }
            else if (bombCount >= 3)
            {
                return new int[3] { (int)CowType.BombCow, bombCount, oneMax };
            }
            else if (fiveFlowerCount == 5)
            {
                return new int[3] { (int)CowType.FiveFlowerCow, fiveFlowerCount, notMax };
            }
            else if (haveSum == 10)
            {
                return new int[3] { (int)CowType.Cow, haveNumber % 10, twoMax };
            }
            else
            {
                return new int[3] { (int)CowType.None, notMax, notMax };
            }
        }
        /// <summary>
        /// 五小牛
        /// </summary>
        public static int[] FiveSmallCow(List<int> card)
        {
            int sum = 0;
            int max = 0;
            for (int i = 0; i < card.Count; i++)
            {
                if (card[i] % 13 != 0) //五小牛
                {
                    sum += card[i] % 13;
                }
                else
                {
                    sum += card[i] % 13 + 13;
                }
                if (card[i] % 13 > max) //无牛
                {
                    max = i;
                }
            }
            return sum < 10 ? new int[] { 4, sum, max } : null;
        }
        /// <summary>
        /// 炸弹牛
        /// </summary>
        public static int[] BombCow(List<int> card)
        {
            int count = 0;
            Dictionary<int, bool> dic = new Dictionary<int, bool>();
            for (int i = 0; i < card.Count; i++)
            {
                if (!dic.ContainsKey(card[i] % 13))
                {
                    dic.Add(card[i] % 13, true);
                }
                else
                {
                    count++;
                }
            }
            return count >= 3 ? new int[3] { 3, count, card[card.Count - 1] } : null;
        }
        /// <summary>
        /// 五花牛
        /// </summary>
        public static int[] FiveFlowerCow(List<int> card)
        {
            int count = 0;
            int max = 0;
            for (int i = 0; i < card.Count; i++)
            {
                if (card[i] % 13 >= 11 || card[i] % 13 == 0)
                {
                    count++;
                }
                if (card[i] % 13 > max) //无牛
                {
                    max = card[i];
                }
            }
            return count >= 5 ? new int[3] { 2, count, max } : null;
        }
        /// <summary>
        /// 有牛
        /// </summary>
        public static int[] HaveCow(List<int> card)
        {
            int sum = 0;
            int number = 0;
            int max = 0;
            for (int i = 0; i < card.Count; i++)
            {
                if (i <= 2) //普通牛
                {
                    sum += card[i];
                }
                else
                {
                    if (number < card[i] % 13) //取最大的，第二次比不过时，不用从新赋值
                    {
                        max = card[i];
                    }
                    number += card[i] % 13;
                }
            }
            return sum == 10 ? new int[3] { 2, sum, max } : null;
        }
        /// <summary>
        /// 散牌中的最大值
        /// </summary>
        public static int[] NotCow(List<int> card)
        {
            int max = 0;
            int index = 0;
            for (int i = 0; i < card.Count; i++)
            {
                if (card[i] % 13 > max)
                {
                    index = i;
                    max = card[i];
                }
            }
            return new int[3] { 0, max, max };
        }
        /// <summary>
        /// 所有有牛组合-----------------客户端需要给玩家有牛的提示
        /// </summary>
        public static string IsHaveCow(List<int> player)
        {
            //先计算是否五小牛
            int sum = 0;
            for (int i = 0; i < player.Count; i++)
            {
                sum += player[i];
            }
            if (sum < 10)
            {
                return "五小牛";
            }
            List<int> sumGroup = new List<int>();
            List<List<int>> allCowGroup = new List<List<int>>();
            List<List<int>> allGroup = new List<List<int>>();
            allGroup = GetAllCombination(player, allGroup, new List<int>(), 0);
            for (int i = 0; i < allGroup.Count; i++)
            {
                sum = 0;
                for (int j = 0; j < allGroup[i].Count; j++)
                {
                    sum += allGroup[i][j] % 13;
                }
                if (sum % 10 == 0) //有牛，还有个排序，取最小花色(数字)的牛
                {
                    sumGroup.Add(sum);
                    allCowGroup.Add(allGroup[i]);
                }
            }
            allGroup.Clear();
            //int index = 0;    //取最小数字的牛
            //int min = sumGroup[index];
            //for (int i = 0; i < sumGroup.Count; i++)
            //{
            //    if (min > sumGroup[i])
            //    {
            //        index = i;
            //        min = sumGroup[i];
            //    }
            //}
            return "有牛";
        }
        /// <summary>
        /// 所有牌型组合
        /// </summary>
        public static List<List<int>> GetAllCombination(List<int> player, List<List<int>> li, List<int> l, int index)
        {
            if (l.Count == 3)
            {
                List<int> temp = new List<int>();
                for (int i = 0; i < l.Count; i++)
                {
                    temp.Add(l[i]);
                }
                li.Add(temp);
            }
            for (int i = index; i < player.Count; i++)
            {
                if (l.Count < 3) //先执行完第一、二行
                {
                    l.Add(player[i]);
                    GetAllCombination(player, li, l, i + 1);
                    l.RemoveAt(l.Count - 1); //直到条件不满足，并且有返回值得时候才从这里继续开始
                }
            }
            return li;
        }
        /// <summary>
        /// 计算阶乘
        /// </summary>
        public static int Factorial(int n)
        {
            if (n == 0)
            {
                return 0;
            }
            if (n == 1)
            {
                return n;
            }
            return n * Factorial(n - 1);
        }
    }
}
