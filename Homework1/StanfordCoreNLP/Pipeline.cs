using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using edu.stanford.nlp.coref.data;
using edu.stanford.nlp.ling;
using edu.stanford.nlp.semgraph;
using edu.stanford.nlp.trees;
using edu.stanford.nlp.util;
using edu.stanford.nlp.util.logging;
using java.io;
using java.util;
using Console = System.Console;
using StanfordNLP = edu.stanford.nlp.pipeline;

namespace StanfordCoreNLP
{
    public class Pipeline
    {
        public static void Execute(string option, string text, bool disableLogging = true)
        {
            if (disableLogging)
                RedwoodConfiguration.current().clear().apply();
            var jarRoot = @"../../../data/paket-files/stanford-corenlp-3.9.1-models/";
            var props = new Properties();
            props.setProperty("annotators", option);
            props.setProperty("ner.useSUTime", "0");

            // We should change current directory, so StanfordCoreNLP could find all the model files automatically
            var curDir = Environment.CurrentDirectory;
            Directory.SetCurrentDirectory(jarRoot);
            var pipeline = new StanfordNLP.StanfordCoreNLP(props);
            Directory.SetCurrentDirectory(curDir);

            // Annotation
            var annotation = new StanfordNLP.Annotation(text);
            pipeline.annotate(annotation);

            //get sentencesAnnotation to get sentences
            var sentencesAnnotation = new CoreAnnotations.SentencesAnnotation().getClass();

            //get tokensAnnotaion to get tokens in each sentence
            var tokensAnnotaion = new CoreAnnotations.TokensAnnotation().getClass();

            //get posAnnotation to get POS result of each token
            var posAnnotation = new CoreAnnotations.PartOfSpeechAnnotation().getClass();

            //get nerAnnotation to get NER result of each token
            var nerAnnotaion = new CoreAnnotations.NamedEntityTagAnnotation().getClass();
            var deparseAnnotation = new TreeCoreAnnotations.TreeAnnotation().getClass();
            //deparseAnnotation = new TypedDependency().getClass();
            var sentences = annotation.get(sentencesAnnotation) as ArrayList;
            foreach (CoreMap sentence in sentences.toArray())
            {

                var tokens = (ArrayList) sentence.get(tokensAnnotaion);
                Console.WriteLine("Token-POS-NER: ");
                foreach (CoreLabel token in tokens)
                {
                    Console.Write($"{token.value()}-{token.get(posAnnotation)}-{token.get(nerAnnotaion)} ");
                }
                Console.WriteLine("\n\n\n");
                var parsedText = (Tree)sentence.get(deparseAnnotation);
                if (parsedText != null)
                {
                    Console.WriteLine("Parsed Text: ");
                    new TreePrint("penn,typedDependenciesCollapsed").printTree(parsedText);
                }
            }
        }
    }
}
