<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sisact_pop_seguros.aspx.cs" Inherits="Claro.SISACT.Web.Paginas.Venta.sisact_pop_seguros" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Detalle Protección Móvil</title>
    
    <link href="../../Estilos/General.css" rel="stylesheet" type="text/css" />
        <script src="../../Scripts/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="../../Scripts/security.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">

        function f_Cerrar() {
            window.close();
        }

    </script>
</head>
<body>
<form id="frmPrincipal" runat="server">
    <table border="0" cellspacing="1" cellpadding="0" width="100%">
        <tr>
            <td class="Header" height="18" align="left">
                &nbsp;Detalle/Resumen de Prima
            </td>
        </tr>
    </table>
    <table class="Contenido" border="0" cellspacing="1" cellpadding="0" width="100%">
        <tr>
            <td>
                <div>
                    <asp:GridView ID="gvSeguros" runat="server" Width="100%" AutoGenerateColumns="False" ShowHeader="True">
                        <HeaderStyle Wrap="False" HorizontalAlign="Center" CssClass="TablaTitulos" />
                        <RowStyle CssClass="TablaFilasGrid" HorizontalAlign="Center" />
                        <Columns>
                            <asp:BoundField DataField="EQUIPO" HeaderText="Equipo" />
                            <asp:BoundField DataField="PRIMA" HeaderText="Prima" />
                            <asp:BoundField DataField="DEDUCIBLE" HeaderText="Deducible" />
                        </Columns>
                    </asp:GridView>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <table id="tblBotonSalir" class="Contenido" width="100%" cellpadding="0" border="0">
                    <tr>
                        <td align="center">
                            <input class="Boton" type="button" id="btnCerrar" name="btnCancelar" onclick="f_Cerrar();"
                                style="width: 100px; cursor: hand; height: 19px" value="Cerrar" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
