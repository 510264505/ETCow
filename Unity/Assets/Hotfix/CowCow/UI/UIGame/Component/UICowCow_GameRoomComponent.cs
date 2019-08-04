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
        private Dictionary<int, string> rules = new Dictionary<int, string>()
        {
            { 0x1, "五小牛" },
            { 0x2, "炸弹牛" },
            { 0x4, "五花牛" },
            { 0x8, "葫芦牛" },
        };
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
                    UI ui = this.GetParent<UI>();
                    gamerComponent = ui.AddComponent<GamerComponent>();
                }
                return gamerComponent;
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

            phizBtn.onClick.Add(Onphiz);
            keyboardBtn.onClick.Add(OnKeyboard);
            voiceBtn.onClick.Add(OnVoice);
            dissBtn.onClick.Add(OnDiss);
            readyBtn.onClick.Add(OnReady);
            inviteBtn.onClick.Add(OnInvite);

            isShowTime = true;
            NowTime(nowTime).Coroutine();

            Game.EventSystem.Run(EventIdCowCowType.RemoveLobby);
        }

        public void Init(string gameName, int bureau, int ruleBit, string roomId)
        {
            this.bureauCount = bureau;
            this.gameName.text = gameName;
            this.gameRule.text = Rule(ruleBit);
            this.roomId.text = roomId;
            this.RoomID = roomId;
            Bureau();
        }
        
        public void AddGamer(GamerInfo info, int posIndex)
        {
            Gamer gamer = GamerComponent.AddGamerUI(info);
            if (gamer != null)
            {
                gamer.AddComponent<UICowCow_GamerInfoComponent, GameObject, GamerInfo, int>(UIRoomGamer, info, posIndex);
                gamer.AddComponent<UICowCow_SSGamerResultComponent, GameObject>(SmallSettlement.SmallBG);
                gamer.AddComponent<UICowCow_BSGamerResultComponent, GameObject>(BigSettlement.BigBG);
            }
            if (GamerComponent.LocalGamer == null)
            {
                if (info.UserID == ETModel.Game.Scene.GetComponent<ClientComponent>().User.UserID)
                {
                    GamerComponent.Init(info.UserID);
                    if (GamerComponent.LocalGamer.GetComponent<UICowCow_GamerInfoComponent>().Status == UIGamerStatus.Down)
                    {
                        ShowHideReadyButton(true);
                    }
                }
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

        public void ShowHideReadyButton(bool isShow)
        {
            readyBtn.GetComponent<CanvasGroup>().alpha = isShow ? 1 : 0;
            readyBtn.GetComponent<CanvasGroup>().blocksRaycasts = isShow;
        }

        public void GamerReady(int[] seatIds)
        {
            for (int i = 0; i < seatIds.Length; i++)
            {
                UICowCow_GamerInfoComponent gc = GamerComponent.Get(seatIds[i]).GetComponent<UICowCow_GamerInfoComponent>();
                if (gc.Status != UIGamerStatus.Ready)
                {
                    gc.SetStatus(UIGamerStatusString.Ready, UIGamerStatus.Ready);
                }
            }
        }

        private string Rule(int ruleBit)
        {
            sb.Clear();
            foreach (var rule in this.rules)
            {
                if ((ruleBit & rule.Key) == rule.Key)
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
        private void Onphiz()
        {
            //表情
        }
        private void OnKeyboard()
        {

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

        public void OpenAllGamerHandCard(CowCowSmallSettlementInfo[] info)
        {
            Dictionary<int,Gamer> gamers = GamerComponent.GetDictAll();
            for (int i = 0; i < info.Length; i++)
            {
                int[] cards = info[i].Cards.ToArray();
                UICowCow_GamerInfoComponent gic = gamers[info[i].SeatID].GetComponent<UICowCow_GamerInfoComponent>();
                UICowCow_SSGamerResultComponent ssgrc = gamers[info[i].SeatID].GetComponent<UICowCow_SSGamerResultComponent>();
                gic.SetCards(cards);
                ssgrc.SetGamerSmallSettlement(info[i].SeatID, gic.gamerName, info[i].BetCoin, info[i].CardsType, info[i].LoseWin, cards);
            }
            //在此延迟显示小结算
            SmallSettlement.ShowHideSmallSettlement(true, 5000).Coroutine();
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

