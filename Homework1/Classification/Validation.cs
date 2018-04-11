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
        public static List<string> revoveLabelOfList(List<string> input)
        {
            for (int i = 0; i < input.Count(); i++)
                input[i] = input[i].Split('-').Last();

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
                // subset[i] is test set, others are training sets
                List<string> trainingSet = new List<string>();
                List<string> testTarget = new List<string>();
                List<string> testSet = new List<string>();
                for( int subIndex = 0; subIndex < numberOfFolds; subIndex++)
                {
                    if (subIndex == i)
                        trainingSet.AddRange(subset[subIndex]);
                    else
                        testTarget = subset[i];
                }
                FileIO.WriteFile(testTarget, "../../validation/testTarget.txt");
                //Remove label of testSetTarget
                testSet = revoveLabelOfList(testTarget);
                //Write testSet and trainSet into file
                FileIO.WriteFile(trainingSet, "../../validation/train.txt");
                FileIO.WriteFile(testSet, "../../validation/test.txt");

                Bow_tfidf.GenerateTFIDFMatrix("../../validation/train.txt", "../../validation/processed.txt", "../../validation/features.txt", "../../validation/tf_idf.txt");
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
                Console.WriteLine("Fmacro " + sum_Fmacro);
                Console.WriteLine("Fmicro " + sum_Fmicro);
            }

            List<string> F_array = new List<string>();
            double avg_Fmacro = (1.0 * sum_Fmicro) / numberOfFolds;
            double avg_Fmicro = (1.0 * sum_Fmacro) / numberOfFolds;

            F_array.Add(avg_Fmacro.ToString());
            F_array.Add(avg_Fmicro.ToString());

            //Write avg Fmacro and avg Fmicro into file
            FileIO.WriteFile(F_array, "../../validation/result.txt");
        }


    }
}
