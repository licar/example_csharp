using Modeling.Common.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Modeling.Modes
{
	[Serializable]
	public class Island 
	{
		public ICell[,] Cells { get; }
        private Random random = new Random();

		public Island()
		{

		}

		public Island(int height, int widht, bool randomWorld)
		{
            Cells = new ICell[height, widht];
            GenerateIslend(randomWorld);
            FillNeighbods();
        }

	    public void UpdateIsland()
	    {
            NextBeat();
            Refresh();
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

	    public void Refresh()
	    {
	        for (int i = 0; i != Cells.GetLength(0); ++i)
	        {
	            for (int j = 0; j != Cells.GetLength(1); ++j)
	            {
	                Cells[i, j].Refresh();
	            }
	        }
        }


        private void GenerateIslend(bool randomWorld)
        {
            for (int i = 0; i != Cells.GetLength(0); ++i)
            {
                for (int j = 0; j != Cells.GetLength(1); ++j)
                {
                    Cells[i, j] = GenerateCell(randomWorld) as ICell;
                }
            }
        }

        private FieldCell GenerateCell(bool randomWorld)
        {
            var max = Enum.GetValues(typeof(Locality)).Cast<int>().Max();
            var min = Enum.GetValues(typeof(Locality)).Cast<int>().Min();

            Thread.Sleep(1);
            int result =  random.Next(min, 8);

            var locality = result > 1 ? Locality.Field : (Locality) result;

            if (locality == Locality.Field)
            {
                var wold = new WolfsField(randomWorld);
                return wold;
            }

            return new FieldCell(locality);
        }

        private void FillNeighbods()
        {
            for (int i = 0; i != Cells.GetLength(0); ++i)
            {
                for (int j = 0; j != Cells.GetLength(1); ++j)
                {
                    var neighboards = new List<ICell>();
                    
                    for (int neighHeight = i - 1; neighHeight <= i + 1; ++neighHeight)
                    {
                        if (neighHeight < 0 || neighHeight > Cells.GetLength(0) - 1)
                        {
                            continue;
                        }
                        for (int neighWidht = j - 1; neighWidht <= j + 1; ++neighWidht)
                        {
                           

                            if (neighWidht < 0 || neighWidht > Cells.GetLength(1) - 1 || (neighHeight == i && neighWidht == j))
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

		public  Island Clone()
		{
			using (var ms = new MemoryStream())
			{
				var formatter = new BinaryFormatter();
				formatter.Serialize(ms, this);
				ms.Position = 0;

				return (Island)formatter.Deserialize(ms);
			}
		}

	}
}
