using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
    [Event(EventIdCowCowType.JoinGameRoom)]
    public class CowCow_JoinGameRoomUI : AEvent<G2C_CowCowJoinGameRoomGate>
    {
        public override void Run(G2C_CowCowJoinGameRoomGate data)
        {
            UI ui = UICowCowGameRoomFactory.Create(data);
            Game.Scene.GetComponent<UIComponent>().Add(ui);
        }
    }
}
