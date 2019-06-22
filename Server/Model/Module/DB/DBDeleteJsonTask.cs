using System;
using MongoDB.Driver;

namespace ETModel
{
    [ObjectSystem]
    public class DBDeleteJsonTaskAwakeSystem : AwakeSystem<DBDeleteJsonTask, string, string, ETTaskCompletionSource>
    {
        public override void Awake(DBDeleteJsonTask self, string collectionName, string json, ETTaskCompletionSource tcs)
        {
            self.CollectionName = collectionName;
            self.Json = json;
            self.Tcs = tcs;
        }
    }
    public sealed class DBDeleteJsonTask : DBTask
    {
        public string CollectionName { get; set; }
        public string Json { get; set; }
        public ETTaskCompletionSource Tcs { get; set; }
        public override async ETTask Run()
        {
            DBComponent dbComponent = Game.Scene.GetComponent<DBComponent>();
            try
            {
                FilterDefinition<ComponentWithId> filter = new JsonFilterDefinition<ComponentWithId>(this.Json);
                await dbComponent.GetCollection(this.CollectionName).DeleteManyAsync(filter);
                this.Tcs.SetResult();
            }
            catch(Exception e)
            {
                this.Tcs.SetException(new Exception($"删除数据库异常! {CollectionName} {this.Json}", e));
            }
        }
    }
}
