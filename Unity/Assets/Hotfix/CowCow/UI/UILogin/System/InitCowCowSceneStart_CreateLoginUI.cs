using ETModel;

namespace ETHotfix
{
	[Event(EventIdCowCowType.InitScensStart)]
	public class InitCowCowSceneStart_CreateLoginUI: AEvent
	{
		public override void Run()
		{
			UI ui = UICowCowLoginFactory.Create();
			Game.Scene.GetComponent<UIComponent>().Add(ui);
		}
	}
}
