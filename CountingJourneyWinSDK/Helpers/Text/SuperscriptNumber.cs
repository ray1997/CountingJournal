using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountingJournal.Helpers.Text;
public static class TinyNumber
{
    private static Dictionary<char, char> superScripts => new()
    {
        { '¹', '1' },
        { '²', '2' },
        { '³', '3' },
        { '⁴', '4' },
        { '⁵', '5' },
        { '⁶', '6' },
        { '⁷', '7' },
        { '⁸', '8' },
        { '⁹', '9' },
        { '⁰', '0' }
    };
    private static Dictionary<char, char> subScripts => new()
    {
        { '₁', '1' },
        { '₂', '2' },
        { '₃', '3' },
        { '₄', '4' },
        { '₅', '5' },
        { '₆', '6' },
        { '₇', '7' },
        { '₈', '8' },
        { '₉', '9' },
        { '₀', '0' }
    };


    public static bool ContainTinyText(string msg) 
        => msg.Any(i => superScripts.ContainsKey(i)) || msg.Any(i => subScripts.ContainsKey(i));

    public static string ToNormal(string message)
    {
        var msg = message.ToCharArray().ToList();
        for (var i = 0; i < msg.Count; i++)
        {
            var c = msg[i];
            if (superScripts.ContainsKey(c))
                msg[i] = superScripts[c];
            else if (subScripts.ContainsKey(c))
                msg[i] = subScripts[c];
        }
        return string.Join(string.Empty, msg);
    }
}
