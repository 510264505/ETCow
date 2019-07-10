namespace ETModel
{
    public class TrusteeshipComponent : Component
    {
        public bool Playing { get; set; }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }
            base.Dispose();

            this.Playing = false;
        }
    }
}
