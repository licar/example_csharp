using System.Collections.Generic;
using Modeling.Common.Enums;

namespace Modeling.Modes
{
	public class Cell : ICell
	{
        public Locality Locality { get;}

        protected IList<Cell> neighboads;

        public Cell(Locality locality)
        {
            Locality = locality;
            neighboads = null;
        }

        public virtual void NextBeat(){}

        public virtual void AddRubbit(){}

        public virtual void Refresh() {}
    }
}
