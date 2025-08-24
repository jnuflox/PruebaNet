using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Claro.SISACT.Entity.MotorPromociones.SimulacionMultilineas
{
    [Serializable]
    [DataContract]
    public class ResultResponse
    {
        [DataMember(Name = "contract")]
        public List<ListContractResponse> contract { get; set; }

        public ResultResponse()
        {
            contract = new List<ListContractResponse>();
        }
    }
}
