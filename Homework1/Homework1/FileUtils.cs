using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Homework1;
using System.Configuration;

namespace Homework1
{
    public class FileUtils
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

        // 1412542 
        // Date: 08/04/2018
        // Delete all files from a directory
        public static void deleteAllFiles(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
        }

        // 1412542 
        // Date: 08/04/2018
        // Get names of all files in a special directory
        public static List<string> getAllFileNames(string path)
        {
            if (Directory.Exists(path))
            {
                return new List<string>(Directory.GetFiles(path, "*.txt"));
            }
            else
                return null;
        }
    }

}
