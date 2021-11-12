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
        private List<Tuple<int, int>> largestPathsLocations;

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
            largestPathsLocations = new List<Tuple<int, int>>();
            ComputeMaximumPaths();
        }

        private void ComputeMaximumPaths()
        {
            // create state matrix

            int x = Elevations.GetLength(0);
            int y = Elevations.GetLength(1);

            int maximumLength = 0;

            // visit each box - retrieve state matrix

            for(int i = 0; i < x; i++)
            {
                for(int j = 0; j < y; j++)
                {
                    int currentLength = ComputeLongestPaths(State, x, y, i, j);
                    if (currentLength > maximumLength)
                    {
                        largestPathsLocations.Clear();
                        largestPathsLocations.Add(new Tuple<int, int>(i, j));
                        maximumLength = currentLength;
                    }
                    else if (currentLength == maximumLength) largestPathsLocations.Add(new Tuple<int, int>(i, j));
                }
            }

            // find steepest path

            maximumPath = GetSteepestPath();
        }

        private int[] GetSteepestPath()
        {
            // list largest paths
            ListLargestPaths();
            
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

        private void ListLargestPaths()
        {
            foreach (Tuple<int, int> coord in largestPathsLocations)
            {
                List<int> path = new List<int>();

                int x = coord.Item1, y = coord.Item2;
                int maxState = State[x, y];

                for (int currentState = maxState; currentState >= 1; currentState--)
                {
                    foreach (int[] direction in directions)
                    {
                        if(!path.Contains(Elevations[x, y]))
                            path.Add(Elevations[x, y]);

                        int u = x + direction[0];
                        int v = y + direction[1];

                        if (u < 0 || v < 0 || u > State.GetLength(0) - 1 || v > State.GetLength(1) - 1) 
                            continue;
                        else if (currentState - State[u, v] == 1)
                        {
                            x = u;
                            y = v;
                            break;
                        }

                    }
                }
                largestPaths.Add(path);
            }
            
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
                        if (currentMaximum > maximum) 
                            maximum = currentMaximum;
                    }

                }
                state[i, j] = maximum + 1;
                return state[i, j];
            }
        }

        public void PrintMaximumPath()
        {
            if (MaximumPathLength > 0)
            {
                foreach (int elevation in MaximumPath)
                    Console.Write("{0}-", elevation);

                Console.Write("\b ");
                Console.WriteLine();
            }
            else
                Console.WriteLine("empty");
        }

        public void PrintState()
        {
            Console.WriteLine("State matrix:");
            for (int i = 0; i < State.GetLength(0); i++)
            {
                for (int j = 0; j < State.GetLength(1); j++)
                {
                    Console.Write("{0} ", State[i, j]);
                }
                Console.WriteLine("");
            }
        }

    }
}
