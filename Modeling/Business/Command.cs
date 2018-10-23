using Modeling.Modes;
using System.Collections;
using System.Collections.Generic;

namespace Modeling.Business
{
    public class Command
    {
        public IList<Island> states = new List<Island>();

        public void AddState(Island state)
        {
            states.Add(state);
        }

        public Island GetState(int i)
        {
            if (i < 0 || i > states.Count - 1)
            {
                return null;
            }

            return states[i];
        }

	    public int GetIndex(Island island)
	    {
		    return states.IndexOf(island);
	    }
	}
}