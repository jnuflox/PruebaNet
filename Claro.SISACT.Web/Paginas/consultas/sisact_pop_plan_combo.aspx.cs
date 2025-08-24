using System;
using System.Collections.Generic;
using System.Collections;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Claro.SISACT.Business;
using Claro.SISACT.Entity;
using System.Text;
using Claro.SISACT.Web.Base; //PROY-140126 - IDEA 140248 

namespace Claro.SISACT.Web.Paginas.consultas
{
    public partial class sisact_pop_plan_combo : Sisact_Webbase //PROY-140126 - IDEA 140248  
    {
        #region [Declaracion de Constantes - Config]

        string strCodSrvSinBolsaCompartida = ConfigurationManager.AppSettings["consCodSrvSinBolsaCompartida"].ToString();
        string strSrvSinBolsaCompartida = ConfigurationManager.AppSettings["consSrvSinBolsaCompartida"].ToString();

        #endregion [Declaracion de Constantes - Config]

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            if (Session["Usuario"] == null)
            {
                string strRutaSite = ConfigurationManager.AppSettings["RutaSite"];
                Response.Redirect(strRutaSite);
                Response.End();
                return;
            }

            Inicio();
        }

        //PROY-24740
        public void Inicio()
        {
            string codPlanBase = Request.QueryString["idPlanBase"];
            string desPlanBase = Request.QueryString["planBase"];

            List<BEPlan> lstPlanCombo = new BLGeneral().ListarPlanBaseCombo(codPlanBase);
            StringBuilder sblValores = new StringBuilder();

            BEPlan objItem = new BEPlan();
            objItem.PLANC_CODIGO = codPlanBase;
            objItem.PLANV_DESCRIPCION = desPlanBase;
            objItem.SERV_CODIGO = strCodSrvSinBolsaCompartida;
            objItem.SERV_DESCRIPCION = strSrvSinBolsaCompartida;
            sblValores.Append(String.Format("<input type='radio' name='radLista' value='{0}|{1}|{2}' onclick='seleccionarBolsa(this.value)'>{3}</input>", objItem.PLANC_CODIGO, objItem.PLANV_DESCRIPCION, objItem.SERV_CODIGO, objItem.SERV_DESCRIPCION));                           

            foreach (BEPlan item in lstPlanCombo)
            {
                StringBuilder sblPlan = new StringBuilder();
                sblPlan.Append(item.PLANC_CODIGO);
                sblPlan.Append("_");
                sblPlan.Append(item.PLANN_CAR_FIJ.ToString());
                sblPlan.Append("_");
                sblPlan.Append(item.PLANC_EQUI_SAP.ToString());
                sblPlan.Append("_");
                sblPlan.Append(item.PLNN_TIPO_PLAN);
                sblPlan.Append("_");
                sblPlan.Append("_");
                sblPlan.Append(item.GPLNV_DESCRIPCION);
                sblPlan.Append("_");
                sblPlan.Append(item.CODIGO_BSCS);
                sblPlan.Append("_");
                sblPlan.Append(item.TIPO_PRODUCTOS);
                sblPlan.Append(";");
                sblPlan.Append(item.PLANV_DESCRIPCION);

                sblValores.Append(String.Format("<br /><input type='radio' name='radLista' value='{0}|{1}|{2}|{3}' onclick='seleccionarBolsa(this.value)'>{4}</input>", sblPlan.ToString(), item.PLANV_DESCRIPCION, item.SERV_CODIGO, item.SERV_DESCRIPCION, item.SERV_DESCRIPCION));                
            }

            lstPlanCombo.Insert(0, objItem);

            litLista.Text = sblValores.ToString();
            txtDesPlanSeleccionado.Text = desPlanBase.ToUpper();
        }
    }
}