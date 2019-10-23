using ETModel;

namespace ETHotfix
{
    [Event(CowCowEventIdType.RemoveGameRoom)]
    public class CowCow_GameRoomRemove : AEvent
    {
        public override void Run()
        {
            Game.Scene.GetComponent<UIComponent>().Remove(UICowCowType.CowCowGameRoom);
            ETModel.Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle(UICowCowAB.CowCow_Prefabs);
        }
    }
}
