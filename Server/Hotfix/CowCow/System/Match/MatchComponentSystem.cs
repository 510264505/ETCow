using ETModel;
using System.Collections.Generic;
using System.Net;

namespace ETHotfix
{
    public static class MatchRoomComponentSystem
    {
        public static async void CreateRoom(this MatchRoomComponent self)
        {
            IPEndPoint roomIPEndPoint = Game.Scene.GetComponent<AllotRoomComponent>().GetAddress().GetComponent<InnerConfig>().IPEndPoint;
            Session session = Game.Scene.GetComponent<NetInnerComponent>().Get(roomIPEndPoint);

            //接收到客户端的创建消息，后发送消息给客户端

            Room room = ComponentFactory.CreateWithId<Room>(123456); //房间号ID
            Game.Scene.GetComponent<MatchRoomComponent>().Add(room);
        }


        /// <summary>
        /// 添加创建了的房间
        /// </summary>
        public static void Add(this MatchRoomComponent self, Room room)
        {
            room.State = RoomState.Wait;
            self.rooms.Add(room.Id, room);
            self.waitRooms.Add(room.Id, room);
        }

        /// <summary>
        /// 获取单个等待中的房间
        /// </summary>
        public static Room GetCreateingRoom(this MatchRoomComponent self, long id)
        {
            if (self.waitRooms.TryGetValue(id, out Room room))
            {
                return room;
            }
            return null;
        }

        /// <summary>
        /// 获取游戏中的房间
        /// </summary>
        public static Room GetGameRoom(this MatchRoomComponent self, long id)
        {
            if (self.gameRooms.TryGetValue(id, out Room room))
            {
                return room;
            }
            return null;
        }
    }
}
