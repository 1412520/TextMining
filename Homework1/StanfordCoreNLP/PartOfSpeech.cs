using edu.stanford.nlp.tagger.maxent;
using java.io;
using java.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StanfordCoreNLP
{
    public class PartOfSpeech
    {

        public MaxentTagger _tagger { get; set; }

        public PartOfSpeech(string modelPath)
        {
            _tagger = new MaxentTagger(modelPath);
        }

        public string TagSentence(string sentence)
        {
            var tokenizedSentence = (ArrayList)MaxentTagger.tokenizeText(new StringReader(sentence)).get(0);
            var taggedSentence = _tagger.tagSentence(tokenizedSentence);
            return taggedSentence.ToString();
        }

        public static void Demonstrate()
        {
            var jarRoot = @"../../../data/paket-files/stanford-postagger-full-2018-02-27";
            var modelsDirectory = jarRoot + @"/models";
            var model = modelsDirectory + @"/wsj-0-18-bidirectional-nodistsim.tagger";

            if (!System.IO.File.Exists(model))
                throw new Exception($"Check path to the model file '{model}'");

            //Loading POS Tagger
           var tagger = new PartOfSpeech(model);

            // Text for tagging
            var text = "I will book a meeting tonight, do you read that book?";
            var taggedResult = tagger.TagSentence(text);
            System.Console.WriteLine(taggedResult);
        }
    }
}
