using Modeling.Common.Enums;
using System.Collections.Generic;

namespace Modeling.Modes
{
	public class FieldCell : ICell
	{
        public Locality Locality { get;}

        protected IList<FieldCell> neighboads;

        public FieldCell(Locality locality)
        {
            Locality = locality;
            //neighboads = null;
        }

        public virtual void NextBeat(){}

        public virtual void AddRubbit(){}

        public virtual void Refresh() {}

        public virtual void AddHunter(){}

        public virtual void AddWolf() {}
    }
}
