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

		private void Listen()
		{
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

		public void Listen(string hostname, int port)
		{
			Client = new UdpClient(hostname, port);
			Listen();
		}

		public void Listen(int port)
		{
			Client = new UdpClient(port);
			Listen();
		}

		public async Task Send(string hostname, int port, ReadOnlyMemory<byte> bytes)
		{
			Client = new UdpClient();
			await Client.SendAsync(bytes.ToArray(), bytes.Span.Length, hostname, port);
		}

		public void Dispose()
		{
			TokenSource?.Cancel();
			Client?.Dispose();
		}
	}
}
