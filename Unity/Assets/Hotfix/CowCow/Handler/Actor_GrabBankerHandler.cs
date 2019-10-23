using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
    //[MessageHandler]
    //public class Actor_GrabBankerHandler : AMHandler<Actor_CowCowGrabBanker>
    //{
    //    /// <summary>
    //    /// 通知所有玩家，谁是庄家
    //    /// </summary>
    //    protected override void Run(ETModel.Session session, Actor_CowCowGrabBanker message)
    //    {
    //        UICowCow_GameRoomComponent room = Game.Scene.GetComponent<UIComponent>().Get(UICowCowType.CowCowGameRoom).GetComponent<UICowCow_GameRoomComponent>();
    //        Dictionary<int, Gamer> gamers = room.GamerComponent.GetDictAll();
    //        foreach (Gamer gamer in gamers.Values)
    //        {
    //            gamer.GetComponent<UICowCow_GamerInfoComponent>().ShowHideBankerIcon(false);
    //        }
    //        room.GamerComponent.Get(message.SeatIDs).GetComponent<UICowCow_GamerInfoComponent>().ShowHideBankerIcon(true);
    //    }
    //}
}
