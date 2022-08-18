using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CountingJournal.Model;

namespace CountingJournal.Helpers;
public static class TimeHelper
{

    public const string TimeFormat = "HH:mm:ss";
    public static bool Between(DateTime input, string min, string max)
    {
        var minTime = TimeOnly.ParseExact(min, TimeFormat);
        var maxTime = TimeOnly.ParseExact(max, TimeFormat);
        var compare = TimeOnly.FromDateTime(input);
        return compare > minTime && compare < maxTime;
    }

    /// <summary>
    /// Check whether the input date is within between the specified month & day
    /// </summary>
    /// <param name="input"></param>
    /// <param name="month"></param>
    /// <param name="day"></param>
    /// <returns></returns>
    public static bool IsWithin(DateTime input, int month, int day)
    {
        if (input.Month == month && input.Day == day)
            return true;
        return false;
    }

    /// <summary>
    /// Check if the input time is at exact from comparing required
    /// </summary>
    /// <param name="input"></param>
    /// <param name="compare"></param>
    /// <returns></returns>
    public static bool IsAt(DateTime input, string compare)
    {
        TimeOnly a = TimeOnly.FromDateTime(input);
        TimeOnly b = TimeOnly.ParseExact(compare, TimeFormat);
        return a == b;
    }

    public static bool IsAt(DateTime input, int hour, int minute, int second) 
        => input.Hour == hour && input.Minute == minute && input.Second == second;
}

public static class TimeHelperExtensions
{
    public static bool SendBetween(this Message message, string min, string max) => TimeHelper.Between(message.SendAt, min, max);

    public static bool SendOn(this Message message, int month, int day) => TimeHelper.IsWithin(message.SendAt, month, day);

    public static bool SendOn(this Message message, int hour, int minute, int second) => TimeHelper.IsAt(message.SendAt, hour, minute, second);
}