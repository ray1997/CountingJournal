using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CountingJournal.Helpers.Text;
public static class ScientificAnnotation
{
    private static char[] multiplySymbols = new char[] { '×', '*' };

    public static bool IsAnnotated(string input)
    {
        if (input.StartsWith('*') || input.EndsWith('*'))
            return false;
        if (!input.Contains('.'))
            return false;
        return input.Any(i => multiplySymbols.Contains(i));
    }

    public static int ResolveAnnotation(string input)
    {
        //5.73 × 10²
        string pattern = @"(\d+.\d+) (\*|×) (\d+)(\D+)";
        
        var match = Regex.Match(input, pattern);
        try
        {
            float initial = float.Parse(match.Groups[1].ValueSpan);
            float multiply = float.Parse(match.Groups[3].ValueSpan);
            float power = float.Parse(TinyNumber.ToNormal(match.Groups[4].Value));
            return Convert.ToInt32(initial * Math.Pow(multiply, power));
        }
        catch
        {

        }
        return -1;
    }
}
