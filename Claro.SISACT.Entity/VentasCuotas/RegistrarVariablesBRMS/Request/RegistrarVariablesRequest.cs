using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.VentasCuotas.RegistrarVariablesBRMS.Request
{
    [DataContract]
    [Serializable]
    public class RegistrarVariablesRequest
    {
        [DataMember(Name = "MessageRequest")]
        public RegistrarVariablesMessageRequest MessageRequest { get; set; }

        public RegistrarVariablesRequest()
        {
            MessageRequest = new RegistrarVariablesMessageRequest();
        }
    }
}
