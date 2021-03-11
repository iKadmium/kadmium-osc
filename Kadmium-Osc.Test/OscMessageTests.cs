using Kadmium_Osc.Arguments;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Kadmium_Osc.Test
{
	public class OscMessageTests
	{
		[Fact]
		public void Given_TheMessageHasArguments_When_TypeTagIsCalled_Then_TheTagsAreCorrect()
		{
			string expected = ",ifsb";

			OscMessage message = new OscMessage();
			message.Arguments.Add(5);
			message.Arguments.Add(10f);
			message.Arguments.Add("My string");
			message.Arguments.Add(new byte[] { 1, 2, 3 });

			Assert.Equal(expected, message.TypeTag);
		}

		[Fact]
		public void Given_TheMessageHasArguments_When_ArgumentsAreChecked_Then_TheValuesAreCorrect()
		{
			int first = 5;
			float second = 10f;
			string third = "My string";
			byte[] fourth = new byte[] { 1, 2, 3, 4 };

			OscMessage message = new OscMessage();
			message.Arguments.Add(first);
			message.Arguments.Add(second);
			message.Arguments.Add(third);
			message.Arguments.Add(fourth);

			Assert.Equal(first, message.GetArgument<OscInt>(0).Value);
			Assert.Equal(second, message.GetArgument<OscFloat>(1).Value);
			Assert.Equal(third, message.GetArgument<OscString>(2).Value);
			Assert.Equal(fourth, message.GetArgument<OscBlob>(3).Value);
		}
	}
}
