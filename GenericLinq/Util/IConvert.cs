using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericLinq.Util
{
    public interface IConvert<T,W>
    {
        T convert(W obj);
    }
}
