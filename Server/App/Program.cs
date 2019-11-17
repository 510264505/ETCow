using System;
using System.Threading;
using ETModel;
using NLog;

namespace App
{
	internal static class Program
	{
		private static void Main(string[] args)
		{
			// 异步方法全部会回掉到主线程
			SynchronizationContext.SetSynchronizationContext(OneThreadSynchronizationContext.Instance);
            try
			{
                Game.EventSystem.Add(DLLType.Model, typeof(Game).Assembly);
				Game.EventSystem.Add(DLLType.Hotfix, DllHelper.GetHotfixAssembly());

				Options options = Game.Scene.AddComponent<OptionComponent, string[]>(args).Options;
				StartConfig startConfig = Game.Scene.AddComponent<StartConfigComponent, string, int>(options.Config, options.AppId).StartConfig;

				if (!options.AppType.Is(startConfig.AppType))
				{
					Log.Error("命令行参数apptype与配置不一致");
					return;
				}

                Log.WriteLine($"启动服务器类型:{startConfig.AppType},ip:{startConfig.ServerIP}");
                Log.Debug("启动服务器类型:" + startConfig.AppType);

				IdGenerater.AppId = options.AppId;

				LogManager.Configuration.Variables["appType"] = $"{startConfig.AppType}";
				LogManager.Configuration.Variables["appId"] = $"{startConfig.AppId}";
				LogManager.Configuration.Variables["appTypeFormat"] = $"{startConfig.AppType, -8}";
				LogManager.Configuration.Variables["appIdFormat"] = $"{startConfig.AppId:0000}";

				Log.Info($"server start........................ {startConfig.AppId} {startConfig.AppType}");

				Game.Scene.AddComponent<TimerComponent>();
				Game.Scene.AddComponent<OpcodeTypeComponent>();
				Game.Scene.AddComponent<MessageDispatcherComponent>();

				// 根据不同的AppType添加不同的组件
				OuterConfig outerConfig = startConfig.GetComponent<OuterConfig>();
				InnerConfig innerConfig = startConfig.GetComponent<InnerConfig>();
				ClientConfig clientConfig = startConfig.GetComponent<ClientConfig>();
				
				switch (startConfig.AppType)
				{
					case AppType.Manager:
						Game.Scene.AddComponent<AppManagerComponent>();
						Game.Scene.AddComponent<NetInnerComponent, string>(innerConfig.Address);
						Game.Scene.AddComponent<NetOuterComponent, string>(outerConfig.Address);
						break;
					case AppType.Realm: //玩家的初次连接对象，实际上可能存在多个，但是对于单个玩家来说他只会遇到其中一个。
                        // 有可能被攻击，需要改多个的话，改RealmGateAddressComponent组件
                        Game.Scene.AddComponent<MailboxDispatcherComponent>();
						Game.Scene.AddComponent<ActorMessageDispatcherComponent>();
						Game.Scene.AddComponent<NetInnerComponent, string>(innerConfig.Address);
						Game.Scene.AddComponent<NetOuterComponent, string>(outerConfig.Address);
						Game.Scene.AddComponent<LocationProxyComponent>();
						Game.Scene.AddComponent<RealmGateAddressComponent>();
						break;
					case AppType.Gate: //消息转发服务器，直接处理玩家的请求和转发Actor消息。
                        Game.Scene.AddComponent<PlayerComponent>();
						Game.Scene.AddComponent<MailboxDispatcherComponent>();
						Game.Scene.AddComponent<ActorMessageDispatcherComponent>();
						Game.Scene.AddComponent<NetInnerComponent, string>(innerConfig.Address);
						Game.Scene.AddComponent<NetOuterComponent, string>(outerConfig.Address);
						Game.Scene.AddComponent<LocationProxyComponent>();
						Game.Scene.AddComponent<ActorMessageSenderComponent>();
						Game.Scene.AddComponent<ActorLocationSenderComponent>();
						Game.Scene.AddComponent<GateSessionKeyComponent>();
                        Game.Scene.AddComponent<PingComponent>();
                        break;
					case AppType.Location: //连接内网，匹配服务器，在玩家更换游戏地图时，通知Gate更新ActorID。
                        Game.Scene.AddComponent<NetInnerComponent, string>(innerConfig.Address);
						Game.Scene.AddComponent<LocationComponent>();
						break;
					case AppType.Map: //游戏逻辑服务器，内部运行整体游戏逻辑。
                        Game.Scene.AddComponent<NetInnerComponent, string>(innerConfig.Address);
						Game.Scene.AddComponent<UnitComponent>();
						Game.Scene.AddComponent<LocationProxyComponent>();
						Game.Scene.AddComponent<ActorMessageSenderComponent>();
						Game.Scene.AddComponent<ActorLocationSenderComponent>();
						Game.Scene.AddComponent<MailboxDispatcherComponent>();
						Game.Scene.AddComponent<ActorMessageDispatcherComponent>();
						Game.Scene.AddComponent<PathfindingComponent>();
                        Game.Scene.AddComponent<PingComponent>();
						break;
					case AppType.AllServer:
						// 发送普通actor消息
						Game.Scene.AddComponent<ActorMessageSenderComponent>();
						
						// 发送location actor消息
						Game.Scene.AddComponent<ActorLocationSenderComponent>();

                        // 数据库管理组件（管理数据库连接地址，数据库名称等）
                        Game.Scene.AddComponent<DBComponent>();
                        // 数据库调用组件（调用DB数据库的组件，添加、查询、修改、删除等操作）
                        Game.Scene.AddComponent<DBProxyComponent>();

                        // location server需要的组件
                        Game.Scene.AddComponent<LocationComponent>();
						
						// 访问location server的组件
						Game.Scene.AddComponent<LocationProxyComponent>();
						
						// 这两个组件是处理actor消息使用的
						Game.Scene.AddComponent<MailboxDispatcherComponent>();
						Game.Scene.AddComponent<ActorMessageDispatcherComponent>();
						
						// 内网消息组件
						Game.Scene.AddComponent<NetInnerComponent, string>(innerConfig.Address);
						
						// 外网消息组件
						Game.Scene.AddComponent<NetOuterComponent, string>(outerConfig.Address);
						
						// manager server组件，用来管理其它进程使用
						Game.Scene.AddComponent<AppManagerComponent>();
						Game.Scene.AddComponent<RealmGateAddressComponent>();
						Game.Scene.AddComponent<GateSessionKeyComponent>();
						
						// 配置管理
						Game.Scene.AddComponent<ConfigComponent>();
						
						// recast寻路组件
						Game.Scene.AddComponent<PathfindingComponent>();
                        Game.Scene.AddComponent<PingComponent>();

                        Game.Scene.AddComponent<PlayerComponent>();
						Game.Scene.AddComponent<UnitComponent>();

						Game.Scene.AddComponent<ConsoleComponent>();
                        // Game.Scene.AddComponent<HttpComponent>();

                        //以下是牛牛服务端自定义全局组件
                        Game.Scene.AddComponent<UserInfoComponent>();
                        Game.Scene.AddComponent<CowCowGateSessionKeyComponent>();
                        Game.Scene.AddComponent<RoomComponent>();

                        Game.Scene.AddComponent<OnlineComponent>();
                        break;
					case AppType.Benchmark:
						Game.Scene.AddComponent<NetOuterComponent>();
						Game.Scene.AddComponent<BenchmarkComponent, string>(clientConfig.Address);
						break;
					case AppType.BenchmarkWebsocketServer:
						Game.Scene.AddComponent<NetOuterComponent, string>(outerConfig.Address);
						break;
					case AppType.BenchmarkWebsocketClient:
						Game.Scene.AddComponent<NetOuterComponent>();
						Game.Scene.AddComponent<WebSocketBenchmarkComponent, string>(clientConfig.Address);
						break;
					default:
						throw new Exception($"命令行参数没有设置正确的AppType: {startConfig.AppType}");
				}
				
				while (true)
				{
					try
					{
						Thread.Sleep(1);
						OneThreadSynchronizationContext.Instance.Update();
						Game.EventSystem.Update();
					}
					catch (Exception e)
					{
						Log.Error(e);
					}
				}
			}
			catch (Exception e)
			{
				Log.Error(e);
			}
		}
	}
}
