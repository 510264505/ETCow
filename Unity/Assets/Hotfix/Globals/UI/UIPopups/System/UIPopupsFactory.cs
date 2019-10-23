using System;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    public static class UIPopupsFactory
    {
        public static UI Create()
        {
            try
            {
                ResourcesComponent res = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
                res.LoadBundle(GlobalsUIAB.Globals_Prefabs);
                GameObject ab = (GameObject)res.GetAsset(GlobalsUIAB.Globals_Prefabs, GlobalsUIType.UIPopupsCanvas);
                GameObject go = UnityEngine.Object.Instantiate(ab);

                UI ui = ComponentFactory.Create<UI, string, GameObject>(GlobalsUIType.UIPopupsCanvas, go, false);

                res.UnloadBundle(GlobalsUIAB.Globals_Prefabs);
                ui.AddComponent<UIPopupsComponent>();
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }
    }
}