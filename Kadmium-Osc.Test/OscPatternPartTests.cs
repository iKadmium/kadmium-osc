using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Kadmium_Osc.Test
{
	public class OscPatternPartTests
	{
		[Theory]
		[InlineData("simple", "simple", true)]
		[InlineData("simple", "notSimple", false)]
		[InlineData("simple*", "simpleStuff", true)]
		[InlineData("simple*", "notSimple", false)]
		[InlineData("simple?", "simple1", true)]
		[InlineData("simple?", "simple12", false)]
		[InlineData("simple[123]", "simple1", true)]
		[InlineData("simple[123]", "simple2", true)]
		[InlineData("simple[123]", "simple3", true)]
		[InlineData("simple[1-5]", "simple3", true)]
		[InlineData("simple[1-5]", "simple5", true)]
		[InlineData("simple[1-5]", "simple1", true)]
		[InlineData("simple[1-5]", "simple6", false)]
		[InlineData("simple[!1-5]", "simple5", false)]
		[InlineData("simple[!1-5]", "simple8", true)]
		[InlineData("simple[a-e]", "simpleb", true)]
		[InlineData("simple[a-e]", "simplef", false)]
		[InlineData("sim{ple,par}", "simple", true)]
		[InlineData("sim{ple,par}", "simpar", true)]
		[InlineData("sim{ple,par}", "simfun", false)]
		public void When_IsMatchIsCalled_Then_TheResultIsAsExpected(string part, string other, bool expectedMatch)
		{
			OscPatternPart patternPart = new OscPatternPart(part);
			Assert.Equal(expectedMatch, patternPart.IsMatch(other));
		}
	}
}
