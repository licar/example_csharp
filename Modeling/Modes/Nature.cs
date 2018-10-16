using System;
using Modeling.Common.Enums;

namespace Modeling.Modes
{
	public class Nature
	{
		public NatureState NatureState {get; set;}
		private readonly Random random = new Random();

		public Nature()
		{
			NextBeat();
		}

		public void NextBeat()
		{
			NatureState =  (NatureState)random.Next(0, 4);
		}
	}
}