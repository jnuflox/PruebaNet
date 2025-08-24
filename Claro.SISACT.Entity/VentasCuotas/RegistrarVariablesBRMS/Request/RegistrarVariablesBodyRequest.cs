using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.VentasCuotas.RegistrarVariablesBRMS.Request
{
    [DataContract]
    [Serializable]
    public class RegistrarVariablesBodyRequest
    {
        [DataMember(Name = "registrarVariablesBRMSRequestType")]
        public RegistrarVariablesTypeRequest registrarVariablesBRMSRequestType { get; set; }

        public RegistrarVariablesBodyRequest()
        {
            registrarVariablesBRMSRequestType = new RegistrarVariablesTypeRequest();
        }

    }
}
