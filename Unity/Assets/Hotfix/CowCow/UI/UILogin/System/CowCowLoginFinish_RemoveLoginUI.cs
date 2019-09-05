using ETModel;

namespace ETHotfix
{
	[Event(EventIdCowCowType.LoginFinish)]
	public class CowCowLoginFinish_RemoveLoginUI : AEvent<G2C_CowCowLoginGate>
	{
		public override void Run(G2C_CowCowLoginGate data)
		{
			Game.Scene.GetComponent<UIComponent>().Remove(UICowCowType.CowCowLogin);
			ETModel.Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle(UICowCowAB.CowCow_Prefabs);
		}
	}
}
