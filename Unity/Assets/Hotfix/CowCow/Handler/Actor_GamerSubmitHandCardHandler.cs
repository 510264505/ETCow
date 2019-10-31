using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class Actor_GamerSubmitHandCardHandler : AMHandler<Actor_CowCowGamerSubmitCardType>
    {
        protected override void Run(ETModel.Session session, Actor_CowCowGamerSubmitCardType message)
        {
            UICowCow_GameRoomComponent room = Game.Scene.GetComponent<UIComponent>().Get(UICowCowType.CowCowGameRoom).GetComponent<UICowCow_GameRoomComponent>();
            for (int i = 0; i < message.SeatIDs.count; i++)
            {
                //room.GamerComponent.Get(message.SeatIDs[i]).GetComponent<UICowCow_GamerInfoComponent>()
            }
        }
    }
}
