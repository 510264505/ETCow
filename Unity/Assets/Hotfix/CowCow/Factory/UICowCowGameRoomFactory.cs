using System;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    public static class UICowCowGameRoomFactory
    {
        public static UI Create(G2C_CowCowCreateGameRoomGate room)
        {
            try
            {
                ResourcesComponent rc = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
                rc.LoadBundle(UICowCowAB.CowCow_Prefabs.StringToAB());
                GameObject ab = (GameObject)rc.GetAsset(UICowCowAB.CowCow_Prefabs.StringToAB(), UICowCowType.CowCowGameRoom);
                GameObject gameObject = UnityEngine.Object.Instantiate(ab);

                UI ui = ComponentFactory.Create<UI, string, GameObject>(UICowCowType.CowCowGameRoom, gameObject);
                ui.AddComponent<UICowCow_GameRoomComponent>(); //加入游戏房间组件
                ui.GetComponent<UICowCow_GameRoomComponent>().Init(room.GameName, room.Bureau, room.RuleBit, room.RoomID);
                ui.GetComponent<UICowCow_GameRoomComponent>().AddGamer(room.GamerInfo, room.GamerInfo.SeatID);
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }
        public static UI Create(G2C_CowCowJoinGameRoomGate room)
        {
            try
            {
                ResourcesComponent rc = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
                rc.LoadBundle(UICowCowAB.CowCow_Prefabs.StringToAB());
                GameObject ab = (GameObject)rc.GetAsset(UICowCowAB.CowCow_Prefabs.StringToAB(), UICowCowType.CowCowGameRoom);
                GameObject gameObject = UnityEngine.Object.Instantiate(ab);

                UI ui = ComponentFactory.Create<UI, string, GameObject>(UICowCowType.CowCowGameRoom, gameObject);
                ui.AddComponent<UICowCow_GameRoomComponent>(); //加入游戏房间组件
                ui.GetComponent<UICowCow_GameRoomComponent>().Init(room.GameName, room.Bureau, room.RuleBit, room.RoomID);
                
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
