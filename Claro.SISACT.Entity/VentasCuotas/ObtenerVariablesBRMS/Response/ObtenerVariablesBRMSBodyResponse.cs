using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.VentasCuotas.ObtenerVariablesBRMS.Response
{
    [DataContract]
    [Serializable]
    public class ObtenerVariablesBRMSBodyResponse
    {
        [DataMember(Name = "variablesBRMSResponse")]
        public ObtenerVariablesBRMSTypeResponse variablesBRMSResponse { get; set; }

        public ObtenerVariablesBRMSBodyResponse()
        {
            variablesBRMSResponse = new ObtenerVariablesBRMSTypeResponse();
        }

    }
}
