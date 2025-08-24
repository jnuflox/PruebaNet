using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.claro_vent_pedidostienda.Response
{
    [DataContract]
    [Serializable]
   public class BEDataResponse
    {
        [DataMember(Name = "listaPedidosExcepPre")]
        public List<BEDatosPTDetalle> listaPedidosExcepPre { get; set; }

        public BEDataResponse()
        {
            this.listaPedidosExcepPre = new List<BEDatosPTDetalle>();
           
        }


    }




}
