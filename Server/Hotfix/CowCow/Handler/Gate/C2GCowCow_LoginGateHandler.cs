using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Gate)]
    public class C2GCowCow_LoginGateHandler : AMRpcHandler<C2G_CowCowLoginGate, G2C_CowCowLoginGate>
    {
        protected override async void Run(Session session, C2G_CowCowLoginGate message, Action<G2C_CowCowLoginGate> reply)
        {
            G2C_CowCowLoginGate response = new G2C_CowCowLoginGate();
            try
            {
                CowCowGateSessionKeyComponent cowCowGateSessionKeyComponent = Game.Scene.GetComponent<CowCowGateSessionKeyComponent>();
                long userId = cowCowGateSessionKeyComponent.Get(message.Key);

                //验证key
                if (userId == 0)
                {
                    response.Error = ErrorCode.ERR_ConnectGateKeyError;
                    reply(response);
                    return;
                }

                //过期
                cowCowGateSessionKeyComponent.Remove(message.Key);

                //创建对象
                User user = UserFactory.Create(userId, session.InstanceId);
                await user.AddComponent<MailBoxComponent>().AddLocation();

                //添加对象关联到session上
                session.AddComponent<SessionUserComponent>().User = user;
                //添加消息转发组件
                await session.AddComponent<MailBoxComponent, string>(ActorInterceptType.GateSession).AddLocation();

                //向登录服务器发送玩家上线消息
                StartConfigComponent config = Game.Scene.GetComponent<StartConfigComponent>();
                IPEndPoint realmIPEndPoint = config.RealmConfig.GetComponent<InnerConfig>().IPEndPoint;
                Session realmSession = Game.Scene.GetComponent<NetInnerComponent>().Get(realmIPEndPoint);
                await realmSession.Call(new G2R_CowCowGamerOnline() { UserID = userId, GateAppID = config.StartConfig.AppId });

                response.PlayerId = user.InstanceId;
                response.GamerInfo = new GamerInfo();
                response.GamerInfo.UserID = user.UserID;
                reply(response);
            }
            catch(Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
