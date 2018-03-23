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
            String inputFile = "../../raw_text.txt";
            string processedTextFile = "../../processedFile.txt";
            string featureFile = "../../featureList.txt";
            String roundFile = "../../round.txt";
            String outputFile = "../../output.txt";
            String stopWordFile = "../../stop-words.txt";
            List<String> list = new List<string>();

            // FileIO file = new FileIO();

            // Pre-processing text
            Bow_tfidf.ReproduceText(inputFile, processedTextFile, stopWordFile);
            
            // Extract features
            Bow_tfidf.CreateFeatureList(processedTextFile, featureFile);

            // Calculate tf_idf
            Bow_tfidf.BoW_tfidf(processedTextFile, outputFile, featureFile, roundFile);        

        }
    }
}
