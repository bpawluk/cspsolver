using CSP.Constraints;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CSP.Input
{
    public class FutoshikiParser : ICSPParser<int>
    {
        public (Variable<int>[], IConstraint<int>[]) ParseCSP(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);

            //Load variables
            int size = int.Parse(lines[0]);
            int[] domain = CreateDomain(size);
            int varIndex = 0;
            Variable<int>[] variables = new Variable<int>[size * size];
            for(int i = 2; i < size + 2; i++)
            {
                string[] line = lines[i].Split(';');
                for (int j = 0; j < size; j++)
                {
                    int newVal = int.Parse(line[j]);
                    Variable<int> newVar;
                    if (newVal == 0) newVar = new Variable<int>(domain);
                    else newVar = new Variable<int>(new int[] { newVal });
                    variables[varIndex++] = newVar;
                }
            }

            //Add specified constraints
            List<IConstraint<int>> constraints = new List<IConstraint<int>>();
            for(int i = size + 3; i < lines.Length; i++)
            {
                string[] line = lines[i].Split(';');
                if(line.Length > 1)constraints.Add(new GreaterThan<int>(variables[ComputeIndex(line[1], size)], variables[ComputeIndex(line[0], size)]));
            }

            //Add constraints on rows and columns
            for(int i = 0; i < size; i++)
            {
                List<Variable<int>> row = new List<Variable<int>>();
                List<Variable<int>> column = new List<Variable<int>>();
                for (int j = 0; j < size; j++)
                {
                    row.Add(variables[i*size + j]);
                    column.Add(variables[j*size + i]);
                }
                constraints.Add(new AllDifferent<int>(row.ToArray()));
                constraints.Add(new AllDifferent<int>(column.ToArray()));
            }

            return (variables, constraints.ToArray());
        }

        private int[] CreateDomain(int length)
        {
            int[] domain = new int[length];
            for(int i = 0; i < length;)
            {
                domain[i] = ++i;
            }
            return domain;
        }

        private int ComputeIndex(string coordinates, int size)
        {
            int xCoord = char.ToUpper(coordinates[0]) - 65;
            int yCoord = (int)char.GetNumericValue(coordinates[1]) - 1;
            return xCoord*size + yCoord; 
        }
    }
}
