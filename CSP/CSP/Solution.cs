using System;
using System.Collections.Generic;
using System.Linq;
using RazorEngine;
namespace CSP
{
    public class Solution<T>
    {
        public List<Dictionary<int, T>> Solutions { get; private set; } = new List<Dictionary<int, T>>();
        public void AddSolution(Stack<Variable<T>> solution)
        {
            Stack<Variable<T>> copy = solution.Clone();
            Dictionary<int, T> newSolution = new Dictionary<int, T>();
            while (copy.Count != 0)
            {
                Variable<T> variable = copy.Pop();
                newSolution.Add(variable.ID, variable.Value);
            }
            Solutions.Add(newSolution);
        }

        public void Clear() { Solutions = new List<Dictionary<int, T>>(); }

        public void Print()
        {
            int solutionNumber = 0;
            foreach(Dictionary<int, T> solution in Solutions)
            {
                var keys = solution.Keys.OrderBy(key => key);
                Console.Write("SOLUTION #" + solutionNumber++);
                int i = 0;
                foreach(int key in keys)
                {
                    if (i++ % Math.Sqrt(keys.Count()) == 0) Console.WriteLine();
                    Console.Write(solution[key] + " ");
                }
                Console.Write("\n\n");
            }
        }

        public void WriteToHTML()
        {
            int solutionNumber = 0;
            foreach (Dictionary<int, T> solution in Solutions)
            {
                var keys = solution.Keys.OrderBy(key => key);
                Console.Write("SOLUTION #" + solutionNumber++);
                int i = 0;
                foreach (int key in keys)
                {
                    if (i++ % Math.Sqrt(keys.Count()) == 0) Console.WriteLine();
                    Console.Write(solution[key] + " ");
                }
                Console.Write("\n\n");
            }
        }
    }
}
