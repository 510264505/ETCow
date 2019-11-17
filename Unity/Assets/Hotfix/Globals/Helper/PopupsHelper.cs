using System;

namespace ETHotfix
{
    public static class PopupsHelper
    {
        public static void ShowPopups(string dec, Action action = null)
        {
            Game.Scene.GetComponent<UIComponent>().Get(GlobalsUIType.UIPopupsCanvas).GetComponent<UIPopupsComponent>().ShowPopups(dec, action);
        }
        public static void ShowLoading(bool isShow)
        {
            Game.Scene.GetComponent<UIComponent>().Get(GlobalsUIType.UIPopupsCanvas).GetComponent<UIPopupsComponent>().ShowLoading(isShow);
        }
    }
}
