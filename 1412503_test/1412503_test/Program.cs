using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1412503_test
{
    class Program
    {
        static void Main(string[] args)
        {
            FileUtils.preprocessDirectory("../../inputFileTest", "../../outputFile/ouputFileTotal.txt");
            Bow_tfidf.CreateFeatureList("../../outputFile/ouputFileTotal.txt", "../../inputFile/stop-words.txt", "../../outputFile/featureList.txt");
            //Bow_tfidf.BoW_tfidf("../../outputFile/ouputFileTotal.txt", "../../outputFile/Bow_tfidf.txt", "../../outputFile/featureList.txt", "../../inputFile/round.txt", "../../inputFile/stop-words.txt");

            List<Vector> test = Vector.VectoriseStandardInputFile("../../outputFile/ouputFileTotal.txt", "../../outputFile/featureList.txt", "../../inputFile/stop-words.txt", true);
            
            //foreach (var item in test)
            //{
            //     Console.WriteLine(item.TextValue.Text);
            //    foreach (var item1 in item.Value)
            //    {
            //        Console.WriteLine(item1);
            //    }
            //}
            //Console.ReadLine();
        }
    }
}
