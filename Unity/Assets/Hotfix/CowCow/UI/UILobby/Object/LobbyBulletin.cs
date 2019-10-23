using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    public class LobbyBulletin
    {
        private CanvasGroup parent;
        public LobbyBulletin(GameObject parent)
        {
            ResourcesComponent res = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
            GameObject go = UnityEngine.Object.Instantiate((GameObject)res.GetAsset(UICowCowAB.CowCow_Prefabs, UICowCowType.CowCowLobbyBulletin));
            go.transform.SetParent(parent.transform, false);
            this.parent = go.GetComponent<CanvasGroup>();
            Button closeBtn = this.parent.transform.Find("CloseBtn").GetComponent<Button>();
            closeBtn.onClick.Add(() => this.ShowHideLobbyServer(false));
        }

        public void ShowHideLobbyServer(bool isShow)
        {
            this.parent.alpha = isShow ? 1 : 0;
            this.parent.blocksRaycasts = isShow;
        }

        public void Destroy()
        {
            UnityEngine.Object.Destroy(this.parent.gameObject);
        }
    }
}
