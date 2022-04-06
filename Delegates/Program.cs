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

            var constant4Expression = Expression.Constant(4);
            var lessThan = Expression.LessThan(xExpression, constant4Expression);

            var or = Expression.Or(greaterThan, lessThan);

            var expr = Expression.Lambda<Func<int, bool>>(or, false, new List<ParameterExpression> { xExpression, });
            var func = expr.Compile();

            Console.WriteLine(func(2));
        }
    }
}