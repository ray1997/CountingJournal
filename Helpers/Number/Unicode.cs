using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CountingJournal.Model;
using static CountingJournal.Helpers.Constants;

namespace CountingJournal.Helpers.Number;
internal class Unicode : ICounting
{
    public int Validate(Message input, int expectedNumber)
    {
        List<char> converse = input.Content.ToArray().ToList();
        //Remove unrelated characters
        converse.RemoveAll(c => c == 55349);
        converse.RemoveAll(c => c == (char)65039);
        converse.RemoveAll(c => c == (char)8419);
        converse.RemoveAll(c => c == ' ');
        converse.RemoveAll(c => c == (char)56820);
        if (converse.Contains((char)55356) && converse.Contains((char)56820))
        {
            converse[converse.IndexOf((char)55356)] = '0';
            converse.Remove((char)56820);
        }
        for (var i = 0; i < converse.Count; i++)
        {
            if (converse[i] > 9311 && converse[i] < 9321)
            {
                converse[i] = (char)(converse[i] - 9263);
            }
            else if (converse[i] > 10101 && converse[i] < 10112)
            {
                converse[i] = (char)(converse[i] - 10053);
            }
            else if (converse[i] > 10111 && converse[i] < 10121)
            {
                converse[i] = (char)(converse[i] - 10063);
            }
            else if (converse[i] > 10121 && converse[i] < 10131)
            {
                converse[i] = (char)(converse[i] - 10073);
            }
            //Weird number
            if (converse[i] >= 57294 && converse[i] < 57304)
            {
                converse[i] = (char)(converse[i] - 57246);
            }
            else if (converse[i] >= 57304 && converse[i] < 57314)
            {
                converse[i] = (char)(converse[i] - 57256);
            }
            //Super & Sub scripts number
            else if (SuperScripts.ContainsKey(converse[i]))
            {
                converse[i] = SuperScripts[converse[i]];
            }
            else if (SubScripts.ContainsKey(converse[i]))
            {
                converse[i] = SubScripts[converse[i]];
            }
        }
        try
        {
            return int.Parse(string.Concat(converse));
        }
        catch { }
        return -1;
    }
}
