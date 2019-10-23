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
            int coin = 0;
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
            bool isWin = false;
            if (g0.cardType > g1.cardType)
            {
                isWin = true;
            }
            else if (g0.cardType == g1.cardType)
            {
                if (g0.CowNumber > g1.CowNumber)
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
                isWin = false;
            }

            return isWin;
        }
    }
}
