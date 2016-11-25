using Data.XML;
using Data.XML.Validacion;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericLinq.Excel.Data
{

    public class DescriptorLayout
    {
        public String Path { get; set; }
        public List<Campo> Campos { get; set; }
        public DateTime UltimaActualizacion { get; set; }
    }

    public interface IContainerLayout {
        List<Campo> GetLayout(String layout);
        String RootPath { get; set; }
        XMLHandler<T> GetHandler<T>(String layout, T objeto);
    }


    public class ContainerLayout: IContainerLayout
    {
        public Dictionary<String, DescriptorLayout> LayoutsN {
            set {
                Layouts = new ConcurrentDictionary<string, DescriptorLayout>(value);
            }
        }

        public ConcurrentDictionary<String, DescriptorLayout> Layouts { get; set; }
        public int TimeMax { get; set; }
        public String RootPath { get; set; }

        public ContainerLayout()
        {
            Layouts = new ConcurrentDictionary<string, DescriptorLayout>();
        }



        public List<Campo> GetLayout(String layout)
        {
            List<Campo> salida = null;
            DateTime now = DateTime.Now;
            if (Layouts.ContainsKey(layout))
            {
                var desc = Layouts[layout];
                TimeSpan diff = now - desc.UltimaActualizacion;
                if (desc.Campos == null || diff.Minutes > TimeMax)
                {
                    if (!String.IsNullOrEmpty(this.RootPath))
                        desc.Campos = LeeCampos.leerEstructuraCamposXML(this.RootPath + desc.Path);
                    else
                        desc.Campos = LeeCampos.leerEstructuraCamposXML(desc.Path);

                    desc.UltimaActualizacion = now;
                }
                return desc.Campos;
            }
            return salida;
        }

        public XMLHandler<T> GetHandler<T>(String layout, T objeto) {
            XMLHandler<T>  salida = null;
            DateTime now = DateTime.Now;
            if (Layouts.ContainsKey(layout))
            {
                var desc = Layouts[layout];
                salida = LeeCampos.leerConfiguracion(this.RootPath + desc.Path, objeto);
            }
            return salida;
        }
    }
}
