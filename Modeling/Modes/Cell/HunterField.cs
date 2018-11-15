using System;
using System.Linq;

namespace Modeling.Modes.Cell
{
	[Serializable]
	public class HunterField : RubbitsField
    {
        private const int MAX_HUNTERS_AMOUNT = 3;
        protected int huntersAmount;
        private int tempHuntersAmount = 0;

        public HunterField(bool random) : base(random) 
        {
            huntersAmount = random ? GenerateRandom(MAX_HUNTERS_AMOUNT + 1) : 0;
        }

        public override void NextBeat(bool hunter = true)
        {
            if (hunter)
            {
                Hunt();
            }
            base.NextBeat();
        }

        public override void AddHunter()
        {
            ++tempHuntersAmount;
        }

        private void Hunt()
        {
            var rubbishLeft = rubbitsAmount - huntersAmount;
            if (rubbishLeft >= 0)
            {
                rubbitsAmount = rubbishLeft;
                return;
            }

            var migrateHunters = Math.Abs(huntersAmount - rubbitsAmount);
            rubbitsAmount = 0;
            huntersAmount = huntersAmount - migrateHunters;

            MigrateHunters(migrateHunters);
        }

        protected void MigrateHunters(int count)
        {
            for (var i = 0; i != count; ++i)
            {
                var alive = neighboads.Where(n => n.GetLocality() == Common.Enums.Locality.Field).ToArray();
                if (!alive.Any())
                {
                    return;
                }
                alive[GenerateRandom(alive.Count())].AddHunter();
            }
            
        }

        public override bool AddOneHunter()
        {
            if (huntersAmount < MAX_HUNTERS_AMOUNT)
            {
                ++huntersAmount;
                return true;
            }

            return false;
        }

        public override void Refresh()
        {
            base.Refresh();

            //check hunters for max amount
            var sum = huntersAmount + tempHuntersAmount;
            huntersAmount = sum <= MAX_HUNTERS_AMOUNT ? sum : MAX_HUNTERS_AMOUNT;
            tempHuntersAmount = sum - huntersAmount;
        }

        public override int GetHunters()
        {
            return this.huntersAmount;
        } 

    }
}
