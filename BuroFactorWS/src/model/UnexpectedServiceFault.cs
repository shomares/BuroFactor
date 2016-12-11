using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BuroFactorWS.src.model
{
    [DataContract]
    public class UnexpectedServiceFault
    {
        [DataMember]
        public string ErrorMessage { get; set; }
        [DataMember]
        public string StackTrace { get; set; }
        [DataMember]
        public string Target { get; set; }
        [DataMember]
        public string Source { get; set; }

        [DataMember]
        public UnexpectedServiceFault[] InnerException { get; set; }



    }
}