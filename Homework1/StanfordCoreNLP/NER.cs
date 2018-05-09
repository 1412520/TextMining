using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using edu.stanford.nlp.ie.crf;
using edu.stanford.nlp.util;

namespace StanfordCoreNLP
{
    public class NER
    {
        // Path to the folder with classifiers models
        string classifiersDirecrory = @"../../stanford-ner-2013-06-20/classifiers";
        string model = @"/english.muc.7class.distsim.crf.ser.gz";

        public NER() {  }

        public NER(string model)
        {
            this.model = model;
        }

        public string classifyToString(string sentence, string outputFormat = "slashTags")
        {
            // Loading classes classifier model
            var classifier = CRFClassifier.getClassifierNoExceptions(
                classifiersDirecrory + model);

            return classifier.classifyToString(sentence, outputFormat, true);
            //.WriteLine("{0}\n", classifier.classifyToString(sentence, outputFormat, true));

            //var classified = classifier.classifyToCharacterOffsets(s1).toArray();

            //for (int i = 0; i < classified.Length; i++)
            //{
            //    Triple triple = (Triple)classified[i];

            //    int second = Convert.ToInt32(triple.second().ToString());
            //    int third = Convert.ToInt32(triple.third().ToString());

            //    Console.WriteLine(triple.first().ToString() + '\t' + s1.Substring(second, third - second));
            //}
        }

    }
        
    
}