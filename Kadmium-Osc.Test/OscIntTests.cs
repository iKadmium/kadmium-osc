using Kadmium_Osc.Arguments;
using System;
using System.Buffers;
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

		[Fact]
		public void When_GetInt32IsCalled_Then_TheDataIsCorrect()
		{
			var rawData = new byte[4] { 0, 0, 0, 42 };
			var expected = 42;

			var actual = OscInt.Parse(rawData);
			Assert.Equal(expected, actual.Value);
		}

		[Fact]
		public void When_WriteIsCalledOnAnInt32_Then_TheDataIsWrittenCorrectly()
		{
			var value = 42;
			var expected = new byte[4] { 0, 0, 0, 42 };

			OscInt oscInt = new OscInt(value);

			using (var owner = MemoryPool<byte>.Shared.Rent())
			{
				var bytes = owner.Memory.Slice(0, 4);
				oscInt.Write(bytes.Span);
				Assert.Equal(expected, bytes.ToArray());
			}
		}
	}
}
