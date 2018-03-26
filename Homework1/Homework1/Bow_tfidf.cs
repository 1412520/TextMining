﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework1
{
    class Bow_tfidf
    {
        //1412595
        public static void CreateFeatureList(string inputFile, string outputFile)
        {
            List<string> processText = FileIO.ReadFile(inputFile);
            Dictionary<string, double> featureList = new Dictionary<string, double>();
            int totalDocs = processText.Count;
            List<Document> processDocs = new List<Document>();
            foreach (string doc in processText)
            {
                processDocs.Add(new Document(doc));
            }
            foreach (string item0 in processText)
            {
                string[] arrListStr = item0.Split(' ');
                foreach (string item1 in arrListStr)
                {
                    bool isExists = featureList.ContainsKey(item1);
                    if (isExists == false)
                    {
                        int quantity = DocContainsFeature(item1, processDocs); 
                        double idf = Math.Log10(1.0 * totalDocs / quantity);
                        featureList.Add(item1, idf);
                    }
                }
            }
            List<string> featureDocs = new List<string>();
            foreach (var item2 in featureList)
            {
                featureDocs.Add(item2.Key + ' ' + item2.Value);
            }
            FileIO.WriteFile(featureDocs, outputFile);
        }

        //1412520
        private static HashSet<char> marks = new HashSet<char> { ',', '.', '!', '/', '-', '=', '~', '?', ':', ';', '\'', '"', '(', ')', '[', ']', '{', '}' };

        //1412520
        public static List<string> ReproduceText(List<string> input, string stopWordFile)
        {

            

            HashSet<string> stopWords = new HashSet<string>(FileIO.ReadFile(stopWordFile));
            List<string> output = new List<string>();
            foreach (var row in input)
            {
                if (string.IsNullOrEmpty(row))
                {
                    output.Add(row);
                    continue;
                }

                var outputRow = StandardizeString(row, stopWords);
                output.Add(outputRow);
            }

            return output;
        }

        public static string StandardizeString(string input, HashSet<string> stopWords)
        {
            var output = input.ToLower();
            output = RemoveMarksExtraSpaces(output);
            output = RemoveWordsFromString(output, stopWords);
            output = Stemming(output);
            return output;
        }


        // 1412542
        // Stemming words in the document by poster stemming alogrithm
        private static string Stemming(string doc)
        {
            var result = new StringBuilder();
            var words = doc.Split(' '); //tach ra tung word vi su dung stringbuilder hieu qua hon, xu ly truc tiep tren string chu khong tao mot ban copy
            string stemWord;
            foreach (var word in words)
            {
                stemWord = PosterStemming.stem(word);
                result = result.Append(stemWord + " ");
            }
           return result.ToString().TrimEnd();
        }

        //1412520
        public static string RemoveMarksExtraSpaces(string input)
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
        public static double CalculateTfidf(string feature, double idf, Document doc)
        {
            if (!doc.Contains(feature))
                return 0;

            double tf_idf = 0;
            try 
            {
                int freq, maxFreq;
                freq = doc.getFrequency(feature);
                maxFreq = doc.getMaxFrequency();
                tf_idf = (1.0 * freq / maxFreq) * idf;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return tf_idf;
        }

        //1412542
        public static void BoW_tfidf(string input, string output, string featureFile, string roundFile)
        {
            //List<List<double>> weight = new List<List<double>>();
            double[,] weight = null;
            List<string> docList;
            Dictionary<string, double> wordList = new Dictionary<string, double>();
            int round;
            try
            {
                docList = FileIO.ReadFile(input);
                round = Int16.Parse(FileIO.ReadFile(roundFile)[0]);
                wordList = GetFeaturesIdf(featureFile);
                
                List<Document> docs = new List<Document>();
                foreach (string doc in docList)
                {
                    docs.Add(new Document(doc));
                }

                int totalDocs, numFeatures;
                totalDocs = docList.Count;
                numFeatures = wordList.Count;

                weight = new double[totalDocs, numFeatures];
                double tfidf;
                for (int i = 0; i < totalDocs; i++)
                {
                    for (int j = 0; j < numFeatures; j++)
                    {
                        //numDocsContainFeature = DocContainsFeature(wordList[j], docs);
                        tfidf = CalculateTfidf(wordList.ElementAt(j).Key, wordList.ElementAt(j).Value, docs[i]);
                        weight[i, j] = Math.Round(tfidf, round, MidpointRounding.AwayFromZero);
                    }
                }

                FileIO.WriteFile(weight, output);
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //get features and their idf values from file
        public static Dictionary<string, double> GetFeaturesIdf(string featureFile)
        {
            var rows = FileIO.ReadFile(featureFile);
            var featuresIdf = new Dictionary<string, double>();
            foreach (var row in rows)
            {
                var standardRow = Bow_tfidf.RemoveMarksExtraSpaces(row);
                var words = standardRow.Split(' ');
                if (words.Length > 1)
                    featuresIdf.Add(words[0], double.Parse(words[1]));
            }
            return featuresIdf;
        }
    }
}
