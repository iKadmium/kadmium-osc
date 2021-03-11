using Kadmium_Osc.ByteConversion;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kadmium_Osc
{
	public class OscServer : IDisposable
	{
		public EventHandler<OscMessage> OnMessageReceived { get; set; }
		private IUdpServer UdpServer { get; }
		private IByteConverter ByteConverter { get; }

		internal OscServer(IUdpServer udpServer, IByteConverter byteConverter)
		{
			UdpServer = udpServer;
			ByteConverter = byteConverter;
		}

		public OscServer() : this(new UdpServer(), BitConverter.IsLittleEndian ? (IByteConverter)new LittleEndianByteConverter() : new BigEndianByteConverter() )
		{
		}

		private void ProcessPacket(OscPacket packet)
		{
			if (packet is OscMessage message)
			{
				OnMessageReceived?.Invoke(this, message);
			}
			else if(packet is OscBundle bundle)
			{
				// deal with the timetag
				if (bundle.TimeTag.Value <= DateTime.Now)
				{
					foreach (var content in bundle.Contents)
					{
						ProcessPacket(content);
					}
				}
				else
				{
					Console.WriteLine("Waiting to process bundle at " + bundle.TimeTag.Value);
				}
			}
		}

		private void AddEventListener()
		{
			UdpServer.OnPacketReceived += (object sender, byte[] packet) =>
			{
				OscPacket oscPacket = ByteConverter.GetPacket(packet);
				ProcessPacket(oscPacket);
			};
		}

		public void Listen(int port)
		{
			UdpServer.Listen(port);
			AddEventListener();
		}

		public void Listen(string hostname, int port)
		{
			UdpServer.Listen(hostname, port);
			AddEventListener();
		}

		public void Dispose()
		{
			UdpServer.Dispose();
		}
	}
}
