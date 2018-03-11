using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Homework1
{
    class FileIO
    {
        //1412543
        public static List<string> ReadFile(string fileInput)
        {
            List<String> list = new List<string>();
            try
            {
                using (StreamReader sr = new StreamReader(fileInput))
                {
                    String line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line != "")
                            list.Add(line);
                    }
                } 
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

            return list;
        }

        //1412543
        public static int ReadFileWithOneNumber (string fileInput)
        {
            int round = 0;

            try
            {
                using (StreamReader sr = new StreamReader(fileInput))
                {
                    round = Int16.Parse(sr.ReadLine());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

            return round;
        }

        //1412543
        public static void WriteListToFile(List<String> list, string fileOutput)
        {
            using (StreamWriter wr = new StreamWriter(fileOutput))
            {
                foreach (string i in list)
                {
                    wr.WriteLine(i);
                }
            }
        }

        //1412543
        public static void WriteMatrixToFile (double[,] matrix, string fileOutput)
        {
            using (StreamWriter wr = new StreamWriter(fileOutput)) 
            {
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    for (int j = 0; j < matrix.GetLength(1); j++)
                        wr.Write(matrix[i, j] + " ");
                    wr.WriteLine();
                }  
            }
        }

        //1412520
        public static HashSet<string> ReadFileIntoHashTable(string fileInput)
        {
            HashSet<string> list = new HashSet<string>();
            try
            {
                using (StreamReader sr = new StreamReader(fileInput))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        list.Add(line);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

            return list;
        }
    }
}
