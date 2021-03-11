using Kadmium_Osc.Arguments;
using Kadmium_Osc.ByteConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kadmium_Osc
{
	public class OscBundle : OscPacket
	{
		public List<OscPacket> Contents { get; }
		public OscTimeTag TimeTag { get; set; }
		public OscString BundleString { get; } = "#bundle";
		public override OscInt Length
		{
			get
			{
				return BundleString.Length + TimeTag.Length + Contents.Sum(x => x.Length);
			}
		}

		public OscBundle()
		{
			Contents = new List<OscPacket>();
		}
	}
}
