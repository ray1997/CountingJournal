using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountingJournal.Helpers.Text;
public static class Markdown
{
    public static bool HasMarkdown(string input)
    {
        System.Diagnostics.Debug.WriteLine($"Is \"{input}\" a markdown?");
        return markdownFormatChars.Any(c => input.Contains(c));
    }

    public static string ToText(string input)
    {
        var chars = input.ToCharArray().ToList();
        chars.RemoveAll(i => markdownFormatChars.Contains(i));
        return string.Concat(chars);
    }

    private static char[] markdownFormatChars = new char[] { '*', '~', '～', '|' };
}
