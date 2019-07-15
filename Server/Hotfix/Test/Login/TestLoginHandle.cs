using System;
using ETModel;
using MongoDB.Bson;

namespace ETHotfix
{
    [MessageHandler(AppType.AllServer)]
    public class TestLoginHandle : AMRpcHandler<C2G_TestPlayerInfo, G2C_TestPlayerInfo>
    {
        protected override async void Run(Session session, C2G_TestPlayerInfo message, Action<G2C_TestPlayerInfo> reply)
        {
            G2C_TestPlayerInfo g2C_TestPlayerInfo = new G2C_TestPlayerInfo();
            try
            {
                BsonDocument bsons = new BsonDocument();
                bsons["Account"] = message.Account;
                //注册成功后，添加到数据库
                DBProxyComponent db = Game.Scene.GetComponent<DBProxyComponent>();
                string s = bsons.ToJson();
                Log.WriteLine("查询这个Bson:" + s);
                //var account = await db.Query<Accounts>(s);
                //if (account.Count > 0)
                //{
                //    g2C_TestPlayerInfo.Error = -1;
                //    g2C_TestPlayerInfo.Message = "用户名已存在！";
                //}
                //else
                //{
                //    Log.WriteLine("UserName:" + message.Account);
                //    Log.WriteLine("Password:" + message.Password);

                //    Accounts accounts = ComponentFactory.Create<Accounts>();
                //    accounts.Account = message.Account;
                //    accounts.Password = message.Password;

                //    await db.Save(accounts);

                //    g2C_TestPlayerInfo.Error = 0;
                //    g2C_TestPlayerInfo.Message = "恭喜您注册成功";
                //}
                //await db.Delete<Accounts>(s);
                //BsonDocument bsons1 = new BsonDocument();
                //bsons1["Password"] = message.Password;
                //await db.QueryToUpdate<Accounts>(bsons, bsons1);
            }
            catch(Exception e)
            {
                g2C_TestPlayerInfo.Error = -1;
                g2C_TestPlayerInfo.Message = "注册失败";
                ReplyError(g2C_TestPlayerInfo, e, reply);
            }
            reply(g2C_TestPlayerInfo);
        }
    }
}
