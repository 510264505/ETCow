using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ETModel
{
    /// <summary>
    /// User对象管理组件
    /// </summary>
    public class UserComponent : Component
    {
        //key:DB.Id, value:用户信息
        private readonly Dictionary<long, User> idUser = new Dictionary<long, User>();

        public void Add(User user)
        {
            this.idUser.Add(user.UserID, user);
        }
        public User Get(long id)
        {
            this.idUser.TryGetValue(id, out User user);
            return user;
        }
        public void Remove(long id)
        {
            this.idUser.Remove(id);
        }
        public int Count
        {
            get { return this.idUser.Count; }
        }
        public User[] GetAll()
        {
            return this.idUser.Values.ToArray();
        }
        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }
            base.Dispose();

            foreach (User user in this.idUser.Values)
            {
                user.Dispose();
            }
            this.idUser.Clear();
        }
    }
}
