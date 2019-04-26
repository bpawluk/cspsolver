using System;
using System.Collections.Generic;
using System.Text;

namespace CSP.Constraints
{
    public abstract class UnaryConstraint<T> : IConstraint<T>
    {
        public Variable<T> Variable { get; set; }

        public UnaryConstraint(Variable<T> variable)
        {
            Variable = variable;
            Variable.AddConstraint(this);
        }

        public abstract bool evaluate();

        public List<Variable<T>> GetVariables()
        {
            return new List<Variable<T>>(new Variable<T>[] { Variable });
        }
    }
}
