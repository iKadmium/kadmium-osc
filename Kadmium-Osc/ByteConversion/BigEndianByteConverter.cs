using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;

namespace Kadmium_Osc.ByteConversion
{
	public class BigEndianByteConverter : ByteConverter
	{
		public override int GetInt32(ReadOnlyMemory<byte> bytes)
		{
			byte[] result = bytes.Slice(0, 4).ToArray();
			return BitConverter.ToInt32(result, 0);
		}

		public override float GetSingle(ReadOnlyMemory<byte> bytes)
		{
			byte[] result = bytes.Slice(0, 4).ToArray();
			return BitConverter.ToSingle(result, 0);
		}

		public override uint GetUInt32(ReadOnlyMemory<byte> bytes)
		{
			byte[] result = bytes.Slice(0, 4).ToArray();
			return BitConverter.ToUInt32(result, 0);
		}

		public override void Write(Memory<byte> bytes, uint value)
		{
			BitConverter.GetBytes(value).CopyTo(bytes);
		}

		public override void Write(Memory<byte> bytes, float value)
		{
			BitConverter.GetBytes(value).CopyTo(bytes);
		}

		public override void Write(Memory<byte> bytes, int value)
		{
			BitConverter.GetBytes(value).CopyTo(bytes);
		}
	}
}
