using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CountingJournal.Helpers.Text;
using CountingJournal.Model;

namespace CountingJournal.Helpers.Number;
internal class Scientific : ICounting
{
    public int Validate(Message input, int expectedNumber)
    {
        string pattern = @"(\d+.\d+) (\*|×) (\d+)(\D+)";

        var match = Regex.Match(input.Content, pattern);
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
