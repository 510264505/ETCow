using ETModel;

namespace ETHotfix
{
    [Event(GlobalsEventType.CreateUIPopups)]
    public class InitCreateUIPopups : AEvent
    {
        public override void Run()
        {
            UI ui = UIPopupsFactory.Create();
            Game.Scene.GetComponent<UIComponent>().Add(ui);
        }
    }
}
