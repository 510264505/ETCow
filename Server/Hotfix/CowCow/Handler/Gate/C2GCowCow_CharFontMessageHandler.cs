using System;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Gate)]
    public class C2GCowCow_CharFontMessageHandler : AMRpcHandler<C2G_CowCowChatFont, G2C_CowCowChatFont>
    {
        protected override void Run(Session session, C2G_CowCowChatFont message, Action<G2C_CowCowChatFont> reply)
        {
            G2C_CowCowChatFont response = new G2C_CowCowChatFont();
            try
            {
                Room room = Game.Scene.GetComponent<RoomComponent>().Get(message.UserID);
                Actor_CowCowChatFont chatMessage = new Actor_CowCowChatFont();
                chatMessage.SeatID = message.SeatID;
                chatMessage.ChatIndex = message.ChatIndex;
                chatMessage.ChatMessage = message.ChatMessage;

                reply(response);
                room.Broadcast(chatMessage);
            }
            catch(Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
