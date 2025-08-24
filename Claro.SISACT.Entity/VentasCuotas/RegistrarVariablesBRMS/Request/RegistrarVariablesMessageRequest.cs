using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Claro.SISACT.Entity.DataPowerRest.Generic;

namespace Claro.SISACT.Entity.VentasCuotas.RegistrarVariablesBRMS.Request
{
    [DataContract]
    [Serializable]
    public class RegistrarVariablesMessageRequest 
    {

        [DataMember(Name = "Header")]
        public DataPowerRest.Generic.HeadersRequest Header { get; set; }

        [DataMember(Name = "Body")]
        public RegistrarVariablesBodyRequest Body { get; set; }

        public RegistrarVariablesMessageRequest()
        {
            Header = new DataPowerRest.Generic.HeadersRequest();
            Body = new RegistrarVariablesBodyRequest();
        }

    }
}
