using Kadmium_Osc.Arguments;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kadmium_Osc
{
	public abstract class OscPacket : IEquatable<OscPacket>
	{
		public abstract UInt32 Length { get; }
		public abstract void Write(Span<byte> bytes);

		public static OscPacket Parse(ReadOnlySpan<byte> value)
		{
			var openingString = OscString.Parse(value);
			if (openingString == "#bundle")
			{
				return OscBundle.Parse(value);
			}
			else
			{
				return OscMessage.Parse(value);
			}
		}

		public bool Equals(OscPacket other)
		{
			if (this is OscMessage thisMsg && other is OscMessage otherMsg)
			{
				return thisMsg.Equals(otherMsg);
			}
			if (this is OscBundle thisBundle && other is OscBundle otherBundle)
			{
				return thisBundle.Equals(otherBundle);
			}
			return false;
		}
	}
}
