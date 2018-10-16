using System.Collections.Generic;
using System.Linq;
using Modeling.Common.Enums;

namespace Modeling.Modes
{
	public class Field : Cell
	{
		public Nature Sun { get; set; }
		public Nature Rain { get; set; }
		public Greass Grass { get; set; }

		public Field() : base(Locality.Field)
		{
			Sun.NextBeat();
			Rain.NextBeat();
		}

		public override void NextBeat()
		{
			Sun.NextBeat();
			Rain.NextBeat();

			if (neighboads.FirstOrDefault(n => n.Locality == Locality.River) != null)
			{
				MapGrassWithoutRiver();
			}
			else
			{
				MapGrassWithRiver();
			}
		}

		private void MapGrassWithoutRiver()
		{
			if (Sun.NatureState == NatureState.No)
			{
				switch (Rain.NatureState)
				{
					case NatureState.Strongest:
						Grass.Die();
						return;
					default:
						return;
				}
			}

			if (Sun.NatureState == NatureState.Light)
			{
				switch (Rain.NatureState)
				{
					case NatureState.Light:
						Grass.Rise();
						return; 
					case NatureState.Average:
						Grass.Rise();
						return;
					default:
						return;
						
				}
			}

			if (Sun.NatureState == NatureState.Average)
			{
				switch (Rain.NatureState)
				{
					case NatureState.Light:
						Grass.Rise();
						return; 
					case NatureState.Average:
						Grass.Rise();
						return;
					case NatureState.Strongest:
						Grass.Rise();
						return;
					default:
						return;
				}
			}

			if (Sun.NatureState == NatureState.Strongest)
			{
				switch (Rain.NatureState)
				{
					case NatureState.No:
						Grass.Die();
						return;
					case NatureState.Average:
						Grass.Rise();
						return;
					case NatureState.Strongest:
						Grass.Rise();
						return;
					default:
						return;
				}
			}
		}

		private void MapGrassWithRiver()
		{
			if (Sun.NatureState != NatureState.No)
			{
				Grass.Rise();
			}
		}
	}
}
