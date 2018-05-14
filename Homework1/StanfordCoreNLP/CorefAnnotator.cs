using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

using java.util;
using java.io;
using edu.stanford.nlp.coref;
using edu.stanford.nlp.ling;
using edu.stanford.nlp.pipeline;
using edu.stanford.nlp.util;
using Console = System.Console;
using edu.stanford.nlp.coref.data;
using java.lang;
using System.IO;
// use .NET framework 3.5
using StanfordNLP = edu.stanford.nlp.pipeline;

namespace StanfordCoreNLP
{
    public class CorefAnnotator
    {
        public static void FindCoreferenceResolution()
        {
            var text = "Barack Obama nominated Hillary Rodham Clinton as his secretary of state on Monday. He chose her because she had foreign affairs experience as a former First Lady.";

            var jarRoot = @"../../../data/paket-files/stanford-corenlp-3.9.1-models/";
            var propsFile = Path.Combine(jarRoot, "StanfordCoreNLP.properties");
            var props = new Properties();
            props.setProperty("annotators", "coref");
            props.load(new FileReader(propsFile));
            props.setProperty("ner.useSUTime", "0");

            // We should change current directory, so StanfordCoreNLP could find all the model files automatically
            var curDir = Environment.CurrentDirectory;
            Directory.SetCurrentDirectory(jarRoot);
            var pipeline = new StanfordNLP.StanfordCoreNLP(props);
            Directory.SetCurrentDirectory(curDir);

            // Annotation
            var annotation = new Annotation(text);
            pipeline.annotate(annotation);

            // Result (detail)
            //using (var stream = new ByteArrayOutputStream())
            //{
            //    pipeline.prettyPrint(annotation, new PrintWriter(stream));
            //    Console.WriteLine(stream.toString());
            //    stream.close();
            //}

            // Result
            var corefChainAnnotation = new CorefCoreAnnotations.CorefChainAnnotation().getClass();
            var sentencesAnnotation = new CoreAnnotations.SentencesAnnotation().getClass();
            var corefMentionsAnnotation = new CorefCoreAnnotations.CorefMentionsAnnotation().getClass();

            Console.WriteLine("---");
            Console.WriteLine("coref chains");
            var corefChain = annotation.get(corefChainAnnotation) as Map;
            foreach (CorefChain cc in corefChain.values().toArray())
            {
                Console.WriteLine($"\t{cc}");
            }
            var sentences = annotation.get(sentencesAnnotation) as ArrayList;
            foreach (CoreMap sentence in sentences.toArray())
            {
                Console.WriteLine("---");
                Console.WriteLine("mentions");
                var corefMentions = sentence.get(corefMentionsAnnotation) as ArrayList;
                foreach (Mention m in corefMentions)
                {
                    Console.WriteLine("\t" + m);
                }
            }
        }
    }
}
