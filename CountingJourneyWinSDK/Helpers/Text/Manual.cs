using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CountingJournal.Model;
using static CountingJournal.Helpers.TimeHelper;
using CountingJournal.Helpers;

namespace CountingJournal.Helpers.Text;
public static class Manual
{
    public static bool IsNoise(Message input)
    {
        if (input.IsItFiller().Item1)
            return true;

        if (input.SendOn(5, 1) && input.SendBetween("14:45:00", "14:49:00"))
        {
            //Pro gamer move incident
            /* "267230094395703297","Rews_red#9505","01-May-22 02:46:53 PM","where tf is 184 then","",""
             * "437618453155938317","tacktor#9598","01-May-22 02:47:49 PM","Pro gamer move","",""
             * "267230094395703297","Rews_red#9505","01-May-22 02:47:53 PM","..?","",""
             * "437618453155938317","tacktor#9598","01-May-22 02:47:55 PM","190","","" //Keep
             * "437618453155938317","tacktor#9598","01-May-22 02:48:09 PM","Check again","",""
             * "267230094395703297","Rews_red#9505","01-May-22 02:48:26 PM","","https://cdn.discordapp.com/attachments/969212093213573140/970230203945222174/unknown.png","" //Keep
             * "267230094395703297","Rews_red#9505","01-May-22 02:48:34 PM","edited bruh","",""
             * "437618453155938317","tacktor#9598","01-May-22 02:48:51 PM","I know","",""
             * "526792117385953312","Kojina#1082","01-May-22 03:42:55 PM","192","","" //Keep */
            if (input.Content == "190")
                return false;
            if (input.Attachments == "https://cdn.discordapp.com/attachments/969212093213573140/970230203945222174/unknown.png")
                return false;
            return true;
        }
        else if (input.SendOn(5, 2) && input.SendBetween("19:42:40", "19:45:03"))
        {
            /* "437618453155938317","tacktor#9598","02-May-22 07:42:50 PM","245","",""
             * "267230094395703297","Rews_red#9505","02-May-22 07:43:36 PM","246","",""
             * "437618453155938317","tacktor#9598","02-May-22 07:43:44 PM","247","",""
             * "267230094395703297","Rews_red#9505","02-May-22 07:44:14 PM","248","",""
             * "267230094395703297","Rews_red#9505","02-May-22 07:44:19 PM","Wiat","",""
             * "267230094395703297","Rews_red#9505","02-May-22 07:44:22 PM","Wait","",""
             * "437618453155938317","tacktor#9598","02-May-22 07:44:30 PM","Yes?","",""
             * "267230094395703297","Rews_red#9505","02-May-22 07:44:35 PM","Didn't we started at 274","",""
             * "267230094395703297","Rews_red#9505","02-May-22 07:44:38 PM","💀","",""
             * "437618453155938317","tacktor#9598","02-May-22 07:45:00 PM","Welp","",""
             * "437618453155938317","tacktor#9598","02-May-22 07:45:05 PM","275","",""
             * "267230094395703297","Rews_red#9505","02-May-22 07:45:10 PM","276","","" */
            //Long miscount incident #1 273 > 243-248
            return true;
        }
        else if (input.SendOn(5, 4) && input.SendBetween("01:02:00", "01:02:30"))
        {
            //Serveral mistake send
            /* "807952989623943189","BackScrasher#4282","04-May-22 01:01:39 AM","399","","" //Keep
             * "267230094395703297","Rews_red#9505","04-May-22 01:01:46 AM","4️⃣ 🇴 🅾️","","" //Keep
             * "267230094395703297","Rews_red#9505","04-May-22 01:02:12 AM","","https://cdn.discordapp.com/attachments/969212093213573140/971109438939340861/unknown.png",""
             * "267230094395703297","Rews_red#9505","04-May-22 01:02:15 AM","(4x100)","",""
             * "267230094395703297","Rews_red#9505","04-May-22 01:02:18 AM","401","",""
             * "267230094395703297","Rews_red#9505","04-May-22 01:02:28 AM","wait","",""
             * "807952989623943189","BackScrasher#4282","04-May-22 01:02:34 AM","401 Unauthorized","","" //Keep
             * "267230094395703297","Rews_red#9505","04-May-22 01:02:35 AM","you say 401 is correcy","",""
             * "267230094395703297","Rews_red#9505","04-May-22 01:02:37 AM","402","","" //Keep
             */
            return true;
        }
        else if (input.SendOn(5, 6) && input.SendBetween("02:03:55", "02:06:10"))
        {
            //Little intermission before continue on 1001
            /*
             * "526792117385953312","Kojina#1082","06-May-22 02:03:28 AM","998","",""
             * "807952989623943189","BackScrasher#4282","06-May-22 02:03:31 AM","","https://cdn.discordapp.com/attachments/969212093213573140/971849642880958564/IMG_2868.jpg",""
             * "526792117385953312","Kojina#1082","06-May-22 02:03:50 AM","","https://cdn.discordapp.com/attachments/969212093213573140/971849722035863612/unknown.png",""
             * Begin intermission:
             * "526792117385953312","Kojina#1082","06-May-22 02:04:00 AM","ok gn","",""
             * "807952989623943189","BackScrasher#4282","06-May-22 02:04:08 AM","GN :ThaksinThumbsUp: :ThaksinThumbsUp:","",""
             * "526792117385953312","Kojina#1082","06-May-22 02:05:03 AM",")618 to 1000 in 20 minutes tho, real fast","",""
             * "807952989623943189","BackScrasher#4282","06-May-22 02:05:50 AM","If we kept going we would be done at 10am","",""
             * "526792117385953312","Kojina#1082","06-May-22 02:05:56 AM","hmm","",""
             * "807952989623943189","BackScrasher#4282","06-May-22 02:06:02 AM","no","",""
             * "526792117385953312","Kojina#1082","06-May-22 02:06:05 AM","no","",""
             * End intermission:
             * "807952989623943189","BackScrasher#4282","06-May-22 02:06:13 AM","1001 no","","" //Continue counting
             * "526792117385953312","Kojina#1082","06-May-22 02:06:23 AM","1002 yes","",""
             */
            return true;
        }
        else if (input.SendOn(5, 6) && input.SendBetween("02:17:28", "02:18:30"))
        {
            /*
             * "437618453155938317","tacktor#9598","06-May-22 02:17:18 AM","1038","","" //Keep
             * "807952989623943189","BackScrasher#4282","06-May-22 02:17:26 AM","1039 why am I always odd","","" //Keep
             * "437618453155938317","tacktor#9598","06-May-22 02:17:29 AM","1039","","" //Ignore
             * "807952989623943189","BackScrasher#4282","06-May-22 02:17:31 AM","1041","","" //Ignore
             * "437618453155938317","tacktor#9598","06-May-22 02:17:32 AM","1040","","" //Ignore
             * "807952989623943189","BackScrasher#4282","06-May-22 02:17:48 AM","Wait","","" 
             * "807952989623943189","BackScrasher#4282","06-May-22 02:17:51 AM","Who messed up","",""
             * "437618453155938317","tacktor#9598","06-May-22 02:18:08 AM","Me so","",""
             * "437618453155938317","tacktor#9598","06-May-22 02:18:35 AM","1040","","" //Keep
             */
            return true;
        }
        if (input.SendOn(5, 8) && input.SendBetween("22:52:56", "22:54:50"))
        {
            //The 1350 incident
            /*
             * "807952989623943189","BackScrasher#4282","08-May-22 10:52:56 PM","1349","",""
             * "250308103105413121","Toon#9209","08-May-22 10:52:57 PM","1350","","" // Keep
             * 
             * "267230094395703297","Rews_red#9505","08-May-22 10:52:57 PM","1350","","" 
             * "807952989623943189","BackScrasher#4282","08-May-22 10:53:03 PM","1350","",""
             * "250308103105413121","Toon#9209","08-May-22 10:53:08 PM","1350","",""
             * "267230094395703297","Rews_red#9505","08-May-22 10:53:12 PM","1350","",""
             * "807952989623943189","BackScrasher#4282","08-May-22 10:53:13 PM","1350","",""
             * "250308103105413121","Toon#9209","08-May-22 10:53:37 PM","1350?","",""
             * "267230094395703297","Rews_red#9505","08-May-22 10:53:41 PM","1350!","",""
             * "807952989623943189","BackScrasher#4282","08-May-22 10:53:45 PM","1350....","",""
             * "267230094395703297","Rews_red#9505","08-May-22 10:54:07 PM","135....1?","","" //Keep
             * "250308103105413121","Toon#9209","08-May-22 10:54:13 PM","1360","",""
             * "807952989623943189","BackScrasher#4282","08-May-22 10:54:18 PM","1","",""
             * "250308103105413121","Toon#9209","08-May-22 10:54:29 PM","1362","",""
             * "267230094395703297","Rews_red#9505","08-May-22 10:54:32 PM","​","",""
             * "250308103105413121","Toon#9209","08-May-22 10:54:38 PM","1364","",""
             * "807952989623943189","BackScrasher#4282","08-May-22 10:54:39 PM","1365","",""
             * "267230094395703297","Rews_red#9505","08-May-22 10:54:46 PM","where tf is 1352","","" 
             * 
             * "807952989623943189","BackScrasher#4282","08-May-22 10:54:55 PM","1352 here","","" //Keep and continue
             */
            if (input.Sender.UserName == "Toon"
                && input.Content == "1350"
                && IsAt(input.SendAt, "22:52:57"))
            {
                return false;
            }
            else if (input.Sender.UserName == "Rews_red"
                && input.Content == "135....1?"
                && IsAt(input.SendAt, "22:54:07"))
            {
                return false;
            }
            else if (input.Sender.UserName == "BackScrasher"
                && input.Content == "1352 here"
                && IsAt(input.SendAt, "22:54:55"))
            {
                return false;
            }
            return true;
        }
        else if (input.SendOn(5, 9) && input.SendBetween("00:32:38", "00:35:10"))
        {
            //The curious case of war joke
            /*
             * "437618453155938317","tacktor#9598","09-May-22 12:32:36 AM","1890","",""
             * "267230094395703297","Rews_red#9505","09-May-22 12:32:37 AM","1891","","" /Keep
             * "437618453155938317","tacktor#9598","09-May-22 12:32:55 AM","Did we miss war joke?","",""
             * "267230094395703297","Rews_red#9505","09-May-22 12:33:02 AM","i think we did","",""
             * "437618453155938317","tacktor#9598","09-May-22 12:33:17 AM","Imma check","",""
             * "437618453155938317","tacktor#9598","09-May-22 12:33:45 AM","Not yet","",""
             * "437618453155938317","tacktor#9598","09-May-22 12:33:57 AM","1892","","" //Keep
             * "267230094395703297","Rews_red#9505","09-May-22 12:34:12 AM","1893 (1890 was kfc's founder ~~birth~~deathday)","","" //Keep
             * "267230094395703297","Rews_red#9505","09-May-22 12:34:18 AM","wait","",""
             * "267230094395703297","Rews_red#9505","09-May-22 12:34:21 AM","or dead day","",""
             * "437618453155938317","tacktor#9598","09-May-22 12:34:37 AM","Just check","",""
             * "267230094395703297","Rews_red#9505","09-May-22 12:34:48 AM","deathday i mis-looked","",""
             * "437618453155938317","tacktor#9598","09-May-22 12:35:06 AM","Ok","",""
             * "437618453155938317","tacktor#9598","09-May-22 12:35:11 AM","1894","","" //Keep
             * "267230094395703297","Rews_red#9505","09-May-22 12:35:28 AM","1895","",""
             */
            switch (input.Content)
            {
                case "1891":
                case "1892":
                case "1893 (1890 was kfc's founder ~~birth~~deathday)":
                case "1894":
                    return false;
            }
            return true;
        }
        else if (input.SendOn(5, 9) && input.SendBetween("00:38:00", "02:30:00"))
        {
            if (string.IsNullOrWhiteSpace(input.Content) && !string.IsNullOrWhiteSpace(input.Attachments))
                return false;
            else if (input.Content == "1913 First And Second Balkan Wars" &&
                input.Attachments == "")
                return false; //1913
            else if (input.Content == "no date so; 1915" &&
                input.Attachments == "https://cdn.discordapp.com/attachments/969212093213573140/972917231371583508/unknown.png")
                return false; //1915
            else if (input.Content == "Spanish flu rise" &&
                input.Attachments == "https://cdn.discordapp.com/attachments/969212093213573140/972918006869004328/9781250139436.jpg")
                return false; //1918
            else if (input.Content == "Le fasion" &&
                input.Attachments == "https://cdn.discordapp.com/attachments/969212093213573140/972918259777146930/67103526e8930e5f374c35212d3394b4.jpg")
                return false; //1920
            else if (input.Content == "Red Army invasion of Georgia 1921" &&
                input.Attachments == "https://cdn.discordapp.com/attachments/969212093213573140/972918534655078440/unknown.png")
                return false; //1921
            else if (input.Content == "1923")
                return false;
            else if (input.Content == "Oh yeah 1924")
                return false;
            else if (input.Content == "1925" && IsAt(input.SendAt, 0, 58, 1))
                return false;
            else if (input.Content == "1926" && IsAt(input.SendAt, 0, 59, 26))
                return false;
            else if (input.Content == "1927" && IsAt(input.SendAt, 1, 30, 40))
                return false;
            //Missed someone birthday and long history:
            /*
             * "267230094395703297","Rews_red#9505","09-May-22 12:37:20 AM","","https://cdn.discordapp.com/attachments/969212093213573140/972915118302187520/unknown.png",""
             * "267230094395703297","Rews_red#9505","09-May-22 12:38:29 AM","(we missed the moustach guy birthday","",""
             * "267230094395703297","Rews_red#9505","09-May-22 12:38:31 AM","oops","",""
             * "437618453155938317","tacktor#9598","09-May-22 12:38:54 AM","Let me download titatic","",""
             * "437618453155938317","tacktor#9598","09-May-22 12:40:30 AM","Cant download","",""
             * "267230094395703297","Rews_red#9505","09-May-22 12:41:02 AM","a picture is ok","",""
             * "437618453155938317","tacktor#9598","09-May-22 12:41:08 AM","","https://cdn.discordapp.com/attachments/969212093213573140/972916075115184179/92214064_2404834716476073_8024392973707902976_n.jpg",""
             * "267230094395703297","Rews_red#9505","09-May-22 12:41:12 AM","1913 First And Second Balkan Wars","",""
             * "437618453155938317","tacktor#9598","09-May-22 12:41:12 AM","Got it","",""
             * "267230094395703297","Rews_red#9505","09-May-22 12:41:18 AM","now do the war","",""
             * "437618453155938317","tacktor#9598","09-May-22 12:41:26 AM","Well well","",""
             * "267230094395703297","Rews_red#9505","09-May-22 12:41:32 AM",":3","",""
             * "437618453155938317","tacktor#9598","09-May-22 12:45:08 AM","","https://cdn.discordapp.com/attachments/969212093213573140/972917079550337034/DC-1914-27-d-Sarajevo.jpg",""
             * "267230094395703297","Rews_red#9505","09-May-22 12:45:44 AM","no date so; 1915","https://cdn.discordapp.com/attachments/969212093213573140/972917231371583508/unknown.png",""
             * "267230094395703297","Rews_red#9505","09-May-22 12:46:02 AM","พลเอกโบโชะ อองซาน","",""
             * "437618453155938317","tacktor#9598","09-May-22 12:46:20 AM","","https://cdn.discordapp.com/attachments/969212093213573140/972917383721271367/220px-1916WorldSeries.png",""
             * "267230094395703297","Rews_red#9505","09-May-22 12:47:39 AM","","https://cdn.discordapp.com/attachments/969212093213573140/972917713662017646/unknown.png",""
             * "437618453155938317","tacktor#9598","09-May-22 12:48:22 AM","Lenin music playing","",""
             * "437618453155938317","tacktor#9598","09-May-22 12:48:49 AM","Spanish flu rise","https://cdn.discordapp.com/attachments/969212093213573140/972918006869004328/9781250139436.jpg",""
             * "267230094395703297","Rews_red#9505","09-May-22 12:49:12 AM","","https://cdn.discordapp.com/attachments/969212093213573140/972918104218816632/unknown.png",""
             * "267230094395703297","Rews_red#9505","09-May-22 12:49:13 AM","lessgoo","",""
             * "437618453155938317","tacktor#9598","09-May-22 12:49:49 AM","Le fasion","https://cdn.discordapp.com/attachments/969212093213573140/972918259777146930/67103526e8930e5f374c35212d3394b4.jpg",""
             * "267230094395703297","Rews_red#9505","09-May-22 12:50:54 AM","Red Army invasion of Georgia 1921","https://cdn.discordapp.com/attachments/969212093213573140/972918534655078440/unknown.png",""
             * "437618453155938317","tacktor#9598","09-May-22 12:51:33 AM","Wrong one","",""
             * "437618453155938317","tacktor#9598","09-May-22 12:51:43 AM","","https://cdn.discordapp.com/attachments/969212093213573140/972918739106406440/large_20190504202134.jpeg",""
             * "267230094395703297","Rews_red#9505","09-May-22 12:53:16 AM","1923","",""
             * "437618453155938317","tacktor#9598","09-May-22 12:53:31 AM","Nothing happen?","",""
             * "267230094395703297","Rews_red#9505","09-May-22 12:53:59 AM","Well the moustache guy did Beer Hall Putsch","",""
             * "267230094395703297","Rews_red#9505","09-May-22 12:54:21 AM","Munich Putsch","",""
             * "437618453155938317","tacktor#9598","09-May-22 12:54:30 AM","Oh okay","",""
             * "437618453155938317","tacktor#9598","09-May-22 12:54:49 AM","Da queen born?","https://cdn.discordapp.com/attachments/969212093213573140/972919517409841242/queen-662641.jpg",""
             * "267230094395703297","Rews_red#9505","09-May-22 12:55:11 AM","where is the number","",""
             * "267230094395703297","Rews_red#9505","09-May-22 12:55:20 AM","wait","",""
             * "437618453155938317","tacktor#9598","09-May-22 12:55:32 AM","Oh yeah 1924","",""
             * "437618453155938317","tacktor#9598","09-May-22 12:55:41 AM","What?","",""
             * "267230094395703297","Rews_red#9505","09-May-22 12:56:15 AM","wasn't she born in 1926","",""
             * "437618453155938317","tacktor#9598","09-May-22 12:56:47 AM","I don't know she born around here","",""
             * "267230094395703297","Rews_red#9505","09-May-22 12:57:04 AM","kay","",""
             * "437618453155938317","tacktor#9598","09-May-22 12:57:14 AM","Thoght she born before star","",""
             * "267230094395703297","Rews_red#9505","09-May-22 12:57:41 AM","idk","",""
             * "267230094395703297","Rews_red#9505","09-May-22 12:58:01 AM","1925","",""
             * "267230094395703297","Rews_red#9505","09-May-22 12:59:10 AM","ima go do smoething else o//","",""
             * "437618453155938317","tacktor#9598","09-May-22 12:59:23 AM","K","",""
             * "437618453155938317","tacktor#9598","09-May-22 12:59:26 AM","1926","",""
             * "807952989623943189","BackScrasher#4282","09-May-22 01:30:40 AM","1927","",""
             * "437618453155938317","tacktor#9598","09-May-22 01:35:43 AM","Wait","",""
             * "437618453155938317","tacktor#9598","09-May-22 01:36:06 AM","Skip 4 number?","",""
             * "807952989623943189","BackScrasher#4282","09-May-22 02:17:32 AM","I don't know what you are talking about","",""
             * "267230094395703297","Rews_red#9505","09-May-22 02:57:00 AM","1928","",""
             * */
            return true;
        }
        else if (input.SendOn(5, 20) && input.SendBetween("21:17:00", "21:19:48"))
        {
            //A fillers
            /*
             * "526792117385953312","Kojina#1082","20-May-22 09:16:53 PM","2588 Seminal fluid means cum","",""
             * "437618453155938317","tacktor#9598","20-May-22 09:17:12 PM","How?","",""
             * "526792117385953312","Kojina#1082","20-May-22 09:17:21 PM","Semen","",""
             * "526792117385953312","Kojina#1082","20-May-22 09:17:23 PM","Seminal","",""
             * "526792117385953312","Kojina#1082","20-May-22 09:17:46 PM","So the great seminal flood...","",""
             * "437618453155938317","tacktor#9598","20-May-22 09:18:28 PM","Fine","",""
             * "437618453155938317","tacktor#9598","20-May-22 09:19:46 PM","Woman that can give birth are pregnant now","",""
             * "437618453155938317","tacktor#9598","20-May-22 09:19:57 PM","2589","",""
             */
        }
        else if (input.SendOn(6, 16) && input.SendBetween("21:00:00", "23:40:00"))
        {
            return true;
            /*
             * "873840836796903464","arizona ranger#9706","16-Jun-22 08:53:28 PM","3326","",""
             * "267230094395703297","Rews_red#9505","16-Jun-22 09:47:53 PM","2237","",""
             * "250308103105413121","Toon#9209","16-Jun-22 09:53:51 PM","2238","",""
             * "873840836796903464","arizona ranger#9706","16-Jun-22 09:54:07 PM","2239","",""
             * "250308103105413121","Toon#9209","16-Jun-22 09:54:36 PM","2240","",""
             * "873840836796903464","arizona ranger#9706","16-Jun-22 09:55:13 PM","2241","",""
             * "250308103105413121","Toon#9209","16-Jun-22 09:55:19 PM","2242 🤫","",""
             * "873840836796903464","arizona ranger#9706","16-Jun-22 09:56:36 PM","2243","",""
             * "250308103105413121","Toon#9209","16-Jun-22 09:56:40 PM","2244","",""
             * "873840836796903464","arizona ranger#9706","16-Jun-22 09:57:02 PM","2245","",""
             * "250308103105413121","Toon#9209","16-Jun-22 09:57:08 PM","2246","",""
             * "873840836796903464","arizona ranger#9706","16-Jun-22 09:57:21 PM","2247","",""
             * "267230094395703297","Rews_red#9505","16-Jun-22 10:02:50 PM","2248","",""
             * "873840836796903464","arizona ranger#9706","16-Jun-22 10:36:56 PM","","https://cdn.discordapp.com/attachments/969212093213573140/987017947379810324/8482fbda608a5e63.png","2️⃣ (1),4️⃣ (1),🇿 (1),9️⃣ (1)"
             * "267230094395703297","Rews_red#9505","16-Jun-22 10:44:53 PM","2250","",""
             * "250308103105413121","Toon#9209","16-Jun-22 10:53:05 PM","2251","",""
             * "873840836796903464","arizona ranger#9706","16-Jun-22 11:00:11 PM","2252","",""
             * "368658808051990540","ggguy#3542","16-Jun-22 11:08:50 PM","2253","",""
             * "873840836796903464","arizona ranger#9706","16-Jun-22 11:18:21 PM","2254","",""
             * "437618453155938317","tacktor#9598","16-Jun-22 11:30:21 PM","2255","",""
             * "267230094395703297","Rews_red#9505","16-Jun-22 11:31:03 PM","2256","",""
             * "873840836796903464","arizona ranger#9706","16-Jun-22 11:31:29 PM","2257","",""
             * "267230094395703297","Rews_red#9505","16-Jun-22 11:31:34 PM","2578","",""
             * "267230094395703297","Rews_red#9505","16-Jun-22 11:31:37 PM","no","",""
             * "267230094395703297","Rews_red#9505","16-Jun-22 11:31:40 PM","2258","",""
             * "267230094395703297","Rews_red#9505","16-Jun-22 11:31:50 PM","wait","",""
             * "267230094395703297","Rews_red#9505","16-Jun-22 11:31:51 PM","wat","",""
             * "267230094395703297","Rews_red#9505","16-Jun-22 11:32:00 PM","i am a dumb fuck","",""
             * "267230094395703297","Rews_red#9505","16-Jun-22 11:32:12 PM","it;s supposed to be 3k","",""
             * "371567557989105664","Ranvie#5623","16-Jun-22 11:43:20 PM","3327","","" */
        }
        else if (input.SendOn(6, 17) && input.SendBetween("12:06:59", "12:07:45"))
        {
            return true;
            /* "267230094395703297","Rews_red#9505","17-Jun-22 12:06:07 PM","3474","",""
             * "873840836796903464","arizona ranger#9706","17-Jun-22 12:06:58 PM","3475","",""
             * "267230094395703297","Rews_red#9505","17-Jun-22 12:07:00 PM","2476","",""
             * "873840836796903464","arizona ranger#9706","17-Jun-22 12:07:19 PM","wut","",""
             * "267230094395703297","Rews_red#9505","17-Jun-22 12:07:44 PM","qait","",""
             * "267230094395703297","Rews_red#9505","17-Jun-22 12:07:48 PM","3476","",""
             * "873840836796903464","arizona ranger#9706","17-Jun-22 12:07:55 PM","3477","","" */
        }
        else if (input.SendOn(6, 24) && input.SendBetween("12:55:00", "20:38:30"))
        {
            /* "250308103105413121","Toon#9209","24-Jun-22 10:45:27 AM","3624","","" //Keep
             * "267230094395703297","Rews_red#9505","24-Jun-22 12:55:02 PM","3265","",""
             * "250308103105413121","Toon#9209","24-Jun-22 12:56:34 PM","3266","",""
             * "743040512365166634","marisa#9299","24-Jun-22 01:11:16 PM","3267","",""
             * "480350715735441409","Joe Mama#0568","24-Jun-22 01:22:19 PM","3268 -do it. I done this.","",""
             * "250308103105413121","Toon#9209","24-Jun-22 02:51:58 PM","","https://cdn.discordapp.com/attachments/969212093213573140/989800036676296745/unknown.png",""
             * "267230094395703297","Rews_red#9505","24-Jun-22 03:05:38 PM","I should be banned from #นับถึงหมื่นด้วยกัน","",""
             * "267230094395703297","Rews_red#9505","24-Jun-22 03:05:53 PM","3625","",""
             * "267230094395703297","Rews_red#9505","24-Jun-22 03:09:30 PM","Im the one who made all the mistake","",""
             * "267230094395703297","Rews_red#9505","24-Jun-22 03:10:14 PM","https://c.tenor.com/3sscVvNm9zkAAAAM/controlmypc-cat.gif","",""
             * "786401021110910986","Win#7206","24-Jun-22 08:17:30 PM","you piece of shit","",""
             * "267230094395703297","Rews_red#9505","24-Jun-22 08:26:03 PM","YOU DIDNT EVEN COUNT","",""
             * "526792117385953312","Kojina#1082","24-Jun-22 08:36:58 PM","3270","",""
             * "267230094395703297","Rews_red#9505","24-Jun-22 08:38:26 PM","NO","",""
             * "267230094395703297","Rews_red#9505","24-Jun-22 08:38:52 PM","3 6 2 5","","" //Keep 
             * "526792117385953312","Kojina#1082","24-Jun-22 08:44:30 PM","fuck","",""
             * "526792117385953312","Kojina#1082","24-Jun-22 08:44:35 PM","3626","","" //Keep */
            return true;
        }
        else if (input.SendOn(7, 8) && input.SendBetween("21:08:30", "21:26:30"))
        {
            return true;
            //Someone's in a hurry!
            /* "267230094395703297","Rews_red#9505","08-Jul-22 05:35:28 PM","3809","",""
             * "575695984298950666","Ani_#9670","08-Jul-22 09:08:26 PM","3810","",""
             * "575695984298950666","Ani_#9670","08-Jul-22 09:08:34 PM","3811","",""
             * "575695984298950666","Ani_#9670","08-Jul-22 09:08:43 PM","3812","",""
             * "267230094395703297","Rews_red#9505","08-Jul-22 09:25:54 PM","No","",""
             * "267230094395703297","Rews_red#9505","08-Jul-22 09:25:58 PM","It doesn't work like thaf","",""
             * "267230094395703297","Rews_red#9505","08-Jul-22 09:26:21 PM","No","",""
             * "267230094395703297","Rews_red#9505","08-Jul-22 09:26:34 PM","3811","",""
             * "250308103105413121","Toon#9209","08-Jul-22 10:03:43 PM","3812","",""
             * "371567557989105664","Ranvie#5623","08-Jul-22 10:27:58 PM","3813","",""
             */
        }
        else if (input.SendOn(7, 18) && input.SendBetween("00:54:50", "00:55:15"))
        {
            //Small mistakes
            /* "743040512365166634","marisa#9299","18-Jul-22 12:54:34 AM","4337","","" //Keep
             * "267230094395703297","Rews_red#9505","18-Jul-22 12:54:55 AM","4357?","",""
             * "743040512365166634","marisa#9299","18-Jul-22 12:55:07 AM","lol cant count","",""
             * "267230094395703297","Rews_red#9505","18-Jul-22 12:55:12 AM","Same","",""
             * "267230094395703297","Rews_red#9505","18-Jul-22 12:55:20 AM","4338","","" //Keep */
            return true;
        }
        else if (input.SendWithin(min: new DateTime(2022, 7, 19, 23, 31, 40),
            max: new DateTime(2022, 7, 20, 12, 19, 0)))
        {
            //It's rewind time
            /* "250308103105413121","Toon#9209","19-Jul-22 11:31:38 PM","4390","","" //Keep
             * "267230094395703297","Rews_red#9505","19-Jul-22 11:31:45 PM","4931","",""
             * .
             * .
             * "371567557989105664","Ranvie#5623","20-Jul-22 12:18:57 PM","It's rewind time","",""
             * "371567557989105664","Ranvie#5623","20-Jul-22 12:19:39 PM","4391","","" //Keep
             * "250308103105413121","Toon#9209","20-Jul-22 12:28:39 PM","4392","","" */
            return true;
        }

        //Fuck this emote
        else if (input.Content.Contains(":volatile_motes:"))
            return true;

        switch (input.Attachments)
        {
            case "https://cdn.discordapp.com/attachments/969212093213573140/971847841221869598/IMG_2866.jpg":
            case "https://cdn.discordapp.com/attachments/969212093213573140/972895985082724372/unknown.png":
            case "https://cdn.discordapp.com/attachments/969212093213573140/972915118302187520/unknown.png":
            case "https://cdn.discordapp.com/attachments/969212093213573140/973584916279336980/IMG_1860.jpg":
            case "https://cdn.discordapp.com/attachments/969212093213573140/973596330087809094/OIP.jpg":
            case "https://cdn.discordapp.com/attachments/969212093213573140/984099216689336390/unknown.png":
            case "https://cdn.discordapp.com/attachments/969212093213573140/994048688643121234/unknown.png":
            case "https://cdn.discordapp.com/attachments/969212093213573140/994525961548738610/Screenshot_20220707-155038_Discord.jpg":
            case "https://cdn.discordapp.com/attachments/969212093213573140/996409216799609003/unknown.png":
                return true;
        }
        switch (input.Content)
        {
            case "(that was in 1775 but still)":
            case "~~1479 is no more~~":
            case "~~1500~~":
            case "~~4*2=8 EZIER~~":
            case "~~Editๆ~~":
            case "102 cause 101 not found":
            case "After this someone have to go through it to see who placed how many":
            case "And i'll seek myself out":
            case "Ayo put =1209":
            case "Can you at =246":
            case "Check again":
            case "bro":
            case "Bye":
            case "dumb":
            case "edited bruh":
            case "Fine":
            case "frick":
            case "Fuck":
            case "Fuck that":
            case "https://tenor.com/view/reggie-vs-iwata-die-fighting-gif-21420375":
            case "https://tenor.com/view/troll-pilled-gif-19289988":
            case "https://tenor.com/view/trollge-uncanny-depressed-sad-trollface-gif-24958563":
            case "I done now":
            case "I don't even know who he is":
            case "I don't know this count or not":
            case "idk how t odo it":
            case "I'll let other enjoy now because we need more active":
            case "i'm retarded":
            case "im":
            case "ima go  play bw":
            case "K":
            case "Kidding":
            case "nice":
            case "n o":
            case "no":
            case "No":
            case "No one saw that":
            case "oh":
            case "Ok nvm":
            case "Pinned a message.":
            case "Same gl":
            case "Shit":
            case "SLEEP!!":
            case "sure":
            case "that's illegal yes":
            case "Thx":
            case "Wait":
            case "Wait how does 4=8/0":
            case "wait no":
            case "wait":
            case "we doing it again?":
            case "We netheir":
            case "Welp":
            case "ya'll are good damn":
            case "Yeah i don't care that much":
            case "Yeah that ok":
            case "yeah gn":
            case "​":
            case "กฏ":
            case "ห้ามนับซ้อนตัวเอง":
            case "นับเกินนับลดเริ่มใหม่ที่นับถูกแล้ว":
            case "เริ่ม":
            case "Amost there":
            case "Got it":
            case "now do the war":
            case "Well well":
            case ":3":
            case "พลเอกโบโชะ อองซาน":
            case "Lenin music playing":
            case "lessgoo":
            case "Wrong one":
            case "Nothing happen?":
            case "Well the moustache guy did Beer Hall Putsch":
            case "Munich Putsch":
            case "Oh okay":
            case "where is the number":
            case "bye sleep":
            case "😱":
            case "(i'll take 1969)":
            case "also first Hydrogen Bomb test":
            case "Yeah that count":
            case "ドナルド・トランプ":
            case "we're getting into science fiction territory":
            case "Why":
            case "why not?":
            case "https://tenor.com/view/earth-ending-meteorite-meteor-earth-impact-gif-7786046":
            case "Holy shit it's win":
            case "20l302":
            case "Ok you want goes sci fi huh":
            case "Woman that can give birth are pregnant now":
            case "18 พฤศิกายน 2568 ภาพยนต์เรื่อง *100 Years* ฉายครั้งแรก":
            case "Doraemon birthday":
            case "I need to delete this":
            case "1 (a palindrome)":
            case "ใครเก็ทนี่โคตรพ่อค้า":
            case "changed my mind":
            case "yeah idk":
            case "...?":
            case "weee":
            case "Almost ⅓ of the way there!":
            case "¾⁵⁰":
            case "Sus":
            case ":PrayutSus:":
            case "Cuz z looks like 2":
            case "Bruh":
            case "Retar":
            case "totally didn't forgot that it was 3752 already nope":
            case "bruh":
            case "time to countdown":
            case "3️⃣7️⃣:ninebutton:3️⃣:BlueDot:5️⃣":
            case "have one zero in front":
            case "0k":
            case "because 10000 is the limit we'd do":
            case "righttttt":
            case "I've been waiting for this":
            case "you say 401 is correcy":
            case "fuck":
            case "what":
            case "Did you get hacked or something":
            case "ah":
            case "I see":
            case "Why am I always the one who fucjs it up":
            case "is that your discord ping":
            case "oh i cant count twice?":
            case "yeah":
            case "Quad fours!":
            case "Too soon":
            case "😳":
                return true;
            case "60":/* * "807952989623943189","BackScrasher#4282","06-May-22 01:35 AM","603 access denied","",""
                       * * "267230094395703297","Rews_red#9505","06-May-22 01:35 AM","60","","" //Drop this
                       * * "267230094395703297","Rews_red#9505","06-May-22 01:35 AM","4","","" //Count this as 604
                       * * "807952989623943189","BackScrasher#4282","06-May-22 01:35 AM","605","","" */
                if (input.Sender.UserName == "Rews_red"
                    && IsWithin(input.SendAt, 5, 6)
                    && input.SendAt.Hour == 1 && input.SendAt.Minute == 35)
                {
                    return true;
                }
                return false;
            case "277":/* * "526792117385953312","Kojina#1082","25-May-22 08:41:44 PM","2772","",""
                        * * "267230094395703297","Rews_red#9505","25-May-22 08:43:09 PM","277","","" //Drop this
                        * * "267230094395703297","Rews_red#9505","25-May-22 08:43:12 PM","3","","" //Keep this
                        * * "526792117385953312","Kojina#1082","25-May-22 08:44:27 PM","2774","","" */
                if (input.Sender.UserName == "Rews_red"
                    && IsWithin(input.SendAt, 5, 25)
                    && IsAt(input.SendAt, 20, 43, 9))
                    return true;
                return false;
            case "3342":
                /* "873840836796903464","arizona ranger#9706","17-Jun-22 11:52:02 AM","3401 nice","",""
                 * "267230094395703297","Rews_red#9505","17-Jun-22 11:52:35 AM","3342","",""
                 * "267230094395703297","Rews_red#9505","17-Jun-22 11:52:39 AM","Wait","",""
                 * "267230094395703297","Rews_red#9505","17-Jun-22 11:52:43 AM","3402","",""
                 */
                if (input.Sender.UserName == "Rews_red" && IsWithin(input.SendAt, 6, 17)
                    && IsAt(input.SendAt, 11, 52, 35))
                    return true;
                return false;
            case "-1":
                /* "267230094395703297","Rews_red#9505","05-Jul-22 08:25:24 AM","3767","",""
                 * "267230094395703297","Rews_red#9505","05-Jul-22 08:25:29 AM","-1","",""
                 * "267230094395703297","Rews_red#9505","05-Jul-22 08:25:39 AM","=3766","","" */
                if (input.Sender.UserName == "Rews_red" && IsWithin(input.SendAt, 7, 5) && IsAt(input.SendAt, 8, 25, 29))
                    return true;
                return false;
            case "203":
            case "~~1~~234":
            case "409":
            case "184":
            case "~~203~~ hehe nothing to see":
            case "2065 monke stonk":
            case "~~3152~~":
            case "3767":
                if (input.Sender.UserName == "Rews_red")
                    return true;
                return false;
            case "133":
            case "234":
            case "2548":
                if (input.Sender.UserName == "Kojina")
                    return true;
                return false;
            case "~~671~~":
            case "~~1200~~":
                if (input.Sender.UserName == "BackScrasher")
                    return true;
                return false;
            case "1๘๕":
            case "1020":
                if (input.Sender.UserName == "tacktor")
                    return true;
                return false;
            case "3811":
            case "3812":
                if (input.Sender.UserName == "Ani_")
                    return true;
                return false;
            case "3752":
                if (input.Sender.UserName == "Toon")
                    return false;
                return true;
        }
        if (Equals(input.Sender, IsCounting.PreviousCounter))
        {
            //Allowed case of double counting
            if (input.Sender.UserName == "Kojina" && input.Content == "119")
                goto Proceed;
            else if (input.Sender.UserName == "Rews_red" && input.Content == "Holy shit it's win")
                goto Proceed;
            else if (input.Sender.UserName == "tacktor" && input.Content == "2589")
                goto Proceed;
            return true;
        }
    Proceed:
        return false;
    }

