using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.Entity;
using Claro.SISACT.Data;
using System.Data;
using System.Configuration;

namespace Claro.SISACT.Business
{
    public class BLReglaNegocio
    {
        #region [Declaracion de Constantes - Config]
        static string constTipoProductoMovil = ConfigurationManager.AppSettings["constTipoProductoMovil"];
        static string constTipoProductoBAM = ConfigurationManager.AppSettings["constTipoProductoBAM"];
        static string constTipoProductoClaroTv = ConfigurationManager.AppSettings["constTipoProductoDTH"];
        static string constTipoProductoHFC = ConfigurationManager.AppSettings["constTipoProductoHFC"];
        static string constTipoProductoFijo = ConfigurationManager.AppSettings["constTipoProductoFijo"];
        static string constTipoOperMigracion = ConfigurationManager.AppSettings["constTipoOperacionMIG"];
        static string constCodTipoDocumentoRUC = ConfigurationManager.AppSettings["constCodTipoDocumentoRUC"];
        static string constTipoDocumentoRUC10 = ConfigurationManager.AppSettings["constTipoDocumentoRUC10"];
        static string constTipoDocumentoRUC20 = ConfigurationManager.AppSettings["constTipoDocumentoRUC20"];
        static string constTipoProducto3PlayInalam = ConfigurationManager.AppSettings["constTipoProducto3PlayInalam"];
        #endregion [Declaracion de Constantes - Config]

        public static string LlenarTipoProductoxOferta(string strTipoOferta, string strTipoFlujo, string strTipoOperacion, string strSoloFijo,
                                                        string strTipoDocumento, string strCasoEspecial, string strModalidadVenta)
        {
            bool flgAdd = false;

            List<BEItemGenerico> objLista = new BLGeneral_II().ListarTipoProductoxOferta(strTipoOferta, strTipoFlujo, strCasoEspecial, strTipoDocumento, strModalidadVenta);
            string strResultado = string.Empty;

            foreach (BEItemGenerico obj in objLista)
            {
                flgAdd = true;

                if (strSoloFijo == "T")
                    flgAdd = (obj.Codigo == constTipoProductoFijo || obj.Codigo == constTipoProductoHFC || obj.Codigo == constTipoProducto3PlayInalam);

                if (strSoloFijo == "P")
                    flgAdd = (obj.Codigo == constTipoProductoFijo || obj.Codigo == constTipoProductoHFC || obj.Codigo == constTipoProductoClaroTv || obj.Codigo == constTipoProducto3PlayInalam);

                if (strTipoOperacion == constTipoOperMigracion)
                {
                   flgAdd = (obj.Codigo == constTipoProductoMovil || obj.Codigo == constTipoProductoBAM || obj.Codigo == constTipoProductoHFC || obj.Codigo == constTipoProducto3PlayInalam);
                    }

                //PROY-29121-INI
                if (strSoloFijo == "T" && strTipoOperacion == constTipoOperMigracion && (obj.Codigo == constTipoProductoMovil || obj.Codigo == constTipoProductoBAM))
                {
                    flgAdd = false;
                }
                //PROY-29121-INI

                if (flgAdd)
                    strResultado += "|" + string.Format("{0};{1}", obj.Codigo, obj.Descripcion);
            }

            return strResultado;
        }

        public static string LlenarCasoEspecial(string strTipoOferta, string strTipoFlujo, string strTipoOperacion, string strOficina)
        {
            List<BECasoEspecial> objLista = new BLGeneral_II().ListarCasoEspecial(strTipoOferta, strTipoFlujo, strTipoOperacion, strOficina);
            string strResultado = string.Empty;

            foreach (BECasoEspecial obj in objLista)
            {
                strResultado += "|" + string.Format("{0};{1}", obj.TCESC_DESCRIPCION2, obj.TCESC_DESCRIPCION);
            }

            return strResultado;
        }

        public static string ListarCombo(string strOficina, string strTipoOferta, string strTipoOperacion, string strTipoFlujo, string strTipoDocumento, string strModalidad)
        {
            List<BEItemGenerico> objLista = new BLGeneral_II().ListarCombo(strOficina, strTipoOferta, strTipoOperacion, strTipoFlujo, strTipoDocumento, strModalidad);
            string strResultado = string.Empty;

            foreach (BEItemGenerico obj in objLista)
            {
                strResultado += "|" + string.Format("{0};{1}", obj.Codigo, obj.Descripcion);
            }

            return strResultado;
        }

