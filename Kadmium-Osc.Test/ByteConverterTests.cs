using Kadmium_Osc.ByteConversion;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Kadmium_Osc.Test
{
	public class ByteConverterTests
	{
		private class TestByteConverter : ByteConverter
		{
			public override int GetInt32(ReadOnlyMemory<byte> bytes)
			{
				throw new NotImplementedException();
			}

			public override float GetSingle(ReadOnlyMemory<byte> bytes)
			{
				throw new NotImplementedException();
			}

			public override uint GetUInt32(ReadOnlyMemory<byte> bytes)
			{
				throw new NotImplementedException();
			}

			public override void Write(Memory<byte> bytes, uint value)
			{
				throw new NotImplementedException();
			}

			public override void Write(Memory<byte> bytes, float value)
			{
				throw new NotImplementedException();
			}

			public override void Write(Memory<byte> bytes, int value)
			{
				throw new NotImplementedException();
			}
		}

		[Theory]
		[InlineData(3, 4, 4)]
		[InlineData(4, 4, 4)]
		[InlineData(5, 4, 8)]
		public void When_PadIsCalled_Then_TheResultIsCorrect(int number, int divisor, int expected)
		{
			var result = ByteConverter.Pad(number, divisor);
			Assert.Equal(expected, result);
		}

		[Fact]
		public void When_AnAsciiStringIsDecoded_Then_TheOutputIsCorrect()
		{
			var expected = "This is the string";
			var bytes = Encoding.ASCII.GetBytes(expected + "\0");

			ByteConverter converter = new TestByteConverter();
			var result = converter.GetAsciiString(bytes);
			Assert.Equal(expected, result);
		}

		[Fact]
		public void When_AnAsciiStringWithPaddingIsDecoded_Then_TheOutputDoesNotIncludeThePaddingIsCorrect()
		{
			var expected = "This is the string";
			var bytes = Encoding.ASCII.GetBytes(expected + "\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0");

			ByteConverter converter = new TestByteConverter();
			var result = converter.GetAsciiString(bytes);
			Assert.Equal(expected, result);
		}

		[Fact]
		public void When_AStringIsEncoded_Then_TheOutputIsPaddedTo4Bytes()
		{
			string str = "abcde";
			byte[] expected = new byte[8] { (byte)'a', (byte)'b', (byte)'c', (byte)'d', (byte)'e', 0, 0, 0 };
			ByteConverter converter = new TestByteConverter();
			using (var memoryOwner = MemoryPool<byte>.Shared.Rent())
			{
				converter.Write(memoryOwner.Memory, str);
				var actual = memoryOwner.Memory.Slice(0, 8);
				Assert .Equal(expected, actual.ToArray());
			}
		}
	}
}
