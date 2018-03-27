using System;
using System.Collections.Generic;
using System.IO;
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
            String searchFile = "../../search.txt";
            String similarDocFile = "../../similarDocuments.txt";
            List<String> list = new List<string>();

            // FileIO file = new FileIO();

            // Pre-processing text
            List<string> input = FileIO.ReadFile(inputFile);
            var output = Bow_tfidf.ReproduceText(input, stopWordFile);
            FileIO.WriteListToFile(output, processedTextFile);

            // Extract features
            Bow_tfidf.CreateFeatureList(processedTextFile, featureFile);

            // Calculate tf_idf
            Bow_tfidf.BoW_tfidf(processedTextFile, outputFile, featureFile, roundFile);

            //  Search similar documents
            Vector str = new Vector();
            Dictionary<string, double> similarDocs = str.Search(searchFile);

            // Wrire similar documents to file
            using (StreamWriter wr = new StreamWriter(similarDocFile))
            {
                foreach (var doc in similarDocs)
                {
                    wr.WriteLine(doc.Key + ' ' + doc.Value);
                }
            }
        }
    }
}
