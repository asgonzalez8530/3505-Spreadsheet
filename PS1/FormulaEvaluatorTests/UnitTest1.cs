using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FormulaEvaluator;

namespace FormulaEvaluatorTests
{
    [TestClass]
    public class UnitTest1
    {
        

        [TestMethod]
        public void TestGivenExample()
        {
            string expression = "(2 + 3) * 5 + 2";
            // int value = (2 + 3) * 5 + 2;
            int value = 27;
            Assert.AreEqual(Evaluator.Evaluate(expression, x => 0), value);
        }

        [TestMethod]
        public void Test2Raised4()
        {
            // create a string which is 2^n power using multiplication
            string expression = "2*2*2*2";

            int expected = 16;
            int actual = Evaluator.Evaluate(expression, x => 0);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test2RaisedN()
        {
            // create a string which is 2^n power using multiplication
            string expression = "";
            // raise to the n'th power
            int n = 30;
            for (int i = 1; i < n; i++)
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
        public void Test2RaisedNTonOfWhitespace()
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


    }
}
