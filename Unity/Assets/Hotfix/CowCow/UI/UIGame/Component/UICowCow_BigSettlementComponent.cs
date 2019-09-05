using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UICowCow_BigSettlementComponentAwake : AwakeSystem<UICowCow_BigSettlementComponent, GameObject>
    {
        public override void Awake(UICowCow_BigSettlementComponent self, GameObject parent)
        {
            self.Awake(parent);
        }
    }
    public class UICowCow_BigSettlementComponent : Component
    {
        public GameObject BigBG { get; set; }
        public void Awake(GameObject parent)
        {
            ResourcesComponent res = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
            GameObject ab = (GameObject)res.GetAsset(UICowCowAB.CowCow_Prefabs, UICowCowType.CowCowBigSettlement);
            this.GameObject = UnityEngine.Object.Instantiate(ab);
            this.GameObject.transform.SetParent(parent.transform, false);
            this.GameObject.name = UICowCowType.CowCowBigSettlement;

            ReferenceCollector rc = this.GameObject.GetComponent<ReferenceCollector>();

            BigBG = rc.Get<GameObject>("BigBG");
            Button shareBtn = rc.Get<GameObject>("ShareBtn").GetComponent<Button>();
            Button continueBtn = rc.Get<GameObject>("ContinueBtn").GetComponent<Button>();

            shareBtn.onClick.Add(OnShare);
            continueBtn.onClick.Add(OnContinue);
        }
        private void OnShare()
        {
            Debug.Log("大结算分享！");
        }
        private void OnContinue()
        {
            Debug.Log("小结算继续游戏！");
        }
    }
}
