using System;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Gate)]
    public class C2GCowCow_EmojiMessageHandler : AMRpcHandler<C2G_CowCowEmoji, G2C_CowCowEmoji>
    {
        protected override void Run(Session session, C2G_CowCowEmoji message, Action<G2C_CowCowEmoji> reply)
        {
            G2C_CowCowEmoji response = new G2C_CowCowEmoji();
            try
            {
                Room room = Game.Scene.GetComponent<RoomComponent>().Get(message.UserID);
                Actor_CowCowEmoji emojiMessage = new Actor_CowCowEmoji();
                emojiMessage.Index = message.Index;
                emojiMessage.SeatID = message.SeatID;

                reply(response);
                room.Broadcast(emojiMessage);
            }
            catch(Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
