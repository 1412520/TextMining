using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework1
{
    class Ngram
    {
        public static List<string> makeNgram(string text, int nGramSize)
        {
            String nGram = "";
            Queue<int> wordLengths = new Queue<int>();
            int wordCount = 0;
            int lastWordLen = 0;
            List<string> result = new List<string>();
            int temp = 0;
            if (text != "" && char.IsLetterOrDigit(text[0]))
            {
                //nGram.Append(text[0]);
                nGram += text[0].ToString();
                lastWordLen++;
                temp = 1;
            }

            for (int i = 1; i < text.Length - 1; i++)
            {
                char before = text[i - 1];
                char after = text[i + 1];
                if (char.IsLetterOrDigit(text[i])
                    ||
                    (text[i]) != ' '
                      && (char.IsSeparator(text[i]) || char.IsPunctuation(text[i])
                      && (char.IsLetterOrDigit(before) || char.IsLetterOrDigit(after))
                      )
                   )
                {
                    nGram += text[i].ToString();
                    lastWordLen++;
                }
                else
                {
                    if (lastWordLen > 0)
                    {
                        wordLengths.Enqueue(lastWordLen);
                        lastWordLen = 0;
                        wordCount++;
                        if (wordCount >= nGramSize)
                        {
                            result.Add(nGram.ToString().Trim());
                            if (nGramSize > 1)
                                nGram = nGram.Remove(0, wordLengths.Dequeue() + 1);
                            else
                                nGram = nGram.Trim().Remove(0, wordLengths.Dequeue());
                            wordCount -= 1;
                        }
                        nGram += " ";
                    }
                }
            }
            if(temp == 1)
                result.Add(nGram.ToString().Trim()+text[text.Length - 1]);
            return result;
        }
    }
}