    //Image to number
    //First it was a sign of defeat
    //Then the OCR came by 
    //To make this list a sign of OCR failure
    public static Dictionary<string, int> ImageToNumber => new()
    {
        //{ "https://cdn.discordapp.com/attachments/969212093213573140/970230203945222174/unknown.png", 191 },
        //{ "https://cdn.discordapp.com/attachments/969212093213573140/970343752818388992/unknown.png", 199 },
        //{ "https://cdn.discordapp.com/attachments/969212093213573140/970646353665462322/ed3a199a9d612a1b2922b96ee6d7470b.png", 222 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/970648166640799764/0034000052004_e06-11-2018.jpg", 226 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/970648719869485116/8850487001128_1.jpg", 228 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/970649063240372324/nokia_230_2.jpg", 230 },
        //{ "https://cdn.discordapp.com/attachments/969212093213573140/970664300521869443/images_-_2022-05-02T193201.860.jpeg", 250 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/970666625080655933/images_-_2022-05-02T194138.929.jpeg", 274 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/971110419630551090/IMG_2835.jpg", 420 }, //Funni weed numba
        { "https://cdn.discordapp.com/attachments/969212093213573140/971394788530216970/505_Games_logo.svg.png", 505 },
        //{ "https://cdn.discordapp.com/attachments/969212093213573140/971396885376016425/506.jpg", 506 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/971398296302149732/IMG_2837.jpg", 508 },
        //Bus battle
        { "https://cdn.discordapp.com/attachments/969212093213573140/971401319267074068/unknown.png", 509 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/971402342874370108/IMG_2838.jpg", 510 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/971402446419165274/unknown.png", 511 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/971402588580876388/IMG_2839.jpg", 512 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/971402850607448104/unknown.png", 513 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/971402888444276826/IMG_2841.jpg", 514 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/971403000218255390/unknown.png", 515 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/971403069868896256/IMG_2843.jpg", 516 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/971403136377958440/unknown.png", 517 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/971405318720479292/unknown.png", 519 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/971406155840639006/IMG_2845.jpg", 520 },
        //And it's over..
        { "https://cdn.discordapp.com/attachments/969212093213573140/971408431015673876/unknown.png", 521 },
        //Oh wait, no. It's back
        { "https://cdn.discordapp.com/attachments/969212093213573140/971410167067459594/IMG_2846.jpg", 526 },
        //{ "https://cdn.discordapp.com/attachments/969212093213573140/971410769277247549/unknown.png", 527 },
        //{ "https://cdn.discordapp.com/attachments/969212093213573140/971410900202446888/IMG_2847.jpg", 528 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/971468617164152872/Untitled.png", 550 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/971469904849043516/IMG_2863.png", 551 },
        //{ "https://cdn.discordapp.com/attachments/969212093213573140/971471552887525426/IMG_2864.jpg", 553 }
        { "https://cdn.discordapp.com/attachments/969212093213573140/971845594060554300/unknown.png", 700 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/971846650224066590/unknown.png", 800 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/971849642880958564/IMG_2868.jpg", 999 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/971849722035863612/unknown.png", 1000 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/972079080176517130/unknown.png", 1100 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/972743152488288266/images_-_2022-05-08T125456.816.jpeg", 1179 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/972783013144440832/unknown.png", 1181 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/972810309624545330/unknown.png", 1183 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/972846825952387112/unknown.png", 1192 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/972862761715511306/TSNBg3wSBdng7ijMhwcfG38UQMFIcJ2EiKxFaRy9TSM.png", 1202 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/972863267913478215/unknown.png", 1203 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/972889711012098048/unknown.png", 1367 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/972599077332193370/images_-_2022-05-08T034114.920.jpeg", 1169 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/972917383721271367/220px-1916WorldSeries.png", 1916 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/972918006869004328/9781250139436.jpg", 1918 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/972918104218816632/unknown.png", 1919 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/972918259777146930/67103526e8930e5f374c35212d3394b4.jpg", 1920 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/972918739106406440/large_20190504202134.jpeg", 1922 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/973069406471077998/51LY33uzCQL._SX342_SY445_QL70_ML2_.jpg", 1938 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/973070707753910322/1941_movie.jpg", 1941 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/973071182037401610/Unknown-Battle-2019-poster.jpg", 1942 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/973128346789609533/unknown.png", 1952 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/973128833916080178/unknown.png", 1953 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/973240868490330243/unknown.png", 1985 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/973561582032396288/unknown.png", 2012 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/974294288953405460/unknown.png", 2077 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/982695340266389554/IMG_0842.png", 2932 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/982976759161057310/unknown.png", 2940 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/983198139387346974/unknown.png", 2955 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/983521766737604618/2976.2976", 2976 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/983734494366404678/unknown.png", 3000 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/984039521232510976/unknown.png", 3009 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/984099545858310164/unknown.png", 3013 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/984106825152622622/unknown.png", 3014 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/984106962918711306/unknown.png", 3015 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/985125537359990864/unknown.png", 3041 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/985163623963951124/unknown.png", 3050 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/985170503436238868/unknown.png", 3060 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/985177583224254474/unknown.png", 3065 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/985221645956435988/unknown.png", 3070 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/985460203640205342/unknown.png", 3080 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/985598085579280504/unknown.png", 3090 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/989800036676296745/unknown.png", 3269 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/987215516299067455/1-33-33-time.jpg", 3333 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/987217522669543454/images.jpeg", 3390 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/987219212894343218/images_1.jpeg", 3434 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/989920202563125248/unknown.png", 3632 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/992068798591025202/42ABEC8F-AECB-4D71-AAB2-ADA620C78870.mp4", 3694 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/992076948123689050/E448F13F-4977-44F6-9041-9DB6E1A71F97.mp4", 3696 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/992087506638098524/unknown.png", 3701 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/992327642525204500/unknown.png", 3709 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/996420035922907167/unknown.png", 4000 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/999233793602883604/images_4.jpeg", 4399 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/1001088488655032360/4468.jpeg", 4468 },
        { "https://cdn.discordapp.com/attachments/969212093213573140/1001089141288747068/IMG_20220725_183027.jpg", 4470 },
    };

    public static Dictionary<string, int> MemeReference => new()
    {
        //{ "Agent 47", 47 },
        { "1 stack", 64 },
        { "black man walking through walls (run)", 106 },
        { "les majeste", 112 },
        //{ "einhundert einundsiebzig (171)", 171 },
        //{ "พอจะมีสัก... 200 มั้ย?", 200 },
        //{ "มั่ย (201)", 201 },
        { "两百零三", 203 },
        //{ "50% of 420 (210)", 210 },
        //{ "มาตรา 288 ผู้ใดฆ่าผู้อื่น ต้องระวางโทษประหารชีวิต จำคุกตลอดชีวิต หรือจำคุกตั้งแต่สิบห้าปีถึงยี่สิบปี", 288 },
        //{ "3️⃣ 0️⃣ 🇴", 300 },
        //{ "Omega 3, 6, 9", 369 },
        //{ "4️⃣ 🇴 🅾️", 400 },
        //"426 illogical formula" - Kojina, May 2022
        //{ "4=2+2", 422 },
        //{ "4 = 2-4", 424 },
        //{ "4+3=7", 437 },
        //{ "4+3!=8", 438 },
        //{ "sqrt4=3!/sqrt9", 439 },
        //{ "4/4=1 no more equations? 😢", 441 },
        //Yeah that would be great! - Me
        //{ "sqrt(sqrt4+sqrt4)=2", 442 }, //FUCK
        //{ "4!/4=3!", 443 },
        //{ "4=4=4", 444 },
        //{ "4+sqrt(sqrt(sqrt(sqrt(sqrt(sqrt(sqrt(4))))))) = 5", 445 },
        //{ "4+4=8", 448 },
        //{ "4-sqrt(sqrt(sqrt(sqrt(sqrt(sqrt(sqrt(4))))))) = sqrt(9)", 449 },
        //{ "4-5=-1", 451 },
        //{ "|sqrt4-5|!=6", 456 },
        //{ "sqrt4+5=7 EZ", 457 },
        //{ "4+5=9 how to cross", 459 },
        //{ "use ~~ in front and behind 46/0", 460 },
        //{ "sqrt4+6=8", 468 },
        //{ "4+6=9 :ThaksinThumbsUp:", 469 },
        //{ "4-7=-3 rip", 473 },
        //{ "ceiling(4!/7)=4 I GOT IT", 474 },
        //{ "sqrt(4)-7=-5 what is this ceiling thing", 475 },
        //{ "|4-7|!=6 it rounds number up, example ceil(2.3)=3, ceil(4.001)=5", 476 },
        //{ "floor(sqrt(sqrt4))=floor(log(7)8) opposite is floor, example floor(3.24)=3, floor (99.999)=99", 478 },
        //{ "sqrt(4)+7=9 my house have floor too", 479 },
        //{ "4=8/0 i am in ur floor", 480 },
        //{ "4=8/log(1) i am in ur ceiling", 481 },
        //{ "4=8/2 yes", 482 },
        //{ "4=8/ceiling(sqrt(3)) no", 483 },
        //{ "sqrt4=8/4 i gotta stop at 500, this is taking too long", 484 },
        //{ "f:R+—>R+, f(1)=493\r\n\r\nf(x)+f(y)=f(x+y)\r\n\r\nfor all x,y>0. Find f(x)/x.", 493 }, 
        //HOLY SHIT IT"S FINALLY OVER!!! AWGFHAIWGHQI@H#WGIAOHGNOAIG
        //{ "4.94 × 10²", 494 }, //Scientific annotate
        //{ "```cs\r\nusing System;\r\n\r\nnamespace Number496\r\n{\r\n    public class _496\r\n    {\r\n        public int Get496()\r\n        {\r\n            int counter = 0;\r\n            while (counter < 495)\r\n                counter += 5;\r\n            while (counter > 495)\r\n            {\r\n                counter--;\r\n                if (counter == 495)\r\n                {\r\n                    counter += 1;\r\n                    break;\r\n                }\r\n            }\r\n            if (counter == 496)\r\n            {\r\n                System.Diagnostics.Debug.WriteLine(counter);\r\n            }\r\n            if (counter != 496)\r\n            {\r\n                throw new InvalidOperationException();\r\n            }\r\n            return 496;\r\n        }\r\n    }\r\n}\r\n```", 496 },
        //Oh no, it's back..
        //{ "5=3+2", 532 },        
        //{ "5+3=5", 535 },
        //{ "5+3=7", 537 },
        //{ "5+4=9", 549 },
        { "⁵⅞", 578 },
        //{ "𝟓𝟖𝟗", 589 },
        //{ "𝟝𝟡𝟙", 591 },
        { "🁈 🁒 🁀", 593 },
        //{ "1 18 8", 1188 },
        { "1~~3~~208", 1208 },
        //{ "135....1?", 1351 },
        //{ "14 69", 1469 },
        { "148o", 1480 },
        { "2III", 2111 },
        { "B2D", 2861 }, //Hexdecimal
        //{ "28cyberpunk77", 2877 },
        { "song-phan-phad-roi-kaw-sip-jed", 2897 },
        { "song-phan-phad-roi-kaw-sip-phad", 2898 },
        { "song-phan-paed-roi-kaw-sip-kaw", 2899 },
        { "Song-phan-kaw-roi", 2900 },
        { "song-phan-kaw-roi-ed", 2901 },
        { "Söŋṕan kawröj söŋ", 2902 },
        { "Songpan kawroy sip jed", 2917 },
        { "Söŋṕan kawröj jisib päd", 2928 },
        { "ສອງພັນເກົ້າຮ້ອຍສາມສິບເອັດ", 2931 },
        { "ຊາວເກົ້າ ແລະ ສາມສິບສາມ", 2933 },
        { "໒໙໓໕", 2935 },
        { "29E6", 2936 },
        { "2g4S", 2944 },
        { "00110010 00111001 00110100 00110101", 2945 },
        { "B82", 2946 },
        { "Z9SZ", 2952 },
        { "ສອງເກົ້າຫົກເກົ້າ", 2969 },
        { "297เ", 2971 },
        { "🕑🕘🕖🕓", 2974 },
        { "1993 + 1K", 2993 },
        { "2\n9\n7\n๕", 2975 },
        { "+1\nhttps://c.tenor.com/ZulNSiQ3ErYAAAAM/avengers-endgame-i-love-you3000.gif", 3001 },
        { "a thousand years later after 2011", 3011 },
        { "a thousand years later after 2012", 3012 },
        { "ตรีพันตรีสิบยี่", 3132 },
        { "ตรีเอ็ดตรีอัฐ", 3138 },
        { "Trois mille cent quarante-et-un", 3141 },
        { "336∞", 3368 },
        { "337O", 3370 },
        { "33∞∞", 3388 },
        { "¾⁴⁴", 3444 },
        { "¾⅘", 3445 },
        { "¾⁴⁶", 3446 },
        { "¾⁴⁷", 3447 },
        { "¾⁴⁸", 3448 },
        { "¾⁴⁹", 3449 },
        { "¾⁵∅", 3450 },
        { "¾⁵1", 3451 },
        { "346Z", 3462 },
        { "364I", 3641 },
        { "Ǝ642", 3642 },
        { "3760\n-5", 3755 },
        /*{ ":ThaksinThumbsUp: :ThaiChanHi: :ThaiChan: :ThaiSus1:", 3796 },*/
        { "386∅", 3860 },
        { "•39•7", 3907 },
        { "0️⃣ 3️⃣ :ninebutton: 0️⃣ :eight_button:", 3908 },
        { "3941.99999999999999999 ¯\\_(ツ)_/¯", 3942 },
        { "https://discord.com/channels/959666934281015296/969212093213573140/969223431843364884 https://discord.com/channels/959666934281015296/969212093213573140/969216436306337833", 4032 },
        { "https://discord.com/channels/959666934281015296/969212093213573140/969223431843364884 \nhttps://discord.com/channels/959666934281015296/969212093213573140/969221537985101844", 4034 },
        { "have fun going back up and go back down again. https://discord.com/channels/959666934281015296/969212093213573140/996409685999624262\n+\nhttps://discord.com/channels/959666934281015296/969212093213573140/969297064611708928", 4037 },
        { "4️⃣ :eight_button: 0️⃣ ๐", 4800 },
    };
}