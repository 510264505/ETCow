using ETModel;

namespace ETHotfix
{
    public static class Actor_CreateRoomHelper
    {
        public static async ETVoid OnCreateGameRoom(string gameName, long userId, int bureau, int ruleBit, int peopleCount)
        {
            Session session = Game.Scene.GetComponent<SessionComponent>().Session;
            G2C_CowCowCreateGameRoomGate g2c_Create = (G2C_CowCowCreateGameRoomGate)await session.Call(new C2G_CowCowCreateGameRoomGate()
            {
                Name = gameName,
                UserID = userId,
                Bureau = bureau,
                RuleBit = ruleBit,
                People = peopleCount,
            });

            if (g2c_Create.Error == 0)
            {
                Game.EventSystem.Run(CowCowEventIdType.CreateGameRoom, g2c_Create);
            }
        }
    }
}
