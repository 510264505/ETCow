using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
    public static class Actor_DissoltionHelper
    {
        public async static ETVoid OnSendVOte(int seatId, bool isAgree)
        {
            Session session = Game.Scene.GetComponent<SessionComponent>().Session;
            G2C_CowCowDissoltion response = (G2C_CowCowDissoltion)await session.Call(new C2G_CowCowDissoltion() { SeatID = seatId, IsAgree = isAgree });
            if (response.Error == 0)
            {
                UICowCow_DissoltionComponent diss = Game.Scene.GetComponent<UIComponent>().Get(UICowCowType.CowCowGameRoom).GetComponent<UICowCow_DissoltionComponent>();
                diss.ShowOtherVote(seatId, isAgree ? "同意" : "不同意");
            }
        }
    }
}
