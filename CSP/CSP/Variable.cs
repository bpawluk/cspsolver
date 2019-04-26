using CSP.Constraints;
using System;
using System.Collections.Generic;

namespace CSP
{
    public class Variable<T>
    {
        private static int _nextID = 0;
        public int ID { get; set; }
        public List<T> Domain { get; private set; }
        public List<T> AvaiableValues { get; private set; }
        public T Value { get; private set; }
        public bool HasValue { get; private set; }
        public List<IConstraint<T>> Constraints { get; }
        public HashSet<Variable<T>> DependentVariables { get; }

        public Variable(T[] domain)
        {
            ID = _nextID++;
            Domain = new List<T>(domain);
            AvaiableValues = new List<T>(domain);
            HasValue = false;
            Constraints = new List<IConstraint<T>>();
            DependentVariables = new HashSet<Variable<T>>();
        }

        public Variable(T[] domain, T value)
        {
            ID = _nextID++;
            Domain = new List<T>(domain);
            AvaiableValues = new List<T>(domain);
            Value = value;
            HasValue = true;
            Constraints = new List<IConstraint<T>>();
            DependentVariables = new HashSet<Variable<T>>();
        }

        public bool AssignValue(T value, bool forwardCheck = false)
        {
            if (Domain.Contains(value))
            {
                Value = value;
                HasValue = true;

                //constraints check
                foreach (IConstraint<T> constraint in Constraints)
                {
                    if (!constraint.evaluate())
                    {
                        RemoveValue();
                        return false;
                    }
                }

                //forward check
                if (forwardCheck)
                {
                    if (!ForwardCheck())
                    {
                        RemoveValue(true);
                        return false;
                    }
                }
                return true;
            }
            else return false;
        }

        public void RemoveValue(bool forwardCheck = false)
        {
            Value = default(T);
            HasValue = false;
            if (forwardCheck) ForwardCheck();
        }

        private bool ForwardCheck()
        {
            foreach (Variable<T> variable in DependentVariables)
                if(!variable.CheckAvaiableValues()) return false;
            return true;
        }

        private bool CheckAvaiableValues()
        {
            if(!HasValue)
            {
                AvaiableValues.Clear();

                foreach (T value in Domain)
                {
                    if (AssignValue(value)) AvaiableValues.Add(value);
                }
                RemoveValue();

                if (AvaiableValues.Count == 0)
                {
                    return false;
                }
            }
            return true;
        }

        public void AddConstraint(IConstraint<T> constraint)
        {
            Constraints.Add(constraint);
            foreach (Variable<T> variable in constraint.GetVariables())
                if (variable != this) DependentVariables.Add(variable);
        }

        public void RemoveConstraint(IConstraint<T> constraint)
        {
            Constraints.Remove(constraint);
            DependentVariables.Clear();
            foreach(IConstraint<T> nextConstraint in Constraints)
                foreach (Variable<T> variable in nextConstraint.GetVariables())
                    if (variable != this) DependentVariables.Add(variable);
        }
    }
}
