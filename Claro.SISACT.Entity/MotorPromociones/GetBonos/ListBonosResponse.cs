using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Claro.SISACT.Entity.MotorPromociones.GetBonos
{
    [Serializable]
    [DataContract]
    public class ListBonosResponse
    {
        [DataMember(Name = "bonoId")]
        public string bonoId { get; set; }

        [DataMember(Name = "endDate")]
        public string endDate { get; set; }

        [DataMember(Name = "observation")]
        public string observation { get; set; }

        [DataMember(Name = "forceEndDate")]
        public string forceEndDate { get; set; }

        [DataMember(Name = "promotionCode")]
        public string promotionCode { get; set; }

        [DataMember(Name = "deactivateDate")]
        public string deactivateDate { get; set; }

        [DataMember(Name = "poType")]
        public string poType { get; set; }

        [DataMember(Name = "msisdn")]
        public string msisdn { get; set; }

        [DataMember(Name = "event")]
        public string _event { get; set; }

        [DataMember(Name = "category")]
        public string category { get; set; }

        [DataMember(Name = "activationDate")]
        public string activationDate { get; set; }

        [DataMember(Name = "scheduleId")]
        public string scheduleId { get; set; }

        [DataMember(Name = "startDate")]
        public string startDate { get; set; }

        [DataMember(Name = "po")]
        public string po { get; set; }

        [DataMember(Name = "status")]
        public string status { get; set; }

        public ListBonosResponse()
        {
            bonoId = string.Empty;
            endDate = string.Empty;
            observation = string.Empty;
            forceEndDate = string.Empty;
            promotionCode = string.Empty;
            deactivateDate = string.Empty;
            poType = string.Empty;
            msisdn = string.Empty;
            _event = string.Empty;
            category = string.Empty;
            activationDate = string.Empty;
            scheduleId = string.Empty;
            startDate = string.Empty;
            po = string.Empty;
            status = string.Empty;
        }
    }
}
