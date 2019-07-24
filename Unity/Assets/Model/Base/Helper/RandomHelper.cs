using System;

namespace ETModel
{
	public static class RandomHelper
	{
		private static readonly Random random = new Random();

		public static UInt64 RandUInt64()
		{
			var bytes = new byte[8];
			random.NextBytes(bytes);
			return BitConverter.ToUInt64(bytes, 0);
		}

		public static Int64 RandInt64()
		{
			var bytes = new byte[8];
			random.NextBytes(bytes);
			return BitConverter.ToInt64(bytes, 0);
		}

		/// <summary>
		/// 获取lower与Upper之间的随机数
		/// </summary>
		/// <param name="lower">闭区间包含</param>
		/// <param name="upper">开区间不包含</param>
		/// <returns></returns>
		public static int RandomNumber(int lower, int upper)
		{
			int value = random.Next(lower, upper);
			return value;
		}

        /// <summary>
        /// 获取0到max且不包含max的整数
        /// </summary>
        public static int RandomNumber(int max)
        {
            return random.Next(max);
        }
	}
}