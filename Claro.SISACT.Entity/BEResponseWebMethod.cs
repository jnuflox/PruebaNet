using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEResponseWebMethod
    {
        public string IdFila { get; set; }
        public string Cadena  { get; set; }
        public List<String> ListaCadena { get; set; }
        public List<BEItemGenerico> Lista { get; set; }
        public object Objeto { get; set; }
        public bool Boleano { get; set; }
        public string TipoRespuesta { get; set; } // C=Cadena,L=List,O=Object,B=Boolean
        public bool EstadoSession { get; set; }
        public bool Error { get; set; }
        public string CodigoError { get; set; }
        public string DescripcionError { get; set; }
        public string Mensaje { get; set; }
        public string Tipo { get; set; }
        //PROY-29121-INI
        public string hidCadenaSituacionRRLL { get; set; }
        public string hidMensajeRRLL { get; set; }
        //PROY-29121-FIN
        //PROY-FULLCLARO.V2-INI
        public string CodigoFC { get; set; }
        public string MensajeFC { get; set; }
        public string CodigoBotonFC { get; set; }
        public string MensajeBotonFC { get; set; }
        public List<BEDatosClienteFC> objDatosClienteFC { get; set; }
        //PROY-FULLCLARO.V2-FIN

        //INICIO PROY-140560 - Beneficio Full Claro antes grabar popup
        public string ObligatoriedadFC { get; set; }
        //FIN PROY-140560 - Beneficio Full Claro antes grabar popup

        //PROY-140457-DEBITO AUTOMATICO-INI
        public bool Obligatorio { get; set; }
        public double MontoMaximo { get; set; }
        //PROY-140457-DEBITO AUTOMATICO-FIN
        //PROY-140585 F2 INI
        public string Canal { get; set; }
        public string Oferta { get; set; }
        public string codTipoProd { get; set; }
        public string TipoDoc { get; set; }
        public string NumeroDoc { get; set; }
        //PROY-140585 F2 FIN

        public string EstadoBonoBSCSFC { get; set; }//INC000003048070 
	
	//IDEA-142010 INI
        public string BeneficioEstado { get; set; }
        public string BeneficioAdicionalMsg { get; set; }
        //IDEA-142010 FIN

        //PROY-140715- INI ANGEL
        public string mensaje_contingencia { get; set; }
        //PROY-140715- INI ANGEL

        public BEResponseWebMethod() 
        {
            this.EstadoSession = false;
            this.Error = false;
        }

        public string LineasSinCP { get; set; } // PROY-140335 RF1

        public string RestriccionCampanasFullClaro { get; set; } //INICIATIVA-1012

        #region [PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil] | [Nueva Propiedad]
        public string isClienteClaro { get; set; }
        public string flagPermitirVV { get; set; }
        public string MensajeErrorBRMS { get; set; }
        public string deudaBloqueo { get; set; }
        #endregion
    }
}

