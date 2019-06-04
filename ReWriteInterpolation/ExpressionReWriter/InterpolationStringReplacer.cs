using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Tests
{
    public class InterpolationStringReplacer<T> : ExpressionVisitor
    {
        private readonly List<PatternMachingStructure> patternMatchingList;

        public InterpolationStringReplacer()
        {
            patternMatchingList = new List<PatternMachingStructure>
            {
                new PatternMachingStructure
                {
                    FilterPredicate = x => FormatMethodsWithObjects.Contains(x),
                    SelectorArgumentsFunc = x => x.Arguments.Skip(1),
                    ReturnFunc = InterpolationToStringConcat
                },
                new PatternMachingStructure
                {
                    FilterPredicate = x => FormatMethodWithArrayParameter.Contains(x),
                    SelectorArgumentsFunc = x => ((NewArrayExpression) x.Arguments.Last()).Expressions,
                    ReturnFunc = InterpolationToStringConcat
                },
                new PatternMachingStructure()
                {
                    FilterPredicate = x => FormatMethods.All(xx => xx != x),
                    SelectorArgumentsFunc = x => x.Arguments,
                    ReturnFunc = (node, _) => base.VisitMethodCall(node)
                }
            };
        }

        private IEnumerable<MethodInfo> FormatMethods =>
            typeof(string).GetMethods().Where(x => x.Name.Contains("Format"));

        private IEnumerable<MethodInfo> FormatMethodsWithObjects => FormatMethods
            .Where(x =>
                x.GetParameters().All(xx =>
                    xx.ParameterType == typeof(string) || xx.ParameterType == typeof(object)));

        private IEnumerable<MethodInfo> FormatMethodWithArrayParameter => FormatMethods
            .Where(x => x.GetParameters().Any(xx => xx.ParameterType == typeof(object[])));

        private MethodInfo StringConcatMethod =>
            typeof(string).GetMethod("Concat", new Type[] {typeof(object), typeof(object)});

        private string RegexPattern = @"\{\d\}";

        private Expression InterpolationToStringConcat(MethodCallExpression node,
            IEnumerable<Expression> formatArguments)
        {
            var formatString = node.Arguments.First();
            var argumentStrings = Regex.Split(formatString.ToString(), RegexPattern).Select(Expression.Constant);
            var merge = argumentStrings.Merge(formatArguments, new ExpressionComparer());
            var result = merge.Aggregate((acc, cur) => Expression.Add(acc, cur, StringConcatMethod));
            return result;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            var pattern = patternMatchingList.First(x => x.FilterPredicate(node.Method));
            var arguments = pattern.SelectorArgumentsFunc(node);
            var expression = pattern.ReturnFunc(node, arguments);
            return expression;
        }
    }
}