using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Gate)]
    public class R2GCowCow_GetLoginKeyHandler : AMRpcHandler<R2G_CowCowGetLoginKey, G2R_CowCowGetLoginKey>
    {
        protected override void RunAsync(Session session, R2G_CowCowGetLoginKey message, Action<G2R_CowCowGetLoginKey> reply)
        {
            G2R_CowCowGetLoginKey response = new G2R_CowCowGetLoginKey();
            try
            {
                long key = RandomHelper.RandInt64();
                Game.Scene.GetComponent<CowCowGateSessionKeyComponent>().Add(key, message.UserID);
                response.Key = key;
                reply(response);
            }
            catch(Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
