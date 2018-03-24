using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework1
{
    public class Vector
    {
        public List<float> Value { get; set; }

        public Vector(List<float> value)
        {
            Value = new List<float>(value);
        }

        public double GetSimilarityMeasure(Vector vector)
        {
            if (Value.Count != vector.Value.Count)
                return 0;
            double squareSum = 0;
            for (int i=0; i<Value.Count; i++)
            {
                squareSum += Math.Pow((Value[i] - vector.Value[i]), 2);
            }
            return Math.Sqrt(squareSum);
        }

        public Dictionary<int, double> GetListSimilarityMeasure (List<Vector> listV, int quantity)
        {
            if(listV.Count < quantity)
            {
                return null;
            }
            else
            {
                Dictionary<int, double> rs = new Dictionary<int, double>(quantity);
                for (int i = 0; i < listV.Count(); i++)
                {
                    rs.Add(i, GetSimilarityMeasure(listV[i]));
                }
                rs = rs.OrderBy(key => key.Value).Take(quantity).ToDictionary(x => x.Key, x => x.Value);
                return rs;
            }
            
        }
    }
}
