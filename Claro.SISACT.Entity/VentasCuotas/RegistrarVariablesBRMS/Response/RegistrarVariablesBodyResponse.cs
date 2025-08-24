using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.VentasCuotas.RegistrarVariablesBRMS.Response
{
    [DataContract]
    [Serializable]
    public class RegistrarVariablesBodyResponse
    {

        [DataMember(Name = "variablesBRMSResponse")]
        public RegistrarVariablesTypeResponse variablesBRMSResponse { get; set; }

        public RegistrarVariablesBodyResponse()
        {
            variablesBRMSResponse = new RegistrarVariablesTypeResponse();
        }
    }

}
