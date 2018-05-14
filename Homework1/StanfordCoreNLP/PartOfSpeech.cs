using edu.stanford.nlp.tagger.maxent;
using java.io;
using java.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console = System.Console;

namespace StanfordCoreNLP
{
    public class PartOfSpeech
    {

        private MaxentTagger _tagger;
        private string _modelPath = @"../../../data/paket-files/stanford-corenlp-3.9.1-models/edu/stanford/nlp/models/pos-tagger";

        public PartOfSpeech(string modelPath)
        {
            _tagger = new MaxentTagger(modelPath,null, false);
        }

        public PartOfSpeech(POSMode posMode)
        {
            _modelPath = _modelPath + GetPOSModel(posMode);
            if (!System.IO.File.Exists(_modelPath))
                throw new Exception($"Check path to the model file '{_modelPath}'");
            var props = new Properties();
            _tagger = new MaxentTagger(_modelPath, props, false);
        }

        public string TagSentence(string sentence)
        {
            var tokenizedSentence = (ArrayList)MaxentTagger.tokenizeText(new StringReader(sentence)).get(0);
            var taggedSentence = _tagger.tagSentence(tokenizedSentence).toArray();
            return string.Join(" ", taggedSentence);
        }

        private static string GetPOSModel(POSMode posMode)
        {
            switch (posMode)
            {
                case POSMode.Bidirectional:
                    return @"/english-bidirectional/english-bidirectional-distsim.tagger";
                default:
                    return @"/english-left3words/english-left3words-distsim.tagger";
            }
        }

        public static void Tag(string text, POSMode posMode = 0)
        {

            //Loading POS Tagger
            var tagger = new PartOfSpeech(posMode);
            var taggedResult = tagger.TagSentence(text);
            System.Console.WriteLine(taggedResult);
        }
    }
}
