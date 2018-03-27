using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework1
{
    public class Vector
    {
        public List<double> Value { get; set; }

        public Vector()
        {
            Value = new List<double>();
        }

        public Vector(List<double> value)
        {
            Value = new List<double>(value);
        }

        //vectorise input base on features and their idf in baseFile
        public Vector Vectorise(string input, string baseFile)
        {
            var featuresIdf = Bow_tfidf.GetFeaturesIdf(baseFile);
            if (string.IsNullOrEmpty(input))
                return null;
            HashSet<string> stopWords = new HashSet<string>(FileIO.ReadFile(ConfigurationManager.AppSettings.Get("StopWordFile")));
            var standardInput = Bow_tfidf.StandardizeString(input, stopWords);
            if (string.IsNullOrEmpty(standardInput))
                return null;    //invalid input
            var result = new Vector();
            if (featuresIdf.Count > 0)
            {
                var features = featuresIdf.Keys.ToArray();
                var document = new Document(standardInput);
                foreach (var feature in features)
                {
                    if (document.Contains(feature))
                    {
                        var tf_idf = 1.0 * document.getFrequency(feature) / document.getMaxFrequency() * featuresIdf[feature];
                        result.Value.Add(tf_idf);
                    }
                    else
                        result.Value.Add(0);
                }
            }
            return result;
        }


        public double GetSimilarityMeasure(Vector vector)
        {
            if (Value.Count != vector.Value.Count)
                return 0;
            double squareSum = 0;
            for (int i=0; i<Value.Count; i++)
            {
                squareSum += Math.Pow((Value[i] - vector.Value[i]), 2);
            }
            return Math.Sqrt(squareSum);
        }

        public Dictionary<int, double> GetListSimilarityMeasure (List<Vector> listV, int quantity)
        {
            if(quantity <= 0)
            {
                return null;
            }
            else
            {
                if (listV.Count < quantity)
                {
                    quantity = listV.Count;
                }
                Dictionary<int, double> rs = new Dictionary<int, double>(listV.Count);
                for (int i = 0; i < listV.Count(); i++)
                {
                    rs.Add(i, GetSimilarityMeasure(listV[i]));
                }
                rs = rs.OrderBy(key => key.Value).Take(quantity).ToDictionary(x => x.Key, x => x.Value);
                return rs;
            }
        }

        //1412542
        public Dictionary<string, double> Search(string inputFile)
        {
            Dictionary<string, double> similarDocs = new Dictionary<string, double>();
            try
            {
                // Read string and the number of documents that we need to search
                List<string> inputs = FileIO.ReadFile(inputFile);
                string searchString = inputs[0];
                int numberOfDocs = int.Parse(inputs[1]);

                // Read tf_idf
                List<string> tf_idfList = FileIO.ReadFile(ConfigurationManager.AppSettings.Get("BowTfIdfFile"));
                List<Vector> vectorList = new List<Vector>(tf_idfList.Count);
                foreach(string doc in tf_idfList)
                {
                    string[] tfIdfDoc = doc.Trim().Split(' ');
                    List<double> tf_idfs = new List<double>(tfIdfDoc.Count());
                    foreach (string tfIdf in tfIdfDoc)
                    {
                        tf_idfs.Add(double.Parse(tfIdf));
                    }
                    vectorList.Add(new Vector(tf_idfs));
                }

                // Vectorise
                Vector searchVector = Vectorise(searchString, ConfigurationManager.AppSettings.Get("FeatureFile"));

                // Get documents that are similar to searchString
                Dictionary<int, double> similarDocIndex = GetListSimilarityMeasure(vectorList, numberOfDocs);

                using (StreamReader sr = new StreamReader(ConfigurationManager.AppSettings.Get("RawFile")))
                {
                    String line;
                    int index = 0;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (similarDocIndex.ContainsKey(index))
                        {
                            similarDocs.Add(line, similarDocIndex[index]);
                            similarDocIndex.Remove(index);

                            // Check if we got enough documents
                            if (similarDocIndex.Count == 0)
                                break;
                        }
                        ++index;
                    }
                }

                // Order documents by similarity
                similarDocs = similarDocs.OrderBy(key => key.Value).ToDictionary(x => x.Key, x => x.Value);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return similarDocs;
        }
    }
}
