using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkiResort
{
    class Field
    {
        private int[][] directions = 
        { 
            new int[]{ 1, 0 },
            new int[]{ -1, 0 },
            new int[]{ 0, 1 },
            new int[]{ 0, -1 } 
        };

        private int[] maximumPath;
        private List<List<int>> largestPaths;

        public int[,] State { get; }
        public int[,] Elevations { get; set; }
        public int[] MaximumPath {
            get 
            {
                return maximumPath;
            } 
        }
        public int MaximumDrop {
            get
            {
                if(MaximumPathLength > 0)
                {
                    return MaximumPath.Max() - MaximumPath.Min();
                }
                else
                {
                    return 0;
                }
            }
        }
        public int MaximumPathLength {
            get
            {
                return MaximumPath.Length;
            }
        }

        public void PrintMaximumPath()
        {
            if(MaximumPathLength > 0)
            {
                foreach (int elevation in MaximumPath)
                {
                    Console.Write("{0} ", elevation);
                    Console.WriteLine();
                }
            }
            else
                Console.WriteLine("empty");
        }

        public void PrintState()
        {
            for(int i = 0; i < State.GetLength(0); i++)
            {
                for(int j = 0; j < State.GetLength(1); j++)
                {
                    Console.Write("{0} ", State[i, j]);
                }
                Console.WriteLine("");
            }
        }

        public Field()
        {
            maximumPath = new int[] { };
        }

        public Field(int[,] elevations)
        {
            Elevations = elevations;
            State = new int[
                Elevations.GetLength(0),
                Elevations.GetLength(1)];
            largestPaths = new List<List<int>>();
            ComputeMaximumPaths();
        }

        private void ComputeMaximumPaths()
        {
            // create state matrix

            int x = Elevations.GetLength(0);
            int y = Elevations.GetLength(1);

            //int[,] state = new int[x, y];

            int maximumLength = 0;

            // visit each box - retrieve state matrix

            for(int i = 0; i < x; i++)
            {
                for(int j = 0; j < y; j++)
                {
                    int currentLength = ComputeLongestPaths(State, x, y, i, j);
                    maximumLength = Math.Max(maximumLength, currentLength);
                }
            }

            // find steepest path

            maximumPath = GetSteepestPath(maximumLength);
        }

        private int[] GetSteepestPath(int maximumLength)
        {
            // list largest paths
            ListLargestPaths(maximumLength);
            
            // find steepest path from paths list
            int maximumSlope = 0;
            int maximumIndex = 0;
            int i, maxBox, minBox;

            for(i = 0; i < largestPaths.Count; i++)
            {
                maxBox = largestPaths[i].ElementAt(0);
                minBox = largestPaths[i].ElementAt(largestPaths[i].Count - 1);

                if (maxBox - minBox > maximumSlope)
                {
                    maximumSlope = maxBox - minBox;
                    maximumIndex = i;
                }                
            }
            
            return largestPaths[maximumIndex].ToArray();
        }

        private void ListLargestPaths(int maximumLength)
        {
            //TODO: list path locations
            for(int i = 0; i < State.GetLength(0); i++)
            {
                for (int j = 0; j < State.GetLength(1); j++)
                {

                }
            }
            
            //TODO: track and save paths

        }

        private int ComputeLongestPaths(int[,] state, int x, int y, int i, int j)
        {
            if (state[i, j] > 0) return state[i, j];
            else
            {
                int maximum = 0;
                foreach (int[] direction in directions)
                {
                    int u = direction[0] + i;
                    int v = direction[1] + j;

                    if(u > -1 && v > -1 && u < x && v < y && Elevations[u, v] < Elevations[i, j])
                    {
                        int currentMaximum = ComputeLongestPaths(state, x, y, u, v);
                        maximum = Math.Max(maximum, currentMaximum);
                    }

                }
                state[i, j] = maximum + 1;
                return state[i, j];
            }
        }

    }
}
