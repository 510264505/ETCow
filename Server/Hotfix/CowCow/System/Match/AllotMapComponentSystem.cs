using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class AllotRoomComponentStart : StartSystem<AllotRoomComponent>
    {
        public override void Start(AllotRoomComponent self)
        {
            self.Start();
        }
    }
    public static class AllotRoomComponentSystem
    {
        public static void Start(this AllotRoomComponent self)
        {
            StartConfig[] startConfigs = self.GetParent<Entity>().GetComponent<StartConfigComponent>().GetAll();
            foreach (StartConfig config in startConfigs)
            {
                if (!config.AppType.Is(AppType.Map))
                {
                    continue;
                }
                self.roomAddress.Add(config);
            }
        }

        public static StartConfig GetAddress(this AllotRoomComponent self)
        {
            int n = RandomHelper.RandomNumber(0, self.roomAddress.Count);
            return self.roomAddress[n];
        }
    }
}
