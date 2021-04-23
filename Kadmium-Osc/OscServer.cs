using Kadmium_Osc.Arguments;
using Kadmium_Udp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Kadmium_Osc
{
	public class OscServer : IDisposable, IOscServer
	{
		public OscServer() : this(new UdpWrapper(), new TimeProvider())
		{
		}

		internal OscServer(IUdpWrapper udpWrapper, ITimeProvider timeProvider)
		{
			UdpWrapper = udpWrapper;
			TimeProvider = timeProvider;
			RouteHandlers = new Dictionary<OscPattern, List<EventHandler<OscMessage>>>();
		}

		public event EventHandler<OscMessage> OnMessageReceived;

		public event EventHandler<OscMessage> OnUnhandledMessageReceived;

		private Dictionary<OscPattern, List<EventHandler<OscMessage>>> RouteHandlers { get; }
		private ITimeProvider TimeProvider { get; }
		private IUdpWrapper UdpWrapper { get; }
		public void AddAddressRoute(string address, EventHandler<OscMessage> eventHandler)
		{
			var pattern = new OscPattern(address);
			if (!RouteHandlers.ContainsKey(pattern))
			{
				RouteHandlers.Add(pattern, new List<EventHandler<OscMessage>>());
			}
			RouteHandlers[pattern].Add(eventHandler);
		}

		public void Dispose()
		{
			UdpWrapper.Dispose();
		}

		public void Listen(IPEndPoint endPoint)
		{
			UdpWrapper.Listen(endPoint);
			AddEventListener();
		}

		private void AddEventListener()
		{
			UdpWrapper.OnPacketReceived += async (object sender, UdpReceiveResult result) =>
			{
				OscPacket oscPacket = OscPacket.Parse(result.Buffer);
				await ProcessPacket(oscPacket);
			};
		}

		private IEnumerable<EventHandler<OscMessage>> GetMatchingHandlers(OscString address)
		{
			foreach (var (pattern, handlers) in RouteHandlers)
			{
				if (pattern.Matches(address))
				{
					foreach (var handler in handlers)
					{
						yield return handler;
					}
				}
			}
		}

		private async Task ProcessPacket(OscPacket packet)
		{
			if (packet is OscMessage message)
			{
				OnMessageReceived?.Invoke(this, message);
				var handlers = GetMatchingHandlers(message.Address).ToList();
				if (handlers.Count > 0)
				{
					foreach (var handler in handlers)
					{
						handler.Invoke(this, message);
					}
				}
				else
				{
					OnUnhandledMessageReceived?.Invoke(this, message);
				}
			}
			else if (packet is OscBundle bundle)
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
	}
}