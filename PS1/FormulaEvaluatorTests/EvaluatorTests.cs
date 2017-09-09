using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FormulaEvaluator;

namespace FormulaEvaluatorTests
{
    [TestClass]
    public class EvaluatorTests
    {

        // test examples given in assignment
        [TestMethod]
        public void AssignmentExample()
        {
            string expression = "(2 + 3) * 5 + 2";
            // int value = (2 + 3) * 5 + 2;
            int value = 27;
            Assert.AreEqual(Evaluator.Evaluate(expression, x => 0), value);
        }

        [TestMethod]
        public void AssignmentExampleVariable()
        {
            string expression = "(2 + A6) * 5 + 2";
            int expected = 47;
            int actual = Evaluator.Evaluate(expression, x => 7);
            Assert.AreEqual(expected, actual);
        }

        // do some basic 2^n calculations
        [TestMethod]
        public void TwoRaised4()
        {
            // create a string which is 2^n power using multiplication
            string expression = "2*2*2*2";

            int expected = 16;
            int actual = Evaluator.Evaluate(expression, x => 0);

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
            int actual = Evaluator.Evaluate(expression, x => 0);

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
            int actual = Evaluator.Evaluate(expression, x => 0);

            // evaluate string and check against Math.Pow
            Assert.AreEqual(expected, actual);
        }

