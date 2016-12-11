using System.Collections.Generic;
using BuroFactorWS.model;
using BuroFactorWS.src.model;

namespace BuroFactorWS.src.bussines.impl
{
    public interface INotificaRiesgo
    {
        void notificaRiesgos(List<deuda> deudaRiesgo);
    }
}