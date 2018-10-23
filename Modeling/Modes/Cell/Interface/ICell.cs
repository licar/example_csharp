using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modeling.Modes
{
    public interface ICell
    {
        void AddRubbit();
        void Refresh();
        void NextBeat();
    }
}
