using System.Collections.Generic;

namespace ETModel
{
    /// <summary>
    /// 牌库组件
    /// </summary>
    public class DeskComponent : Component
    {
        public readonly List<Card> library = new List<Card>();

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }
            base.Dispose();

            this.library.Clear();
        }
    }
}
