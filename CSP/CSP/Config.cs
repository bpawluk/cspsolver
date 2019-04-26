using System;
using System.Collections.Generic;
using System.Text;

namespace CSP
{
    public class Config
    {
        public bool ForwardChecking { get; set; }
        public VariableOrdering VariableOrdering { get; set; } 
        public ValueOrdering ValueOrdering { get; set; }

        public Config(bool forwardChecking = false, VariableOrdering variableOrdering = 0, ValueOrdering valueOrdering = 0)
        {
            ForwardChecking = forwardChecking;
            VariableOrdering = variableOrdering;
            ValueOrdering = valueOrdering;
        }
    }

    public enum VariableOrdering { None, MostConstrainedFirst, LeastPossibleValues }
    public enum ValueOrdering { None, Random, LeastUsed }
}
