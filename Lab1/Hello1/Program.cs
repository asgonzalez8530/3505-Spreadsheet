using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hello1
{

    class HelloWorld
    {
        /// <summary>
        /// Writes "Hello World" to the console and waits for enter key to be pressed
        /// to close the console window. 
        /// </summary>
        static void Main(string[] args)
        {
            Console.Out.WriteLine("Hello World");

            Console.Out.Write("This is a change");
            Console.In.ReadLine();
        }
    }

}
