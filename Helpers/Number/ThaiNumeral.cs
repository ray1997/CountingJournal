using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CountingJournal.Model;
using static CountingJournal.Helpers.Constants;

namespace CountingJournal.Helpers.Number;
public class ThaiNumeral : ICounting
{
    public int Validate(Message input, int expectedNumber)
    {
        string output = input.Content;
        foreach (var c in ThaiToArabic)
        {
            output = output.Replace(c.Key, c.Value);
        }
        try
        {
            return int.Parse(output);
        }
        catch { }
        return -1;
    }
}
