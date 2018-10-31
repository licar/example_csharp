using Modeling.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Modeling.Modes
{
	public class Island
	{
		public ICell[,] Cells { get; }
        private Random random = new Random();

        public Island(int height, int widht)
		{
            Cells = new ICell[height, widht];
            GenerateIslend();
            FillNeighbods();
        }

        public void NextBeat()
        {
            for (int i = 0; i != Cells.GetLength(0); ++i)
            {
                for (int j = 0; j != Cells.GetLength(1); ++j)
                {
                    Cells[i, j].NextBeat();
                }
            }
        }

        private void GenerateIslend()
        {
            for (int i = 0; i != Cells.GetLength(0); ++i)
            {
                for (int j = 0; j != Cells.GetLength(1); ++j)
                {
                    Cells[i, j] = GenerateCell();
                }
            }
        }

        private FieldCell GenerateCell()
        {
            var max = Enum.GetValues(typeof(Locality)).Cast<int>().Max();
            var min = Enum.GetValues(typeof(Locality)).Cast<int>().Min();

            
            var locality = (Locality) random.Next(min, max + 1);
            
            if (locality == Locality.Field)
            {
                return new WolfsField();
            }

            return new FieldCell(locality);
        }

        private void FillNeighbods()
        {
            for (int i = 0; i != Cells.GetLength(0) - 1; ++i)
            {
                for (int j = 0; j != Cells.GetLength(1) - 1; ++j)
                {
                    var neighboards = new List<ICell>();
                    
                    for (int neighHeight = i - 1; neighHeight <= i + 1; ++neighHeight)
                    {
                        if (neighHeight < 0 || neighHeight > Cells.GetLength(0))
                        {
                            continue;
                        }
                        for (int neighWidht = j - 1; neighWidht <= j + 1; ++neighWidht)
                        {
                            if (neighWidht < 0 || neighWidht > Cells.GetLength(1) || (neighHeight == i && neighWidht == j))
                            {
                                continue;
                            }

                            neighboards.Add(Cells[neighHeight, neighWidht]);
                        }
                    }

                    Cells[i, j].SetNeighboads(neighboards);
                }
            }
        }
    }
}
