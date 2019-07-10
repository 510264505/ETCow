using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ETModel;
using UnityEngine.UI;

namespace ETHotfix
{
    public class UICowCow_GameRoomComponentAwake : AwakeSystem<UICowCow_GameRoomComponent, G2C_CowCowEnterGameRoomGate>
    {
        public override void Awake(UICowCow_GameRoomComponent self, G2C_CowCowEnterGameRoomGate data)
        {
            self.Awake(data);
        }
    }
    public class UICowCow_GameRoomComponent : Component
    {
        private GameObject BackGround { get; set; }
        private Text gameNameText;
        private Text gameRuleText;
        private Text nowTimeText;
        private GameObject UIGamer { get; set; }
        private Button phizBtn;
        private Button keyboardBtn;
        private Button voiceBtn;
        private Button dissBtn;
        private Button readyBtn;
        private Button inviteBtn;
        private CanvasGroup UISmallSettlement { get; set; }
        private GameObject SmallBG { get; set; }
        private Button sShareBtn;
        private Button sContinueBtn;
        private CanvasGroup UIBigSettlement { get; set; }
        private GameObject BigBG { get; set; }
        private Button bShareBtn;
        private Button bContinueBtn;

        public void Awake(G2C_CowCowEnterGameRoomGate data)
        {
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            BackGround = rc.Get<GameObject>("BackGround");
            gameNameText = rc.Get<GameObject>("GameName").GetComponent<Text>();
            gameRuleText = rc.Get<GameObject>("GameRule").GetComponent<Text>();
            nowTimeText = rc.Get<GameObject>("NowTime").GetComponent<Text>();
            UIGamer = rc.Get<GameObject>("UIGamer");
            phizBtn = rc.Get<GameObject>("PhizBtn").GetComponent<Button>();
            keyboardBtn = rc.Get<GameObject>("KeyboardBtn").GetComponent<Button>();
            voiceBtn = rc.Get<GameObject>("VoiceBtn").GetComponent<Button>();
            dissBtn = rc.Get<GameObject>("DissBtn").GetComponent<Button>();
            readyBtn = rc.Get<GameObject>("ReadyBtn").GetComponent<Button>();
            inviteBtn = rc.Get<GameObject>("InviteBtn").GetComponent<Button>();
            UISmallSettlement = rc.Get<GameObject>("UISmallSettlement").GetComponent<CanvasGroup>();
            SmallBG = rc.Get<GameObject>("SmallBG");
            sShareBtn = rc.Get<GameObject>("SShareBtn").GetComponent<Button>();
            sContinueBtn = rc.Get<GameObject>("SContinueBtn").GetComponent<Button>();
            UIBigSettlement = rc.Get<GameObject>("UIBigSettlement").GetComponent<CanvasGroup>();
            BigBG = rc.Get<GameObject>("BigBG");
            bShareBtn = rc.Get<GameObject>("BShareBtn").GetComponent<Button>();
            bContinueBtn = rc.Get<GameObject>("BContinueBtn").GetComponent<Button>();
        }

    }
}

