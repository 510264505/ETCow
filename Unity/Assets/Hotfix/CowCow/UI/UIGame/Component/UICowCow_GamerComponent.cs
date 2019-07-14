using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UICowCow_GamerComponentAwake : AwakeSystem<UICowCow_GamerComponent, GameObject, GamerInfo>
    {
        public override void Awake(UICowCow_GamerComponent self, GameObject parent, GamerInfo info)
        {
            self.Awake(parent, info);
        }
    }
    public class UICowCow_GamerComponent : Component
    {
        private GameObject UIGameInfo { get; set; }
        private Text coin;
        private Text status;
        private CanvasGroup HandCard { get; set; }
        private Image[] cards = new Image[5];

        public void Awake(GameObject parent, GamerInfo info)
        {
            UIGameInfo = GamerFactory.Create(UICowCowType.CowCowGamerInfo);
            UIGameInfo.transform.SetParent(parent.transform, false);
            UIGameInfo.transform.localPosition = GamerData.Pos[info.SeatID].HeadPos;
            ReferenceCollector rc = UIGameInfo.GetComponent<ReferenceCollector>();

            Image headIcon = rc.Get<GameObject>("HeadIcon").GetComponent<Image>();
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
            SetCoin(info.Coin);
            //headIcon.sprite = info.HeadIcon;
        }

        /// <summary>
        /// 显示或隐藏手牌
        /// </summary>
        public void ShowHideHandCard(bool isShow)
        {
            HandCard.alpha = isShow ? 1 : 0;
        }

        /// <summary>
        /// 显示状态
        /// </summary>
        public void SetStatus(string status)
        {
            this.status.text = status;
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
            for (int i = 0; i < cards.Length; i++)
            {
                cards[i].sprite = sprites[i];
            }
        }
    }
}
