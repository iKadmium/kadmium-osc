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
	public class OscBlobTests
	{
		[Fact]
		public void When_GetBlobIsCalled_Then_TheDataIsCorrect()
		{
			var rawData = new byte[8] { 0, 0, 0, 4, 1, 2, 3, 4 };
			var expected = new byte[4] { 1, 2, 3, 4 };

			var blob = OscBlob.Parse(rawData);
			Assert.Equal(expected, blob.Value);
		}

		[Fact]
		public void When_WriteIsCalledOnABlob_Then_TheDataIsWrittenCorrectly()
		{
			var expected = new byte[8] { 0, 0, 0, 4, 1, 2, 3, 4 };
			var payload = new byte[4] { 1, 2, 3, 4 };
			
			OscBlob blob = new OscBlob(payload);

			using var owner = MemoryPool<byte>.Shared.Rent();
			var bytes = owner.Memory.Slice(0, 8);
			blob.Write(bytes.Span);
			Assert.Equal(expected, bytes.ToArray());
		}
	}
}
