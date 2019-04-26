using CSP.Constraints;
using RazorEngine;
using RazorEngine.Templating;
using System.Collections.Generic;
using System.IO;

namespace CSP.Output
{
    public class FutoshikiWriter
    {
        private Problem<int> _problem;
        private Solution<int> _solution;

        public FutoshikiWriter(Problem<int> problem, Solution<int> solution)
        {
            _problem = problem;
            _solution = solution;
            if (_solution.Solutions.Count == 0) _solution.AddSolution(new Stack<Variable<int>>(problem.Variables));
        }

        public void WriteSolution(string filePath)
        {
            Dictionary<(int, int), bool> constraints = new Dictionary<(int, int), bool>();
            foreach (IConstraint<int> constraint in _problem.Constraints)
                if (constraint.GetType() == typeof(GreaterThan<int>))
                    constraints.Add((((GreaterThan<int>)constraint).First.ID, ((GreaterThan<int>)constraint).Second.ID), true); 
            string template =
            @"<!DOCTYPE html>
            <html lang=""en"" xmlns=""http://www.w3.org/1999/xhtml"">
            <head>
             <meta charset=""utf-8""/>
             <link rel=""stylesheet"" href=""styles.css"">
             <title>@Model.Title</title>
            </head>
            <body>
             <h1>@Model.Title</h1>
             @{
               int solutionNumber = 1;
               foreach (Dictionary<int, int> solution in Model.Solutions)
               {
                <h3>SOLUTION #@solutionNumber</h3>
                solutionNumber++;
                <table>
                @{
                  var keys = solution.Keys.OrderBy(key => key).ToArray();
                  int size = keys.Length;
                  int rowSize = (int)Math.Sqrt(size);
                  <tr>
                   <th></th>
                  @for(int i = 1; i <= rowSize; i++)
                  {
                   <th>@i</th>
                   @if(i != rowSize) {<td class=""constraint""></td>}
                  }
                  </tr>
                  for (int i = 0; i < size; i += rowSize)
                  {
                   <tr>
                    <th>@Convert.ToChar(i/rowSize+65)</th>
                   @for (int j = 0; j < rowSize; j++)
                   {
                    <td>@solution[keys[i+j]]</td>
                    @if(j != rowSize - 1) 
                    {
                     <td class=""constraint"">
                     @{
                       if(Model.Constraints.ContainsKey((i+j, i+j+1))) 
                       {
                        <p>&gt</p>
                       }
                       else if(Model.Constraints.ContainsKey((i+j+1, i+j))) 
                       {
                        <p>&lt</p>
                       }
                      }
                     </td>
                    }
                   }
                   </tr>
                   <tr>
                    <td class=""constraint""></td>
                    @for (int j = 0; j < rowSize; j++)
                    {
                     <td class=""constraint"">
                     @{
                       if(Model.Constraints.ContainsKey((i+j, i+j+rowSize))) 
                       {
                        <p class=""rotate"">&gt</p>
                       }
                       else if(Model.Constraints.ContainsKey((i+j+rowSize, i+j))) 
                       {
                        <p class=""rotate"">&lt</p>
                       }
                      }
                     </td>
                     @if(j != rowSize - 1) 
                     {
                      <td class=""constraint""></td>
                     }
                    }
                   </tr>
                  }
                 }
                </table>
               }
              }
             </body>
            </html>";
            string title = filePath.Split('/')[filePath.Split('/').Length - 1].Split('.')[0];
            string result = Engine.Razor.RunCompile(template, "templateKey", null, new { Title = title, Solutions = _solution.Solutions, Constraints = constraints });
            File.WriteAllText(filePath, result);
        }
    }
}
