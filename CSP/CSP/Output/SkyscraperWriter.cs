using CSP.Constraints;
using RazorEngine;
using RazorEngine.Templating;
using System.Collections.Generic;
using System.IO;

namespace CSP.Output
{
    public class SkyscraperWriter
    {
        private Problem<int> _problem;
        private Solution<int> _solution;

        public SkyscraperWriter(Problem<int> problem, Solution<int> solution)
        {
            _problem = problem;
            _solution = solution;
            if (_solution.Solutions.Count == 0) _solution.AddSolution(new Stack<Variable<int>>(problem.Variables));
        }

        public void WriteSolution(string filePath)
        {
            Dictionary<string, int> constraints = new Dictionary<string, int>();
            foreach (IConstraint<int> constraint in _problem.Constraints)
                if (constraint.GetType() == typeof(SkyscraperLine))
                    constraints.Add(((SkyscraperLine)constraint).Coordinates, ((SkyscraperLine)constraint).VisibleCount);
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
                  Dictionary<string, int> constraints = Model.Constraints;
                  int size = keys.Length;
                  int rowSize = (int)Math.Sqrt(size);
                  <tr>
                   <th></th>
                  @for(int i = 0; i < rowSize; i++)
                  {
                   @if(constraints.ContainsKey(""G"" + i)) {<th>@constraints[""G"" + i]</th>}
                   else {<th>X</th>}
                  }
                   <th></th>
                  </tr>
                  for (int i = 0; i < size; i += rowSize)
                  {
                   <tr>
                    @if(constraints.ContainsKey(""L"" + i/rowSize)) {<th>@constraints[""L"" + i/rowSize]</th>}
                    else {<th>X</th>}
                    @for (int j = 0; j < rowSize; j++)
                    {
                     <td>@solution[keys[i+j]]</td>
                    }
                    @if(constraints.ContainsKey(""P"" + i/rowSize)) {<th>@constraints[""P"" + i/rowSize]</th>}
                    else {<th>X</th>}
                   </tr>
                  }
                  <tr>
                   <th></th>
                  @for(int i = 0; i < rowSize; i++)
                  {
                   @if(constraints.ContainsKey(""D"" + i)) {<th>@constraints[""D"" + i]</th>}
                   else {<th>X</th>}
                  }
                   <th></th>
                  </tr>
                 }
                </table>
               }
              }
             </body>
            </html>";
            string title = filePath.Split('/')[filePath.Split('/').Length - 1].Split('.')[0];
            string result = Engine.Razor.RunCompile(template, "skyscraperTemplateKey", null, new { Title = title, Solutions = _solution.Solutions, Constraints = constraints });
            File.WriteAllText(filePath, result);
        }

    }
}
