using System;
using ETModel;
using MongoDB.Bson;

namespace ETHotfix
{
    [MessageHandler(AppType.Gate)]
    public class C2GCowCow_RefreshDiamondHandler : AMRpcHandler<C2G_CowCowRefreshGate, G2C_CowCowRefreshGate>
    {
        protected override async void Run(Session session, C2G_CowCowRefreshGate message, Action<G2C_CowCowRefreshGate> reply)
        {
            G2C_CowCowRefreshGate response = new G2C_CowCowRefreshGate();
            try
            {
                BsonDocument bsons = new BsonDocument();
                bsons["_id"] = message.UserID;
                DBProxyComponent db = Game.Scene.GetComponent<DBProxyComponent>();
                var userInfo = await db.Query<UserInfo>(bsons.ToJson());
                if (userInfo.Count > 0)
                {
                    UserInfo userInfo1 = (UserInfo)userInfo[0];
                    response.Diamond = userInfo1.Diamond;
                }

                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
