using System;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Realm)]
    public class C2RCowCow_RegisterHandler : AMRpcHandler<C2R_CowCowRegister, R2C_CowCowRegister>
    {
        protected override async void Run(Session session, C2R_CowCowRegister message, Action<R2C_CowCowRegister> reply)
        {
            R2C_CowCowRegister response = new R2C_CowCowRegister();
            try
            {
                DBProxyComponent db = Game.Scene.GetComponent<DBProxyComponent>();
                
                //查询账号是否存在
                var accounts = await db.Query<Accounts>(_account => _account.Account == message.Account);
                if (accounts.Count > 0)
                {
                    response.Error = ErrorCode.ERR_AccountAlreadyRegister;
                    reply(response);
                    return;
                }

                //新建账号，之后如果操作用户信息，通过唯一ID查找并修改或删除等
                Accounts newAccounts = ComponentFactory.CreateWithId<Accounts>(IdGenerater.GenerateId());
                newAccounts.Account = message.Account;
                newAccounts.Password = message.Password;
                newAccounts.LoginTime = DateTime.Now;

                Log.WriteLine($"注册新账号:{newAccounts.Account},密码:{message.Password}");

                //新建用户信息
                UserInfo newUser = ComponentFactory.CreateWithId<UserInfo>(newAccounts.Id);
                newUser.NickName = $"用户{message.Account}";
                newUser.Sex = 0;
                newUser.Diamond = 1000000;
                newUser.RegisterTime = DateTime.Now;

                await db.Save(newAccounts);
                await db.Save(newUser);

                await RealmHelper.KickOut(newAccounts.Id);

                //随机分配网管服务器
                StartConfig gateConfig = Game.Scene.GetComponent<RealmGateAddressComponent>().GetAddress();
                Session gateSession = Game.Scene.GetComponent<NetInnerComponent>().Get(gateConfig.GetComponent<InnerConfig>().IPEndPoint);

                //请求登录gate服务器密匙
                G2R_CowCowGetLoginKey g2R_GetLoginKey = (G2R_CowCowGetLoginKey)await gateSession.Call(new R2G_CowCowGetLoginKey() { UserID = newAccounts.Id });

                response.Key = g2R_GetLoginKey.Key;
                response.Address = gateConfig.GetComponent<OuterConfig>().Address2;
                response.Error = 0;
                response.Message = "登录成功";
                reply(response);
            }
            catch(Exception e)
            {
                response.Error = ErrorCode.ERR_AccountAlreadyRegister;
                response.Message = "注册失败！";
                ReplyError(response, e, reply);
            }
        }
    }
}
