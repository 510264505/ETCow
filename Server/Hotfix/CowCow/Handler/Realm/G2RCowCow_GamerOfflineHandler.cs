using System;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Realm)]
    public class G2RCowCow_GamerOfflineHandler : AMRpcHandler<G2R_CowCowGamerOffline, R2G_CowCowGamerOffline>
    {
        protected override async void Run(Session session, G2R_CowCowGamerOffline message, Action<R2G_CowCowGamerOffline> reply)
        {
            R2G_CowCowGamerOffline response = new R2G_CowCowGamerOffline();
            try
            {
                Game.Scene.GetComponent<OnlineComponent>().Remove(message.UserID);
                Log.Debug($"玩家{message.UserID}下线");

                reply(response);
            }
            catch(Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
