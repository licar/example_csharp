using System;
using System.Threading;
using Modeling.Common.Enums;

namespace Modeling.Modes
{
	public class Nature
	{
		public NatureState NatureState {get; set;}
		private readonly Random random = new Random();

		public Nature()
		{
			RefreshState();
		}

		public void RefreshState()
		{
            Thread.Sleep(5);
            NatureState =  (NatureState)random.Next(0, 4);
		}
	}
}