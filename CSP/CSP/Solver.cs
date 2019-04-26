using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace CSP
{
    internal class Solver<T>
    {
        private Config _config;
        private Solution<T> _solution;
        private Dictionary<T, int> _valueCount;
        private Random _rng = new Random();
        private int _assignmentsCount = 0;

        public Solution<T> Solve(Problem<T> problem, Config config)
        {
            _config = config;
            problem.Clear();
            _solution = new Solution<T>();

            Variable<T>[] variables = problem.Variables;
            if (_config.VariableOrdering == VariableOrdering.MostConstrainedFirst) variables = problem.Variables.OrderBy(var => var.Constraints.Count).ToArray();
            if (_config.ValueOrdering == ValueOrdering.LeastUsed) _valueCount = new Dictionary<T, int>();
 
            SolvingRecursion(new Stack<Variable<T>>(), new List<Variable<T>>(variables));
            return _solution;
        }

        private void SolvingRecursion(Stack<Variable<T>> assignedVars, List<Variable<T>> remainingVars)
        {
            if (remainingVars.Count == 0)
            {
                _solution.AddSolution(assignedVars);
                return;
            }

            SortVariables(remainingVars);
            Variable<T> nextVar = remainingVars.Last();
            SortValues(nextVar);

            foreach (T value in nextVar.AvaiableValues)
            {
                if(nextVar.AssignValue(value, _config.ForwardChecking))
                {
                    _assignmentsCount++;
                    if (_config.ValueOrdering == ValueOrdering.LeastUsed) IncrementUse(nextVar);
                    remainingVars.RemoveAt(remainingVars.Count - 1);
                    assignedVars.Push(nextVar);
                    SolvingRecursion(assignedVars, remainingVars);
                    Backtrack(assignedVars, remainingVars);
                }
            }

            return;
        }

        private void Backtrack(Stack<Variable<T>> assignedVars, List<Variable<T>> remainingVars)
        {
            Variable<T> lastVar = assignedVars.Pop();
            if (_config.ValueOrdering == ValueOrdering.LeastUsed) DecrementUse(lastVar);
            lastVar.RemoveValue(_config.ForwardChecking);
            remainingVars.Add(lastVar);
        }

        private void IncrementUse(Variable<T> nextVar)
        {
            if(nextVar.HasValue)
            {
                if (_valueCount.ContainsKey(nextVar.Value)) _valueCount[nextVar.Value]++;
                else _valueCount.Add(nextVar.Value, 1);
            }
        }

        private void DecrementUse(Variable<T> nextVar)
        {
            if (nextVar.HasValue)
            {
                if (_valueCount.ContainsKey(nextVar.Value) && _valueCount[nextVar.Value] > 0) _valueCount[nextVar.Value]--;
            }
        }

        private void SortValues(Variable<T> nextVar)
        {
            if (_config.ValueOrdering == ValueOrdering.Random)
                nextVar.AvaiableValues.Sort((first, second) => _rng.Next(0, 3) - 1);
            else if (_config.ValueOrdering == ValueOrdering.LeastUsed)
                nextVar.AvaiableValues.Sort((first, second) => (_valueCount.ContainsKey(first) ? _valueCount[first] : 0).CompareTo((_valueCount.ContainsKey(second) ? _valueCount[second] : 0)));
        }

        private void SortVariables(List<Variable<T>> remainingVars)
        {
            if (_config.VariableOrdering == VariableOrdering.LeastPossibleValues) remainingVars.Sort((first, second) => second.AvaiableValues.Count.CompareTo(first.AvaiableValues.Count));
        }
    }
}
