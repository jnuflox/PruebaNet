<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sisact_pop_detalle_linea_prepago.aspx.cs"
    Inherits="Claro.SISACT.Web.Paginas.consultas.sisact_pop_detalle_linea_prepago" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Consulta Detalle Líneas Prepago</title>
    <link href="../../Estilos/general.css" type="text/css" rel="stylesheet" />
        <script language="javascript" type="text/javascript" src="../../Scripts/jquery-1.9.1.js"></script>
        <script language="javascript" type="text/javascript" src="../../Scripts/security.js"></script>
    <style type="text/css">
        .error
        {
            margin-top: 5px;
            font-family: Verdana;
            margin-bottom: 5px;
            float: left;
            color: red;
            font-size: 10px;
        }
        .success
        {
            text-align: justify;
            margin-top: 5px;
            font-family: Verdana;
            margin-bottom: 5px;
            float: left;
            color: #003399;
            font-size: 10px;
        }
        .content_tabla
        {
            overflow-y: auto;
            overflow-x: hidden;
            max-height: 220px;
        }
    </style>
</head>
<body>
    <form id="frmPrincipal" runat="server">
    <table cellspacing="2" cellpadding="0" width="100%" border="0">
        <tr>
            <td class="Header" align="left" height="20">
                &nbsp;Lineas del Cliente
            </td>
        </tr>
        <tr>
            <td>
                <label id="labelMsgErrorListaLineas" runat="server" class="error">
                </label>
                <p id="labelMsgSuccessListaLineas" runat="server" class="success">
                </p>
            </td>
        </tr>
        <tr>
            <td height="1">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="left" class="content_tabla">
                <asp:GridView ID="gvListaPrepago" runat="server" Width="100%" BorderWidth="1px" AutoGenerateColumns="False">
                    <HeaderStyle Wrap="False" HorizontalAlign="Center" CssClass="TablaTitulos" />
                    <RowStyle BackColor="#E9EBEE" HorizontalAlign="Center" CssClass="Arial10" />
                    <AlternatingRowStyle BackColor="#DDDEE2" HorizontalAlign="Center" CssClass="Arial10" />
                    <Columns>
                        <asp:BoundField DataField="LINEA_PREPAGO" HeaderText="Linea Prepago"></asp:BoundField>
                        <asp:BoundField DataField="PLAN" HeaderText="Plan"></asp:BoundField>
                        <asp:BoundField DataField="ESTADO" HeaderText="Estado"></asp:BoundField>
                        <asp:BoundField DataField="TIPO_BLOQUEO" HeaderText="Tipo de bloqueo"></asp:BoundField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td align="right" height="30">
                <input id="btnAceptar" class="Boton" style="width: 100px; height: 19px" type="button"
                    value="Aceptar" onclick="window.close();" />
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
