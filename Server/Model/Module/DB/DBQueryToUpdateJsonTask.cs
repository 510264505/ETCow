using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace ETModel
{
    [ObjectSystem]
    public class DBQueryToUpdateJsonTaskAwakeSystem : AwakeSystem<DBQueryToUpdateJsonTask, string, string, ETTaskCompletionSource>
    {
        public override void Awake(DBQueryToUpdateJsonTask self, string collectionName, string json, ETTaskCompletionSource tcs)
        {
            self.CollectionName = collectionName;
            self.Json = json;
            self.Tcs = tcs;
        }
    }
    public sealed class DBQueryToUpdateJsonTask : DBTask
    {
        public string CollectionName { get; set; }
        public string Json { get; set; }
        public ETTaskCompletionSource Tcs { get; set; }
        public override async ETTask Run()
        {
            DBComponent dbComponent = Game.Scene.GetComponent<DBComponent>();
            try
            {
                BsonDocument bsons = BsonSerializer.Deserialize<BsonDocument>(this.Json);
                string QueryJson = bsons["QueryJson"].ToJson();
                var set = bsons["UpdateJson"];
                bsons.Clear();
                bsons["$set"] = set;
                string UpdateJson = bsons.ToJson();
                FilterDefinition<ComponentWithId> queryFilter = new JsonFilterDefinition<ComponentWithId>(QueryJson);
                UpdateDefinition<ComponentWithId> updateFilter = new JsonUpdateDefinition<ComponentWithId>(UpdateJson);
                await dbComponent.GetCollection(this.CollectionName).UpdateOneAsync(queryFilter, updateFilter);
                this.Tcs.SetResult();
            }
            catch (Exception e)
            {
                this.Tcs.SetException(new Exception($"更改数据库异常! {CollectionName} {this.Json}", e));
            }
        }
    }
}
