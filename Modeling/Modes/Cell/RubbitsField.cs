using System;
using System.Linq;

namespace Modeling.Modes
{
	public abstract class RubbitsField : Field
	{
		private const int MAX_RUBBISH_AMOUNT = 3;

		public int RubbitsAmount { get; set; }
		private int tempRubbits = 0;

		protected readonly Random random = new Random();

		public RubbitsField() : base()
		{
			RubbitsAmount = random.Next(0, MAX_RUBBISH_AMOUNT);
		}

		public override void NextBeat()
		{
			base.NextBeat();
			FeedRabbits();
            Breeding();
		}

		private void FeedRabbits()
		{
			var grassesJuiciness = Grass.Juiciness - RubbitsAmount;
			if (grassesJuiciness >= 0)
			{
				Grass.Juiciness = grassesJuiciness;
				return;
			}
			
			Grass.Juiciness = 0;
			RubbitsAmount = RubbitsAmount - Grass.Juiciness;

			var migrateRabbishAmount = Math.Abs(grassesJuiciness);

			for (var i = 0; i != migrateRabbishAmount; ++i)
			{
				neighboads[random.Next(0, neighboads.Count() - 1)].AddRubbit();
                --RubbitsAmount;
			}
			
		}

        private void Breeding()
        {
            if (RubbitsAmount == 2)
            {
                ++RubbitsAmount;
            }
        }

        public override void AddRubbit()
        {
            ++tempRubbits;
        }

        public override void Refresh()
        {
            //check alive for grass loginc
            tempRubbits = Grass.Juiciness >= tempRubbits ? tempRubbits : tempRubbits - Grass.Juiciness;


            //check alive for max amount
            var sum = RubbitsAmount + tempRubbits;
            RubbitsAmount =  sum == MAX_RUBBISH_AMOUNT ? MAX_RUBBISH_AMOUNT : sum % MAX_RUBBISH_AMOUNT;
            tempRubbits = 0;
        }
    }
}
