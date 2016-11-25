using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuroComun.src.security
{
    public interface IGeneraContrasena
    {
        string generaSal();
        string generaPassword();
        string generaAleatorio();
        string generaPassword(String salt, String pass);
    }
}
