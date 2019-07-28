namespace ETModel
{
	public static class IdGenerater
	{
		private static long instanceIdGenerator;
		
		private static long appId;
		
		public static long AppId
		{
			set
			{
				appId = value;
				instanceIdGenerator = appId << 48; //返回appId*2^48次方
            }
		}

		private static ushort value;

		public static long GenerateId()
		{
			long time = TimeHelper.ClientNowSeconds();

			return (appId << 48) + (time << 16) + ++value;
		}
		
		public static long GenerateInstanceId()
		{
			return ++instanceIdGenerator;
		}
        /// <summary>
        /// 移位运算 2^48次方的时候返回0，2^49次方的时候返回1，以此类推
        /// </summary>
		public static int GetAppId(long v)
		{
			return (int)(v >> 48);
		}
	}
}