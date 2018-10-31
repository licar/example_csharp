using System;
using System.Linq;
using System.Threading;

namespace Modeling.Modes
{
	public abstract class RubbitsField : Field
	{
		private const int MAX_RUBBISH_AMOUNT = 3;

        protected int rubbitsAmount;
		private int tempRubbits = 0;
        protected Random random = new Random();

        public RubbitsField() : base()
		{
            rubbitsAmount = GenerateRandom(0, MAX_RUBBISH_AMOUNT + 1);
		}

		public override void NextBeat()
		{
			base.NextBeat();
			FeedRabbits();
            Breeding();
		}

		private void FeedRabbits()
		{
			var grassesJuiciness = Grass.Juiciness - rubbitsAmount;
			if (grassesJuiciness >= 0)
			{
				Grass.Juiciness = grassesJuiciness;
				return;
			}
			
			Grass.Juiciness = 0;
			rubbitsAmount = rubbitsAmount - Grass.Juiciness;

			var migrateRabbishAmount = Math.Abs(grassesJuiciness);

			for (var i = 0; i != migrateRabbishAmount; ++i)
			{
                neighboads[GenerateRandom(0, neighboads.Count())].AddRubbit();
                --rubbitsAmount;
			}
			
		}

        private void Breeding()
        {
            if (rubbitsAmount == 2)
            {
                ++rubbitsAmount;
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
            var sum = rubbitsAmount + tempRubbits;
            rubbitsAmount =  sum == MAX_RUBBISH_AMOUNT ? MAX_RUBBISH_AMOUNT : sum % MAX_RUBBISH_AMOUNT;
            tempRubbits = 0;
        }

        public override int GetRubbits()
        {
            return this.rubbitsAmount;
        }

        protected int GenerateRandom(int min, int max)
        {
            Thread.Sleep(1);
            return random.Next(min, max);
        }
    }
}
