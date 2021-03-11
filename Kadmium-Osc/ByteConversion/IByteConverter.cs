using System;
using System.Collections.Generic;
using System.Text;

namespace Kadmium_Osc.ByteConversion
{
	public interface IByteConverter
	{
		UInt32 GetUInt32(ReadOnlyMemory<byte> bytes);
		Single GetSingle(ReadOnlyMemory<byte> bytes);
		string GetAsciiString(ReadOnlyMemory<byte> bytes);
		Int32 GetInt32(ReadOnlyMemory<byte> bytes);
		DateTime GetTimeTag(ReadOnlyMemory<byte> bytes);
		ReadOnlyMemory<byte> GetBlob(ReadOnlyMemory<byte> value);
		OscMessage GetMessage(ReadOnlyMemory<byte> value);
		OscBundle GetBundle(ReadOnlyMemory<byte> value);
		OscPacket GetPacket(ReadOnlyMemory<byte> value);

		void Write(Memory<byte> bytes, UInt32 value);
		void Write(Memory<byte> bytes, Single value);
		void Write(Memory<byte> bytes, string value);
		void Write(Memory<byte> bytes, Int32 value);
		void Write(Memory<byte> bytes, DateTime value);
		void Write(Memory<byte> bytes, ReadOnlyMemory<byte> value);
		void Write(Memory<byte> bytes, OscMessage value);
		void Write(Memory<byte> bytes, OscBundle value);
		void Write(Memory<byte> bytes, OscPacket value);
	}
}
