using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.VentasCuotas.RegistrarVariablesBRMS.Response
{
    [DataContract]
    [Serializable]
    public class RegistrarVariablesMessageResponse
    {
         [DataMember(Name = "Header")]
        public DataPowerRest.HeadersResponse Header { get; set; }

        [DataMember(Name = "Body")]
        public RegistrarVariablesBodyResponse Body { get; set; }

        public RegistrarVariablesMessageResponse()
        {
            Header = new DataPowerRest.HeadersResponse();
            Body = new RegistrarVariablesBodyResponse();
        }

    }
}
