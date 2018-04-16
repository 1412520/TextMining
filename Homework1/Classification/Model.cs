using System;
using System.Collections.Generic;
using System.Linq;
using Homework1;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Classification
{
    public class Model
    {
        static string GetLabelOfDocument(List<int> docIndexes, string rawFile)
        {
            Dictionary<string, int> labelFreq = new Dictionary<string, int>();
            using (StreamReader sr = new StreamReader(rawFile))
            {
                string line;

                int index = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    if (docIndexes.Contains(index))
                    {
                        // Get label of similar document
                        string label = StringHelper.getLabel(line);
                        if (labelFreq.ContainsKey(label))
                            ++labelFreq[label];
                        else
                            labelFreq.Add(label, 1);

                        docIndexes.Remove(index);
                        if (docIndexes.Count == 0) break;
                    }
                    ++index;

                }
            }

            string labelOfSearchString = labelFreq.FirstOrDefault(x => x.Value == labelFreq.Values.Max()).Key;
            return labelOfSearchString;
        }

        public static void classify()
        {
            string inputFile = "../../test/test.txt";
            string tfidfFile = "../../training/tf_idf.txt";
            string featureFile = "../../training/features.txt";
            string rawFile = "../../training/raw_text.txt";
            string classifiedFile = "../../test/classified-file.txt";

            //FileUtils.preprocessDirectory("../../input", "../../training/raw_text.txt");
            //Bow_tfidf.GenerateTFIDFMatrix("../../training/raw_text.txt", "../../training/processed.txt", "../../training/features.txt", "../../training/tf_idf.txt");


            // Read string and the number of documents that we need to search
            List<string> inputs = FileIO.ReadFile(inputFile);
            int numberOfDocs = int.Parse(inputs[inputs.Count - 2]);
            string similarityName = inputs[inputs.Count - 1];

            //Remove k and similarity name from search string list
            inputs.RemoveAt(inputs.Count - 1);
            inputs.RemoveAt(inputs.Count - 1);

            //Get Tf_idf from file
            List<string> tfidfList = FileIO.ReadFile(tfidfFile);
            List<Vector> tfidfVector = new List<Vector>(tfidfList.Count);
            //int i = 0; 
            //foreach (string tf_idf in tfidfList)
            //{
               //tfidfVector.Add(new Vector(tf_idf));
                //i++;
            //}

            List<string> labeledString = new List<string>();
            string label;
            int count = 0;
            foreach (string searchString in inputs)
            {
                //Vectorise, calculate tf_idf
                Vector searchVector = Vector.Vectorise(searchString, featureFile);

                //Get label of documents that are similar to searchString
                Console.WriteLine(count);
                Dictionary<int, double> similarDocIndex = searchVector.GetListSimilarityMeasure(tfidfList, numberOfDocs, similarityName);
                label = GetLabelOfDocument(similarDocIndex.Keys.ToList(), rawFile);

                labeledString.Add(label + " - " + searchString);
                count++;
            }

            FileIO.WriteFile(labeledString, classifiedFile);


            //Bow_tfidf.GenerateTFIDFMatrix("../../test/test.txt", "../../test/processed.txt", "../../training/features.txt", "../../test/tf_idf.txt");

        }

        public static void classifyForValidate()
        {
            //Read file to get k and similarity name
            string kFile = "../../k.txt";

            string trainFile = "../../validation/train.txt";
            string testFile = "../../validation/test.txt";
            string tfidfFile = "../../validation/tf_idf.txt";
            string featureFile = "../../validation/features.txt";
            string classifiedFile = "../../validation/testResult.txt";

            //Get k and similarity name
            List<string> inputs = FileIO.ReadFile(kFile);
            int numberOfDocs = int.Parse(inputs[0]);
            string similarityName = inputs[1];

            //Get Tf_idf from file
            List<string> tfidfList = FileIO.ReadFile(tfidfFile);
            List<Vector> tfidfVector = new List<Vector>(tfidfList.Count);

            List<string> labeledString = new List<string>();
            string label;
            int count = 0;

            List<string> testInput = FileIO.ReadFile(testFile);
            for (int i = 0; i < testInput.Count; i++)
            {
                var searchString = testInput[i];
                //Vectorise, calculate tf_idf
                Vector searchVector = Vector.Vectorise(searchString, featureFile);

                //Get label of documents that are similar to searchString
                Dictionary<int, double> similarDocIndex = searchVector.GetListSimilarityMeasure(tfidfList, numberOfDocs, similarityName);
                var watch = System.Diagnostics.Stopwatch.StartNew();
                label = GetLabelOfDocument(similarDocIndex.Keys.ToList(), trainFile);
                watch.Stop();
                Console.WriteLine("Get label of document {0} takes: {1} ", i, watch.Elapsed);
                labeledString.Add(label + " - " + searchString);
                count++;
            }

            FileIO.WriteFile(labeledString, classifiedFile);

        }
    }
}
