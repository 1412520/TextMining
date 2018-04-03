using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1412503_test
{
    public class Vector
    {
        public List<double> Value { get; set; }
        public Document TextValue { get; set; }
        public string ValueType { get; set; }

        public Vector()
        {
            Value = new List<double>();
        }

        public Vector(List<double> value)
        {
            Value = new List<double>(value);
        }

        public Vector(string vector)
        {
            Value = new List<double>();
            var numbers = vector.TrimEnd().Split(' ');
            foreach (var number in numbers)
            {
                Value.Add(double.Parse(number));
            }

        }

        public static Vector VectoriseStandardInput(string standardInput, string baseFile)
        {
            var featuresIdf = Bow_tfidf.GetFeaturesIdf(baseFile);
            var result = new Vector();
            //var indexOfDash = input.IndexOf('-');
            //if (hasValueType)
            //{
            //    var input = string.Copy(standardInput);
            //    var type = indexOfDash != -1 ? input.Substring(0, indexOfDash) : null;
            //    result.ValueType = type;
            //}
            
            if (featuresIdf.Count > 0)
            {
                var features = featuresIdf.Keys.ToArray();
                var document = new Document(standardInput);
                result.TextValue = document;
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

        public static List<Vector> VectoriseMultiple(List<string> input)
        {

        }

        public static List<Vector> VectoriseStandardInputFile(string inputFile, string baseFile, string stopwordFile, bool hasValueType)
        {
            List<string> processText = FileUtils.processFileContent(inputFile, stopwordFile);
            List<Vector> result = new List<Vector>();
            foreach (string str in processText)
            {
                result.Add(VectoriseStandardInput(str, baseFile, hasValueType));
            }
            return result;
        }

        public double getEuclidSimilarity(Vector vector)
        {
            if (Value.Count != vector.Value.Count)
                return 0;
            double squareSum = 0;
            for (int i = 0; i < Value.Count; i++)
            {
                squareSum += Math.Pow((Value[i] - vector.Value[i]), 2);
            }
            return Math.Sqrt(squareSum);
        }

        public double getCosinSimilarity(Vector vector)
        {
            if (Value.Count != vector.Value.Count)
                return 0;
            double dot = 0, mag1 = 0, mag2 = 0;

            for (int i = 0; i < Value.Count; i++)
            {
                dot += Value[i] * vector.Value[i];
                mag1 += Math.Pow(Value[i], 2);
                mag2 += Math.Pow(vector.Value[i], 2);
            }

            if (mag1 == 0 || mag2 == 0)
                return 0;
            else
                return (1.0 * dot) / (Math.Sqrt(mag1) * Math.Sqrt(mag2));
        }

        public double GetSimilarityMeasure(Vector vector, Func<Vector, double> funcName)
        {
            return funcName(vector);
        }

        public Dictionary<int, double> GetListSimilarityMeasure(List<Vector> listV, int quantity, string similarityName)
        {
            if (quantity <= 0)
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
                if (similarityName == "Euclid")
                {
                    for (int i = 0; i < listV.Count(); i++)
                        rs.Add(i, getEuclidSimilarity(listV[i]));

                    rs = rs.OrderBy(key => key.Value).Take(quantity).ToDictionary(x => x.Key, x => x.Value);
                }
                else if (similarityName == "Cosine")
                {
                    for (int i = 0; i < listV.Count(); i++)
                        rs.Add(i, getCosinSimilarity(listV[i]));

                    rs = rs.OrderByDescending(key => key.Value).Take(quantity).ToDictionary(x => x.Key, x => x.Value);
                }
                else
                    Console.WriteLine("Similarity name is invalid!");

                return rs;
            }
        }

        //1412542
        public static List<KeyValuePair<string, double>> Search(string inputFile, List<Vector> tfidf_Vector)
        {
            //Dictionary<string, double> similarDocs = new Dictionary<string, double>();
            List<KeyValuePair<string, double>> similarDocs = new List<KeyValuePair<string, double>>();
            try
            {
                // Read string and the number of documents that we need to search
                List<string> inputs = FileIO.ReadFile(inputFile);
                int numberOfDocs = int.Parse(inputs[inputs.Count - 2]);
                string similarityName = inputs[inputs.Count - 1];

                // Remove k and similarity name from search string list
                inputs.RemoveAt(inputs.Count - 1);
                inputs.RemoveAt(inputs.Count - 1);


                //List<Vector> searchVector = new List<Vector>(inputs.Count);
                foreach (string searchString in inputs)
                {
                    // Vectorise, calculate tf_idf
                    Vector searchVector = VectoriseStandardInput(searchString, ConfigurationManager.AppSettings.Get("FeatureFile"));

                    // Get documents that are similar to searchString
                    Dictionary<int, double> indexesAndSimilarities = searchVector.GetListSimilarityMeasure(tfidf_Vector, numberOfDocs, similarityName);

                    using (StreamReader sr = new StreamReader(ConfigurationManager.AppSettings.Get("RawFile")))
                    {
                        String line;
                        int index = 0;
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (indexesAndSimilarities.ContainsKey(index))
                            {
                                similarDocs.Add(new KeyValuePair<string, double>(line, indexesAndSimilarities[index]));
                                indexesAndSimilarities.Remove(index);

                                // Check if we got enough documents
                                if (indexesAndSimilarities.Count == 0)
                                    break;
                            }
                            ++index;
                        }
                    }
                }

                /*
                // Order documents by similarity
                if (similarityName == "Euclid")
                    similarDocs = similarDocs.OrderBy(x => x.Value).ToList();
                else
                    similarDocs = similarDocs.OrderByDescending(x => x.Value).ToList();*/
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return similarDocs;
        }
    }
}
