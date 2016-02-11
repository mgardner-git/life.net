using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication2
{
    class Delegator
    {

        public static Boolean doesThisMatch(String checkMe)
        {
            return checkMe.Length == 3;
        }
        public static void main (String[] args)
        {
            List<String> myList = new List<String>();
            myList = new List<String>();
            myList.Add("A");
            myList.Add("AB");
            myList.Add("ABC");
            Predicate<String> matcher = doesThisMatch;
            String match = myList.Find(matcher);
            Console.WriteLine(match);



        }

    }
}
