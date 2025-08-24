using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.VentasCuotas.RegistrarVariablesBRMS.Response
{
    [DataContract]
    [Serializable]
    public class RegistrarVariablesResponse
    {
        [DataMember(Name = "MessageResponse")]
        public RegistrarVariablesMessageResponse MessageResponse { get; set; }

        public RegistrarVariablesResponse()
        {
            MessageResponse = new RegistrarVariablesMessageResponse();
        }

    }

}
