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
        public delegate int Lookup(String v);

        /// <summary>
        /// <para>
        /// Evaluates integer arithmatic expressions using standard infix notation. 
        /// Evaluate is capable of evaluating expressions which are made up of non negative
        /// integers and addition, subtraction, multiplication and division operations using 
        /// standard presidence rules. For Example "(2 + 7) * 5 - 3" will evaluate to 42.
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
            IEnumerable<string> tokens = CleanAndValidateTokens(Regex.Split(exp, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)"));

            Stack<int> values = new Stack<int>();
            Stack<string> operators = new Stack<string>();

            // this is the main body of the algorithm where the expression is evaluated
            foreach(string token in tokens)
            {
                // check if token is integer
                int operand;
                if(int.TryParse(token, out operand))
                {
                    HandleInt(operand, values, operators);
                }
                // check if token is variable
                else if(token.StartsWithLetter())
                {
                    HandleInt(variableEvaluator(token), values, operators);
                }
                else if(token == "+" || token == "-")
                {
                    // if plus or minus is at top of operator stack,
                    // apply it to the top two operands on top of values stack
                    if(operators.IsAtTop("+") || operators.IsAtTop("-"))
                    {
                        ApplyOperatorStack(values, operators);
                    }

                    // push + or - token to operators stack
                    operators.Push(token);

                }
                else if(token == "*" || token == "/")
                {
                    operators.Push(token);
                }
                else if(token == "(")
                {
                    operators.Push(token);
                }
                else if(token == ")")
                {
                    // if plus or minus is at top of operator stack,
                    // apply it to the top two operands on top of values stack
                    if(operators.IsAtTop("+") || operators.IsAtTop("-"))
                    {
                        ApplyOperatorStack(values, operators);
                    }

                    // now that + or - has been applied, next operator on stack
                    // should be "("
                    if(operators.IsAtTop("("))
                    {
                        operators.Pop();
                    }
                    else
                    {
                        throw new ArgumentException("Missmatched Parenthesis. ')' encountered without paired '(' ");
                    }

                    if(operators.IsAtTop("*") || operators.IsAtTop("/"))
                    {
                        ApplyOperatorStack(values, operators);
                    }

                }
            }
            // the last token has been processed.

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
                    throw new ArgumentException("Syntax error: There are operands without associated operators");
                }
            }
            // operator stack is not empty
            // there should be exactly one operator in operators stack
            else if (operators.Count == 1)
            {
                // the only operator token should be '+' or '-'
                if(operators.IsAtTop("+") || operators.IsAtTop("-"))
                {
                    if(values.Count == 2)
                    {
                        ApplyOperatorStack(values, operators);
                        return values.Pop();
                    }
                    else
                    {
                        throw new ArgumentException("Syntax error: There are an incorrect number of operands for operators");
                    }
                }
                // the operator wasn't a + or -
                else
                {
                    throw new ArgumentException("Syntax error: unable to apply proper order of operations");
                }
            }
            // there is more than one operator in operators stack
            else
            {
                throw new ArgumentException("Syntax error: there are operators without proper number of operands");
            }

        }

        /// <summary>
        /// If * or / is at the top of the operator stack, pops the value stack and pops the operator stack. 
        /// then applies the popped operator to the popped number and t. Pushes the result onto the value stack.
        /// Otherwise, pushes t onto the value stack.
        /// 
        /// If the value stack is empty, or the operation results in divide by zero, throws ArgumentException
        /// </summary>
        /// <param name="t">the integer token</param>
        /// <param name="values">a stack containing values for the evaluate method</param>
        /// <param name="operators">a stack containing operators for the evaluate method</param>
        private static void HandleInt(int t, Stack<int> values, Stack<string> operators)
        {
            // operand is an integer, check if operator * or / is at top of stack and apply it
            if(operators.IsAtTop("*") || operators.IsAtTop("/"))
            {
                // make sure there are two values to multiply
                if(values.Count < 1)
                {
                    throw new ArgumentException("There is only one value to apply " + operators.Peek() + " operator");
                }

                int result = ApplyOperator(values.Pop(), t, operators.Pop());
                values.Push(result);
            }
            // * or / is not at top of operator stack, push operand to top of stack
            else
            {
                values.Push(t);
            }
        }

        /// <summary>
        /// Pops the value stack twice and the operator stack once, then applies the 
        /// popped operator to the popped values, then pushes the result onto the value stack.
        /// 
        /// If there are less than two operators on the operator stack, throws ArgumentException
        /// </summary>
        /// <param name="values">a stack containing values for the evaluate method</param>
        /// <param name="operators">a stack containing operators for the evaluate method</param>
        private static void ApplyOperatorStack(Stack<int> values, Stack<string> operators)
        {
            // make sure there are enough operands to apply the operator
            if(values.Count < 2)
            {
                throw new ArgumentException("There are not enough operands to apply " + operators.Peek() + " operator");
            }

            // pop value stack twice 
            int value2 = values.Pop();
            int value1 = values.Pop();
            // pop the operator stack once
            string op = operators.Pop();

            // apply operator and push the result to the values stack
            values.Push(ApplyOperator(value1, value2, op));

        }

        /// <summary>
        /// Applies the operator op to val1 and val2 respectively and returns the result
        /// For example, if op is "*" will return val1 * val2. 
        /// 
        /// The only valid operators are "+", "-", "*", and "/". 
        /// 
        /// If val2 = 0 and op = "/" will throw ArgumentException.
        /// </summary>
        private static int ApplyOperator(int val1, int val2, string op)
        {
            int result = 0;
            switch(op)
            {
                case "+":
                    // apply + operator
                    return val1 + val2;
                case "-":
                    // apply - operator
                    return val1 - val2;
                case "*":
                    // apply * operator
                    return val1 * val2;
                case "/":
                    // check for divide by zero
                    if(val2 == 0)
                    {
                        throw new ArgumentException("Cannot divide by zero");
                    }
                    // apply / operator
                    return val1 / val2;
            }
            return result;
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
                        throw new ArgumentException(errorMessage);
                    }
                    yield return s;
                }
            }
        }

        /// <summary>
        /// Takes in a string, token, and returns true if it's a valid token
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
