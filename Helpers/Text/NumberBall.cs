using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountingJournal.Helpers.Text;
public static class NumberBall
{
    public static bool IsNumberBall(string input)
    {
        if (input.Any(c => c > 9311 && c < 9321))
        {
            return true;
        }
        else if (input.Any(c => c > 10101 && c < 10112))
        {
            return true;
        }
        else if (input.Any(c => c > 10111 && c < 10121))
        {
            return true;
        }
        else if (input.Any(c => c > 10121 && c < 10131))
        {
            return true;
        }
        return false;
    }

    public static string ToNormal(string input)
    {
        List<char> converse = input.ToArray().ToList();
        for (var i = 0; i < converse.Count; i++) 
        {
            if (converse[i] > 9311 && converse[i] < 9321)
            {
                converse[i] = (char)(converse[i] - 9263);
            }
            else if (converse[i] > 10101 && converse[i] < 10112)
            {
                converse[i] = (char)(converse[i] - 10053);
            }
            else if (converse[i] > 10111 && converse[i] < 10121)
            {
                converse[i] = (char)(converse[i] - 10063);
            }
            else if (converse[i] > 10121 && converse[i] < 10131)
            {
                converse[i] = (char)(converse[i] - 10073);
            }
        }
        return string.Concat(converse);
    }
}