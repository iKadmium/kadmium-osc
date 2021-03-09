using Kadmium_Osc.Arguments;
using System;
using System.Text;
using Xunit;

namespace Kadmium_Osc.Test
{
	public class OscStringTests
	{
		[Fact]
		public void When_TheConstructorIsUsed_Then_TheValueIsSet()
		{
			string value = "This is my string";
			OscString oscString = new OscString(value);

			Assert.Equal(value, oscString.Value);
		}

		[Theory]
		[InlineData("abc", 4)]
		[InlineData("abcd", 8)]
		public void When_LengthIsCalled_Then_TheValueIsPaddedTo4(string value, int expectedLength)
		{
			OscString oscString = new OscString(value);
			var result = oscString.Length;
			Assert.Equal(expectedLength, result);
		}
	}
}
