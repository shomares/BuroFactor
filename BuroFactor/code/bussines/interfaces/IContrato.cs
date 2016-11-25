using BuroFactor.Models.dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuroFactor.code.bussines.interfaces
{
    public interface IContrato
    {
        void registraContrato(plancontratado plan);
        void altaContrato(plancontratado plan);
        plancontratado getPlanContratado(int idFinanciera, int idPlan);
        plancontratado getPlanContratado(string hash);
        void creaUsuarioFinanciera(financiera financiera, string contrasena);
    }
}
