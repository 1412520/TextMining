using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework1
{
    public static class StringHelper
    {
        //1412520
        private static HashSet<char> marks = new HashSet<char> { ',', '.', '!', '/', '-', '=', '~', '?', ':', ';', '\'', '"', '(', ')', '[', ']', '{', '}' };

        public static string StandardizeString(string input, HashSet<string> stopWords)
        {
            var output = input.ToLower();
            output = RemoveMarksExtraSpaces(output);
            output = RemoveWordsFromString(output, stopWords);
            output = Stemming(output);
            return output;
        }

        // 1412542
        // Stemming words in the document by poster stemming alogrithm
        private static string Stemming(string doc)
        {
            var result = new StringBuilder();
            var words = doc.Split(' '); //tach ra tung word vi su dung stringbuilder hieu qua hon, xu ly truc tiep tren string chu khong tao mot ban copy
            string stemWord;
            foreach (var word in words)
            {
                stemWord = PosterStemming.stem(word);
                result = result.Append(stemWord + " ");
            }
            return result.ToString().TrimEnd();
        }


        //1412520
        public static string RemoveMarksExtraSpaces(string input, HashSet<char> markss = null)
        {
            var result = new StringBuilder();
            var lastWasSpace = input[0] == 32;
            var markList = markss ?? marks;
            for (int i = 0; i < input.Length; i++)
            {
                var character = input[i];
                if (markList.Contains(character))
                {
                    if (!lastWasSpace)
                    {
                        result.Append(' ');
                        lastWasSpace = true;
                    }
                    continue;
                }
                if ((lastWasSpace == false) || (character != 32))
                {
                    result.Append(input[i]);
                }
                lastWasSpace = character == 32;
            }
            return result.ToString().Trim();
        }

        //1412520
        //xu ly xoa dau va khoang trang thua truoc, de dua cac chuoi ve mot chuan, cac word cach deu nhau mot khoang trang, loai bo word se de dang va chinh xac hon
        //neu khong xu ly dau cau va khoang trang, cac word k co mot chuan nhat dinh, loai bo word khong chinh xac
        public static string RemoveWordsFromString(string input, HashSet<string> words)
        {
            var result = new StringBuilder();
            var inputWords = input.Split(' '); //tach ra tung word vi su dung stringbuilder hieu qua hon, xu ly truc tiep tren string chu khong tao mot ban copy
            HashSet<string> appearedWords = new HashSet<string>();
            foreach (var word in inputWords)
            {
                if (words.Contains(word))
                {
                    if (!appearedWords.Contains(word))
                        appearedWords.Add(word);
                }
                else
                    result.Append(word + ' ');
            }

            return result.ToString().TrimEnd();
        }



        //1412520
        //public static List<string> ReproduceText(List<string> input, string stopWordFile)
        //{
        //    HashSet<string> stopWords = FileIO.ReadFileIntoHashTable(stopWordFile);
        //    List<string> output = new List<string>();
        //    for (int i = 0; i < input.Count; i++)
        //    {
        //        var row = input[i];
        //        if (string.IsNullOrEmpty(row))
        //        {
        //            output.Add(row);
        //            continue;
        //        }

        //        var outputRow = StandardizeString(row, stopWords);
        //        output.Add(outputRow);
        //    }

        //    return output;
        //}

        public static List<string> ReproduceText(List<Vector> input, string stopWordFile)
        {
            HashSet<string> stopWords = FileIO.ReadFileIntoHashTable(stopWordFile);
            List<string> output = new List<string>();
            for (int i = 0; i < input.Count; i++)
            {
                var row = input[i].TextValue.RawText;
                if (string.IsNullOrEmpty(row))
                {
                    output.Add(row);
                    input[i].TextValue.Text = row;
                    continue;
                }

                var outputRow = StandardizeString(row, stopWords);
                input[i].TextValue.Text = outputRow;
                output.Add(outputRow);
            }

            return output;
        }

    }
}
