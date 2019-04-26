using System;
using System.Collections.Generic;
using System.Text;

namespace CSP.Input
{
    public class CSPLoader<T>
    {
        private ICSPParser<T> _parser;

        CSPLoader(ICSPParser<T> parser)
        {
            _parser = parser;
        }

        public Problem<T> LoadCSP(string filePath)
        {
            var problem = _parser.ParseCSP(filePath);
            return new Problem<T>(problem.Item1, problem.Item2);
        }
    }
}
