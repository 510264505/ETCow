using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
    public static class Actor_GamerReadyHelper
    {
        public static async ETVoid OnReady(int seatId, string roomId)
        {
            Session session = Game.Scene.GetComponent<SessionComponent>().Session;
            M2C_CowCowGamerReady m2c_Ready = (M2C_CowCowGamerReady)await session.Call(new C2M_CowCowGamerReady() { RoomID = roomId, SeatID = seatId });
            if (m2c_Ready.Error == 0)
            {
                UICowCow_GameRoomComponent room = Game.Scene.GetComponent<UIComponent>().Get(UICowCowType.CowCowGameRoom).GetComponent<UICowCow_GameRoomComponent>();
                UICowCow_GamerInfoComponent gc = room.GamerComponent.LocalGamer.GetComponent<UICowCow_GamerInfoComponent>();
                room.ShowHideReadyButton(false);
                gc.SetStatus(UIGamerStatusString.Ready, UIGamerStatus.Ready);
            }
        }
    }
}
