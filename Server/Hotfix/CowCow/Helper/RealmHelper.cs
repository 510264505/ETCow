using ETModel;
using System.Net;

namespace ETHotfix
{
    public static class RealmHelper
    {
        /// <summary>
        /// 踢出服务器
        /// </summary>
        public static async ETTask KickOut(long userId)
        {
            int gateAppId = Game.Scene.GetComponent<OnlineComponent>().Get(userId);
            if (gateAppId != 0)
            {
                StartConfig userConfig = Game.Scene.GetComponent<StartConfigComponent>().Get(gateAppId);
                IPEndPoint userGateIPEndPoint = userConfig.GetComponent<InnerConfig>().IPEndPoint;
                Session userGateSession = Game.Scene.GetComponent<NetInnerComponent>().Get(userGateIPEndPoint);
                await userGateSession.Call(new R2G_CowCowGamerKickOut() { UserID = userId });
                Log.Debug($"玩家{userId}已被踢出登录服务器！");
            }
        }
    }
}
