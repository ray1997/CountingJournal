using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountingJournal.Helpers.Text;
public static class ThaiNumber
{

    public static string ToArabic(string message)
    {
        var msg = message.ToCharArray().ToList();
        for (var i = 0; i < msg.Count; i++)
        {
            var c = msg[i];
            if (THtoEN.ContainsKey(c))
            {
                msg[i] = THtoEN[c];
            }
        }
        return string.Join(string.Empty, msg);
    }

    public static bool ContainThaiNumber(string msg)
    {
        return msg.Any(i => THtoEN.ContainsKey(i));
    }

    public static Dictionary<char, char> THtoEN => new()
            {
                { '๐', '0' },
                { '๑', '1' },
                { '๒', '2' },
                { '๓', '3' },
                { '๔', '4' },
                { '๕', '5' },
                { '๖', '6' },
                { '๗', '7' },
                { '๘', '8' },
                { '๙', '9' }
            };

}