using Kadmium_Osc.Arguments;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Kadmium_Osc.Test
{
	public class OscTimeTagTests
	{
		[Fact]
		public void When_TheConstructorIsUsed_Then_TheValueIsSet()
		{
			DateTime value = DateTime.Now;
			OscTimeTag oscTime = new OscTimeTag(value);

			Assert.Equal(value, oscTime.Value);
		}
	}
}
