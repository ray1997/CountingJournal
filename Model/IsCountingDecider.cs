﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CountingJournal.Helpers;
using CountingJournal.Helpers.Text;
using static CountingJournal.Helpers.Text.Manual;

namespace CountingJournal.Model;
public static class IsCounting
{
    private static User? _lastCount = null;
    public static void ResetLastSender() => _lastCount = null;

    public static async Task<bool> Decide(int number, Message input)
    {
        //turn text into number:
        //Compare if it's more than number:
        //Return if it's correct count
        //Otherwise, nope.
        var msg = input.Content;
        var compare = -1;
        int triedRoman = -1;
        int triedThaiText = -1;
        int triedThaiNum = -1;

        if (input.Content == "Jan 10th 1946, First UN meeting (cringe 🤮)")
        {
            System.Diagnostics.Debug.Write("E");
        }

        Retry:
        try
        {
            if (IsNoise(input))
            {
                return false;
            }
            if (Equals(input.Sender, _lastCount))
            {
                //tacktor u/succcguypc: Let say it never happen
                if (input.Content == "119" && input.Sender.UserName == "Kojina")
                    goto NeverHappen;
                return false;
            }
            //Split count info
            if (TimeHelper.IsWithin(input.SendAt, 5, 6) &&
            input.SendAt.Hour == 1 && input.SendAt.Minute == 35 &&
            input.Sender.UserName == "Rews_red" && input.Content == "4")
            {
                msg = 604.ToString();
            }
            if (TimeHelper.IsWithin(input.SendAt, 5, 8) &&
                input.SendAt.Hour == 21 && input.SendAt.Minute == 11 &&
                input.Sender.UserName == "Rews_red")
            {
                //Another error happen
                //They wrote -3 and it got parse without error :/
                if (msg == "-3")
                    msg = "";
            }
        NeverHappen:
            compare = int.Parse(msg);
            if (compare > number + 1 || compare < number)
            {
                throw new FormatException();
            }
        }
        catch
        {
            System.Diagnostics.Debug.WriteLine($"Currently stuck on {msg} ({input.Content}");

            if (MemeReference.ContainsKey(msg))
            {
                msg = MemeReference[msg].ToString();
                goto Retry;
            }
            else if (msg.Contains('=')) //Predetermine formula
            {
                if (msg.EndsWith((number + 1).ToString()))
                { //2×3×41=246
                    msg = msg.Substring(msg.LastIndexOf('=') + 1);
                }
                else if (msg.StartsWith((number + 1).ToString()))
                { //250=(10×10+5×5)+(1+2+3+4+5+6+7+8+9+(5×4)+(2.5+2.5)+1
                    msg = msg.Substring(msg.IndexOf('='));
                }
                else
                {
                    if (IllogicalFormula.IsAlright(msg, number + 1))
                    {
                        msg = (number + 1).ToString();
                        goto Retry;
                    }
                    return false;
                }
                goto Retry;
            }
            else if (msg.Contains('-'))
            {
                //2-3-6
                var removal = msg.ToCharArray().ToList();
                removal.RemoveAll(c => c == '-');
                msg = string.Concat(removal);
                goto Retry;
            }
            else if (msg.Contains('^') && !msg.StartsWith('^')) //Math power equation
            {
                var baseNumber = int.Parse(msg[..msg.IndexOf('^')]);
                var power = int.Parse(msg[(msg.IndexOf('^') + 1)..]);
                msg = Math.Pow(baseNumber, power).ToString();
                goto Retry;
            }
            else if (RepeaterSymbol.HasRepeater(msg))
            {
                msg = RepeaterSymbol.Translate(msg, number + 1);
                goto Retry;
            }
            else if (ThaiNumber.ContainThaiNumber(msg))
            {
                //TH to en
                msg = ThaiNumber.ToArabic(msg);
                goto Retry;
            }
            else if (Roman.IsRoman(msg) && triedRoman < 0)
            {
                triedRoman = Roman.ToNumber(msg);
                if (triedRoman == 0)
                {
                    msg = input.Content;
                    goto Retry;
                }
                msg = triedRoman.ToString();
                goto Retry;
            }
            //Contain message afterward?
            else if (ThaiTextNumber.IsThaiText(msg) && triedThaiText < 0)
            {
                msg = ThaiTextNumber.ConvertToNumber(msg).ToString();
                if (msg == "-1")
                {
                    triedThaiText++;
                    msg = input.Content;
                    goto Retry;
                }
                goto Retry;
            }
            else if (ScientificAnnotation.IsAnnotated(msg))
            {
                msg = ScientificAnnotation.ResolveAnnotation(msg).ToString();
                goto Retry;
            }
            else if (TinyNumber.ContainTinyText(msg))
            {
                msg = TinyNumber.ToNormal(msg);
                goto Retry;
            }
            else if (NumberBall.IsNumberBall(msg))
            {
                msg = NumberBall.ToNormal(msg);
                goto Retry;
            }
            else if (WeirdNumber.IsWeird(msg))
            {
                msg = WeirdNumber.ToNormal(msg);
                goto Retry;
            }
            else if (Markdown.HasMarkdown(msg))
            {
                msg = Markdown.ToText(msg);
                goto Retry;
            }
            else if (msg.Contains((char)65039) && msg.Contains((char)8419))
            {
                //Regional emoji number
                var chars = msg.ToCharArray().ToList();
                chars.RemoveAll(c => c == (char)65039);
                chars.RemoveAll(c => c == (char)8419);
                chars.RemoveAll(c => c == ' ');
                msg = string.Concat(chars);
                goto Retry;
            }
            else if (msg.Contains(' ')) //Space removal
            {
                var msgList = msg.ToList();
                msgList.RemoveAll(c => c == ' ');
                msg = string.Concat(msgList);
                goto Retry;
            }
            //End with or start with number:
            else if (msg.EndsWith((number + 1).ToString()))
            {
                //trim
                msg = msg[(msg.Length - number.ToString().Length)..];
                goto Retry;
            }
            else if (msg.Length > number.ToString().Length && (char.IsDigit(msg[0]) && char.IsDigit(msg[1])))
            {
                //Try trim?
                msg = msg[..number.ToString().Length];
                goto Retry;
            }
            else if (msg.Contains((number + 1).ToString()))
            {
                //It's somewhere in the middle..
                msg = (number + 1).ToString(); //Don't bother..
                goto Retry;
            }
            else if (!string.IsNullOrWhiteSpace(input.Attachments))
            {
                //Attachment check
                if (ImageToNumber.ContainsKey(input.Attachments))
                    msg = ImageToNumber[input.Attachments].ToString();
                else
                {
                    if (string.IsNullOrEmpty(msg))
                    {
                        msg = await URLOCR.GetTexts(input.Attachments);
                        goto Retry;
                    }
                    return false;
                }
                goto Retry;
            }
        }
        _lastCount = input.Sender;
        return compare != -1 && (compare - 1 == number);
    }
}