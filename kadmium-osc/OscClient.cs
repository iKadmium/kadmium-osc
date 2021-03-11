using System;
using System.Buffers;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Kadmium_Osc.ByteConversion;

namespace Kadmium_Osc
{
	public class OscClient : IDisposable
	{
		private IUdpServer UdpServer { get; }
		private IByteConverter ByteConverter { get; }

		internal OscClient(IUdpServer udpServer, IByteConverter byteConverter)
		{
			UdpServer = udpServer;
			ByteConverter = byteConverter;
		}

		public OscClient() : this(new UdpServer(), BitConverter.IsLittleEndian ? (IByteConverter)new LittleEndianByteConverter() : new BigEndianByteConverter())
		{
		}

		public async Task Send(string hostname, int port, OscPacket packet)
		{
			using (var owner = MemoryPool<byte>.Shared.Rent(packet.Length))
			{
				var bytes = owner.Memory.Slice(0, packet.Length);
				ByteConverter.Write(bytes, packet);
				await UdpServer.Send(hostname, port, bytes);
			}
		}

		public void Dispose()
		{
			UdpServer.Dispose();
		}
	}
}
