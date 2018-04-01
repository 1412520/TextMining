﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace _1412503_test
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

        public static HashSet<string> ReadFileIntoHashTable(string fileInput)
        {
            HashSet<string> list = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
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


        //1412543
        public static void WriteFile(List<String> list, string fileOutput)
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
        public static void WriteFile(double[,] matrix, string fileOutput)
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

        //1412543
        //Ghi từng chuỗi tiếp tục vào file
        public static void WriteLine(String s, string fileOutput)
        {
            using (StreamWriter wr = new StreamWriter(fileOutput, true))
            {
                wr.WriteLine(s);
            }
        }

    }
}
