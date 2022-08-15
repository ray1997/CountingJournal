using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountingJournal.Helpers;
public static class Constants
{
    public static char[] Calculations = new char[] { '+', '-', '*', '×', '÷', '^' };
    public static char[] AdditionalCalculations = new char[] { '=', '(', ')' };
    public static char[] AllCalculations = Calculations.Concat(AdditionalCalculations).ToArray();

    public static char[] Markdowns = new char[] { '*', '~', '～', '|' };
    public static Dictionary<char, char> SuperScripts => new()
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
    public static Dictionary<char, char> SubScripts => new()
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

    public const char Repeater = 'ๆ';

    public static Dictionary<char, char> ThaiToArabic => new()
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

    public static Dictionary<string, int> ThaiTextNumber => new()
    {
        { "หนึ่ง", 1},
        { "สอง", 2},
        { "สาม", 3},
        { "สี่", 4},
        { "ห้า", 5},
        { "หก", 6},
        { "เจ็ด", 7},
        { "แปด", 8},
        { "เก้า", 9}
    };


    public static Dictionary<string, int> ThaiTextNumberMultiply => new()
    {
        { "สิบ", 10},
        { "ร้อย", 100},
        { "พัน", 1000},
        { "หมื่น", 10000},
        { "แสน", 100000},
        { "ล้าน", 1000000},
    };

    public static char[] RomanNumeralSymbol => new char[] { 'I', 'V', 'X', 'C', 'D', 'M', 'L' };
}
