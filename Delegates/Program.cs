using System;

namespace Delegates
{
    internal class Program
    {
        public delegate void WriteMessageFunction(string message);

        public static void WriteMessage(string message)
        {
            Console.WriteLine(message);
        }

        public static void WriteTimelyMessage(string message)
        {
            Console.WriteLine(message + " " + DateTime.Now);
        }

        static void Main(string[] args)
        {
            var selection = Console.ReadLine();

            WriteMessageFunction func;

            if (selection == "1")
            {
                func = WriteTimelyMessage;
            }
            else
            {
                func = WriteMessage;
            }

            func("Hello World");
        }
    }
}