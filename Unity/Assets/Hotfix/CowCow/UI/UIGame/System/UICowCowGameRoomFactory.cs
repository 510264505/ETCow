using System;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    public static class UICowCowGameRoomFactory
    {
        public static UI Create(G2C_CowCowEnterGameRoomGate data)
        {
            try
            {
                ResourcesComponent rc = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
                rc.LoadBundle(UICowCowAB.CowCow_Prefabs.StringToAB());
                GameObject ab = (GameObject)rc.GetAsset(UICowCowAB.CowCow_Prefabs, UICowCowType.CowCowGameRoom);
                GameObject gameObject = UnityEngine.Object.Instantiate(ab);

                UI ui = ComponentFactory.Create<UI, string, GameObject>(UICowCowType.CowCowGameRoom, gameObject);
                ui.AddComponent<UICowCow_GameRoomComponent, G2C_CowCowEnterGameRoomGate>(data);
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
