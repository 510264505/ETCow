using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETHotfix
{
    public struct CalculateCowTypeData
    {
        public int maxCard;
        public CowType cowType;
        public CardFlowerColor floweColor;
        public int cowNumber;
        public int[] indexs;
        public void Init()
        {
            this.maxCard = 0;
            this.cowType = CowType.None;
            this.floweColor = CardFlowerColor.None;
            this.cowNumber = 0;
        }
    }
    public static class CalculateCowTypeHelper
    {
        private const int thirteen = 13;
        private const int eleven = 11;
        private const int tan = 10;
        private const int three = 3;
        private static CalculateCowTypeData data = new CalculateCowTypeData();
        /// <summary>
        /// 取最大的牛牌型，并取出同牌型最大的那张比拼
        /// </summary>
        public static CalculateCowTypeData MostMaxCowType(List<int> card)
        {
            data.Init();
            if (FiveSmallCow(card))
            {
                return data;
            }
            else if (BombCow(card))
            {
                return data;
            }
            else if (FiveFlowerCow(card))
            {
                return data;
            }
            else if (HaveCow(card))
            {
                return data;
            }
            return data;
        }
        /// <summary>
        /// 五小牛
        /// </summary>
        public static bool FiveSmallCow(List<int> card)
        {
            int sum = 0;
            for (int i = 0; i < card.Count; i++)
            {
                if (card[i] % thirteen != 0) //五小牛
                {
                    sum += card[i] % thirteen;
                }
                else
                {
                    sum += card[i] % thirteen + thirteen;
                }
                if (card[i] % thirteen >= data.maxCard % thirteen && card[i] >= data.maxCard)
                {
                    data.maxCard = card[i]; //五张中最大
                }
            }
            bool isFiveSmallCow = sum < tan;
            if (isFiveSmallCow)
            {
                data.cowType = CowType.FiveSmallCow;
            }
            data.floweColor = (CardFlowerColor)(data.maxCard % thirteen);
            return isFiveSmallCow;
        }
        /// <summary>
        /// 炸弹牛
        /// </summary>
        public static bool BombCow(List<int> card)
        {
            int count = 0;
            int num = 0;
            Dictionary<int, bool> dic = new Dictionary<int, bool>();
            for (int i = 0; i < card.Count; i++)
            {
                if (!dic.ContainsKey(card[i] % thirteen))
                {
                    dic.Add(card[i] % thirteen, true);
                }
                else
                {
                    count++;
                    num = card[i] % thirteen;
                }
            }
            bool isBomb = count >= three;
            if (isBomb)
            {
                data.cowType = CowType.BombCow;
                data.indexs = new int[4];
                int n = 0;
                for (int i = 0; i < card.Count; i++)
                {
                    if (card[i] % thirteen == num)
                    {
                        data.indexs[n] = i;
                        n++;
                    }
                }
            }
            return isBomb;
        }
        /// <summary>
        /// 五花牛
        /// </summary>
        public static bool FiveFlowerCow(List<int> card)
        {
            int count = 0;
            for (int i = 0; i < card.Count; i++)
            {
                if (card[i] % 13 >= eleven || card[i] % thirteen == 0)
                {
                    count++;
                }
            }
            bool isFiveFlowerCow = count >= 5;
            if (isFiveFlowerCow)
            {
                data.cowType = CowType.FiveFlowerCow;
            }
            return isFiveFlowerCow;
        }
        /// <summary>
        /// 有牛
        /// </summary>
        public static bool HaveCow(List<int> card)
        {
            tempHaveCow.Clear();
            Combination(card, three);
            bool isHaveCow = tempHaveCow.Count > 0;
            if (isHaveCow)
            {
                data.cowType = CowType.HaveCow;
                data.indexs = tempHaveCow[0];
                data.cowNumber = cowNumber;
            }
            return isHaveCow;
        }
        /// <summary>
        /// 散牌中的最大值
        /// </summary>
        public static bool NotCow(List<int> card)
        {
            data.cowType = CowType.None;
            return true;
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
        private static List<int[]> tempHaveCow = new List<int[]>();
        private static int cowNumber = 0;
        public static void Combination(List<int> list, int nCount, List<int> result = null, int head = 0)
        {
            if (result == null)
            {
                result = new List<int>();
            }
            if (result.Count == nCount)
            {
                int sum = 0;
                for (int i = 0; i < nCount; i++)
                {
                    int num = result[i] % thirteen;
                    if (num < tan)
                    {
                        sum += num;
                    }
                }
                if (sum % tan == 0)
                {
                    List<int> indexs = new List<int>();
                    List<int> tempSum = new List<int>();
                    tempSum.AddRange(list);
                    for (int i = 0; i < result.Count; i++)
                    {
                        for (int j = 0; j < list.Count; j++)
                        {
                            if (result[i] == list[j])
                            {
                                indexs.Add(j);
                                break;
                            }
                        }
                        tempSum.Remove(result[i]);
                    }
                    tempHaveCow.Add(indexs.ToArray());
                    cowNumber = 0;
                    for (int i = 0; i < tempSum.Count; i++)
                    {
                        int num = tempSum[i] % thirteen;
                        if (num < tan)
                        {
                            cowNumber += num;
                        }
                    }
                    cowNumber %= tan;
                }
            }
            else
            {
                for (int i = head; i < list.Count; i++)
                {
                    if (result.Count < nCount)
                    {
                        result.Add(list[i]);
                        Combination(list, nCount, result, i + 1);
                        result.RemoveAt(result.Count - 1);
                    }
                }
            }
        }
    }
}
