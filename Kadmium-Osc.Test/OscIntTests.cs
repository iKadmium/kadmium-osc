using Kadmium_Osc.Arguments;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Xunit;

namespace Kadmium_Osc.Test
{
	public class OscIntTests
	{
		[Fact]
		public void When_TheConstructorIsUsed_Then_TheValueIsSet()
		{
			int value = 42;
			OscInt oscInt = new OscInt(value);

			Assert.Equal(value, oscInt.Value);
		}
	}
}
