using System;
using System.Collections.Generic;
using System.Text;

namespace CSP.Constraints
{
    public class GreaterThan<T> : BinaryConstraint<T>, IConstraint<T> where T : IComparable
    {
        public GreaterThan(Variable<T> first, Variable<T> second) : base(first, second) { }
    
        public override bool evaluate()
        {
            if (First.HasValue && Second.HasValue)
            {
                return First.Value.CompareTo(Second.Value) > 0;
            }
            else return true;
        }
    }
}
