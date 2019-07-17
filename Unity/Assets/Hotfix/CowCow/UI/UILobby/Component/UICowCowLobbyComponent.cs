using ETModel;
using System;
using System.Collections.Generic;
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
        private Button quitBtn;
        private Button settingBtn;
        private Button bulletinBtn;
        private Button rankBtn;
        private Button serviceBtn;
        private Button shareBtn;
        private Button mallBtn;

        private int bureauLen = 2;
        private int ruleLen = 4;
        private int numLen = 6;
        private int numberLen = 10;
        private List<int> roomNumber = new List<int>();

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
            quitBtn = rc.Get<GameObject>("QuitBtn").GetComponent<Button>();
            settingBtn = rc.Get<GameObject>("SettingBtn").GetComponent<Button>();
            bulletinBtn = rc.Get<GameObject>("BulletinBtn").GetComponent<Button>();
            rankBtn = rc.Get<GameObject>("RankBtn").GetComponent<Button>();
            serviceBtn = rc.Get<GameObject>("ServiceBtn").GetComponent<Button>();
            shareBtn = rc.Get<GameObject>("ShareBtn").GetComponent<Button>();
            mallBtn = rc.Get<GameObject>("MallBtn").GetComponent<Button>();

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

            ShowPlayerInfo(data);
        }
        private void ShowPlayerInfo(G2C_CowCowLoginGate data)
        {
            //头像还没确定好
            //Texture2D texture2D = new Texture2D((int)headIcon.transform.GetComponent<Rect>().width, (int)headIcon.transform.GetComponent<Rect>().height);
            //texture2D.LoadImage(Convert.FromBase64String(data.PlayerInfo.HeadIcon));
            //headIcon.sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
            nameText.text = data.GamerInfo.Name;
            roomCardText.text = data.GamerInfo.Diamond.ToString();
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
            ShowHideCreateRoomWindows(true);
        }
        private void OnJoinRoom()
        {
            ShowHideJoinRoomWindows(true);
        }
        private void OnCreate()
        {
            //向服务器发送创建房间
            long userId = ETModel.Game.Scene.GetComponent<ClientComponent>().User.UserID;
            Actor_CreateRoomHelper.OnCreateGameRoom("牛牛", userId, 1, 1).Coroutine();
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
            roomNumber.Add(n);
            numText[roomNumber.Count - 1].text = n.ToString();
            if (roomNumber.Count >= numLen)
            {
                //向服务器发送加入房间
                OnRepeat();
            }
        }
        private void OnQuit()
        {
            Game.EventSystem.Run(EventIdCowCowType.InitScensStart);
            Game.EventSystem.Run(EventIdCowCowType.RemoveLobby);
        }
        private void OnSetting()
        {

        }
        private void OnBulletin()
        {

        }
        private void OnRank()
        {

        }
        private void OnService()
        {

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
		

	}
}
