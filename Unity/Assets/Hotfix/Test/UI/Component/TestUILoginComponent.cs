using ETModel;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class TestUILoginComponentAwakeSystem : AwakeSystem<TestUILoginComponent>
    {
        public override void Awake(TestUILoginComponent self)
        {
            self.Awake();
        }
    }

    public class TestUILoginComponent : Component
    {
        private InputField account;
        private InputField password;
        private Button loginBtn;
        public void Awake()
        {
            //GameObject parent = this.GetParent<UI>().GameObject;
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            account = rc.Get<GameObject>("Account").GetComponent<InputField>();
            password = rc.Get<GameObject>("Password").GetComponent<InputField>();
            loginBtn = rc.Get<GameObject>("LoginBtn").GetComponent<Button>();
            //Debug.Log("此物体名称:" + parent.name);
            //account = parent.transform.Find("TestUILogin/Account").GetComponent<InputField>();
            //password = parent.transform.Find("TestUILogin/Password").GetComponent<InputField>();
            //loginBtn = parent.transform.Find("TestUILogin/LoginBtn").GetComponent<Button>();
            loginBtn.onClick.Add(() => OnLogin());
        }
        public async void OnLogin()
        {
            //Debug.Log("销毁自身!");
            //Game.Scene.GetComponent<UIComponent>().Remove(UIType.TestUILogin);
            //ETModel.Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle(UIType.TestUILogin);

            Debug.Log("账号:" + account.text + "   密码:" + password.text);
            //通过配置文件获取到服务器的IP和端口  开始连接到服务器，并返回一个Session
            ETModel.Session session = ETModel.Game.Scene.GetComponent<NetOuterComponent>().Create(GlobalConfigComponent.Instance.GlobalProto.Address);
            Session h_session = ComponentFactory.Create<Session, ETModel.Session>(session);
            G2C_TestPlayerInfo resp = (G2C_TestPlayerInfo)await h_session.Call(new C2G_TestPlayerInfo() { Account = account.text, Password = password.text });
            if (resp.Error == 0)
            {
                Debug.Log("注册成功！");
            }
            else
            {
                Log.Debug("注册失败！");
            }
            Debug.Log(resp.Message);
        }
    }
}
