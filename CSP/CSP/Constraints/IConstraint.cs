using System;
using System.Collections.Generic;
using System.Text;

namespace CSP.Constraints
{
    public interface IConstraint<T>
    {
        bool evaluate();
        List<Variable<T>> GetVariables();
    }
}
