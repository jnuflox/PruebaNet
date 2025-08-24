using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Configuration;
using System.Web.UI.WebControls;
using Claro.SISACT.Web.Base;
using Claro.SISACT.WS;
using Claro.SISACT.Common;
using Claro.SISACT.Entity;
using Claro.SISACT.Business;

namespace Claro.SISACT.Web.Paginas.frames
{
    public partial class siact_pop_consulta3g :  Sisact_Webbase //System.Web.UI.Page
    {
        private string canal;
        public string consMensajeLineas3G;
       
        protected void Page_Load(object sender, EventArgs e)
        {
            GeneradorLog objLog = new GeneradorLog("webmethod averiguar", "12345", null, "sisact_ifr_consulta3g");
            objLog.CrearArchivolog("[llamada a servicio web consultalineas 3g]", null, null);
            obtenerMsgLinea3g();
            if (!Page.IsPostBack)
            {
                //procedo con la invocacion de las lineas para la persona con el documento de indentidad se muestra
                canal = Request.QueryString["canal"];
                string tdoc  = Request.QueryString["tipodoc"];
                string ndoc = Request.QueryString["numdoc"];
                BLConsultaLineasTecnologia consultaWS = new BLConsultaLineasTecnologia();
                //llamado al servicio y enlezarlo al databinding
                string codResp, mensaje;
                WS.WSLineasTecnologiaCliente.ListaLineaTypeListaLineas[] lista = null;
                WS.WSLineasTecnologiaCliente.ListaLineaTypeListaLineas[] resultado = consultaWS.consultarLineasPrePost(tdoc, ndoc, out codResp, out mensaje);
                int cantidad = obtenerCantLinea3g();
                if(resultado!=null) lista = resultado.OrderBy(x => x.planLinea).ToArray<WS.WSLineasTecnologiaCliente.ListaLineaTypeListaLineas>();
                if (resultado != null && lista.Length >= cantidad)
                {
                    WS.WSLineasTecnologiaCliente.ListaLineaTypeListaLineas[] sublista=new WS.WSLineasTecnologiaCliente.ListaLineaTypeListaLineas[cantidad];
                    for (int i = 0; i < cantidad; i++)
                        sublista[i] = lista[i];
                    //lista.CopyTo(sublista,(long) cantidad);
                    this.dgLineas3g.DataSource = sublista;
                }
                else
                    this.dgLineas3g.DataSource = lista;
                objLog.CrearArchivolog("[cantidad de lineas] "+cantidad, null, null);
                dgLineas3g.DataBind();
            }
        }

         private void obtenerMsgLinea3g()
        {
            //obtengo el mensaje desde el sisact_parametro
            long codGrupoLineas3G = Funciones.CheckInt64(ConfigurationManager.AppSettings["codGrupoLineas3g"]);
            List<BEParametro> Lista = (new BLGeneral()).ListaParametrosGrupo(codGrupoLineas3G);
            //busco de la lista el mensaje a mostrar.
            BEParametro sisactParam = Lista.SingleOrDefault(x => x.Valor1 == "34");
            consMensajeLineas3G = sisactParam.Valor;
        }

        protected void dgLineas3g_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;
            WS.WSLineasTecnologiaCliente.ListaLineaTypeListaLineas lineamost = (WS.WSLineasTecnologiaCliente.ListaLineaTypeListaLineas)e.Row.DataItem;

            Literal nlinea = (Literal)e.Row.FindControl("numlinea");
            //si el canal no es CAC, debo de mostrar las lineas con los asteriscos
            if (canal != ConfigurationManager.AppSettings["constCodTipoCAC"])
            {
                string cad=lineamost.linea.Substring(0,6);
                nlinea.Text = cad + "***";
            }
            else
                nlinea.Text = lineamost.linea;
        }

         private int obtenerCantLinea3g()
        {
            //obtengo el mensaje desde el sisact_parametro
            long codGrupoLineas3G = Funciones.CheckInt64(ConfigurationSettings.AppSettings["codGrupoLineas3g"]);
            List<BEParametro> Lista = (new BLGeneral()).ListaParametrosGrupo(codGrupoLineas3G);
            //busco de la lista el mensaje a mostrar.
            BEParametro sisactParam = Lista.SingleOrDefault(x => x.Valor1 == "35");
            return Int32.Parse(sisactParam.Valor);
        }

       
    }
}