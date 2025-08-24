using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data; //PROY-24724-IDEA-28174
using Claro.SISACT.Entity;
using Claro.SISACT.Data;
using Claro.SISACT.Common;
using System.Configuration; // PROY-140743

namespace Claro.SISACT.Business
{
    public class BLSincronizaSap
    {
        //13-10-2014

        //public static List<BEConsultaDatosOficina> ConsultaParrametrosOficina(string codOficina, string desOficina)
        //{
        //    var oDaSincronizaSap = new DASincronizaSap();
        //    return oDaSincronizaSap.ConsultaDatosOficina(codOficina, desOficina);//.ConsultaParrametrosOficina(codOficina, desOficina);
        //}

        public static List<BEConsultarPrecioBase> ConsultarPrecioBase(string codMaterial, string desMaterial)
        {
            var oDaSincronizaSap = new DASincronizaSap();
            return oDaSincronizaSap.ConsultarPrecioBase(codMaterial, desMaterial);
        }

        public static List<BEConsultarMaterialXCampania> ConsultarMaterialXCampania(string vCodigoOficina, string vDescripcionOficina, string vCodigoCentro, string vCodigoAlmacen, string vTipoOficina)
        {
            var oDaSincronizaSap = new DASincronizaSap();
            return oDaSincronizaSap.ConsultarMaterialXCampania(vCodigoOficina, vDescripcionOficina, vCodigoCentro, vCodigoAlmacen, vTipoOficina);
        }

        public static List<BEConsultaListaPrecios> ConsultarPrecio(BEFormEvaluacion objForm)
        {            
            return new DASincronizaSap().ConsultarPrecio(objForm);
        }

        //PROY-140736 INI
        public static List<BEConsultaListaPrecios> ConsultarPrecioBuyback(BEFormEvaluacion objForm)
        {
            return new DASincronizaSap().ConsultarPreciobBuyback(objForm);
        }
        //PROY-140736 FIN
        public static List<BETipoDocOficina> ConsultaTipoDocumentoOficina(string tipOficina, out int result)
        {
            var oDaSincronizaSap = new DASincronizaSap();
            return oDaSincronizaSap.ConsultaTipoDocumentoOficina(tipOficina, out result);
        }

        public static BEConsultaDatosOficina ConsultaDatosOficina(string vCodigoOficina, string vDescripcionOficina)
        {
            var oDaSincronizaSap = new DASincronizaSap();
            return oDaSincronizaSap.ConsultaDatosOficina(vCodigoOficina, vDescripcionOficina);
        }

        public DataTable ConsultarDatosMaterial(string strCodMaterial) //PROY-24724-IDEA-28174 - INICIO
        {
            return new DASincronizaSap().ConsultarDatosMaterial(strCodMaterial);
        } //PROY-24724-IDEA-28174 - FIN

        //INICIO|PROY-30162-IDEA-32487 ENVIO BOLETA ELECTRONICA PREPAGO //RGP_BOL_18
        public static BEClienteSAP ConsultaCliente(string strTipoDoc, string strNumDoc, out string strCodRpta, out string strMsgRpta)
        {            
            return DASincronizaSap.ConsultaCliente(strTipoDoc, strNumDoc, out strCodRpta, out strMsgRpta);
        }
        //FIN|PROY-30162-IDEA-32487 ENVIO BOLETA ELECTRONICA PREPAGO //RGP_BOL_18
		
		//PROY-30859-IDEA-39316-RU02-by-LCEJ INI
        public List<BEConsultarMaterialXCampania> ConsultaArticulosBam(String StrCanal, String StrTipoOper, String StrTipoVent, String StrProd, String CodOfic, String DesOfic, String CodCentroOfic, String CodAlmacOfic, String strEval)
        {
            return new DASincronizaSap().ConsultaArticulosBam(StrCanal, StrTipoOper, StrTipoVent, StrProd, CodOfic, DesOfic, CodCentroOfic, CodAlmacOfic, strEval);
        }
        //PROY-30859-IDEA-39316-RU02-by-LCEJ FIN

        //INICIO|PROY-140533 - CONSULTA STOCK
        public static String ConsultarStock(String strPDV, String strCodMaterial, out String strCodRpta, out String strMsgRpta)
        {
            return DASincronizaSap.ConsultarStock(strPDV, strCodMaterial, out strCodRpta, out strMsgRpta);
        }
        //FIN|PROY-140533 - CONSULTA STOCK

        #region [PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil]
        public static List<BEConsultaStock> ConsultarStockXTipoMaterial(BEParametrosMSSAP oParamMSSAP)
        {
            return new DASincronizaSap().ConsultarStockXTipoMaterial(oParamMSSAP);
        }

        public static string[] ListCodigosMerchandising()
        {
            string strTiposMaterial = ConfigurationManager.AppSettings["ConsTipoMaterialMerchandising"];
            return strTiposMaterial.Split(',');
        }

        public static string[] ListCodigosServicios()
        {
            string strTiposMaterial = ConfigurationManager.AppSettings["ConsTipoMaterialServicios"];
            return strTiposMaterial.Split(',');
        }

        public static string[] ListCodigosRecarga()
        {
            string strTiposMaterial = ConfigurationManager.AppSettings["ConsTipoMaterialRecarga"];
            return strTiposMaterial.Split(',');
        }

        public static string[] ListCodigosAccesorios()
        {
            string strTiposMaterial = ConfigurationManager.AppSettings["ConsTipoMaterialAccesorios"];
            return strTiposMaterial.Split(',');
        }

        public static string[] ListCodigosTipoCHIP()
        {
            string strTiposMaterial = ConfigurationManager.AppSettings["ConsTipoMaterialesCHIP"];
            return strTiposMaterial.Split(',');
        }

        public static string[] ListCodigosTipoEQUIPO()
        {
            string strTiposMaterial = ConfigurationManager.AppSettings["ConsTipoMaterialEQUIPO"];
            return strTiposMaterial.Split(',');
        }

        public static string[] ListCodigosTarjetas()
        {
            string strTiposMaterial = ConfigurationManager.AppSettings["ConsTipoMaterialTarjetas"];
            return strTiposMaterial.Split(',');
        }

        public static List<BEConsultaStock> listarAccesoriosCuotas()
        {
            return new DASincronizaSap().listarAccesoriosCuotas();
        }
        #endregion
    }
}
