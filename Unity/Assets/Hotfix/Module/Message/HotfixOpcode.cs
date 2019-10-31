using ETModel;
namespace ETHotfix
{
	[Message(HotfixOpcode.C2R_Login)]
	public partial class C2R_Login : IRequest {}

	[Message(HotfixOpcode.R2C_Login)]
	public partial class R2C_Login : IResponse {}

	[Message(HotfixOpcode.C2G_LoginGate)]
	public partial class C2G_LoginGate : IRequest {}

	[Message(HotfixOpcode.G2C_LoginGate)]
	public partial class G2C_LoginGate : IResponse {}

	[Message(HotfixOpcode.G2C_TestHotfixMessage)]
	public partial class G2C_TestHotfixMessage : IMessage {}

	[Message(HotfixOpcode.C2M_TestActorRequest)]
	public partial class C2M_TestActorRequest : IActorLocationRequest {}

	[Message(HotfixOpcode.M2C_TestActorResponse)]
	public partial class M2C_TestActorResponse : IActorLocationResponse {}

	[Message(HotfixOpcode.PlayerInfo)]
	public partial class PlayerInfo : IMessage {}

	[Message(HotfixOpcode.C2G_PlayerInfo)]
	public partial class C2G_PlayerInfo : IRequest {}

	[Message(HotfixOpcode.G2C_PlayerInfo)]
	public partial class G2C_PlayerInfo : IResponse {}

//Test注册协议
	[Message(HotfixOpcode.C2G_TestPlayerInfo)]
	public partial class C2G_TestPlayerInfo : IRequest {}

	[Message(HotfixOpcode.G2C_TestPlayerInfo)]
	public partial class G2C_TestPlayerInfo : IResponse {}

//-----------------------------------------以下是牛牛协议-----------------------------------------
//登录
	[Message(HotfixOpcode.C2R_CowCowLogin)]
	public partial class C2R_CowCowLogin : IRequest {}

	[Message(HotfixOpcode.R2C_CowCowLogin)]
	public partial class R2C_CowCowLogin : IResponse {}

//注册
	[Message(HotfixOpcode.C2R_CowCowRegister)]
	public partial class C2R_CowCowRegister : IRequest {}

	[Message(HotfixOpcode.R2C_CowCowRegister)]
	public partial class R2C_CowCowRegister : IResponse {}

//登录消息服务器
	[Message(HotfixOpcode.C2G_CowCowLoginGate)]
	public partial class C2G_CowCowLoginGate : IRequest {}

	[Message(HotfixOpcode.G2C_CowCowLoginGate)]
	public partial class G2C_CowCowLoginGate : IResponse {}

	[Message(HotfixOpcode.C2G_CowCowRefreshGate)]
	public partial class C2G_CowCowRefreshGate : IRequest {}

	[Message(HotfixOpcode.G2C_CowCowRefreshGate)]
	public partial class G2C_CowCowRefreshGate : IResponse {}

	[Message(HotfixOpcode.GamerInfo)]
	public partial class GamerInfo : IMessage {}

	[Message(HotfixOpcode.C2G_CowCowCreateGameRoomGate)]
	public partial class C2G_CowCowCreateGameRoomGate : IRequest {}

	[Message(HotfixOpcode.G2C_CowCowCreateGameRoomGate)]
	public partial class G2C_CowCowCreateGameRoomGate : IResponse {}

	[Message(HotfixOpcode.C2G_CowCowJoinGameRoomGate)]
	public partial class C2G_CowCowJoinGameRoomGate : IRequest {}

	[Message(HotfixOpcode.G2C_CowCowJoinGameRoomGate)]
	public partial class G2C_CowCowJoinGameRoomGate : IResponse {}

	[Message(HotfixOpcode.Actor_CowCowJoinGameRoomGroupSend)]
	public partial class Actor_CowCowJoinGameRoomGroupSend : IActorMessage {}

	[Message(HotfixOpcode.C2M_CowCowGamerReady)]
	public partial class C2M_CowCowGamerReady : IRequest {}

	[Message(HotfixOpcode.M2C_CowCowGamerReady)]
	public partial class M2C_CowCowGamerReady : IResponse {}

	[Message(HotfixOpcode.Actor_CowCowGamerReady)]
	public partial class Actor_CowCowGamerReady : IActorMessage {}

	[Message(HotfixOpcode.Actor_CowCowRoomDealCards)]
	public partial class Actor_CowCowRoomDealCards : IActorMessage {}

	[Message(HotfixOpcode.C2M_CowCowGrabBanker)]
	public partial class C2M_CowCowGrabBanker : IRequest {}

	[Message(HotfixOpcode.M2C_CowCowGrabBanker)]
	public partial class M2C_CowCowGrabBanker : IResponse {}

	[Message(HotfixOpcode.C2M_CowCowGamerSubmitCardType)]
	public partial class C2M_CowCowGamerSubmitCardType : IRequest {}

	[Message(HotfixOpcode.M2C_CowCowGamerSubmitCardType)]
	public partial class M2C_CowCowGamerSubmitCardType : IResponse {}

	[Message(HotfixOpcode.Actor_CowCowGamerSubmitCardType)]
	public partial class Actor_CowCowGamerSubmitCardType : IActorMessage {}

	[Message(HotfixOpcode.CowCowSmallSettlementInfo)]
	public partial class CowCowSmallSettlementInfo : IMessage {}

