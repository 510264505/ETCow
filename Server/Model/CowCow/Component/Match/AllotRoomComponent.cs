using System.Collections.Generic;

namespace ETModel
{
    /// <summary>
    /// 分配房间服务器组件，逻辑在AllotRoomComponentSystem扩展
    /// </summary>
    public class AllotRoomComponent : Component
    {
        public readonly List<StartConfig> roomAddress = new List<StartConfig>();
    }
}
