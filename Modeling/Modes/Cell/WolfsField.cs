using Modeling.Modes.Cell;
using System;
using System.Linq;

namespace Modeling.Modes
{
	[Serializable]
	public sealed class WolfsField : HunterField
    {
		private const int MAX_WOLFS_AMOUNT = 3;

        protected int wolfsAmount;
		private int tempWolfs = 0;


		public WolfsField() : base()
		{
            MigrateWolfs(tempWolfs);
            tempWolfs = 0;
            wolfsAmount = GenerateRandom(MAX_WOLFS_AMOUNT + 1);
        }

		public override void NextBeat()
		{
            Eat();
            base.NextBeat();
        }

        private void Eat()
        {
            if (wolfsAmount == huntersAmount && huntersAmount != 0)
            {
                MigrateHunters(huntersAmount);
                MigrateWolfs(wolfsAmount);
                wolfsAmount = 0;
                huntersAmount = 0;
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
                
                if (tmpRubbitsAmount > 0)
                {
                    rubbitsAmount = tmpRubbitsAmount;
                    return;
                }
                rubbitsAmount = 0;

                var migrateWolfs = Math.Abs(tmpRubbitsAmount) / 2;
                wolfsAmount = wolfsAmount - migrateWolfs;
                MigrateWolfs(migrateWolfs);


                return;
            }
        }

        private void MigrateWolfs(int count)
        {
            for (var i = 0; i != count; ++i)
            {
                var alive = neighboads.Where(n => n.GetLocality() == Common.Enums.Locality.Field).ToArray();
                if (!alive.Any())
                {
                    return;
                }
                alive[GenerateRandom(alive.Count())].AddWolf();
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
            wolfsAmount = sum <= MAX_WOLFS_AMOUNT ? sum : MAX_WOLFS_AMOUNT;
            tempWolfs = sum - wolfsAmount;
        }

        public override int GetWolfs()
        {
            return this.wolfsAmount;
        }
    }
}
