using ETModel;

namespace ETHotfix
{
    [Event(EventIdType.InitSceneStart)]
    class TestScene_CreateLogin : AEvent
    {
        public override void Run()
        {
            UI ui = TestUILoginFactory.Create();
            Game.Scene.GetComponent<UIComponent>().Add(ui); //将创建的对象ui放在UIComponent对象下
        }
    }
}
