using System;
using System.IO;
using System.Configuration;
using System.Linq;

namespace SkiResort
{
    class Program
    {
        static void Main(string[] args)
        {
            // creating elevations 2D-array
            int[,] elevations = ReadFile();
            // reading external file
            if (elevations != null)
            {
                // creating Field object
                Field field = new Field(elevations);

                // retrieving MaximumPathLength
                Console.WriteLine("MaximumPathLength is {0}", field.MaximumPathLength);
                // retrieving MaximumDrop
                Console.WriteLine("MaximumDrop is {0}", field.MaximumDrop);
                // retrieving MaximumPath
                Console.Write("MaximumPath is ");
                field.PrintMaximumPath();
            }
            else
            {
                Console.WriteLine("No valid elevations field found.");
            }

            Console.ReadKey();
        }

        private static int[,] ReadFile()
        {
            string workingDirectory = Environment.CurrentDirectory;
            string projectPath = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
            string fileName;
            string path;

            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                fileName = appSettings["FileName"] ?? string.Empty;

            }
            catch (Exception)
            {
                Console.WriteLine("Error reading App.config file.");
                return null;
            }

            if (fileName != string.Empty)
            {
                path = String.Format(@"{0}\input\{1}", projectPath, fileName);
            }
            else
            {
                Console.WriteLine("Not valid file name found in App.config. Please check the FileName key.");
                return null;
            }  

            // parsing input file

            string[] lines = File.ReadAllLines(path);
            int i = 0, j = 0;

            int[,] elevations = new int[
                int.Parse(lines[0].Split(' ')[0]),
                int.Parse(lines[0].Split(' ')[0])
                ];

            lines = lines.Where((item, index) => index != 0).ToArray();

            foreach (string line in lines)
            {
                j = 0;
                foreach (var elevation in line.Split(' '))
                {
                    elevations[i, j] = int.Parse(elevation);
                    j++;
                }
                i++;
            }

            return elevations;
        }
    }
}
