using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    [ObjectSystem]
    public class PingComponentAwake : AwakeSystem<PingComponent>
    {
        public override void Awake(PingComponent self)
        {
            self.Awake().Coroutine();
        }
    }
    [ObjectSystem]
    public class PingComponentUpdate : UpdateSystem<PingComponent>
    {
        public override void Update(PingComponent self)
        {
            self.Update();
        }
    }

    /// <summary>
    /// 心跳组件
    /// </summary>
    public class PingComponent : Component
    {
        /// <summary>
        /// 发送时间
        /// </summary>
        private long _sendTimer;
        /// <summary>
        /// 接收时间
        /// </summary>
        private long _receviceTimer;
        /// <summary>
        /// 延迟
        /// </summary>
        private long ping = 0;
        /// <summary>
        /// 间隔
        /// </summary>
        private long interval = 5000;
        /// <summary>
        /// 心跳协议包
        /// </summary>
        private readonly C2G_CowCowPing _request = new C2G_CowCowPing();
        /// <summary>
        /// 是否开始心跳
        /// </summary>
        private bool isStart = false;

        public async ETVoid Awake()
        {
            TimerComponent tiemr = ETModel.Game.Scene.GetComponent<TimerComponent>();
            Session session = this.GetParent<Session>();
            this.isStart = true;
            int connectCount = 0;
            while (isStart)
            {
                try
                {
                    //服务器太久没有接收到心跳，会踢掉客户端
                    _sendTimer = TimeHelper.ClientNowSeconds();
                    await session.Call(_request);
                    _receviceTimer = TimeHelper.ClientNowSeconds();
                    long time = (_receviceTimer - _sendTimer) / 2;
                    ping = time < 0 ? -1 : time;
                    if (ping == -1)
                    {
                        this.ReConnect(connectCount < 2);
                        connectCount++;
                        Log.Debug("Ping断线了");
                    }
                }
                catch (Exception e)
                {
                    //重连两次，或者弹窗跳到登录框界面
                    this.ReConnect(connectCount < 2);
                    connectCount++;
                    Log.Debug($"Ping异常(在这里重连):{e.Message}");
                }
                await tiemr.WaitAsync(interval);
            }
        }

        public void Update()
        {
            if (isStart)
            {
                if (Application.internetReachability == NetworkReachability.NotReachable)//没有联网 
                {
                    //弹窗提示查看网络状态，这里出现了，说明无网络。
                    Log.Debug("掉线的一瞬间了");
                    this.isStart = false;
                    Game.EventSystem.Run(CowCowEventIdType.TCPReConnect); //触发断线
                }
            }
        }

        private void ReConnect(bool isRe)
        {
            if (isRe)
            {
                Actor_LoginHelper.ReConnect().Coroutine();
            }
            else
            {
                //弹窗
                PopupsHelper.ShowPopups("连接失败，请重新登录", () => {
                    Game.EventSystem.Run(CowCowEventIdType.RemoveGameRoom);
                    Game.EventSystem.Run(CowCowEventIdType.RemoveLobby);
                    Game.Scene.GetComponent<SessionComponent>().Session.RemoveComponent<PingComponent>();
                    Game.Scene.GetComponent<SessionComponent>().Session.Dispose();
                });
            }
        }

        public string NetState()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)//没有联网 
            {
                return "断线";
            }
            else if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)//连接的移动网络 
            {
                return "4G";
            }
            else if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)//连接的Wifi 
            {
                return "WIFI";
            }
            return "";
        }

        public override void Dispose()
        {
            //被服务器踢掉，直接退到登录框
            //在这里跳转到登录界面的事件
            Game.EventSystem.Run(CowCowEventIdType.InitScensStart);
            Log.Debug($"销毁:{this.IsDisposed}");
            if (this.IsDisposed)
            {
                return;
            }
            base.Dispose();

            this.isStart = false;
        }
    }
}
