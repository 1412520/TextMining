﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework1
{
    public class Bow_tfidf
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
                        if (idf != 0) featureList.Add(item1, idf);
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
            List<string> docList;
            Dictionary<string, double> wordList = new Dictionary<string, double>();
            int round;
            try
            {
                docList = FileIO.ReadFile(input);
                round = Int16.Parse(FileIO.ReadFile(roundFile)[0]);
                wordList = GetFeaturesIdf(featureFile);
                if (File.Exists(output))
                {
                    File.Delete(output);
                }

                
                using (StreamWriter wr = new StreamWriter(output, true))
                {
                    for (int i = 0; i < docList.Count; i++)
                    {
                        var vector = Vector.VectoriseInput(docList[i], wordList);
                        for (int j = 0; j < vector.Count; j++)
                        {
                            wr.Write(vector[j]);
                            wr.Write(' ');
                        }
                        wr.WriteLine();
                    }

                }
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
                if (!String.IsNullOrEmpty(standardRow))
                {
                    int i = standardRow.LastIndexOf(' ');
                    var idf = standardRow.Substring(i + 1);
                    var words = standardRow.Substring(0, i);
                    featuresIdf.Add(words, double.Parse(idf));
                }
            }
            return featuresIdf;
        }

        public static List<Vector> tf_idf(List<Vector> vectors, string featureFile)
        {
            var result = new List<Vector>();
            foreach (var vector in vectors)
            {
                var item = new Vector(vector);
                item.Value = Vector.VectoriseStandardInput(vector.TextValue.Text, featureFile);
                result.Add(item);
            }
            return result;
        }

        public static void GenerateTFIDFMatrix(string inputFile, string processedFile, string featureFile, string outputFile)
        {
            // Pre-processing text
            var vectors = new List<Vector>();
            bool hasValueType = true;
            List<string> input = FileIO.ReadFileIntoVector(inputFile, out vectors, hasValueType);
            var output = StringHelper.ReproduceText(vectors, ConfigurationManager.AppSettings.Get("StopWordFile"));
            FileIO.WriteFile(output, processedFile);

            // Extract features
            Bow_tfidf.CreateFeatureList(processedFile, featureFile);

            // Calculate tf_idf
            //var watch = System.Diagnostics.Stopwatch.StartNew();
            Bow_tfidf.BoW_tfidf(processedFile, outputFile, featureFile, ConfigurationManager.AppSettings.Get("RoundFile"));
            //watch.Stop();
            //var elapsedMs = watch.ElapsedMilliseconds;
            //Console.WriteLine(elapsedMs);
        }
    }
}
