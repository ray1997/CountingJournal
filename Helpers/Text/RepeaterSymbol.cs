using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountingJournal.Helpers.Text;
public static class RepeaterSymbol
{
    public static bool HasRepeater(string input)
    {
        if (input.Contains(Repeater))
            return true;
        return false;
    }

    public const char Repeater = 'ๆ';

    public static string Translate(string input, int expectNumber)
    {
        List<char> inputList = input.ToCharArray().ToList();
        if (expectNumber.ToString().Length > inputList.Count)
        {
            //Specific case of 14ๆ & 15ๆ 
            inputList.Remove(Repeater);
            inputList.AddRange(inputList);
            return string.Concat(inputList);
        }
        for (var i = 0; i < inputList.Count; i++)
        {
            if (inputList[i] == Repeater)
            {
                inputList[i] = inputList[i - 1];
            }
        }
        return string.Concat(inputList);
    }
}
