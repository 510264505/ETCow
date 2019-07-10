namespace ETModel
{
    /// <summary>
    /// 房间配置
    /// </summary>
    public struct RoomConfig
    {
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
    }
}
