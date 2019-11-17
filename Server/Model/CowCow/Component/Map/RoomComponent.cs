using System.Collections.Generic;

namespace ETModel
{
    /// <summary>
    /// 房间管理组件
    /// </summary>
    public class RoomComponent : Component
    {
        public readonly Dictionary<string, Room> rooms = new Dictionary<string, Room>();
        public readonly Dictionary<long, string> roomIds = new Dictionary<long, string>();
        
        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }
            base.Dispose();

            foreach (Room room in this.rooms.Values)
            {
                room.Dispose();
            }
            this.rooms.Clear();
        }
    }
}
