using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Homework1;

namespace Classification
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputFile = "../../test.txt";

            // Read string and the number of documents that we need to search
            List<string> inputs = FileIO.ReadFile(inputFile);
            int numberOfDocs = int.Parse(inputs[inputs.Count - 2]);
            string similarityName = inputs[inputs.Count - 1];

            // Remove k and similarity name from search string list
            inputs.RemoveAt(inputs.Count - 1);
            inputs.RemoveAt(inputs.Count - 1);
            /*
            List<string> labeledString = new List<string>();
            string label;
            foreach (string searchString in inputs)
            {
                label = Homework1.Program.Main(searchString, numberOfDocs, similarityName, )
                labeledString.Add(searchString + " - " + label);
            }*/
        }
    }
}
