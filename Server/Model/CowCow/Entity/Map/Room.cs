using System.Collections.Generic;

namespace ETModel
{
    public enum RoomState
    {
        None,
        Wait,
        End,
        Game,
    }
    public sealed class Room : Entity
    {
        public readonly Dictionary<int, Gamer> gamers = new Dictionary<int, Gamer>();
        public readonly Dictionary<int, bool> dissoltions = new Dictionary<int, bool>();
        public readonly List<int> grabBankers = new List<int>();
        public string GameName { get; set; }
        //创建房间的玩家ID
        public long UserID { get; set; }
        public string RoomID { get; set; }
        public int Bureau { get; set; }
        public int RuleBit { get; set; }
        public RoomState State { get; set; } = RoomState.None;
        public int GamerCount { get { return gamers.Values.Count; } }
        public int DissoltionCount { get { return dissoltions.Values.Count; } }
        public int PeopleCount { get; set; }
        //当前庄家
        public int CurBankerSeatID { get; set; }
        //当前赔率
        public int CurMultiple { get; set; }
        //当前局数
        public int CurBureau { get; set; }
        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }
            base.Dispose();

            foreach (Gamer gamer in this.gamers.Values)
            {
                if (gamer != null)
                {
                    gamer.Dispose();
                }
            }
            this.gamers.Clear();
            this.GameName = string.Empty;
            this.UserID = 0;
            this.RoomID = string.Empty;
            this.Bureau = 0;
            this.RuleBit = 0;
            this.State = RoomState.None;
        }
    }
}
