using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CountingJournal.Model;
using static CountingJournal.Helpers.Constants;

namespace CountingJournal.Helpers.Number;
public class Roman : ICounting
{
    public int Validate(Message input, int expectedNumber)
    {
        foreach (var c in RomanNumeralSymbol)
        {
            if (!input.Content.Contains(c))
                return -1;
        }
        RomanNumerals.RomanNumeral.TryParse(input.Content, out int output);
        return output;
    }
}
