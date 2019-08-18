using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ETModel;
using UnityEngine.UI;
using System.Text;
using System;
using System.Threading;
using System.Linq;

namespace ETHotfix
{
    [ObjectSystem]
    public class UICowCow_GameRoomComponentAwake : AwakeSystem<UICowCow_GameRoomComponent>
    {
        public override void Awake(UICowCow_GameRoomComponent self)
        {
            self.Awake();
        }
    }
    public class UICowCow_GameRoomComponent : Component
    {
        private UICowCow_SmallSettlementComponent smallSettlement;
        private UICowCow_BigSettlementComponent bigSettlement;
        private GamerComponent gamerComponent;
        private UICowCow_ChatComponent chatComponent;
        
        private GameObject BackGround { get; set; }
        private GameObject UIRoomGamer { get; set; }
        private Text bureau;
        private Text gameName;
        private Text gameRule;
        private Text roomId;
        private Button readyBtn;
        private Button inviteBtn;

        private StringBuilder sb = new StringBuilder();
        private int curBureauCount = 1;
        private int bureauCount;

        private bool isShowTime = false;
        private CancellationTokenSource tokenSource = new CancellationTokenSource();
        private CancellationToken cancellationToken;
        private string timeFormat = "t"; //时:分

        public UICowCow_BigSettlementComponent BigSettlement
        {
            get
            {
                if (bigSettlement == null)
                {
                    UI uiRoom = this.GetParent<UI>();
                    UI ui = UICowCowCreateChildGameObjectFactory.Create<UICowCow_BigSettlementComponent>(UICowCowType.CowCowBigSettlement, uiRoom, BackGround);
                    bigSettlement = ui.GetComponent<UICowCow_BigSettlementComponent>();
                }
                return bigSettlement;
            }
        }

        public UICowCow_SmallSettlementComponent SmallSettlement
        {
            get
            {
                if (smallSettlement == null)
                {
                    UI uiRoom = this.GetParent<UI>();
                    UI ui = UICowCowCreateChildGameObjectFactory.Create<UICowCow_SmallSettlementComponent>(UICowCowType.CowCowSmallSettlement, uiRoom, BackGround);
                    smallSettlement = ui.GetComponent<UICowCow_SmallSettlementComponent>();
                }
                return smallSettlement;
            }
        }

        public GamerComponent GamerComponent
        {
            get
            {
                if (gamerComponent == null)
                {
                    UI uiRoom = this.GetParent<UI>();
                    gamerComponent = uiRoom.AddComponent<GamerComponent>();
                }
                return gamerComponent;
            }
        }

        public UICowCow_ChatComponent ChatComponent
        {
            get
            {
                if (chatComponent == null)
                {
                    UI uiRoom = this.GetParent<UI>();
                    UI ui = UICowCowCreateChildGameObjectFactory.Create<UICowCow_ChatComponent>(UICowCowType.CowcowChat, uiRoom, BackGround);
                    chatComponent = ui.GetComponent<UICowCow_ChatComponent>();
                }
                return chatComponent;
            }
        }

        public string RoomID { get; set; }

        private void ReSet()
        {
            sb.Clear();
            curBureauCount = 1;
            isShowTime = false;
            GamerComponent.LocalGamer = null;
        }
        public void Awake()
        {
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            BackGround = rc.Get<GameObject>("BackGround");
            UIRoomGamer = rc.Get<GameObject>("UIRoomGamer");
            this.bureau = rc.Get<GameObject>("GameBureau").GetComponent<Text>();
            this.gameName = rc.Get<GameObject>("GameName").GetComponent<Text>();
            this.gameRule = rc.Get<GameObject>("GameRule").GetComponent<Text>();
            this.roomId = rc.Get<GameObject>("RoomID").GetComponent<Text>();
            Text nowTime = rc.Get<GameObject>("NowTime").GetComponent<Text>();
            Button phizBtn = rc.Get<GameObject>("PhizBtn").GetComponent<Button>();
            Button keyboardBtn = rc.Get<GameObject>("KeyboardBtn").GetComponent<Button>();
            Button voiceBtn = rc.Get<GameObject>("VoiceBtn").GetComponent<Button>();
            Button dissBtn = rc.Get<GameObject>("DissBtn").GetComponent<Button>();
            readyBtn = rc.Get<GameObject>("ReadyBtn").GetComponent<Button>();
            inviteBtn = rc.Get<GameObject>("InviteBtn").GetComponent<Button>();

            phizBtn.onClick.Add(Onphiz); // 表情
            keyboardBtn.onClick.Add(OnKeyboard);
            voiceBtn.onClick.Add(OnVoice);
            dissBtn.onClick.Add(OnDiss);
            readyBtn.onClick.Add(OnReady);
            inviteBtn.onClick.Add(OnInvite);

            isShowTime = true;
            NowTime(nowTime).Coroutine();

            Game.EventSystem.Run(EventIdCowCowType.RemoveLobby);
        }

        public void Init(string gameName, int bureau, int ruleBit, string roomId, int people)
        {
            this.bureauCount = bureau;
            this.gameName.text = gameName;
            this.gameRule.text = $"{people}人 " + Rule(ruleBit);
            this.roomId.text = roomId;
            this.RoomID = roomId;
            Bureau();
        }
        
