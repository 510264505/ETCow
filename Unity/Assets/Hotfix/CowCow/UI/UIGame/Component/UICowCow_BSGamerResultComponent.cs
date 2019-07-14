using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    public class UICowCow_BSGamerResultComponentAwake : AwakeSystem<UICowCow_BSGamerResultComponent, GameObject>
    {
        public override void Awake(UICowCow_BSGamerResultComponent self, GameObject parent)
        {
            self.Awake(parent);
        }
    }
    public class UICowCow_BSGamerResultComponent : Component
    {
        private Image headIcon;
        private Text gamerName;
        private Text banker;
        private Text fiveSmallCow;
        private Text fiveFlowerCow;
        private Text fiveBombCow;
        private Text doubleCow;
        private Text haveCow;
        private Text notCow;
        private Text totleScore;
        public void Awake(GameObject parent)
        {
            GameObject BSGamerResult = GamerFactory.Create(UICowCowType.CowCowBSPlayerResult);
            BSGamerResult.transform.SetParent(parent.transform, false);

            ReferenceCollector rc = BSGamerResult.GetComponent<ReferenceCollector>();
            headIcon = rc.Get<GameObject>("HeadIcon").GetComponent<Image>();
            banker = rc.Get<GameObject>("Banker").GetComponent<Text>();
            fiveSmallCow = rc.Get<GameObject>("FiveSmallCow").GetComponent<Text>();
            fiveFlowerCow = rc.Get<GameObject>("fiveFlowerCow").GetComponent<Text>();
            fiveBombCow = rc.Get<GameObject>("FiveBombCow").GetComponent<Text>();
            doubleCow = rc.Get<GameObject>("DoubleCow").GetComponent<Text>();
            haveCow = rc.Get<GameObject>("HaveCow").GetComponent<Text>();
            notCow = rc.Get<GameObject>("NotCow").GetComponent<Text>();
            totleScore = rc.Get<GameObject>("TotleScore").GetComponent<Text>();
        }

        public void SetGamerBigSettlement()
        {
            headIcon.sprite = null;
            banker.text = "";
            fiveSmallCow.text = "";
            fiveFlowerCow.text = "";
            fiveBombCow.text = "";
            doubleCow.text = "";
            haveCow.text = "";
            notCow.text = "";
            totleScore.text = "";
        }
    }
}
