using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.RegistrarOmisionPINRest
{
    [DataContract]
    [Serializable]
   public class MessageRequestRegistrarOmisionPIN
    {
       [DataMember (Name = "Header")]
       public DataPowerRest.HeadersRequest header { get; set; }

       [DataMember (Name = "Body")]
       public BodyRequestRegistrarOmisionPIN body { get; set; }

       public MessageRequestRegistrarOmisionPIN()
       {
           header = new DataPowerRest.HeadersRequest();
           body = new BodyRequestRegistrarOmisionPIN();
       }

    }
}
