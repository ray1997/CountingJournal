using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CountingJournal.Model;
using CountingJournal.Helpers;

namespace CountingJournal.Helpers.Text;
public static class Splits
{
    /// <summary>
    /// Use to send info to IsNoise()
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static (bool, int) IsItFiller(this Message input)
    {
        /*
         * "807952989623943189","BackScrasher#4282","06-May-22 01:35 AM","603 access denied","",""
         * "267230094395703297","Rews_red#9505","06-May-22 01:35:22 AM","60","","" //Drop this
         * "267230094395703297","Rews_red#9505","06-May-22 01:35:24 AM","4","","" //Count this as 604
         * "807952989623943189","BackScrasher#4282","06-May-22 01:35 AM","605","",""
         */
        if (input.SendOn(5, 6) && input.SendOn(1, 35, 22)
            && input.Sender.UserName == "Rews_red" && input.Content == "60")
            return (true, -1);
        else if (input.SendOn(5, 6) && input.SendOn(1, 35, 24)
            && input.Sender.UserName == "Rews_red" && input.Content == "4")
            return (false, 604);

        /* "267230094395703297","Rews_red#9505","25-May-22 08:43:09 PM","277","",""
         * "267230094395703297","Rews_red#9505","25-May-22 08:43:12 PM","3","","" */
        else if (input.SendOn(5, 25) && input.SendOn(20, 43, 9)
            && input.Sender.UserName == "Rews_red" && input.Content == "277")
            return (true, -1);
        else if (input.SendOn(5, 25) && input.SendOn(20, 43, 12)
            && input.Sender.UserName == "Rews_red" && input.Content == "3")
            return (false, 2773);

        /* "267230094395703297","Rews_red#9505","04-Jul-22 12:02:39 PM","3752","",""
         * "267230094395703297","Rews_red#9505","04-Jul-22 12:02:42 PM","+1","","" */
        else if (input.SendOn(7, 4) && input.SendOn(12, 2, 39))
            return (true, -1);
        else if (input.SendOn(7, 4) && input.SendOn(12, 2, 42))
            return (false, 3753);

        /* "437618453155938317","tacktor#9598","04-Jul-22 12:19:11 PM","3752","",""
"437618453155938317","tacktor#9598","04-Jul-22 12:19:15 PM","+2","","" */
        else if (input.SendOn(7, 4) && input.SendOn(12, 19, 11))
            return (true, -1);
        else if (input.SendOn(7, 4) && input.SendOn(12, 19, 15))
            return (false, 3754);

        return (false, -1);
    }
}
