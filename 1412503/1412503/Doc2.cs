using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1412503
{
    //1412503
    class Doc2
    {
        //Hàm lấy số lần xuất hiện của word trong văn bản doc
        public static int countWordInDoc(string word, string doc)
        {
            int count = 0;
            string[] splitString = doc.Split(' ');
            for (int i = 0; i < splitString.Count(); i++)
            { 
                if(word.CompareTo(splitString[i]) == 0)
                {
                    count++;
                }
            }
            return count;
        }

        //Hàm lấy số lần xuất hiện của từ có số lân xuất hiện nhiều nhất trong văn bản doc
        public static int maxCountInDoc(string doc)
        {
            int max = -1;
            List<string> feature = new List<string>();
            string[] splitString = doc.Split(' ');
            for (int i = 0; i < splitString.Count(); i++)
            {
                if (!feature.Contains(splitString[i])){
                    int temp = countWordInDoc(splitString[i], doc);
                    if (temp > max)
                    {
                        max = temp;
                    }
                }
            }
            return max;
        }
    }
}
