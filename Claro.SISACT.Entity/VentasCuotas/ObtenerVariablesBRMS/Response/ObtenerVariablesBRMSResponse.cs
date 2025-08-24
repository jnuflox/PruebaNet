using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.VentasCuotas.ObtenerVariablesBRMS.Response
{
    [DataContract]
    [Serializable]
    public class ObtenerVariablesBRMSResponse
    {
         [DataMember(Name = "MessageResponse")]
        public ObtenerVariablesBRMSMessageResponse MessageResponse { get; set; }

         public ObtenerVariablesBRMSResponse()
        {
            MessageResponse = new ObtenerVariablesBRMSMessageResponse();
        }

    }
}
