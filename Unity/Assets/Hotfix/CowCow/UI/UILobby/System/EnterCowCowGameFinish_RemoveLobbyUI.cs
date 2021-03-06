﻿using ETModel;

namespace ETHotfix
{
	[Event(CowCowEventIdType.RemoveLobby)]
	public class EnterCowCowGameFinish_RemoveLobbyUI: AEvent
	{
		public override void Run()
		{
			Game.Scene.GetComponent<UIComponent>().Remove(UICowCowType.CowCowLobby);
			ETModel.Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle(UICowCowAB.CowCow_Prefabs);
        }
	}
}
