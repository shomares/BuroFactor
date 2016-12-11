using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuroFactor.code.bussines.interfaces
{
    public interface ITokenOperaciones
    {
        bool validaToken(string user, string token);
    }
}