        public static BECasoEspecial CambiarCasoEspecial(string strTipoOperacion, string strTipoOferta, string strTipoFlujo, string strSoloFijo, string strTipoDocumento, 
                                                         string strNroDocumento, string strCasoEspecial, string strWhitelist, string strModalidadVenta)
        {
            double dblCFMaximo = 0;
            string blnWhitelistOK = string.Empty;
            string listaCEPlanBscs = string.Empty;
            string listaCEPlan = string.Empty;
            string listaCEPlanxProd = string.Empty;
            string strTipoProducto = string.Empty;

            BECasoEspecial objCasoEspecial = new BECasoEspecial();

            if (!string.IsNullOrEmpty(strCasoEspecial))
            {
                BLEvaluacion objConsulta = new BLEvaluacion();
                if (strWhitelist == "S")
                {
                    blnWhitelistOK = objConsulta.ConsultarBlackListCE(strCasoEspecial, strTipoDocumento, strNroDocumento, ref dblCFMaximo);
                }
                objConsulta.ConsultarCEReglas(strCasoEspecial, ref listaCEPlanBscs, ref listaCEPlan, ref listaCEPlanxProd);
            }

            // Tipo Productos x Oferta Configurados
            if (strWhitelist == "S" & !(blnWhitelistOK == "S"))
                strTipoProducto = LlenarTipoProductoxOferta(strTipoOferta, strTipoFlujo, strTipoOperacion, strSoloFijo, strTipoDocumento, string.Empty, strModalidadVenta);
            else
                strTipoProducto = LlenarTipoProductoxOferta(strTipoOferta, strTipoFlujo, strTipoOperacion, strSoloFijo, strTipoDocumento, strCasoEspecial, strModalidadVenta);

            objCasoEspecial.CARGO_FIJO_MAX = dblCFMaximo;
            objCasoEspecial.FLAG_WHITELIST = blnWhitelistOK;
            objCasoEspecial.PLANES_BSCS = listaCEPlanBscs;
            objCasoEspecial.PLANES_SISACT = listaCEPlan;
            objCasoEspecial.PLANES_X_PRODUCTO = listaCEPlanxProd;
            objCasoEspecial.LISTA_PRODUCTOS = strTipoProducto;

            //PROY-32129 :: INI
            List<BEParametro> lstParamCampUniv = (new BLGeneral()).ListaParametrosGrupo(Int64.Parse(ConfigurationManager.AppSettings["consGrupoCasoEspecial"].ToString()));
            objCasoEspecial.COD_CASO_ESPECIAL_UNIV = lstParamCampUniv.Where(p => p.Valor1 == "1").SingleOrDefault().Valor;
            objCasoEspecial.PREG_CASO_ESPECIAL_UNIV = lstParamCampUniv.Where(p => p.Valor1 == "3").SingleOrDefault().Valor;
            //PROY-32129 :: FIN
              //PROY-140245
            List<BEParametro> lstParamCampColab = (new BLGeneral()).ListaParametrosGrupo(Int64.Parse(ConfigurationManager.AppSettings["consGrupoCasoEspecialColab"].ToString()));
            objCasoEspecial.COD_CASO_ESPECIAL_COLAB = lstParamCampColab.Where(p => p.Valor1 == "1").SingleOrDefault().Valor;
            objCasoEspecial.MSJ_CASO_ESPECIAL_COLAB_NO_ENCONTRADO = lstParamCampColab.Where(p => p.Valor1 == "3").SingleOrDefault().Valor;
            objCasoEspecial.MSJ_CASO_ESPECIAL_COLAB_AUTOGESTION = lstParamCampColab.Where(p => p.Valor1 == "4").SingleOrDefault().Valor;
            objCasoEspecial.MSJ_CASO_ESPECIAL_COLAB_VALID_CANT_PROD = lstParamCampColab.Where(p => p.Valor1 == "5").SingleOrDefault().Valor;
            objCasoEspecial.CANT_MAX_POR_PROD_CASO_ESP_COLAB = lstParamCampColab.Where(p => p.Valor1 == "7").SingleOrDefault().Valor;
            //FIN PROY-140245
            return objCasoEspecial;
        }

        //PROY-32439 MAS INI Validación Insertar en BL
        public bool InsertarDatosBRMSCliente(BEDatosClienteBrms objDatosClienteBrms)
        {
            return new DAEvaluacion().InsertarDatosBRMSCliente(objDatosClienteBrms);
        }
        //PROY-32439 MAS FIN Validación Insertar en BL

        //PROY-32439 - CAMBIOS_LOG INI
        public bool InsertarLogNuevoBRMS(ValidacionDeudaBRMSrequest objDatosRequestNuevoBRMS, string strTipoBloqueoBRMS, string strTipoLineaBloqueoBRMS, string strTipoSusBRMS, string strTipoLineaSusBRMS, string strTipoFraudeBRMS, ValidacionDeudaBRMSresponse objResponse, Int64 strFlagWhilist, Int64 strFlagTieneDeuda, string strCodUsuario, Int64 intFlagErrorBRMS, string strMensajeErrorBRMS, string strResProComercial, string strResProducto, string strResTipoOperacion, string strMensaje) //PROY-140743
        {
            return new DAEvaluacion().InsertarLogNuevoBRMS(objDatosRequestNuevoBRMS, strTipoBloqueoBRMS, strTipoLineaBloqueoBRMS, strTipoSusBRMS, strTipoLineaSusBRMS, strTipoFraudeBRMS, objResponse, strFlagWhilist, strFlagTieneDeuda, strCodUsuario, intFlagErrorBRMS, strMensajeErrorBRMS, strResProComercial, strResProducto, strResTipoOperacion, strMensaje); //PROY-140743
        }
        //PROY-32439 - CAMBIOS_LOG FIN
    }
}
