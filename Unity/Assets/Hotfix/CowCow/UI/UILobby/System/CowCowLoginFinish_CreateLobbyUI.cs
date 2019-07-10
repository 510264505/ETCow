using ETModel;

namespace ETHotfix
{
	[Event(EventIdCowCowType.LoginFinish)]
	public class CowCowLoginFinish_CreateLobbyUI : AEvent<G2C_CowCowLoginGate>
	{
		public override void Run(G2C_CowCowLoginGate data)
		{
            UI ui = UICowCowLobbyFactory.Create(data);
            Game.Scene.GetComponent<UIComponent>().Add(ui);
        }
	}
}
