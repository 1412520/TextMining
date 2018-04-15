using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Homework1;

namespace Classification
{
    public class CrossValidation
    {
        //1412503
        public static double CalculatePi(string valueType, List<Vector> sourceVectors, List<Vector> targetVectors)
        {
            if (Vector.CountClassElements(valueType, sourceVectors) == 0)
                return 1.0;
            else
                return 1.0 * Vector.CountShareSameTypeRecords(valueType, sourceVectors, targetVectors) / Vector.CountClassElements(valueType, sourceVectors);
        }

        //1412503
        public static double CalculatePmacro(List<Vector> sourceVectors, List<Vector> targetVectors)
        {
            HashSet<String> classes = Vector.GetDistinctClassTypes(targetVectors);
            double sumPi = 0;
            for (int i = 0; i < classes.Count; i++)
            {
                sumPi += CalculatePi(classes.ElementAt(i), sourceVectors, targetVectors);
            }
            return 1.0 * sumPi / classes.Count;
        }

        //1412595
        public static double calculateRi(string valueType, List<Vector> sourceVectors, List<Vector> targetVectors)
        {
            return 1.0 * Vector.CountShareSameTypeRecords(valueType, sourceVectors, targetVectors) / Vector.CountClassElements(valueType, targetVectors);
        }

        //1412595
        public static double calculateRmacro(List<Vector> sourceVectors, List<Vector> targetVectors)
        {
            HashSet<String> classes = Vector.GetDistinctClassTypes(targetVectors);
            double sumR = 0;
            for (int i = 0; i < classes.Count; i++)
            {
                sumR += calculateRi(classes.ElementAt(i), sourceVectors, targetVectors);
            }
            return 1.0 * sumR / classes.Count;
        }

        //1412595
        public static double calculateFmicro(List<Vector> sourceVectors, List<Vector> targetVectors)
        {
            HashSet<String> classes = Vector.GetDistinctClassTypes(targetVectors);
            double sum = 0;
            for (int i = 0; i < classes.Count; i++)
            {
                sum += Vector.CountShareSameTypeRecords(classes.ElementAt(i), sourceVectors, targetVectors);
            }
            return 1.0 * sum / targetVectors.Count();
        }

        public static double calculateFmacroOrFscore(Double R, Double P)
        {
            return (1.0 * 2 * R * P) / (R + P);
        }

        //1412543
        public static List<string> RemoveLabelOfList(List<string> input)
        {
            for (int i = 0; i < input.Count(); i++)
            {
                //input[i] = input[i].Split('-').Last();
                string sub = input[i].Remove(input[i].IndexOf("-"));
                input[i] = input[i].Substring(input[i].IndexOf("-") + 1, input[i].Count() - sub.Count() -1 );
            }

            return input;
        }

        // 141242
        // Date: 08/04/2018
        static void SplitTrainTest(List<string> data, int numberOfFolds)
        {
            //List<List<string>> subSets = new List<List<string>>(numberOfFolds);

            // Delete all files in folder
            FileUtils.deleteAllFiles("../../splited_data");

            int numberOfData, numberOfOneFold, remainder;
            numberOfData = data.Count;
            numberOfOneFold = numberOfData / numberOfFolds;
            remainder = numberOfData % numberOfFolds;

            // Split data into k folds
            for (int i = 1; i <= numberOfFolds; i++)
            {
                // Nếu số lượng dữ liệu không chia hết cho k, thì (remainder fold) đầu tiên có (numberOfOneFold + 1) dữ liệu
                int realNumberOfOneFold = i < remainder ? numberOfOneFold + 1 : numberOfOneFold;
                List<string> subSet = new List<string>(realNumberOfOneFold);

                // Set subset i 
                for (int j = 0; j < realNumberOfOneFold; j++)
                {
                    Random rnd = new Random();
                    int index = rnd.Next(data.Count);
                    subSet.Add(data[index]);
                    data.RemoveAt(index);
                }

                // Write subset to file
                string filename = "subset" + i + ".txt";
                FileIO.WriteFile(subSet, "../../splited_data/" + filename);
            }
            
        }

