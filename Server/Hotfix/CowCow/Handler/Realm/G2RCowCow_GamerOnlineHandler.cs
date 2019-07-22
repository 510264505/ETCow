using System;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Realm)]
    public class G2RCowCow_GamerOnlineHandler : AMRpcHandler<G2R_CowCowGamerOnline, R2G_CowCowGamerOnline>
    {
        protected override async void RunAsync(Session session, G2R_CowCowGamerOnline message, Action<R2G_CowCowGamerOnline> reply)
        {
            R2G_CowCowGamerOnline response = new R2G_CowCowGamerOnline();
            try
            {
                OnlineComponent onlineComponent = Game.Scene.GetComponent<OnlineComponent>();

                //将玩家踢下线
                await RealmHelper.KickOut(message.UserID);

                //玩家上线
                onlineComponent.Add(message.UserID, message.GateAppID);
                Log.Debug($"玩家{message.UserID}上线");

                reply(response);
            }
            catch(Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
