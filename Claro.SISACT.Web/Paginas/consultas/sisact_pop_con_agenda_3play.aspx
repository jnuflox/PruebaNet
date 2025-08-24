<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sisact_pop_con_agenda_3play.aspx.cs"
    Inherits="Claro.SISACT.Web.Paginas.consultas.sisact_pop_con_agenda_3play" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Agenda 3Play</title>
    <link href="../../Estilos/general.css" type="text/css" rel="stylesheet" />
        <script language="javascript" type="text/javascript" src="../../Scripts/jquery-1.9.1.js"></script>
        <script language="javascript" type="text/javascript" src="../../Scripts/security.js"></script>
</head>
<body>
    <form id="frmPrincipal" runat="server">
    <table class="Contenido">
        <tr>
            <td>
                <table>
                    <tr>
                        <td class="Arial10B" style="width: 100px">
                            &nbsp; Nro SEC
                        </td>
                        <td class="Arial10B" style="width: 5px">
                            :
                        </td>
                        <td class="Arial10B" style="width: 100px">
                            <asp:TextBox ID="txtNroSec" runat="server" CssClass="clsInputDisable" Width="107px" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td class="Arial10B" style="width: 50px">
                            &nbsp;Nro Doc
                        </td>
                        <td class="Arial10B" style="width: 5px">
                            :
                        </td>
                        <td class="Arial10B" style="width: 100px">
                            <asp:TextBox ID="txtNroDoc" runat="server" CssClass="clsInputDisable" Width="107px" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td class="Arial10B" style="width: 150px">
                            &nbsp;Apellidos y Nombres
                        </td>
                        <td class="Arial10B" style="width: 5px">
                            :
                        </td>
                        <td class="Arial10B" style="width: 300px">
                            &nbsp;<asp:TextBox ID="txtNombre" runat="server" CssClass="clsInputDisable" Width="295px" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="Arial10B" style="width: 100px">
                            &nbsp;Contrato
                        </td>
                        <td class="Arial10B" style="width: 5px">
                            :
                        </td>
                        <td class="Arial10B" style="width: 100px">
                            <asp:TextBox ID="txtNroContrato" runat="server" CssClass="clsInputDisable" Width="107px" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td class="Arial10B" style="width: 50px">
                            &nbsp;Nro SOT
                        </td>
                        <td class="Arial10B" style="width: 5px">
                            :
                        </td>
                        <td class="Arial10B" style="width: 100px">
                            <asp:TextBox ID="txtNroSot" runat="server" CssClass="clsInputDisable" Width="107px" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td class="Arial10B" style="width: 150px">
                            &nbsp;Estado SGA
                        </td>
                        <td class="Arial10B" style="width: 5px">
                            :
                        </td>
                        <td class="Arial10B" style="width: 300px">
                            <asp:TextBox ID="txtEstadoSGA" runat="server" CssClass="clsInputDisable" Width="295px" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="Header" align="left" colspan="9" height="20">
                            PANEL DE PROGRAMACIÓN
                        </td>
                    </tr>
                    <tr>
                        <td class="Arial10B" style="width: 100px">
                            &nbsp;Fecha Inst.
                        </td>
                        <td class="Arial10B" style="width: 5px">
                            :
                        </td>
                        <td class="Arial10B" style="width: 100px">
                            <asp:TextBox ID="txtFechaInst" runat="server" CssClass="clsInputDisable" Width="107px" ReadOnly="true"></asp:TextBox>
                        </td>                        
                        <td class="Arial10B" style="width: 100px">
                            <asp:TextBox ID="txtHora" runat="server" CssClass="clsInputDisable" Width="80px" ReadOnly="true"></asp:TextBox>
                        </td>                        
                        
                        <td class="Arial10B" style="width: 50px" colspan="2">
                        </td>
                        <td class="Arial10B" style="width: 150px">
                            &nbsp;Contratista Instalación
                        </td>
                        <td class="Arial10B" style="width: 5px">
                            :
                        </td>
                        <td class="Arial10B" style="width: 300px">
                            <asp:TextBox ID="txtContratista" runat="server" CssClass="clsInputDisable" Width="295px" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center">
                <input class="Boton" id="btnCanselar" onmouseover="this.className='BotonResaltado';"
                    style="cursor: hand" onclick="window.close();" onmouseout="this.className='Boton';"
                    type="button" value="Cerrar" width="126" height="19">
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
