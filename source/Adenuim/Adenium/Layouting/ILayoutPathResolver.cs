using System;
using System.Collections.Generic;

namespace Adenium.Layouting
{
    public interface ILayoutPathResolver
    {
        List<string> Resolve(Type viewModelType);
    }
}
