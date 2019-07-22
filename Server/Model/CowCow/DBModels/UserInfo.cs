using System;
using System.Collections.Generic;
using ETModel;
using MongoDB.Bson.Serialization.Attributes;

namespace ETModel
{
    /// <summary>
    /// 用户信息
    /// </summary>
    [BsonIgnoreExtraElements]
    public class UserInfo : Entity
    {
        //昵称
        public string NickName { get; set; }
        //头像
        public string HeadIcon { get; set; }
        //性别
        public int Sex { get; set; }
        //钻石
        public int Diamond { get; set; }
        //注册时间
        public DateTime RegisterTime { get; set; }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }
            base.Dispose();

            this.Id = 0;
        }
    }
}
