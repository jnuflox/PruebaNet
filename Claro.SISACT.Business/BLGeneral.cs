using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.Entity;
using Claro.SISACT.Data;
using System.Collections;
using System.Configuration;
using System.Data;

namespace Claro.SISACT.Business
{
    public class BLGeneral
    {
        public List<BETipoDocumento> ListarTipoDocumento()
        {
            return new DAGeneral().ListarTipoDocumento();
        }
        
        public List<BEItemGenerico> ListarProducto()
        {
            return new DAGeneral().ListarProducto();
        }
        
        public List<BEBilletera> ListarBilletera()
        {
            return new DAGeneral().ListarBilletera();
        }

        public List<BEItemGenerico> ListarTipoOferta(string tipoDocumento)
        {
            return new DAGeneral().ListarTipoOferta(tipoDocumento);
        }

        public List<BEItemGenerico> ListarTipoOperacion()
        {
            return new DAGeneral().ListarTipoOperacion();
        }

        public List<BEItemGenerico> ListarPlazoAcuerdo(string strCasoEspecial)
        {
            return new DAGeneral().ListarPlazoAcuerdo(strCasoEspecial);
        }

        public List<BEItemGenerico> ListarTipoGarantia(string strTipoGarantia, string strEstado)
        {
            return new DAGeneral().ListarTipoGarantia(strTipoGarantia, strEstado);
        }

        public List<BETipoGarantia> ListaTipoGarantia(string tipoGarantia, string estado)
        {
            return new DAGeneral().ListaTipoGarantia(tipoGarantia, estado);
        }

        public List<BEItemGenerico> ListarParametroGeneral(string strCodigo)
        {
            return new DAGeneral().ListarParametroGeneral(strCodigo);
        }
        
        public List<BEParametro> ListaParametros(Int64 intCodigo)
        {
            return new DAGeneral().ListaParametros(intCodigo);
        }
        
        public string ListaPrefijosApellidoCompuesto()
        {
            return new DAGeneral().ListaPrefijosApellidoCompuesto();
        }

        public BEUsuario ConsultaDatosUsuario(string strCtaRed)
        {
            return new DAGeneral().ConsultaDatosUsuario(strCtaRed);
        }
        
        public List<BEPuntoVenta> ConsultaPDVUsuario(Int64 intCodUsuario, string strCodProducto)
        {
            return new DAGeneral().ConsultaPDVUsuario(intCodUsuario, strCodProducto);
        }
        
        public List<BEItemGenerico> ConsultaTipoOficinaUsuario(Int64 intCodUsuario, string strCodProducto)
        {
            return new DAGeneral().ConsultaTipoOficinaUsuario(intCodUsuario, strCodProducto);
        }

        public List<BEItemGenerico> ConsultaLPrecioxPlazo(string strPlazo)
        {
            return new DAGeneral().ConsultaLPrecioxPlazo(strPlazo);
        }

        public DataTable ListarMotivoDesactivaLinea()
        {
            return new DAGeneral().ListarMotivoDesactivaLinea();
        }

        public List<BEItemGenerico> ListarComodato()
        {
            return new DAGeneral().ListarComodato();
        }

        public void ConsultarDatosDireccion(string idDepartamento, string idProvincia, string idDistrito,ref string strDepartamento, ref string strProvincia, ref string strDistrito)
        {
            new DAGeneral().ConsultarDatosDireccion(idDepartamento, idProvincia, idDistrito, ref strDepartamento, ref strProvincia, ref strDistrito);
        }

        public List<BEItemGenerico> ListarTopesConsumoHFC(string prod) //PROY-29296
        {
            return new DAGeneral().ListarTopesConsumoHFC(prod); //PROY-29296
        }

        public List<BEPlan> ListarPlanBaseCombo(string strPlanBase)
        {
            return new DAGeneral().ListarPlanBaseCombo(strPlanBase);
        }
        
        public List<BEItemGenerico> ListarPlanBase()
        {
            return new DAGeneral().ListarPlanBase();
        }
        
        public List<BEItemGenerico> ListarPlanCombo()
        {
            return new DAGeneral().ListarPlanCombo();
        }

        public List<BECuota> ListarTipoCuota()
        {
            return new DAGeneral().ListarTipoCuota();
        }

        public List<BEItemGenerico> ListarTipoItem(string strTipoItem)
        {
            return new DAGeneral().ListarTipoItem(strTipoItem);
        }

        public List<BEItemGenerico> ListarListaPrecioxCuota(string strCuota)
        {
            return new DAGeneral().ListarListaPrecioxCuota(strCuota);
        }

