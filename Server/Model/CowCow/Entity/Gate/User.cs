
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
        //用户唯一ID
        public long UserID { get; set; }
        //是否正则匹配中
        public bool IsMatching { get; set; }
        //Gate转发ID
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

            this.IsMatching = false;
            this.ActorID = 0;
        }
    }
}
