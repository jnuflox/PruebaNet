<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sisact_pop_validar_supervisor.aspx.cs"
    Inherits="Claro.SISACT.Web.Paginas.consultas.sisact_pop_validar_supervisor" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Identifica Supervisor</title>
    <link href="../../Estilos/general.css" type="text/css" rel="stylesheet" />
        <script language="javascript" type="text/javascript" src="../../Scripts/jquery-1.9.1.js"></script>
        <script language="javascript" type="text/javascript" src="../../Scripts/security.js"></script>
    <script type="text/javascript">

        function ValidarSupervisor() {
            if (validaData()) {
                document.getElementById('btnAceptar').disabled = true;
                document.getElementById('btnCancelar').disabled = true;
                var userSuperv = document.getElementById("txtIdSuperv").value;
                var userPass = document.getElementById("txtPassSuperv").value;
                var causalOmision = document.getElementById('hidCausal').value;
                PageMethods.ValidarSupervisor(userSuperv, userPass, causalOmision,validarSupervisor_callBack);
            } else {
                document.getElementById('btnAceptar').disabled = false;
                document.getElementById('btnCancelar').disabled = false;
                return;
            }
        }

        function validaData() {
            var msjValidacion = document.getElementById("lblMensaje");
            var usrTxt = document.getElementById('txtIdSuperv').value;
            var passTxt = document.getElementById('txtPassSuperv').value;
            var hdCausal = document.getElementById('hidCausal').value;
            var cont = 0;
            if (usrTxt.length == 0) {
                msjValidacion.innerHTML = "Ingresar nombre de Usuario";
                cont++;
            }
            else if (passTxt.length == 0) {
                    msjValidacion.innerHTML = "Ingresar constraseña de Usuario";
                    cont++;
                }
            else {
                if (hdCausal == '') {
                    msjValidacion.innerHTML = "Seleccione una Causal de Omisión de PIN.";
                    cont++;
                }
            }

            if (cont > 0) {
                return false;
            }
            else {
                return true;
            }


        }

        function validarSupervisor_callBack(response) {
            document.getElementById('btnAceptar').disabled = false;
            document.getElementById('btnCancelar').disabled = false;
            var msjValidacion = document.getElementById("lblMensaje");
            if (response != null) {
                if (response.CodigoError != 0) {
                    msjValidacion.innerHTML = response.Mensaje;
                    retornoValue(response.Mensaje);
                }
                else {
                    msjValidacion.innerHTML = response.Mensaje;
                    alert(response.Mensaje);
                    retornoValue("true");
                }
            } else {
                msjValidacion.innerHTML = response.Mensaje;
                retornoValue(response.Mensaje);
            }

        }

        function retornoValue(msg) {
            window.returnValue = msg;
            window.close();
            return;
        }

        function f_ValidarEnter(event) {
            if (document.all) {
                if (event.keyCode == 13) {
                    ValidarSupervisor();
                    event.keyCode = 0;
                }
            }
        }

        function f_Respuesta(valor) {
            var nroCausal = valor.split('_')[0];
            var descripcionCausal = valor.split('_')[1];

            if (nroCausal != "" && descripcionCausal != "") {
                document.getElementById('hidCausal').value = valor; 
            }

        }

    </script>
</head>
<body>
    <form runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" EnablePartialRendering="true">
    </asp:ScriptManager>
    <table id="tblValSuper" class="Arial11BV" style="z-index: 101; position: absolute;
        width: 420px; height: 300px; top: 2px; left: 2px" bordercolor="#336699" cellspacing="0"
        cellpadding="0" border="2">
        <tr>
            <td>
                <table class="Arial11BV" bordercolor="#336699" cellspacing="4" cellpadding="6" width="100%"
                    border="0" >
                    <thead>
                        <tr>
                            <td valign="top" align="center" style="background-color: #5991bb" colspan="3">
                                <asp:Label ID="lblTitulo" runat="server" Font-Size="12pt" Font-Bold="True" ForeColor="White">AUTORIZACIÓN</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" align="center" bgcolor="#ffffff" colspan="3">
                                <label id="lblMensaje" class="Arial10B" style="color:Red";></label>
                            </td>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td class="Arial10B" style="height: 20px; width: 200px;" align="right">
                                Usuario:
                            </td>
                            <%-- <td style="padding-left: 3px; width: 20px;" align="left">
                            </td>--%>
                            <td colspan="2">
                                <asp:TextBox ID="txtIdSuperv" name="inputSuperv" runat="server" CssClass="form-control inp-t"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="Arial10B" style="height: 20px; width: 200px;" align="right">
                                Contraseña:
                            </td>
                            <%--<td style="padding-left: 3px; width: 20px;" align="left">
                            </td>--%>
                            <td colspan="2">
                                <asp:TextBox ID="txtPassSuperv" name="inputSuperv" runat="server" CssClass="form-control inp-t"
                                    TextMode="Password" onkeypress="f_ValidarEnter(event)"></asp:TextBox>
                            </td>
                        </tr>
                    </tbody>
                    <tfoot>
                        <tr>
                            <td colspan="3">
                                <table width="100%" cellspacing="2" cellpadding="0" border="0" align="center">
                                    <tr>
                                        <td style="border: 0">
                                            <div id="Lista" style="overflow: auto; width: 100%; border: 0">
                                                <asp:GridView ID="dgdLista" runat="server" BorderWidth="0px" AutoGenerateColumns="False"
                                                    OnRowDataBound="dgdLista_RowDataBound" Width="420px">
                                                    <AlternatingRowStyle HorizontalAlign="Center" Height="15px" CssClass="Arial10" />
                                                    <RowStyle HorizontalAlign="Center" Height="15px" CssClass="Arial10" />
                                                    <HeaderStyle CssClass="TablaTitulos"></HeaderStyle>
                                                    <Columns>
                                                        <asp:BoundField DataField="descripcion" HeaderText="Causal de Omisión de PIN :">
                                                            <HeaderStyle HorizontalAlign="left" Width="350px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Left" Width="0px" BorderStyle="None"></ItemStyle>
                                                        </asp:BoundField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td align="right">
                                            <input type="button" id="btnAceptar" class="Boton" onclick="ValidarSupervisor()"
                                                value="Aceptar" style="cursor: hand" />
                                        </td>
                                        <td style="width: 15px">
                                        </td>
                                        <td align="left">
                                            <input type="button" id="btnCancelar" class="Boton" onclick="retornoValue('')" value="Cancelar"
                                                style="cursor: hand" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </tfoot>
                </table>
            </td>
        </tr>
    </table>
    </form>
    <input id="hidCausal" type="hidden" name="hidCausal" runat="server" />
</body>
</html>
