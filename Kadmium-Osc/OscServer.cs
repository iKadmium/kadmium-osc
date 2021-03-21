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
		private ITimeProvider TimeProvider { get; }

		internal OscServer(IUdpWrapper udpWrapper, ITimeProvider timeProvider)
		{
			UdpWrapper = udpWrapper;
			TimeProvider = timeProvider;
		}

		public OscServer() : this(new UdpWrapper(), new TimeProvider())
		{
		}

		private async Task ProcessPacket(OscPacket packet)
		{
			if (packet is OscMessage message)
			{
				OnMessageReceived?.Invoke(this, message);
			}
			else if(packet is OscBundle bundle)
			{
				// deal with the timetag
				if (bundle.TimeTag.Value > TimeProvider.Now)
				{
					await TimeProvider.WaitUntil(bundle.TimeTag.Value);
				}

				var tasks = new List<Task>();
				foreach (var content in bundle.Contents)
				{
					tasks.Add(ProcessPacket(content));
				}
				await Task.WhenAll(tasks);
			}
		}

		private void AddEventListener()
		{
			UdpWrapper.OnPacketReceived += async (object sender, UdpReceiveResult result) =>
			{
				OscPacket oscPacket = OscPacket.Parse(result.Buffer);
				await ProcessPacket(oscPacket);
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
