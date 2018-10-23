using System;
using System.Linq;

namespace Modeling.Modes
{
	class LivingField : Field
	{
		private const int MAX_RUBBISH_AMOUNT = 3;

		public int RubbishAmount { get; set; }
		public int TempRubbish { get; set; } = 0;

		private readonly Random random = new Random();

		public LivingField()
		{
			RubbishAmount = random.Next(0, MAX_RUBBISH_AMOUNT);
		}

		public override void NextBeat()
		{
			base.NextBeat();
			FeedRabbits();
		}

		public virtual void Migrate()
		{

		}


		private void FeedRabbits()
		{
			var grassesJuiciness = Grass.Juiciness - RubbishAmount;
			if (grassesJuiciness >= 0)
			{
				Grass.Juiciness = grassesJuiciness;
				return;
			}
			
			Grass.Juiciness = 0;
			RubbishAmount = RubbishAmount - Grass.Juiciness;

			var migrateRabbishAmount = Math.Abs(grassesJuiciness);

			for (var i = 0; i != migrateRabbishAmount; ++i)
			{
				neighboads[random.Next(0, neighboads.Count() - 1)].;
			}
			
		}
	}
}
