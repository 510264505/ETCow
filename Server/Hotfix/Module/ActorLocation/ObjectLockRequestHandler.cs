﻿using System;
using ETModel;

namespace ETHotfix
{
	[MessageHandler(AppType.Location)]
	public class ObjectLockRequestHandler : AMRpcHandler<ObjectLockRequest, ObjectLockResponse>
	{
		protected override void RunAsync(Session session, ObjectLockRequest message, Action<ObjectLockResponse> reply)
		{
			ObjectLockResponse response = new ObjectLockResponse();
			try
			{
				Game.Scene.GetComponent<LocationComponent>().Lock(message.Key, message.InstanceId, message.Time).Coroutine();
				reply(response);
			}
			catch (Exception e)
			{
				ReplyError(response, e, reply);
			}
		}
	}
}