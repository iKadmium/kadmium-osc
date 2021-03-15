using System;
using System.Buffers;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Kadmium_Udp;

namespace Kadmium_Osc
{
	public class OscClient : IDisposable
	{
		private IUdpWrapper UdpWrapper { get; }
		
		internal OscClient(IUdpWrapper udpWrapper)
		{
			UdpWrapper = udpWrapper;
		}

		public OscClient() : this(new UdpWrapper())
		{
		}

		public async Task Send(string hostname, int port, OscPacket packet)
		{
			using (var owner = MemoryPool<byte>.Shared.Rent((int)packet.Length))
			{
				var bytes = owner.Memory.Slice(0, (int)packet.Length);
				packet.Write(bytes.Span);
				await UdpWrapper.Send(hostname, port, bytes);
			}
		}

		public void Dispose()
		{
			UdpWrapper.Dispose();
		}
	}
}
