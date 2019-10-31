using ETModel;


namespace ETHotfix
{
    public static class Actor_LoginHelper
    {
        /// <summary>
        /// 登陆
        /// </summary>
        public static async ETVoid OnLoginAsync(string account, string password)
        {
            ETModel.Session session = ETModel.Game.Scene.GetComponent<NetOuterComponent>().Create(GlobalConfigComponent.Instance.GlobalProto.Address);
            Session realm_session = ComponentFactory.Create<Session, ETModel.Session>(session);
            //给登陆服务器发送登陆
            R2C_CowCowLogin r2cLogin = (R2C_CowCowLogin)await realm_session.Call(new C2R_CowCowLogin() { Account = account, Password = password });
            realm_session.Dispose();
            //将与消息服务器的链接session加入到SessionComponent组件
            if (r2cLogin.Error == ErrorCode.ERR_LoginError)
            {
                Log.Debug($"登录错误:{r2cLogin.Message}");
                PopupsHelper.ShowPopups($"登录错误:{r2cLogin.Message}");
                return;
            }
            Log.Debug("地址" + r2cLogin.Address);
            ETModel.Session gateSession = ETModel.Game.Scene.GetComponent<NetOuterComponent>().Create(r2cLogin.Address);
            if (ETModel.Game.Scene.GetComponent<ETModel.SessionComponent>() == null)
            {
                ETModel.Game.Scene.AddComponent<ETModel.SessionComponent>().Session = gateSession;
                Game.Scene.AddComponent<SessionComponent>().Session = ComponentFactory.Create<Session, ETModel.Session>(gateSession);
                SessionComponent.Instance.Session.AddComponent<PingComponent>();
            }
            else
            {
                Game.Scene.GetComponent<SessionComponent>().Session.Dispose();
                ETModel.Game.Scene.GetComponent<ETModel.SessionComponent>().Session = gateSession;
                Game.Scene.GetComponent<SessionComponent>().Session = ComponentFactory.Create<Session, ETModel.Session>(gateSession);
            }

            G2C_CowCowLoginGate g2cLoginGate = (G2C_CowCowLoginGate)await SessionComponent.Instance.Session.Call(new C2G_CowCowLoginGate() { Key = r2cLogin.Key });

            if (g2cLoginGate.Error == 0)
            {
                Log.Debug("登录成功！");
                Game.EventSystem.Run(CowCowEventIdType.LoginFinish, g2cLoginGate);
                ClientComponent clientComponent = ETModel.Game.Scene.GetComponent<ClientComponent>();
                clientComponent.User = ETModel.ComponentFactory.Create<User, long>(g2cLoginGate.UserID);
            }
            else
            {
                Log.Debug($"登录失败:{ g2cLoginGate.Message}");
                PopupsHelper.ShowPopups($"登录失败:{ g2cLoginGate.Message}");
                Game.EventSystem.Run(CowCowEventIdType.LoginFail, g2cLoginGate);
            }
        }
        /// <summary>
        /// 注册并登陆
        /// </summary>
        public static async ETVoid OnRegisteredLoginAsync(string account, string password)
        {
            ETModel.Session session = ETModel.Game.Scene.GetComponent<NetOuterComponent>().Create(GlobalConfigComponent.Instance.GlobalProto.Address);
            Session realm_session = ComponentFactory.Create<Session, ETModel.Session>(session);
            
            //给登陆服务器发送登陆
            R2C_CowCowRegister r2cLogin = (R2C_CowCowRegister)await realm_session.Call(new C2R_CowCowRegister() { Account = account, Password = password });
            realm_session.Dispose();
            if (r2cLogin.Error == 0)
            {
                Log.Debug("注册成功！");
                
                //将与消息服务器的链接session加入到SessionComponent组件
                ETModel.Session gateSession = ETModel.Game.Scene.GetComponent<NetOuterComponent>().Create(r2cLogin.Address);
                if (ETModel.Game.Scene.GetComponent<ETModel.SessionComponent>() == null)
                {
                    ETModel.Game.Scene.AddComponent<ETModel.SessionComponent>().Session = gateSession;
                    Game.Scene.AddComponent<SessionComponent>().Session = ComponentFactory.Create<Session, ETModel.Session>(gateSession);
                    SessionComponent.Instance.Session.AddComponent<PingComponent>();
                }
                else
                {
                    Game.Scene.GetComponent<SessionComponent>().Session.Dispose();
                    ETModel.Game.Scene.GetComponent<ETModel.SessionComponent>().Session = gateSession;
                    Game.Scene.GetComponent<SessionComponent>().Session = ComponentFactory.Create<Session, ETModel.Session>(gateSession);
                }

                G2C_CowCowLoginGate g2cLoginGate = (G2C_CowCowLoginGate)await SessionComponent.Instance.Session.Call(new C2G_CowCowLoginGate() { Key = r2cLogin.Key });

                if (g2cLoginGate.Error == 0)
                {
                    Log.Debug("登录成功！");
                    Game.EventSystem.Run(CowCowEventIdType.LoginFinish, g2cLoginGate);
                    ClientComponent clientComponent = ETModel.Game.Scene.GetComponent<ClientComponent>();
                    clientComponent.User = ETModel.ComponentFactory.Create<User, long>(g2cLoginGate.UserID);
                }
                else
                {
                    Log.Debug("登录失败:" + g2cLoginGate.Message);
                    PopupsHelper.ShowPopups($"登录失败:{ g2cLoginGate.Message}");
                    Game.EventSystem.Run(CowCowEventIdType.LoginFail, g2cLoginGate);
                }
            }
            else
            {
                Log.Debug($"注册失败:{r2cLogin.Message}");
                PopupsHelper.ShowPopups($"注册失败:{r2cLogin.Message}");
                Game.EventSystem.Run(CowCowEventIdType.RegisteredFail, r2cLogin);
            }
        }
    }
}

