using ETModel;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace ETHotfix
{
    [ObjectSystem]
    public class UICowCow_GamerComponentAwake : AwakeSystem<UICowCow_GamerInfoComponent, GameObject, GamerInfo, int>
    {
        public override void Awake(UICowCow_GamerInfoComponent self, GameObject parent, GamerInfo info, int pos)
        {
            self.Awake(parent, info, pos);
        }
    }
    public class UICowCow_GamerInfoComponent : Component
    {
        private GameObject UIGameInfo { get; set; }
        private Text coin;
        private Text status;
        private Text cowType;
        private CanvasGroup HandCard { get; set; }
        private Image[] cards = new Image[5];
        private Button promptBtn;
        private Button submitBtn;
        private List<int> cardList = new List<int>();
        private CalculateCowTypeData cowTypeData;
        private C2M_CowCowGamerSubmitCardType submitCard;
        public string gamerName { get; set; }
        //是否准备
        public UIGamerStatus Status { get; set; } = UIGamerStatus.None;

        public void Awake(GameObject parent, GamerInfo info, int posIndex)
        {
            this.Status = (UIGamerStatus)info.Status;
            this.gamerName = info.Name;
            UIGameInfo = GamerFactory.Create(UICowCowType.CowCowGamerInfo);
            UIGameInfo.transform.SetParent(parent.transform, false);
            ReferenceCollector rc = UIGameInfo.GetComponent<ReferenceCollector>();

            Image headIcon = rc.Get<GameObject>("HeadIcon").GetComponent<Image>();
            headIcon.transform.localPosition = GamerData.Pos[info.SeatID].HeadPos;
            Text gamerNames = rc.Get<GameObject>("Names").GetComponent<Text>();
            coin = rc.Get<GameObject>("Coin").GetComponent<Text>();
            status = rc.Get<GameObject>("Status").GetComponent<Text>();
            HandCard = rc.Get<GameObject>("HandCard").GetComponent<CanvasGroup>();
            HandCard.transform.localPosition = GamerData.Pos[info.SeatID].CardPos;
            for (int i = 0; i < cards.Length; i++)
            {
                cards[i] = rc.Get<GameObject>("Card" + i).GetComponent<Image>();
            }
            promptBtn = rc.Get<GameObject>("PromptBtn").GetComponent<Button>();
            submitBtn = rc.Get<GameObject>("SubmitBtn").GetComponent<Button>();

            promptBtn.onClick.Add(OnPrompt);
            submitBtn.onClick.Add(OnSubmit);
            gamerNames.text = info.Name;
            SetCoin(info.Coin.ToString());
            //headIcon.sprite = info.HeadIcon;
        }

        private void InitSubmitCard()
        {
            submitCard.MaxCard = 0;
            submitCard.CardType = 0;
            submitCard.FlowerColor = 0;
            submitCard.CowNumber = 0;
            submitCard.Cards.Clear();
        }

        /// <summary>
        /// 提示按钮，系统计算牌型，并从新排列和关闭按钮
        /// </summary>
        private void OnPrompt()
        {
            InitSubmitCard();
            cowTypeData = CalculateCowTypeHelper.MostMaxCowType(cardList);
            switch (cowTypeData.cowType)
            {
                case CowType.None:
                    SetCowType("无牛");
                    break;
                case CowType.HaveCow:
                    for (int i = 0; i < cowTypeData.indexs.Length; i++)
                    {
                        UpMoveCard(cowTypeData.indexs[i]);
                    }
                    SetCowType($"牛{cowTypeData.cowNumber}");
                    break;
                case CowType.FiveFlowerCow:
                    SetCowType("五花牛");
                    break;
                case CowType.BombCow:
                    for (int i = 0; i < cowTypeData.indexs.Length; i++)
                    {
                        UpMoveCard(cowTypeData.indexs[i]);
                    }
                    SetCowType("炸弹牛");
                    break;
                case CowType.FiveSmallCow:
                    SetCowType("五小牛");
                    break;
            }
        }

        private void OnSubmit()
        {
            submitCard.MaxCard = cowTypeData.maxCard;
            submitCard.CardType = (int)cowTypeData.cowType;
            submitCard.FlowerColor = (int)cowTypeData.floweColor;
            submitCard.CowNumber = cowTypeData.cowNumber;
            for (int i = 0; i < cowTypeData.indexs.Length; i++)
            {
                int temp = cardList[cowTypeData.indexs[i]];
                cardList.Remove(temp);
                cardList.Insert(0, temp);
            }
            submitCard.Cards.AddRange(cardList);
        }

        /// <summary>
        /// 显示或隐藏手牌
        /// </summary>
        public void ShowHideHandCard(bool isShow)
        {
            this.HandCard.alpha = isShow ? 1 : 0;
        }

        /// <summary>
        /// 显示状态
        /// </summary>
        public void SetStatus(string status, UIGamerStatus uiStatus)
        {
            this.status.text = status;
            this.Status = uiStatus;
        }

        /// <summary>
        /// 设置当前金额
        /// </summary>
        public void SetCoin(string coin)
        {
            this.coin.text = coin;
        }

        private void SetCowType(string type)
        {
            this.cowType.text = type;
        }

        /// <summary>
        /// 设置手牌
        /// </summary>
        public void SetCards(Sprite[] sprites)
        {
            for (int i = 0; i < this.cards.Length; i++)
            {
                this.cards[i].sprite = sprites[i];
            }
        }

        public void SetCards(int[] indexs)
        {
            ResourcesComponent rc = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
            
            for (int i = 0; i < this.cards.Length; i++)
            {
                Sprite sprite = (Sprite)rc.GetAsset(UICowCowAB.CowCow_Texture.StringToAB(), CardHelper.GetCardAssetName(indexs[i]));
                this.cards[i].sprite = sprite;
            }
        }

        public void SetCard(int index, int card, Sprite sprite)
        {
            if (index == 0)
            {
                cardList.Clear();
            }
            cardList.Add(card);
            this.cards[index].sprite = sprite;
            ShowHideHandCard(index + 1 == this.cards.Length);
        }

        private void UpMoveCard(int index)
        {
            cards[index].transform.DOLocalMoveY(20, 0.5f);
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }
            base.Dispose();

            UnityEngine.Object.Destroy(UIGameInfo);
        }
    }
}
