using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    public static class UserFactory
    {
        public static User Create(long userId, long sessionId)
        {
            User user = ComponentFactory.Create<User, long>(userId);
            user.AddComponent<UnitGateComponent, long>(sessionId);
            Game.Scene.GetComponent<UserComponent>().Add(user);
            return user;
        }
    }
}
