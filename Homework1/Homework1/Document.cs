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
        private Dictionary<String, int> termFrequency;
        private int maxFreq;

        //1412503
        public Document() { }

        //1412503
        public Document(String _doc) 
        {
            maxFreq = 1;

            string[] splitDoc = _doc.Split(' ');
            termFrequency = new Dictionary<string, int>();
            for (int i = 0; i < splitDoc.Count(); i++)
            {
                if (termFrequency.ContainsKey(splitDoc[i]))
                    termFrequency[splitDoc[i]]++;
                else
                    termFrequency.Add(splitDoc[i], 1);
            }

            foreach(int freq in termFrequency.Values)
            {
                if (freq > maxFreq)
                    maxFreq = freq;
            }
        }

        //1412503
        public int getFrequency(string word)
        {
            return termFrequency[word];
        }

        //1412503
        public int getMaxFrequency()
        {
            return maxFreq;
        }

        //1412542
        public Dictionary<string, int> getTermFreq()
        {
            return termFrequency;
        }

        //1412542
        public bool Contains(string word)
        {
            return termFrequency.ContainsKey(word);
        }
    }
}

