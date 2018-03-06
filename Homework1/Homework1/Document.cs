using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework1
{
    public class Document
    {
        //1412503
        private Dictionary<String, int> doc;
        private int maxFreq;

        //1412503
        public Document() { }

        //1412503
        public Document(String _doc)
        {
            maxFreq = 0;
            int freq;

            string[] splitDoc = _doc.Split(' ');
            doc = new Dictionary<string, int>();
            for (int i = 0; i < splitDoc.Count(); i++)
            {
                freq = 0;

                try
                {
                    freq = ++doc[splitDoc[i]];
                }
                catch
                {
                    freq = 1;
                }

                if (freq > maxFreq)
                {
                    maxFreq = freq;
                }
                doc[splitDoc[i]] = freq;
            }
        }

        //1412503
        public int getFrequency(string word)
        {
            return doc[word];
        }

        //1412503
        public int getMaxFrequency()
        {
            return maxFreq;
        }

        //1412542
        public bool Contains(string word)
        {
            return doc.ContainsKey(word);
        }
    }
}