        // 1412542
        // Date: 08/04/2018
        // Cross validation
        public static void Validate(string kFoldsFile, string inputFile)
        {
            
            // Read the number of subset
            int numberOfFolds = int.Parse(FileIO.ReadFile(kFoldsFile)[0]);

            
            /*// Read data
            List<string> data = FileIO.ReadFile(inputFile);

            // Split data into subsets
            SplitTrainTest(data, numberOfFolds);*/
           

            // Read subsets
            List<List<string>> subset = new List<List<string>>();
            List<string> fileNameList = FileUtils.getAllFileNames("../../splited_data");

            foreach( string fileName in fileNameList)
            {
                subset.Add(FileIO.ReadFile(fileName));
            }

            //calculate avg Fmicro and avg Fmacro
            double sum_Fmicro = 0;
            double sum_Fmacro = 0;

            // build model and test by k-folds cross validation
            for (int i = 0; i < numberOfFolds; i++)
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();
                //Console.WriteLine("Fold {0}: ", i);
                // subset[i] is test set, others are training sets
                List<string> trainingSet = new List<string>();
                List<string> testTarget = new List<string>();
                List<string> testSet = new List<string>();
                for( int subIndex = 0; subIndex < numberOfFolds; subIndex++)
                {
                    if (subIndex != i)
                        trainingSet.AddRange(subset[subIndex]);
                    else
                        testTarget = subset[i];
                }
                //Console.WriteLine("The time to create training and test sets: {0} ", watch.ElapsedMilliseconds);
                FileIO.WriteFile(testTarget, "../../validation/testTarget.txt");
                //Remove label of testSetTarget
                testSet = RemoveLabelOfList(testTarget);
                //Write testSet and trainSet into file
                FileIO.WriteFile(trainingSet, "../../validation/train.txt");
                FileIO.WriteFile(testSet, "../../validation/test.txt");
                //Console.WriteLine("The time to write training and test sets: {0} ", watch.ElapsedMilliseconds);
            
                Bow_tfidf.GenerateTFIDFMatrix("../../validation/train.txt", "../../validation/processed.txt", "../../validation/features.txt", "../../validation/tf_idf.txt");
                //Console.WriteLine("The time to calculate tf_idf: {0} ", watch.ElapsedMilliseconds);

                Model.classifyForValidate();

                bool hasValueType = true;

                List<Vector> targetVector = new List<Vector>();
                List<string> testSetTarget = FileIO.ReadFileIntoVector("../../validation/testTarget.txt", out targetVector, hasValueType);

                List<Vector> sourceVector = new List<Vector>();
                List<string> testSetResult = FileIO.ReadFileIntoVector("../../validation/testResult.txt", out sourceVector, hasValueType);

                Double Rmacro = calculateRmacro(sourceVector, targetVector);
                Double Pmacro = CalculatePmacro(sourceVector, targetVector);

                sum_Fmacro += calculateFmacroOrFscore(Rmacro, Pmacro);
                sum_Fmicro += calculateFmicro(sourceVector, targetVector);
                //watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                //Console.WriteLine("Validating the fold {0} takes: {1} ",i, elapsedMs);
            }

            List<string> F_array = new List<string>();
            double avg_Fmacro = (1.0 * sum_Fmicro) / numberOfFolds;
            double avg_Fmicro = (1.0 * sum_Fmacro) / numberOfFolds;

            F_array.Add(avg_Fmacro.ToString());
            F_array.Add(avg_Fmicro.ToString());

            //Write avg Fmacro and avg Fmicro into file
            FileIO.WriteFile(F_array, "../../validation/result.txt");
        }

        //public static void ClassifyAndValidateOne(string trainFile, string sampleClassifiedFile, string classificationFile, string validationFile)
        //1412520
        public static void ClassifyAndValidateOne()
        {
            var testsWithValueType = FileIO.ReadFile("../../validation/testTarget.txt");
            var testRecords = RemoveLabelOfList(testsWithValueType);
            FileIO.WriteFile(testRecords, "../../validation/testResult.txt");
            Model.classifyForValidate();
            Validate("../../validation/testTarget.txt", "../../validation/testResult.txt", "../../validation/result.txt");
        }

        //1412520
        public static void Validate(string sampleClassificationFile, string classificationFile, string resultFile)
        {
            var sampleVectors = new List<Vector>();
            var vectors = new List<Vector>();

            FileIO.ReadFileIntoVector(sampleClassificationFile, out sampleVectors, true);
            FileIO.ReadFileIntoVector(classificationFile, out vectors, true);

            var classes = Vector.GetDistinctClassTypes(sampleVectors);

            List<string> result = new List<string>(classes.Count * 3 + 2);
            int i = 0;
            double sumP = 0;
            double sumR = 0;
            int preciseClassification = 0;
            foreach (var type in classes)
            {
                var Ptype = CalculatePi(type, vectors, sampleVectors);
                var Rtype = calculateRi(type, vectors, sampleVectors);
                var Ftype = calculateFmacroOrFscore(Rtype, Ptype);
                result.Add('P' + type + ' ' + Ptype.ToString());
                result.Add('R' + type + ' ' + Rtype.ToString());
                result.Add('F' + type + ' ' + Ftype.ToString());
                sumP += Ptype;
                sumR += Rtype;
                preciseClassification += Vector.CountShareSameTypeRecords(type, vectors, sampleVectors);
            }
            double Pmacro = sumP / classes.Count;
            double Rmacro = sumR / classes.Count;
            double Fmacro = calculateFmacroOrFscore(Rmacro, Pmacro);
            result.Add("Fmacro " + Fmacro.ToString());
            double Fmicro = preciseClassification / sampleVectors.Count;
            result.Add("Fmicro " + Fmicro.ToString());

            FileIO.WriteFile(result, resultFile);
        }
    }
}
