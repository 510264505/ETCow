namespace ETHotfix
{
    public class Gamer : Entity
    {
        //玩家唯一ID
        public long UserID { get; set; }
        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }
            base.Dispose();

            this.UserID = 0;
        }
    }
}

