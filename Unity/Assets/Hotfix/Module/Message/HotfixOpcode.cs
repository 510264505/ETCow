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
		 public const ushort GamerInfo = 10019;
		 public const ushort C2G_CowCowCreateGameRoomGate = 10020;
		 public const ushort G2C_CowCowCreateGameRoomGate = 10021;
		 public const ushort C2G_CowCowJoinGameRoomGate = 10022;
		 public const ushort G2C_CowCowJoinGameRoomGate = 10023;
		 public const ushort Actor_CowCowJoinGameRoomGroupSend = 10024;
	}
}
