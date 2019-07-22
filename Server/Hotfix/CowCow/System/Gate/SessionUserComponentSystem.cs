using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class SessionUserComponentSystem : DestroySystem<SessionUserComponent>
    {
        public override async void Destroy(SessionUserComponent self)
        {
            try
            {
                //释放User对象时，将User对象从管理组件中移除
                Game.Scene.GetComponent<UserComponent>()?.Remove(self.User.UserID);
                StartConfigComponent config = Game.Scene.GetComponent<StartConfigComponent>();
                ActorMessageSenderComponent actorProxyComponent = Game.Scene.GetComponent<ActorMessageSenderComponent>();
                
                //正在匹配中发送玩家退出匹配请求
                //if (self.User.IsMatching)
                //{
                //    IPEndPoint iPEndPoint = config.LocationConfig.GetComponent<InnerConfig>().IPEndPoint;
                //    Session session = Game.Scene.GetComponent<NetInnerComponent>().Get(iPEndPoint);
                //    //await session.Call()
                //}

                //正则游戏中发送玩家退出房间请求
                if (self.User.ActorID  != 0)
                {
                    ActorMessageSender actorProxy = actorProxyComponent.Get(self.User.ActorID);
                    //await actorProxy.Call()
                }

                //向登录服务器发送玩家下线消息
                IPEndPoint realmIPEndPoint = config.RealmConfig.GetComponent<InnerConfig>().IPEndPoint;
                Session realmSession = Game.Scene.GetComponent<NetInnerComponent>().Get(realmIPEndPoint);
                //await realmSession.Call()

                self.User.Dispose();
                self.User = null;
            }
            catch (Exception e)
            {
                Log.Trace(e.ToString());
            }
        }
    }
}
