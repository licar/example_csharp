using System;
using System.Linq;

namespace Modeling.Modes.Cell
{
    public class HunterField : RubbitsField
    {
        private const int MAX_HUNTERS_AMOUNT = 3;
        public int HuntersAmount { get; set; }
        private int tempHuntersAmount = 0;

        public HunterField() : base()
        {
          HuntersAmount = random.Next(0, MAX_HUNTERS_AMOUNT);
        }

        public override void NextBeat()
        {
            base.NextBeat();
            Hunt();

            //MigrateHunters(tempHuntersAmount);
            //tempHuntersAmount = 0;
        }

        public override void AddHunter()
        {
            ++tempHuntersAmount;
        }

        private void Hunt()
        {
            var rubbishLeft = RubbitsAmount - HuntersAmount;
            if (rubbishLeft >= 0)
            {
                RubbitsAmount = rubbishLeft;
                return;
            }

            HuntersAmount = 0;

            var migrateHunters = Math.Abs(HuntersAmount - RubbitsAmount);
            MigrateHunters(migrateHunters);
        }

        private void MigrateHunters(int count)
        {
            for (var i = 0; i != count; ++i)
            {
                neighboads[random.Next(0, neighboads.Count() - 1)].AddHunter();
            }
            
        }

        public override void Refresh()
        {
            base.Refresh();

            //check hunters for max amount
            var sum = HuntersAmount + tempHuntersAmount;
            var sumHunters = sum <= MAX_HUNTERS_AMOUNT ? sum : MAX_HUNTERS_AMOUNT;
            HuntersAmount = sumHunters;

            tempHuntersAmount = HuntersAmount + tempHuntersAmount - sumHunters;
        }
    }
}
