using System.Collections.Generic;
using System.Linq;

namespace ETModel
{
    /// <summary>
    /// UserInfo对象管理组件
    /// </summary>
    public class UserInfoComponent : Component
    {
        //key:DB.Id, value:用户信息
        private readonly Dictionary<long, UserInfo> userInfos = new Dictionary<long, UserInfo>();

        public void Add(UserInfo userInfo)
        {
            this.userInfos.Add(userInfo.Id, userInfo);
        }
        public UserInfo Get(long id)
        {
            this.userInfos.TryGetValue(id, out UserInfo userInfo);
            return userInfo;
        }
        public void Remove(long id)
        {
            this.userInfos.Remove(id);
        }
        public int Count
        {
            get { return this.userInfos.Count; }
        }
        public UserInfo[] GetAll()
        {
            return this.userInfos.Values.ToArray();
        }
        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }
            base.Dispose();

            foreach (UserInfo userInfo in this.userInfos.Values)
            {
                userInfo.Dispose();
            }
            this.userInfos.Clear();
        }
    }
}
