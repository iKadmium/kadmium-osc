using System;
using System.Net;

namespace Kadmium_Osc
{
	public interface IOscServer
	{
		/// <summary>
		/// This is invoked whenever a message is received, regardless of whether it is handled
		/// </summary>
		event EventHandler<OscMessage> OnMessageReceived;
		/// <summary>
		/// This is only invoked when a message received but it does not match any route handlers
		/// </summary>
		event EventHandler<OscMessage> OnUnhandledMessageReceived;

		/// <summary>
		/// Adds a handler to the route handler list. This will be invoked when a message is received
		/// matching the given route pattern.
		/// '?' in the OSC Address Pattern matches any single character
		/// '*' in the OSC Address Pattern matches any sequence of zero or more characters
		/// A string of characters in square brackets(e.g., "[string]") in the OSC Address Pattern matches any character in the string. Inside square brackets, the minus sign(-) and exclamation point(!) have special meanings:
		///    two characters separated by a minus sign indicate the range of characters between the given two in ASCII collating sequence. (A minus sign at the end of the string has no special meaning.)
		///    An exclamation point at the beginning of a bracketed string negates the sense of the list, meaning that the list matches any character not in the list. (An exclamation point anywhere besides the first character after the open bracket has no special meaning.)
		/// A comma-separated list of strings enclosed in curly braces(e.g., "{foo,bar}") in the OSC Address Pattern matches any of the strings in the list.
		/// </summary>
		/// <param name="address">The address to register</param>
		/// <param name="eventHandler">A handler to invoke when messages are received</param>
		void AddAddressRoute(string address, EventHandler<OscMessage> eventHandler);
		/// <summary>
		/// Start listening for messages on the specified endpoint.
		/// To listen on every available IPv4 address, use IPAddress.Any or IPAddress.IPv6Any
		/// </summary>
		/// <param name="endPoint">The endpoint on which to listen</param>
		void Listen(IPEndPoint endPoint);
	}
}