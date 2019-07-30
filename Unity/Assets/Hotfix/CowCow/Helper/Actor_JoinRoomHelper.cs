using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
    public static class Actor_JoinRoomHelper
    {
        public static async ETVoid OnJoinRoomAsync(long userId, string roomId)
        {
            Session session = Game.Scene.GetComponent<SessionComponent>().Session;
            G2C_CowCowJoinGameRoomGate g2c_Join = (G2C_CowCowJoinGameRoomGate)await session.Call(new C2G_CowCowJoinGameRoomGate() { UserID = userId, RoomID = roomId });

            if (g2c_Join.Error == 0)
            {
                Game.EventSystem.Run(EventIdCowCowType.JoinGameRoom, g2c_Join);
            }
        }
    }
    [MessageHandler]
    public class Actor_CowCowJoinRoomHandler : AMHandler<Actor_CowCowJoinGameRoomGroupSend>
    {
        protected override void Run(ETModel.Session session, Actor_CowCowJoinGameRoomGroupSend message)
        {
            //这里接收到所有玩家的消息
            UICowCow_GameRoomComponent room = Game.Scene.GetComponent<UIComponent>().Get(UICowCowType.CowCowGameRoom).GetComponent<UICowCow_GameRoomComponent>();
            for (int i = 0; i < message.GamerInfo.count; i++)
            {
                int seatId = message.GamerInfo[i].SeatID - message.SeatID; //移位
                if (seatId < 0)
                {
                    room.AddGamer(message.GamerInfo[i], seatId + GamerData.Pos.Count);
                }
                else
                {
                    room.AddGamer(message.GamerInfo[i], seatId);
                }
            }
            room.ShowHideInviteButton(!(message.GamerInfo.count == GamerData.Pos.Count - 5));
        }
    }
}
