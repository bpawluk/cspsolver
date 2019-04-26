using System;
using System.Collections.Generic;
using System.Text;

namespace CSP.Constraints
{
    public class SkyscraperLine : MultivariableConstraint<int>, IConstraint<int>
    {
        public string Coordinates { get; }
        public int VisibleCount { get; }

        public SkyscraperLine(string coordinates, int visibleCount) : base()
        {
            Coordinates = coordinates;
            VisibleCount = visibleCount;
        }

        public SkyscraperLine(Variable<int>[] variables, string coordinates, int visibleCount) : base(variables)
        {
            Coordinates = coordinates;
            VisibleCount = visibleCount;
        }

        public override bool evaluate()
        {
            int maxVal = int.MinValue;
            int count = 0;
            foreach(Variable<int> variable in Variables)
            {
                if(!variable.HasValue) return true;
                else if(variable.Value > maxVal)
                {
                    maxVal = variable.Value;
                    count++;
                }
            }
            return count == VisibleCount;
        }
    }
}
