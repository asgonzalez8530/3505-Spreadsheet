using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FormulaEvaluator
{
    /// <summary>
    /// Evaluator class provides methods for evaluating integer arithmatic expressions using standard infix notation.
    /// </summary>
    public static class Evaluator
    {
        /// <summary>
        /// A Lookup function maps the value of some string v to an integer value
        /// </summary>
        /// <param name="v">The string to be mapped to a value</param>
        /// <returns></returns>
        public delegate int Lookup(String v);

        public static int Evaluate(String exp, Lookup variableEvaluator)
        {
            List<string> tokens = new List<string>(RemoveWhitespace(Regex.Split(exp, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)")));
            
            // TODO...
            return 0;
        }

        /// <summary>
        /// Takes an IEnumberable object filled with strings and removes all leading and ending whitespace from
        /// each token, and returns an IEnumerable with no empty tokens
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        private static IEnumerable<string> RemoveWhitespace(string[] tokens)
        {
            foreach (string token in tokens)
            {
                if (!string.IsNullOrWhiteSpace(token))
                {
                    string s = token.Trim();
                    yield return s;
                }
            }
        }

        /// <summary>
        /// Takes in a string, token, and returns true if it a valid token
        /// 
        /// Tokens are valid if they are  "(", ")", "+", "-", "*", or "/".
        /// Tokens are also valid if it is a string which is a letter followed 
        /// by one or more digits or is a non negative integer.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static bool IsValidToken(string token)
        {
            string pattern = @"( ^\($ ) | ( ^\)$ ) | (^-$) | ( ^\+$ ) | ( ^\*$ ) | ( ^/$ ) | ( ^[a-zA-Z]+\d+$ ) | ( ^\d+$ )";
            return Regex.IsMatch(token, pattern, RegexOptions.IgnorePatternWhitespace);
        }

        private static bool ValidateTokens()
        {
            return false;
        }
    }
}