        //test some invalid tokens
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void InvalidTokenCarrot()
        {
            // expression evaluates to 11 
            string expression = "2+3^2";

            // ^ is an invalid token, should throw exception
            int actual = Evaluator.Evaluate(expression, x => 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void InvalidTokenDollar()
        {
            // expression evaluates to 7, but with invalid token on end
            string expression = "2+3+2$";

            // $ is an invalid token, should throw exception
            int actual = Evaluator.Evaluate(expression, x => 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void InvalidTokenComplicatedExpression()
        {
            // expression evaluates to 7, but with invalid token on end
            string expression = "(2+3) * 2 \n\n\t   /4 %#@@!";

            // $ is an invalid token, should throw exception
            int actual = Evaluator.Evaluate(expression, x => 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void InvalidTokenInVariableName()
        {
            // an expression with an otherwise ok variable name, with invalid token in it
            string expression = "(2+BB3?7) * 2 \n\n\t   /4";

            // $ is an invalid token, should throw exception
            int actual = Evaluator.Evaluate(expression, x => 7);
        }

        // test variable names
        [TestMethod]
        public void ValidVariableNames()
        {
            // an array with several valid variable names
            string[] variables = { "A1", "B2", "AaBbCc1234", "alskdjfalsdkjf1", "TheAnswerToLifeTheUniverseAndEverythingIs42", "pI3141592654", "z129384701923874109" };
            // an array with several just a bunch of values to lookup
            int[] values = { 0, 10, 1234, 2 * 15, 42, 314159265, -18, 300, 254, 122584 };

            // test each variable name in variables
            for(int i = 0; i < variables.Length; i++)
            {
                // the lookup function in this case will look up the value in the values field at the same index
                // B2 should map to 10
                Assert.AreEqual(values[i], Evaluator.Evaluate(variables[i], x => values[i]));
            }
        }

        [TestMethod]
        public void ValidVariableNamesLookupTimesTwo()
        {
            // an array with several valid variable names
            string[] variables = { "A1", "B2", "AaBbCc1234", "alskdjfalsdkjf1", "TheAnswerToLifeTheUniverseAndEverythingIs42", "pI3141592654", "z129384701923874109" };
            // an array with several just a bunch of values to lookup
            int[] values = { 0, 10, 1234, 2 * 15, 42, 314159265, -18, 300, 254, 122584 };

            // test each variable name in variables
            for(int i = 0; i < variables.Length; i++)
            {
                // the lookup function in this case will look up the value in the values field at the same index
                // and add it to itself, IE B2 should evaluate to 20
                Assert.AreEqual(values[i] * 2, Evaluator.Evaluate(variables[i], x => values[i] + values[i]));
            }
        }

        [TestMethod]
        public void InvalidVariableNamesLookup()
        {
            // an array with several invalid variable names
            string[] variables = { "A", "b b", "1B", "AaBbCc", "alskdjfalsdkjf", "TheAnswerToLifeThe\nUniverseAndEverythingIs42", "p I3141592654", "129384701923874109asdfSDFss" };
            // an array with several just a bunch of values to lookup
            int[] values = { 0, 10, 1234, 2 * 15, 42, 314159265, -18, 300, 254, 122584 };

            // test each variable name in variables
            for(int i = 0; i < variables.Length; i++)
            {
                try
                {
                    Assert.AreEqual(values[i] * 2, Evaluator.Evaluate(variables[i], x => values[i] + values[i]));
                    // should never reach the following statement because an exception should always be thrown
                    Assert.Fail();
                }
                catch(ArgumentException)
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
            int expected = 15;
            string expression = "4*3+3";
            Assert.AreEqual(expected, Evaluator.Evaluate(expression, x => 0));

            // test standard order of operations
            // multiplication should be done first
            expected = 13;
            expression = "4+3*3";
            Assert.AreEqual(expected, Evaluator.Evaluate(expression, x => 0));

            // the two previous expressions should not be equal when evaluated
            Assert.AreNotEqual(Evaluator.Evaluate("4+3*3", x => 0), Evaluator.Evaluate("4*3+3", x => 0));

            // force addition to be evaluated first using parenthesis
            expected = 21;
            expression = "(4+3)*3";
            Assert.AreEqual(expected, Evaluator.Evaluate(expression, x => 0));

            // force addition to be evaluated first again using parenthesis
            expected = 24;
            expression = "4*(3+3)";
            Assert.AreEqual(expected, Evaluator.Evaluate(expression, x => 0));

        }

        // check a bunch of division properties
        [TestMethod]
        public void IntDivisionSumTest()
        {
            // integer division at each step should result in 0, 
            // as apposed to truncation at the end which would lead to 1
            string expression = "1/3 + 1/3 + 1/3 + 1/3";
            int expected = 0;

            Assert.AreEqual(expected, Evaluator.Evaluate(expression, x => 0));

            // integer division should result in 2
            expression = "5/3 + 5/3 ";
            expected = 2;

            Assert.AreEqual(expected, Evaluator.Evaluate(expression, x => 0));

        }

        [TestMethod]
        public void DivideBySelf()
        {
            // anything divided by itself should be 1
            string expression = "b3/b3 ";
            int expected = 1;

            for(int i = 1; i < 10; i++)
            {
                int n = (int)Math.Pow(2, i);
                Assert.AreEqual(expected, Evaluator.Evaluate(expression, x => n));
            }

        }

        [TestMethod]
        public void ZeroDivideSomething()
        {
            // zero divided anything should be 0
            string expression = "0/b3 ";
            int expected = 0;

            for(int i = 1; i < 10; i++)
            {
                int n = (int)Math.Pow(2, i);
                Assert.AreEqual(expected, Evaluator.Evaluate(expression, x => n));
            }
        }

        [TestMethod]
        public void DivideByOne()
        {
            // something divided by 1 should equal that something
            string expression = "z58/1 ";

            for(int i = 1; i < 10; i++)
            {
                int n = (int)Math.Pow(2, i);
                Assert.AreEqual(n, Evaluator.Evaluate(expression, x => n));
            }
        }

        [TestMethod]
        public void MultiplyAndDivideLtoR()
        {
            // multiplication and divide should be applied left to right
            string expression = "14 / 7 * 4 / 2  ";
            int expected = 4;

            Assert.AreEqual(expected, Evaluator.Evaluate(expression, x => 0));
        }

        [TestMethod]
        public void FractionDividedByNumber()
        {
            //  a fraction divided by a number is the same as a fraction multiplied
            // by the reciprical of that number
            string expression = "14 / 7 / b1";

            string equivalent = "14 / 7 * 1 / b1";

            for(int i = 1; i < 100; i = i + 2)
            {
                Assert.AreEqual(Evaluator.Evaluate(expression, x => i), Evaluator.Evaluate(equivalent, x => i));
            }

        }

        [TestMethod]
        public void DivideByZero()
        {
            //Cannot divide by zero, should throw exception
            string expression = "14 / 0";

            try
            {
                Evaluator.Evaluate(expression, x => 0);
                //should never reach the next line
                Assert.Fail();
            }
            catch(ArgumentException)
            {
                // if no fail was detected, will pass test
            }

            expression = "b1 / (b1 - b1)";

            for(int i = 1; i < 25; i++)
            {

                try
                {
                    int n = (int)Math.Pow(2, i);
                    Evaluator.Evaluate(expression, x => n);

                    //should never reach the next line
                    Assert.Fail();
                }
                catch(ArgumentException)
                {
                    // if no fail was detected, will pass test
                }
            }

        }

        [TestMethod]
        public void DivideByNegative()
        {
            // a number divided by something negative should equal a negative number 
            string expression = "14 / (0 - 2)";
            int expected = -7;

            Assert.AreEqual(expected, Evaluator.Evaluate(expression, x => 0));
        }

        // multiplication properties
        [TestMethod]
        public void MultiplyByZero()
        {
            // a number multiplied by zero equals zero
            string expression = "18 * 0";
            int expected = 0;

            Assert.AreEqual(expected, Evaluator.Evaluate(expression, x => 0));

            // test it with another operator
            expression = "bbsd45 * 99 + 3";
            expected = 3;

            Assert.AreEqual(expected, Evaluator.Evaluate(expression, x => 0));

            // zero should follow the multiplication
            expression = "bbsd45 * 2 * 4 * 8 * 16 * 32 * 64 * 128";
            expected = 0;

            Assert.AreEqual(expected, Evaluator.Evaluate(expression, x => 0));
        }

        [TestMethod]
        public void MultiplyByOne()
        {
            //something multiplied by 1 equals that something
            string expression = "a4 * 1";


            for(int i = 1; i < 10; i++)
            {
                int n = (int)Math.Pow(2, i);
                Assert.AreEqual(n, Evaluator.Evaluate(expression, x => n));
            }
        }

        // addition properties
        [TestMethod]
        public void AddToZero()
        {
            //something added to 0 equals that something
            string expression = "a4 + 0 ";


            for(int i = 1; i < 10; i++)
            {
                int n = (int)Math.Pow(2, i);
                Assert.AreEqual(n, Evaluator.Evaluate(expression, x => n));
            }
        }

        [TestMethod]
        public void DivisionPrecidence()
        {
            string expression = "4 / 3 + 3";
            int expected = 4;
            Assert.AreEqual(expected, Evaluator.Evaluate(expression, x => 0));

            expression = "4 + 3 / 3";
            expected = 5;
            Assert.AreEqual(expected, Evaluator.Evaluate(expression, x => 0));

            expression = "(4 + 3) / 3";
            expected = 2;
            Assert.AreEqual(expected, Evaluator.Evaluate(expression, x => 0));

            expression = "4 / (3 + 3)";
            expected = 0;
            Assert.AreEqual(expected, Evaluator.Evaluate(expression, x => 0));

            expression = "4 + 3 *2 / 3";
            expected = 6;
            Assert.AreEqual(expected, Evaluator.Evaluate(expression, x => 0));

        }

        // mess with operators
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TooManyOperatorsAtEnd()
        {

            // extra operator at end
            string expression = "14 / 7 * 4 / 2  +";

            Evaluator.Evaluate(expression, x => 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TooManyOperatorsAtStart()
        {

            // extra operator at end
            string expression = " * 14 / 7 * 4 / 2 ";

            Evaluator.Evaluate(expression, x => 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TooManyOperatorsInMiddle()
        {

            // extra operator at end
            string expression = "14 / 7 * 4 * / 2 ";

            Evaluator.Evaluate(expression, x => 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NoOpperators()
        {

            // no opperators
            string expression = "14  7";

            Evaluator.Evaluate(expression, x => 0);

        }

        [TestMethod]
        public void TryToPassNegativeInt()
        {


            string expression = "14 / -7 * 4  / 2 ";

            try
            {
                Evaluator.Evaluate(expression, x => 0);
                // should never reach this line of code
                Assert.Fail();
            }
            catch(ArgumentException)
            {
                // will pass if no fail is detected
            }

        }

        [TestMethod]
        public void MismatchedParenthesis()
        {


            string expression = "(14 / 7)) * 4 / 2 + 5";

            try
            {
                Evaluator.Evaluate(expression, x => 0);
                // should never reach this line of code
                Assert.Fail();
            }
            catch(ArgumentException)
            {
                // will pass if no fail is detected
            }

            expression = "((14 / 7) * 4 / 2 ";

            try
            {
                Evaluator.Evaluate(expression, x => 0);
                // should never reach this line of code
                Assert.Fail();
            }
            catch(ArgumentException)
            {
                // will pass if no fail is detected
            }

            expression = "(14 / 7) * 4 / 2 ) ";

            try
            {
                Evaluator.Evaluate(expression, x => 0);
                // should never reach this line of code
                Assert.Fail();
            }
            catch(ArgumentException)
            {
                // will pass if no fail is detected
            }

            expression = "((14 / 7 * 4 / 2 ";

            try
            {
                Evaluator.Evaluate(expression, x => 0);
                // should never reach this line of code
                Assert.Fail();
            }
            catch(ArgumentException)
            {
                // will pass if no fail is detected
            }

            expression = "14 / 7 * 4 / 2 ))";

            try
            {
                Evaluator.Evaluate(expression, x => 0);
                // should never reach this line of code
                Assert.Fail();
            }
            catch(ArgumentException)
            {
                // will pass if no fail is detected
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TryImplicitMultiplication()
        {
            string expression = "2(14+5)";
            Evaluator.Evaluate(expression, x => 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TryPassingNullString()
        {
            string expression = null;
            Evaluator.Evaluate(expression, x => 0);
        }

        [TestMethod]
        public void NestedParenthesis()
        {
            string expression = "(5 + ((((b4)))))";
            int expected = 10;
            Assert.AreEqual(expected, Evaluator.Evaluate(expression, x => 5));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void EmptyParenthesis()
        {
            string expression = "()";
            Evaluator.Evaluate(expression, x => 5);
        }

        [TestMethod]
        public void EveryOperandAndVariable()
        {
            string expression = "(2 * b5) - 4 + 7 / 4";
            int expected = 7;
            Assert.AreEqual(expected, Evaluator.Evaluate(expression, x => 5));
        }

        [TestMethod]
        public void EveryOperandAndVariableCrazyWWhiteSpace()
        {
            string expression = "(2 + b5 * 3) - (4 + 7 / 4 -3+8-1* \t\t\t\n  zd56) / (b5)";
            int expected = 6;
            Assert.AreEqual(expected, Evaluator.Evaluate(expression, x => x == "b5"? 2: 5));
        }
    }
}
