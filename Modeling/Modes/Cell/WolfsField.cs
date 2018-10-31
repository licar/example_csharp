using Modeling.Modes.Cell;
using System;
using System.Linq;

namespace Modeling.Modes
{
	public abstract class WolfsField : HunterField
    {
		private const int MAX_WOLFS_AMOUNT = 3;

		public int WolfsAmount { get; set; }
		private int tempWolfs = 0;

		protected readonly Random random = new Random();

		public WolfsField() : base()
		{
			RubbitsAmount = random.Next(0, MAX_WOLFS_AMOUNT);
            MigrateWolfs(tempWolfs);
            tempWolfs = 0;
        }

		public override void NextBeat()
		{
			base.NextBeat();
            Eat();
        }

        private void Eat()
        {
            if (WolfsAmount == HuntersAmount)
            {
                MigrateHunters(HuntersAmount);
                MigrateWolfs(WolfsAmount);
                return;
            }

            if (WolfsAmount > HuntersAmount && HuntersAmount > 0)
            {
                --HuntersAmount;
                MigrateHunters(HuntersAmount);
                return;
            }

            if (WolfsAmount < HuntersAmount && WolfsAmount > 0)
            {
                --WolfsAmount;
                for (var i = 0; i != WolfsAmount; ++i)
                {
                    MigrateWolfs(WolfsAmount);
                }
                return;
            }

            if (WolfsAmount > 0 && RubbitsAmount > 0)
            {
                var tmpRubbitsAmount = RubbitsAmount - WolfsAmount * 2;
                RubbitsAmount = tmpRubbitsAmount < 0 ? 0 : tmpRubbitsAmount;
                return;
            }
        }

        protected void MigrateWolfs(int count)
        {
            for (var i = 0; i != count; ++i)
            {
                neighboads[random.Next(0, neighboads.Count() - 1)].AddWolf();
            }

        }

        public override void AddWolf()
        {
            ++tempWolfs;
        }

        public override void Refresh()
        {
            base.Refresh();

            //check hunters for max amount
            var sum = WolfsAmount + tempWolfs;
            var sumWolfs = sum <= MAX_WOLFS_AMOUNT ? sum : MAX_WOLFS_AMOUNT;
            WolfsAmount = sumWolfs;

            tempWolfs = WolfsAmount + tempWolfs - sumWolfs;
        }
    }
}
