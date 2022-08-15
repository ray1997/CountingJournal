using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountingJournal.Helpers.Text;
public static class BasicCalculation
{
    public static char[] calculateSymbols = new char[] { '+', '-', '*', '×', '÷', '^' };
    public static char[] additionalCalculateSymbols = new char[] { '=', '(', ')' };
    public static char[] symbols = calculateSymbols.Concat(additionalCalculateSymbols).ToArray();

    public static bool IsBasicCalculation(string input)
    {
        if (PreCalculatedFormula.ContainsKey(input))
            return true;

        if (input.Contains('.'))
            return false;

        if (input.Contains(' '))
            input = input.Replace(" ", string.Empty);

        int testing = input.IndexOfAny(calculateSymbols);
        if (testing <= 0 || !char.IsDigit(input[testing - 1]) || !char.IsDigit(input[testing + 1])) //Start with symbols or didn't actually found symbols
            return false;

        return input.Any(c => calculateSymbols.Contains(c) 
        || additionalCalculateSymbols.Contains(c));
    }

    public static string Calculate(string input)
    {
        if (input.Contains('^') && !input.StartsWith('^'))
        {
            input = TryPow(input);
            if (!IsBasicCalculation(input))
                return input;
        }
        if (PreCalculatedFormula.ContainsKey(input))
            return PreCalculatedFormula[input].ToString();

        if (input.ToLower().StartsWith("x=")
            || input.ToLower().StartsWith("x =")
            || input.ToLower().EndsWith("=x")
            || input.ToLower().EndsWith("= x"))
        {
            //Unfinished formula
            input = input.Replace('=', ' ');
            input = input.Replace('X', ' ');
            input = input.Replace('x', ' ');
            input = input.Trim();
        }
        var complexity = input.Count(c => calculateSymbols.Contains(c));
        if (complexity == 1)
        {
            var position = input.IndexOfAny(calculateSymbols);
            var first = int.Parse(input[..position].Trim());
            var second = int.Parse(input[(position + 1)..].Trim());
            switch (input[position])
            {
                case '+':
                    return (first + second).ToString();
                case '-':
                    return (first - second).ToString();
                case '*':
                case '×':
                    return (first * second).ToString();
                case '/':
                case '÷':
                    return (first / second).ToString();
            }
        }

#if DEBUG
        throw new Exception($"No formula for {input}");
#else
        return input;
#endif
    }

    private static string TryPow(string input)
    {
        var msg = input;
        var additional = 0;
        try
        {
            if (msg.Contains('+'))
            {
                //2900 + 7^2
                additional = int.Parse(msg[..msg.IndexOf('+')].Trim());
                msg = msg[(msg.IndexOf('+') + 1)..].Trim();
            }
            var baseNumber = int.Parse(msg[..msg.IndexOf('^')]);
            var power = int.Parse(msg[(msg.IndexOf('^') + 1)..]);
            msg = (additional + Math.Pow(baseNumber, power)).ToString();
            return msg;
        }
        catch
        {

        }
        return msg;
    }

    public static Dictionary<string, int> PreCalculatedFormula => new()
    {
        { "2-3-6", 236 },
        { "250=(10×10+5×5)+(1+2+3+4+5+6+7+8+9+(5×4)+(2.5+2.5)+1", 251 },
        { "5+3 isn’t equal to 3", 533 },
        { "2000+800+30+1", 2831 },
        { "2800+2^5", 2832 },
        //{ "2845+1", 2846 },
        //{ "2848-1", 2847 },
        { "X=1000+1850", 2850 },
        { "(2000×(⁴²⁰∕₄₂₀))+((20×20)×2)+(100÷(√4))+(2³)", 2858 },
        { "(41x69)+31", 2860 },
        { "(420×69)-26118", 2862 },
        { "(420×69)-26117", 2863 },
        //{ "2869+3", 2872 },
        //{ "2008+900", 2908 },
        //{ "2913+1", 2914 },
        //{ "2919 - 1", 2918 },
        //{ "2929-10", 2919 },
        //{ "2929-8", 2921 },
        { "2900 + 7^2", 2949 },
        { "(2900+7²)+1", 2950 },
        { "[69²] - ((420 × 4) + (69 + 7))", 3005 },
    };
}
