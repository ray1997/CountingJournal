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
        float power = 0;
        float initial = 0;
        float multiply = 0;
        //0.3935×10^4
        if (input.Contains('^'))
        {
            int index = input.LastIndexOf('^');
            int multiplyIndex = input.IndexOfAny(multiplySymbols);
            power = float.Parse(input.Substring(index + 1));
            input = input.Substring(0, index);
            initial = float.Parse(input.Substring(0, multiplyIndex));
            multiply = float.Parse(input.Substring(multiplyIndex + 1));
            return Convert.ToInt32(initial * Math.Pow(multiply, power));
        }
        //5.73 × 10²
        string pattern = @"(\d+.\d+) (\*|×) (\d+)(\D+)";
        
        var match = Regex.Match(input, pattern);
        try
        {
            initial = float.Parse(match.Groups[1].ValueSpan);
            multiply = float.Parse(match.Groups[3].ValueSpan);
            power = float.Parse(TinyNumber.ToNormal(match.Groups[4].Value));
            return Convert.ToInt32(initial * Math.Pow(multiply, power));
        }
        catch
        {

        }
        return -1;
    }
}
