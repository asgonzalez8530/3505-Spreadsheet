// Tests written by Aaron Bellis u0981638 for PS3 solution in 
// cs3500 2017 fall semester
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using System.Collections.Generic;

namespace FormulaClassTests
{
    [TestClass]
    public class FormulaClassTests
    {
        // test examples given in assignment
        [TestMethod]
        public void AssignmentExample()
        {

            string expression = "(2 + 3) * 5 + 2";
            Formula formula = new Formula(expression);
            // int value = (2 + 3) * 5 + 2;
            double value = 27;
            Assert.AreEqual(formula.Evaluate(x => 0), value);
        }

        [TestMethod]
        public void AssignmentExampleVariable()
        {
            string expression = "(2 + A6) * 5 + 2";
            int expected = 47;
            Formula formula = new Formula(expression);
            double actual = (double)formula.Evaluate(x => 7.0);
            Assert.AreEqual(expected, actual);
        }

        // do some basic 2^n calculations
        [TestMethod]
        public void TwoRaised4()
        {
            // create a string which is 2^n power using multiplication
            string expression = "2*2*2*2";

            int expected = 16;
            Formula formula = new Formula(expression);
            double actual = (double)formula.Evaluate(x => 0);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TwoRaisedN()
        {
            // create a string which is 2^n power using multiplication
            string expression = "";
            // raise to the n'th power
            int n = 30;
            for(int i = 1; i < n; i++)
            {
                expression = expression + "2*";
            }
            expression = expression + "2";

            int expected = (int)Math.Pow(2, n);
            Formula formula = new Formula(expression);
            double actual = (double)formula.Evaluate(x => 0);

            // evaluate string and check against Math.Pow
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TwoRaisedNTonOfWhitespace()
        {
            // create a string which is 2^n power using multiplication
            string expression = "";
            // raise to the n'th power
            int n = 30;
            for(int i = 1; i < n; i++)
            {
                expression = expression + "2\t\t\n  * \t\t\n     \t\n";
            }
            expression = expression + "2";

            int expected = (int)Math.Pow(2, n);
            Formula formula = new Formula(expression);
            double actual = (double)formula.Evaluate(x => 0);

            // evaluate string and check against Math.Pow
            Assert.AreEqual(expected, actual);
        }

        //test some invalid tokens
        [TestMethod]
        [Ignore]
        [ExpectedException(typeof(FormulaFormatException))]
        public void InvalidTokenCarrot()
        {
            // expression evaluates to 11 
            string expression = "2+3^2";

            // ^ is an invalid token, should throw exception
            Formula formula = new Formula(expression);
           
        }

        [TestMethod]
        [Ignore]
        [ExpectedException(typeof(FormulaFormatException))]
        public void InvalidTokenDollar()
        {
            // expression evaluates to 7, but with invalid token on end
            string expression = "2+3+2$";

            // $ is an invalid token, should throw exception
            Formula formula = new Formula(expression);
            
        }

        [TestMethod]
        [Ignore]
        [ExpectedException(typeof(FormulaFormatException))]
        public void InvalidTokenComplicatedExpression()
        {
            // expression evaluates to 7, but with invalid token on end
            string expression = "(2+3) * 2 \n\n\t   /4 %#@@!";

            // $ is an invalid token, should throw exception
            Formula formula = new Formula(expression);
            
        }

        [TestMethod]
        [Ignore]
        [ExpectedException(typeof(FormulaFormatException))]
        public void InvalidTokenInVariableName()
        {
            // an expression with an otherwise ok variable name, with invalid token in it
            string expression = "(2+BB3?7) * 2 \n\n\t   /4";

            // $ is an invalid token, should throw exception
            Formula formula = new Formula(expression);
            
        }

        // test variable names
        [TestMethod]
        public void ValidVariableNames()
        {
            // an array with several valid variable names
            string[] variables = { "A1", "B2", "AaBbCc1234", "alskdjfalsdkjf1", "TheAnswerToLifeTheUniverseAndEverythingIs42", "pI3141592654", "z129384701923874109" };
            // an array with several just a bunch of values to lookup
            double[] values = { 0, 10, 1234, 2 * 15, 42, 314159265, -18, 300, 254, 122584 };

            Formula formula;

            // test each variable name in variables
            for(int i = 0; i < variables.Length; i++)
            {
                formula = new Formula(variables[i]);

                // the lookup function in this case will look up the value in the values field at the same index
                // B2 should map to 10
                Assert.AreEqual(values[i], formula.Evaluate(x => values[i]));
            }
        }

        [TestMethod]
        public void ValidVariableNamesLookupTimesTwo()
        {
            // an array with several valid variable names
            string[] variables = { "A1", "B2", "AaBbCc1234", "alskdjfalsdkjf1", "TheAnswerToLifeTheUniverseAndEverythingIs42", "pI3141592654", "z129384701923874109" };
            // an array with several just a bunch of values to lookup
            double[] values = { 0, 10, 1234, 2 * 15, 42, 314159265, -18, 300, 254, 122584 };
            Formula formula;

            // test each variable name in variables
            for(int i = 0; i < variables.Length; i++)
            {
                formula = new Formula(variables[i]);
                // the lookup function in this case will look up the value in the values field at the same index
                // and add it to itself, IE B2 should evaluate to 20
                Assert.AreEqual(values[i] * 2, formula.Evaluate(x => values[i] + values[i]));
            }
        }

        [TestMethod]
        [Ignore]
        public void InvalidVariableNamesLookup()
        {
            // an array with several invalid variable names
            string[] variables = { "12A", "1b 2b", "1B", "%AaBbCc", "alsk$djfalsdkjf", "TheAnswerToLifeThe\nUniverseAndEverythingIs42", "p I3141592654", "129384701923874109asdfSDFss" };
            // an array with several just a bunch of values to lookup
            double[] values = { 0, 10, 1234, 2 * 15, 42, 314159265, -18, 300, 254, 122584 };
            Formula formula;

            // test each variable name in variables
            for(int i = 0; i < variables.Length; i++)
            {
                try
                {
                    formula = new Formula(variables[i]);
                    
                    // should never reach the following statement because an exception should always be thrown
                    Assert.Fail();
                }
                catch(FormulaFormatException)
                {
                    // exception was caught, continue testing the next invalid name
                }
            }
        }

        // test some order of operations stuff
        [TestMethod]
        public void MultiplicationAndPlusPrecidence()
        {


            // test standard order of operations
            // multiplication should be done first
            double expected = 15;
            string expression = "4*3+3";
            Formula formula = new Formula(expression);
            Assert.AreEqual(expected, formula.Evaluate(x => 0));

            // test standard order of operations
            // multiplication should be done first
            expected = 13;
            expression = "4+3*3";
            formula = new Formula(expression);
            Assert.AreEqual(expected, formula.Evaluate(x => 0));

            // the two previous expressions should not be equal when evaluated
            Formula formula2 = new Formula("4*3+ 3");
            Assert.AreNotEqual(formula2.Evaluate(x => 0), formula.Evaluate(x => 0));

            // force addition to be evaluated first using parenthesis
            expected = 21;
            expression = "(4+3)*3";
            formula = new Formula(expression);
            Assert.AreEqual(expected, formula.Evaluate(x => 0));

            // force addition to be evaluated first again using parenthesis
            expected = 24;
            expression = "4*(3+3)";
            formula = new Formula(expression);
            Assert.AreEqual(expected, formula.Evaluate(x => 0));

        }

        // check a bunch of division properties
        [TestMethod]
        public void IntDivisionSumTest()
        {
            // integer division at each step should result in 0, 
            // as apposed to truncation at the end which would lead to 1
            string expression = "1/3 + 1/3 + 1/3 + 1/3";
            Formula formula = new Formula(expression);
            double expected = 1.0 / 3.0 + 1.0 / 3.0 + 1.0 / 3.0 + 1.0 / 3.0;

            Assert.AreEqual(expected, formula.Evaluate(x => 0));

            // integer division should result in 2
            expression = "5/3 + 5/3 ";
            formula = new Formula(expression);
            expected = 5.0 / 3.0 + 5.0 / 3.0;

            Assert.AreEqual(expected, formula.Evaluate(x => 0));

        }

        [TestMethod]
        public void DivideBySelf()
        {
            // anything divided by itself should be 1
            string expression = "b3/b3 ";
            double expected = 1;
            Formula formula = new Formula(expression);

            for(int i = 1; i < 10; i++)
            {
                double n = Math.Pow(2, i);
                Assert.AreEqual(expected, formula.Evaluate(x => n));
            }

        }

        [TestMethod]
        public void ZeroDivideSomething()
        {
            // zero divided anything should be 0
            string expression = "0/b3 ";
            double expected = 0;
            Formula formula = new Formula(expression);

            for(int i = 1; i < 10; i++)
            {
                double n = Math.Pow(2, i);
                Assert.AreEqual(expected, formula.Evaluate(x => n));
            }
        }

        [TestMethod]
        public void DivideByOne()
        {
            // something divided by 1 should equal that something
            string expression = "_z58/1 ";
            Formula formula = new Formula(expression);

            for(int i = 1; i < 10; i++)
            {
                double n = Math.Pow(2, i);
                Assert.AreEqual(n, formula.Evaluate(x => n));
            }
        }

        [TestMethod]
        public void MultiplyAndDivideLtoR()
        {
            // multiplication and divide should be applied left to right
            string expression = "14 / 7 * 4 / 2  ";
            double expected = 4;
            Formula formula = new Formula(expression);

            Assert.AreEqual(expected, formula.Evaluate(x => 0));
        }

        [TestMethod]
        public void FractionDividedByNumber()
        {
            //  a fraction divided by a number is the same as a fraction multiplied
            // by the reciprical of that number
            string expression = "14 / 7 / b1";
            Formula formula = new Formula(expression);

            string equivalent = "14 / 7 * 1 / b1";
            Formula formula2 = new Formula(equivalent);

            for(int i = 1; i < 100; i = i + 2)
            {
                Assert.AreEqual(formula.Evaluate(x => i), formula2.Evaluate(x => i));
            }

        }

        [TestMethod]
        public void DivideByZero()
        {
            //Cannot divide by zero, should return FormulaError
            string expression = "14 / 0";
            Formula formula = new Formula(expression);

            object o = formula.Evaluate(x => 0);
            string type = o.GetType().ToString();
            Assert.IsInstanceOfType(o, new FormulaError().GetType());



            expression = "b1 / (b1 - b1)";
            formula = new Formula(expression);
            for(int i = 1; i < 25; i++)
            {
                o = formula.Evaluate(x => i);
                type = o.GetType().ToString();
                Assert.IsInstanceOfType(o, new FormulaError().GetType());
            }

        }

        [TestMethod]
        public void DivideByNegative()
        {
            // a number divided by something negative should equal a negative number 
            string expression = "14 / (0 - 2)";
            double expected = -7;
            Formula formula = new Formula(expression);

            Assert.AreEqual(expected, formula.Evaluate(x => 0));
        }

        // multiplication properties
        [TestMethod]
        public void MultiplyByZero()
        {
            // a number multiplied by zero equals zero
            string expression = "18 * 0";
            double expected = 0;
            Formula formula = new Formula(expression);

            Assert.AreEqual(expected, formula.Evaluate(x => 0));

            // test it with another operator
            expression = "_b_bs_d45 * 99 + 3";
            expected = 3;
            formula = new Formula(expression);

            Assert.AreEqual(expected, formula.Evaluate(x => 0));

            // zero should follow the multiplication
            expression = "bbsd45 * 2 * 4 * 8 * 16 * 32 * 64 * 128";
            expected = 0;
            formula = new Formula(expression);

            Assert.AreEqual(expected, formula.Evaluate(x => 0));
        }

        [TestMethod]
        public void MultiplyByOne()
        {
            //something multiplied by 1 equals that something
            string expression = "a4 * 1";
            Formula formula = new Formula(expression);

            for(int i = 1; i < 10; i++)
            {
                double n = Math.Pow(2, i);
                Assert.AreEqual(n, formula.Evaluate(x => n));
            }
        }

        // addition properties
        [TestMethod]
        public void AddToZero()
        {
            //something added to 0 equals that something
            string expression = "a4 + 0 ";
            Formula formula = new Formula(expression);

            for(int i = 1; i < 10; i++)
            {
                double n = Math.Pow(2, i);
                Assert.AreEqual(n, formula.Evaluate(x => n));
            }
        }

        [TestMethod]
        public void DivisionPrecidence()
        {
            string expression = "4 / 3 + 3";
            double expected = 4.0 / 3.0 +3.0;
            Formula formula = new Formula(expression);
            Assert.AreEqual(expected, formula.Evaluate(x => 0));

            expression = "4 + 3 / 3";
            expected = 4.0 + 3.0 / 3.0;
            formula = new Formula(expression);
            Assert.AreEqual(expected, formula.Evaluate(x => 0));

            expression = "(4 + 3) / 3";
            expected = (4.0 + 3.0) / 3.0;
            formula = new Formula(expression);
            Assert.AreEqual(expected, formula.Evaluate(x => 0));

            expression = "4 / (3 + 3)";
            expected = 4.0 / (3.0 + 3.0);
            formula = new Formula(expression);
            Assert.AreEqual(expected, formula.Evaluate(x => 0));

            expression = "4 + 3 *2 / 3";
            expected = 4.0 + 3.0 * 2.0 / 3.0;
            formula = new Formula(expression);
            Assert.AreEqual(expected, formula.Evaluate(x => 0));

        }

        // mess with operators
        [TestMethod]
        [Ignore]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TooManyOperatorsAtEnd()
        {
            // extra operator at end
            string expression = "14 / 7 * 4 / 2  +";
            Formula formula = new Formula(expression);

            
        }

        [TestMethod]
        [Ignore]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TooManyOperatorsAtStart()
        {
            // extra operator at end
            string expression = " * 14 / 7 * 4 / 2 ";
            Formula formula = new Formula(expression);

            
        }

        [TestMethod]
        [Ignore]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TooManyOperatorsInMiddle()
        {
            // extra operator at end
            string expression = "14 / 7 * 4 * / 2 ";
            Formula formula = new Formula(expression);

            
        }

        [TestMethod]
        [Ignore]
        [ExpectedException(typeof(FormulaFormatException))]
        public void NoOpperators()
        {

            // no opperators
            string expression = "14  7";
            Formula formula = new Formula(expression);

            

        }

        [TestMethod]
        [Ignore]
        public void TryToPassNegativeInt()
        {


            string expression = "14 / -7 * 4  / 2 ";

            try
            {
                Formula formula = new Formula(expression);
                
                // should never reach this line of code
                Assert.Fail();
            }
            catch(FormulaFormatException)
            {
                // will pass if no fail is detected
            }

        }

        [TestMethod]
        [Ignore]
        public void MismatchedParenthesis()
        {


            string expression = "(14 / 7)) * 4 / 2 + 5";
            Formula formula;

            try
            {
                formula = new Formula(expression);
                
                // should never reach this line of code
                Assert.Fail();
            }
            catch(FormulaFormatException)
            {
                // will pass if no fail is detected
            }

            expression = "((14 / 7) * 4 / 2 ";

            try
            {
                formula = new Formula(expression);
                
                // should never reach this line of code
                Assert.Fail();
            }
            catch(FormulaFormatException)
            {
                // will pass if no fail is detected
            }

            expression = "(14 / 7) * 4 / 2 ) ";

            try
            {
                formula = new Formula(expression);
                
                // should never reach this line of code
                Assert.Fail();
            }
            catch(FormulaFormatException)
            {
                // will pass if no fail is detected
            }

            expression = "((14 / 7 * 4 / 2 ";

            try
            {
                formula = new Formula(expression);
                
                // should never reach this line of code
                Assert.Fail();
            }
            catch(FormulaFormatException)
            {
                // will pass if no fail is detected
            }

            expression = "14 / 7 * 4 / 2 ))";

            try
            {
                formula = new Formula(expression);
                
                // should never reach this line of code
                Assert.Fail();
            }
            catch(FormulaFormatException)
            {
                // will pass if no fail is detected
            }
        }

        [TestMethod]
        [Ignore]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TryImplicitMultiplication()
        {
            string expression = "2(14+5)";
            Formula formula = new Formula(expression);
            
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TryPassingNullString()
        {
            string expression = null;
            Formula formula = new Formula(expression);
            
        }

        [TestMethod]
        [Ignore]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TryPassingEmptyString()
        {
            string expression = "";
            Formula formula = new Formula(expression);
            
        }

        [TestMethod]
        public void NestedParenthesis()
        {
            string expression = "(5 + ((((b4)))))";
            double expected = 10;
            Formula formula = new Formula(expression);
            Assert.AreEqual(expected, formula.Evaluate(x => 5));
        }

        [TestMethod]
        [Ignore]
        [ExpectedException(typeof(FormulaFormatException))]
        public void EmptyParenthesis()
        {
            string expression = "()";
            Formula formula = new Formula(expression);
            
        }

        [TestMethod]
        public void EveryOperandAndVariable()
        {
            string expression = "(2 * b5) - 4 + 7 / 4";
            double expected = 7.75;
            Formula formula = new Formula(expression);
            
            Assert.AreEqual(expected, formula.Evaluate( x => 5));
        }

        [TestMethod]
        public void EveryOperandAndVariableCrazyWWhiteSpace()
        {
            string expression = "(2.0 + b5 * 3) - (4 + 7 / 4 -3+8-1* \t\t\t\n  zd56) / (b5)";
            double expected = 5.125;
            Formula formula = new Formula(expression);
            Assert.AreEqual(expected, formula.Evaluate(x => x == "b5" ? 2 : 5));
        }

        [TestMethod]
        public void NormalizerValidatorConstructor()
        {
            string expression = "_b5 + 2";
            double expected = 7.0;
            // create object with multi-argument constructor
            Formula formula = new Formula(expression, x => x.ToUpper(), s => s == s.ToUpper());
            Assert.AreEqual(expected, formula.Evaluate(x => 5));
        }

        [TestMethod]
        [Ignore]
        [ExpectedException(typeof(FormulaFormatException))]
        public void BadNormalizerValidatorConstructor()
        {
            string expression = "_b5 + 2";
            double expected = 7.0;
            // create object with multi-argument constructor
            // there can never be valid variables in this, we should expect an error
            Formula formula = new Formula(expression, x => x.ToUpper(), s => s == s.ToLower());
            
        }

        [TestMethod]
        public void LookupThrowsException()
        {
            string expression = "_b5 + 2";
            double expected = 7.0;
            // create object with multi-argument constructor
            
            Formula formula = new Formula(expression, x => x.ToUpper(), s => true);
            object o = formula.Evaluate(x => throw new ArgumentException());
            string type = o.GetType().ToString();
            Assert.IsInstanceOfType(o, new FormulaError().GetType());
        }

        [TestMethod]
        public void GetVariablesDoubles()
        {
            string expression = "_b5 + c17 + _b5";
            double expected = 15.0;
            // create object with multi-argument constructor
            Formula formula = new Formula(expression, x => x.ToUpper(), s => s == s.ToUpper());
            List<string> variables = new List<string>(formula.GetVariables());

            string[] expectedVariables = { "_B5", "C17" }; 

            for (int i = 0; i < variables.Count; i++)
            {
                Assert.AreEqual(expectedVariables[i], variables[i]);
            }

            Assert.AreEqual(expectedVariables.Length, variables.Count);

            Assert.AreEqual(expected, formula.Evaluate(x => 5));
        }

        [TestMethod]
        public void GetVariablesNoVariables()
        {
            string expression = "5 + 5 + 5";
            double expected = 15.0;
            // create object with multi-argument constructor
            Formula formula = new Formula(expression, x => x.ToUpper(), s => s == s.ToUpper());
            List<string> variables = new List<string>(formula.GetVariables());

            string[] expectedVariables = {  };

            

            Assert.AreEqual(expectedVariables.Length, variables.Count);

            Assert.AreEqual(expected, formula.Evaluate(x => 5));
        }

        [TestMethod]
        public void ToStringCheck()
        {
            string expression = "5 + 5 + 5";
            
            // create object with multi-argument constructor
            Formula formula = new Formula(expression);
            string actual = formula.ToString();
            string expected = "5+5+5";

            Assert.AreEqual(expected, actual);

            expression = "b3 + \n\t\t25";
            formula = new Formula(expression, x => x.ToUpper(), s => s == s.ToUpper());
            actual = formula.ToString();
            expected = "B3+25";

            Assert.AreEqual(expected, actual);

        }

        [TestMethod]
        public void EqualsCheck()
        {
            string expression1 = "5 + 5 + \t\t5";
            string expression2 = "5 + 5 + 5";
            string expression3 = "5 + 5 + 5.0";
            // create object with multi-argument constructor
            Formula formula1 = new Formula(expression1);
            Formula formula2 = new Formula(expression2);
            Formula formula3 = new Formula(expression3);

            Assert.IsTrue(formula1.Equals(formula2));
            Assert.IsTrue(formula2.Equals(formula1));
            Assert.IsTrue(formula2.Equals(formula3));

            Assert.IsFalse(formula2.Equals("5+5+5"));
            Assert.IsFalse(formula1.Equals(null));

            string expression4 = "5 + 5";
            Formula formula4 = new Formula(expression4);
            Assert.IsFalse(formula1.Equals(formula4));

        }

        [TestMethod]
        public void EqualsOperator()
        {
            string expression1 = "5 + 5 + \t\t5";
            string expression2 = "5 + 5 + 5";
            string expression3 = "5 + 5 + 5.0";
            // create object with multi-argument constructor
            Formula formula1 = new Formula(expression1);
            Formula formula2 = new Formula(expression2);
            Formula formula3 = new Formula(expression3);

            Assert.IsTrue(formula1 == formula2);
            Assert.IsTrue(formula2 == formula1);
            Assert.IsTrue(formula2 == formula3);

            
            Assert.IsFalse(formula1 == null);
            Assert.IsTrue(null == null);
            Assert.IsFalse(null == formula1);

            string expression4 = "5 + 5";
            Formula formula4 = new Formula(expression4);
            Assert.IsFalse(formula1==formula4);

        }

        [TestMethod]
        public void NotEqualsOperator()
        {
            string expression1 = "5 + 5 + \t\t5";
            string expression2 = "5 + 5 + 5";
            string expression3 = "5 + 5 + 5.0";
            // create object with multi-argument constructor
            Formula formula1 = new Formula(expression1);
            Formula formula2 = new Formula(expression2);
            Formula formula3 = new Formula(expression3);

            Assert.IsFalse(formula1 != formula2);
            Assert.IsFalse(formula2 != formula1);
            Assert.IsFalse(formula2 != formula3);


            Assert.IsTrue(formula1 != null);
            Assert.IsFalse(null != null);
            Assert.IsTrue(null != formula1);

            string expression4 = "5 + 5";
            Formula formula4 = new Formula(expression4);
            Assert.IsTrue(formula1 != formula4);

        }

        [TestMethod]
        public void GetHashcodeCheck()
        {
            string expression1 = "5 + 5 + \t\t5";
            string expression2 = "5 + 5 + 5";
            string expression3 = "5 + 5 + 5.0";
            // create object with multi-argument constructor
            Formula formula1 = new Formula(expression1);
            Formula formula2 = new Formula(expression2);
            Formula formula3 = new Formula(expression3);

            Assert.IsTrue(formula1.GetHashCode() == formula2.GetHashCode());
            Assert.IsTrue(formula2.GetHashCode() == formula1.GetHashCode());
            Assert.IsTrue(formula2.GetHashCode() == formula3.GetHashCode());
            Assert.IsTrue(formula2.GetHashCode() == "5+5+5".GetHashCode());

            Assert.IsFalse(formula1.GetHashCode() == null);
            
            Assert.IsFalse(null == formula1.GetHashCode());

            string expression4 = "5 + 5";
            Formula formula4 = new Formula(expression4);
            Assert.IsFalse(formula1.GetHashCode() == formula4.GetHashCode());

        }



    }
}
