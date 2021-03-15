using System;
using Kadmium_Udp;
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
		private IUdpWrapper UdpWrapper { get; }

		internal OscServer(IUdpWrapper udpWrapper)
		{
			UdpWrapper = udpWrapper;
		}

		public OscServer() : this(new UdpWrapper())
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
			UdpWrapper.OnPacketReceived += (object sender, UdpReceiveResult result) =>
			{
				OscPacket oscPacket = OscPacket.Parse(result.Buffer);
				ProcessPacket(oscPacket);
			};
		}

		public void Listen(int port)
		{
			UdpWrapper.Listen(port);
			AddEventListener();
		}

		public void Listen(string hostname, int port)
		{
			UdpWrapper.Listen(hostname, port);
			AddEventListener();
		}

		public void Dispose()
		{
			UdpWrapper.Dispose();
		}
	}
}
