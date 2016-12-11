using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BuroComun.src.model.cargas
{
    [DataContract]
    [Serializable]
    public class OperacionCarga
    {
        [Required]
        [DataMember]
        [RegularExpression(@"^[0-9A-Za-z\u00f1\u00d1\,\/\.\-\ ]{0,100}$")]
        public string idEmisor { get; set; }


        [DataMember]
        [Required]
        [RegularExpression(@"^[0-9A-Za-z\u00f1\u00d1\,\/\.\-\ ]{0,100}$")]
        public string idProveedor { get; set; }

        [DataMember]
        [Required]
        [RegularExpression(@"^[0-9A-Za-z\u00f1\u00d1\,\/\.\-\ ]{0,100}$")]
        public string Folio { get; set; }

        [DataMember]
        [Required]
        public DateTime FechaEmision { get; set; }

        [DataMember]
        [Required]
        public Decimal MontoNominal { get; set; }

        [DataMember]
        [Required]
        public Decimal Anticipo { get; set; }

        [DataMember]
        [Required]
        [RegularExpression(@"^[0-9A-Za-z\u00f1\u00d1\,\/\.\-\ ]{0,100}$")]
        public String TipoDocumento { get; set; }

        [DataMember]
        [Required]
        [RegularExpression(@"^[0-9A-Za-z\u00f1\u00d1\,\/\.\-\ ]{0,100}$")]
        public String Divisa { get; set; }

        public decimal MontoFinanciado
        {
            get
            {
                return MontoNominal * Anticipo / 100;
            }

        }

        [DataMember]
        [Required]
        public DateTime fechaPago { get; set; }
    }
}
