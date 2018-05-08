using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using java.io;
using edu.stanford.nlp.process;
using edu.stanford.nlp.ling;
using edu.stanford.nlp.trees;
using edu.stanford.nlp.parser.lexparser;
using edu.stanford.nlp.parser.nndep.demo;
using Console = System.Console;

namespace StanfordCoreNLP
{
    class Parser
    {
        public Parser() { }

        public static void ParserString(string sentence)
        {
            // Path to models extracted from `stanford-parser-3.6.0-models.jar`
            var modelsDirectory = @"../../stanford-parser-3.9.1-models/edu/stanford/nlp/models";
            var model = @"/lexparser/englishPCFG.ser.gz";
            //var model = @"/parser/nndep/english_SD.gz";

            // Loading english PCFG parser from file
            var lp = LexicalizedParser.loadModel(modelsDirectory + model);

            // This sample shows parsing a list of correctly tokenized words
            var rawWords = SentenceUtils.toCoreLabelList(sentence);
            var tree = lp.apply(rawWords);
            tree.pennPrint();

            // This option shows loading and using an explicit tokenizer
            var tokenizerFactory = PTBTokenizer.factory(new CoreLabelTokenFactory(), "");
            var sent2Reader = new StringReader(sentence);
            var rawWords2 = tokenizerFactory.getTokenizer(sent2Reader).tokenize();
            sent2Reader.close();
            var tree2 = lp.apply(rawWords2);

            // Extract dependencies from lexical tree
            var tlp = new PennTreebankLanguagePack();
            var gsf = tlp.grammaticalStructureFactory();
            var gs = gsf.newGrammaticalStructure(tree2);
            var tdl = gs.typedDependenciesCCprocessed();
            Console.WriteLine("\n{0}\n", tdl);

            // Extract collapsed dependencies from parsed tree
            var tp = new TreePrint("penn,typedDependenciesCollapsed");
            tp.printTree(tree2);
        }

    }
}
