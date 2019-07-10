using System;
using System.Net;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
	[ObjectSystem]
	public class UICowCowLoginComponentSystem : AwakeSystem<UICowCowLoginComponent>
	{
		public override void Awake(UICowCowLoginComponent self)
		{
			self.Awake();
		}
	}
	
	public class UICowCowLoginComponent: Component
	{
        private GameObject backGround;
        private CanvasGroup uiLogin;
        private InputField account;
        private InputField password;
        private Button registeredBtn;
		private Button loginBtn;
        private CanvasGroup uiRegistered;
        private InputField rAccount;
        private InputField rPassword;
        private InputField rRePassword;
        private Button closeBtn;
        private Button rLoginBtn;

		public void Awake()
		{
			ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            backGround = rc.Get<GameObject>("BackGround");
            uiLogin = rc.Get<GameObject>("UILogin").GetComponent<CanvasGroup>();
            account = rc.Get<GameObject>("Account").GetComponent<InputField>();
            password = rc.Get<GameObject>("Password").GetComponent<InputField>();
            registeredBtn = rc.Get<GameObject>("RegisteredBtn").GetComponent<Button>();
            loginBtn = rc.Get<GameObject>("LoginBtn").GetComponent<Button>();
            uiRegistered = rc.Get<GameObject>("UIRegistered").GetComponent<CanvasGroup>();
            rAccount = rc.Get<GameObject>("RAccount").GetComponent<InputField>();
            rPassword = rc.Get<GameObject>("RPassword").GetComponent<InputField>();
            rRePassword = rc.Get<GameObject>("RRePassword").GetComponent<InputField>();
            closeBtn = rc.Get<GameObject>("CloseBtn").GetComponent<Button>();
            rLoginBtn = rc.Get<GameObject>("RLoginBtn").GetComponent<Button>();

            registeredBtn.onClick.Add(OnRegistered);
            loginBtn.onClick.Add(OnLogin);
            closeBtn.onClick.Add(OnClose);
            rLoginBtn.onClick.Add(OnRegistered);
        }
        private void ShowHideLogin(bool isShow)
        {
            uiLogin.alpha = isShow ? 1 : 0;
            uiLogin.blocksRaycasts = isShow;
            uiRegistered.alpha = !isShow ? 1 : 0;
            uiRegistered.blocksRaycasts = !isShow;
        }
        private void OnRegistered()
        {
            ShowHideLogin(false);
        }
        private void OnLogin()
		{
            Actor_LoginHelper.OnLoginAsync(account.text, password.text).Coroutine();
		}
        private void OnClose()
        {
            ShowHideLogin(true);
        }
        private void OnRLogin()
        {
            if (rPassword.text == rRePassword.text)
            {
                Actor_LoginHelper.OnRegisteredLoginAsync(rAccount.text, rPassword.text).Coroutine();
            }
            else
            {
                Debug.Log("两个密码不一样！");
            }
        }
	}
}
