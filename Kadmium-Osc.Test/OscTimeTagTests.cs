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

		[Fact]
		public void Given_TheValueIsGreaterThanTheMaximum_When_TheConstructorIsUsed_Then_AnExceptionIsThrown()
		{
			DateTime value = OscTimeTag.MaxValue.AddTicks(1);
			Assert.Throws<ArgumentException>(() => new OscTimeTag(value));
		}

		[Fact]
		public void Given_TheValueIsLessThanTheMinimum_When_TheConstructorIsUsed_Then_AnExceptionIsThrown()
		{
			DateTime value = new DateTime(1900, 1, 1).AddTicks(-1);
			Assert.Throws<ArgumentException>(() => new OscTimeTag(value));
		}
	}
}
