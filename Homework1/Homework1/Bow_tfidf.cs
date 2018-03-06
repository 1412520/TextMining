using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework1
{
    class Bow_tfidf
    {
        //1412595
        public static void FeatureList(string inputFile, string outputFile)
        {
            List<string> docs = FileIO.ReadFile(inputFile);
            List<string> featureList = new List<string>();
            foreach (string item0 in docs)
            {
                string[] arrListStr = item0.Split(' ');
                foreach (string item1 in arrListStr)
                {
                    bool isExists = featureList.Contains(item1);
                    if (isExists == false)
                    {
                        featureList.Add(item1);
                    }
                }
            }
            FileIO.WriteListToFile(featureList, outputFile);
        }

        //1412520
        private static HashSet<char> marks = new HashSet<char> { ',', '.', '!', '/', '-', '=', '~', '?', ':', ';', '\'', '"', '(', ')', '[', ']', '{', '}'};

        //1412520
        public static void ReproduceText(string inputFile, string outputFile, string stopWordFile)
        {
            //FileIO fileReader = new FileIO();
            List<string> input = FileIO.ReadFile(inputFile);
            HashSet<string> stopWords = FileIO.ReadFileIntoHashTable(stopWordFile);
            List<string> output = new List<string>();
            foreach (var row in input)
            { 
                var outputRow = row.ToLower();
                outputRow = RemoveMarksExtraSpaces(outputRow, marks);
                outputRow = RemoveWordsFromString(outputRow, stopWords);
                output.Add(outputRow);
            }
            FileIO.WriteListToFile(output, outputFile);
        }

        //1412520
        public static string RemoveMarksExtraSpaces(string input, HashSet<char> marks)
        {
            var result = new StringBuilder();
            var lastWasSpace = input[0] == 32;
            for (int i = 0; i < input.Length; i++)
            {
                var character = input[i];
                if (marks.Contains(character))
                {
                    if (!lastWasSpace)
                    {
                        result.Append(' ');
                        lastWasSpace = true;
                    }
                    continue;
                }
                if ((lastWasSpace == false) || (character != 32))
                {
                    result.Append(input[i]);
                }
                lastWasSpace = character == 32;
            }
            return result.ToString().Trim();
        }

        //1412520
        //xu ly xoa dau va khoang trang thua truoc, de dua cac chuoi ve mot chuan, cac word cach deu nhau mot khoang trang, loai bo word se de dang va chinh xac hon
        //neu khong xu ly dau cau va khoang trang, cac word k co mot chuan nhat dinh, loai bo word khong chinh xac
        public static string RemoveWordsFromString(string input, HashSet<string> words)
        {
            var result = new StringBuilder();
            var inputWords = input.Split(' '); //tach ra tung word vi su dung stringbuilder hieu qua hon, xu ly truc tiep tren string chu khong tao mot ban copy
            HashSet<string> appearedWords = new HashSet<string>();
            foreach (var word in inputWords)
            {
                if (words.Contains(word))
                {
                    if (!appearedWords.Contains(word))
                        appearedWords.Add(word);
                }
                else
                    result.Append(word + ' ');
            }

            return result.ToString().TrimEnd();
        }

        //1412542
        // Return the number of documents contain the feature
        public static int DocContainsFeature(string feature, List<Document> docList)
        {
            int quantity = 0;
            try
            {
                foreach (Document doc in docList)
                {
                    if (doc.Contains(feature))
                        quantity++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return quantity;
        }

        //1412542
        // Return tf_idf of a feature in a document
        public static double CalculateTfidf(string feature, Document doc, int totalDocs, int num_DocsContainFeature)
        {
            if (!doc.Contains(feature))
                return 0;

            double tf_idf = 0;
            try
            {
                int freq, maxFreq;
                freq = doc.getFrequency(feature);
                maxFreq = doc.getMaxFrequency();
                double log = Math.Log10(totalDocs / num_DocsContainFeature);
                double temp = (1.0 * freq / maxFreq);
                tf_idf = temp * log;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return tf_idf;
        }

        //1412542
        public static void BoW_tfidf(string input, string output, string featureList, string roundFile)
        {
            //List<List<double>> weight = new List<List<double>>();
            double[,] weight = null;
            List<string> docList, wordList;
            int round;
            try
            {
                docList = FileIO.ReadFile(input);
                wordList = FileIO.ReadFile(featureList);
                round = FileIO.ReadFileWithOneNumber(roundFile);

                List<Document> docs = new List<Document>();
                foreach (string doc in docList)
                {
                    docs.Add(new Document(doc));
                }

                int totalDocs, numDocsContainFeature, numFeatures;
                totalDocs = docList.Count;
                numFeatures = wordList.Count;

                weight = new double[totalDocs, numFeatures];
                double tfidf;
                for (int i = 0; i < totalDocs; i++)
                {
                    for (int j = 0; j < numFeatures; j++)
                    {
                        numDocsContainFeature = DocContainsFeature(wordList[j], docs);
                        tfidf = CalculateTfidf(wordList[j], docs[i], totalDocs, numDocsContainFeature);
                        weight[i, j] = Math.Round(tfidf, round, MidpointRounding.AwayFromZero);
                    }
                }

                FileIO.WriteMatrixToFile(weight, output);
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
