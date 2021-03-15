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
		public void Given_TheBundleHasASingleMessage_When_ParseIsCalled_Then_TheResultIsParses()
		{
			var bundleString = new OscString("#bundle");
			var timeTag = new OscTimeTag(OscTimeTag.MinValue);
			OscMessage payload = new OscMessage("/test");

			OscBundle expected = new OscBundle();
			expected.TimeTag = timeTag;
			expected.Contents.Add(payload);

			using var owner = MemoryPool<byte>.Shared.Rent((int)expected.Length);
			var memory = owner.Memory.Slice(0, (int)expected.Length);
			var bytes = memory;

			bundleString.Write(bytes.Span);
			bytes = bytes.Slice((int)bundleString.Length);

			timeTag.Write(bytes.Span);
			bytes = bytes.Slice((int)timeTag.Length);

			var payloadLength = new OscInt((int)payload.Length);
			payloadLength.Write(bytes.Span);
			bytes = bytes.Slice((int)payloadLength.Length);

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
	}
}
