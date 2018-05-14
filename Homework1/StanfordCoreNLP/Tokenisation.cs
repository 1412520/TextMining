using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using edu.stanford.nlp;
using Console = System.Console;
using java.util;
using edu.stanford.nlp.pipeline;
using java.io;
using StanfordNLP = edu.stanford.nlp.pipeline;
using edu.stanford.nlp.ling;
using edu.stanford.nlp.process;

namespace StanfordCoreNLP
{
    public class Tokenisation
    {
        public static void TokenizeText(string text)
        {

            var temp = new StringReader(text);
            PTBTokenizer ptbt = new PTBTokenizer(temp,
              new CoreLabelTokenFactory(), "");
            while (ptbt.hasNext())
            {
                CoreLabel label = (CoreLabel) ptbt.next();
                Console.WriteLine(String.Format("{0}\t| BEGIN_OFFSET: {1}\t| END_OFFSET: {2}", label.value(), label.beginPosition(), label.endPosition()));
            }
        }
    }
}
