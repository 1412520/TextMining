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
            //string kFoldFile = "../../k_folds.txt";
            //string rawFile = "../../training/raw_text.txt";
            //CrossValidation.Validate(kFoldFile, rawFile);
            //Bow_tfidf.GenerateTFIDFMatrix("../../training/raw_text.txt", "../../training/processed.txt", "../../training/features.txt", "../../training/tf_idf.txt");
            Model.classify();
        }
    }
}
