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
                if (!String.IsNullOrEmpty(doc))
                    processDocs.Add(new Document(doc));
            }
            foreach (Document doc in processDocs)
            {
                //string[] arrListStr = item0.Split(' ');
                foreach (string item1 in doc.getTermFreq().Keys)
                {
                    bool isExists = featureList.ContainsKey(item1);
                    if (isExists == false)
                    {
                        int quantity = Document.DocContainsFeature(item1, processDocs); 
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
                var standardRow = StringHelper.RemoveMarksExtraSpaces(row, new HashSet<char> { '!', '/', '-', '=', '~', '?', ':', ';', '\'', '"', '(', ')', '[', ']', '{', '}' });
                var words = standardRow.Split(' ');
                if (words.Length > 1)
                    featuresIdf.Add(words[0], double.Parse(words[1]));
            }
            return featuresIdf;
        }
    }
}
