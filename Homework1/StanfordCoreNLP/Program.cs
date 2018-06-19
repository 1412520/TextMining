using edu.stanford.nlp.ling;
using edu.stanford.nlp.tagger.maxent;
//using java.io;
//using java.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using edu.stanford.nlp.util.logging;
using static edu.stanford.nlp.util.logging.StanfordRedwoodConfiguration;

namespace StanfordCoreNLP
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Demonstrate("Will Smith booked a meeting at the library to read a book that he ordered from France in July", POSMode.Bidirectional, NERMode.SevenClasses, false);
            //Pipeline.Execute("tokenize, ssplit, pos, lemma, ner, parse", "This is a sentence", false);
        }

        public static void Demonstrate(string text, POSMode partOfSpeechMode, NERMode namedEntityRecognitionMode, bool disableLogging = true)
        {
            if (disableLogging)
                RedwoodConfiguration.current().clear().apply();
            //Input
            //Console.WriteLine("Input: {0}\n\n\n", text);

            ////Tokenization
            //Console.WriteLine("Tokenization:");
            //Tokenisation.TokenizeText(text);
            //Console.WriteLine("\n\n\n");

            ////POS
            //Console.WriteLine("Part Of Speech:");
            //PartOfSpeech.Tag(text, partOfSpeechMode);
            //Console.WriteLine("\n\n\n");

            ////NER
            //Console.WriteLine("Named Entity Recognition:");
            //var ner = new NER(namedEntityRecognitionMode);
            //Console.WriteLine(ner.classifyToString(text));
            //Console.WriteLine("\n\n\n");


            ////Parser
            //Console.WriteLine("Parsed Text:");
            //Parser.ParseString(text);
            //Console.WriteLine("\n\n\n");

            //Find co-reference
            CorefAnnotator.FindCoreferenceResolution(text);
        }
    }
}
