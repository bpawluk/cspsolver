using System;
using System.Collections.Generic;
using System.Text;

namespace CSP.Constraints
{
    public abstract class BinaryConstraint<T> : IConstraint<T>
    {
        public Variable<T> First { get; set; }
        public Variable<T> Second { get; set; }

        public BinaryConstraint(Variable<T> first, Variable<T> second)
        {
            First = first;
            Second = second;
            First.AddConstraint(this);
            Second.AddConstraint(this);
        }

        public abstract bool evaluate();

        public List<Variable<T>> GetVariables()
        {
            return new List<Variable<T>>(new Variable<T>[] { First, Second });
        }
    }
}
