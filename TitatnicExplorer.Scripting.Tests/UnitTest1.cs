using AgileObjects.ReadableExpressions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using TitanicExplorer.Scripting;
using Xunit;

namespace TitatnicExplorer.Scriptings.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void IsPrime()
        {
            var value = Expression.Parameter(typeof(int), "value");

            var result = ScriptingEngine.IsPrime(value);

            var expr = Expression.Lambda<Func<int, bool>>(result, value);

            //Debug.WriteLine(expr.ToReadableString());

            var func = expr.Compile();

            Assert.False(func(0));
            Assert.False(func(1));
            Assert.True(func(2));
            Assert.True(func(3));
            Assert.False(func(4));
            Assert.True(func(5));
            Assert.False(func(144));
        }

        [Fact]
        public void Test2()
        {
            Func<int, bool> func = (value) =>
            {
                if (value <= 1)
                {
                    return false;
                }

                if (value == 2)
                {
                    return true;
                }

                if ((value % 2) == 0)
                {
                    return true;
                }

                var i = 3;
                var boundary = (int)Math.Floor(Math.Sqrt((double)value));
                while (true)
                {
                    if (i <= boundary)
                    {
                        if ((value % i) == 0)
                        {
                            return false;
                        }

                        i += 2;
                    }
                    else
                    {
                        break;
                    }
                }

                return true;
            };

            Assert.Equal(true, func(3));
        }

        [Fact]
        public void Factorial()
        {
            var value = Expression.Parameter(typeof(int));

            var result = ScriptingEngine.Factorial(value);

            var expr = Expression.Lambda<Func<int, int>>(result, value);

            var func = expr.Compile();

            Assert.Equal(6, func(3));
            Assert.Equal(120, func(5));
        }
    }
}