<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sisact_pop_consulta3g.aspx.cs" Inherits="Claro.SISACT.Web.Paginas.frames.siact_pop_consulta3g" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Consulta Líneas 3G</title>
     <link href="../../Estilos/general.css" type="text/css" rel="stylesheet" />
        <script src="../../Scripts/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="../../Scripts/security.js" type="text/javascript"></script>
     <script language="javascript" type="text/javascript">
         function f_Salir() {
             window.opener.mostrarSiguientesTr();
             window.close();
         }
         </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table style="width:100%">
				<tr>
					<td class="Header">Líneas del cliente</td>
				</tr>
				<tr>
					<td class="Arial10B"><%=consMensajeLineas3G%></td>
				</tr>
				<tr>
					<td>
        <asp:GridView ID="dgLineas3g" runat="server" AutoGenerateColumns="False" 
             onrowdatabound="dgLineas3g_RowDataBound" Width="100%">
            <HeaderStyle Wrap="False" HorizontalAlign="Center" CssClass="TablaTitulos" />
            <RowStyle BackColor="#E9EBEE" HorizontalAlign="Center" CssClass="Arial10" />
            <AlternatingRowStyle BackColor="#DDDEE2" HorizontalAlign="Center" CssClass="Arial10" />
            <Columns>
                <asp:TemplateField HeaderText="Línea">
                    <ItemTemplate>
                        <asp:Literal ID="numlinea" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="planLinea" HeaderText="Plan" ReadOnly="True" />
                <asp:BoundField DataField="estadoLinea" HeaderText="Estado" ReadOnly="True" />
                <asp:BoundField DataField="tipoBloqueo" HeaderText="Tipo bloqueo" 
                    ReadOnly="True" />
            </Columns>
        </asp:GridView>
    </tr>
				<tr>
					<td align="right">
						<INPUT class="BotonOptm" id="btnAceptarLineas3G" onclick="f_Salir();" type="button" value="Aceptar"
								name="btnAceptarLineas3G">
					</td>
				</tr>
			</table>
			</div>
    </div>
    </form>
</body>
</html>
