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
    class Tokenisation
    {
        public static void TokenizeText(string text)
        {
            //// Path to the folder with models extracted from `stanford-corenlp-3.7.0-models.jar`
            //var jarRoot = @"../../data/paket-files/stanford-corenlp-full-2016-10-31/models";


            //// Annotation pipeline configuration
            //var props = new Properties();
            //props.setProperty("annotators", "tokenize");
            //props.setProperty("ner.useSUTime", "0");

            //// We should change current directory, so StanfordCoreNLP could find all the model files automatically
            //var curDir = Environment.CurrentDirectory;
            //Directory.SetCurrentDirectory(jarRoot);
            //var pipeline = new StanfordNLP.StanfordCoreNLP(props);
            //Directory.SetCurrentDirectory(curDir);

            //// Annotation
            //var annotation = new Annotation(text);
            //pipeline.annotate(annotation);

            //// Result - Pretty Print
            //using (var stream = new ByteArrayOutputStream())
            //{
            //    pipeline.prettyPrint(annotation, new PrintWriter(stream));
            //    Console.WriteLine(stream.toString());
            //    stream.close();
            //    System.Console.Read();
            //}

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
