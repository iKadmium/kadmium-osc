using Kadmium_Osc.Arguments;
using System;
using System.Buffers;
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

		[Fact]
		public void When_GetSingleIsCalled_Then_TheDataIsParsedProperly()
		{
			var expected = 42f;
			var bytes = new byte[4] { 0x42, 0x28, 0x00, 0x00 };

			Assert.Equal(expected, OscFloat.Parse(bytes).Value);
		}

		[Fact]
		public void When_WriteIsCalledForASingle_Then_TheDataIsCorrect()
		{
			var value = 42f;
			var expected = new byte[4] { 0x42, 0x28, 0x00, 0x00 };

			var oscFloat = new OscFloat(value);

			using var owner = MemoryPool<byte>.Shared.Rent();
			var bytes = owner.Memory.Slice(0, 4);
			oscFloat.Write(bytes.Span);
			Assert.Equal(expected, bytes.ToArray());
		}
	}
}
