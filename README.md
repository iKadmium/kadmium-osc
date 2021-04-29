# kadmium-osc
An OSC Library for .Net

[![codecov](https://codecov.io/gh/iKadmium/kadmium-osc/branch/main/graph/badge.svg?token=2QMOKGS6OD)](https://codecov.io/gh/iKadmium/kadmium-osc)
![build status](https://github.com/iKadmium/kadmium-osc/actions/workflows/publish.yml/badge.svg)
## Usage

### Receiving OSC events

```c#
int port = 10000;
using OscServer server = new OscServer();

// Listen for any message at all
server.OnMessageReceived += (object sender, OscMessage message) => 
{
	Console.WriteLine(message.Address);
	foreach (var argument in message.Arguments)
	{
		if (argument is OscString str)
		{
			Console.WriteLine(str); 
			string otherString = str;
		}
		else if (argument is OscFloat flt)
		{
			// process it somehow...
		}
	}
	// OscStrings, OscFloats, etc, have implicit conversions to their C# types
	float firstArgument = message.GetArgument<OscFloat>(0);
};

// Listen for messages to a specific address route
server.AddAddressRoute("/stopAll", (sender, msg) => Process(msg));
server.AddAddressRoute("/track/*/{play,stop}", (sender, msg) => Process(msg));

// Listen for any messages that don't match any address routes
server.OnUnhandledMessageReceived += (sender, msg) => ProcessUnhandled(msg);

server.Listen(IPAddress.Loopback.ToString(), port);
```

## Sending OSC events

```c#
int port = 10000;
string address = "/test";
string payload = "Hello world!";
string hostname = "example.com";

using OscClient client = new OscClient();

OscMessage message = new OscMessage(address, payload);
await client.Send(hostname, port, message);
```