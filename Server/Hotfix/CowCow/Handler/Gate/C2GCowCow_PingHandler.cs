using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Gate)]
    public class C2GCowCow_PingHandler : AMRpcHandler<C2G_CowCowPing, G2C_CowCowPing>
    {
        protected override void Run(Session session, C2G_CowCowPing message, Action<G2C_CowCowPing> reply)
        {
            G2C_CowCowPing response = new G2C_CowCowPing();
            try
            {
                Game.Scene.GetComponent<PingComponent>().UpdateSession(session.Id);
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
