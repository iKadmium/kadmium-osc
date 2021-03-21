using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kadmium_Osc
{
	public class TimeProvider : ITimeProvider
	{
		public DateTime Now => DateTime.Now;

		public async Task WaitUntil(DateTime time)
		{
			var timeToWait = time - Now;
			if (timeToWait > TimeSpan.MinValue)
			{
				await Task.Delay(timeToWait);
			}
		}
	}
}
