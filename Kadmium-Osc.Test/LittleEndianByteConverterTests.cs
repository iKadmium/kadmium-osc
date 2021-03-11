using Kadmium_Osc.ByteConversion;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Kadmium_Osc.Test
{
	public class LittleEndianByteConverterTests
	{
		#region TimeTag
		[Fact]
		public void When_GetTimeTagIsCalledWithAFractionalTime_Then_TheValueIsParsedCorrectly()
		{
			var expected = new DateTime(1900, 1, 1, 0, 0, 0, 500);
			var bytes = new byte[8] { 0, 0, 0, 0, 0x7F, 0xFF, 0xFF, 0xFF };

			LittleEndianByteConverter converter = new LittleEndianByteConverter();
			Assert.Equal(expected, converter.GetTimeTag(bytes));
		}

		[Fact]
		public void When_GetTimeTagIsCalledWithJustSecondData_Then_TheTagIsCreatedProperly()
		{
			var expected = new DateTime(1900, 1, 1, 0, 0, 1, 0);
			var bytes = new byte[8] { 0, 0, 0, 0x01, 0, 0, 0, 0 };

			LittleEndianByteConverter converter = new LittleEndianByteConverter();
			Assert.Equal(expected, converter.GetTimeTag(bytes));
		}

		[Fact]
		public void When_WriteIsCalledForATimeTag_Then_TheDataIsCorrect()
		{
			var value = new DateTime(1900, 1, 1, 0, 0, 1, 0);
			var expected = new byte[8] { 0, 0, 0, 1, 0, 0, 0, 0 };

			LittleEndianByteConverter converter = new LittleEndianByteConverter();

			using (var owner = MemoryPool<byte>.Shared.Rent())
			{
				var bytes = owner.Memory.Slice(0, 8);
				converter.Write(bytes, value);
				Assert.Equal(expected, bytes.ToArray());
			}
		}
		#endregion TimeTag

		#region Single
		[Fact]
		public void When_GetSingleIsCalled_Then_TheDataIsParsedProperly()
		{
			var expected = 42f;
			var bytes = new byte[4] { 0x42, 0x28, 0x00, 0x00 };

			LittleEndianByteConverter converter = new LittleEndianByteConverter();
			Assert.Equal(expected, converter.GetSingle(bytes));
		}

		[Fact]
		public void When_WriteIsCalledForASingle_Then_TheDataIsCorrect()
		{
			var value = 42f;
			var expected = new byte[4] { 0x42, 0x28, 0x00, 0x00 };

			LittleEndianByteConverter converter = new LittleEndianByteConverter();

			using (var owner = MemoryPool<byte>.Shared.Rent())
			{
				var bytes = owner.Memory.Slice(0, 4);
				converter.Write(bytes, value);
				Assert.Equal(expected, bytes.ToArray());
			}
		}
		#endregion Single

		#region Blob
		[Fact]
		public void When_GetBlobIsCalled_Then_TheDataIsCorrect()
		{
			ByteConverter converter = new LittleEndianByteConverter();
			var rawData = new byte[8] { 0, 0, 0, 4, 1, 2, 3, 4 };
			var expected = new byte[4] { 1, 2, 3, 4 };

			var blob = converter.GetBlob(rawData);
			Assert.Equal(expected, blob.ToArray());
		}

		[Fact]
		public void When_WriteIsCalledOnABlob_Then_TheDataIsWrittenCorrectly()
		{
			ByteConverter converter = new LittleEndianByteConverter();
			var expected = new byte[8] { 0, 0, 0, 4, 1, 2, 3, 4 };
			var payload = new byte[4] { 1, 2, 3, 4 };

			using (var owner = MemoryPool<byte>.Shared.Rent())
			{
				var bytes = owner.Memory.Slice(0, 8);
				converter.Write(bytes, payload);
				Assert.Equal(expected, bytes.ToArray());
			}
		}
		#endregion Blob

		#region Int32
		[Fact]
		public void When_GetInt32IsCalled_Then_TheDataIsCorrect()
		{
			ByteConverter converter = new LittleEndianByteConverter();
			var rawData = new byte[4] { 0, 0, 0, 42 };
			var expected = 42;

			var actual = converter.GetInt32(rawData);
			Assert.Equal(expected, actual);
		}

		[Fact]
		public void When_WriteIsCalledOnAnInt32_Then_TheDataIsWrittenCorrectly()
		{
			ByteConverter converter = new LittleEndianByteConverter();
			var value = 42;
			var expected = new byte[4] { 0, 0, 0, 42 };

			using (var owner = MemoryPool<byte>.Shared.Rent())
			{
				var bytes = owner.Memory.Slice(0, 4);
				converter.Write(bytes, value);
				Assert.Equal(expected, bytes.ToArray());
			}
		}
		#endregion Int32
	}
}
