using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    public static class UICowCowCreateChildGameObjectFactory
    {
        public static UI Create<T>(string prefabName, UI uiParent, GameObject parent) where T : Component, new()
        {
            ResourcesComponent rc = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
            rc.LoadBundle(UICowCowAB.CowCow_Prefabs.StringToAB());
            GameObject prefab = (GameObject)rc.GetAsset(UICowCowAB.CowCow_Prefabs.StringToAB(), prefabName);
            GameObject gameObject = UnityEngine.Object.Instantiate(prefab);

            UI ui = ComponentFactory.Create<UI, string, GameObject>(prefabName, gameObject);
            uiParent.Add(ui);
            ui.GameObject.transform.SetParent(parent.transform, false);
            ui.AddComponent<T>();
            return ui;
        }
    }
}
