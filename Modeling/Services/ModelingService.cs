using Modeling.Business;
using Modeling.Modes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modeling.Services
{
    class ModelingService
    {
        Command commad = new Command();
        Islend island;

        ModelingService(int height, int widht)
        {
            island = new Islend(height, widht);
        }

        public void NextBeat()
        {
            island.NextBeat();
        }

        public void Render(int state)
        {
            return commad.GetState(state);
        }

        public void Render()
        {

        }
    }
}
