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

        public Vector(Vector vector)
        {
            Value = new List<double>(vector.Value);
            TextValue = new Document(vector.TextValue);
            ValueType = string.Copy(vector.ValueType);
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

        public static void GetValue(List<double> values, ref string formerString)
        {
            //string result = "";
            foreach (var val in values)
            {
                formerString += val + " ";
            }
            //return result;
            formerString += '\n';
        }

        //vectorise input base on features and their idf in baseFile
        public static Vector Vectorise(string input, string featureFile)
        {
            
            if (string.IsNullOrEmpty(input))
                return null;
            HashSet<string> stopWords = new HashSet<string>(FileIO.ReadFile(ConfigurationManager.AppSettings.Get("StopWordFile")));
            var standardInput = StringHelper.StandardizeString(input, stopWords);
            if (string.IsNullOrEmpty(standardInput))
                return null;    //invalid input
            var result = new Vector {
                TextValue = new Document {
                    RawText = input,
                    Text = standardInput
                },
                Value = VectoriseStandardInput(standardInput, featureFile) };
            return result;
        }

        public static List<double> VectoriseStandardInput(string standardInput, string featureFile)
        {
            var featuresIdf = Bow_tfidf.GetFeaturesIdf(featureFile);
            var result = VectoriseInput(standardInput, featuresIdf);
            return result;
        }

        public static List<double> VectoriseInput(string standardInput, Dictionary<string, double> featuresIdf)
        {
            //var featuresIdf = Bow_tfidf.GetFeaturesIdf(featureFile);
            var result = new List<double>();
            if (featuresIdf.Count > 0)
            {
                var features = featuresIdf.Keys.ToArray();
                var document = new Document(standardInput);
                foreach (var feature in features)
                {
                    if (document.Contains(feature))
                    {
                        var tf_idf = 1.0 * document.getFrequency(feature) / document.getMaxFrequency() * featuresIdf[feature];
                        result.Add(tf_idf);
                    }
                    else
                        result.Add(0);
                }
            }
            return result;
        }

        public double getEuclidSimilarity(string weighs)
        {
            var vector = new Vector(weighs);
            if (Value.Count != vector.Value.Count)
                return 0;
            double squareSum = 0;
            for (int i=0; i<Value.Count; i++)
            {
                squareSum += Math.Pow((Value[i] - vector.Value[i]), 2);
            }
            return Math.Sqrt(squareSum);
        }

        public double getCosinSimilarity(string weighs)
        {
            var vector = new Vector(weighs);
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

        //public Dictionary<int, double> GetListSimilarityMeasure (List<Vector> listV, int quantity, string similarityName)
        //{
        //    if(quantity <= 0)
        //    {
        //        return null;
        //    }
        //    else
        //    {
        //        if (listV.Count < quantity)
        //        {
        //            quantity = listV.Count;
        //        }
        //        Dictionary<int, double> rs = new Dictionary<int, double>(listV.Count);
        //        if (similarityName == "Euclid")
        //        {
        //            for (int i = 0; i < listV.Count(); i++)
        //                rs.Add(i, getEuclidSimilarity(listV[i]));

        //            rs = rs.OrderBy(key => key.Value).Take(quantity).ToDictionary(x => x.Key, x => x.Value);
        //        }
        //        else if (similarityName == "Cosine")
        //        {
        //            for (int i = 0; i < listV.Count(); i++)
        //                rs.Add(i, getCosinSimilarity(listV[i]));

        //            rs = rs.OrderByDescending(key => key.Value).Take(quantity).ToDictionary(x => x.Key, x => x.Value);
        //        }
        //        else
        //            Console.WriteLine("Similarity name is invalid!");

        //        return rs;
        //    }
        //}

        public Dictionary<int, double> GetListSimilarityMeasure(List<string> vectors, int quantity, string similarityName)
        {
            if (quantity <= 0)
            {
                return null;
            }
            else
            {
                if (vectors.Count < quantity)
                {
                    quantity = vectors.Count;
                }
                Dictionary<int, double> rs = new Dictionary<int, double>(vectors.Count);
                if (similarityName == "Euclid")
                {
                    for (int i = 0; i < vectors.Count(); i++)
                        rs.Add(i, getEuclidSimilarity(vectors[i]));

                    rs = rs.OrderBy(key => key.Value).Take(quantity).ToDictionary(x => x.Key, x => x.Value);
                }
                else if (similarityName == "Cosine")
                {
                    var watch = System.Diagnostics.Stopwatch.StartNew();

                    for (int i = 0; i < vectors.Count(); i++)
                        rs.Add(i, getCosinSimilarity(vectors[i]));
                    watch.Stop();
                    var elapsedMs = watch.ElapsedMilliseconds;
                    Console.WriteLine(elapsedMs);
                    rs = rs.OrderByDescending(key => key.Value).Take(quantity).ToDictionary(x => x.Key, x => x.Value);
                }
                else
                    Console.WriteLine("Similarity name is invalid!");

                return rs;
            }
        }


        //1412542
        //public static List<KeyValuePair<string, double>> Search(string inputFile)
        //{
        //    //Dictionary<string, double> similarDocs = new Dictionary<string, double>();
        //    List<KeyValuePair<string, double>> similarDocs = new List<KeyValuePair<string, double>>();
        //    try
        //    {
        //        // Read string and the number of documents that we need to search
        //        List<string> inputs = FileIO.ReadFile(inputFile);
        //        string searchString = inputs[0];
        //        int numberOfDocs = int.Parse(inputs[1]);
        //        string similarityName = inputs[2];

        //        // Read tf_idf
        //        List<string> tf_idfList = FileIO.ReadFile(ConfigurationManager.AppSettings.Get("BowTfIdfFile"));
        //        List<Vector> vectorList = new List<Vector>(tf_idfList.Count);
        //        foreach (string doc in tf_idfList)
        //        {
        //            vectorList.Add(new Vector(doc));
        //        }

        //        // Vectorise
        //        Vector searchVector = Vectorise(searchString, ConfigurationManager.AppSettings.Get("FeatureFile"));

        //        // Get documents that are similar to searchString
        //        Dictionary<int, double> indexesAndSimilarities = searchVector.GetListSimilarityMeasure(vectorList, numberOfDocs, similarityName);

        //        using (StreamReader sr = new StreamReader(ConfigurationManager.AppSettings.Get("RawFile")))
        //        {
        //            String line;
        //            int index = 0;
        //            while ((line = sr.ReadLine()) != null)
        //            {
        //                if (indexesAndSimilarities.ContainsKey(index))
        //                {
        //                    similarDocs.Add(new KeyValuePair<string, double>(line, indexesAndSimilarities[index]));
        //                    indexesAndSimilarities.Remove(index);

        //                    // Check if we got enough documents
        //                    if (indexesAndSimilarities.Count == 0)
        //                        break;
        //                }
        //                ++index;
        //            }
        //        }

        //        // Order documents by similarity
        //        if (similarityName == "Euclid")
        //            similarDocs = similarDocs.OrderBy(x => x.Value).ToList();
        //        else
        //            similarDocs = similarDocs.OrderByDescending(x => x.Value).ToList();
        //    }
        //    catch(Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }

        //    return similarDocs;
        //}

        //1412520
        public static int CountClassElements(string valueType, List<Vector> vectors)
        {
            var result = 0;
            foreach (var vector in vectors)
            {
                if (vector.ValueType == valueType)
                    result++;
            }
            return result;
        }

        //1412520
        public static HashSet<string> GetDistinctClassTypes(List<Vector> vectors)
        {
            var types = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var vector in vectors)
            {
                if (!types.Contains(vector.ValueType))
                    types.Add(vector.ValueType);
            }
            return types;
        }

        public static bool CheckIfContained(Vector vector, List<Vector> vectors)
        {
            foreach (var vec in vectors)
            {
                if ((vector.TextValue.RawText == vec.TextValue.RawText) && (vector.ValueType == vec.ValueType))
                    return true;
            }
            return false;
        }

        //1412520
        //count number of shared records of same type
        //source là file test, target là file train
        public static int CountShareSameTypeRecords(string valueType, List<Vector> sourceVectors, List<Vector> targetVectors)
        {
            int count = 0;
            for (int i =0; i<sourceVectors.Count; i++)
            {
                if (CheckIfContained(sourceVectors[i], targetVectors))
                    count++;
            }
            return count;
        }
    }
}
