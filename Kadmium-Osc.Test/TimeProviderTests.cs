using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Kadmium_Osc.Test
{
	public class TimeProviderTests
	{
		[Fact]
		public void When_NowIsCalled_Then_ItShouldReturnTheCurrentTime()
		{
			TimeProvider provider = new TimeProvider();
			Assert.Equal(DateTime.Now, provider.Now, new TimeSpan(0, 0, 0, 0, 50));
		}

		[Fact]
		public async Task When_WaitUntilIsCalled_Then_ItShouldWaitUntilTheCurrentTime()
		{
			TimeProvider provider = new TimeProvider();
			DateTime timeBefore = DateTime.Now;
			DateTime expectedTime = timeBefore.AddMilliseconds(500);
			await provider.WaitUntil(expectedTime);
			DateTime timeAfter = DateTime.Now;
			Assert.Equal(expectedTime, timeAfter, new TimeSpan(0, 0, 0, 0, 50));
		}
	}
}
