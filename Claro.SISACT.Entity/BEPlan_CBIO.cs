using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    public class BEPlan_CBIO
    {
        //PLAN
        public string PLANC_CODIGO { get; set; }
        public string PLANV_DESCRIPCION { get; set; }
        public double PLANN_CAR_FIJ { get; set; }
        public int CODIGO_BSCS { get; set; }
        public string PRDC_CODIGO { get; set; }
        public string ESTADO { get; set; }
        public string TECNOLOGIA_CODIGO { get; set; }
        public string TECNOLOGIA_DESCRIPCION { get; set; }
        public List<BEPlan_ServiciosCBIO> lstServicios { get; set; }
        public List<BEPlan_BilleterasCBIO> lstBilletera { get; set; }
        //PLAN
    }

    public class BEPlan_ServiciosCBIO{
        //SERVICIO
        public string SERVV_CODIGO { get; set; }
        public string SERVV_DESCRIPCION { get; set; }
        public string GSRVC_CODIGO { get; set; }
        public double SERVN_PRECIO_BASE { get; set; }
        public string SERVV_ID_BSCS { get; set; }
        public string S_PRDC_CODIGO { get; set; }
        public string SERVV_DES_BSCS { get; set; }
        public string SERVV_PO_ID { get; set; }
        //SERVICIO
    }

    public class BEPlan_BilleterasCBIO
    {
        //BILLETERA
        public string B_PLANC_CODIGO { get; set; }
        public string B_PRDC_CODIGO { get; set; }
        public string PLNV_PO_ID { get; set; }
        public int PRCLN_CODIGO { get; set; }
        public string PRCLV_DESCRIPCION { get; set; }
        //BILLETERA
    }
}
