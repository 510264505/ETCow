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
        private int tan = 10;
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

        public void SetGamerSmallSettlement(CowCowSmallSettlementInfo info)
        {
            banker.alpha = info.SeatID == 0 ? 1 : 0;
            gamerName.text = Game.Scene.GetComponent<UIComponent>().Get(UICowCowType.CowCowGameRoom).GetComponent<UICowCow_GameRoomComponent>().GamerComponent.Get(info.SeatID).GetComponent<UICowCow_GamerInfoComponent>().gamerName;
            bets.text = info.BetCoin.ToString();
            switch ((CowType)info.CardsType)
            {
                case CowType.None:
                    cardType.text = "无牛";
                    break;
                case CowType.HaveCow:
                    int num = info.CowNumber % tan;
                    if (num == 0)
                    {
                        cardType.text = "牛牛";
                    }
                    else
                    {
                        cardType.text = $"牛{num}";
                    }
                    break;
                case CowType.FiveFlowerCow:
                    cardType.text = "五花牛";
                    break;
                case CowType.BombCow:
                    cardType.text = "炸弹牛";
                    break;
                case CowType.FiveSmallCow:
                    cardType.text = "五小牛";
                    break;
            }
            score.text = info.LoseWin.ToString();
            ResourcesComponent rc = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
            for (int i = 0; i < cards.Length; i++)
            {
                cards[i].sprite = (Sprite)rc.GetAsset(UICowCowAB.CowCow_Texture.StringToAB(), CardHelper.GetCardAssetName(info.Cards[i]));
            }
        }
    }
}
