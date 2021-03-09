using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kadmium_Osc.ByteConversion
{
	public class LittleEndianByteConverter : ByteConverter
	{
		public override int GetInt32(ReadOnlyMemory<byte> bytes)
		{
			byte[] result = bytes.Slice(0, 4).ToArray();
			Array.Reverse(result);
			return BitConverter.ToInt32(result, 0);
		}

		public override float GetSingle(ReadOnlyMemory<byte> bytes)
		{
			byte[] result = bytes.Slice(0, 4).ToArray();
			Array.Reverse(result);
			return BitConverter.ToSingle(result, 0);
		}

		public override uint GetUInt32(ReadOnlyMemory<byte> bytes)
		{
			byte[] result = bytes.Slice(0, 4).ToArray();
			Array.Reverse(result);
			return BitConverter.ToUInt32(result, 0);
		}

		public override void Write(Memory<byte> bytes, uint value)
		{
			var tempBytes = BitConverter.GetBytes(value);
			Array.Reverse(tempBytes);
			tempBytes.CopyTo(bytes);
		}

		public override void Write(Memory<byte> bytes, float value)
		{
			var tempBytes = BitConverter.GetBytes(value);
			Array.Reverse(tempBytes);
			tempBytes.CopyTo(bytes);
		}

		public override void Write(Memory<byte> bytes, int value)
		{
			var tempBytes = BitConverter.GetBytes(value);
			Array.Reverse(tempBytes);
			tempBytes.CopyTo(bytes);
		}
	}
}
