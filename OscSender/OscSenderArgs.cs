using PowerArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OscSender
{
	public class OscSenderArgs
	{
		[ArgShortcut("h")]
		[ArgRequired]
		public string Hostname { get; set; }
		[ArgRequired]
		[ArgShortcut("a")]
		public string Address { get; set; }

		[ArgRequired]
		[ArgShortcut("p")]
		public int Port { get; set; }

		[ArgShortcut("s")]
		public string StringValue { get; set; }
	}
}
