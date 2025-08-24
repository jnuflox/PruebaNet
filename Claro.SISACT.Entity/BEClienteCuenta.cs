using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using Claro.SISACT.Entity.claro_vent_ventascontingencia;
using Claro.SISACT.Entity.VentasCuotas.ObtenerDatosPedidoAccCuotas.Response;//PROY-140743

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEClienteCuenta
    {
        public string idCliente { get; set; }
        public string nroDoc { get; set; }
        public string tipoDoc { get; set; }
        public string tipoDocDes { get; set; }
        public string nroDocAsociado { get; set; }
        public string nombres { get; set; }
        public string apellidos { get; set; }
        public string apellidoPaterno { get; set; }
        public string apellidoMaterno { get; set; }
        public string razonSocial { get; set; }
        public string nacionalidad { get; set; } //PROY-31636
        public double CF_Total { get; set; }
        public double CF_Menor { get; set; }
        public double CF_Mayor { get; set; }
        public double deudaVencida { get; set; }
        public double deudaCastigada { get; set; }
        public double deudaFraude { get; set; }
        public double deudaTotal { get; set; }
        public int nroDiasDeuda { get; set; }
        public int nroBloqueo { get; set; }
        public int nroSuspension { get; set; }

        public string soloEvaluarFijo { get; set; }
        public bool esClienteClaro { get; set; }
        public bool isBlackList { get; set; }
        public bool isWhiteList { get; set; }
        public bool isTop { get; set; }
        public bool soloBloqueoRoboPerdida { get; set; }

        public string mensajeDeudaBloqueo { get; set; }

        public DataTable lineaSGA { get; set; }
        public DataTable lineaBSCS { get; set; }
        public DataTable lineaSISACT { get; set; }
        public DataTable lineaPrepago { get; set; }
        public ArrayList lineaOAC { get; set; }

        public int nroRangoDiasBSCS { get; set; }
        public int nroPlanesActivos { get; set; }
        public int nroLineasBSCS { get; set; }
        public int nroLineasSGA { get; set; }
        public int nroLineasSISACT { get; set; }
        public int nroLineaMenor7Dia { get; set; }
        public int nroLineaMenor30Dia { get; set; }
        public int nroLineaMenor90Dia { get; set; }
        public int nroLineaMenor180Dia { get; set; }
        public int nroLineaMayor90Dia { get; set; }
        public int nroLineaMayor180Dia { get; set; }

        //Atributos Cliente BRMS
        public string tipoCliente { get; set; }
        public int tiempoPermanencia { get; set; }
        public int comportamientoPago { get; set; }
        public double montoFacturadoTotal { get; set; }
        public double montoNoFacturadoTotal { get; set; }

        public string oficina { get; set; }
        public string nroOperacionBuro { get; set; }

        public List<BEPlanBilletera> oPlanesActivosxBilletera { get; set; }
        public List<BEPlanBilletera> oPlanesActivosCorporativo { get; set; }
        public List<BEBilletera> oLCBuroxBilletera { get; set; }
        public List<BEBilletera> oMontoFacturadoxBilletera { get; set; }
        public List<BEBilletera> oMontoNoFacturadoxBilletera { get; set; }
        public List<BEBilletera> oLCDisponiblexBilletera { get; set; }
        public ArrayList oGarantiaxProducto { get; set; }

        public ArrayList oOAC { get; set; }

        public BEVistaEvaluacion oVistaEvaluacion { get; set; }
        //gaa20170215
        public string buroConsultado { get; set; }
        //fin gaa20170215

        public string deudaCliente { get; set; } //PROY-29121
        public string cumpleReglaA { get; set; } //PROY-29121

//PROY-30748 ini
        public int totalplanes { get; set; }
        public string Deuda { get; set; }
        //PROY-30748 fin
        //PROY-26963-GPRD - PROMFACT
        public List<BEPlanBilletera> ListaMontoFactura { get; set; }
        public List<BEPlanBilletera> ListaPlanesActivos { get; set; }
        //PROY-26963-GPRD  - PROMFACT
        //PROY-32439 INI MAS
        public Boolean errorBrms { get; set; }
        //PROY-32439 FIN MAS
       /*PROY-32438 INI*/
       public List<BERepresentanteLegal> oRepresentanteLegal { get; set; }
 
       public string TipContribuyente { get; set; }
       public string NomComercial { get; set; }
       public string FecIniActividades { get; set; }
       public string EstContribuyente { get; set; }
       public string CondContribuyente { get; set; }
       public string CiiuContribuyente { get; set; }
       public string CantTrabajadores { get; set; }
       public string EmisionComp { get; set; }
       public string SistEmielectronica { get; set; }
       public int CantMesIniActividades { get; set; }
       /*PROY-32438 FIN*/

       public bool clienteCBIO { get; set; } //INICIATIVA-219

       public List<BEVentasContingencia> listaVentasContingencia { get; set; }//PROY-140715

       public List<BEObtenerDatosPedidoAccCuotas> DatosPedidoAccCuotas { get; set; } //PROY-140743
       public DateTime fechaActivacion { get; set; } //PROY-140743
    }
}
