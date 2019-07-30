using ETModel;
using UnityEngine;
using UnityEngine.UI;

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
        private CanvasGroup HandCard { get; set; }
        private Image[] cards = new Image[5];
        //是否准备
        public UIGamerStatus Status { get; set; } = UIGamerStatus.None;

        public void Awake(GameObject parent, GamerInfo info, int posIndex)
        {
            this.Status = (UIGamerStatus)info.Status;
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

            gamerNames.text = info.Name;
            SetCoin(info.Coin.ToString());
            //headIcon.sprite = info.HeadIcon;
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

        public void SetCard(int n, Sprite sprite)
        {
            this.cards[n].sprite = sprite;
            ShowHideHandCard(n + 1 == this.cards.Length);
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
