using System;


namespace ETHotfix
{
    public static class CardHelper
    {
        private const string BottomLine = "_";
        public static string GetCardAssetName(int card)
        {
            int dot = card % 13;
            int flower = card / 13;
            int dotCard = dot == 0 ? 13 : dot;
            int flowerCard = dot == 0 ? flower : flower + 1;
            return $"{flowerCard}{BottomLine}{dotCard}";
        }
    }
}
