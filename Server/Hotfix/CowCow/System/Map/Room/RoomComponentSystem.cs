using System.Collections.Generic;
using ETModel;

namespace ETHotfix
{
    public static class RoomComponentSystem
    {
        public static void Add(this RoomComponent self, Room room)
        {
            if (!self.rooms.ContainsKey(room.RoomID))
            {
                self.rooms.Add(room.RoomID, room);
            }
            else
            {
                throw new System.Exception("RoomID Already Exist");
            }
        }

        public static Room Get(this RoomComponent self, string id)
        {
            Room room;
            self.rooms.TryGetValue(id, out room);
            return room;
        }

        public static bool IsExist(this RoomComponent self, string id)
        {
            return self.rooms.ContainsKey(id);
        }
        public static Room Remove(this RoomComponent self, string id)
        {
            Room room = self.Get(id);
            self.rooms.Remove(id);
            Dictionary<int, Gamer> gamers = room.GetAll();
            foreach (var gamer in gamers.Values)
            {
                self.Remove(gamer.UserID);
            }
            return room;
        }

        public static void Add(this RoomComponent self, long id, string roomId)
        {
            if (!self.roomIds.ContainsKey(id))
            {
                self.roomIds.Add(id, roomId);
            }
            else
            {
                self.roomIds[id] = roomId;
            }
        }
        public static Room Get(this RoomComponent self, long id)
        {
            Room room;
            if (self.roomIds.TryGetValue(id, out string roomId))
            {
                self.rooms.TryGetValue(roomId, out room);
                return room;
            }
            return null;
        }
        public static string Remove(this RoomComponent self, long id)
        {
            string roomId = self.roomIds[id];
            self.roomIds.Remove(id);
            return roomId;
        }
    }
}
