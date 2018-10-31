using Modeling.Common.Enums;
using System.Collections.Generic;

namespace Modeling.Modes
{
    public interface ICell
    {
        void AddRubbit();
        void Refresh();
        void NextBeat();
        void AddHunter();
        void AddWolf();
        int GetWolfs();
        int GetRubbits();
        int GetHunters();
        IList<ICell> GetNeighboads();
        void SetNeighboads(IList<ICell> neighbords);
        Locality GetLocality();
        int GetJuiciness();
    }
}
