using Kadmium_Osc.Arguments;
using System;
using System.Buffers;
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

		[Fact]
		public void When_GetTimeTagIsCalledWithAFractionalTime_Then_TheValueIsParsedCorrectly()
		{
			var expected = OscTimeTag.MinValue.AddTicks(5000000);
			var bytes = new byte[8] { 0, 0, 0, 0, 0x80, 0, 0, 0 };

			var timeTag = OscTimeTag.Parse(bytes);
			Assert.Equal(expected, timeTag.Value);
		}

		[Fact]
		public void When_GetTimeTagIsCalledWithJustSecondData_Then_TheTagIsCreatedProperly()
		{
			var expected = OscTimeTag.MinValue.AddSeconds(1);
			var bytes = new byte[8] { 0, 0, 0, 1, 0, 0, 0, 0 };

			var timeTag = OscTimeTag.Parse(bytes);
			Assert.Equal(expected, timeTag.Value);
		}

		[Fact]
		public void When_GetTimeTagIsCalledWithTheMaxValue_Then_TheValueIsParsedCorrectly()
		{
			var expected = OscTimeTag.MaxValue;
			var bytes = new byte[8] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };

			var timeTag = OscTimeTag.Parse(bytes);
			Assert.Equal(expected, timeTag.Value);
		}

		[Fact]
		public void When_WriteIsCalledForATimeTag_Then_TheDataIsCorrect()
		{
			var value = new DateTime(1900, 1, 1, 0, 0, 1, 0);
			var expected = new byte[8] { 0, 0, 0, 1, 0, 0, 0, 0 };

			OscTimeTag oscTimeTag = new OscTimeTag(value);

			using var owner = MemoryPool<byte>.Shared.Rent();
			var bytes = owner.Memory.Slice(0, 8);
			oscTimeTag.Write(bytes.Span);
			Assert.Equal(expected, bytes.ToArray());
		}
	}
}
