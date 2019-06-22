using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.DB)]
    public class DBUpdateJsonRequestHandle : AMRpcHandler<DBUpdateJsonRequest, DBUpdateJsonResponse>
    {
        protected override void Run(Session session, DBUpdateJsonRequest message, Action<DBUpdateJsonResponse> reply)
        {
            RunAsync(session, message, reply).Coroutine();
        }
        protected async ETVoid RunAsync(Session session, DBUpdateJsonRequest message, Action<DBUpdateJsonResponse> reply)
        {
            DBUpdateJsonResponse response = new DBUpdateJsonResponse();
            try
            {
                await Game.Scene.GetComponent<DBComponent>().UpdJson(message.CollectionName, message.Json);
                reply(response);
            }
            catch(Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
