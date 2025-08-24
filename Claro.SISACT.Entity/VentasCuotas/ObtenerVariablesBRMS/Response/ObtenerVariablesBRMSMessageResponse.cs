using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.VentasCuotas.ObtenerVariablesBRMS.Response
{
    [DataContract]
    [Serializable]
    public class ObtenerVariablesBRMSMessageResponse
    {
        [DataMember(Name = "Header")]
        public DataPowerRest.HeadersResponse Header { get; set; }

        [DataMember(Name = "Body")]
        public ObtenerVariablesBRMSBodyResponse Body { get; set; }

        public ObtenerVariablesBRMSMessageResponse()
        {
            Header = new DataPowerRest.HeadersResponse();
            Body = new ObtenerVariablesBRMSBodyResponse();
        }

    }
}
