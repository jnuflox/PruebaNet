//PROY-140546
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity.ClientePagoAnticipadoFijaRest.DataPower
{
    public class ConsultaPAHeaderRequest
    {
        
        public string consumer { get; set; }
        
        public string country { get; set; }
        
        public string dispositivo { get; set; }
        
        public string language { get; set; }
        
        public string modulo { get; set; }
        
        public string msgType { get; set; }
        
        public string operation { get; set; }
        
        public string pid { get; set; }
        
        public string system { get; set; }
        
        public string timestamp { get; set; }
        
        public string userId { get; set; }
        
        public string wsIp { get; set; }
        
    }
}
