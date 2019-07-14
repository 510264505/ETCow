using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UICowCow_SmallSettlementAwake : AwakeSystem<UICowCow_SmallSettlementComponent>
    {
        public override void Awake(UICowCow_SmallSettlementComponent self)
        {
            self.Awake();
        }
    }
    public class UICowCow_SmallSettlementComponent : Component
    {
        public GameObject SmallBG { get; set; }
        public void Awake()
        {
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

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
            Debug.Log("小结算继续游戏！");
        }
    }
}
