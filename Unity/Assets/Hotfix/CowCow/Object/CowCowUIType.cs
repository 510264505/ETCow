using System.Collections.Generic;
using UnityEngine;

namespace ETHotfix
{
    /// <summary>
    /// AB包名
    /// </summary>
    public static partial class UICowCowAB
    {
        public const string CowCow_Prefabs = "cowcow_prefabs";
        public const string CowCow_Sound = "cowcow_sound";
        public const string CowCow_Texture = "cowcow_Texture";
    }
    public static partial class UICowCowType
    {
        public const string CowCowLogin = "UILoginCanvas";
        public const string CowCowLobby = "UILobbyCanvas";
        public const string CowCowGameRoom = "UIRoomCanvas";
        public const string CowCowRoomGamer = "UIRoomGamer";
        public const string CowCowRoomResult = "UIRoomResult";
        public const string CowCowSmallSettlement = "UIRoomSmallSettlement";
        public const string CowCowBigSettlement = "UIRoomBigSettlement";
        public const string CowCowSSPlayerResult = "UIRoomSSGamerResult";
        public const string CowCowBSPlayerResult = "UIRoomBSGamerResult";
        public const string CowCowGamerInfo = "UIGamerInfo";
    }
    public static partial class SoundCowCow
    {
        public const string W0 = "bf_0";
    }
    public enum UIGamerStatus
    {
        None,       //无
        Down,       //坐下
        Up,         //起立
        Ready,      //准备
        Playing,    //游戏中
        End,        //结束
        Offline,    //离线
    }

    public class GamerPosData
    {
        public Vector2 HeadPos { get; set; }
        public Vector2 CardPos { get; set; }
        public GamerPosData(Vector2 hPos, Vector2 cPos)
        {
            HeadPos = hPos;
            CardPos = cPos;
        }
    }
    /// <summary>
    /// 玩家数据
    /// </summary>
    public static class GamerData
    {
        public static Dictionary<int, GamerPosData> Pos = new Dictionary<int, GamerPosData>()
        {
            { 0, new GamerPosData(new Vector2(-300, -270), new Vector2(-290, -160)) },
            { 1, new GamerPosData(new Vector2( 300, -270), new Vector2( 290, -160)) },
            { 2, new GamerPosData(new Vector2( 500,    0), new Vector2( 350,    0)) },
            { 3, new GamerPosData(new Vector2( 350,  270), new Vector2( 330,  160)) },
            { 4, new GamerPosData(new Vector2(   0,  270), new Vector2(   0,  160)) },
            { 5, new GamerPosData(new Vector2(-400,  270), new Vector2(-330,  160)) },
            { 6, new GamerPosData(new Vector2(-600,    0), new Vector2(-350,    0)) },
        };
    }
}
