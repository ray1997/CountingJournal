using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountingJournal.Helpers.Text;
public static class Scatter
{
    public static bool IsAlright(string formula, int expectNumber)
    {
        if (formula.Length <= expectNumber.ToString().Length)
            return false;
        try
        {
            var expects = expectNumber.ToString().ToCharArray().ToList();
            var formulas = formula.ToCharArray().ToList();
            
            while (expects.Count > 0)
            {
                int index = -1;
                index = formulas.IndexOf(expects[0]);
                if (index == -1)
                    return false;
                formulas.Remove(expects[0]);
                expects.RemoveAt(0);
            }
            return true;
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
