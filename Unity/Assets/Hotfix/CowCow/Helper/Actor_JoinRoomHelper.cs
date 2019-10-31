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
                Game.EventSystem.Run(CowCowEventIdType.JoinGameRoom, g2c_Join);
            }
            else
            {
                //弹窗显示异常消息
            }
        }
    }
    [MessageHandler]
    public class Actor_CowCowJoinRoomHandler : AMHandler<Actor_CowCowJoinGameRoomGroupSend>
    {
        protected override void Run(ETModel.Session session, Actor_CowCowJoinGameRoomGroupSend message)
        {
            //这里接收到所有玩家的消息，做处理显示新加入房间的玩家
            UICowCow_GameRoomComponent room = Game.Scene.GetComponent<UIComponent>().Get(UICowCowType.CowCowGameRoom).GetComponent<UICowCow_GameRoomComponent>();
            room.AddLocalGamer(message.LocalGamerInfo);
            for (int i = 0; i < message.GamerInfo.count; i++)
            {
                int seatId = message.GamerInfo[i].SeatID - room.GamerComponent.LocalSeatID; //移位
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
