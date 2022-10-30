using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountingJournal.Helpers.Text;
public static class Roman 
{
    private static char[] romans => new char[] { 'I', 'V', 'X', 'C', 'D', 'M', 'L' };

    public static bool IsRoman(string input)// => input.Any(i => romans.Contains(i));
    {
        if (string.IsNullOrEmpty(input))
            return false;
        foreach (char c in input)
        {
            if (!romans.Contains(c))
                return false;
        }
        return true;
    }

    public static int ToNumber(string msg)
    {
        var attempt = RomanNumerals.RomanNumeral.TryParse(msg, out var romanNumerals);
        return attempt ? romanNumerals : -1;
    }
}