        //Inicio -Evalenzs - ListaParametrosGrupo -Negocio
        public List<BEParametro> ListaParametrosGrupo(Int64 strCodigo)
        {
            return new DAGeneral().ListaParametrosGrupo(strCodigo);
        }
        //Fin - EValenzs
	public void ConsultarPrecioListaPrepago(string strCodMaterial, string strCodListaPrecio, ref double dblPrecioPrepago) //PROY-24724-IDEA-28174 - INICIO
        {
            new DAGeneral().ConsultarPrecioListaPrepago(strCodMaterial, strCodListaPrecio, ref dblPrecioPrepago);
        } //PROY-24724-IDEA-28174 - FIN

        //INICIO|PROY-140533 - CONSULTA STOCK
        public static BEItemGenerico ConsultarFlagsPicking(string codigo_oficina, ref string codigo_rpta, ref string mensaje_rpta)
        {
            return DAGeneral.ConsultarFlagsPicking(codigo_oficina, ref codigo_oficina, ref mensaje_rpta);
        }
        //FIN|PROY-140533 - CONSULTA STOCK

        //INICIO - IDEA-141897
        public static void consultaBeneficioFullClaro(String strTipoDocumento, String strNumeroDocumento, String strFlagCondicion, out List<BEFullClaroBeneficio> lstFullClaro, out List<BEAcuerdoDetalle> lstPedido, ref string codigo_rpta, ref string mensaje_rpta)
        {
            DAGeneral.consultaBeneficioFullClaro(strTipoDocumento, strNumeroDocumento, strFlagCondicion, out lstFullClaro, out lstPedido, ref codigo_rpta, ref mensaje_rpta);
        }
        //FIN - IDEA-141897

        //IDEA-142010 INICIO
        public int ValidarVigenciaCampana(string strCodigosCampanas, ref string strCodigoRespuesta, ref string strMensajeRespuesta)
        {
            return new DAGeneral().ValidarVigenciaCampana(strCodigosCampanas, ref strCodigoRespuesta, ref strMensajeRespuesta);
        }
        //IDEA-142010 FIN

        //INI IDEA-142717
        public void ValidarCampVacunaton(string strTipoDocumento, string strNroDocumento, ref string strCodigoRespuesta, ref string strMensajeRespuesta)        
        {
            DAGeneral objConsulta = new DAGeneral();
            objConsulta.ValidarCampVacunaton(strTipoDocumento, strNroDocumento, ref strCodigoRespuesta, ref strMensajeRespuesta);      
        }
        //FIN IDEA-142717

        //PROY-140736 INI
        public List<BEItemGenerico> ListarComboBuyback()
        {
            return new DAGeneral().ListarComboBuyback();
        }

        public List<BEItemGenerico> ListarBuyback(long nrosec)
        {
            return new DAGeneral().ListarBuyback(nrosec);
        }

        public void EliminarBuyback(string sopln, ref int codigo, ref string mnsjrpta)
        {
             new DAGeneral().EliminarBuyback(sopln, ref codigo, ref mnsjrpta);
        }

        public void ValidarBuyback(string strIMEI, string strcupon, ref int sec, ref int codigo, ref string mensajebuyback){
           new DAGeneral().ValidarBuyback(strIMEI, strcupon,ref sec,ref codigo, ref mensajebuyback);
        }

        public void EliminarBuybackEvalAnt(string strsec, string strusuario,string strtevac,string nrodoc, ref int codigo, ref string mensajebuyback)
        {
            new DAGeneral().EliminarBuybackEvalAnt(strsec, strusuario, strtevac, nrodoc, ref codigo, ref  mensajebuyback);
        }
        
        //PROY-140736 FIN

        #region [PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil] | [Consulta para obtener las promociones vigentes]
        public static DataSet ConsultarPromocionesVigentesxCampania(string strCodigoCampania) //PROY-23111-IDEA-29841 - INICIO
        {
            return DAGeneral.ConsultarPromocionesVigentesxCampania(strCodigoCampania);
        }

        public static List<BESolicitud> ConsultarSolicitudesCliente(string strNroDocCliente, int intMaxDiasBuscaSEC)
        {
            return new DAGeneral().ConsultarSolicitudesCliente(strNroDocCliente, intMaxDiasBuscaSEC);
        } 

        public static List<BESolicitud> ConsultarSolicitudesPrepago(string strNroDocCliente, int intMaxDiasBuscaSEC)
        {
            return new DAGeneral().ConsultarSolicitudesPrepago(strNroDocCliente, intMaxDiasBuscaSEC);
        }
        #endregion
    }
}
