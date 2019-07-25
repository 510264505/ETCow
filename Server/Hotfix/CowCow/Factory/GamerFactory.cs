using ETModel;

namespace ETHotfix
{
    public static class GamerFactory
    {
        public static Gamer Create(long userId, long sessionId)
        {
            Gamer gamer = ComponentFactory.CreateWithId<Gamer>(userId);
            gamer.AddComponent<GamerGateComponent, long>(sessionId);
            return gamer;
        }
    }
}
