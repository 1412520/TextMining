using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework1
{
    class Program
    {
        static void Main(string[] args)
        {
            String fileInput = "../../input.txt";
            String fileRound = "../../round.txt";
            String fileOutput = "../../output.txt";
            List<String> list = new List<string>();

            FileIO file = new FileIO();

            list = file.ReadFile(fileInput);
            file.WriteFile(list, fileOutput);
            int round =  file.ReadFileWithOneNumber(fileRound);
            Console.WriteLine("Round: " + round);
        }
    }
}
