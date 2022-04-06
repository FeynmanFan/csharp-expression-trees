using System;
using System.Linq.Expressions;

namespace Delegates
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var xExpression = Expression.Parameter(typeof(int), "x");
            var constantExpression = Expression.Constant(12);
            var greaterThan = Expression.GreaterThan(xExpression, constantExpression);

            var expr = Expression.Lambda<Func<int, bool>>(greaterThan, false, new List<ParameterExpression> { xExpression, });
            var func = expr.Compile();

            Console.WriteLine(func(11));
        }
    }
}