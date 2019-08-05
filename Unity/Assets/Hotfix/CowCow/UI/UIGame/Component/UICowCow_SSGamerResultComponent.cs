using System;
using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UICowCow_SSGamerResultComponentAwake : AwakeSystem<UICowCow_SSGamerResultComponent, GameObject>
    {
        public override void Awake(UICowCow_SSGamerResultComponent self, GameObject parent)
        {
            self.Awake(parent);
        }
    }
    public class UICowCow_SSGamerResultComponent : Component
    {
        private CanvasGroup banker;
        private Text gamerName;
        private Text bets;
        private Text cardType;
        private Image[] cards = new Image[5];
        private Text score;
        public void Awake(GameObject parent)
        {
            GameObject SSGamerResult = GamerFactory.Create(UICowCowType.CowCowSSPlayerResult);
            SSGamerResult.transform.SetParent(parent.transform, false);

            ReferenceCollector rc = SSGamerResult.GetComponent<ReferenceCollector>();
            banker = rc.Get<GameObject>("Banker").GetComponent<CanvasGroup>();
            gamerName = rc.Get<GameObject>("Name").GetComponent<Text>();
            bets = rc.Get<GameObject>("Bets").GetComponent<Text>();
            cardType = rc.Get<GameObject>("CardType").GetComponent<Text>();
            score = rc.Get<GameObject>("Score").GetComponent<Text>();
            for (int i = 0; i < cards.Length; i++)
            {
                cards[i] = rc.Get<GameObject>("Card" + i).GetComponent<Image>();
            }
        }

        public void SetGamerSmallSettlement(int seatId, string name, int betCoin, int type, int losewin, int[] list)
        {
            banker.alpha = seatId == 0 ? 1 : 0;
            gamerName.text = name;
            bets.text = betCoin.ToString();
            //switch ((CowType)type)
            //{
            //    case CowType.None:
            //        break;
            //    case CowType.HaveCow:
            //        break;
            //    case CowType.FiveFlowerCow:
            //        break;
            //    case CowType.BombCow:
            //        break;
            //    case CowType.FiveSmallCow:
            //        break;
            //}
            cardType.text = ((CowType)type).ToString();
            score.text = losewin.ToString();
            for (int i = 0; i < cards.Length; i++)
            {
                cards[i].sprite = null;
            }
        }
    }
}
