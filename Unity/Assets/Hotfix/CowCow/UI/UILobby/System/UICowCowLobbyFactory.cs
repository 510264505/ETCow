using System;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    public static class UICowCowLobbyFactory
    {
        public static UI Create(G2C_CowCowLoginGate data)
        {
	        try
	        {
				ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
		        resourcesComponent.LoadBundle(UICowCowAB.CowCow_Prefabs);
				GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset(UICowCowAB.CowCow_Prefabs, UICowCowType.CowCowLobby);
				GameObject gameObject = UnityEngine.Object.Instantiate(bundleGameObject);
		        UI ui = ComponentFactory.Create<UI, string, GameObject>(UICowCowType.CowCowLobby, gameObject, false);

				ui.AddComponent<UICowCowLobbyComponent, G2C_CowCowLoginGate>(data);
				return ui;
	        }
	        catch (Exception e)
	        {
				Log.Error(e);
		        return null;
	        }
		}
    }
}