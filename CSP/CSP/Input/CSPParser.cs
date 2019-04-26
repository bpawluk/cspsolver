using CSP.Constraints;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSP.Input
{
    public interface ICSPParser<T>
    {
        (Variable<T>[], IConstraint<T>[]) ParseCSP(string filePath);
    }
}
