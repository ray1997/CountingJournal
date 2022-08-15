using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CountingJournal.Model;
using static CountingJournal.Helpers.Constants;

namespace CountingJournal.Helpers.Number;
public class TextNumber : ICounting
{
    public int Validate(Message msg, int expectedNumber)
    {
        if (!ThaiTextNumberMultiply.Keys.Any(key => msg.Content.Contains(key)))
        {
            return NumberOnly(msg.Content);
        }

        var workingInput = msg.Content;
        var number = true;
        var foundedNumber = 0;
        var value = 0;
    startOver:
        for (var i = 0; i <= workingInput.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(workingInput))
            {
                value += foundedNumber;
                return value;
            }
            if (number)
            {
                if (ThaiTextNumber.ContainsKey(workingInput[..i]))
                {
                    foundedNumber = ThaiTextNumber[workingInput[..i]];
                    number = !number;
                    workingInput = workingInput[i..];
                    goto startOver;
                }
                else
                    continue;
            }
            else //Multiply
            {
                if (ThaiTextNumberMultiply.ContainsKey(workingInput[..i]))
                {
                    value += (foundedNumber * ThaiTextNumberMultiply[workingInput[..i]]);
                    number = !number;
                    workingInput = workingInput[i..];
                    foundedNumber = 0;
                    goto startOver;
                }
                else
                    continue;
            }
        }
        return -1;
    }

    public int NumberOnly(string originalMsg)
    {
        string input = originalMsg;
        foreach (var key in ThaiTextNumber.Keys)
        {
            input = input.Replace(key.ToString(), ThaiTextNumber[key].ToString());
        }
        return int.Parse(input);
    }
}
