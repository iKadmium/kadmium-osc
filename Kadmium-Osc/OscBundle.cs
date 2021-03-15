using Kadmium_Osc.Arguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kadmium_Osc
{
	public class OscBundle : OscPacket, IEquatable<OscBundle>
	{
		public static OscString BundleString { get; } = "#bundle";
		
		public List<OscPacket> Contents { get; }
		public OscTimeTag TimeTag { get; set; }
		
		public override UInt32 Length
		{
			get
			{
				return (UInt32)(BundleString.Length + TimeTag.Length + Contents.Sum(x => x.Length) + (4 * Contents.Count));
			}
		}

		public OscBundle()
		{
			Contents = new List<OscPacket>();
		}

		public override void Write(Span<byte> bytes)
		{
			BundleString.Write(bytes);
			bytes = bytes.Slice((int)BundleString.Length);

			TimeTag.Write(bytes);
			bytes = bytes.Slice((int)TimeTag.Length);

			foreach (var element in Contents)
			{
				var length = new OscInt((int)element.Length);
				length.Write(bytes);
				bytes = bytes.Slice((int)length.Length);
				element.Write(bytes);
				bytes = bytes.Slice((int)element.Length);
			}
		}

		public static new OscBundle Parse(ReadOnlySpan<byte> bytes)
		{
			var bundleString = OscString.Parse(bytes);
			bytes = bytes.Slice((int)bundleString.Length);

			var timeTag = OscTimeTag.Parse(bytes);
			bytes = bytes.Slice((int)timeTag.Length);

			OscBundle bundle = new OscBundle();
			bundle.TimeTag = timeTag;

			while (bytes.Length > 0)
			{
				var packetLength = OscInt.Parse(bytes);
				bytes = bytes.Slice((int)packetLength.Length);

				var packetBytes = bytes.Slice(0, packetLength);
				OscPacket packet = OscPacket.Parse(packetBytes);
				bundle.Contents.Add(packet);
				bytes = bytes.Slice(packetLength);
			}

			return bundle;
		}

		public bool Equals(OscBundle other)
		{
			return other != null &&
				   Length == other.Length &&
				   Contents.SequenceEqual(other.Contents) &&
				   TimeTag.Equals(other.TimeTag) &&
				   Length == other.Length;
		}
	}
}
