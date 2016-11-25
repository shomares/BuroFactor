using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuroComun.src.security
{
    public interface IEncripta
    {
        string Decrypt(String value);
        string Encrypt(String value);
    }
}