        /// <summary>
        /// 先讲自己克隆出来
        /// </summary>
        public void AddLocalGamer(GamerInfo info)
        {
            if (info.UserID != ETModel.Game.Scene.GetComponent<ClientComponent>().User.UserID)
            {
                return;
            }
            Gamer gamer = GamerComponent.AddGamerUI(info);
            if (gamer != null)
            {
                gamer.AddComponent<UICowCow_GamerInfoComponent, GameObject, GamerInfo, int>(UIRoomGamer, info, 0);
                gamer.AddComponent<UICowCow_SSGamerResultComponent, GameObject>(SmallSettlement.SmallBG);
                gamer.AddComponent<UICowCow_BSGamerResultComponent, GameObject>(BigSettlement.BigBG);
            }
            if (GamerComponent.LocalGamer == null)
            {
                GamerComponent.Init(info.UserID);
                if (GamerComponent.LocalGamer.GetComponent<UICowCow_GamerInfoComponent>().Status == UIGamerStatus.Down)
                {
                    ShowHideReadyButton(true);
                }
            }
        }

        /// <summary>
        /// 再添加其他玩家
        /// </summary>
        public void AddGamer(GamerInfo info, int posIndex)
        {
            Gamer gamer = GamerComponent.AddGamerUI(info);
            if (gamer != null)
            {
                gamer.AddComponent<UICowCow_GamerInfoComponent, GameObject, GamerInfo, int>(UIRoomGamer, info, posIndex);
                gamer.AddComponent<UICowCow_SSGamerResultComponent, GameObject>(SmallSettlement.SmallBG);
                gamer.AddComponent<UICowCow_BSGamerResultComponent, GameObject>(BigSettlement.BigBG);
            }
        }

        public void RemoveGamer(long id)
        {
            GamerComponent.Remove(id);
        }

        public void ShowHideInviteButton(bool isShow)
        {
            inviteBtn.GetComponent<CanvasGroup>().alpha = isShow ? 1 : 0;
            inviteBtn.GetComponent<CanvasGroup>().blocksRaycasts = isShow;
        }

        /// <summary>
        /// 显示隐藏准备按钮
        /// </summary>
        public void ShowHideReadyButton(bool isShow)
        {
            readyBtn.GetComponent<CanvasGroup>().alpha = isShow ? 1 : 0;
            readyBtn.GetComponent<CanvasGroup>().blocksRaycasts = isShow;
            if (!isShow)
            {
                //隐藏所有玩家手牌
                Dictionary<int, Gamer> gamers = GamerComponent.GetDictAll();
                foreach (Gamer gamer in gamers.Values)
                {
                    gamer.GetComponent<UICowCow_GamerInfoComponent>().ShowHideHandCard(isShow);
                }
            }
        }

        public void GamerReady(int[] seatIds)
        {
            for (int i = 0; i < seatIds.Length; i++)
            {
                UICowCow_GamerInfoComponent gc = GamerComponent.Get(seatIds[i]).GetComponent<UICowCow_GamerInfoComponent>();
                if (gc.Status != UIGamerStatus.Ready)
                {
                    gc.SetStatus(UIGamerStatus.Ready, UIGamerStatusString.Ready);
                }
            }
        }

        private string Rule(int ruleBit)
        {
            sb.Clear();
            foreach (KeyValuePair<CowType, string> rule in GameInfo.Rule)
            {
                if ((ruleBit & (int)rule.Key) == (int)rule.Key)
                {
                    sb.Append(rule.Value + "/");
                }
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }
        private async ETVoid NowTime(Text nowTiemr)
        {
            cancellationToken = tokenSource.Token;
            while (isShowTime)
            {
                nowTiemr.text = DateTime.Now.ToString(timeFormat);
                await ETModel.Game.Scene.GetComponent<TimerComponent>().WaitAsync(10 * 1000, cancellationToken);
            }
        }

        /// <summary>
        /// 表情
        /// </summary>
        private void Onphiz()
        {
            ChatComponent.ShowHideEmoji(true);
        }
        private void OnKeyboard()
        {
            ChatComponent.ShowHideChatFont(true);
        }
        private void OnVoice()
        {

        }
        private void OnDiss()
        {

        }
        private void OnReady()
        {
            Actor_GamerReadyHelper.OnReady(GamerComponent.LocalSeatID, this.RoomID).Coroutine();
        }
        private void OnInvite()
        {

        }
        public void Bureau()
        {
            bureau.text = curBureauCount + "/" + bureauCount;
        }
        public void ChangeDesk(Sprite sprite)
        {
            BackGround.GetComponent<Image>().sprite = sprite;
        }

        /// <summary>
        /// 打开手牌并结算
        /// </summary>
        public void OpenAllGamerHandCard(CowCowSmallSettlementInfo[] info)
        {
            Dictionary<int,Gamer> gamers = GamerComponent.GetDictAll();
            for (int i = 0; i < info.Length; i++)
            {
                int[] cards = info[i].Cards.ToArray();
                UICowCow_GamerInfoComponent gic = gamers[info[i].SeatID].GetComponent<UICowCow_GamerInfoComponent>();
                UICowCow_SSGamerResultComponent ssgrc = gamers[info[i].SeatID].GetComponent<UICowCow_SSGamerResultComponent>();
                gic.SetCards(cards);
                gic.SetStatus(UIGamerStatus.Down);
                ssgrc.SetGamerSmallSettlement(info[i]);
            }
            //在此延迟显示小结算
            SmallSettlement.ShowHideSmallSettlement(true, 5000).Coroutine();
        }

        public void DealCardsGiveAllGamer()
        {
            ResourcesComponent rc = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
            Dictionary<int, Gamer> gamers = GamerComponent.GetDictAll();
            foreach (KeyValuePair<int, Gamer> gamer in gamers)
            {
                if (gamer.Key != GamerComponent.LocalSeatID)
                {
                    UICowCow_GamerInfoComponent gic = gamer.Value.GetComponent<UICowCow_GamerInfoComponent>();
                    gic.SetCards((Sprite)rc.GetAsset(UICowCowAB.CowCow_Texture.StringToAB(), "CardBG"));
                }
            }
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }
            base.Dispose();

            ReSet();
        }
    }
}

