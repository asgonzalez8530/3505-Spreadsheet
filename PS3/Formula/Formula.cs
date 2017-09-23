// implemented by Aaron Bellis u0981638 for CS 3500 Fall 2017

// Skeleton written by Joe Zachary for CS 3500, September 2013
// Read the entire skeleton carefully and completely before you
// do anything else!

// Version 1.1 (9/22/13 11:45 a.m.)

// Change log:
//  (Version 1.1) Repaired mistake in GetTokens
//  (Version 1.1) Changed specification of second constructor to
//                clarify description of how validation works

// (Daniel Kopta) 
// Version 1.2 (9/10/17) 

// Change log:
//  (Version 1.2) Changed the definition of equality with regards
//                to numeric tokens


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SpreadsheetUtilities
{
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  The allowed symbols are non-negative numbers written using double-precision 
    /// floating-point syntax; variables that consist of a letter or underscore followed by 
    /// zero or more letters, underscores, or digits; parentheses; and the four operator 
    /// symbols +, -, *, and /.  
    /// 
    /// Spaces are significant only insofar that they delimit tokens.  For example, "xy" is
    /// a single variable, "x y" consists of two variables "x" and y; "x23" is a single variable; 
    /// and "x 23" consists of a variable "x" and a number "23".
    /// 
    /// Associated with every formula are two delegates:  a normalizer and a validator.  The
    /// normalizer is used to convert variables into a canonical form, and the validator is used
    /// to add extra restrictions on the validity of a variable (beyond the standard requirement 
    /// that it consist of a letter or underscore followed by zero or more letters, underscores,
    /// or digits.)  Their use is described in detail in the constructor and method comments.
    /// </summary>
    public class Formula
    {
        // A list which contains a tokenized version of this formula. Tokens contained in this 
        // list will be valid and in an order that is syntactically correct. 
        private List<string> tokens;

        // Contains the set of all varaibles contained in this formula. The variables are stored in
        // normalized form. 
        private HashSet<string> variables;


        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically invalid,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer is the identity function, and the associated validator
        /// maps every string to true.  
        /// </summary>
        public Formula(String formula) :
            this(formula, s => s, s => true)
        {
        }

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically incorrect,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer and validator are the second and third parameters,
        /// respectively.  
        /// 
        /// If the formula contains a variable v such that normalize(v) is not a legal variable, 
        /// throws a FormulaFormatException with an explanatory message. 
        /// 
        /// If the formula contains a variable v such that isValid(normalize(v)) is false,
        /// throws a FormulaFormatException with an explanatory message.
        /// 
        /// Suppose that N is a method that converts all the letters in a string to upper case, and
        /// that V is a method that returns true only if a string consists of one letter followed
        /// by one digit.  Then:
        /// 
        /// new Formula("x2+y3", N, V) should succeed
        /// new Formula("x+y3", N, V) should throw an exception, since V(N("x")) is false
        /// new Formula("2x+y3", N, V) should throw an exception, since "2x+y3" is syntactically incorrect.
        /// </summary>
        public Formula(String formula, Func<string, string> normalize, Func<string, bool> isValid)
        {
            variables = new HashSet<string>();
            // clean, validate and normalize tokens. And since we are checking variables, we may as well add them to
            // the variables set.
            List<string> validCleanedTokens = CleanAndValidate(GetTokens(formula), normalize, isValid);

            // verify correct syntax
            VerifySyntaxAndGetVariables(validCleanedTokens);

            // store valid function
            tokens = validCleanedTokens;
        }



        /// <summary>
        /// Takes an IEnumerator<string> object which enumerates the individual tokens which make up
        /// a formula, normalizes each token using the provided normalize deligate and checks 
        /// that each token is valid. If a token is not valid throws a a FormulaFormatException with an 
        /// explanatory message. 
        /// 
        /// Will return a list of all valid tokens which are cleaned and validated.
        /// </summary>

        /// <returns></returns>
        private List<string> CleanAndValidate(IEnumerable<string> tokens, Func<string, string> normalize, Func<string, bool> isValid)
        {
            List<string> t = new List<string>();
            foreach(string token in tokens)
            {

                // normalize
                string v = normalize(token);

                // normalize double values
                v = DoubleNormalize(v);

                // if normalized token is not valid, throw exception.
                if(!IsValidToken(v))
                {
                    string message = "Token, \"" + token + ",\" was invalid ";
                    message += "when normalized to \"" + v + "\"";
                    throw new FormulaFormatException(message);
                }

                // now that we know it is a valid token by normal 
                // rules we can do a simple check for variables
                if(v.StartsWithLetterOrUnderscore())
                {
                    if(!isValid(v))
                    {
                        string message = "Variable, \"" + token + ",\" was invalid ";
                        message += "when normalized to \"" + v + "\"";
                        throw new FormulaFormatException(message);
                    }

                    // we have a valid variable, may as well add it to variables set
                    variables.Add(v);
                }

                t.Add(v);
            }
            return t;
        }

        /// <summary>
        /// Takes in a string value, if it can be parsed to a double d, returns 
        /// the d.ToString() else returns v unchanged. 
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        private string DoubleNormalize(string v)
        {
            double d = 0;
            if(double.TryParse(v, out d))
            {
                return d.ToString();
            }

            return v;
        }

        /// <summary>
        /// Evaluates this Formula, using the lookup delegate to determine the values of
        /// variables.  When a variable symbol v needs to be determined, it should be looked up
        /// via lookup(normalize(v)). (Here, normalize is the normalizer that was passed to 
        /// the constructor.)
        /// 
        /// For example, if L("x") is 2, L("X") is 4, and N is a method that converts all the letters 
        /// in a string to upper case:
        /// 
        /// new Formula("x+7", N, s => true).Evaluate(L) is 11
        /// new Formula("x+7").Evaluate(L) is 9
        /// 
        /// Given a variable symbol as its parameter, lookup returns the variable's value 
        /// (if it has one) or throws an ArgumentException (otherwise).
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, the value is returned.  Otherwise, a FormulaError is returned.  
        /// The Reason property of the FormulaError should have a meaningful explanation.
        ///
        /// This method should never throw an exception.
        /// </summary>
        public object Evaluate(Func<string, double> lookup)
        {
            Stack<double> values = new Stack<double>();
            Stack<string> operators = new Stack<string>();




            // this is the main body of the algorithm where the expression is evaluated
            foreach(string token in tokens)
            {
                // check if token is double
                double operand;
                if(double.TryParse(token, out operand))
                {
                    try
                    {
                        HandleDouble(operand, values, operators);
                    }
                    catch(ArgumentException e)
                    {
                        return new FormulaError(e.Message);
                    }
                }
                // check if token is variable
                else if(token.StartsWithLetterOrUnderscore())
                {
                    try
                    {
                        operand = lookup(token);
                        HandleDouble(operand, values, operators);
                    }
                    catch(ArgumentException e)
                    {

                        return new FormulaError(e.Message);
                    }

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
                    operators.Pop();


                    if(operators.IsAtTop("*") || operators.IsAtTop("/"))
                    {
                        try
                        {
                            ApplyOperatorStack(values, operators);
                        }
                        catch(ArgumentException e)
                        {

                            return new FormulaError(e.Message);
                        }

                    }

                }
            }
            // the last token has been processed.
            if(operators.Count == 0)
            {
                return values.Pop();
            }
            else
            // there should be one operator a + or -
            {
                // the only operator token should be '+' or '-'
                ApplyOperatorStack(values, operators);
                return values.Pop();
            }

        }

        /// <summary>
        /// If * or / is at the top of the operator stack, pops the value stack and pops the operator stack. 
        /// then applies the popped operator to the popped number and t. Pushes the result onto the value stack.
        /// Otherwise, pushes t onto the value stack.
        /// 
        /// </summary>
        /// <param name="t">the double token</param>
        /// <param name="values">a stack containing values for the evaluate method</param>
        /// <param name="operators">a stack containing operators for the evaluate method</param>
        private static void HandleDouble(double t, Stack<double> values, Stack<string> operators)
        {
            // operand is a double, check if operator * or / is at top of stack and apply it
            if(operators.IsAtTop("*") || operators.IsAtTop("/"))
            {
                double result = ApplyOperator(values.Pop(), t, operators.Pop());
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
        /// </summary>
        /// <param name="values">a stack containing values for the evaluate method</param>
        /// <param name="operators">a stack containing operators for the evaluate method</param>
        private static void ApplyOperatorStack(Stack<double> values, Stack<string> operators)
        {

            // pop value stack twice 
            double value2 = values.Pop();
            double value1 = values.Pop();
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
        private static double ApplyOperator(double val1, double val2, string op)
        {
            double result = 0;
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
        /// Enumerates the normalized versions of all of the variables that occur in this 
        /// formula.  No normalization may appear more than once in the enumeration, even 
        /// if it appears more than once in this Formula.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x+y*z", N, s => true).GetVariables() should enumerate "X", "Y", and "Z"
        /// new Formula("x+X*z", N, s => true).GetVariables() should enumerate "X" and "Z".
        /// new Formula("x+X*z").GetVariables() should enumerate "x", "X", and "z".
        /// </summary>
        public IEnumerable<String> GetVariables()
        {
            return variables.ToList();
        }

        /// <summary>
        /// Returns a string containing no spaces which, if passed to the Formula
        /// constructor, will produce a Formula f such that this.Equals(f).  All of the
        /// variables in the string should be normalized.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x + y", N, s => true).ToString() should return "X+Y"
        /// new Formula("x + Y").ToString() should return "x+Y"
        /// </summary>
        public override string ToString()
        {
            string s = "";
            foreach(string t in tokens)
            {
                s += t;
            }

            return s;
        }

        /// <summary>
        /// If obj is null or obj is not a Formula, returns false.  Otherwise, reports
        /// whether or not this Formula and obj are equal.
        /// 
        /// Two Formulae are considered equal if they consist of the same tokens in the
        /// same order.  To determine token equality, all tokens are compared as strings 
        /// except for numeric tokens and variable tokens.
        /// Numeric tokens are considered equal if they are equal after being "normalized" 
        /// by C#'s standard conversion from string to double, then back to string. This 
        /// eliminates any inconsistencies due to limited floating point precision.
        /// Variable tokens are considered equal if their normalized forms are equal, as 
        /// defined by the provided normalizer.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        ///  
        /// new Formula("x1+y2", N, s => true).Equals(new Formula("X1  +  Y2")) is true
        /// new Formula("x1+y2").Equals(new Formula("X1+Y2")) is false
        /// new Formula("x1+y2").Equals(new Formula("y2+x1")) is false
        /// new Formula("2.0 + x7").Equals(new Formula("2.000 + x7")) is true
        /// </summary>
        public override bool Equals(object obj)
        {
            Formula f = obj as Formula;
            return f != null && ToString() == f.ToString();
        }

        /// <summary>
        /// Reports whether f1 == f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return true.  If one is
        /// null and one is not, this method should return false.
        /// </summary>
        public static bool operator ==(Formula f1, Formula f2)
        {
            // check if both formulas are null
            if(ReferenceEquals(f1, null))
            {
                return ReferenceEquals(f2, null);
            }

            // return the overridden value of Equals()
            return f1.Equals(f2);
        }

        /// <summary>
        /// Reports whether f1 != f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return false.  If one is
        /// null and one is not, this method should return true.
        /// </summary>
        public static bool operator !=(Formula f1, Formula f2)
        {
            // check if both formulas are null
            if(ReferenceEquals(f1, null))
            {
                return !ReferenceEquals(f2, null);
            }

            // return the overridden value of Equals()
            return !f1.Equals(f2);
        }

        /// <summary>
        /// Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
        /// case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two 
        /// randomly-generated unequal Formulae have the same hash code should be extremely small.
        /// </summary>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        /// <summary>
        /// Takes a list of valid tokens making up this formula and enumerates each variable.
        /// 
        /// If there is a syntax error in the cleaned tokens, throws a FormulaFormatException. 
        /// </summary>
        private void VerifySyntaxAndGetVariables(List<string> cleanedTokens)
        {

            // C complexity rules
            OneTokenRule(cleanedTokens);
            StartingTokenRule(cleanedTokens);
            EndingTokenRule(cleanedTokens);

            // N complexity rules
            // counter for parentheses rules
            int parenthesesCount = 0;
            for(int i = 0; i < cleanedTokens.Count; i++)
            {
                if(cleanedTokens[i] == "(")
                {
                    parenthesesCount++;
                    ParenthesesFollowRule(cleanedTokens, i);
                }
                else if(cleanedTokens[i] == ")")
                {
                    RightParenthesesRule(--parenthesesCount);
                    ExtraFollowRule(cleanedTokens, i);
                }
                else if(cleanedTokens[i].StartsWithLetterOrUnderscore() || cleanedTokens[i].StartsWithNumber())
                {
                    ExtraFollowRule(cleanedTokens, i);
                }
            }

            BalancedParenthesesRule(parenthesesCount);
        }

        /// <summary>
        /// Extra Follow Rule: Any token that immediately follows a number, a variable, or a closing  
        /// parenthesis must be either an operator or a closing parenthesis.
        /// 
        /// If the Extra Follow Rule is violated, throws a FormulaFormatException
        /// </summary>
        private void ExtraFollowRule(List<string> cleanedTokens, int index)
        {
            string message = "Extra Follow Rule Violation: Any token that immediately follows ";
            message += "a number, a variable, or a closing parenthesis must be either an operator ";
            message += "or a closing parenthesis.";

            // check next token
            if(cleanedTokens.HasNext(index))
            {
                string nextToken = cleanedTokens[index + 1];
                if(!(nextToken.IsOperator() || nextToken == ")"))
                {
                    throw new FormulaFormatException(message);
                }
            }
            // these tokens can end an expression
        }

        /// <summary>
        /// Parentheses Follow Rule: Any token that immediately follows an opening parenthesis or
        /// an operator must be either a number, a variable, or an opening parenthesis.
        /// 
        /// If the Parentheses Follow Rule is violated, throws a FormulaFormatException
        /// </summary>
        private void ParenthesesFollowRule(List<string> cleanedTokens, int index)
        {
            string message = "Parentheses Follow Rule Violation: Any token that immediately follows an ";
            message += "opening parenthesis or an operator must be either a number, a variable, or an ";
            message += "opening parenthesis.";

            // check next token
            if(cleanedTokens.HasNext(index))
            {
                string nextToken = cleanedTokens[index + 1];
                if(!(nextToken.StartsWithNumber() || nextToken.StartsWithLetterOrUnderscore() || nextToken == "("))
                {
                    throw new FormulaFormatException(message);
                }
            }
            // there wasn't a next token ... shouldn't have happened 
            else
            {
                throw new FormulaFormatException(message);
            }
        }

        /// <summary>
        /// Ending Token Rule: The last token of an expression must be a number, a variable, or a closing parenthesis.
        /// 
        /// If the Ending Token Rule is violated, throws a FormulaFormatException
        /// </summary>
        private void EndingTokenRule(List<string> cleanedTokens)
        {
            if(cleanedTokens.Count > 0)
            {
                string endingToken = cleanedTokens[cleanedTokens.Count - 1];
                if(!(endingToken == ")" || endingToken.StartsWithNumber() || endingToken.StartsWithLetterOrUnderscore()))
                {
                    string message = "Ending Token Rule Violation: The last token of an expression must be a number, a ";
                    message += "variable, or a closing parenthesis.";
                    throw new FormulaFormatException(message);
                }
            }
        }

        /// <summary>
        /// Starting Token Rule: The first token of an expression must be a number, a variable, or an opening parenthesis.
        /// 
        /// If Starting Token Rule is violated, throws FormulaFormatException
        /// </summary>
        private void StartingTokenRule(List<string> cleanedTokens)
        {
            if(cleanedTokens.Count > 0)
            {
                string startingToken = cleanedTokens[0];
                if(!(startingToken == "(" || startingToken.StartsWithNumber() || startingToken.StartsWithLetterOrUnderscore()))
                {
                    string message = "Starting Token Rule Violation: The first token of an expression must be ";
                    message += "a number, a variable, or an opening parenthesis";
                    throw new FormulaFormatException(message);
                }
            }

        }

        /// <summary>
        /// One Token Rule: there must be at least one token.
        /// 
        /// If One Token Rule is violated, throws FormulaFormatException
        /// </summary>
        /// <param name="cleanedTokens"></param>
        private void OneTokenRule(List<string> cleanedTokens)
        {
            // one token rule: there must be at least one token
            if(cleanedTokens.Count < 1)
            {
                string message = "One Token Rule Violation: ";
                message += "Formula must contain at least one token.";
                throw new FormulaFormatException(message);
            }
        }

        /// <summary>
        /// Balanced Parentheses Rule: The total number of opening parentheses must equal the total number 
        /// of closing parentheses.
        /// 
        /// Takes an int which represents the number of opening parentheses minus the number of closing
        /// parentheses. If Balanced Parentheses rule is violated throws FormulaFormatException.
        /// </summary>
        /// <param name="parenthesesCount"> The number of opening parentheses minus the number of closing
        /// parentheses</param>
        private void BalancedParenthesesRule(int parenthesesCount)
        {
            if(parenthesesCount != 0)
            {
                string message = "Balanced Parentheses Rule Violation: The total number of opening ";
                message += "parentheses must equal the total number of closing parentheses";
                throw new FormulaFormatException(message);
            }
        }

        /// <summary>
        /// Right Parenthesis Rule: When reading tokens from left to right, at no point should 
        /// the number of closing parentheses seen so far be greater than the number of opening 
        /// parentheses seen so far.
        /// 
        /// Takes an int which represents the number of opening parentheses minus the number of closing
        /// parentheses seen thus far when read from left to right. If right parenthesis rule is violated
        /// throws FormulaFormatException.
        /// </summary>
        /// <param name="parenthesesCount"> The number of opening parentheses minus the number of closing
        /// parentheses seen thus far when read from left to right</param>
        private void RightParenthesesRule(int parenthesesCount)
        {
            if(parenthesesCount < 0)
            {
                string message = "Right Parentheses Rule Violation: Number of closing parentheses greater than ";
                message += "opening parentheses when read from left to right";
                throw new FormulaFormatException(message);
            }
        }

        /// <summary>
        /// Given an expression, enumerates the tokens that compose it.  Tokens are left paren;
        /// right paren; one of the four operator symbols; a string consisting of a letter or underscore
        /// followed by zero or more letters, digits, or underscores; a double literal; and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            foreach(String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if(!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }

        }

        /// <summary>
        /// Takes in a string, token, and returns true if it's a valid token
        /// 
        /// Tokens are valid if they are  "(", ")", "+", "-", "*", or "/".
        /// Tokens are also valid if it is a string which consists of a letter
        /// or underscore followed by zero or more letters, underscores,
        /// or digits or is a valid floating point number.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static bool IsValidToken(string token)
        {
            double d = 0;
            // a pattern that matches all valid tokens without white space
            string pattern = @"( ^\($ ) | ( ^\)$ ) | (^-$) | ( ^\+$ ) | ( ^\*$ ) | ( ^/$ ) | ( ^[a-zA-Z_][a-zA-Z\d_]*$ )";
            return Regex.IsMatch(token, pattern, RegexOptions.IgnorePatternWhitespace) || double.TryParse(token, out d);
        }
    }

    /// <summary>
    /// Used to report syntactic errors in the argument to the Formula constructor.
    /// </summary>
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// Used as a possible return value of the Formula.Evaluate method.
    /// </summary>
    public struct FormulaError
    {
        /// <summary>
        /// Constructs a FormulaError containing the explanatory reason.
        /// </summary>
        /// <param name="reason"></param>
        public FormulaError(String reason)
            : this()
        {
            Reason = reason;
        }

        /// <summary>
        ///  The reason why this FormulaError was created.
        /// </summary>
        public string Reason { get; private set; }
    }

    internal static class ExtensionMethods
    {
        /// <summary>
        /// Returns true if string begins with a number
        /// </summary>
        public static bool StartsWithNumber(this string s)
        {
            return (s[0] >= '0' && s[0] <= '9');
        }

        /// <summary>
        /// Returns true if this string begins with a letter of the english alphabet or an underscore
        /// </summary>
        public static bool StartsWithLetterOrUnderscore(this String s)
        {
            return ((s[0] >= 'a' && s[0] <= 'z') || (s[0] >= 'A' && s[0] <= 'Z') || (s[0] == '_'));
        }

        /// <summary>
        /// Returns true if this string is an operator '+', '-', '*' or '/'.
        /// </summary>
        public static bool IsOperator(this String s)
        {
            return (s == "+" || s == "-" || s == "*" || s == "/");
        }

        /// <summary>
        /// Takes a string s and returns true if a string matching s is at the 
        /// top of the stack, else returns false.
        /// </summary>
        public static bool IsAtTop(this Stack<string> stack, string s)
        {
            return (stack.Count > 0 && stack.Peek() == s);
        }

        /// <summary>
        /// Takes an index and returns true if there is an item contained in this
        /// list after that index. Else returns false.
        /// </summary>
        public static bool HasNext(this List<string> list, int index)
        {
            return (list.Count > (index + 1));
        }
    }
}
