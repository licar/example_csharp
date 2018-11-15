using System;
using System.Linq;
using System.Threading;

namespace Modeling.Modes
{
	[Serializable]
	public abstract class RubbitsField : Field
	{
		private const int MAX_RUBBISH_AMOUNT = 3;

        protected int rubbitsAmount;
		private int tempRubbits = 0;
        protected Random random = new Random();

        public RubbitsField(bool random) : base()
		{
            rubbitsAmount = random ? GenerateRandom(MAX_RUBBISH_AMOUNT + 1) : 0;
		}

		public override void NextBeat(bool hunter = true)
		{
			FeedRabbits();
            Breeding();
		    base.NextBeat();
        }

		private void FeedRabbits()
		{
			var grassesJuiciness = Grass.Juiciness - rubbitsAmount;
			if (grassesJuiciness >= 0)
			{
				Grass.Juiciness = grassesJuiciness;
				return;
			}
			
			
			rubbitsAmount = Grass.Juiciness;
            Grass.Juiciness = 0;
            var migrateRabbishAmount = Math.Abs(grassesJuiciness);
            MigrateRubbit(migrateRabbishAmount);
        }

	    public void MigrateRubbit(int migrateRabbishAmount)
	    {
	        for (var i = 0; i != migrateRabbishAmount; ++i)
	        {
	            var alive = neighboads.Where(n => n.GetLocality() == Common.Enums.Locality.Field).ToArray();
	            if (!alive.Any())
	            {
	                return;
	            }
	            alive[GenerateRandom(alive.Count())].AddRubbit();
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

	    public override bool AddOneRubbit()
	    {
	        if (rubbitsAmount < MAX_RUBBISH_AMOUNT)
	        {
	            ++rubbitsAmount;
	            return true;
	        }

	        return false;
	    }


        public override void Refresh()
        {
            //check alive for max amount
            var sum = rubbitsAmount + tempRubbits;
            rubbitsAmount =  sum == MAX_RUBBISH_AMOUNT ? MAX_RUBBISH_AMOUNT : sum % MAX_RUBBISH_AMOUNT;
            tempRubbits = 0;
        }

        public override int GetRubbits()
        {
            return this.rubbitsAmount;
        }

        protected int GenerateRandom(int max)
        {
            Thread.Sleep(1);
            return random.Next(max);
        }
    }
}
