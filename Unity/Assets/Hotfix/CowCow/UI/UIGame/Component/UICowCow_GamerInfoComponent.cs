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
        private ResourcesComponent res;
        private MicrophoneComponent microphone;
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
        private C2M_CowCowGamerSubmitCardType submitCard = new C2M_CowCowGamerSubmitCardType();

        private Image emoji;
        private CanvasGroup chatBG;
        private Text chatText;
        private float delayTimer = 2; //聊天信息延迟3秒消失
        public string gamerName { get; set; }
        public int Sex { get; set; }
        private Tweener emojiTweener;
        private Tweener chatTweener;
        //是否准备
        public UIGamerStatus Status { get; set; } = UIGamerStatus.None;

        private int upOffy = 20;

        public void Awake(GameObject parent, GamerInfo info, int posIndex)
        {
            res = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
            res.LoadBundle(UICowCowAB.CowCow_Texture);
            res.LoadBundle(UICowCowAB.CowCow_SoundOther);
            microphone = Game.Scene.GetComponent<MicrophoneComponent>();
            this.Status = (UIGamerStatus)info.Status;
            this.gamerName = info.Name;
            UIGameInfo = GamerFactory.Create(UICowCowType.CowCowGamerInfo);
            UIGameInfo.transform.SetParent(parent.transform, false);
            UIGameInfo.name = UICowCowType.CowCowGamerInfo;
            ReferenceCollector rc = UIGameInfo.GetComponent<ReferenceCollector>();

            Image headIcon = rc.Get<GameObject>("HeadIcon").GetComponent<Image>();
            headIcon.transform.localPosition = GamerData.Pos[posIndex].HeadPos;
            Text gamerNames = rc.Get<GameObject>("Names").GetComponent<Text>();
            coin = rc.Get<GameObject>("Coin").GetComponent<Text>();
            status = rc.Get<GameObject>("Status").GetComponent<Text>();
            cowType = rc.Get<GameObject>("CowType").GetComponent<Text>();
            HandCard = rc.Get<GameObject>("HandCard").GetComponent<CanvasGroup>();
            HandCard.transform.localPosition = GamerData.Pos[posIndex].CardPos;
            for (int i = 0; i < cards.Length; i++)
            {
                cards[i] = rc.Get<GameObject>("Card" + i).GetComponent<Image>();
            }
            promptBtn = rc.Get<GameObject>("PromptBtn").GetComponent<Button>();
            submitBtn = rc.Get<GameObject>("SubmitBtn").GetComponent<Button>();

            emoji = rc.Get<GameObject>("Emoji").GetComponent<Image>();
            chatBG = rc.Get<GameObject>("ChatBG").GetComponent<CanvasGroup>();
            chatText = rc.Get<GameObject>("ChatText").GetComponent<Text>();
            chatBG.transform.localPosition = GamerData.Pos[posIndex].ChatPosData.Pos;
            chatBG.transform.Rotate(GamerData.Pos[posIndex].ChatPosData.Rotate);
            chatText.transform.Rotate(GamerData.Pos[posIndex].ChatPosData.Rotate);

            promptBtn.onClick.Add(OnPrompt);
            submitBtn.onClick.Add(OnSubmit);
            gamerNames.text = info.Name;
            SetCoin(info.Coin.ToString());
            this.Sex = info.Sex;
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
                        UpMoveCard(cowTypeData.indexs[i], upOffy);
                    }
                    SetCowType($"牛{cowTypeData.cowNumber}");
                    break;
                case CowType.FiveFlowerCow:
                    SetCowType("五花牛");
                    break;
                case CowType.BombCow:
                    for (int i = 0; i < cowTypeData.indexs.Length; i++)
                    {
                        UpMoveCard(cowTypeData.indexs[i], upOffy);
                    }
                    SetCowType("炸弹牛");
                    break;
                case CowType.FiveSmallCow:
                    SetCowType("五小牛");
                    break;
            }
            ShowHidePromptButton(false);
            ShowHideSubmitButton(true);
        }

        private void OnSubmit()
        {
            submitCard.MaxCard = cowTypeData.maxCard;
            submitCard.CardType = (int)cowTypeData.cowType;
            submitCard.FlowerColor = (int)cowTypeData.floweColor;
            submitCard.CowNumber = cowTypeData.cowNumber;
            if (cowTypeData.indexs != null)
            {
                for (int i = 0; i < cowTypeData.indexs.Length; i++)
                {
                    int temp = cardList[cowTypeData.indexs[i]];
                    cardList.Remove(temp);
                    cardList.Insert(0, temp);
                }
            }
            submitCard.Cards.AddRange(cardList);
            Actor_SubmitHandCardHelper.OnSubmitHandCard(submitCard).Coroutine();
            ShowHideSubmitButton(false);
        }

        public void ShowHidePromptButton(bool isShow)
        {
            CanvasGroup canvasGroup = promptBtn.GetComponent<CanvasGroup>();
            canvasGroup.alpha = isShow ? 1 : 0;
            canvasGroup.blocksRaycasts = isShow;
        }

        public void ShowHideSubmitButton(bool isShow)
        {
            CanvasGroup canvasGroup = submitBtn.GetComponent<CanvasGroup>();
            canvasGroup.alpha = isShow ? 1 : 0;
            canvasGroup.blocksRaycasts = isShow;
        }

        /// <summary>
        /// 显示或隐藏手牌
        /// </summary>
        public void ShowHideHandCard(bool isShow)
        {
            this.HandCard.alpha = isShow ? 1 : 0;
            if (!isShow)
            {
                for (int i = 0; i < cards.Length; i++)
                {
                    UpMoveCard(i, 0);
                }
            }
        }

        /// <summary>
        /// 显示状态
        /// </summary>
        public void SetStatus(UIGamerStatus uiStatus, string status = null)
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
            ShowHideHandCard(true);
        }

        /// <summary>
        /// 设置牌背面
        /// </summary>
        public void SetCards(Sprite sprite)
        {
            for (int i = 0; i < this.cards.Length; i++)
            {
                this.cards[i].sprite = sprite;
            }
            ShowHideHandCard(true);
        }

        public void SetCards(int[] indexs)
        {
            for (int i = 0; i < this.cards.Length; i++)
            {
                Sprite sprite = (Sprite)res.GetAsset(UICowCowAB.CowCow_Texture, CardHelper.GetCardAssetName(indexs[i]));
                this.cards[i].sprite = sprite;
            }
            ShowHideHandCard(true);
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

        private void UpMoveCard(int index, int offy)
        {
            cards[index].transform.DOLocalMoveY(offy, 0.5f);
        }

        public void ShowEmoji(int index)
        {
            Sprite sprite = (Sprite)res.GetAsset(UICowCowAB.CowCow_Texture, $"Emoji_{index}");
            emoji.sprite = sprite;
            emoji.DOFade(1, 0.5f);
            emoji.transform.DOShakeScale(1);
            if (emojiTweener != null)
            {
                emojiTweener.Kill();
                emojiTweener = null;
            }
            emojiTweener = emoji.DOFade(0, 1).SetDelay(delayTimer);
        }

        public void ShowChatFont(int index, string message, int sex)
        {
            chatText.text = message;
            chatBG.DOFade(1, 0.5f);
            if (index <= 8) //只有8个声音
            {
                //播放声音
                string str = sex == 0 ? $"boy{index}" : $"girl{index}";
                AudioClip ac = (AudioClip)res.GetAsset(UICowCowAB.CowCow_SoundOther, str);
                microphone.PlaySound(ac);
            }
            if (chatTweener != null)
            {
                chatTweener.Kill();
                chatTweener = null;
            }
            chatTweener = chatBG.DOFade(0, 1).SetDelay(delayTimer);
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }
            base.Dispose();

            UnityEngine.Object.Destroy(UIGameInfo);
            res.UnloadBundle(UICowCowAB.CowCow_Texture);
            res.UnloadBundle(UICowCowAB.CowCow_SoundOther);
        }
    }
}
