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
    }
}
