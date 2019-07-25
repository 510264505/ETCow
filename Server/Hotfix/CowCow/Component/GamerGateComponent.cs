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

    public class GamerGateComponent : Component, ISerializeToEntity
    {
        public long GateSessionActorId; //是该用户的session.InstanceId

        public bool IsDisconnect;

        public void Awake(long gateSessionId)
        {
            this.GateSessionActorId = gateSessionId;
        }

        public ActorMessageSender GetActorMessageSender()
        {
            return Game.Scene.GetComponent<ActorMessageSenderComponent>().Get(this.GateSessionActorId);
        }
    }
}