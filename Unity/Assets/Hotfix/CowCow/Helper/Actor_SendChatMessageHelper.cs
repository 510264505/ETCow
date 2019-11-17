using ETModel;

namespace ETHotfix
{
    public static class Actor_SendChatMessageHelper
    {
        public static async ETVoid SendFont(int index, int seatId, string msg = null)
        {
            Session session = Game.Scene.GetComponent<SessionComponent>().Session;
            G2C_CowCowChatFont response = (G2C_CowCowChatFont)await session.Call(new C2G_CowCowChatFont() { ChatIndex = index, SeatID = seatId, ChatMessage = msg, UserID = ClientComponent.Instance.User.UserID });
            if (response.Error != 0)
            {
                //发送失败弹窗
            }
        }
        public static async ETVoid SendEmoji(int index, int seatId)
        {
            Session session = Game.Scene.GetComponent<SessionComponent>().Session;
            G2C_CowCowEmoji response = (G2C_CowCowEmoji)await session.Call(new C2G_CowCowEmoji() { Index = index, SeatID = seatId, UserID = ClientComponent.Instance.User.UserID });
            if (response.Error != 0)
            {
                //发送失败弹窗
            }
        }
    }
}
