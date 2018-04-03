﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework1
{
    public class Program
    {
        public static void Main(string[] args)
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
            var vectors = new List<Vector>();
            bool hasValueType = true;
            List<string> input = FileIO.ReadFileIntoVector(inputFile, out vectors, hasValueType);
            var output = StringHelper.ReproduceText(vectors, stopWordFile);
            FileIO.WriteFile(output, processedTextFile);

            // Extract features
            Bow_tfidf.CreateFeatureList(processedTextFile, featureFile);

            // Calculate tf_idf
            Bow_tfidf.BoW_tfidf(processedTextFile, outputFile, featureFile, roundFile);
            //var result = Bow_tfidf.tf_idf(vectors, featureFile);
            Bow_tfidf.tf_idf(ref vectors, featureFile);

/*
            //  Search similar documents
            var similarDocs = Vector.Search(searchFile);

            // Wrire similar documents to file
            using (StreamWriter wr = new StreamWriter(similarDocFile))
            {
                foreach (var doc in similarDocs)
                {
                    wr.WriteLine(doc.Key + ' ' + doc.Value);
                }
            }*/
        }
    }
}
