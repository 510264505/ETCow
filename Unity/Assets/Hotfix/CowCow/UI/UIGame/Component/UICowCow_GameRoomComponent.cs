using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ETModel;
using UnityEngine.UI;
using System.Text;
using System;
using System.Threading;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using DG.Tweening;

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
        private class UIChatVoice
        {
            public CanvasGroup parent;
            public Image image;
        }
        private ResourcesComponent res;

        private UICowCow_SmallSettlementComponent smallSettlement;
        private UICowCow_BigSettlementComponent bigSettlement;
        private GamerComponent gamerComponent;
        private UICowCow_ChatComponent chatComponent;
        private UIChatVoice uiChatVoice;
        private UICowCow_DissoltionComponent dissComponent;
        private UICowCow_GameSettingComponent settingComponent;
        
        private GameObject BackGround { get; set; }
        private GameObject UIRoomGamer { get; set; }
        private Text bureau;
        private Text gameName;
        private Text gameRule;
        private Text roomId;
        private Button readyBtn;
        private Button inviteBtn;

        private StringBuilder sb = new StringBuilder();
        private int bureauCount;
        public bool IsLastBureau { get; set; } = false;

        private bool isShowTime = false;
        private CancellationTokenSource tokenSource = new CancellationTokenSource();
        private CancellationToken cancellationToken;
        private string timeFormat = "t"; //时:分

        private Image cardHeap;
        private CanvasGroup betButtonPanel;
        private CanvasGroup grabBankerButtonPanel;

        public UICowCow_BigSettlementComponent BigSettlement
        {
            get
            {
                if (bigSettlement == null)
                {
                    bigSettlement = this.GetParent<UI>().AddComponent<UICowCow_BigSettlementComponent, GameObject>(BackGround);
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
                    smallSettlement = this.GetParent<UI>().AddComponent<UICowCow_SmallSettlementComponent, GameObject>(BackGround);
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
                    gamerComponent = this.GetParent<UI>().AddComponent<GamerComponent>();
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
                    chatComponent = this.GetParent<UI>().AddComponent<UICowCow_ChatComponent, GameObject>(BackGround);
                }
                return chatComponent;
            }
        }

        private UIChatVoice ChatVoice
        {
            get
            {
                if (uiChatVoice == null)
                {
                    uiChatVoice = new UIChatVoice();
                    GameObject prefab = (GameObject)ETModel.Game.Scene.GetComponent<ResourcesComponent>().GetAsset(UICowCowAB.CowCow_Prefabs, UICowCowType.CowCowChatVoice);
                    uiChatVoice.parent = UnityEngine.Object.Instantiate(prefab).GetComponent<CanvasGroup>();
                    uiChatVoice.image = uiChatVoice.parent.transform.GetChild(0).GetComponent<Image>();
                }
                return uiChatVoice;
            }
        }

        public UICowCow_DissoltionComponent DissComponent
        {
            get
            {
                if (dissComponent == null)
                {
                    dissComponent = this.GetParent<UI>().AddComponent<UICowCow_DissoltionComponent, GameObject>(BackGround);
                }
                return dissComponent;
            }
        }

        public UICowCow_GameSettingComponent SettingComponent
        {
            get
            {
                if (settingComponent == null)
                {
                    settingComponent = this.GetParent<UI>().AddComponent<UICowCow_GameSettingComponent, GameObject>(BackGround);
                }
                return settingComponent;
            }
        }

        public string RoomID { get; set; }

        private void ReSet()
        {
            this.sb.Clear();
            this.isShowTime = false;
            this.GamerComponent.LocalGamer = null;
            this.IsLastBureau = false;
        }
        public void Awake()
        {
            res = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
            res.LoadBundle(UICowCowAB.CowCow_Prefabs);
            res.LoadBundle(UICowCowAB.CowCow_Texture);

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
            EventTrigger voiceEvent = rc.Get<GameObject>("VoiceBtn").GetComponent<EventTrigger>();
            Button dissBtn = rc.Get<GameObject>("DissBtn").GetComponent<Button>();
            Button settingBtn = rc.Get<GameObject>("SettingBtn").GetComponent<Button>();
            readyBtn = rc.Get<GameObject>("ReadyBtn").GetComponent<Button>();
            inviteBtn = rc.Get<GameObject>("InviteBtn").GetComponent<Button>();

            cardHeap = rc.Get<GameObject>("CardHeap").GetComponent<Image>();
            betButtonPanel = rc.Get<GameObject>("BetButtonPanel").GetComponent<CanvasGroup>();
            grabBankerButtonPanel = rc.Get<GameObject>("GrabBankerButtonPanel").GetComponent<CanvasGroup>();
            Button[] betBtns = new Button[betButtonPanel.transform.childCount];
            Button[] grabBankerBtns = new Button[grabBankerButtonPanel.transform.childCount];
            for (int i = 0; i < betBtns.Length; i++)
            {
                int n = i + 1;
                betBtns[i] = rc.Get<GameObject>($"X{(i + 1)}Btn").GetComponent<Button>();
                betBtns[i].onClick.Add(() => { this.OnBet(n); });
                grabBankerBtns[i] = rc.Get<GameObject>($"GrabBankerX{(i + 1)}Btn").GetComponent<Button>();
                grabBankerBtns[i].onClick.Add(() => { this.OnGrabBanker(n); });
            }
            Button grabBankerCloseBtn = rc.Get<GameObject>("GrabBankerCloseBtn").GetComponent<Button>();
            grabBankerCloseBtn.onClick.Add(() => { 
                this.OnGrabBanker(1); //所有玩家都不抢庄时，默认为1
            });
            

            phizBtn.onClick.Add(Onphiz); // 表情
            keyboardBtn.onClick.Add(OnKeyboard);
            dissBtn.onClick.Add(OnDiss);
            readyBtn.onClick.Add(OnReady);
            inviteBtn.onClick.Add(OnInvite);
            settingBtn.onClick.Add(OnSetting);

            voiceEvent.triggers.Add(AddEventTrigger(EventTriggerType.PointerDown, OnVoiceDown));
            voiceEvent.triggers.Add(AddEventTrigger(EventTriggerType.PointerUp, OnVoiceUp));

            isShowTime = true;
            NowTime(nowTime).Coroutine();


            // 隐藏即可
            Game.Scene.GetComponent<UIComponent>().Get(UICowCowType.CowCowLobby).GetComponent<UICowCowLobbyComponent>().ShowHideLobby(false);
            //Game.EventSystem.Run(CowCowEventIdType.RemoveLobby);
        }

        public void Init(string gameName, int bureau, int ruleBit, string roomId, int people, int curBureau)
        {
            this.bureauCount = bureau;
            this.gameName.text = gameName;
            this.gameRule.text = $"{people}人 " + Rule(ruleBit);
            this.roomId.text = roomId;
            this.RoomID = roomId;
            this.SetBureau(curBureau);
        }
        
        private EventTrigger.Entry AddEventTrigger(EventTriggerType type, UnityAction<BaseEventData> action)
        {
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = type;
            entry.callback.Add(action);
            return entry;
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

        private void OnVoiceDown(BaseEventData eventData)
        {
            Game.Scene.GetComponent<MicrophoneComponent>().OnButtonDown();
        }
        private void OnVoiceUp(BaseEventData eventData)
        {
            Game.Scene.GetComponent<MicrophoneComponent>().OnButtonUp();
        }
        private void OnDiss()
        {
            //弹窗是否发起解散
            GameObject go = UnityEngine.Object.Instantiate((GameObject)res.GetAsset(UICowCowAB.CowCow_Prefabs, UICowCowType.CowCowInitiateDissPanel));
            go.transform.SetParent(BackGround.transform, false);
            Action action = () => { UnityEngine.Object.Destroy(go.gameObject); };
            go.GetComponent<Button>().onClick.Add(action);
            Button initiateBtn = go.transform.Find("BG/InitiateBtn").GetComponent<Button>();
            Button cancelBtn = go.transform.Find("BG/CancelBtn").GetComponent<Button>();
            initiateBtn.onClick.Add(() =>
            {
                Actor_DissoltionHelper.OnSendVOte(GamerComponent.LocalSeatID, true).Coroutine();
                action?.Invoke();
            });
            cancelBtn.onClick.Add(action);
        }
        private void OnReady()
        {
            Actor_GamerReadyHelper.OnReady(GamerComponent.LocalSeatID).Coroutine();
        }
        private void OnInvite()
        {

        }
        private void OnSetting()
        {
            SettingComponent.ShowHideUIGameSetting(true);
        }
        private void OnBet(int n)
        {
            GamerComponent.Get(GamerComponent.LocalSeatID).GetComponent<UICowCow_GamerInfoComponent>().SeeSelfCards(n);
            this.ShowBetBtns(false);
        }
        private void OnGrabBanker(int n)
        {
            //把发送准备那里改了，以及服务器的准备并发牌逻辑那里
            Actor_GrabBankerHelper.SendGrabBanker(GamerComponent.LocalSeatID, n).Coroutine();
        }
        public void SetBureau(int? bureau = null)
        {
            if (bureau != null)
            {
                this.bureau.text = bureau + "/" + this.bureauCount;
                if (bureau == this.bureauCount)
                {
                    this.IsLastBureau = true;
                }
            }
        }
        public void ChangeDesk(Sprite sprite)
        {
            BackGround.GetComponent<Image>().sprite = sprite;
        }

        public void ShowBetBtns(bool isShow)
        {
            this.betButtonPanel.alpha = isShow ? 1 : 0;
            this.betButtonPanel.blocksRaycasts = isShow;
        }

        public void ShowHideCardHeap(bool isShow)
        {
            this.cardHeap.DOFade(isShow ? 1 : 0, 0.5f);
        }

        public void ShowHideGrabBanker(bool isShow, int? bureau = null)
        {
            this.grabBankerButtonPanel.alpha = isShow ? 1 : 0;
            this.grabBankerButtonPanel.blocksRaycasts = isShow;
            if (bureau != null)
            {
                this.SetBureau(bureau);
            }
        }

        /// <summary>
        /// 打开手牌并结算
        /// </summary>
        public async ETVoid OpenAllGamerHandCard(CowCowSmallSettlementInfo[] info)
        {
            Dictionary<int,Gamer> gamers = GamerComponent.GetDictAll();
            for (int i = 0; i < info.Length; i++)
            {
                int[] cards = info[i].Cards.ToArray();
                UICowCow_GamerInfoComponent gic = gamers[info[i].SeatID].GetComponent<UICowCow_GamerInfoComponent>();
                UICowCow_SSGamerResultComponent ssgrc = gamers[info[i].SeatID].GetComponent<UICowCow_SSGamerResultComponent>();
                gic.ShowCards(cards);
                ssgrc.SetGamerSmallSettlement(info[i]);
            }
            await ETModel.Game.Scene.GetComponent<TimerComponent>().WaitAsync(5000);
            //在此延迟显示小结算
            SmallSettlement.ShowHideSmallSettlement(true);
            for (int i = 0; i < info.Length; i++)
            {
                int[] cards = info[i].Cards.ToArray();
                UICowCow_GamerInfoComponent gic = gamers[info[i].SeatID].GetComponent<UICowCow_GamerInfoComponent>();
                UICowCow_SSGamerResultComponent ssgrc = gamers[info[i].SeatID].GetComponent<UICowCow_SSGamerResultComponent>();
                gic.SetStatus(UIGamerStatus.Down);
                gic.SetCoin(info[i].BetCoin.ToString());
            }
        }

        public void DealCardsGiveAllGamer(int seatId, int multiple)
        {
            this.ShowHideCardHeap(true);
            ResourcesComponent rc = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
            Dictionary<int, Gamer> gamers = GamerComponent.GetDictAll();
            Sprite cardBG = (Sprite)rc.GetAsset(UICowCowAB.CowCow_Texture, "CardBG");
            foreach (KeyValuePair<int, Gamer> gamer in gamers)
            {
                UICowCow_GamerInfoComponent gic = gamer.Value.GetComponent<UICowCow_GamerInfoComponent>();
                if (seatId == gamer.Key && gamer.Key == GamerComponent.LocalSeatID)
                {
                    //庄家
                    gic.SetBankerCards(cardBG, multiple);
                }
                else
                {
                    gic.SetCards(cardBG, gamer.Key == GamerComponent.LocalSeatID);
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
            res.UnloadBundle(UICowCowAB.CowCow_Prefabs);
            res.UnloadBundle(UICowCowAB.CowCow_Texture);
            smallSettlement?.Dispose();
            bigSettlement?.Dispose();
            gamerComponent?.Dispose();
            chatComponent?.Dispose();
            dissComponent?.Dispose();
            settingComponent?.Dispose();

            smallSettlement = null;
            bigSettlement = null;
            gamerComponent = null;
            chatComponent = null;
            uiChatVoice = null;
            dissComponent = null;
            settingComponent = null;
        }
    }
}

