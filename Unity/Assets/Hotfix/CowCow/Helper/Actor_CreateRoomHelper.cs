using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
    public static class Actor_CreateRoomHelper
    {
        public static async ETVoid OnCreateGameRoom(string gameName, long userId, int bureau, int ruleBit)
        {
            Session session = Game.Scene.GetComponent<SessionComponent>().Session;
            G2C_CowCowCreateGameRoomGate g2c_Create = (G2C_CowCowCreateGameRoomGate)await session.Call(new C2G_CowCowCreateGameRoomGate()
            {
                Name = gameName,
                UserID = userId,
                Bureau = bureau,
                RuleBit = ruleBit
            });

            if (g2c_Create.Error == 0)
            {
                Game.EventSystem.Run(EventIdCowCowType.CreateGameRoom, g2c_Create);
            }
        }
    }
}
