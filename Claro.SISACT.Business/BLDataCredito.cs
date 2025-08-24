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
    public class BLDataCredito
    {
        public List<BEItemGenerico> ListarRespuestaDC()
        {
            return new DADataCredito().ListarRespuestaDC();
        }

        public string AsignarBuroCrediticio(string tipoDocumento, ref string strUrl, ref string strKey)
        {
            return new DADataCredito().AsignarBuroCrediticio(tipoDocumento, ref strUrl, ref strKey);
        }

        public BEDataCreditoOUT ConsultarDCRepositorioPersona(BEDataCreditoIN objIN, string tipoSEC, ref BEItemMensaje objMensaje)
        {
            return new DADataCredito().ConsultarDCRepositorioPersona(objIN, tipoSEC, ref objMensaje);
        }

        public BEDataCreditoCorpOUT ConsultarDCRepositorioEmpresa(BEDataCreditoCorpIN objIN)
        {
            return new DADataCredito().ConsultarDCRepositorioEmpresa(objIN);
        }

        public BEItemMensaje GrabarDatosDCPersona(BEDataCreditoIN objIN, BEDataCreditoOUT objOUT)
        {
            return new DADataCredito().GrabarDatosDCPersona(objIN, objOUT);
        }

        public BEItemMensaje GrabarDatosDCEmpresa(BEDataCreditoCorpOUT obj)
        {
            return new DADataCredito().GrabarDatosDCEmpresa(obj);
        }

        public BEItemMensaje GrabarDCInputOutput(BEDataCreditoINOUT obj)
        {
            return new DADataCredito().GrabarDCInputOutput(obj);
        }

        public void GrabarDCHistorico(BEDataCreditoHistorico obj)
        {
            new DADataCredito().GrabarDCHistorico(obj);
        }

        public bool GrabarDCReporte(Vista_SolicitudDC_Reporte obj)
        {
            return new DADataCredito().GrabarDCReporte(obj);
        }

        public bool GrabarRazonesEvaluacion(Int64 nroSEC, string strNodo)
        {
            return new DADataCredito().GrabarRazonesEvaluacion(nroSEC, strNodo);
        }

        public bool ActualizarDCHistorico(string nroOperacion, string validarCliente)
        {
            return new DADataCredito().ActualizarDCHistorico(nroOperacion, validarCliente);
        }
    }
}
