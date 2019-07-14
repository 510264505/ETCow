using System;
using ETModel;
using MongoDB.Bson;

namespace ETHotfix
{
    [MessageHandler(AppType.Realm)]
    public class C2RCowCow_LoginHandler : AMRpcHandler<C2R_CowCowLogin, R2C_CowCowLogin>
    {
        protected override async void Run(Session session, C2R_CowCowLogin message, Action<R2C_CowCowLogin> reply)
        {
            R2C_CowCowLogin response = new R2C_CowCowLogin();
            try
            {
                //BsonDocument bsons = new BsonDocument();
                //bsons["Account"] = message.Account;
                //string s = bsons.ToJson();
                //Log.WriteLine("查询这个Bson:" + s);
                //注册成功后，添加到数据库
                DBProxyComponent db = Game.Scene.GetComponent<DBProxyComponent>();
                var account = await db.Query<Accounts>(_account => _account.Account == message.Account && _account.Password == message.Password);
                if (account.Count == 0)
                {
                    response.Error = ErrorCode.ERR_LoginError;
                    response.Message = "用户名不存在！";
                    reply(response);
                    return;
                }
                Log.WriteLine($"UserName:{message.Account},Password:{message.Password}");

                Accounts accounts = (Accounts)account[0];
                //将已在线玩家踢下线
                await RealmHelper.KickOut(accounts.Id);

                //随机分配网管服务器
                StartConfig gateConfig = Game.Scene.GetComponent<RealmGateAddressComponent>().GetAddress();
                Session gateSession = Game.Scene.GetComponent<NetInnerComponent>().Get(gateConfig.GetComponent<InnerConfig>().IPEndPoint);

                //请求登录gate服务器密匙
                G2R_CowCowGetLoginKey g2R_GetLoginKey = (G2R_CowCowGetLoginKey)await gateSession.Call(new R2G_CowCowGetLoginKey() { UserID = accounts.Id });

                response.Key = g2R_GetLoginKey.Key;
                response.Address = gateConfig.GetComponent<OuterConfig>().Address2;
                response.Error = 0;
                response.Message = "恭喜您登录成功";
                reply(response);
            }
            catch (Exception e)
            {
                response.Error = ErrorCode.ERR_LoginError;
                response.Message = "登录失败！";
                ReplyError(response, e, reply);
            }
        }
    }
}