	[Message(HotfixOpcode.Actor_CowCowRoomOpenCardsAndSettlement)]
	public partial class Actor_CowCowRoomOpenCardsAndSettlement : IActorMessage {}

	[Message(HotfixOpcode.C2G_CowCowPing)]
	public partial class C2G_CowCowPing : IRequest {}

	[Message(HotfixOpcode.G2C_CowCowPing)]
	public partial class G2C_CowCowPing : IResponse {}

	[Message(HotfixOpcode.C2G_CowCowChatFont)]
	public partial class C2G_CowCowChatFont : IRequest {}

	[Message(HotfixOpcode.G2C_CowCowChatFont)]
	public partial class G2C_CowCowChatFont : IResponse {}

	[Message(HotfixOpcode.Actor_CowCowChatFont)]
	public partial class Actor_CowCowChatFont : IActorMessage {}

	[Message(HotfixOpcode.C2G_CowCowEmoji)]
	public partial class C2G_CowCowEmoji : IRequest {}

	[Message(HotfixOpcode.G2C_CowCowEmoji)]
	public partial class G2C_CowCowEmoji : IResponse {}

	[Message(HotfixOpcode.Actor_CowCowEmoji)]
	public partial class Actor_CowCowEmoji : IActorMessage {}

	[Message(HotfixOpcode.DissolutionInfo)]
	public partial class DissolutionInfo : IMessage {}

	[Message(HotfixOpcode.C2G_CowCowDissoltion)]
	public partial class C2G_CowCowDissoltion : IRequest {}

	[Message(HotfixOpcode.G2C_CowCowDissoltion)]
	public partial class G2C_CowCowDissoltion : IResponse {}

	[Message(HotfixOpcode.Actor_CowCowDissoltion)]
	public partial class Actor_CowCowDissoltion : IActorMessage {}

	[Message(HotfixOpcode.CowCowBigSettlementInfo)]
	public partial class CowCowBigSettlementInfo : IMessage {}

	[Message(HotfixOpcode.Actor_CowCowBigSettlement)]
	public partial class Actor_CowCowBigSettlement : IActorMessage {}

}
namespace ETHotfix
{
	public static partial class HotfixOpcode
	{
		 public const ushort C2R_Login = 10001;
		 public const ushort R2C_Login = 10002;
		 public const ushort C2G_LoginGate = 10003;
		 public const ushort G2C_LoginGate = 10004;
		 public const ushort G2C_TestHotfixMessage = 10005;
		 public const ushort C2M_TestActorRequest = 10006;
		 public const ushort M2C_TestActorResponse = 10007;
		 public const ushort PlayerInfo = 10008;
		 public const ushort C2G_PlayerInfo = 10009;
		 public const ushort G2C_PlayerInfo = 10010;
		 public const ushort C2G_TestPlayerInfo = 10011;
		 public const ushort G2C_TestPlayerInfo = 10012;
		 public const ushort C2R_CowCowLogin = 10013;
		 public const ushort R2C_CowCowLogin = 10014;
		 public const ushort C2R_CowCowRegister = 10015;
		 public const ushort R2C_CowCowRegister = 10016;
		 public const ushort C2G_CowCowLoginGate = 10017;
		 public const ushort G2C_CowCowLoginGate = 10018;
		 public const ushort C2G_CowCowRefreshGate = 10019;
		 public const ushort G2C_CowCowRefreshGate = 10020;
		 public const ushort GamerInfo = 10021;
		 public const ushort C2G_CowCowCreateGameRoomGate = 10022;
		 public const ushort G2C_CowCowCreateGameRoomGate = 10023;
		 public const ushort C2G_CowCowJoinGameRoomGate = 10024;
		 public const ushort G2C_CowCowJoinGameRoomGate = 10025;
		 public const ushort Actor_CowCowJoinGameRoomGroupSend = 10026;
		 public const ushort C2M_CowCowGamerReady = 10027;
		 public const ushort M2C_CowCowGamerReady = 10028;
		 public const ushort Actor_CowCowGamerReady = 10029;
		 public const ushort Actor_CowCowRoomDealCards = 10030;
		 public const ushort C2M_CowCowGrabBanker = 10031;
		 public const ushort M2C_CowCowGrabBanker = 10032;
		 public const ushort C2M_CowCowGamerSubmitCardType = 10033;
		 public const ushort M2C_CowCowGamerSubmitCardType = 10034;
		 public const ushort Actor_CowCowGamerSubmitCardType = 10035;
		 public const ushort CowCowSmallSettlementInfo = 10036;
		 public const ushort Actor_CowCowRoomOpenCardsAndSettlement = 10037;
		 public const ushort C2G_CowCowPing = 10038;
		 public const ushort G2C_CowCowPing = 10039;
		 public const ushort C2G_CowCowChatFont = 10040;
		 public const ushort G2C_CowCowChatFont = 10041;
		 public const ushort Actor_CowCowChatFont = 10042;
		 public const ushort C2G_CowCowEmoji = 10043;
		 public const ushort G2C_CowCowEmoji = 10044;
		 public const ushort Actor_CowCowEmoji = 10045;
		 public const ushort DissolutionInfo = 10046;
		 public const ushort C2G_CowCowDissoltion = 10047;
		 public const ushort G2C_CowCowDissoltion = 10048;
		 public const ushort Actor_CowCowDissoltion = 10049;
		 public const ushort CowCowBigSettlementInfo = 10050;
		 public const ushort Actor_CowCowBigSettlement = 10051;
	}
}
