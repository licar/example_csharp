using System.Collections.Generic;
using Modeling.Common.Enums;

namespace Modeling.Modes
{
	public class Cell
	{
        public Locality Locality { get;}
        public IList<Cell> neighboads;

        public Cell(Locality locality)
		{
			Locality = locality;
			neighboads = null;
		}

		public virtual void NextBeat()
        {
        }
	}
}
