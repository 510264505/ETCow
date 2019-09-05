using System.Collections.Generic;
using UnityEngine;

namespace ETHotfix
{
    /// <summary>
    /// AB包名
    /// </summary>
    public static partial class UICowCowAB
    {
        public const string CowCow_Prefabs = "cowcow_prefabs.unity3d";
        public const string CowCow_Sound = "cowcow_sound.unity3d";
        public const string CowCow_Texture = "cowcow_texture.unity3d";
        public const string CowCow_SoundOther = "cowcow_soundother.unity3d";
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
        public const string CowcowChat = "UIChat";
        public const string CowCowChatContentBtn = "ChatContentBtn";
        public const string CowCowChatVoice = "UIChatVoice";
        public const string CowCowDissoltion = "UIDissoltion";
        public const string CowCowGamerDissInfo = "GamerDissInfo";
        public const string CowCowGameSetting = "UIGameSetting";
    }
    public static partial class SoundCowCow
    {
        public const string W0 = "bf_0";
    }
    public enum UIGamerStatus : ushort
    {
        None,       //无
        Down,       //坐下
        Up,         //起立
        Ready,      //准备
        Playing,    //游戏中
        End,        //结束
        Offline,    //离线
    }

    public static class UIGamerStatusString
    {
        public const string Ready = "已准备";
    }

    public class ChatPosData
    {
        public Vector2 Pos { get; set; }
        public Vector2 Rotate { get; set; }
        public ChatPosData(Vector2 pos, Vector2 rotate)
        {
            this.Pos = pos;
            this.Rotate = rotate;
        }
    }
    public class GamerPosData
    {
        public Vector2 HeadPos { get; set; }
        public Vector2 CardPos { get; set; }
        public ChatPosData ChatPosData { get; set; }
        public GamerPosData(Vector2 hPos, Vector2 cPos, ChatPosData chat)
        {
            this.HeadPos = hPos;
            this.CardPos = cPos;
            this.ChatPosData = chat;
        }
    }
    /// <summary>
    /// 玩家数据
    /// </summary>
    public static class GamerData
    {
        public static Dictionary<int, GamerPosData> Pos = new Dictionary<int, GamerPosData>()
        {
            { 0, new GamerPosData(new Vector2(-300, -270), new Vector2(-290, -160), new ChatPosData(new Vector2( 100,  80), Vector2.zero)) },
            { 1, new GamerPosData(new Vector2( 300, -270), new Vector2( 290, -160), new ChatPosData(new Vector2( 100,  80), Vector2.zero)) },
            { 2, new GamerPosData(new Vector2( 500,    0), new Vector2( 350,    0), new ChatPosData(new Vector2(-100,  80), new Vector2(  0, 180))) },
            { 3, new GamerPosData(new Vector2( 350,  270), new Vector2( 330,  160), new ChatPosData(new Vector2( 100, -80), new Vector2(180,   0))) },
            { 4, new GamerPosData(new Vector2(   0,  270), new Vector2(   0,  160), new ChatPosData(new Vector2( 100, -80), new Vector2(180,   0))) },
            { 5, new GamerPosData(new Vector2(-400,  270), new Vector2(-330,  160), new ChatPosData(new Vector2( 100, -80), new Vector2(180,   0))) },
            { 6, new GamerPosData(new Vector2(-600,    0), new Vector2(-350,    0), new ChatPosData(new Vector2( 100,  80), Vector2.zero)) },
        };
        
    }
    /// <summary>
    /// 游戏规则名称
    /// </summary>
    public static class GameInfo
    {
        public static Dictionary<CowType, string> Rule = new Dictionary<CowType, string>()
        {
            { CowType.FiveFlowerCow, "五花牛" },
            { CowType.BombCow, "炸弹牛" },
            { CowType.FiveSmallCow, "五小牛" },
        };
    }
    public enum CowType : ushort
    {
        None = 0x0,           //无牛
        HaveCow = 0x1,        //有牛
        FiveFlowerCow = 0x2,  //五花牛
        BombCow = 0x4,        //炸弹牛
        FiveSmallCow = 0x8,   //五小牛
    }
    public enum CardFlowerColor : ushort
    {
        Diamond,    //方块
        Plum,       //梅花
        Heart,      //红心
        Spades,     //黑桃
        None,       
    }

}
