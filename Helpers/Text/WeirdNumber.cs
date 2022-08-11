using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountingJournal.Helpers.Text;
public static class WeirdNumber
{
    public static bool IsWeird(string input)
    {
        if (input.Contains((char)55349))
            return true;
        return false;
    }

    public static string ToNormal(string input)
    {
        List<char> converse = input.ToArray().ToList();
        converse.RemoveAll(c => c == 55349);
        for (var i = 0; i < converse.Count; i++) 
        {
            if (converse[i] >= 57294 && converse[i] < 57304)
            {
                converse[i] = (char)(converse[i] - 57246);
            }
            else if (converse[i] >= 57304 && converse[i] < 57314)
            {
                converse[i] = (char)(converse[i] - 57256);
            }
        }
        return string.Concat(converse);
    }
}