using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework1
{
    class Bow_tfidf
    {
        public void FeatureList(List<string> tienXuLy)
        {
            String fileFeatureList = "../../featureList.txt";
            FileIO file = new FileIO();
            List<string> featureList = new List<string>();
            foreach (string item0 in tienXuLy)
            {
                string[] arrListStr = item0.Split(' ');
                foreach (string item1 in arrListStr)
                {
                    bool isExists = featureList.Contains(item1);
                    if (isExists == false)
                    {
                        featureList.Add(item1);
                    }
                }
            }
            file.WriteListToFile(featureList, fileFeatureList);
        }

        //1412520
        private static HashSet<char> marks = new HashSet<char> { ',', '.', '!', '/', '-', '=', '~', '?' };

        //1412520
        public static void ReproduceText(string inputFile, string outputFile)
        {
            FileIO fileReader = new FileIO();
            List<string> input = fileReader.ReadFile(inputFile);
            HashSet<string> stopWords = fileReader.ReadFileIntoHashTable("../../stop-words.txt");
            List<string> output = new List<string>();
            foreach (var row in input)
            {
                var outputRow = row.ToLower();
                outputRow = RemoveMarksExtraSpaces(outputRow, marks);
                outputRow = RemoveWordsFromString(outputRow, stopWords);
                output.Add(outputRow);
            }
            fileReader.WriteListToFile(output, outputFile);
        }

        //1412520
        public static string RemoveMarksExtraSpaces(string input, HashSet<char> marks)
        {
            var result = new StringBuilder();
            var lastWasSpace = input[0] == 32;
            for (int i = 0; i < input.Length; i++)
            {
                var character = input[i];
                if (marks.Contains(character))
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
    }
}
