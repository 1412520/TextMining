using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Homework1;
using libsvm;
using System.Configuration;

namespace Classification
{
    class SVM
    {
        private static Dictionary<int, string> _predictionDictionary;

        private static IEnumerable<string> GetWords(string x)
        {
            return x.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static double[] GetLableOfDocument(List<Vector> input)
        {
            string[] labelArray = new string[input.Count()];
            for (int i = 0; i < input.Count(); i++)
            {
                labelArray[i] = input[i].ValueType;
            }

            var typeClass = Vector.GetDistinctClassTypes(input);

            for (int i = 0; i < labelArray.Count(); i++)
            {
                for (int j = 0; j < typeClass.Count(); j++)
                    if (labelArray[i] == typeClass.ElementAt(j))
                        labelArray[i] = j.ToString();
            }

            double[] numLabelArray = labelArray.Select(double.Parse).ToArray();

            return numLabelArray;
        }
        public static void ClassifyBySVM(string trainFile, string testFile, string testTarget)
        {
            string testResultFile = "../../svm/testResult.txt";

            var watch = System.Diagnostics.Stopwatch.StartNew();
            //STEP 1 : READ DATA
            List<Vector> vectorsTrain = new List<Vector>();
            var content = FileIO.ReadFileIntoVector(trainFile, out vectorsTrain, true);
            var typeClass = Vector.GetDistinctClassTypes(vectorsTrain);

            //Get content of document and lable of document
            double[] label = GetLableOfDocument(vectorsTrain);

            //Get features list
            var features = content.SelectMany(GetWords).Distinct().OrderBy(word => word).ToList();

            //STEP 2: Generate a problem
            var problem = TextClassificationProblemBuilder.CreateProblem(content, label, features.ToList());

            //STEP 3: Create and train a SVM model
            const int C = 1;
            var model = new C_SVC(problem, KernelHelper.LinearKernel(), C);

            //STEP 4: Predict
            List<string> test = FileIO.ReadFile(testFile);
            List<string> resultList = new List<string>();

            _predictionDictionary = new Dictionary<int, string>();
            List<Vector> targetVector = new List<Vector>();
            FileIO.ReadFileIntoVector(testTarget, out targetVector, true);

            for (int l = 0; l < typeClass.Count(); l++)
            {
                _predictionDictionary.Add(l, typeClass.ElementAt(l));
            }

            for (int i = 0; i < test.Count(); i++)
            {
                var newX = TextClassificationProblemBuilder.CreateNode(test[i], features);
                var predictedY = model.Predict(newX);
                var result = _predictionDictionary[(int)predictedY];
                resultList.Add(result + " - " + test[i]);
            }
            FileIO.WriteFile(resultList, testResultFile);

            List<Vector> sourceVector = new List<Vector>();
            FileIO.ReadFileIntoVector(testResultFile, out sourceVector, true);

            double score = 0;
            for (int i = 0; i < typeClass.Count(); i++)
            {
                score = 1.0 * Vector.CountShareSameTypeRecords(typeClass.ElementAt(i), sourceVector, targetVector) / Vector.CountClassElements(typeClass.ElementAt(i), targetVector);
                Console.WriteLine("correct label: " + Vector.CountShareSameTypeRecords(typeClass.ElementAt(i), sourceVector, targetVector));
                Console.WriteLine("total label: " + Vector.CountClassElements(typeClass.ElementAt(i), targetVector));
                Console.WriteLine("SVM score: " + score);
            }

            Console.WriteLine("The time for SVM: {0} ", watch.ElapsedMilliseconds);
        }

    }
}
