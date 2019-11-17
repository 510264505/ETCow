using ETModel;

namespace ETHotfix
{
    public static class Actor_SubmitHandCardHelper
    {
        public static async ETVoid OnSubmitHandCard(C2M_CowCowGamerSubmitCardType request)
        {
            Session session = Game.Scene.GetComponent<SessionComponent>().Session;
            UICowCow_GameRoomComponent room = Game.Scene.GetComponent<UIComponent>().Get(UICowCowType.CowCowGameRoom).GetComponent<UICowCow_GameRoomComponent>();
            request.UserID = ClientComponent.Instance.User.UserID;
            request.SeatID = room.GamerComponent.LocalSeatID;
            M2C_CowCowGamerSubmitCardType response = (M2C_CowCowGamerSubmitCardType)await session.Call(request);
            if (response.Error == 0)
            {
                //盖牌
            }
        }
    }
}
