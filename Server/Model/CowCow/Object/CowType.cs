namespace ETModel
{
    public enum CardGroupType
    {
        None,
        FiveSmall,      //五小牛
        FourArticle,    //四条
        FiveFlowers,    //五花牛
        Gourd,          //葫芦
        CowCow,         //牛牛
        HaveCow,        //有牛
        NotCow          //无牛
    }
    public enum GamerStatus : ushort
    {
        None,       //无
        Down,       //坐下
        Up,         //起立
        Ready,      //准备
        Playing,    //游戏中
        End,        //结束
        Offline,    //离线
    }
}
