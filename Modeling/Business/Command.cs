using Modeling.Modes;
using System.Collections;
using System.Collections.Generic;

namespace Modeling.Business
{
    public class Command
    {
        public IList<Islend> states = new List<Islend>();

        public void SetState(Islend state)
        {
            states.Add(state);
        }

        public Islend GetState(int i)
        {
            if (i < 0 || i > states.Count - 1)
            {
                return null;
            }

            return states[i];
        }
	}
}