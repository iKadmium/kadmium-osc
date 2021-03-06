using System;
using System.Collections.Generic;
using System.Text;

namespace Kadmium_Osc.Arguments
{
	public abstract class OscArgument : IEquatable<OscArgument>
	{
		public abstract UInt32 Length { get; }
		public abstract char TypeTag { get; }
		public abstract void Write(Span<byte> bytes);

		public static implicit operator OscArgument(string s) => new OscString(s);
		public static implicit operator OscArgument(int i) => new OscInt(i);
		public static implicit operator OscArgument(float f) => new OscFloat(f);
		public static implicit operator OscArgument(byte[] b) => new OscBlob(b);
		public static implicit operator OscArgument(DateTime d) => new OscTimeTag(d);

		public static OscArgument Parse(ReadOnlySpan<byte> bytes, char typeTag)
		{
			switch (typeTag)
			{
				case 'i':
					return OscInt.Parse(bytes);
				case 's':
					return OscString.Parse(bytes);
				case 't':
					return OscTimeTag.Parse(bytes);
				case 'b':
					return OscBlob.Parse(bytes);
				case 'f':
					return OscFloat.Parse(bytes);
				case ',':
					return null;
				default:
					Console.Error.WriteLine("Received unknown typetag: " + typeTag);
					return null;
			}
		}

		public static UInt32 Pad(UInt32 original, UInt32 divisor)
		{
			var remainder = original % divisor;
			if (remainder != 0)
			{
				return original + (divisor - remainder);
			}
			else
			{
				return original;
			}
		}

		public bool Equals(OscArgument other)
		{
			if(other == null)
			{
				return false;
			}
			if (this is OscString thisStr && other is OscString otherStr)
			{
				return thisStr.Equals(otherStr);
			}
			else if (this is OscInt thisInt && other is OscInt otherInt)
			{
				return thisInt.Equals(otherInt);
			}
			else if (this is OscFloat thisFloat && other is OscFloat otherFloat)
			{
				return thisFloat.Equals(otherFloat);
			}
			else if (this is OscBlob thisBlob && other is OscBlob otherBlob)
			{
				return thisBlob.Equals(otherBlob);
			}
			else if (this is OscTimeTag thisTimeTag && other is OscTimeTag otherTimeTag)
			{
				return thisTimeTag.Equals(otherTimeTag);
			}
			return false;
		}
	}
}
