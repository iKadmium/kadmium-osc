using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Kadmium_Osc
{
	internal interface IUdpServer : IDisposable
	{
		event EventHandler<byte[]> OnPacketReceived;
		void Listen(string hostname, int port);
		Task Send(string hostname, int port, ReadOnlyMemory<byte> packet);
	}
}
