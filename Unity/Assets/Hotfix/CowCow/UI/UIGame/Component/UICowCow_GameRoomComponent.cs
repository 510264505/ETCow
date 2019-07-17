using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ETModel;
using UnityEngine.UI;
using System.Text;
using System;
using System.Threading;

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

        private void ReSet()
        {
            sb.Clear();
            curBureauCount = 1;
            isShowTime = false;
        }
        public void Awake()
        {
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            BackGround = rc.Get<GameObject>("BackGround");
            UIRoomGamer = rc.Get<GameObject>("UIRoomGamer");
            this.bureau = rc.Get<GameObject>("GameBureau").GetComponent<Text>();
            this.gameName = rc.Get<GameObject>("GameName").GetComponent<Text>();
            this.gameRule = rc.Get<GameObject>("GameRule").GetComponent<Text>();
            Text nowTime = rc.Get<GameObject>("NowTime").GetComponent<Text>();
            Button phizBtn = rc.Get<GameObject>("PhizBtn").GetComponent<Button>();
            Button keyboardBtn = rc.Get<GameObject>("KeyboardBtn").GetComponent<Button>();
            Button voiceBtn = rc.Get<GameObject>("VoiceBtn").GetComponent<Button>();
            Button dissBtn = rc.Get<GameObject>("DissBtn").GetComponent<Button>();
            Button readyBtn = rc.Get<GameObject>("ReadyBtn").GetComponent<Button>();
            Button inviteBtn = rc.Get<GameObject>("InviteBtn").GetComponent<Button>();

            phizBtn.onClick.Add(Onphiz);
            keyboardBtn.onClick.Add(OnKeyboard);
            voiceBtn.onClick.Add(OnVoice);
            dissBtn.onClick.Add(OnDiss);
            readyBtn.onClick.Add(OnReady);
            inviteBtn.onClick.Add(OnInvite);

            isShowTime = true;
            NowTime(nowTime).Coroutine();
        }

        public void Init(string gameName, int bureau, int ruleBit)
        {
            this.bureauCount = bureau;
            this.gameName.text = gameName;
            this.gameRule.text = Rule(ruleBit);
            Bureau();
        }
        
        public void AddGamer(GamerInfo info)
        {
            Gamer gamer = GamerComponent.AddGamerUI(info);
            if (gamer != null)
            {
                gamer.AddComponent<UICowCow_GamerInfoComponent, GameObject, GamerInfo>(UIRoomGamer, info);
                gamer.AddComponent<UICowCow_SSGamerResultComponent, GameObject>(SmallSettlement.SmallBG);
                gamer.AddComponent<UICowCow_BSGamerResultComponent, GameObject>(BigSettlement.BigBG);
            }
        }

        public void RemoveGamer(long id)
        {
            GamerComponent.Remove(id);
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

