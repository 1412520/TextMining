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
    }
}
