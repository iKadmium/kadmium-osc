using Kadmium_Osc.Arguments;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Kadmium_Osc.Test
{
	public class OscArgumentTests
	{
		[Theory]
		[InlineData(3, 4, 4)]
		[InlineData(4, 4, 4)]
		[InlineData(5, 4, 8)]
		public void When_PadIsCalled_Then_TheResultIsCorrect(UInt32 number, UInt32 divisor, UInt32 expected)
		{
			var result = OscArgument.Pad(number, divisor);
			Assert.Equal(expected, result);
		}

		[Fact]
		public void When_ParseIsCalledForAString_Then_TheResultIsCorrect()
		{
			var expected = new OscString("Expected");
			using var owner = MemoryPool<byte>.Shared.Rent();
			var bytes = owner.Memory.Slice(0, (int)expected.Length);
			expected.Write(bytes.Span);

			var actual = OscArgument.Parse(bytes.Span, 's');
			Assert.Equal(expected, actual);
		}

		[Fact]
		public void When_ParseIsCalledForAFloat_Then_TheResultIsCorrect()
		{
			var expected = new OscFloat(42f);
			using var owner = MemoryPool<byte>.Shared.Rent();
			var bytes = owner.Memory.Slice(0, (int)expected.Length);
			expected.Write(bytes.Span);

			var actual = OscArgument.Parse(bytes.Span, 'f');
			Assert.Equal(expected, actual);
		}

		[Fact]
		public void When_ParseIsCalledForAnInt_Then_TheResultIsCorrect()
		{
			var expected = new OscInt(42);
			using var owner = MemoryPool<byte>.Shared.Rent();
			var bytes = owner.Memory.Slice(0, (int)expected.Length);
			expected.Write(bytes.Span);

			var actual = OscArgument.Parse(bytes.Span, 'i');
			Assert.Equal(expected, actual);
		}

		[Fact]
		public void When_ParseIsCalledForABlob_Then_TheResultIsCorrect()
		{
			var expected = new OscBlob(new byte[] { 1, 2, 3, 4 });
			using var owner = MemoryPool<byte>.Shared.Rent();
			var bytes = owner.Memory.Slice(0, (int)expected.Length);
			expected.Write(bytes.Span);

			var actual = OscArgument.Parse(bytes.Span, 'b');
			Assert.Equal(expected, actual);
		}
	}
}
