using System;
using System.Collections.Generic;
using System.Text;

namespace CSP.Constraints
{
    //All Assigned Variables have different values
    public class AllDifferent<T> : MultivariableConstraint<T>, IConstraint<T>
    {
        public AllDifferent() : base() {}

        public AllDifferent(Variable<T>[] variables) : base(variables) {}

        public override bool evaluate()
        {
            bool occurred = false;
            Dictionary<T, bool> values = new Dictionary<T, bool>();
            foreach(Variable<T> var in Variables)
            {
                if (var.HasValue)
                {
                    if (values.TryGetValue(var.Value, out occurred)) return false;
                    values.Add(var.Value, true);
                }
            }
            return true;
        }
    }
}
