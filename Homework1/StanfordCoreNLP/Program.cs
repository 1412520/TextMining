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
           //PartOfSpeech.Tag("Barack Obama nominated Hillary Rodham Clinton as his secretary of state on Monday.\r\nHe chose her because she had foreign affairs experience as a former First Lady", POSMode.Left3Words);
          // Parser.ParserString("This is an example of dependency grammar");
           //var ner = new NER(NERMode.SevenClasses);
           //Console.WriteLine(ner.classifyToString("Barack Obama nominated Hillary Rodham Clinton as his secretary of state on Monday.\r\nHe chose her because she had foreign affairs experience as a former First Lady"));
           //Tokenisation.TokenizeText("Kosgi Santosh sent an email to Stanford University. He didn't get a reply.");
            //CorefAnnotator.FindCoreferenceResolution();
            Demonstrate("The original CRF code is by Jenny", POSMode.Left3Words, NERMode.SevenClasses);

        }

        public static void Demonstrate(string text, POSMode partOfSpeechMode, NERMode namedEntityRecognitionMode, bool disableLogging = true)
        {
            if (disableLogging)
                RedwoodConfiguration.current().clear().apply();
            //Input
            Console.WriteLine("Input: {0}\n\n\n", text);

            //Tokenization
            Console.WriteLine("Tokenization:");
            Tokenisation.TokenizeText(text);
            Console.WriteLine("\n\n\n");

            //POS
            Console.WriteLine("Part Of Speech:");
            PartOfSpeech.Tag(text, partOfSpeechMode);
            Console.WriteLine("\n\n\n");

            //NER
            Console.WriteLine("Named Entity Recognition:");
            var ner = new NER(namedEntityRecognitionMode);
            Console.WriteLine(ner.classifyToString(text));
            Console.WriteLine("\n\n\n");


            //Parser
            Console.WriteLine("Parsed Text:");
            Parser.ParserString(text);
            Console.WriteLine("\n\n\n");

            //Find co-reference
            CorefAnnotator.FindCoreferenceResolution();
        }
    }
}
