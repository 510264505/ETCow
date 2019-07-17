using UnityEngine;
using ETModel;

namespace ETHotfix
{
    [Event(EventIdCowCowType.CreateGameRoom)]
    public class CowCow_CreateGameRoomUI : AEvent<G2C_CowCowCreateGameRoomGate>
    {
        public override void Run(G2C_CowCowCreateGameRoomGate data)
        {
            UI ui = UICowCowGameRoomFactory.Create(data);
            Game.Scene.GetComponent<UIComponent>().Add(ui);
        }
    }
}

