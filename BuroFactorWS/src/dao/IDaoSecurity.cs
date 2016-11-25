using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuroFactorWS.model;

namespace BuroFactorWS.src.dao
{
    public interface IDaoSecurity
    {
        bool validaUsuarioWs(string userName, string password);
        usuario getUser(string userName);
        plancontratado getPlan(string userName);
    }
}
