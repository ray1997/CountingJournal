using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountingJournal.Helpers.Text;
public static class Roman 
{
    private static char[] romans => new char[] { 'I', 'V', 'X', 'C', 'D', 'M' };

    public static bool IsRoman(string input) => input.Any(i => romans.Contains(i));

    public static int ToNumber(string msg)
    {
        RomanNumerals.RomanNumeral.TryParse(msg, out var romanNumerals);
        return romanNumerals;
    }
}
