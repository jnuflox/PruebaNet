using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Collections;
using Claro.SISACT.Business;
using Claro.SISACT.Common;
using Claro.SISACT.Web.Base; //PROY-140126 - IDEA 140248 

namespace Claro.SISACT.Web.Paginas.frames
{
    public partial class sisact_ifr_GrillaCentroPoblado : Sisact_Webbase //PROY-140126 - IDEA 140248  
    {
        private void Page_Load(System.Object sender, System.EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            if (Session["Usuario"] == null)
            {
                string strRutaSite = ConfigurationManager.AppSettings["RutaSite"];
                Response.Redirect(strRutaSite);
                Response.End();
                return;
            }

            llenarGrilla();
        }

        public void llenarGrilla()
        {
            try
            {
                string StrValor = Request.QueryString["strValor"];
                string CodProd = Request.QueryString["CodProd"];//FTTH -Cod. Producto Plano
                int codError = 0;
                ArrayList lista = new BLIntegracionDTHNegocio().ListarCentrosPoblados(StrValor, ref codError);
                //FTTH Inicio
                ArrayList listaFTTH = new ArrayList();
                Claro.SISACT.Entity.BEDireccionPlano objItemCP = new Claro.SISACT.Entity.BEDireccionPlano();

                if (string.IsNullOrEmpty(Funciones.CheckStr(ConfigurationManager.AppSettings["constFLAGProductoFTTH"])))//FLAG - FTTH - HFC
                {
                    string[] arrsPlanoCampanaFTTH = ConfigurationManager.AppSettings["ConsPlanoCampanaFTTH"].Split('|');

                    if (CodProd == Funciones.CheckStr(ConfigurationManager.AppSettings["constTipoProductoFTTH"])) //FTTH - COD. PRODUCTO  PLANO
                    {
                        foreach (Claro.SISACT.Entity.BEDireccionPlano array in lista)
                        {
                            if (array.IdPlano.ToUpper().Contains(arrsPlanoCampanaFTTH[1].ToUpper()))
                            {
                                listaFTTH.Add(array);
                            }
                        }

                    }
                    else
                    {
                        foreach (Claro.SISACT.Entity.BEDireccionPlano array in lista)
                        {
                            if (!array.IdPlano.ToUpper().Contains(arrsPlanoCampanaFTTH[1].ToUpper()))
                            {
                                listaFTTH.Add(array);
                            }
                        }
                    }
                    lista = listaFTTH;
                }
                //FTTH Fin

                if (codError != -1)
                {

                    
                    if (CodProd == Funciones.CheckStr(ConfigurationManager.AppSettings["constTipoProductoFTTH"]))
                    {
                        string[] arrsPlanoCampanaFTTH = ConfigurationManager.AppSettings["ConsPlanoCampanaFTTH"].Split('|');


                        
                        ArrayList listNewFTTH = new ArrayList();
                        foreach (Claro.SISACT.Entity.BEDireccionPlano array in lista)
                        {
                            if (array.IdPlano.ToUpper().Contains(arrsPlanoCampanaFTTH[1].ToUpper()))
                            {
                                listNewFTTH.Add(array);
                            }

                        }

                        this.dgPDV.DataSource = listNewFTTH;
                        this.dgPDV.DataBind();
                        hidNResponseValue.Value = "TIENEDATOS";
                        hidDatosRetorno.Value = "";
                        hidNroFilas.Value = Funciones.CheckStr(listNewFTTH.Count);

                    }
                    else
                    {
                    this.dgPDV.DataSource = lista;
                    this.dgPDV.DataBind();
                    hidNResponseValue.Value = "TIENEDATOS";
                    hidDatosRetorno.Value = "";
                    hidNroFilas.Value = Funciones.CheckStr(lista.Count);

                    }
                                 
                }
                else
                {
                    hidNResponseValue.Value = "ERROR";
                    hidDatosRetorno.Value = "";
                }
            }
            catch (Exception)
            {
                hidNResponseValue.Value = "ERROR";
                hidDatosRetorno.Value = "";
            }
        }

        protected void dgPDV_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            dgPDV.CurrentPageIndex = e.NewPageIndex;
            llenarGrilla();
        }

        protected void rdoSeleccion_CheckedChanged(object sender, System.EventArgs e)
        {
            try
            {
                RadioButton myRadioButton;
                foreach (DataGridItem dgritem in dgPDV.Items)
                {
                    myRadioButton = (RadioButton)dgritem.FindControl("fila");
                    //Selecciona solo uno, deselecciona los demas
                    if ((!object.ReferenceEquals(myRadioButton, (RadioButton)sender)))
                        myRadioButton.Checked = false;
                    else
                        dgritem.BackColor = System.Drawing.Color.FromArgb(244, 250, 182);
                }
            }
            catch (Exception)
            { }
        }
    }
}