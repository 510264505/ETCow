using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 抢庄
    /// </summary>
    public static class Actor_GrabBankerHelper
    {
        public static async ETVoid SendGrabBanker(string roomId, int seatId, int multiple)
        {
            Session session = Game.Scene.GetComponent<SessionComponent>().Session;
            M2C_CowCowGrabBanker response = (M2C_CowCowGrabBanker)await session.Call(new C2M_CowCowGrabBanker() { RoomID = roomId, SeatID = seatId, Multiple = multiple });
            if (response.Error == 0)
            {
                //成功
                UICowCow_GameRoomComponent room = Game.Scene.GetComponent<UIComponent>().Get(UICowCowType.CowCowGameRoom).GetComponent<UICowCow_GameRoomComponent>();
                room.ShowHideGrabBanker(false);
            }
        }
    }
}
