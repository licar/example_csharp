using System;
using Modeling.Common.Enums;
using System.Collections.Generic;

namespace Modeling.Modes
{
	[Serializable]
	public class FieldCell : ICell
	{
        private Locality locality;

        protected IList<ICell> neighboads;

        public FieldCell(Locality locality)
        {
            this.locality = locality;
            neighboads = new List<ICell>();
        }

        public virtual void NextBeat(){}

        public virtual void AddRubbit(){}

        public virtual void Refresh() {}

        public virtual void AddHunter(){}

        public virtual void AddWolf() {}

        public virtual int GetWolfs()
        {
            return 0;
        }

        public virtual int GetRubbits()
        {
            return 0;
        }

        public virtual int GetHunters()
        {
            return 0; ;
        }

        public IList<ICell> GetNeighboads()
        {
           return neighboads;
        }

        public void SetNeighboads(IList<ICell> neighbords)
        {
            this.neighboads = neighbords;
        }

        public Locality GetLocality()
        {
            return locality;
        }

        public virtual int GetJuiciness()
        {
            return 0;
        }

        public virtual bool AddOneRubbit() {return  false;}

        public virtual bool AddOneHunter() { return false; }

        public virtual bool AddOneWolf() { return false; }

        public virtual int GetSun()
        {
            return 0;
        }

        public virtual int GetRain()
        {
            return 0;
        }
    }
}
