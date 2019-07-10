using System;
using System.Collections.Generic;
using System.Text;

namespace ETModel
{
    /// <summary>
    /// 在线组件，记录在线玩家
    /// </summary>
    public class OnlineComponent : Component
    {
        private readonly Dictionary<long, int> dict = new Dictionary<long, int>();

        public void Add(long userId, int gateAppId)
        {
            dict.Add(userId, gateAppId);
        }
        public int Get(long userId)
        {
            int gateAppId;
            dict.TryGetValue(userId, out gateAppId);
            return gateAppId;
        }
        public void Remove(long userId)
        {
            dict.Remove(userId);
        }
    }
}
