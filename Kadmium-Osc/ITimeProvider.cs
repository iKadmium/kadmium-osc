using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kadmium_Osc
{
	public interface ITimeProvider
	{
		public DateTime Now { get; }
		public Task WaitUntil(DateTime time);
	}
}
