using System;

namespace Modeling.Modes
{
	[Serializable]
	public class Greass
	{
		public int Juiciness { get; set; } = 0;

		public void Rise()
		{
			if (Juiciness < 5)
			{
				++Juiciness;
			}
		}

		public void Die()
		{
			Juiciness = 0;
		}
	}
}