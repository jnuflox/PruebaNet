<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sisact_pop_motivo_evaluar_consumer.aspx.cs"
    Inherits="Claro.SISACT.Web.Paginas.consultas.sisact_motivo_evaluar_consumer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../Estilos/general.css" type="text/css" rel="stylesheet" />
    <script language="javascript" src='<%= ResolveClientUrl("~/Scripts/jquery-1.9.1.js") %>'
        type="text/javascript"></script>
    <script language="javascript" type="text/javascript">

        $(document).ready(function () {

            $("#btnGrabar").click(function () {
                rechazarSEC();
            });

            $("#btnCancelar").click(function () {
                liberarSEC();
            });

        });

        function liberarSEC() {
            PageMethods.liberarSEC($('#txtNroSEC').val(), callbacks.liberarSEC);
        }

        function rechazarSEC() {
            PageMethods.rechazarSEC($('#txtNroSEC').val(), $('#hidUsuarioRed').val(), $('#txtComentario').val(), $('#hidTiempoInicio').val(), callbacks.rechazarSEC);
        }

        var callbacks = {

            rechazarSEC: function (objResponse) {
                if (objResponse.Boleano) {
                    alert("Proceso culminado satisfactoriamente.");
                    window.opener.document.getElementById('btnConsultar').click();
                    window.close();
                }
                else {
                    liberarSEC();
                }
                window.opener.formatoPool();
            },

            liberarSEC: function (objResponse) {
                if (objResponse.Boleano) {
                    window.close();
                }
                window.opener.formatoPool();
            }
        }

    </script>
</head>
<body>
    <form id="frmPrincipal" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    <table cellspacing="2" cellpadding="0" width="100%" border="0">
        <tr>
            <td class="Header" align="left" height="20">
                &nbsp;Información del motivo del Rechazo de la Solicitud
            </td>
        </tr>
        <tr>
            <td>
                <table class="Contenido" cellspacing="2" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="Arial10B" width="100">
                            &nbsp;N° Evaluación:
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtNroSEC" runat="server" CssClass="clsInputDisable" Width="85px"
                                ReadOnly="True" Font-Size="X-Small"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="Arial10B" width="100">
                            &nbsp;Nro DNI::
                        </td>
                        <td width="100">
                            <asp:TextBox ID="txtNroDocumento" runat="server" CssClass="clsInputDisable" Width="85px"
                                ReadOnly="True"></asp:TextBox>
                        </td>
                        <td class="Arial10B" width="140">
                            Nombres y Apellidos:
                        </td>
                        <td>
                            <asp:TextBox ID="txtNombres" runat="server" CssClass="clsInputDisable" Width="280px"
                                ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="Arial10B" width="100">
                            &nbsp;Comentario:
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtComentario" runat="server" Width="522px" CssClass="inputTextArea"
                                TextMode="MultiLine" Columns="70" Height="56px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4">
                            <input class="Boton" id="btnGrabar" onmouseover="this.className='BotonResaltado';"
                                style="width: 126px; cursor: hand; height: 19px" onmouseout="this.className='Boton';"
                                type="button" value="Grabar" name="btnGrabar" />
                            <input class="Boton" id="btnCancelar" onmouseover="this.className='BotonResaltado';"
                                style="width: 126px; cursor: hand; height: 19px" onmouseout="this.className='Boton';"
                                type="button" value="Cancelar" name="btnCancelar" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <input id="hidTiempoInicio" type="hidden" runat="server" />
    <input id="hidUsuarioRed" type="hidden" runat="server" />
    </form>
</body>
</html>
