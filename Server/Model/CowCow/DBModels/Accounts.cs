using ETModel;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace ETModel
{
    [BsonIgnoreExtraElements]
    public class Accounts : Entity
    {
        public string Account { get; set; }
        public string Password { get; set; }
        public DateTime LoginTime { get; set; }
    }
}
