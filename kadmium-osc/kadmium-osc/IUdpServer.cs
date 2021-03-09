using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Kadmium_Osc
{
	internal interface IUdpServer : IDisposable
	{
		event EventHandler<byte[]> OnPacketReceived;
		void Listen(IPEndPoint endPoint);
	}
}
