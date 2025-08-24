using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.claro_int_consultacliente.consultarDatos
{
    [DataContract]
    [Serializable]
    public class BodyResponseTicklers
    {
        [DataMember(Name = "tickNumber")]
        public string tickNumber { get; set; }

        [DataMember(Name = "tickCode")]
        public string tickCode { get; set; }

        [DataMember(Name = "tickStatus")]
        public string tickStatus { get; set; }

        [DataMember(Name = "tickShdes")]
        public string tickShdes { get; set; }

        [DataMember(Name = "tickLdes")]
        public string tickLdes { get; set; }

        [DataMember(Name = "creationDate")]
        public string creationDate { get; set; }
    }
}
