namespace ETModel
{
    public class GameControllerComponent : Component
    {
        //房间配置
        public RoomConfig Config { get; set; }
        //局数
        public int BureauCount { get; set; }
        //底分
        public int BaseScore { get; set; }
        //房间初始赔率
        public int Multiples { get; set; }
        //规则
        public string Rule { get; set; }
        //人数
        public int PeopleCount { get; set; }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }
            base.Dispose();

            this.BureauCount = 0;
            this.BaseScore = 0;
            this.Multiples = 0;
            this.Rule = string.Empty;
            this.PeopleCount = 0;
        }
    }
}
