using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kadmium_Osc
{
	internal class UdpServer : IUdpServer, IDisposable
	{
		private UdpClient Client { get; set; }

		private CancellationTokenSource TokenSource { get; set; }

		public event EventHandler<byte[]> OnPacketReceived;

		public void Listen(IPEndPoint endPoint)
		{
			Client = new UdpClient(endPoint);
			TokenSource = new CancellationTokenSource();
			var token = TokenSource.Token;
			Task.Run(async () =>
			{
				while (!token.IsCancellationRequested)
				{
					var result = await Client.ReceiveAsync();
					OnPacketReceived?.Invoke(this, result.Buffer);
				}
			});
		}

		public void Dispose()
		{
			TokenSource.Cancel();
			Client.Dispose();
		}
	}
}
