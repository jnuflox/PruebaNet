using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Claro.SISACT.Entity.MotorPromociones.GetBonos
{
    [Serializable]
    [DataContract]
    public class BodyResponseGetBonos
    {
        [DataMember(Name = "result")]
        public List<ListResultResponse> result { get; set; }

        [DataMember(Name = "responseAudit")]
        public ResponseAudit responseAudit { get; set; }

        [DataMember(Name = "all")]
        public bool all { get; set; }

        [DataMember(Name = "documentType")]
        public string documentType { get; set; }

        [DataMember(Name = "document")]
        public string document { get; set; }

        [DataMember(Name = "msisdn")]
        public string msisdn { get; set; }

        [DataMember(Name = "category")]
        public string category { get; set; }

        [DataMember(Name = "event")]
        public string _event { get; set; }

        [DataMember(Name = "actived")]
        public bool actived { get; set; }

        [DataMember(Name = "deactivated")]
        public bool deactivated { get; set; }

        [DataMember(Name = "group")]
        public string group { get; set; }

        public BodyResponseGetBonos()
        {
            result = new List<ListResultResponse>();
            responseAudit = new ResponseAudit();
            all = false;
            documentType = string.Empty;
            document = string.Empty;
            msisdn = string.Empty;
            category = string.Empty;
            _event = string.Empty;
            actived = false;
            deactivated = false;
            group = string.Empty;

        }

    }
}
