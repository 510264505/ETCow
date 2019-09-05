using System;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    public static class UICowCowLoginFactory
    {
        public static UI Create()
        {
	        try
	        {
				ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
				resourcesComponent.LoadBundle(UICowCowAB.CowCow_Prefabs);
				GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset(UICowCowAB.CowCow_Prefabs, UICowCowType.CowCowLogin);
				GameObject gameObject = UnityEngine.Object.Instantiate(bundleGameObject);

		        UI ui = ComponentFactory.Create<UI, string, GameObject>(UICowCowType.CowCowLogin, gameObject, false);

				ui.AddComponent<UICowCowLoginComponent>();
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