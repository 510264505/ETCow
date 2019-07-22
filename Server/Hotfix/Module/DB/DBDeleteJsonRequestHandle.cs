using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.DB)]
    public class DBDeleteJsonRequestHandle : AMRpcHandler<DBDeleteRequest, DBDeleteResponse>
    {
        protected override void RunAsync(Session session, DBDeleteRequest message, Action<DBDeleteResponse> reply)
        {
            RunAsync(session, message, reply).Coroutine();
        }
        protected async ETVoid RunAsync(Session session, DBDeleteRequest message, Action<DBDeleteResponse> reply)
        {
            DBDeleteResponse response = new DBDeleteResponse();
            try
            {
                await Game.Scene.GetComponent<DBComponent>().DelJson(message.CollectionName, message.Json);
                reply(response);
            }
            catch(Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
