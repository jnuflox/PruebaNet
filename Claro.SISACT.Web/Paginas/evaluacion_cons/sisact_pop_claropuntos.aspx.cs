using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Claro.SISACT.WS;
using Claro.SISACT.WS.ConsultarPuntosWS;
using Claro.SISACT.Entity;
using System.Configuration;
using Claro.SISACT.Web.Base;
using Claro.SISACT.Common;
using System.Data;
using System.Collections;
using System.Text;
using Claro.SISACT.Business;

namespace Claro.SISACT.Web.Paginas.Venta
{
    public partial class sisact_pop_claropuntos : Sisact_Webbase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string tipo = Request.QueryString["tipo"];
            string strTipoDoc = Request.QueryString["strTipoDoc"];
            string strNumDoc = Request.QueryString["strNumDoc"];
            string strTelefono = Request.QueryString["strTelefono"];

            CargarPuntosClaroClub(strTipoDoc, strNumDoc, strTelefono);
        }

        private void CargarPuntosClaroClub(string strTipoDoc, string strNumDoc, string strTelefono)
        {
            try
            {
                GeneradorLog.EscribirLog("LogClaroPunto", strNumDoc, "Inicio");
                string strTipoDocCC = Comun.WebComunes.ListaTipoDocumento().Where(x => x.Codigo == strTipoDoc).FirstOrDefault().Codigo3;
                BWClaroClub_Services oBWClaroClub_Services = new BWClaroClub_Services();
                consultarPuntosResponse oConsultarPuntosResponse = new consultarPuntosResponse();
                BEItemGenerico oAudit = new BEItemGenerico()
                {
                    Codigo = strNumDoc,
                    Codigo2 = CurrentTerminal, //PROY-24740
                    Descripcion = ConfigurationManager.AppSettings["constAplicacion"],
                    Descripcion2 = CurrentUser
                };
                oConsultarPuntosResponse = oBWClaroClub_Services.consultarPuntosClaroClub(strTipoDocCC, strNumDoc, strTelefono, oAudit);

                if (oConsultarPuntosResponse.audit.errorCode == ConfigurationManager.AppSettings["constErrorBolsaMovilDes"])
                {
                    throw new Exception(oConsultarPuntosResponse.audit.errorMsg);
                }
                else if (oConsultarPuntosResponse.audit.errorCode != "0")
                {
                    throw new Exception(ConfigurationManager.AppSettings["constErrorClaroClubNoDisponible"]);
                }

                string[] arrBolsasCC = Funciones.CheckStr(ConfigurationManager.AppSettings["constBolsasCC"]).Split(';');
                List<BEBolsa> listBolsa = new List<BEBolsa>();

                int cantPuntosPostpago = 0;
                int cantOtrosPuntos = 0;
                int cantTotalPuntos = 0;
                string codCampanaCC = "";
                int valorPuntosPostpago = 0;
                string strFactorClaroClub = Funciones.CheckStr(oConsultarPuntosResponse.factorConversion);
                hidFactorClaroClub.Value = strFactorClaroClub;
                if (oConsultarPuntosResponse.cursorSaldos != null)
                {
                    foreach (CursorSaldosType oCursorSaldos in oConsultarPuntosResponse.cursorSaldos)
                    {
                        string strTipoCliente = oCursorSaldos.tipoCliente;
                        if (strTipoCliente == ConfigurationManager.AppSettings["constTipoClientePOSTPAGO"])
                        {
                            cantPuntosPostpago += Funciones.CheckInt(oCursorSaldos.saldoTT);
                        }
                        else
                        {
                            cantOtrosPuntos += Funciones.CheckInt(oCursorSaldos.saldoTT);
                        }

                        foreach (string strDatosBolsa in arrBolsasCC)
                        {
                            if (strDatosBolsa.Split(':')[0] == strTipoCliente)
                            {
                                BEBolsa oBolsa = new BEBolsa();
                                oBolsa.TipoCliente = oCursorSaldos.tipoCliente;
                                oBolsa.Descripcion = strDatosBolsa.Split(':')[1];
                                oBolsa.SaldoTT = oCursorSaldos.saldoTT;
                                listBolsa.Add(oBolsa);
                                break;
                            }
                        }
                    }
                }

                if (oConsultarPuntosResponse.curCampana != null)
                {
                    txtCampania.Text = oConsultarPuntosResponse.curCampana[0].descripcion;
                    codCampanaCC = oConsultarPuntosResponse.curCampana[0].idCampana.ToString();
                    hidCodCampanaCC.Value = codCampanaCC;
                    string strFechaIni = Funciones.CheckStr(oConsultarPuntosResponse.curCampana[0].fecha_inicio);
                    if (strFechaIni.Length >= 10)
                    {
                        strFechaIni = string.Format("{0:dd/MM/yyyy}", Funciones.CheckDate(strFechaIni));
                    }
                    string strFechaFin = Funciones.CheckStr(oConsultarPuntosResponse.curCampana[0].fecha_fin);
                    if (strFechaFin.Length >= 10)
                    {
                        strFechaFin = string.Format("{0:dd/MM/yyyy}", Funciones.CheckDate(strFechaFin));
                    }
                    txtInicio.Text = strFechaIni;
                    txtFin.Text = strFechaFin;

                    foreach (CurCampanaType oCurCampana in oConsultarPuntosResponse.curCampana)
                    {
                        if (oCurCampana.tipo == Funciones.CheckStr(ConfigurationManager.AppSettings["constPOSTPAGO"]))
                        {
                            valorPuntosPostpago = Funciones.CheckInt(oCurCampana.valor);
                            break;
                        }
                    }
                }
                else
                {
                    txtCampania.Text = ConfigurationManager.AppSettings["constNoCampanhas"];
                }
                txtSegmento.Text = Funciones.CheckStr(oConsultarPuntosResponse.codigoSegmento);

                cantTotalPuntos = cantPuntosPostpago + cantOtrosPuntos;
                txtPuntosActual.Text = Funciones.CheckStr(cantTotalPuntos);

                double dlbPuntosEnSoles = 0.0;
                ConfigurarDatosDetalleDescuento(oConsultarPuntosResponse, ref dlbPuntosEnSoles);
                txtDescuentoActual.Text = string.Format("{0:#,#,0.00}", Math.Ceiling(dlbPuntosEnSoles));

                dgDescuento.DataSource = listBolsa;
                dgDescuento.DataBind();

                //CargarHidden();

                if (TieneReserva(strTipoDocCC, strNumDoc))
                {
                    txtPuntosActual.Text = "0";
                    txtPuntosActual.Enabled = false;
                    //lblClaroClubMsgError.Text = "El cliente tiene un bloqueo de canje puntos pendiente";
                }
                else
                {
                    //lblClaroClubMsgError.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                GeneradorLog.EscribirLog("LogClaroPunto", strNumDoc, ex.Message);
            }
        }

        private void ConfigurarDatosDetalleDescuento(consultarPuntosResponse objConsultarPuntosResponse, ref double dlbPuntosEnSoles)
        {
            decimal factorConversion = default(decimal);
            DataTable dtBase = new DataTable();
            int orden = 1;
            bool bExiste = false;

            dtBase.Columns.Add(new DataColumn("NORMAL", typeof(decimal)));
            dtBase.Columns.Add(new DataColumn("PROMOCIONAL", typeof(decimal)));
            dtBase.Columns.Add(new DataColumn("TOTAL", typeof(decimal)));
            dtBase.Columns.Add(new DataColumn("TIPO_DE_DSCTO", typeof(string)));
            dtBase.Columns.Add(new DataColumn("FACTOR", typeof(int)));
            dtBase.Columns.Add(new DataColumn("TIPO_CLIENTE", typeof(string)));
            dtBase.Columns.Add(new DataColumn("VALOR_SEGMENTO", typeof(string)));
            dtBase.Columns.Add(new DataColumn("PUNTOS", typeof(string)));
            dtBase.Columns.Add(new DataColumn("ORDEN", typeof(int)));

            factorConversion = Funciones.CheckDecimal(objConsultarPuntosResponse.factorConversion);
            if ((objConsultarPuntosResponse.curCampana != null) && objConsultarPuntosResponse.curCampana.Length > 0)
            {
                CursorSaldosType cursorSaldo = new CursorSaldosType();

                // Valor de conversión de puntos postpago.
                string constPOSTPAGO = ConfigurationManager.AppSettings["constPOSTPAGO"];
                string constTipoClientePOSTPAGO = ConfigurationManager.AppSettings["constTipoClientePOSTPAGO"];
                int PuntosPostCampaña = 1;
                int PuntosPostSegmento = 1;
                int FactorPost = 1;
                string TipoDsctoPost = null;
                for (int i = 0; i <= objConsultarPuntosResponse.curCampana.Length - 1; i++)
                {
                    if (objConsultarPuntosResponse.curCampana[i].tipo == constPOSTPAGO)
                    {
                        PuntosPostCampaña = Funciones.CheckInt(objConsultarPuntosResponse.curCampana[i].valor);
                        break;
                    }
                }
                if (objConsultarPuntosResponse.cursorSaldos != null)
                {
                    for (int i = 0; i <= objConsultarPuntosResponse.cursorSaldos.Length - 1; i++)
                    {
                        if (objConsultarPuntosResponse.cursorSaldos[i].tipoCliente == constTipoClientePOSTPAGO)
                        {
                            PuntosPostSegmento = Funciones.CheckInt(objConsultarPuntosResponse.cursorSaldos[i].valorSegmento);
                            cursorSaldo = objConsultarPuntosResponse.cursorSaldos[i];
                            bExiste = true;
                            break;
                        }
                    }
                }

                if (PuntosPostCampaña > PuntosPostSegmento)
                {
                    FactorPost = PuntosPostCampaña;
                    TipoDsctoPost = ConfigurationManager.AppSettings["constDescuentoCampanha"];
                }
                else
                {
                    FactorPost = PuntosPostSegmento;
                    TipoDsctoPost = ConfigurationManager.AppSettings["constDescuentoSegmento"];
                }
                DataRow rowPost = dtBase.NewRow();
                rowPost["NORMAL"] = Funciones.CheckDecimal(cursorSaldo.saldoTT) * factorConversion;
                rowPost["PROMOCIONAL"] = Funciones.CheckDecimal(cursorSaldo.saldoTT) * factorConversion * FactorPost;
                rowPost["TOTAL"] = Funciones.CheckDecimal(cursorSaldo.saldoTT) * factorConversion * FactorPost;
                rowPost["TIPO_DE_DSCTO"] = TipoDsctoPost;
                rowPost["FACTOR"] = FactorPost;
                rowPost["TIPO_CLIENTE"] = cursorSaldo.tipoCliente;
                rowPost["VALOR_SEGMENTO"] = cursorSaldo.valorSegmento;
                rowPost["PUNTOS"] = cursorSaldo.saldoTT;
                rowPost["ORDEN"] = orden;
                if (bExiste)
                {
                    orden = orden + 1;
                    dtBase.Rows.Add(rowPost);
                }
                bExiste = false;

                // Valor de conversión de puntos prepago.
                string constPREPAGO = ConfigurationManager.AppSettings["constPREPAGO"];
                string constTipoClientePREPAGO = ConfigurationManager.AppSettings["constTipoClientePREPAGO"];
                int PuntosPreCampaña = 1;
                int PuntosPreSegmento = 0;
                int FactorPre = 1;
                string TipoDsctoPre = null;
                for (int i = 0; i <= objConsultarPuntosResponse.curCampana.Length - 1; i++)
                {
                    if (objConsultarPuntosResponse.curCampana[i].tipo == constPREPAGO)
                    {
                        PuntosPreCampaña = Funciones.CheckInt(objConsultarPuntosResponse.curCampana[i].valor);
                        break;
                    }
                }
                if (objConsultarPuntosResponse.cursorSaldos != null)
                {
                    for (int i = 0; i <= objConsultarPuntosResponse.cursorSaldos.Length - 1; i++)
                    {
                        if (objConsultarPuntosResponse.cursorSaldos[i].tipoCliente == constTipoClientePREPAGO)
                        {
                            PuntosPreSegmento = Funciones.CheckInt(objConsultarPuntosResponse.cursorSaldos[i].valorSegmento);
                            cursorSaldo = objConsultarPuntosResponse.cursorSaldos[i];
                            bExiste = true;
                            break;
                        }
                    }
                }

                if ((PuntosPreCampaña > PuntosPreSegmento))
                {
                    FactorPre = PuntosPreCampaña;
                    TipoDsctoPre = ConfigurationManager.AppSettings["constDescuentoCampanha"];
                }
                else
                {
                    FactorPre = PuntosPreSegmento;
                    TipoDsctoPre = ConfigurationManager.AppSettings["constDescuentoSegmento"];
                }
                DataRow rowPre = dtBase.NewRow();
                rowPre["NORMAL"] = Funciones.CheckDecimal(cursorSaldo.saldoTT) * factorConversion;
                rowPre["PROMOCIONAL"] = Funciones.CheckDecimal(cursorSaldo.saldoTT) * factorConversion * FactorPre;
                rowPre["TOTAL"] = Funciones.CheckDecimal(cursorSaldo.saldoTT) * factorConversion * FactorPre;
                rowPre["TIPO_DE_DSCTO"] = TipoDsctoPre;
                rowPre["FACTOR"] = FactorPre;
                rowPre["TIPO_CLIENTE"] = cursorSaldo.tipoCliente;
                rowPre["VALOR_SEGMENTO"] = cursorSaldo.valorSegmento;
                rowPre["PUNTOS"] = cursorSaldo.saldoTT;
                rowPre["ORDEN"] = orden;
                if (bExiste)
                {
                    orden = orden + 1;
                    dtBase.Rows.Add(rowPre);
                }

                // Los demás
                string[] vBolsasCC = ConfigurationManager.AppSettings["constBolsasCC"].Split(';');
                Hashtable saldosCC = new Hashtable();
                for (int i = 0; i <= vBolsasCC.Length - 1; i++)
                {
                    string[] bolsaCC = vBolsasCC[i].Split(':');
                    saldosCC.Add(bolsaCC[0], bolsaCC[2]);
                }
                for (int i = 0; i <= objConsultarPuntosResponse.curCampana.Length - 1; i++)
                {
                    // Se evaluan aquellos que no son pre o postpago.
                    if (objConsultarPuntosResponse.curCampana[i].tipo != constPOSTPAGO && objConsultarPuntosResponse.curCampana[i].tipo != constPREPAGO && objConsultarPuntosResponse.cursorSaldos != null)
                    {
                        for (int j = 0; j <= objConsultarPuntosResponse.cursorSaldos.Length - 1; j++)
                        {
                            string tipo = saldosCC[objConsultarPuntosResponse.cursorSaldos[j].tipoCliente].ToString().Trim();
                            if (tipo == objConsultarPuntosResponse.curCampana[i].tipo.Trim())
                            {
                                DataRow rowOtros = dtBase.NewRow();
                                rowOtros["NORMAL"] = Funciones.CheckDecimal(objConsultarPuntosResponse.cursorSaldos[j].saldoTT) * factorConversion;
                                rowOtros["PROMOCIONAL"] = Funciones.CheckDecimal(objConsultarPuntosResponse.cursorSaldos[j].saldoTT) * factorConversion * FactorPost;
                                rowOtros["TOTAL"] = Funciones.CheckDecimal(objConsultarPuntosResponse.cursorSaldos[j].saldoTT) * factorConversion * FactorPost;
                                rowOtros["TIPO_DE_DSCTO"] = TipoDsctoPost;
                                rowOtros["FACTOR"] = FactorPost;
                                rowOtros["TIPO_CLIENTE"] = objConsultarPuntosResponse.cursorSaldos[j].tipoCliente;
                                rowOtros["VALOR_SEGMENTO"] = objConsultarPuntosResponse.cursorSaldos[j].valorSegmento;
                                rowOtros["PUNTOS"] = objConsultarPuntosResponse.cursorSaldos[j].saldoTT;
                                rowOtros["ORDEN"] = orden;
                                orden = orden + 1;
                                dtBase.Rows.Add(rowOtros);
                            }
                        }
                    }
                }
            }
            else
            {
                CursorSaldosType cursorSaldo = new CursorSaldosType();

                // Valor de conversión de puntos postpago.
                string constPOSTPAGO = ConfigurationManager.AppSettings["constPOSTPAGO"];
                string constTipoClientePOSTPAGO = ConfigurationManager.AppSettings["constTipoClientePOSTPAGO"];
                int PuntosPostCampaña = 1;
                int PuntosPostSegmento = 1;
                int FactorPost = 1;
                string TipoDsctoPost = null;

                if (objConsultarPuntosResponse.cursorSaldos != null)
                {
                    for (int i = 0; i <= objConsultarPuntosResponse.cursorSaldos.Length - 1; i++)
                    {
                        if (objConsultarPuntosResponse.cursorSaldos[i].tipoCliente == constTipoClientePOSTPAGO)
                        {
                            PuntosPostSegmento = Funciones.CheckInt(objConsultarPuntosResponse.cursorSaldos[i].valorSegmento);
                            cursorSaldo = objConsultarPuntosResponse.cursorSaldos[i];
                            bExiste = true;
                            break;
                        }
                    }
                }

                if (PuntosPostCampaña > PuntosPostSegmento)
                {
                    FactorPost = PuntosPostCampaña;
                    TipoDsctoPost = ConfigurationManager.AppSettings["constDescuentoCampanha"];
                }
                else
                {
                    FactorPost = PuntosPostSegmento;
                    TipoDsctoPost = ConfigurationManager.AppSettings["constDescuentoSegmento"];
                }
                DataRow rowPost = dtBase.NewRow();
                rowPost["NORMAL"] = Funciones.CheckDecimal(cursorSaldo.saldoTT) * factorConversion;
                rowPost["PROMOCIONAL"] = Funciones.CheckDecimal(cursorSaldo.saldoTT) * factorConversion * FactorPost;
                rowPost["TOTAL"] = Funciones.CheckDecimal(cursorSaldo.saldoTT) * factorConversion * FactorPost;
                rowPost["TIPO_DE_DSCTO"] = TipoDsctoPost;
                rowPost["FACTOR"] = FactorPost;
                rowPost["TIPO_CLIENTE"] = cursorSaldo.tipoCliente;
                rowPost["VALOR_SEGMENTO"] = cursorSaldo.valorSegmento;
                rowPost["PUNTOS"] = cursorSaldo.saldoTT;
                rowPost["ORDEN"] = orden;
                if (bExiste)
                {
                    orden = orden + 1;
                    dtBase.Rows.Add(rowPost);
                }
                bExiste = false;

                // Valor de conversión de puntos prepago.
                string constPREPAGO = ConfigurationManager.AppSettings["constPREPAGO"];
                string constTipoClientePREPAGO = ConfigurationManager.AppSettings["constTipoClientePREPAGO"];
                int PuntosPreCampaña = 1;
                int PuntosPreSegmento = 0;
                int FactorPre = 1;
                string TipoDsctoPre = null;
                if (objConsultarPuntosResponse.cursorSaldos != null)
                {
                    for (int i = 0; i <= objConsultarPuntosResponse.cursorSaldos.Length - 1; i++)
                    {
                        if (objConsultarPuntosResponse.cursorSaldos[i].tipoCliente == constTipoClientePREPAGO)
                        {
                            PuntosPreSegmento = Funciones.CheckInt(objConsultarPuntosResponse.cursorSaldos[i].valorSegmento);
                            cursorSaldo = objConsultarPuntosResponse.cursorSaldos[i];
                            bExiste = true;
                            break;
                        }
                    }
                }

                if ((PuntosPreCampaña > PuntosPreSegmento))
                {
                    FactorPre = PuntosPreCampaña;
                    TipoDsctoPre = ConfigurationManager.AppSettings["constDescuentoCampanha"];
                }
                else
                {
                    FactorPre = PuntosPreSegmento;
                    TipoDsctoPre = ConfigurationManager.AppSettings["constDescuentoSegmento"];
                }
                DataRow rowPre = dtBase.NewRow();
                rowPre["NORMAL"] = Funciones.CheckDecimal(cursorSaldo.saldoTT) * factorConversion;
                rowPre["PROMOCIONAL"] = Funciones.CheckDecimal(cursorSaldo.saldoTT) * factorConversion * FactorPre;
                rowPre["TOTAL"] = Funciones.CheckDecimal(cursorSaldo.saldoTT) * factorConversion * FactorPre;
                rowPre["TIPO_DE_DSCTO"] = TipoDsctoPre;
                rowPre["FACTOR"] = FactorPre;
                rowPre["TIPO_CLIENTE"] = cursorSaldo.tipoCliente;
                rowPre["VALOR_SEGMENTO"] = cursorSaldo.valorSegmento;
                rowPre["PUNTOS"] = cursorSaldo.saldoTT;
                rowPre["ORDEN"] = orden;
                if (bExiste)
                {
                    orden = orden + 1;
                    dtBase.Rows.Add(rowPre);
                }

                // Los demás
                string[] vBolsasCC = ConfigurationManager.AppSettings["constBolsasCC"].Split(';');
                Hashtable saldosCC = new Hashtable();
                for (int i = 0; i <= vBolsasCC.Length - 1; i++)
                {
                    string[] bolsaCC = vBolsasCC[i].Split(':');
                    saldosCC.Add(bolsaCC[0], bolsaCC[2]);
                }
                // Se evaluan aquellos que no son pre o postpago.
                if (objConsultarPuntosResponse.cursorSaldos != null)
                {
                    for (int j = 0; j <= objConsultarPuntosResponse.cursorSaldos.Length - 1; j++)
                    {
                        string tipoCliente = objConsultarPuntosResponse.cursorSaldos[j].tipoCliente;
                        if (tipoCliente != constTipoClientePOSTPAGO && tipoCliente != constTipoClientePREPAGO)
                        {
                            DataRow rowOtros = dtBase.NewRow();
                            rowOtros["NORMAL"] = Funciones.CheckDecimal(objConsultarPuntosResponse.cursorSaldos[j].saldoTT) * factorConversion;
                            rowOtros["PROMOCIONAL"] = Funciones.CheckDecimal(objConsultarPuntosResponse.cursorSaldos[j].saldoTT) * factorConversion * FactorPost;
                            rowOtros["TOTAL"] = Funciones.CheckDecimal(objConsultarPuntosResponse.cursorSaldos[j].saldoTT) * factorConversion * FactorPost;
                            rowOtros["TIPO_DE_DSCTO"] = TipoDsctoPost;
                            rowOtros["FACTOR"] = FactorPost;
                            rowOtros["TIPO_CLIENTE"] = objConsultarPuntosResponse.cursorSaldos[j].tipoCliente;
                            rowOtros["VALOR_SEGMENTO"] = objConsultarPuntosResponse.cursorSaldos[j].valorSegmento;
                            rowOtros["PUNTOS"] = objConsultarPuntosResponse.cursorSaldos[j].saldoTT;
                            rowOtros["ORDEN"] = orden;
                            orden = orden + 1;
                            dtBase.Rows.Add(rowOtros);
                        }
                    }
                }
            }

            if (dtBase.Rows.Count > 0)
            {
                StringBuilder qs = new StringBuilder();
                decimal total = 0;
                int puntos = 0;

                foreach (DataRow dr in dtBase.Rows)
                {
                    qs.Append(dr["NORMAL"]);
                    qs.Append("|");
                    qs.Append(dr["PROMOCIONAL"]);
                    qs.Append("|");
                    qs.Append(dr["TOTAL"]);
                    qs.Append("|");
                    qs.Append(dr["TIPO_DE_DSCTO"]);
                    qs.Append("|");
                    qs.Append(dr["FACTOR"]);
                    qs.Append("|");
                    qs.Append(dr["TIPO_CLIENTE"]);
                    qs.Append("|");
                    qs.Append(dr["VALOR_SEGMENTO"]);
                    qs.Append("|");
                    qs.Append(dr["PUNTOS"]);
                    qs.Append("|");
                    qs.Append(dr["ORDEN"]);
                    qs.Append("~");
                    puntos = puntos + Funciones.CheckInt(dr["PUNTOS"]);
                    total = total + Funciones.CheckDecimal(dr["TOTAL"]);
                }
                qs.Append(Math.Ceiling(total).ToString("n2"));

                //txtClaroClubSolesDeDescuento.Text = Math.Ceiling(total).ToString("n2");
                //hidClaroClubSolesDeDescuento.Value = Math.Ceiling(total).ToString("n2");
                hidDetalleDescuento.Value = qs.ToString();
                dlbPuntosEnSoles = Double.Parse(Math.Ceiling(total).ToString("n2"));
            }
        }

        public static bool TieneReserva(string k_tipo_doc, string k_num_doc)
        {
            string k_tipo_clie = ConfigurationManager.AppSettings["consTipoclie"];
            string k_tipo_doc2 = null;
            string k_estado = null;
            double k_coderror = 0;
            string k_descerror = null;
            string constVALBLOQUEOBOLSA = ConfigurationManager.AppSettings["constVALBLOQUEOBOLSA"];

            try
            {
                BLConsultaClaroClub.ValidaBloqueoBolsa(k_tipo_doc, k_num_doc, k_tipo_clie, ref k_tipo_doc2, ref k_estado, ref k_coderror, ref k_descerror);

                if (k_coderror == 0.0 && constVALBLOQUEOBOLSA == k_estado)
                {
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}