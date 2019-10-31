using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    public static class CowComparisonHelper
    {
        public static int CalculCoin(Gamer g0, Gamer g1, int multiple)
        {
            bool isWin = IsBankerWin(g0, g1);
            int coin;
            if (isWin)
            {
                coin = -(g1.Multiple * multiple);
            }
            else
            {
                coin = g1.Multiple * multiple;
            }
            return coin;
        }
        private static bool IsBankerWin(Gamer g0, Gamer g1)
        {
            bool isWin;
            if (g0.cardType > g1.cardType)
            {
                isWin = true;
            }
            else if (g0.cardType == g1.cardType)
            {
                // 无牛时
                if (g0.cardType == 0)
                {
                    if (g0.MaxValue > g1.MaxValue)
                    {
                        isWin = true;
                    }
                    else
                    {
                        isWin = false;
                    }
                }
                else
                {
                    //其中一个等于0时
                    if (g0.CowNumber == 0 || g1.CowNumber == 0)
                    {
                        if (g0.CowNumber < g1.CowNumber)
                        {
                            isWin = true;
                        }
                        //相同点数牛 或者 牛牛
                        else if (g0.CowNumber == g1.CowNumber)
                        {
                            isWin = IsMaxValue(g0, g1);
                        }
                        else
                        {
                            isWin = false;
                        }
                    }
                    else
                    {
                        if (g0.CowNumber > g1.CowNumber)
                        {
                            isWin = true;
                        }
                        //相同点数牛 或者 牛牛
                        else if (g0.CowNumber == g1.CowNumber)
                        {
                            isWin = IsMaxValue(g0, g1);
                        }
                        else
                        {
                            isWin = false;
                        }
                    }
                }
            }
            else
            {
                isWin = false;
            }

            return isWin;
        }

        private static bool IsMaxValue(Gamer g0, Gamer g1)
        {
            bool isWin;
            if (g0.MaxValue > g1.MaxValue)
            {
                isWin = true;
            }
            else
            {
                isWin = false;
            }
            return isWin;
        }
    }
}
