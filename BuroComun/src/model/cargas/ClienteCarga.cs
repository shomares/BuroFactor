using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BuroComun.src.model.cargas
{
    [Serializable]
    [DataContract]
    public class ClienteCarga
    {
        [Required]
        [DataMember]
        [RegularExpression(@"^[0-9A-Za-z\u00f1\u00d1\,\/\.\-\ ]{0,100}$")]
        public string idInterno { get; set; }

        [Required]
        [DataMember]
        [RegularExpressionAttribute(@"^[a-zA-Z]{3,4}(\d{6})((\D|\d){3})?$")]
        public string rfc { get; set; }

        [DataMember]
        [Required]
        [RegularExpressionAttribute(@"^[0-9A-Za-z\u00f1\u00d1\,\/\.\-\ ]{0,100}$")]
        public string nombre { get; set; }

        [DataMember]
        [Required]
        public DateTime fechaConstitucion { get; set; }

        [DataMember]
        [Required]
        [RegularExpressionAttribute("^[_a-z0-9-]+(.[_a-z0-9-]+)*@[a-z0-9-]+(.[a-z0-9-]+)*(.[a-z]{2,4})$")]
        public string correo { get; set; }
    }
}
