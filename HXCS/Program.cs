using HXCS.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HXCS
{
    class Program
    {
        enum asd
        {
            a = 1,
            b = 2,
            c = 3
        }
        static void Main(string[] args)
        {
            HXCS.Base.HXCS main = new HXCS.Base.HXCS
                (
                    typeof(Iops),
                    typeof(SecondTable),
                     typeof(Oji)
                );
            string str = main.InitTable();

            asd u = (asd)2;
            int c = (int)u;

            SecondTable st = main.ExecuteSelect<SecondTable>();

            Console.WriteLine();

            Console.WriteLine(u.ToString());
            Console.WriteLine(c);

            Console.WriteLine(str);
            Console.Read();
        }

        
    }
}
