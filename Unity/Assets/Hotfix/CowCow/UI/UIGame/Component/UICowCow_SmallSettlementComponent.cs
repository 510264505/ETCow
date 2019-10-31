using ETModel;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace ETHotfix
{
    [ObjectSystem]
    public class UICowCow_SmallSettlementAwake : AwakeSystem<UICowCow_SmallSettlementComponent, GameObject>
    {
        public override void Awake(UICowCow_SmallSettlementComponent self, GameObject parent)
        {
            self.Awake(parent);
        }
    }
    public class UICowCow_SmallSettlementComponent : Component
    {
        public GameObject SmallBG { get; set; }
        public void Awake(GameObject parent)
        {
            ResourcesComponent res = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
            GameObject ab = (GameObject)res.GetAsset(UICowCowAB.CowCow_Prefabs, UICowCowType.CowCowSmallSettlement);
            this.GameObject = UnityEngine.Object.Instantiate(ab);
            this.GameObject.transform.SetParent(parent.transform, false);
            this.GameObject.name = UICowCowType.CowCowSmallSettlement;

            ReferenceCollector rc = this.GameObject.GetComponent<ReferenceCollector>();

            SmallBG = rc.Get<GameObject>("SmallBG");
            Button shareBtn = rc.Get<GameObject>("ShareBtn").GetComponent<Button>();
            Button continueBtn = rc.Get<GameObject>("ContinueBtn").GetComponent<Button>();

            shareBtn.onClick.Add(OnShare);
            continueBtn.onClick.Add(OnContinue);
        }
        private void OnShare()
        {
            Debug.Log("小结算分享！");
        }
        private void OnContinue()
        {
            UICowCow_GameRoomComponent room = Game.Scene.GetComponent<UIComponent>().Get(UICowCowType.CowCowGameRoom).GetComponent<UICowCow_GameRoomComponent>();
            if (room.IsLastBureau)
            {
                room.BigSettlement.ShowHideBigSettlement(true);
                return;
            }
            else
            {
                this.ShowHideSmallSettlement(false);
            }
        }

        public void ShowHideSmallSettlement(bool isShow)
        {
            CanvasGroup canvasGroup = this.GameObject.GetComponent<CanvasGroup>();
            canvasGroup.blocksRaycasts = isShow;
            canvasGroup.DOFade(isShow ? 1 : 0, 0.5f);
            UICowCow_GameRoomComponent room = Game.Scene.GetComponent<UIComponent>().Get(UICowCowType.CowCowGameRoom).GetComponent<UICowCow_GameRoomComponent>();
            if (!isShow)
            {
                Actor_GamerReadyHelper.OnReady(room.GamerComponent.LocalSeatID, room.RoomID).Coroutine();
            }
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }
            base.Dispose();

            UnityEngine.Object.Destroy(this.GameObject);
        }
    }
}
