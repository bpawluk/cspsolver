using CSP.Constraints;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CSP.Input
{
    public class SkyscraperParser : ICSPParser<int>
    {
        public (Variable<int>[], IConstraint<int>[]) ParseCSP(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);

            //Load variables
            int size = int.Parse(lines[0]);
            int[] domain = CreateDomain(size);
            Variable<int>[] variables = new Variable<int>[size * size];
            for (int i = 0; i < variables.Length; i++) variables[i] = new Variable<int>(domain);

            //Add specified constraints
            List<IConstraint<int>> constraints = new List<IConstraint<int>>();
            for (int i = 1; i <= 4; i++) parseConstraints(constraints, variables, lines[i].Split(';'));

            //Add constraints on rows and columns
            for (int i = 0; i < size; i++)
            {
                List<Variable<int>> row = new List<Variable<int>>();
                List<Variable<int>> column = new List<Variable<int>>();
                for (int j = 0; j < size; j++)
                {
                    row.Add(variables[i * size + j]);
                    column.Add(variables[j * size + i]);
                }
                constraints.Add(new AllDifferent<int>(row.ToArray()));
                constraints.Add(new AllDifferent<int>(column.ToArray()));
            }

            return (variables, constraints.ToArray());
        }

        private void parseConstraints(List<IConstraint<int>> constraints, Variable<int>[] variables, string[] line)
        {
            int length = variables.Length;
            int rowSize = (int)Math.Sqrt(length);

            switch(line[0].ToUpper())
            {
                case "G":
                    for (int i = 0; i < rowSize; i++)
                    {
                        if (int.Parse(line[i + 1]) != 0)
                        {
                            int index = 0;
                            Variable<int>[] constrictedVariables = new Variable<int>[rowSize];
                            for (int j = i; j <= length - rowSize + i; j += rowSize)
                            {
                                constrictedVariables[index++] = variables[j];
                            }
                            constraints.Add(new SkyscraperLine(constrictedVariables, line[0] + i, int.Parse(line[i + 1])));
                        }
                    }
                    break;
                case "D":
                    for (int i = 0; i < rowSize; i++)
                    {
                        if (int.Parse(line[i + 1]) != 0)
                        {
                            int index = 0;
                            Variable<int>[] constrictedVariables = new Variable<int>[rowSize];
                            for (int j = length - rowSize + i; j >= i; j -= rowSize)
                            {
                                constrictedVariables[index++] = variables[j];
                            }
                            constraints.Add(new SkyscraperLine(constrictedVariables, line[0] + i, int.Parse(line[i + 1])));
                        }
                    }
                    break;
                case "L":
                    for (int i = 0; i < rowSize; i++)
                    {
                        if (int.Parse(line[i + 1]) != 0)
                        {
                            int index = 0;
                            Variable<int>[] constrictedVariables = new Variable<int>[rowSize];
                            for (int j = i * rowSize; j < rowSize * i + rowSize; j++)
                            {
                                constrictedVariables[index++] = variables[j];
                            }
                            constraints.Add(new SkyscraperLine(constrictedVariables, line[0] + i, int.Parse(line[i + 1])));
                        }
                    }
                    break;
                case "P":
                    for (int i = 0; i < rowSize; i++)
                    {
                        if (int.Parse(line[i + 1]) != 0)
                        {
                            int index = 0;
                            Variable<int>[] constrictedVariables = new Variable<int>[rowSize];
                            for (int j = rowSize * i + rowSize - 1; j >= i * rowSize; j--)
                            {
                                constrictedVariables[index++] = variables[j];
                            }
                            constraints.Add(new SkyscraperLine(constrictedVariables, line[0] + i, int.Parse(line[i + 1])));
                        }
                    }
                    break;
            }
        }

        private int[] CreateDomain(int length)
        {
            int[] domain = new int[length];
            for (int i = 0; i < length;)
            {
                domain[i] = ++i;
            }
            return domain;
        }
    }
}
