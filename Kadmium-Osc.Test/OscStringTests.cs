using Kadmium_Osc.Arguments;
using System;
using System.Buffers;
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
		public void When_LengthIsCalled_Then_TheValueIsPaddedTo4(string value, UInt32 expectedLength)
		{
			OscString oscString = new OscString(value);
			var result = oscString.Length;
			Assert.Equal(expectedLength, result);
		}

		[Fact]
		public void When_AnAsciiStringIsDecoded_Then_TheOutputIsCorrect()
		{
			var expected = "This is the string";
			var bytes = Encoding.ASCII.GetBytes(expected + "\0");

			var result = OscString.Parse(bytes);
			Assert.Equal(expected, result.Value);
		}

		[Fact]
		public void When_AnAsciiStringWithPaddingIsDecoded_Then_TheOutputDoesNotIncludeThePadding()
		{
			var expected = "This is the string";
			var bytes = Encoding.ASCII.GetBytes(expected + "\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0");

			var result = OscString.Parse(bytes);
			Assert.Equal(expected, result.Value);
		}

		[Fact]
		public void When_AStringIsEncoded_Then_TheOutputIsPaddedTo4Bytes()
		{
			string str = "abcde";
			byte[] expected = new byte[8] { (byte)'a', (byte)'b', (byte)'c', (byte)'d', (byte)'e', 0, 0, 0 };

			OscString oscString = new OscString(str);

			using var memoryOwner = MemoryPool<byte>.Shared.Rent();
			var actual = memoryOwner.Memory.Slice(0, 8);
			oscString.Write(actual.Span);
			Assert.Equal(expected, actual.ToArray());
		}
	}
}
