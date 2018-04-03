using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Homework1;
using System.Configuration;

namespace _1412503_test
{
    class FileUtils
    {
        public static void preprocessDirectory(string path, string outputFile)
        {
            List<string> result = new List<string>();
            if (Directory.Exists(path))
            {
                List<string> listFile = new List<string>(Directory.GetFiles(path, "*.txt"));
                foreach (string file in listFile)
                {
                    processFile(file, result);
                }
            }
            FileIO.WriteFile(result, outputFile);
        }

        private static void processFile(string file, List<string> result)
        {
            List<string> content = FileIO.ReadFile(file);
            string name = Path.GetFileNameWithoutExtension(file);
            foreach (string str in content)
            {
                result.Add(name + " - " + str);
            }
        }
    }
        
}
