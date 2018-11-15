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


		public WolfsField(bool random) : base(random)
		{
            wolfsAmount = random ? GenerateRandom(MAX_WOLFS_AMOUNT + 1) : 0;
        }

		public override void NextBeat(bool hunter = true)
		{
            hunter = Eat();
            base.NextBeat(hunter);
        }

        private bool Eat()
        {
            if (wolfsAmount == huntersAmount && huntersAmount != 0)
            {
                MigrateHunters(huntersAmount);
                MigrateWolfs(wolfsAmount);
                wolfsAmount = 0;
                huntersAmount = 0;
                return false;
            }

            if (wolfsAmount > huntersAmount && huntersAmount > 0)
            {
                --huntersAmount;
                MigrateHunters(huntersAmount);
                huntersAmount = 0;
                return false;
            }

            if (wolfsAmount < huntersAmount && wolfsAmount > 0)
            {
                --wolfsAmount;
                MigrateWolfs(wolfsAmount);
                wolfsAmount = 0;
                return false;
            }

            if (wolfsAmount > 0 && rubbitsAmount > 0)
            {
                var tmpRubbitsAmount = rubbitsAmount - wolfsAmount * 2;

                rubbitsAmount = 0;
                if (tmpRubbitsAmount > 0)
                {
                    MigrateRubbit(tmpRubbitsAmount);
                }
                return true;
            }

            return true;
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

        public override bool AddOneWolf()
        {
            if (wolfsAmount < MAX_WOLFS_AMOUNT)
            {
                ++wolfsAmount;
                return true;
            }

            return false;

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
