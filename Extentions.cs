using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SharpDdos
{
    internal static class Extentions
    {
        internal static List<Target> RemoveDuplicates(this List<Target> newList, List<Target> historyList)
        {
            var listWithoutDuplicates = new List<Target>();

            if (historyList == null || historyList == null)
            {
                return listWithoutDuplicates;
            }

            foreach (var item in newList)
            {
                if (!historyList.ContainsTarget(item))
                {
                    listWithoutDuplicates.Add(item);
                }
            }            
           
            return listWithoutDuplicates;
        }

        internal static bool ContainsTarget(this List<Target> listToCheck, Target compareObject)
        {
            foreach (var compareObject1 in listToCheck)
            {
                if (compareObject1.Method == compareObject.Method &&
                compareObject1.IpAddress == compareObject.IpAddress &&
                compareObject1.Port == compareObject.Port)
                {
                    return true;
                }
            }

            return false;
        }

        internal static string ReplaceWhitespace(this string input)
        {
            return string.Join("", input.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
        }
    }
}
