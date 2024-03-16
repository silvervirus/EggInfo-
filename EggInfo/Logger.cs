using System;

namespace EggInfo
{
	// Token: 0x02000002 RID: 2
	public static class Logger
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public static void Log(string message)
		{
			Console.WriteLine("[EggInfo] " + message);
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002064 File Offset: 0x00000264
		public static void Log(string format, params object[] args)
		{
			Logger.Log(string.Format(format, args));
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002074 File Offset: 0x00000274
		public static void Error(string message)
		{
			Console.WriteLine("[EggInfo:ERROR] " + message);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002088 File Offset: 0x00000288
		public static void Error(string format, params object[] args)
		{
			Logger.Error(string.Format(format, args));
		}
	}
}
