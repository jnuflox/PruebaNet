using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEFormEvaluacion
    {
        public string metodo { get; set; }
        public int idFila { get; set; }
        public Int64 nroSEC { get; set; }

        public string idOficina { get; set; }
        public string idFlujo { get; set; }
        public string idOferta { get; set; }
        public string idTipoOperacion { get; set; }
        public string idCasoEspecial { get; set; }
        public string idCombo { get; set; }
        public string idTipoVenta { get; set; }
        public string flgPorta { get; set; }

        public string tipoDocumento { get; set; }
        public string nroDocumento { get; set; }

        public string idProducto { get; set; }
        public string idCampana { get; set; }
        public string idCampanaSap { get; set; }
        public string idPlazo { get; set; }
        public string idPaquete { get; set; }
        public int nroSecuencia { get; set; }
        public string idPlan { get; set; }
        public string listaPlan { get; set; }
        public string idPlanSap { get; set; }
        public string idMaterial { get; set; }
        public string idListaPrecio { get; set; }
        public int nroCuotas { get; set; }

        public string fechaSap { get; set; }
        public string canalSap { get; set; }
        public string centroSap { get; set; }
        public string orgVentaSap { get; set; }
        public string tipoDocumentoSap { get; set; }
        public string idTipoOperacionSap { get; set; }

        public string evaluarFijo { get; set; }
        public string modalidadVenta { get; set; }
        public string tipoOficina { get; set; }
        public string idDepartamento { get; set; }
        public string idTipoOficina { get; set; }
        public string idCuota { get; set; }
        //gaa20161020
        public string idFamiliaPlan { get; set; }
        //fin gaa20161020
        public string CumpleReglaAClienteRRLL { get; set; }//PROY-29121
        public string codServicio { get; set; } //PROY 29296
        public string strmaterialbuyback { get; set; } //PROY-140736
        public string strcampaniabuyback { get; set; } //PROY-140736
        public string idPromocion { get; set; }//PROY-140743
        public string codProdLinAsociada { get; set; }//PROY-140743
    
    }
}
