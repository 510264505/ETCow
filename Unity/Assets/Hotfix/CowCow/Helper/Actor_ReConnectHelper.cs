using ETModel;

namespace ETHotfix
{
    public static class Actor_ReConnectHelper
    {
        public static async ETVoid OnReConnect(long userId)
        {
            Session session = Game.Scene.GetComponent<SessionComponent>().Session;
            G2C_CowCowReConnect response = (G2C_CowCowReConnect)await session.Call(new C2G_CowCowReConnect()
            {
                UserID = userId,
            });

            if (response.Error == 0)
            {
                Game.EventSystem.Run(CowCowEventIdType.ReConnectInfo, response);
            }
        }
    }
}
