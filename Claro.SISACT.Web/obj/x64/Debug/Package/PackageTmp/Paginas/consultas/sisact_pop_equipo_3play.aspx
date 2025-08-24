<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sisact_pop_equipo_3play.aspx.cs" Inherits="Claro.SISACT.Web.Paginas.consultas.sisact_pop_equipo_3play" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Oferta Comercial</title>
    <link href="../../Estilos/general.css" type="text/css" rel="stylesheet">
</head>
<body style="margin: 0px;">
    <form id="frmPrincipal" runat="server">
			<table>
				<tr>
					<td colspan="3">
						<table>
							<tr>
								<td class="Arial10B" width="100%">&nbsp;Resumen de Equipos en Venta y Alquiler</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td align="center" valign="top">
						<table height="20" cellSpacing="0" cellPadding="0" border="0" width="100%">
							<tr>
								<td width="100%" class="TablaTitulos" align="center">ALQUILER DE EQUIPOS 3 PLAY</td>
							</tr>
						</table>
						<br>
						<table height="25" cellSpacing="0" cellPadding="0" border="0">
							<tr>
								<td>
									<asp:datagrid id="dgCabListaEquipo" ShowHeader="True" AutoGenerateColumns="False" height="20px"
										Runat="server">
										<Columns>
											<asp:TemplateColumn HeaderText="N°" HeaderStyle-Width="25px">
												<HeaderStyle Wrap="False" HorizontalAlign="Center" CssClass="TablaTitulos"></HeaderStyle>
											</asp:TemplateColumn>
											<asp:TemplateColumn HeaderText="Equipo" HeaderStyle-Width="150px">
												<headerstyle Wrap="False" HorizontalAlign="Center" CssClass="TablaTitulos"></headerstyle>
											</asp:TemplateColumn>
										</Columns>
									</asp:datagrid>
								</td>
							</tr>
						</table>
						<table cellSpacing="0" cellPadding="0" border="0">
							<tr>
								<td>
									<asp:datagrid id="dgListaEquipo" runat="server" ShowHeader="False" 
                                        AutoGenerateColumns="FALSE" onitemdatabound="dgListaEquipo_ItemDataBound">
										<ItemStyle HorizontalAlign="Center" CssClass="Arial10B" backcolor="#E9EBEE"></ItemStyle>
										<AlternatingItemStyle HorizontalAlign="Center" CssClass="Arial10B" backcolor="#dddee2"></AlternatingItemStyle>
										<SelectedItemStyle HorizontalAlign="Center" CssClass="Arial10B" backcolor="#ffffff"></SelectedItemStyle>
										<Columns>
											<asp:BoundColumn>
												<ItemStyle Wrap="False" HorizontalAlign="Center" CssClass="Arial10B" Width="25px"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="SERVICIO">
												<ItemStyle Wrap="False" HorizontalAlign="Left" CssClass="Arial10B" Width="150px"></ItemStyle>
											</asp:BoundColumn>
										</Columns>
									</asp:datagrid>
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr height="25">
					<td align="center" colSpan="3"><input class="Boton" id="btnCanselar" onmouseover="this.className='BotonResaltado';" style="CURSOR: hand"
							onclick="window.close();" onmouseout="this.className='Boton';" type="button" value="Cerrar" Width="126" Height="19">
					</td>
				</tr>
			</table>
			<INPUT id="hidString" type="hidden" name="hidString" runat="server">
    </form>
</body>
</html>
