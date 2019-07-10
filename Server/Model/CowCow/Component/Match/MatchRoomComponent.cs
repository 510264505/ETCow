using System;
using System.Collections.Generic;
using System.Text;

namespace ETModel
{
    /// <summary>
    /// 匹配房间管理组件，逻辑在MatchRoomComponentSystem扩展
    /// </summary>
    public class MatchRoomComponent : Component
    {
        //所有房间列表
        public readonly Dictionary<long, Room> rooms = new Dictionary<long, Room>();
        //游戏中房间列表
        public readonly Dictionary<long, Room> gameRooms = new Dictionary<long, Room>();
        //等待中房间列表
        public readonly Dictionary<long, Room> readyRooms = new Dictionary<long, Room>();
        //房间总数
        public int TotalCount { get { return this.rooms.Count; } }
        //游戏中房间数
        public int GameRoomCount { get { return this.gameRooms.Count; } }
        //等待中房间数
        public int ReadyRoomCount { get { return this.readyRooms.Count; } }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }
            base.Dispose();

            foreach (var room in this.rooms.Values)
            {
                room.Dispose();
            }
        }
    }
}
