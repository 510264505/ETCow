using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
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
        private Text bombCow;
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
            fiveFlowerCow = rc.Get<GameObject>("FiveFlowerCow").GetComponent<Text>();
            bombCow = rc.Get<GameObject>("FiveBombCow").GetComponent<Text>();
            doubleCow = rc.Get<GameObject>("DoubleCow").GetComponent<Text>();
            haveCow = rc.Get<GameObject>("HaveCow").GetComponent<Text>();
            notCow = rc.Get<GameObject>("NotCow").GetComponent<Text>();
            totleScore = rc.Get<GameObject>("TotalScroe").GetComponent<Text>();
        }

        public void SetGamerBigSettlement(int banker, int fiveSmallCow,int fiveFlowerCow,int bombCow,int doubleCow,int haveCow, int notCow,int score)
        {
            this.headIcon.sprite = null;
            this.banker.text = banker.ToString();
            this.fiveSmallCow.text = fiveSmallCow.ToString();
            this.fiveFlowerCow.text = fiveFlowerCow.ToString();
            this.bombCow.text = bombCow.ToString();
            this.doubleCow.text = doubleCow.ToString();
            this.haveCow.text = haveCow.ToString();
            this.notCow.text = notCow.ToString();
            if (score > 0)
            {
                this.totleScore.text = $"+{score}";
            }
            else
            {
                this.totleScore.text = $"-{score}";
            }
        }
    }
}
