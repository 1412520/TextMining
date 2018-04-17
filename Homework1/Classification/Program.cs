using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Homework1;

namespace Classification
{
    public class Program
    {
        static void Main(string[] args)
        {
            string kFoldFile = "../../k_folds.txt";
            string rawFile = "../../training/raw_text.txt";
            //FileUtils.preprocessDirectory("../../input", "../../training/raw_text.txt");
            //CrossValidation.Validate(kFoldFile, rawFile);
            CrossValidation.ClassifyAndValidateOne();
            //Model.classify();

            //SVM.ClassifyBySVM("../../validation/train.txt", "../../validation/test.txt", "../../validation/testTarget.txt");
        }
    }
}
