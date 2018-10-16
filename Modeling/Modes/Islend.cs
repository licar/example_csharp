using Modeling.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Modeling.Modes
{
	public class Islend
	{
		public Cell[,] Cells { get; }
        
		public Islend(int height, int widht)
		{
            Cells = new Cell[height, widht];
            GenerateIslend(height, widht);
            SetNeighbods(height, widht);
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
            var cells = new Cell[Cells.GetLength(0), Cells.GetLength(1)];
            for (int i = 0; i != Cells.GetLength(0); ++i)
            {
                for (int j = 0; j != Cells.GetLength(1); ++j)
                {
                    cells[i, j] = GenerateCell();
                }
            }
        }

        private Cell GenerateCell()
        {
            var max = Enum.GetValues(typeof(Locality)).Cast<int>().Max();
            var min = Enum.GetValues(typeof(Locality)).Cast<int>().Min();

            Random random = new Random();
            var locality = (Locality) random.Next(min, max);
            
            if (locality == Locality.Field)
            {
                return new Field();
            }

            return new Cell(locality);
        }

        private void SetNeighbods()
        {
            for (int i = 0; i != Cells.GetLength(0); ++i)
            {
                for (int j = 0; j != Cells.GetLength(1); ++j)
                {
                    var neighboards = new List<Cell>();
                    
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

                    Cells[i, j].neighboads = neighboards;
                }
            }
        }
    }
}
