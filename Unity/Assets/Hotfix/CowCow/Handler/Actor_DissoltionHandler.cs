using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class Actor_DissoltionHandler : AMHandler<Actor_CowCowDissoltion>
    {
        protected override void Run(ETModel.Session session, Actor_CowCowDissoltion message)
        {
            UICowCow_GameRoomComponent room = Game.Scene.GetComponent<UIComponent>().Get(UICowCowType.CowCowGameRoom).GetComponent<UICowCow_GameRoomComponent>();
            if (!message.IsDiss)
            {
                for (int i = 0; i < message.Info.count; i++)
                {
                    if (i == 0)
                    {
                        room.DissComponent.ShowInitiateDissoltion(message.Info[i].SeatID);
                    }
                    else
                    {
                        room.DissComponent.ShowOtherVote(message.Info[i].SeatID, message.Info[i].IsAgree ? "同意" : "不同意");
                    }
                }
            }
            else
            {
                //解散(销毁)房间，返回大厅
                if (room.GamerComponent.GetGamerCount() == message.Info.count)
                {
                    room.DissComponent.DelayCloseDissoltion().Coroutine();
                }
                room.Dispose();
                Game.EventSystem.Run(CowCowEventIdType.RemoveGameRoom);
                Game.Scene.GetComponent<UIComponent>().Get(UICowCowType.CowCowLobby).GetComponent<UICowCowLobbyComponent>().ShowHideLobby(true);
            }
        }
    }
}
