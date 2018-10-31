﻿using Modeling.Common.Enums;
using System.Collections.Generic;

namespace Modeling.Modes
{
	public class FieldCell : ICell
	{
        public Locality Locality { get;}

        protected IList<ICell> neighboads;

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
            this.neighboads = neighboads;
        }
    }
}