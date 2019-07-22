
namespace ETModel
{
    [ObjectSystem]
    public class UserAwakeSystem : AwakeSystem<User, long>
    {
        public override void Awake(User self, long id)
        {
            self.Awake(id);
        }
    }
    public sealed class User : Entity
    {
        //用户唯一ID    DB数据库的 _id
        public long UserID { get; set; }
        //Gate转发ID     是Gamer.Id
        public long ActorID { get; set; }
        public void Awake(long id)
        {
            this.UserID = id;
        }
        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }
            base.Dispose();

            this.UserID = 0;
            this.ActorID = 0;
        }
    }
}
