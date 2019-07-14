namespace ETHotfix
{
    public class Gamer : Entity
    {
        //玩家唯一ID
        public long UserID { get; set; }
        //是否准备
        public UIGamerStatus Status { get; set; }
        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }
            base.Dispose();

            this.UserID = 0;
            this.Status = UIGamerStatus.None;
        }
    }
}

