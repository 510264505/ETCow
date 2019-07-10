namespace ETModel
{
    /// <summary>
    /// Session关联user对象组件，用于Session断开触发下线
    /// </summary>
    public class SessionUserComponent : Component
    {
        public User User { get; set; }
    }
}
