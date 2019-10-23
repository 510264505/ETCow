using ETModel;

namespace ETHotfix
{
	[Event(CowCowEventIdType.InitScensStart)]
	public class InitCowCowSceneStart_CreateLoginUI: AEvent
	{
		public override void Run()
		{
			UI ui = UICowCowLoginFactory.Create();
			Game.Scene.GetComponent<UIComponent>().Add(ui);
		}
	}
}
