using CountingJournal.Model;
using static CountingJournal.Helpers.Constants;

namespace CountingJournal.Helpers.Number;
/// <summary>
/// Messages that contains number in someway
/// Either illogical formula or 2-3-6 etc.
/// </summary>
internal class Scatters : ICounting
{
    public int Validate(Message input, int expectedNumber)
    {
        var expects = expectedNumber.ToString().ToCharArray().ToList();
        var msgs = input.Content.ToCharArray().ToList();

        while (expects.Count > 0)
        {
            int index = msgs.IndexOf(expects[0]);
            if (index == -1) //Not found
                return -1;
            msgs.RemoveAt(index);
            expects.RemoveAt(0);
        }
        if (expects.Count == 0)
            return expectedNumber;
        return -1;
    }
}
