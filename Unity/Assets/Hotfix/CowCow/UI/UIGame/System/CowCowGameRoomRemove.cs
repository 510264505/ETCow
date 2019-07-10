using ETModel;

namespace ETHotfix
{
    [Event(EventIdCowCowType.RemoveGameRoom)]
    public class CowCowGameRoomRemove : AEvent
    {
        public override void Run()
        {
            Game.Scene.GetComponent<UIComponent>().Remove(UICowCowType.CowCowGameRoom);
            ETModel.Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle(UICowCowAB.CowCow_Prefabs.StringToAB());
        }
    }
}
