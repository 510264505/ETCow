using ETModel;

namespace ETHotfix
{
	[Event(EventIdCowCowType.RemoveLobby)]
	public class EnterCowCowGameFinish_RemoveLobbyUI: AEvent
	{
		public override void Run()
		{
			Game.Scene.GetComponent<UIComponent>().Remove(UICowCowType.CowCowLobby);
			ETModel.Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle(UICowCowAB.CowCow_Prefabs.StringToAB());
            ETModel.Game.Scene.GetComponent<ClientComponent>().User.Dispose();
        }
	}
}
