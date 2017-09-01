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
        //private const string allTokensPattern= @"(^\($)|(^\)$)|(^-$)|(^\+$)|(^\*$)|(^/$)|(^[a-zA-Z]+\d+$)|(^\d+$)";

        /// <summary>
        /// A Lookup function maps the value of some string v to an integer value
        /// </summary>
        /// <param name="v">The string to be mapped to a value</param>
        public delegate int Lookup(String v);

        /// <summary>
        /// <para>
        /// Evaluates integer arithmatic expressions using standard infix notation. 
        /// Evaluate is capable of evaluating expressions which are made up of non negative
        /// integers and addition, subtraction, multiplication and division operations using 
        /// standard presidence srules. For Example "(2 + 7) * 5 - 3" will evaluate to 42.
        /// </para>
        /// <para>
        /// The only accepted mathmatic symbols are "(", ")", "+", "-", "*", or "/".
        /// </para>
        /// Variables are permitted and must be one or more letters followed by one or more 
        /// numbers and are mapped to integer values using the provided Lookup delegate. 
        /// <para>
        /// Examples of valid Variables:
        ///     "A1"
        ///     "BB3"
        ///     "Nn14"
        ///     "yYn137"
        /// </para>
        /// <para>
        /// Examples of invalid Variables:
        ///     "A"
        ///     "12B"
        /// </para>
        /// If an invalid variable is included in the expression, or if evaluate is unable to 
        /// evaluate the function (ie devide by zero,) will throw an argument exception.
        /// </summary>
        public static int Evaluate(String exp, Lookup variableEvaluator)
        {
            // split the expression into tokens, clean it of empty strings and excess white space and check that each token is valid
            List<string> tokens = new List<string>(CleanAndValidateTokens(Regex.Split(exp, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)")));

            Stack<int> values = new Stack<int>();
            Stack<string> operators = new Stack<string>();

            // this is the main body of the algorithm where the expression is evaluated
            foreach(string token in tokens)
            {
                // check if token is integer
                int operand;
                if(int.TryParse(token, out operand))
                {
                    if(operators.IsAtTop("*"))
                    {

                    }
                    else if(operators.IsAtTop("/"))
                    {

                    }
                }
                // check if token is variable
                else if(token.StartsWithLetter())
                {

                }
                else if(token == "+" || token == "-")
                {

                }
                else if(token == "*" || token == "/")
                {

                }
                else if(token == "(")
                {

                }
                else if(token == ")")
                {

                }
            }

            // operator stack is empty
            if(operators.Count == 0)
            {
                if(values.Count == 1)
                {
                    return values.Pop();
                }
                else
                {
                    // there is more than one value in the values stack
                    // which is an error
                }
            }

            // operator stack is not empty


            // TODO...
            return 0;
        }

        /// <summary>
        /// Takes an array object filled with the tokens that make up a formula and removes all leading and ending whitespace from
        /// each token, and checks that each token is valid. If tokens are not valid throws an ArgumentException.
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        private static IEnumerable<string> CleanAndValidateTokens(string[] tokens)
        {
            foreach(string token in tokens)
            {
                // skip all empty strings and and strings filled with only whitespace
                if(!string.IsNullOrWhiteSpace(token))
                {
                    // remove whitespace from tokens
                    string s = token.Trim();
                    //validate tokens 
                    if(!IsValidToken(s))
                    {
                        // if invalid, throw exception
                        string errorMessage = "Invalid token \"" + s + "\"";
                        throw new ArgumentException();
                    }
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
            // a pattern that matches all valid tokens without white space
            string pattern = @"( ^\($ ) | ( ^\)$ ) | (^-$) | ( ^\+$ ) | ( ^\*$ ) | ( ^/$ ) | ( ^[a-zA-Z]+\d+$ ) | ( ^\d+$ )";
            return Regex.IsMatch(token, pattern, RegexOptions.IgnorePatternWhitespace);
        }
    }

    internal static class ExtensionMethods
    {
        /// <summary>
        /// Takes a string s and returns true if a string matching s is at the 
        /// top of the stack, else returns false.
        /// </summary>
        public static bool IsAtTop(this Stack<string> stack, string s)
        {
            return (stack.Count > 0 && stack.Peek() == s);
        }

        /// <summary>
        /// Returns true if this string begins with a letter of the english alphabet
        /// </summary>
        public static bool StartsWithLetter(this String s)
        {
            return ((s[0] >= 'a' && s[0] <= 'z') || (s[0] >= 'A' && s[0] <= 'Z'));
        }
    }
}
