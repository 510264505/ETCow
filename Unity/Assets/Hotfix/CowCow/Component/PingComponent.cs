using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class PingComponentAwake : AwakeSystem<PingComponent>
    {
        public override void Awake(PingComponent self)
        {
            self.Awake();
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
        /// 断线执行的事件
        /// </summary>
        public Action action;
        /// <summary>
        /// 心跳协议包
        /// </summary>
        private readonly C2G_CowCowPing _request = new C2G_CowCowPing();

        public async void Awake()
        {
            TimerComponent tiemr = ETModel.Game.Scene.GetComponent<TimerComponent>();
            Session session = this.GetParent<Session>();
            while (true)
            {
                try
                {
                    _sendTimer = TimeHelper.ClientNowSeconds();
                    await session.Call(_request);
                    _receviceTimer = TimeHelper.ClientNowSeconds();
                    long time = (_receviceTimer - _sendTimer) / 2;
                    ping = time < 0 ? 0 : time;
                    if (ping == 0)
                    {
                        Log.Debug("断线了");
                        action?.Invoke();
                    }
                }
                catch(Exception e)
                {
                    Log.Debug("断线了");
                    action?.Invoke();
                }
                await tiemr.WaitAsync(interval);
            }
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }
            base.Dispose();
        }
    }
}
