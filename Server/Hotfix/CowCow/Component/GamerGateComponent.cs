namespace ETModel
{
    [ObjectSystem]
    public class GamerGateComponentAwakeSystem : AwakeSystem<GamerGateComponent, long>
    {
        public override void Awake(GamerGateComponent self, long a)
        {
            self.Awake(a);
        }
    }
    [ObjectSystem]
    public class GamerGateComponentLoad : LoadSystem<GamerGateComponent>
    {
        public override void Load(GamerGateComponent self)
        {
            self.Load();
        }
    }

    public class GamerGateComponent : Component, ISerializeToEntity
    {
        public long GateSessionActorId; //是该用户的session.InstanceId

        public bool IsDisconnect;

        public void Awake(long gateSessionId)
        {
            this.GateSessionActorId = gateSessionId;
        }

        public void Load()
        {
            Log.WriteLine("控制台输入reload就能热更服务器，执行这个方法吗?");
        }

        public ActorMessageSender GetActorMessageSender()
        {
            return Game.Scene.GetComponent<ActorMessageSenderComponent>().Get(this.GateSessionActorId);
        }
    }
}