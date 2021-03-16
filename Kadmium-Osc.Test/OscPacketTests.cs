using Kadmium_Osc.Arguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Kadmium_Osc.Test
{
	public class OscPacketTests
	{
		[Fact]
		public void Given_TheInputsAreAMessageAndABundle_When_EqualsIsCalled_Then_ItReturnsFalse()
		{
			OscMessage first = new OscMessage("/test", "Hello World");
			OscBundle second = new OscBundle(OscTimeTag.MinValue, first);

			Assert.NotEqual(first as OscPacket, second as OscPacket);
		}

		[Fact]
		public void Given_TheInputsAreBothTheSameMessage_When_EqualsIsCalled_Then_ItReturnsTrue()
		{
			OscMessage first = new OscMessage("/test", "Hello World");
			OscMessage second = new OscMessage("/test", "Hello World");

			Assert.Equal(first as OscPacket, second as OscPacket);
		}

		[Fact]
		public void Given_TheInputsAreBothTheSameBundle_When_EqualsIsCalled_Then_ItReturnsTrue()
		{
			OscMessage message = new OscMessage("/test", "Hello World");

			OscBundle first = new OscBundle(OscTimeTag.MinValue, message);
			OscBundle second = new OscBundle(OscTimeTag.MinValue, message);

			Assert.Equal(first as OscPacket, second as OscPacket);
		}
	}
}
