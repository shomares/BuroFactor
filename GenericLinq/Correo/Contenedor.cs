using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GenericLinq.Correo
{
    public static class Extension
    {
        public static string getMensaje(this ContenedorPlantillas plantilla, string nombre, Dictionary<string, string> elementos)
        {
            if (plantilla.Checks.ContainsKey(nombre))
            {
                if (plantilla.Checks[nombre].consulta.AddHours(1).CompareTo(DateTime.Now) >= 0)
                {
                    if (File.Exists(plantilla.Ruta + plantilla.plantillas[nombre]))
                    {
                        plantilla.Checks[nombre].consulta = DateTime.Now;
                        plantilla.Checks[nombre].HTML = File.ReadAllText(plantilla.Ruta + plantilla.plantillas[nombre]);
                    }
                }
            }
            else
            {
                if (File.Exists(plantilla.Ruta + plantilla.plantillas[nombre]))
                    plantilla.Checks.Add(nombre, new HistoricoPlantilla()
                    {
                        consulta = DateTime.Now,
                        HTML = File.ReadAllText(plantilla.Ruta + plantilla.plantillas[nombre])
                    });
            }

            if (string.IsNullOrWhiteSpace(plantilla.Checks[nombre]?.HTML))
                throw new Exception("La plantilla no se encuentra configurada correctamente");

            return getHtml(elementos, plantilla.Checks[nombre]?.HTML);
        }
        public static string getHtml(Dictionary<string, string> elementos, string html)
        {
            StringBuilder sb = new StringBuilder();
            string[] arreglo = html.Split('?');
            foreach (string st in arreglo)
            {
                if (elementos.ContainsKey(st))
                    sb.Append(elementos[st]);
                else
                    sb.Append(st);
            }

            return sb.ToString();
        }
    }
    public class ContenedorPlantillas
    {
        public ContenedorPlantillas()
        {
            Checks = new Dictionary<string, HistoricoPlantilla>();
        }
        public Dictionary<string, string> plantillas { get; set; }
        public string Ruta { get; set; }
        internal Dictionary<string, HistoricoPlantilla> Checks { get; set; }


    }
    public class HistoricoPlantilla
    {
        public DateTime consulta { get; set; }
        public string HTML { get; set; }
    }
}
