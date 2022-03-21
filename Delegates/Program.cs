using System;

namespace Delegates
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var selection = Console.ReadLine();

            Func<string, bool> func;

            if (selection == "1")
            {
                func = message =>
                {
                    Console.WriteLine(message + " " + DateTime.Now);
                    return true;
                }
                ;
            }
            else
            {
                func = message =>
                {
                    Console.WriteLine(message);
                    return true;
                };
            }

            var result =    ExecuteWrite(func);
        }

        private static bool ExecuteWrite(Func<string, bool> func)
        {
            return func("Hello World");
        }
    }
}