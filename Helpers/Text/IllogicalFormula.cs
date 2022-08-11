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
        List<NumberOnFormula> expand = 
            new(expectNumber.ToString().ToCharArray().Select(i => new NumberOnFormula() { number = i }));
        foreach (var num in expand)
        {
            num.isInFormula = formula.Contains(num.number);
            formula = formula[formula.IndexOf(num.number)..];
        }
        return expand.Select(i => i.isInFormula).Distinct().First();
    }

    private class NumberOnFormula
    {
        public char number;
        public bool isInFormula;
    }
}
