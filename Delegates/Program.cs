using System;

namespace Delegates
{
    internal class Program
    {
        public static bool WriteMessage(string message)
        {
            Console.WriteLine(message);
            return true;
        }

        public static bool WriteTimelyMessage(string message)
        {
            Console.WriteLine(message + " " + DateTime.Now);
            return true;
        }

        static void Main(string[] args)
        {
            var selection = Console.ReadLine();

            Func<string, bool> func;

            if (selection == "1")
            {
                func = WriteTimelyMessage;
            }
            else
            {
                func = WriteMessage;
            }

            var result =    ExecuteWrite(func);
        }

        private static bool ExecuteWrite(Func<string, bool> func)
        {
            return func("Hello World");
        }
    }
}