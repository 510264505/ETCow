using ETModel;
using System;

namespace ETHotfix
{
    public class Accounts : Entity
    {
        public string Account { get; set; }
        public string Password { get; set; }
        public DateTime LoginTime { get; set; }
    }
}
