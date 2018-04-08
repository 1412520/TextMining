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

            /*
            // Read data
            List<string> data = FileIO.ReadFile(inputFile);

            // Split data into subsets
            SplitTrainTest(data, numberOfFolds);
           */

            // Read subsets
            List<List<string>> subset = new List<List<string>>();
            List<string> fileNameList = FileUtils.getAllFileNames("../../splited_data");

            foreach( string fileName in fileNameList)
            {
                subset.Add(FileIO.ReadFile(fileName));
            }

            // build model and test by k-folds cross validation
            for( int i = 0; i < numberOfFolds; i++)
            {
                // subset[i] is test set, others are training sets
                List<string> trainingSet = new List<string>();
                for( int subIndex = 0; subIndex < numberOfFolds; subIndex++)
                {
                    if (subIndex != i)
                        trainingSet.AddRange(subset[subIndex]);
                }
            }
        }
    }
}
