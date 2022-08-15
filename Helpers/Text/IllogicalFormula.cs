using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountingJournal.Helpers.Text;
public static class IllogicalFormula
{
    public static bool IsAlright(string formula, int expectNumber)
    {
        if (formula.Length <= expectNumber.ToString().Length)
            return false;
        if (Roman.IsRoman(formula))
            return false;
        if (formula.Any())
        if (!formula.Any(c => BasicCalculation.symbols.Contains(c)))
            return false;
        try
        {
            int latestFound = 0;
            List<NumberOnFormula> expand =
                new(expectNumber.ToString().ToCharArray().Select(i => new NumberOnFormula() { number = i }));
            foreach (var num in expand)
            {
                num.isInFormula = formula.Contains(num.number);
                latestFound = formula.IndexOf(num.number, latestFound);
            }
            var results = expand.Select(i => i.isInFormula).Distinct();
            if (results.Count() > 1)
                return false;
            return results.First();
        }
        catch
        {

        }
        return false;
    }

    private class NumberOnFormula
    {
        public char number;
        public bool isInFormula;
    }
}
