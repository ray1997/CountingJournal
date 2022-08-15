using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CountingJournal.Model;
using static CountingJournal.Helpers.Constants;

namespace CountingJournal.Helpers.Number;
internal class Yamok : ICounting
{
    public int Validate(Message input, int expectedNumber)
    {
        if (!input.Content.Contains(Repeater))
            return -1;
        List<char> inputList = input.Content.ToCharArray().ToList();
        if (expectedNumber.ToString().Length > inputList.Count)
        {
            //Specific case of 14ๆ & 15ๆ 
            inputList.Remove(Repeater);
            inputList.AddRange(inputList);
            try
            {
                return int.Parse(string.Concat(inputList));
            }
            catch { }
        }
        for (var i = 0; i < inputList.Count; i++)
        {
            if (inputList[i] == Repeater)
            {
                inputList[i] = inputList[i - 1];
            }
        }
        try
        {
            return int.Parse(string.Concat(inputList));
        }
        catch { }
        return -1;
    }
}
