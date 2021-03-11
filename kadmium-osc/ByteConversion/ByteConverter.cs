using Kadmium_Osc.Arguments;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;

namespace Kadmium_Osc.ByteConversion
{
	public abstract class ByteConverter : IByteConverter
	{
		private static DateTime ReferenceDate { get; } = new DateTime(1900, 1, 1);

		public string GetAsciiString(ReadOnlyMemory<byte> bytes)
		{
			var length = bytes.Span.IndexOf((byte)0);
			var result = Encoding.ASCII.GetString(bytes.Slice(0, length).ToArray());
			return result;
		}

		public DateTime GetTimeTag(ReadOnlyMemory<byte> bytes)
		{
			var seconds = GetInt32(bytes.Slice(0, 4));
			var fraction = GetUInt32(bytes.Slice(4, 4));

			double fractionSeconds = seconds + ((double)fraction / UInt32.MaxValue);

			var date = ReferenceDate.AddSeconds(fractionSeconds);

			return date;
		}

		public abstract uint GetUInt32(ReadOnlyMemory<byte> bytes);
		public abstract float GetSingle(ReadOnlyMemory<byte> bytes);
		public abstract int GetInt32(ReadOnlyMemory<byte> bytes);

		public ReadOnlyMemory<byte> GetBlob(ReadOnlyMemory<byte> value)
		{
			int length = GetInt32(value);
			var result = new byte[length];
			value.Slice(4, length).CopyTo(result);
			return result;
		}

		public OscMessage GetMessage(ReadOnlyMemory<byte> value)
		{
			OscMessage message = new OscMessage
			{
				Address = GetAsciiString(value)
			};
			value = value.Slice(message.Address.Length);
			var typeTag = new OscString(GetAsciiString(value));
			value = value.Slice(typeTag.Length);

			foreach (char typeChar in typeTag.Value)
			{
				OscArgument argument = GetArgument(typeChar, value);
				if (argument != null)
				{
					message.Arguments.Add(argument);
					value.Slice(argument.Length);
				}
			}

			return message;
		}

		private OscArgument GetArgument(char typeTag, ReadOnlyMemory<byte> bytes)
		{
			switch (typeTag)
			{
				case 'i':
					return new OscInt(GetInt32(bytes));
				case 's':
					return new OscString(GetAsciiString(bytes));
				case 't':
					return new OscTimeTag(GetTimeTag(bytes));
				case 'b':
					return new OscBlob(GetBlob(bytes));
				case 'f':
					return new OscFloat(GetSingle(bytes));
				case ',':
					return null;
				default:
					Console.Error.WriteLine("Received unknown typetag: " + typeTag);
					return null;
			}
		}

		public OscBundle GetBundle(ReadOnlyMemory<byte> value)
		{
			OscBundle bundle = new OscBundle();

			var bundleString = new OscString(GetAsciiString(value));
			if (bundleString.Value != "#bundle")
			{
				throw new ArgumentException("The given data does not start with a #bundle tag");
			}
			value = value.Slice(bundleString.Length);
			bundle.TimeTag = GetTimeTag(value);
			value = value.Slice(bundle.TimeTag.Length);

			while (value.Length > 0)
			{
				var size = GetInt32(value);
				value = value.Slice(4);

				var contentBytes = value.Slice(0, size);
				value = value.Slice(size);
				var content = GetPacket(contentBytes);
				bundle.Contents.Add(content);
			}

			return bundle;
		}

		public OscPacket GetPacket(ReadOnlyMemory<byte> value)
		{
			var openingString = GetAsciiString(value);
			if (openingString == "#bundle")
			{
				return GetBundle(value);
			}
			else
			{
				return GetMessage(value);
			}
		}

		public void Write(Memory<byte> bytes, string value)
		{
			int length = Pad(value.Length + 1, 4);
			byte[] asciiBytes = new byte[length];
			Encoding.ASCII.GetBytes(value, 0, value.Length, asciiBytes, 0);
			asciiBytes.CopyTo(bytes);
		}

		public void Write(Memory<byte> bytes, ReadOnlyMemory<byte> value)
		{
			int paddedLength = Pad(value.Length, 4);
			Write(bytes, paddedLength);
			value.CopyTo(bytes.Slice(4, paddedLength));
		}

		public void Write(Memory<byte> bytes, DateTime value)
		{
			var totalSeconds = (value - ReferenceDate).TotalSeconds;
			var wholeSeconds = (int)Math.Floor(totalSeconds);
			var fractionSeconds = (totalSeconds - wholeSeconds);
			var fraction = (UInt32)(fractionSeconds * UInt32.MaxValue);

			Write(bytes, wholeSeconds);
			Write(bytes.Slice(4), fraction);
		}

		public abstract void Write(Memory<byte> bytes, uint value);
		public abstract void Write(Memory<byte> bytes, float value);
		public abstract void Write(Memory<byte> bytes, int value);

		public static int Pad(int original, int divisor)
		{
			int remainder = original % divisor;
			if (remainder != 0)
			{
				return original + (divisor - remainder);
			}
			else
			{
				return original;
			}
		}

		public void Write(Memory<byte> bytes, OscMessage value)
		{
			int byteStreamAddress = 0;

			Write(bytes, value.Address.Value);
			bytes = bytes.Slice(value.Address.Length);

			Write(bytes, value.TypeTag.Value);
			bytes = bytes.Slice(value.TypeTag.Length);

			foreach (var argument in value.Arguments)
			{
				if (argument.Length > 0)
				{
					if (argument is OscString str)
					{
						Write(bytes, str);
					}
					else if (argument is OscFloat f)
					{
						Write(bytes, f);
					}
					else if (argument is OscInt i)
					{
						Write(bytes, i);
					}
					else if (argument is OscTimeTag t)
					{
						Write(bytes, t);
					}
					else if (argument is OscBlob b)
					{
						Write(bytes, b);
					}
					bytes = bytes.Slice(argument.Length);
					byteStreamAddress += argument.Length;
				}
			}
		}

		public void Write(Memory<byte> bytes, OscBundle value)
		{
			var bundleString = new OscString("#bundle");
			
			Write(bytes, bundleString.Value);
			bytes.Slice(bundleString.Length);

			Write(bytes, value.TimeTag);
			bytes = bytes.Slice(value.TimeTag.Length);
			
			foreach (var element in value.Contents)
			{
				Write(bytes, value.Length);
				bytes = bytes.Slice(value.Length);
				Write(bytes, element);
			}
		}

		public void Write(Memory<byte> bytes, OscPacket value)
		{
			if (value is OscBundle bundle)
			{
				Write(bytes, bundle);
			}
			else if (value is OscMessage message)
			{
				Write(bytes, message);
			}
			else
			{
				throw new ArgumentException("Given value was neither a bundle nor a message");
			}
		}
	}
}
