using ETModel;

namespace ETHotfix
{
    //Session断开时触发下线
    public class SessionOfflineComponent : Component
    {
        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }
            base.Dispose();

            //移除连接组件
            Game.Scene.RemoveComponent<SessionComponent>();
            ETModel.Game.Scene.RemoveComponent<ETModel.SessionComponent>();

            //释放本地玩家对象
            ClientComponent clientComponent = ETModel.Game.Scene.GetComponent<ClientComponent>();
            if (clientComponent.User != null)
            {
                clientComponent.User.Dispose();
                clientComponent.User = null;
            }

            //游戏关闭，不用回到登录界面
            UIComponent ui = Game.Scene.GetComponent<UIComponent>();
            if (ui == null || ui.IsDisposed)
            {
                return;
            }

            //跳转到登录界面，并删除大厅和房间的UI
        }
    }
}
