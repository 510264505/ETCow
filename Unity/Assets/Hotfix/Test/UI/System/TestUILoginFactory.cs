using ETModel;
using System;
using UnityEngine;

namespace ETHotfix
{
    class TestUILoginFactory
    {
        public static UI Create()
        {
            try
            {
                ResourcesComponent rc = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
                rc.LoadBundle(UIType.TestUILogin.StringToAB());
                GameObject go = (GameObject)rc.GetAsset(UIType.TestUILogin.StringToAB(), UIType.TestUILogin);
                GameObject gameObject = UnityEngine.Object.Instantiate(go); //创建对象

                UI ui = ComponentFactory.Create<UI, string, GameObject>(UIType.TestUILogin, gameObject);
                ui.AddComponent<TestUILoginComponent>(); //给创建的对象加入这个组件功能
                return ui;
            }
            catch (Exception e)
            {
                Log.Error("创建UI异常:" + e.Message);
                return null;
            }
        }
    }
}
