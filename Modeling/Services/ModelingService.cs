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
        Island island;

        ModelingService(int height, int widht)
        {
            island = new Island(height, widht);
	        commad.AddState(island);
        }

        public void NextBeat()
        {
            island.NextBeat();
        }

        //public void Render(int state)
        //{
        //    return commad.GetState(state);
        //}

        public void Render()
        {

        }
    }
}
