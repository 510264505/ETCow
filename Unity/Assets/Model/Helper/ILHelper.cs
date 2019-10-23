using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using ILRuntime.CLR.Method;
using ILRuntime.CLR.TypeSystem;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Generated;
using ILRuntime.Runtime.Intepreter;
using UnityEngine;

namespace ETModel
{
	public static class ILHelper
	{
		public static void InitILRuntime(ILRuntime.Runtime.Enviorment.AppDomain appdomain)
		{
			// 注册重定向函数

			// 注册委托
			appdomain.DelegateManager.RegisterMethodDelegate<List<object>>();
			appdomain.DelegateManager.RegisterMethodDelegate<AChannel, System.Net.Sockets.SocketError>();
			appdomain.DelegateManager.RegisterMethodDelegate<byte[], int, int>();
			appdomain.DelegateManager.RegisterMethodDelegate<IResponse>();
			appdomain.DelegateManager.RegisterMethodDelegate<Session, object>();
			appdomain.DelegateManager.RegisterMethodDelegate<Session, ushort, MemoryStream>();
			appdomain.DelegateManager.RegisterMethodDelegate<Session>();
			appdomain.DelegateManager.RegisterMethodDelegate<ILTypeInstance>();
			appdomain.DelegateManager.RegisterFunctionDelegate<Google.Protobuf.Adapt_IMessage.Adaptor>();
			appdomain.DelegateManager.RegisterMethodDelegate<Google.Protobuf.Adapt_IMessage.Adaptor>();
            appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.EventSystems.BaseEventData>();
            appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction<UnityEngine.EventSystems.BaseEventData>>((act) =>
            {
                return new UnityEngine.Events.UnityAction<UnityEngine.EventSystems.BaseEventData>((arg0) =>
                {
                    ((Action<UnityEngine.EventSystems.BaseEventData>)act)(arg0);
                });
            });
            appdomain.DelegateManager.RegisterMethodDelegate<float>();            appdomain.DelegateManager.RegisterDelegateConvertor<DG.Tweening.TweenCallback>((act) =>
            {
                return new DG.Tweening.TweenCallback(() =>
                {
                    ((Action)act)();
                });
            });


            CLRBindings.Initialize(appdomain);

			// 注册适配器
			Assembly assembly = typeof(Init).Assembly;
			foreach (Type type in assembly.GetTypes())
			{
				object[] attrs = type.GetCustomAttributes(typeof(ILAdapterAttribute), false);
				if (attrs.Length == 0)
				{
					continue;
				}
				object obj = Activator.CreateInstance(type);
				CrossBindingAdaptor adaptor = obj as CrossBindingAdaptor;
				if (adaptor == null)
				{
					continue;
				}
				appdomain.RegisterCrossBindingAdaptor(adaptor);
			}

			LitJson.JsonMapper.RegisterILRuntimeCLRRedirection(appdomain);
		}
	}
}