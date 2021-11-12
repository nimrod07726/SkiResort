using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkiResort
{
    class Field
    { 
        public int[,] Elevations { get; set; }
        public int[,] Cache { get; set; }
        public int[] MaximumPath { get; set; }
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

        public Field()
        {
            MaximumPath = new int[] { };
        }

        public Field(int[,] elevations)
        {
            Elevations = elevations;
            ComputeMaximumPath();
        }

        private void ComputeMaximumPath()
        {
            MaximumPath = new int[] { };
        }

        

    }
}
