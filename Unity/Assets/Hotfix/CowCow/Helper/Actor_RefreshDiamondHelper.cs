using ETModel;

namespace ETHotfix
{
    public static class Actor_RefreshDiamondHelper
    {
        public static async ETVoid OnRefreshDiamond()
        {
            Session session = Game.Scene.GetComponent<SessionComponent>().Session;
            G2C_CowCowRefreshGate g2c_Refresh = (G2C_CowCowRefreshGate)await session.Call(new C2G_CowCowRefreshGate()
            {
                UserID = ClientComponent.Instance.User.UserID,
            });

            if (g2c_Refresh.Error == 0)
            {
                //刷新房卡
                UICowCowLobbyComponent lobby = Game.Scene.GetComponent<UIComponent>().Get(UICowCowType.CowCowLobby).GetComponent<UICowCowLobbyComponent>();
                lobby.SetDiamond(g2c_Refresh.Diamond);
            }
        }
    }
}

