using System.Collections.Generic;
using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class Actor_RoomBigSettlementHandler : AMHandler<Actor_CowCowBigSettlement>
    {
        protected override void Run(ETModel.Session session, Actor_CowCowBigSettlement message)
        {
            //直接克隆并设置信息，小结算那里处理何时显示
            UICowCow_GameRoomComponent room = Game.Scene.GetComponent<UIComponent>().Get(UICowCowType.CowCowGameRoom).GetComponent<UICowCow_GameRoomComponent>();
            Dictionary<int, Gamer> gamers = room.GamerComponent.GetDictAll();
            for (int i = 0; i < message.Info.Count; i++)
            {
                UICowCow_BSGamerResultComponent bsgrc = gamers[message.Info[i].SeatID].GetComponent<UICowCow_BSGamerResultComponent>();
                bsgrc.SetGamerBigSettlement(message.Info[i].Banker, 
                    message.Info[i].FiveSmallCow, 
                    message.Info[i].FiveFlowerCow, 
                    message.Info[i].BombCow, 
                    message.Info[i].DoubleCow, 
                    message.Info[i].HaveCow, 
                    message.Info[i].NotCow, 
                    message.Info[i].TotalScore);
            }
        }
    }
}
