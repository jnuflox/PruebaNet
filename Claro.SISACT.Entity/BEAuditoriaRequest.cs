using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Collections; //INICIATIVA-219

namespace Claro.SISACT.Entity
{
    //PROY-140245 
    [DataContract]
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEAuditoriaRequest
    {
        [DataMember(Name = "canal")]
        public string canal { get; set; }
        [DataMember(Name = "idAplicacion")]
        public string idAplicacion { get; set; }
        [DataMember(Name = "usuarioAplicacion")]
        public string usuarioAplicacion { get; set; }
        [DataMember(Name = "usuarioSesion")]
        public string usuarioSesion { get; set; }
        [DataMember(Name = "idTransaccionESB")]
        public string idTransaccionESB { get; set; }
        [DataMember(Name = "idTransaccionNegocio")]
        public string idTransaccionNegocio { get; set; }
        [DataMember(Name = "fechaInicio")]
        public string fechaInicio { get; set; }
        [DataMember(Name = "nodoAdicional")]
        public string nodoAdicional { get; set; }
        [DataMember(Name = "timestamp")]
        public string timestamp { get; set; }
        [DataMember(Name = "idTransaccion")]
        public string idTransaccion { get; set; }
        [DataMember(Name = "userId")]
        public string userId { get; set; }
        [DataMember(Name = "nameRegEdit")]
        public string nameRegEdit { get; set; }
        [DataMember(Name = "applicationCodeWS")]
        public string applicationCodeWS { get; set; }
        [DataMember(Name = "applicationCode")]
        public string applicationCode { get; set; }
        [DataMember(Name = "ipApplication")]
        public string ipApplication { get; set; }
        [DataMember(Name = "accept")] //FullClaro-v2
        public string accept { get; set; }
        [DataMember(Name = "msgid")]
        public string msgid { get; set; }

        //INI: INICIATIVA-219
        [DataMember(Name = "wsIp")]
        public string wsIp { get; set; }
        [DataMember(Name = "ipTransaccion")]
        public string ipTransaccion { get; set; }
        [DataMember(Name = "usuarioAplicacionEncriptado")]
        public string usuarioAplicacionEncriptado { get; set; }
        [DataMember(Name = "claveEncriptada")]
        public string claveEncriptada { get; set; }
        [DataMember(Name = "urlRest")]
        public string urlRest { get; set; }
        [DataMember(Name = "urlTimeOut_Rest")]
        public string urlTimeOut_Rest { get; set; }

        public bool dataPower { get; set; }
        public Hashtable table { get; set; }
        //FIN: INICIATIVA-219

        public string strTipoDocumentoPvu { get; set; } //INICIATIVA-710
    }
}
