using CountingJournal.Model;
using static CountingJournal.Helpers.Constants;

namespace CountingJournal.Helpers.Number;
internal class Calculation : ICounting
{
    public int Validate(Message msg, int expect)
    {
        //Finished formula
        var input = msg.Content;
        string expectedNumber = expect.ToString();
        if (input.EndsWith(expectedNumber))
            return expect;
        else if (input.StartsWith(expectedNumber))
            return expect;

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

        var complexity = input.Count(c => Calculations.Contains(c));
        if (complexity == 1)
        {
            var position = input.IndexOfAny(Calculations);
            var first = int.Parse(input[..position].Trim());
            var second = int.Parse(input[(position + 1)..].Trim());
            switch (input[position])
            {
                case '+':
                    return first + second;
                case '-':
                    return first - second;
                case '*':
                case '×':
                    return first * second;
                case '/':
                case '÷':
                    return first / second;
            }
        }
        return -1;
    }
}