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
            String fileOutputMatrix = "../../ntt.txt";
            List<String> list = new List<string>();

            FileIO file = new FileIO();

            list = file.ReadFile(fileInput);

            // 1412595
            Bow_tfidf bow_Tfidf = new Bow_tfidf();
            // list là văn bản đã tiền xử lý
            bow_Tfidf.FeatureList(list);

            file.WriteListToFile(list, fileOutput);
            int round =  file.ReadFileWithOneNumber(fileRound);
            Console.WriteLine("Round: " + round);

            float[,] IntArray = 
                    { 
                        {1, 2}, 
                        {3, 4}, 
                        {5, 6} 
                    };

            file.WriteMatrixToFile(IntArray, fileOutputMatrix);
        }
    }
}
