using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    public static class UserInfoFactory
    {
        public static UserInfo Create(long userId, long sessionId)
        {
            UserInfo userInfo = ComponentFactory.Create<UserInfo, long>(userId);
            userInfo.AddComponent<UnitGateComponent, long>(sessionId);
            Game.Scene.GetComponent<UserInfoComponent>().Add(userInfo);
            return userInfo;
        }
    }
}
