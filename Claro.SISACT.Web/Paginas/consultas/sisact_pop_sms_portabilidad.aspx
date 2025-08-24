<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sisact_pop_sms_portabilidad.aspx.cs"
    Inherits="Claro.SISACT.Web.Paginas.consultas.sisact_pop_sms_portabilidad" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Verificación SMS Portabilidad</title>
    <link href="../../Estilos/general.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" language="JavaScript" src="../../Scripts/funciones_generales.js"></script>
        <script language="javascript" type="text/javascript" src="../../Scripts/jquery-1.9.1.js"></script>
    <script type="text/javascript" language="JavaScript" src="../../Scripts/Paginas/consultas/sisact_pop_sms_portabilidad.js"></script>
        <script language="javascript" type="text/javascript" src="../../Scripts/security.js"></script>
</head>
<body style="height: 216px" onload="EnviarSMSPortabilidades('<%=Request.QueryString["tipoDocumento"] %>','<%=Request.QueryString["nroDocumento"]%>','<%=Request.QueryString["nroPortabilidad"]%>','<%=Request.QueryString["codPortabilidad"]%>')">
    <!--INC-SMS_PORTA-->
    <table cellspacing="2" cellpadding="0" width="100%" border="0">
        <tr>
            <td align="center">
    <div style="text-align: center">
                    <h2 class="Arial15">
                        <strong style="text-align: center; color: red">Solicita al Cliente PIN de seguridad</strong></h2>
    </div>
            </td>
        </tr>
        <tr>
            <td align="center">
    <form id="frmPrincipal" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" EnablePartialRendering="true">
    </asp:ScriptManager>
    <% string tipoDocumento = Request.QueryString["tipoDocumento"];
       string nroDocumento = Request.QueryString["nroDocumento"];
       string datosportabilidad = Request.QueryString["nroPortabilidad"];
                   string codPortabilidad = Request.QueryString["codPortabilidad"];
       string estado = "1";
       string[] nrotelefonos = datosportabilidad.Split(';');
       int cantNroTelefono = nrotelefonos.Length;
    %>
    <input id="hidlineas" type="hidden" value="<%=cantNroTelefono %>" />
                <!--//INC-SMS_PORTA_INI-->
                <input id="hidtipoDocumento" type="hidden" value="<%=tipoDocumento %>" />
                <input id="hidnroDocumento" type="hidden" value="<%=nroDocumento %>" />
                <input id="hiddatosportabilidad" type="hidden" value="<%=datosportabilidad %>" />
                <input id="hidcodportabilidad" type="hidden" value="<%=codPortabilidad %>" />
                <!--//INC-SMS_PORTA_FIN-->
    <div class="style1" style="display: inline-block; text-align: center">
        <% for (int i = 0; i < nrotelefonos.Length; i++)
               {%>
        <div id="registro<%= i %>">
            <table class="Contenido" cellspacing="4" cellpadding="6">
                <tr>
                    <td>
                        <p id="nrotelefonos<%= i %>" style="text-align: left" class="Arial10B">
                            <%= nrotelefonos[i]%></p>
                    </td>
                    <td>
                         <input id="codPin<%=i %>" style="width: 50px" class="clsInputDisable" maxlength="5" onkeypress="eventoSoloNumeros(event);" />
                    </td>
                    <td>
                        &nbsp;<input id="msjServicio<%= i %>" class="clsInputDisable" readonly="true"/>
                        <input id="hidEstado<%= i %>" type="hidden" value="<%=estado %>" />
                    </td>
                </tr>
            </table>
        </div>
        <%} %>
                    <table cellspacing="2" cellpadding="6">
                        <tr>
                            <td align="center">
            <asp:Label ID="tiempoRest" runat="server" Text="Tiempo restante" Style="text-align: center"
                CssClass="Arial10B"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                        <td align="center">
                        <asp:Label ID="temporizador" runat="server" Text=":" Style="text-align: center"
                                    CssClass="Arial10B"></asp:Label>
                        </td>
                        </tr>
                        <tr>
                            <td align="center">
							
							<input type="button" id="btnValidar1" class="Boton" style="width: 100px; cursor: hand; height: 19px"
                                    onclick="getValidarPin();" value="Validar" /> 
							
                           
                                <input type="button" id="btnCancelar" class="Boton" style="width: 100px; cursor: hand; height: 19px"
                                    onclick="retornoValue('')" value="Cancelar" />
                            </td>
                        </tr>
                    </table>
        </div>
        <input id="hidTimer" type="hidden" />
        <input id="hidCodSMS" type="hidden" />
        <input id="hidMensajeSMS" type="hidden" />
    </form>
            </td>
        </tr>
    </table>
</body>
</html>
