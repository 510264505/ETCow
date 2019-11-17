using ETModel;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
	[ObjectSystem]
	public class UiLobbyComponentSystem : AwakeSystem<UICowCowLobbyComponent, G2C_CowCowLoginGate>
	{
		public override void Awake(UICowCowLobbyComponent self, G2C_CowCowLoginGate data)
		{
			self.Awake(data);
		}
	}
	
	public class UICowCowLobbyComponent : Component
	{
		private GameObject backGround;
        private Button createRoomBtn;
        private Button joinRoomBtn;
        private CanvasGroup createRoomWindows;
        private CanvasGroup joinRoomWindows;
        private Toggle[] bureauTog;
        private Toggle[] ruleTog;
        private Dropdown peopleDrop;
        private Button createBtn;
        private Button closeCreateBtn;
        private Button closeJoinBtn;
        private Text[] numText;
        private Button[] numberBtn;
        private Button backSpaceBtn;
        private Button repeatBtn;
        private Image headIcon;
        private Text nameText;
        private Text roomCardText;
        private GameObject uiWindow;

        private int bureauLen = 2;
        private int ruleLen = 3;
        private int numLen = 6;
        private int numberLen = 10;
        private List<string> roomNumber = new List<string>();
        private StringBuilder roomId = new StringBuilder();

        private LobbySetting lobbySetting;
        private LobbyServer lobbyServer;
        private LobbyBulletin lobbyBulletin;
        public LobbySetting LobbySetting
        {
            get
            {
                if (lobbySetting == null)
                {
                    lobbySetting = new LobbySetting(uiWindow);
                }
                return lobbySetting;
            }
        }
        public LobbyServer LobbyServer
        {
            get
            {
                if (lobbyServer == null)
                {
                    lobbyServer = new LobbyServer(uiWindow);
                }
                return lobbyServer;
            }
        }
        public LobbyBulletin LobbyBulletin
        {
            get
            {
                if (lobbyBulletin == null)
                {
                    lobbyBulletin = new LobbyBulletin(uiWindow);
                }
                return lobbyBulletin;
            }
        }

		public void Awake(G2C_CowCowLoginGate data)
		{
			ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            backGround = rc.Get<GameObject>("BackGround");
            createRoomBtn = rc.Get<GameObject>("CreateRoomBtn").GetComponent<Button>();
            joinRoomBtn = rc.Get<GameObject>("JoinRoomBtn").GetComponent<Button>();
            createRoomWindows = rc.Get<GameObject>("CreateRoomWindows").GetComponent<CanvasGroup>();
            joinRoomWindows = rc.Get<GameObject>("JoinRoomWindows").GetComponent<CanvasGroup>();
            bureauTog = new Toggle[bureauLen];
            for (int i = 0; i < bureauTog.Length; i++)
            {
                bureauTog[i] = rc.Get<GameObject>("BToggle" + i).GetComponent<Toggle>();
            }
            ruleTog = new Toggle[ruleLen];
            for (int i = 0; i < ruleTog.Length; i++)
            {
                ruleTog[i] = rc.Get<GameObject>("RToggle" + i).GetComponent<Toggle>();
            }
            peopleDrop = rc.Get<GameObject>("PDropdown0").GetComponent<Dropdown>();
            createBtn = rc.Get<GameObject>("CreateBtn").GetComponent<Button>();
            closeCreateBtn = rc.Get<GameObject>("CloseCreateBtn").GetComponent<Button>();
            closeJoinBtn = rc.Get<GameObject>("CloseJoinBtn").GetComponent<Button>();
            numText = new Text[numLen];
            for (int i = 0; i < numText.Length; i++)
            {
                numText[i] = rc.Get<GameObject>("Num" + i).GetComponent<Text>();
            }
            numberBtn = new Button[numberLen];
            for (int i = 0; i < numberBtn.Length; i++)
            {
                int n = i;
                numberBtn[i] = rc.Get<GameObject>("NumberButton" + i).GetComponent<Button>();
                numberBtn[i].onClick.Add(() => { OnNumber(n); });
            }
            backSpaceBtn = rc.Get<GameObject>("BackSpaceButton").GetComponent<Button>();
            repeatBtn = rc.Get<GameObject>("RepeatButton").GetComponent<Button>();
            headIcon = rc.Get<GameObject>("HeadIcon").GetComponent<Image>();
            nameText = rc.Get<GameObject>("Name").GetComponent<Text>();
            roomCardText = rc.Get<GameObject>("RoomCard").GetComponent<Text>();
            Button quitBtn = rc.Get<GameObject>("QuitBtn").GetComponent<Button>();
            Button settingBtn = rc.Get<GameObject>("SettingBtn").GetComponent<Button>();
            Button bulletinBtn = rc.Get<GameObject>("BulletinBtn").GetComponent<Button>();
            Button rankBtn = rc.Get<GameObject>("RankBtn").GetComponent<Button>();
            Button serviceBtn = rc.Get<GameObject>("ServiceBtn").GetComponent<Button>();
            Button shareBtn = rc.Get<GameObject>("ShareBtn").GetComponent<Button>();
            Button mallBtn = rc.Get<GameObject>("MallBtn").GetComponent<Button>();
            uiWindow = rc.Get<GameObject>("UIWindow");

            createRoomBtn.onClick.Add(OnCreateRoom);
            joinRoomBtn.onClick.Add(OnJoinRoom);
            createBtn.onClick.Add(OnCreate);
            closeCreateBtn.onClick.Add(OnCloseCreateRoomWindows);
            closeJoinBtn.onClick.Add(OnCloseJoinRoomWindows);
            backSpaceBtn.onClick.Add(OnBackSpace);
            repeatBtn.onClick.Add(OnRepeat);
            quitBtn.onClick.Add(OnQuit);
            settingBtn.onClick.Add(OnSetting);
            bulletinBtn.onClick.Add(OnBulletin);
            rankBtn.onClick.Add(OnRank);
            serviceBtn.onClick.Add(OnService);
            shareBtn.onClick.Add(OnShare);
            mallBtn.onClick.Add(OnMall);

            this.ShowPlayerInfo(data);
        }
        private void ShowPlayerInfo(G2C_CowCowLoginGate data)
        {
            //头像还没确定好
            //Texture2D texture2D = new Texture2D((int)headIcon.transform.GetComponent<Rect>().width, (int)headIcon.transform.GetComponent<Rect>().height);
            //texture2D.LoadImage(Convert.FromBase64String(data.PlayerInfo.HeadIcon));
            //headIcon.sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
            nameText.text = data.NickName;
            roomCardText.text = data.Diamond.ToString();
        }
        private void ShowHideCreateRoomWindows(bool isShow)
        {
            createRoomWindows.alpha = isShow ? 1 : 0;
            createRoomWindows.blocksRaycasts = isShow;
        }
        private void ShowHideJoinRoomWindows(bool isShow)
        {
            joinRoomWindows.alpha = isShow ? 1 : 0;
            joinRoomWindows.blocksRaycasts = isShow;
        }
        private void OnCreateRoom()
        {
            this.ShowHideCreateRoomWindows(true);
        }
        private void OnJoinRoom()
        {
            this.ShowHideJoinRoomWindows(true);
        }
        private void OnCreate()
        {
            //向服务器发送创建房间
            int bureau = 0;
            int ruleBit = 0;
            for (int i = 0; i < bureauTog.Length; i++)
            {
                if (bureauTog[i].isOn)
                {
                    bureau = i;
                    break;
                }
            }
            for (int i = 0; i < ruleTog.Length; i++)
            {
                if (ruleTog[i].isOn)
                {
                    //五花牛是0x2开始的，所以加 +1
                    ruleBit += 1 << (i + 1);
                }
            }
            long userId = ETModel.Game.Scene.GetComponent<ClientComponent>().User.UserID;
            Actor_CreateRoomHelper.OnCreateGameRoom("牛欢喜", userId, bureau, ruleBit, peopleDrop.value + 2).Coroutine();
            this.ShowHideCreateRoomWindows(false);
        }
        private void OnCloseCreateRoomWindows()
        {
            ShowHideCreateRoomWindows(false);
        }
        private void OnCloseJoinRoomWindows()
        {
            ShowHideJoinRoomWindows(false);
        }
        private void OnBackSpace()
        {
            int len = roomNumber.Count - 1;
            numText[len].text = string.Empty;
            roomNumber.RemoveAt(len);
        }
        private void OnRepeat()
        {
            roomNumber.Clear();
            for (int i = 0; i < numText.Length; i++)
            {
                numText[i].text = string.Empty;
            }
        }
        private void OnNumber(int n)
        {
            roomId.Clear();
            string num = n.ToString();
            roomNumber.Add(num);
            numText[roomNumber.Count - 1].text = num;
            if (roomNumber.Count >= numLen)
            {
                //向服务器发送加入房间
                long userId = ETModel.Game.Scene.GetComponent<ClientComponent>().User.UserID;
                for (int i = 0; i < roomNumber.Count; i++)
                {
                    roomId.Append(roomNumber[i]);
                }
                Actor_JoinRoomHelper.OnJoinRoomAsync(userId, roomId.ToString()).Coroutine();
                this.ShowHideJoinRoomWindows(false);
                OnRepeat();
            }
        }
        private void OnQuit()
        {
            Game.EventSystem.Run(CowCowEventIdType.RemoveLobby);
            Game.Scene.GetComponent<SessionComponent>().Session.RemoveComponent<PingComponent>();
            Game.Scene.GetComponent<SessionComponent>().Session.Dispose();
        }
        private void OnSetting()
        {
            LobbySetting.ShowHideLobbySetting(true);
        }
        private void OnBulletin()
        {
            LobbyBulletin.ShowHideLobbyServer(true);
        }
        private void OnRank()
        {

        }
        private void OnService()
        {
            LobbyServer.ShowHideLobbyServer(true);
        }
        private void OnShare()
        {

        }
        private void OnMall()
        {

        }
		private void EnterMap()
		{
			MapHelper.EnterMapAsync().Coroutine();
		}

        public void SetDiamond(int diamond)
        {
            roomCardText.text = diamond.ToString();
        }

        public void ShowHideLobby(bool isShow)
        {
            Actor_RefreshDiamondHelper.OnRefreshDiamond().Coroutine();
            this.GetParent<UI>().GameObject.SetActive(isShow);
        }


        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }
            base.Dispose();

            LobbySetting.Destroy();
            LobbyServer.Destroy();
            LobbyBulletin.Destroy();
            this.lobbySetting = null;
            this.lobbyServer = null;
            this.lobbyBulletin = null;
        }
    }
}
