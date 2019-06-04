using System.Collections.Generic;
using System.Linq.Expressions;

namespace Tests
{
    public class ExpressionComparer : IEqualityComparer<Expression>
    {
        public bool Equals(Expression x, Expression y)
        {
            return x.ToString().Equals(y.ToString());
        }

        public int GetHashCode(Expression obj)
        {
            return obj.ToString().GetHashCode();
        }
    }
}