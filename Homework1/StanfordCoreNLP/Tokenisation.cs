using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using edu.stanford.nlp;
using Console = System.Console;
using System.IO;
using java.util;
using edu.stanford.nlp.pipeline;
using java.io;
using StanfordNLP = edu.stanford.nlp.pipeline;

namespace StanfordCoreNLP
{
    public class Tokenisation
    {
        public static void TokenizeText(string text)
        {
            // Path to the folder with models extracted from `stanford-corenlp-3.7.0-models.jar`
            var jarRoot = @"../../../data/paket-files/stanford-corenlp-3.9.1-models/edu/stanford/nlp/models";

            // Text for processing
            //var text = "Kosgi Santosh sent an email to Stanford University. He didn't get a reply.";

            // Annotation pipeline configuration
            var props = new Properties();
            props.setProperty("annotators", "tokenize");
            props.setProperty("ner.useSUTime", "0");

            // We should change current directory, so StanfordCoreNLP could find all the model files automatically
            var curDir = Environment.CurrentDirectory;
            Directory.SetCurrentDirectory(jarRoot);
            var pipeline = new StanfordNLP.StanfordCoreNLP(props);
            Directory.SetCurrentDirectory(curDir);

            // Annotation
            var annotation = new Annotation(text);
            pipeline.annotate(annotation);

            // Result - Pretty Print
            using (var stream = new ByteArrayOutputStream())
            {
                pipeline.prettyPrint(annotation, new PrintWriter(stream));
                Console.WriteLine(stream.toString());
                stream.close();
                System.Console.Read();
            }
        }
    }
}
