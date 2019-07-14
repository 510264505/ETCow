using ETModel;

namespace ETHotfix
{
    public static class GamerFactory
    {
        public static Gamer Create(long actorId, long userId, long ? id = null)
        {
            Gamer gamer = ComponentFactory.CreateWithId<Gamer, long>(id ?? IdGenerater.GenerateId(), userId);
            gamer.ActorID = actorId;
            return gamer;
        }
    }
}
