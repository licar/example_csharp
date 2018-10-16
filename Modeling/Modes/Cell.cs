using System.Collections.Generic;
using Modeling.Common.Enums;

namespace Modeling.Modes
{
	public class Cell
	{
        public Locality Locality { get;}
        public IEnumerable<Cell> neighboads;

        public Cell(Locality locality)
		{
			this.Locality = locality;
            this.neighboads = null;
		}

		public virtual void NextBeat()
        {
        }
	}
}
