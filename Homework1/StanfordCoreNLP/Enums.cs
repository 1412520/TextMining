using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StanfordCoreNLP
{
    public enum POSMode
    {
        Left3Words,
        Bidirectional
    }

    public enum NERMode
    {
        ThreeClasses,
        FourClasses,
        SevenClasses
    }
}
