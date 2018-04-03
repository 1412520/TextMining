using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Homework1;
using System.IO;


namespace Classification
{
    class Program
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

        static void Main(string[] args)
        {
            string inputFile = "../../test.txt";
            string tfidfFile = "../../../Homework1/output.txt";
            string featureFile = "../../../Homework1/featureList.txt";
            string rawFile = "../../../Homework1/raw_text.txt";
            string classifiedFile = "../../classifiedFile";

            // Read string and the number of documents that we need to search
            List<string> inputs = FileIO.ReadFile(inputFile);
            int numberOfDocs = int.Parse(inputs[inputs.Count - 2]);
            string similarityName = inputs[inputs.Count - 1];

            // Remove k and similarity name from search string list
            inputs.RemoveAt(inputs.Count - 1);
            inputs.RemoveAt(inputs.Count - 1);

            // Get Tf_idf from file
            List<string> tfidfList = FileIO.ReadFile(tfidfFile);
            List<Vector> tfidfVector = new List<Vector>(tfidfList.Count);
            foreach(string tf_idf in tfidfList)
            {
                tfidfVector.Add( new Vector(tf_idf));
            }
            
            List<string> labeledString = new List<string>();
            string label;
            foreach (string searchString in inputs)
            {
                // Vectorise, calculate tf_idf
                Vector searchVector = Vector.Vectorise(searchString, featureFile);

                // Get label of documents that are similar to searchString
                Dictionary<int, double> similarDocIndex = searchVector.GetListSimilarityMeasure(tfidfVector, numberOfDocs, similarityName);
                label = GetLabelOfDocument(similarDocIndex.Keys.ToList(), rawFile);
                              
                labeledString.Add(searchString + " - " + label);
            }

            FileIO.WriteFile(labeledString, classifiedFile);
        }
    }
}
