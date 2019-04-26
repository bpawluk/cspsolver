using CSP.Constraints;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSP
{
    public class Problem<T>
    {
        public Variable<T>[] Variables { get; set; }
        public IConstraint<T>[] Constraints { get; }

        public Problem(Variable<T>[] variables, IConstraint<T>[] constraints)
        {
            Variables = variables;
            Constraints = constraints;
        }

        public Solution<T> Solve(Config config)
        {
            Solver <T> solver = new Solver<T>();
            return solver.Solve(this, config);
        }

        public void Clear()
        {
            foreach (Variable<T> variable in Variables) variable.RemoveValue(); 
        }
    }
}
