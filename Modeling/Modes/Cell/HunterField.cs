using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modeling.Modes.Cell
{
    public class HunterField : RubbitsField
    {
        private const int MAX_HUNTERS_AMOUNT = 3;
        public int HuntersAmount { get; set; }

        public HunterField() : base()
        {
          HuntersAmount = random.Next(0, MAX_HUNTERS_AMOUNT);
        }
        

    }
}
