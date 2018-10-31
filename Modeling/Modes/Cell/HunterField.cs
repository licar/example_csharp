using System;
using System.Linq;

namespace Modeling.Modes.Cell
{
    public class HunterField : RubbitsField
    {
        private const int MAX_HUNTERS_AMOUNT = 3;
        protected int huntersAmount;
        private int tempHuntersAmount = 0;

        public HunterField() : base() 
        {
            huntersAmount = GenerateRandom(0, MAX_HUNTERS_AMOUNT + 1);
        }

        public override void NextBeat()
        {
            base.NextBeat();
            Hunt();

            MigrateHunters(tempHuntersAmount);
            tempHuntersAmount = 0;
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

            huntersAmount = 0;

            var migrateHunters = Math.Abs(huntersAmount - rubbitsAmount);
            MigrateHunters(migrateHunters);
        }

        protected void MigrateHunters(int count)
        {
            for (var i = 0; i != count; ++i)
            {
                neighboads[GenerateRandom(0, neighboads.Count())].AddHunter();
            }
            
        }

        public override void Refresh()
        {
            base.Refresh();

            //check hunters for max amount
            var sum = huntersAmount + tempHuntersAmount;
            var sumHunters = sum <= MAX_HUNTERS_AMOUNT ? sum : MAX_HUNTERS_AMOUNT;
            huntersAmount = sumHunters;

            tempHuntersAmount = huntersAmount + tempHuntersAmount - sumHunters;
        }

        public override int GetHunters()
        {
            return this.huntersAmount;
        } 

    }
}
