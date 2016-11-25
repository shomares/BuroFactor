using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuroComun.src.model.cargas;

namespace BuroComun.src.security
{
    public interface ISerialize
    {
        string Serialize(object v);
        object Derialize(string data);
    }
}
