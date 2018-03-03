using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1412503
{
    class test
    {
        public static void Main()
        {
            List<String> input = new List<string>();
            input.Add("i like dog cat man girl boy school women tree like air hand some child adult people person green red blue yellow orange gray black pink white light hurt love facebook fuck hell anime film haha hoho hihi hehe hic huhu");
            //input.Add("i like dog mary wea eagraeaew wgga sf qwr geg esehserh grege gewg gewgew gergew gregerg gerge rgerg gerge  gerhe wfeg gerger egrerg gergerg gerg gege gdger wgfwef hthrth fgewgw wefwegf wfwgw egrgerg wgwegew gewger gerge geger vs plgelgoe gple pgle goe g lgp fdlog lfdo god golfd ogkd foigkofdlgp ldf og fdogk ofdkgo ldo gki j dumgikd sfsdf sfsgs gssgsdugh7jsdjg ukugjsgukgsug ugjud8 fgufdkg udf mugd ngu dmguj fdmgu dfnguh dgu dmgu dfmgu ndug mfdg mij mgud mgidm gid ig djugj duj awawefg like dog i i i mary dfs sfd gdfg dsfsg gsgs gsdgfs gsgsdg dsgs sgsdg dsgsdg sgsdg sgsg sgs like dog i thang cho like dog i thang cho v v like dog i thang cho like dog i thang cho like dog i thang cho like dog i thang cho like dog i thang cho v v like dog i thang cho like dog i thang cho like dog i thang cho like dog i thang cho like dog i thang cho v v like dog i thang cho like dog i thang cho like dog i thang cho like dog i thang cho like dog i thang cho v v like dog i thang cho like dog i thang cho like dog i thang cho like dog i thang cho like dog i thang cho v v like dog i tha");
           
            //Cách 1
            Doc docHandler = new Doc(input);
            Console.WriteLine(docHandler.countWordInDoc("i", 0));
            Console.WriteLine(docHandler.countWordInDoc("like", 0));
            Console.WriteLine("max_word: {0}", docHandler.maxCountInDoc(0));
            
            //Cách 2
            Console.WriteLine(Doc2.countWordInDoc("i", input[0]));
            Console.WriteLine(Doc2.countWordInDoc("like", input[0]));
            Console.WriteLine("max_word: {0} ", Doc2.maxCountInDoc(input[0]));

            Console.ReadLine();


        }
    }
}
