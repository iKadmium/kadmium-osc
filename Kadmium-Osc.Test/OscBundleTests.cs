using Kadmium_Osc.Arguments;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Kadmium_Osc.Test
{
	public class OscBundleTests
	{
		[Fact]
		public void Given_TheBundleHasASingleMessage_When_ParseIsCalled_Then_TheResultIsParsed()
		{
			var bundleString = new OscString("#bundle");
			var timeTag = new OscTimeTag(OscTimeTag.MinValue);
			OscMessage payload = new OscMessage("/test");

			OscBundle expected = new OscBundle(timeTag, payload);

			using var owner = MemoryPool<byte>.Shared.Rent((int)expected.Length);
			var memory = owner.Memory.Slice(0, (int)expected.Length);
			var bytes = memory;

			bundleString.Write(bytes.Span);
			bytes = bytes[(int)bundleString.Length..];

			timeTag.Write(bytes.Span);
			bytes = bytes[(int)timeTag.Length..];

			var payloadLength = new OscInt((int)payload.Length);
			payloadLength.Write(bytes.Span);
			bytes = bytes[(int)payloadLength.Length..];

			payload.Write(bytes.Span);

			OscBundle actual = OscBundle.Parse(memory.Span);

			Assert.Equal(expected, actual);
		}

		[Fact]
		public void Given_TheMessageHasArguments_When_ArgumentsAreChecked_Then_TheValuesAreCorrect()
		{
			int first = 5;
			float second = 10f;
			string third = "My string";
			byte[] fourth = new byte[] { 1, 2, 3, 4 };

			OscMessage message = new OscMessage();
			message.Arguments.Add(first);
			message.Arguments.Add(second);
			message.Arguments.Add(third);
			message.Arguments.Add(fourth);

			Assert.Equal(first, message.GetArgument<OscInt>(0).Value);
			Assert.Equal(second, message.GetArgument<OscFloat>(1).Value);
			Assert.Equal(third, message.GetArgument<OscString>(2).Value);
			Assert.Equal(fourth, message.GetArgument<OscBlob>(3).Value);
		}

		[Fact]
		public void Given_TheBundleHasASingleMessage_When_WriteIsCalled_Then_TheResultIsCorrect()
		{
			var expected = new byte[]
			{
				//bundle tag
				(byte)'#', (byte)'b', (byte)'u', (byte)'n', (byte)'d', (byte)'l', (byte)'e', 0,
				//time tag
				0, 0, 0, 0, 0, 0, 0, 0,
				//length
				0, 0, 0, 12,
				// address
				(byte)'/', (byte)'t', (byte)'e', (byte)'s', (byte)'t', 0, 0, 0,
				// type tag
				(byte)',', 0, 0, 0
			};
			var timeTag = new OscTimeTag(OscTimeTag.MinValue);
			OscMessage payload = new OscMessage("/test");

			OscBundle bundle = new OscBundle(timeTag, payload);
			
			using var actualOwner = MemoryPool<byte>.Shared.Rent((int)bundle.Length);
			var actualMemory = actualOwner.Memory.Slice(0, (int)bundle.Length);
			bundle.Write(actualMemory.Span);

			Assert.Equal(expected, actualMemory.ToArray());
		}

		[Fact]
		public void Given_TheTwoAreEqual_When_EqualsIsCalled_Then_ItReturnsTrue()
		{
			OscMessage message = new OscMessage("/test", "Hello World");
			OscTimeTag timeTag = OscTimeTag.MinValue;

			OscBundle first = new OscBundle(timeTag, message);
			OscBundle second = new OscBundle(timeTag, message);

			Assert.Equal(first, second);
		}

		[Fact]
		public void Given_DifferentTimeTags_When_EqualsIsCalled_Then_ItReturnsFalse()
		{
			OscMessage message = new OscMessage("/test", "Hello World");
			OscTimeTag firstTimeTag = OscTimeTag.MinValue;
			OscTimeTag secondTimeTag = OscTimeTag.MinValue.AddDays(1);

			OscBundle first = new OscBundle(firstTimeTag, message);
			OscBundle second = new OscBundle(secondTimeTag, message);

			Assert.NotEqual(first, second);
		}

		[Fact]
		public void Given_DifferentMessages_When_EqualsIsCalled_Then_ItReturnsFalse()
		{
			OscMessage firstMessage = new OscMessage("/test", "Hello World");
			OscMessage secondMessage = new OscMessage("/test", "Goodbye World");
			OscTimeTag timeTag = OscTimeTag.MinValue;

			OscBundle first = new OscBundle(timeTag, firstMessage);
			OscBundle second = new OscBundle(timeTag, secondMessage);

			Assert.NotEqual(first, second);
		}
	}
}
