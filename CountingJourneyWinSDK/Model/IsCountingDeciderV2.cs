using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CountingJournal.Helpers.Text;
using static System.Net.Mime.MediaTypeNames;

namespace CountingJournal.Model;
public static class IsCountingDeciderV2
{
    public static bool ShouldAddIn(Message previous, Message next, int allowGapSize)
        => ShouldAddIn(previous.Content, next.Content, allowGapSize);

    public static bool ShouldAddIn(string previous, string next, int allowGapSize)
    {
        int previousNumber = -1;
        int nextNumber = -1;
        int.TryParse(previous, out previousNumber);
        int.TryParse(next, out nextNumber);
        if (nextNumber == -1)
            return false;
        if (nextNumber > previousNumber)
        {
            int gap = nextNumber - previousNumber;
            if (gap <= 0)
                return false; //Miscount?
            else if (gap <= allowGapSize)
                return true;
            else if (gap > allowGapSize)
                return false;

            //TODO:Check for acceptable gap size
            return false;
        }
        return false;
    }

    public static string PerhapsTheNumberIsMixedWithText(Message now)
    {
        var charArray = now.Content
            .Where(c => char.IsDigit(c))
            .ToArray();
        return new string(charArray);
    }

    public static string PerhapsTheNumberIsMixedWithTextAndNumber(Message now, string previous)
    {
        //Check if the next contains the next number
        int previousInt = int.Parse(previous);
        int nextInt = previousInt + 1;
        string nextStr = nextInt.ToString();
        bool[] hasAll = new bool[nextInt.ToString().Length];
        List<char> nums = now.Content.Where(c => char.IsDigit(c)).ToList();

        int lastIndex = -1;
        try
        {
            for (var i = 0; i < hasAll.Length; i++)
            {
                if (nums.IndexOf(nextStr[i], lastIndex) > 0)
                {
                    int index = nums.IndexOf(nextStr[i], lastIndex);
                    lastIndex = index;
                    nums.RemoveAt(index);
                    hasAll[i] = true;
                }
            }
        }
        catch
        {
            return string.Empty;
        }
        var collapsed = hasAll.Distinct().ToList();
        if (collapsed.Count > 1)
            return string.Empty;
        else if (collapsed[0] == false)
            return string.Empty;
        return nextStr;
    }

    public static string PerhapsThereIsThaiNumber(Message now)
    {
        var text = now.Content.ToCharArray();
        for (var i = 0; i < text.Length; i++)
        {
            if (ThaiNumber.THtoEN.ContainsKey(text[i]))
            {
                text[i] = ThaiNumber.THtoEN[text[i]];
            }
        }
        return new(text);
    }
}