using System;
using System.Collections.Generic;
using System.Linq;

namespace ETModel
{
    [ObjectSystem]
    public class PingComponentAwake : AwakeSystem<PingComponent>
    {
        public override void Awake(PingComponent self)
        {
            self.Awake();
        }
    }
    public class PingComponent : Component
    {
        private long interval = 5000;
        private readonly Dictionary<long, long> _session = new Dictionary<long, long>();
        public Action<long> action;
        public async void Awake()
        {
            TimerComponent timer = Game.Scene.GetComponent<TimerComponent>();
            while (true)
            {
                try
                {
                    Log.Debug($"在线人数:{_session.Count}");
                    await timer.WaitAsync(interval);
                    //检查所有session吗，如果有时间超过指定的间隔就执行action
                    for (int i = 0; i < _session.Count; i++)
                    {
                        if (TimeHelper.ClientNowSeconds() - _session.ElementAt(i).Value > interval)
                        {
                            long key = _session.ElementAt(i).Key;
                            action?.Invoke(key);
                            _session.Remove(key);
                        }
                    }
                }
                catch (Exception e)
                {
                    Log.Error(e.Message);
                }
            }
        }
        public void AddSession(long id)
        {
            _session.Add(id, TimeHelper.ClientNowSeconds());
        }
        public void UpdateSession(long id)
        {
            if (_session.ContainsKey(id))
            {
                _session[id] = TimeHelper.ClientNowSeconds();
            }
        }
    }
}
