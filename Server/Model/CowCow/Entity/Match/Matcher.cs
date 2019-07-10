namespace ETModel
{
    [ObjectSystem]
    public class MatcherAwakeSystem : AwakeSystem<Matcher, long>
    {
        public override void Awake(Matcher self, long id)
        {
            self.Awake(id);
        }
    }
    public sealed class Matcher : Entity
    {
        public long UserID { get; set; }
        public long ActorID { get; set; }
        //客户端与网关服务器的SessionID
        public long GateSessionID { get; set; }
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
            this.GateSessionID = 0;
        }
    }
}
