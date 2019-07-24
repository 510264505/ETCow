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
                //从数据库拿用户数据用于大厅显示
                UserInfo userInfo = await Game.Scene.GetComponent<DBProxyComponent>().Query<UserInfo>(userId);
                //添加进用户信息管理组件，下线的时候保存并从组件中移除改用户信息
                Game.Scene.GetComponent<UserInfoComponent>().Add(userInfo);
                //向登录服务器发送玩家上线消息
                StartConfigComponent config = Game.Scene.GetComponent<StartConfigComponent>();
                IPEndPoint realmIPEndPoint = config.RealmConfig.GetComponent<InnerConfig>().IPEndPoint;
                Session realmSession = Game.Scene.GetComponent<NetInnerComponent>().Get(realmIPEndPoint);
                await realmSession.Call(new G2R_CowCowGamerOnline() { UserID = userId, GateAppID = config.StartConfig.AppId });

                response.UserID = userInfo.Id; //永久ID，DB.Id
                response.NickName = userInfo.NickName;
                response.Diamond = userInfo.Diamond;
                response.Sex = userInfo.Sex;
                response.HeadIcon = userInfo.HeadIcon;
                reply(response);
            }
            catch(Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
