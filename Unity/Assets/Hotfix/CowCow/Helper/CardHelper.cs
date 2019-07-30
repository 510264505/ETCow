using System;
namespace ETHotfix
{
    public static class CardHelper
    {
        private const string BottomLine = "_";
        public static string GetCardAssetName(int card)
        {
            return $"{((card / 13) + 1)}{BottomLine}{(card % 13)}";
        }
    }
}
