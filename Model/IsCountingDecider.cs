using System;
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
    public static User? PreviousCounter = null;
    public static void ResetLastSender() => PreviousCounter = null;

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

        
        Retry:
        try
        {
            if (IsNoise(input))
            {
                return false;
            }
            if (Equals(input.Sender, PreviousCounter))
            {
                //tacktor u/succcguypc: Let say it never happen
                if (input.Content == "119" && input.Sender.UserName == "Kojina")
                    goto NeverHappen;
                return false;
            }

            //Split count info
            if (TimeHelper.IsWithin(input.SendAt, 5, 6) &&
                TimeHelper.IsAt(input.SendAt, 1, 35, 24) &&
                input.Sender.UserName == "Rews_red" && input.Content == "4")
                msg = 604.ToString();
            else if (TimeHelper.IsWithin(input.SendAt, 5, 25) &&
                TimeHelper.IsAt(input.SendAt, 20, 43, 12) &&
                input.Sender.UserName == "Rews_red" && input.Content == "3")
                msg = 2773.ToString();
            else if (TimeHelper.IsWithin(input.SendAt, 7, 4) && TimeHelper.IsAt(input.SendAt, 12, 2, 42) &&
                input.Content == "+1" && input.Sender.UserName == "Rews_red")
                msg = 3753.ToString();
            else if (TimeHelper.IsWithin(input.SendAt, 7, 4) && TimeHelper.IsAt(input.SendAt, 12, 19, 15) &&
                input.Content == "+2" && input.Sender.UserName == "tacktor")
                msg = 3754.ToString();

                if (TimeHelper.IsWithin(input.SendAt, 5, 8) &&
                input.SendAt.Hour == 21 && input.SendAt.Minute == 11 &&
                input.Sender.UserName == "Rews_red")
            {
                //Another error happen
                //They wrote -3 and it got parse without error :/
                if (msg == "-3")
                    msg = "";
            }
            else if (TimeHelper.IsWithin(input.SendAt, 5, 21) &&
                TimeHelper.Between(input.SendAt, "12:50:00", "13:07:25"))
            {
                if (input.Sender.UserName == "BackScrasher" && input.Content == "2636")
                    goto NeverHappen;
                if (input.Sender.UserName == "Aekkawin" && input.Content == "2738")
                {
                    msg = input.Content.Replace('7', '6');
                    goto NeverHappen;
                }
                msg = input.Content.Replace('9', '6');
                //The 6 9 incident
                /*
                 * "267230094395703297","Rews_red#9505","21-May-22 11:30:20 AM","2630","",""
                 * "368658808051990540","ggguy#3542","21-May-22 12:57:02 PM","2931","",""
                 * "807952989623943189","BackScrasher#4282","21-May-22 01:06:12 PM","2932","",""
                 * "526792117385953312","Kojina#1082","21-May-22 01:06:31 PM","2933","",""
                 * "807952989623943189","BackScrasher#4282","21-May-22 01:06:49 PM","2934","",""
                 * "746178884323639387","Aekkawin#8587","21-May-22 01:06:54 PM","2935","",""
                 * "807952989623943189","BackScrasher#4282","21-May-22 01:07:12 PM","2636","","" -_-'
                 * "526792117385953312","Kojina#1082","21-May-22 01:07:17 PM","2937","",""
                 * "746178884323639387","Aekkawin#8587","21-May-22 01:07:23 PM","2738","","" B R U H
                 * "807952989623943189","BackScrasher#4282","21-May-22 01:07:27 PM","2639","",""
                 */
            }
            //Small mistake
            if (input.Sender.UserName == "tacktor" && input.Content == "2473")
                msg = 2743.ToString();
            if (input.Content == "28" && input.Attachments == "https://cdn.discordapp.com/attachments/969212093213573140/981555605649117264/unknown.png")
                msg = 2869.ToString();
            if ((input.Content == "8290" || input.Content == "8292") &&
                input.Sender.UserName == "Rews_red")
                msg = input.Content.Replace("82", "32");

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
            else if (Scatter.IsAlright(msg, number + 1))
            {
                msg = (number + 1).ToString();
                goto Retry;
            }
            else if (BasicCalculation.IsBasicCalculation(msg)) //Predetermine formula
            {
                msg = BasicCalculation.Calculate(msg, (number + 1).ToString());
                //Before calculate, check if it's already calculate
                //if (msg.EndsWith((number + 1).ToString()))
                //{ //2×3×41=246
                //    msg = msg.Substring(msg.LastIndexOf('=') + 1);
                //}
                //else if (msg.StartsWith((number + 1).ToString()))
                //{ //250=(10×10+5×5)+(1+2+3+4+5+6+7+8+9+(5×4)+(2.5+2.5)+1
                //    msg = msg.Substring(msg.IndexOf('='));
                //}
                //else
                //{
                //    goto Retry;
                //}
                goto Retry;
            }
            else if (msg.Contains('-') && !msg.Contains('='))
            {
                //2-3-6
                var removal = msg.ToCharArray().ToList();
                removal.RemoveAll(c => c == '-');
                msg = string.Concat(removal);
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
                for (var i = 0; i < chars.Count; i++)
                {
                    if (chars[i] == "🇴"[0])
                    {
                        chars[i] = '0';
                    }
                }
                chars.RemoveAll(c => c == (char)56820);
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
            else if (msg.StartsWith((number + 1).ToString()))
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
            else if (HasAllNumberScattered(msg, number + 1))
            {
                //It's scatter in the text
                msg = (number + 1).ToString();
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
                        if (string.IsNullOrEmpty(msg))
                            throw new Exception($"The image: {input.Attachments} isn't indexed yet");
                        goto Retry;
                    }
                    return false;
                }
                goto Retry;
            }
        }
        PreviousCounter = input.Sender;
        return compare != -1 && (compare - 1 == number);
    }

    private static bool HasAllNumberScattered(string msg, int v)
    {
        var txt = v.ToString();
        foreach (var c in txt)
        {
            if (!msg.Contains(c))
                return false;
        }
        return true;
    }
}
