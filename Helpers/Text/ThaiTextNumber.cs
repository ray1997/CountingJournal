using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountingJournal.Helpers.Text;
public static class ThaiTextNumber
{
    public static int ConvertToNumber(string input)
    {
        var workingInput = input;
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
                if (numbers.ContainsKey(workingInput[..i]))
                {
                    foundedNumber = numbers[workingInput[..i]];
                    number = !number;
                    workingInput = workingInput[i..];
                    goto startOver;
                }
                else
                    continue;
            }
            else //Multiply
            {
                if (multiply.ContainsKey(workingInput[..i]))
                {
                    value += (foundedNumber * multiply[workingInput[..i]]);
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

    public static bool IsThaiText(string input)
    {
        if (numbers.Keys.Any(i => input.Contains(i)))
        {
            return true;
        }
        return false;
    }

    private static Dictionary<string, int> numbers => new()
    {
        { "หนึ่ง", 1},
        { "สอง", 2},
        { "สาม", 3},
        { "สี่", 4},
        { "ห้า", 5},
        { "หก", 6},
        { "เจ็ด", 7},
        { "แปด", 8},
        { "เก้า", 9}
        };


    private static Dictionary<string, int> multiply => new()
    {
        { "สิบ", 10},
        { "ร้อย", 100},
        { "พัน", 1000},
        { "หมื่น", 10000},
        { "แสน", 100000},
        { "ล้าน", 1000000},
    };

}