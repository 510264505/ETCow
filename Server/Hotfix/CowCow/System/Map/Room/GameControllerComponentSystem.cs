using ETModel;
using System.Collections.Generic;

namespace ETHotfix
{
    [ObjectSystem]
    public class GameControllerComponentAwake : AwakeSystem<GameControllerComponent, RoomConfig>
    {
        public override void Awake(GameControllerComponent self, RoomConfig config)
        {
            self.Awake(config);
        }
    }
    public static class GameControllerComponentSystem
    {
        public static void Awake(this GameControllerComponent self, RoomConfig config)
        {
            self.Config = config;
            self.BureauCount = config.BureauCount;
            self.BaseScore = config.BaseScore;
            self.Multiples = config.Multiples;
            self.Rule = config.Rule;
            self.PeopleCount = config.PeopleCount;
        }

        public static void DealCards(this GameControllerComponent self)
        {
            Room room = self.GetParent<Room>();

            room.GetComponent<DeskComponent>().Shuffle();

            Dictionary<int, Gamer> gamers = room.GetAll();
            int index = 0;
            for (int i = 0; i < 51; i++)
            {

            }
        }

        public static void DealTo(this GameControllerComponent self, long id)
        {
            Room room = self.GetParent<Room>();
            Card card = room.GetComponent<DeskComponent>().Deal();
            if (id == room.InstanceId)
            {
                DeskCardCacheComponent deskCardsCache = room.GetComponent<DeskCardCacheComponent>();
                //deskCardsCache.
            }
        }
    }
}
