using System;
using System.Collections.Generic;
using System.Text;

namespace CSP.Constraints
{
    public abstract class MultivariableConstraint<T> : IConstraint<T>
    {
        public List<Variable<T>> Variables { get; private set; }

        public MultivariableConstraint()
        {
            Variables = new List<Variable<T>>();
        }

        public MultivariableConstraint(Variable<T>[] variables)
        {
            Variables = new List<Variable<T>>(variables);
            foreach (Variable<T> variable in variables) variable.AddConstraint(this);
        }

        public abstract bool evaluate();

        public List<Variable<T>> GetVariables()
        {
            return Variables;
        }
    }
}
