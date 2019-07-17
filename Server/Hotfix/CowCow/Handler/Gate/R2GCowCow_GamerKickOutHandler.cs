using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Gate)]
    public class R2GCowCow_GamerKickOutHandler : AMRpcHandler<R2G_CowCowGamerKickOut, G2R_CowCowGamerKickOut>
    {
        protected override async void Run(Session session, R2G_CowCowGamerKickOut message, Action<G2R_CowCowGamerKickOut> reply)
        {
            G2R_CowCowGamerKickOut response = new G2R_CowCowGamerKickOut();
            try
            {
                User user = Game.Scene.GetComponent<UserComponent>().Get(message.UserID);
                //服务器主动断开客户端连接
                if (user != null)
                {
                    long userSessionId = user.GetComponent<UnitGateComponent>().GateSessionActorId;
                    Game.Scene.GetComponent<NetOuterComponent>().Remove(userSessionId);
                }
                
                await session.Call(new G2R_CowCowGamerOffline() { UserID = message.UserID });
                Log.Info($"将玩家{message.UserID}连接断开");

                reply(response);
            }
            catch(Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
