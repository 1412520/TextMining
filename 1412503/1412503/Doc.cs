using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1412503
{
    //1412503
    class Doc
    {
        private List<Dictionary<String, int>> docs; //list feature - số lần xuất hiện ứng với từng văn bản (d)
        private List<int> max_count;                //list số lần xuất hiện của từ (word) có số lần xuất hiện nhiều nhất ứng với từng văn bản (d)
        public Doc(List<String> input)
        {
            max_count = new List<int>(input.Count);
            docs = new List<Dictionary<string, int>>(input.Count);
            for (int i = 0; i < input.Count; i++)
            {
                max_count.Add(-1);
                string[] splitItems = input[i].Split(' ');
                Dictionary<String, int> ret = new Dictionary<string, int>();
                for (int j = 0; j < splitItems.Count(); j++)
                {
                    int count;
                    try
                    {
                        count = ++ret[splitItems[j]];
                    }catch(Exception e)
                    {
                        count = 1;
                    }
                    if (count > max_count[i])
                    {
                        max_count[i] = count;
                    }
                    ret[splitItems[j]] = count;
                }
                docs.Add(ret);
            }
        }

        //Hàm lấy số lần xuất hiện của word trong văn bản thứ docOffset
        public int countWordInDoc(string word, int docOffset)
        {
            return docs[docOffset][word];
        }

        //Hàm lấy số lần xuất hiện của từ có số lân xuất hiện nhiều nhất trong văn bản thứ docOffset
        public int maxCountInDoc(int docOffset)
        {
            return max_count[docOffset];
        }
    }
}
