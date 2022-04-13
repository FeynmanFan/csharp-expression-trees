namespace TitanicExplorer.Scripting
{
    using System.Linq.Expressions;

    public class ScriptingEngine
    {
        public static Expression IsPrime(ParameterExpression value)
        {
            var label = Expression.Label();

            var result = Expression.Parameter(typeof(bool), "result");

            var returnLabel = Expression.Label(typeof(bool));

            var valueLessThanEqualToOne = Expression.LessThanOrEqual(value, Expression.Constant(1));
            var valueEqualTwo = Expression.Equal(value, Expression.Constant(2));
            var valueModTwoZero = Expression.Equal(Expression.Modulo(value, Expression.Constant(2)), Expression.Constant(0));

            var sqRt = typeof(Math).GetMethod("Sqrt");
            var floor = typeof(Math).GetMethod("Floor", new Type[] { typeof(double) });

            var valueSqRt = Expression.Call(null, sqRt, Expression.Convert(value, typeof(double)));

            var evalFunction = Expression.Convert(Expression.Call(null, floor, valueSqRt), typeof(int));

            var boundary = Expression.Variable(typeof(int), "boundary");

            var i = Expression.Variable(typeof(int), "i");

            var modBlock = Expression.Block(
                new[] { i, boundary },
                Expression.IfThen(
                    Expression.Equal(Expression.Modulo(value, i), Expression.Constant(0)),
                    Expression.Return(label, Expression.Constant(false))
                ),
                Expression.AddAssign(i, Expression.Constant(2))
                );

            BlockExpression block = Expression.Block(
                new[] { result, i, boundary },
                    Expression.IfThen(
                            valueLessThanEqualToOne,
                            Expression.Return(returnLabel, Expression.Constant(false))
                    ),
                    Expression.IfThen(
                            valueEqualTwo,
                            Expression.Return(returnLabel, Expression.Constant(true))
                    ),
                    Expression.IfThen(
                            valueModTwoZero,
                            Expression.Return(returnLabel, Expression.Constant(true))
                    ),

                    Expression.Assign(i, Expression.Constant(3)),
                    Expression.Assign(boundary, evalFunction),
                    Expression.Loop(
                        Expression.IfThenElse
                        (
                            Expression.LessThanOrEqual(i, boundary),
                            modBlock,
                            Expression.Break(label)
                        ),
                        label
                        ),
                    Expression.Return(returnLabel, Expression.Constant(true)),
                    Expression.Label(returnLabel, Expression.Constant(true))
                    );

            return block;
        }

        /// <summary>
        /// From Microsoft Docs: https://docs.microsoft.com/en-us/dotnet/csharp/expression-trees-building
        /// </summary>
        /// <returns>Returns a factorial expression</returns>
        public static Expression Factorial(ParameterExpression value)
        {
            ParameterExpression result = Expression.Variable(typeof(int), "result");

            // Creating a label that represents the return value
            LabelTarget label = Expression.Label(typeof(int));

            var initializeResult = Expression.Assign(result, Expression.Constant(1));

            // This is the inner block that performs the multiplication,
            // and decrements the value of 'n'
            var block = Expression.Block(
                Expression.Assign(result,
                    Expression.Multiply(result, value)),
                Expression.PostDecrementAssign(value)
            );

            // Creating a method body.
            BlockExpression body = Expression.Block(
                new[] { result },
                initializeResult,
                Expression.Loop(
                    Expression.IfThenElse(
                        Expression.GreaterThan(value, Expression.Constant(1)),
                        block,
                        Expression.Break(label, result)
                    ),
                    label
                )
            );

            return body;
        }
    }
}