using Modeling.Modes.Cell;
using System;
using System.Linq;

namespace Modeling.Modes
{
	public class WolfsField : HunterField
    {
		private const int MAX_WOLFS_AMOUNT = 3;

        protected int wolfsAmount;
		private int tempWolfs = 0;

		protected readonly Random random = new Random();

		public WolfsField() : base()
		{
			rubbitsAmount = random.Next(0, MAX_WOLFS_AMOUNT);
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
            if (wolfsAmount == huntersAmount)
            {
                MigrateHunters(huntersAmount);
                MigrateWolfs(wolfsAmount);
                return;
            }

            if (wolfsAmount > huntersAmount && huntersAmount > 0)
            {
                --huntersAmount;
                MigrateHunters(huntersAmount);
                return;
            }

            if (wolfsAmount < huntersAmount && wolfsAmount > 0)
            {
                --wolfsAmount;
                for (var i = 0; i != wolfsAmount; ++i)
                {
                    MigrateWolfs(wolfsAmount);
                }
                return;
            }

            if (wolfsAmount > 0 && rubbitsAmount > 0)
            {
                var tmpRubbitsAmount = rubbitsAmount - wolfsAmount * 2;
                rubbitsAmount = tmpRubbitsAmount < 0 ? 0 : tmpRubbitsAmount;
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
            var sum = wolfsAmount + tempWolfs;
            var sumWolfs = sum <= MAX_WOLFS_AMOUNT ? sum : MAX_WOLFS_AMOUNT;
            wolfsAmount = sumWolfs;

            tempWolfs = wolfsAmount + tempWolfs - sumWolfs;
        }

        public override int GetWolfs()
        {
            return wolfsAmount;
        }
    }
}
