using Kadmium_Osc.Arguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Kadmium_Osc.Test
{
	public class OscFloatTests
	{
		[Fact]
		public void When_TheConstructorIsUsed_Then_TheValueIsSet()
		{
			float value = 42f;
			OscFloat oscFloat = new OscFloat(value);

			Assert.Equal(value, oscFloat.Value);
		}
	}
}
