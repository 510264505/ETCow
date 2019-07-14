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
    public class UICowCow_BigSettlementAwake : AwakeSystem<UICowCow_BigSettlementComponent>
    {
        public override void Awake(UICowCow_BigSettlementComponent self)
        {
            self.Awake();
        }
    }
    public class UICowCow_BigSettlementComponent : Component
    {
        public GameObject BigBG { get; set; }
        public void Awake()
        {
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

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
