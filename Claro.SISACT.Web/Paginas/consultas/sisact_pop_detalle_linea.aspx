<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sisact_pop_detalle_linea.aspx.cs" Inherits="Claro.SISACT.Web.Paginas.consultas.sisact_pop_detalle_linea" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/tr/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Consulta Detalle Líneas</title>
    <link href="../../Estilos/general.css" type="text/css" rel="stylesheet" />
		<script language="javascript" type="text/javascript" src="../../Scripts/jquery-1.9.1.js"></script>
	    <script language="javascript" type="text/javascript" src="../../Scripts/security.js"></script>
    <script language="javascript" type="text/javascript">

        function mostrarDetalle(id) {
            if (!document.getElementById) return false;
            fila = document.getElementById(id);
            if (fila.style.display != "none")
                fila.style.display = "none";
            else
                fila.style.display = "";
        }
    </script>
</head>
<body>
    <form id="frmPrincipal" runat="server">
		<table cellspacing="2" cellpadding="0" width="100%" border="0">
			<tr>
				<td class="Header" align="left" height="20">&nbsp;Resumen</td>
			</tr>
			<tr>
				<td>
					<table id="tblResumen" class="Contenido" cellspacing="1" cellpadding="0" width="100%" border="0">
						<tr>
							<td class="Arial10B">&nbsp;Tipo y Nro Documento</td>
							<td><asp:textbox id="txtTipoDocumento" runat="server" CssClass="clsInputDisable"
									ReadOnly="True" Width="60px"></asp:textbox>&nbsp;</td>
							<td><asp:textbox id="txtNroDocumento" runat="server" CssClass="clsInputDisable"
									Width="100px"></asp:textbox></td>
							<td>&nbsp;</td>
							<td class="Arial10B">Nombres y apellidos/Razon Social</td>
							<td><asp:textbox id="txtNombre" runat="server" CssClass="clsInputDisable"
									ReadOnly="True" Width="300px"></asp:textbox></td>
						</tr>
						<tr id="trAnexoDocumento" runat="server">
							<td class="Arial10B">&nbsp;Tipo y Nro Documento Anexo</td>
							<td><asp:textbox id="txtTipoDocumentoAnexo" runat="server" CssClass="clsInputDisable"
									ReadOnly="True" Width="60px"></asp:textbox>&nbsp;</td>
							<td colspan="4"><asp:textbox id="txtNroDocumentoAnexo" runat="server" CssClass="clsInputDisable"
									ReadOnly="True" Width="100px"></asp:textbox></td>
						</tr>
						<tr>
							<td class="Arial10B">&nbsp;Cantidad de Planes/Soluciones</td>
							<td><asp:textbox id="txtCantidadPlan" runat="server" CssClass="clsInputDisable"
									ReadOnly="True" Width="60px"></asp:textbox></td>
							<td></td>
							<td>&nbsp;</td>
							<td class="Arial10B">Total Cargos Fijos (S/.)</td>
							<td><asp:textbox id="txtCargoFijo" runat="server" CssClass="clsInputDisable"
									ReadOnly="True" Width="60px"></asp:textbox></td>
						</tr>
						<tr>
							<td class="Arial10B">&nbsp;Deuda vencida (S/.)</td>
							<td colspan="2">
                                <asp:textbox id="txtDeudaVencida" runat="server" CssClass="clsInputDisable"
									ReadOnly="True" Width="60px"></asp:textbox></td>
							<td>&nbsp;</td>
							<td class="Arial10B">Deuda Castigada (S/.)</td>
							<td><asp:textbox id="txtDeudaCastigada" runat="server" CssClass="clsInputDisable"
									ReadOnly="True" Width="60px"></asp:textbox></td>
						</tr>
						<tr>
							<td class="Arial10B">&nbsp;Bloqueos</td>
							<td><asp:textbox id="txtBloqueo" runat="server" CssClass="clsInputDisable" ReadOnly="True" Width="60px"></asp:textbox></td>
							<td>&nbsp;</td>
							<td>&nbsp;</td>
							<td class="Arial10B">Suspensiones</td>
							<td><asp:textbox id="txtSuspension" runat="server" CssClass="clsInputDisable" 
                                    ReadOnly="True" Width="60px"></asp:textbox></td>
						</tr>
						<tr>
							<td class="Arial10B">&nbsp;Cantidad de Líneas Bloqueadas</td>
							<td><asp:textbox id="txtCantBloqueoMovil" runat="server" CssClass="clsInputDisable" ReadOnly="True"
									Width="60px"></asp:textbox></td>
							<td>&nbsp;</td>
							<td>&nbsp;</td>
							<td class="Arial10B">Cantidad de Líneas Suspendidas</td>
							<td><asp:textbox id="txtCantSuspMovil" runat="server" CssClass="clsInputDisable" ReadOnly="True"
									Width="60px"></asp:textbox></td>
						</tr>
						<tr id="trCantLineaMayorMenor" runat="server">
							<td class="Arial10B">&nbsp;Cantidad de Líneas Menor a
								<asp:label id="lblCantLineaMenor" runat="server"></asp:label>&nbsp;días</td>
							<td><asp:textbox id="txtCantLineaMenor" runat="server" CssClass="clsInputDisable" ReadOnly="True"
									Width="60px"></asp:textbox></td>
							<td>&nbsp;</td>
							<td>&nbsp;</td>
							<td class="Arial10B">Cantidad de Líneas Mayor a
								<asp:label id="lblCantLineaMayor" runat="server"></asp:label>&nbsp;días</td>
							<td><asp:textbox id="txtCantLineaMayor" runat="server" CssClass="clsInputDisable" ReadOnly="True"
									Width="60px"></asp:textbox></td>
						</tr>
					</table>
				</td>
			</tr>
			<tr>
				<td class="Header" align="left" height="20">&nbsp;Detalle BSCS</td>
			</tr>
            <tr>
                <td align="left">
                    <div class="clsGridRow">
                        <asp:GridView ID="gvListaBSCS" runat="server" Width="100%" AutoGenerateColumns="False"
                            OnRowDataBound="gvListaBSCS_RowDataBound" ShowHeaderWhenEmpty="true">
                            <HeaderStyle Wrap="False" HorizontalAlign="Center" CssClass="TablaTitulos" />
                            <RowStyle BackColor="#E9EBEE" HorizontalAlign="Center" CssClass="Arial10" />
                            <AlternatingRowStyle BackColor="#DDDEE2" HorizontalAlign="Center" CssClass="Arial10" />
                            <Columns>
                                <asp:BoundField DataField="CODIGO_CUENTA" HeaderText="C&#243;digo Cuenta"></asp:BoundField>
                                <asp:BoundField DataField="ESTADO_CUENTA" HeaderText="Estado de Cuenta"></asp:BoundField>
                                <asp:BoundField DataField="CUSTOMER_ID" HeaderText="Customer ID"></asp:BoundField>
                                <asp:BoundField DataField="CENTRAL_RIESGO" HeaderText="Central de Riesgo"></asp:BoundField>
                                <asp:BoundField DataField="CF" HeaderText="Cargo Fijo"></asp:BoundField>
                                <asp:BoundField DataField="PROMEDIO_FACTURADO" HeaderText="Promedio Facturado"></asp:BoundField>
                                <asp:BoundField DataField="MONTO_NO_FACTURADO" HeaderText="Monto aún NO Facturado">
                                </asp:BoundField>
                                <asp:BoundField DataField="MONTO_VENCIDO" HeaderText="Monto Vencido" ItemStyle-Width="35px"></asp:BoundField>
                                <asp:BoundField DataField="MONTO_CASTIGADO" HeaderText="Monto Castigado" ItemStyle-Width="35px"></asp:BoundField>
                                <asp:BoundField DataField="DEUDA_REINTEGRO" HeaderText="Deuda Reintegro de Equipo">
                                </asp:BoundField>
                                <asp:BoundField DataField="CANTIDAD_SERVICIOS" HeaderText="Cantidad de Planes/Servicios" ItemStyle-Width="50px">
                                </asp:BoundField>
                                <asp:BoundField DataField="CANTIDAD_BLOQUEOS" HeaderText="Cantidad de Bloqueados" ItemStyle-Width="50px">
                                </asp:BoundField>
                                <asp:BoundField DataField="CANTIDAD_SUSPENDIDOS" HeaderText="Cantidad de Suspendidos" ItemStyle-Width="50px">
                                </asp:BoundField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Image ID="btnMostrarDetalle" ImageUrl="~/Imagenes/ico_lupa.gif" Style="cursor: pointer;"
                                            ToolTip="Ver Detalle Linea" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" FooterStyle-CssClass="hiddencol">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCodCuenta" Text='<%# DataBinder.Eval(Container.DataItem, "CODIGO_CUENTA") %>'
                                            runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </td>
            </tr>
            <tr>
                <td class="Header" align="left" height="20">&nbsp;Detalle SGA</td>
            </tr>
            <tr>
                <td align="left">
                    <div class="clsGridRow">
                        <asp:GridView ID="gvListaSGA" runat="server" Width="100%" AutoGenerateColumns="False"
                            OnRowDataBound="gvListaSGA_RowDataBound" ShowHeaderWhenEmpty="true">
                            <HeaderStyle Wrap="False" HorizontalAlign="Center" CssClass="TablaTitulos" />
                            <RowStyle BackColor="#E9EBEE" HorizontalAlign="Center" CssClass="Arial10" />
                            <AlternatingRowStyle BackColor="#DDDEE2" HorizontalAlign="Center" CssClass="Arial10" />
                            <Columns>
                                <asp:BoundField DataField="CODIGO_CUENTA" HeaderText="C&#243;digo Cuenta">
                                    <ItemStyle Width="100px"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="ESTADO_CUENTA" HeaderText="Estado de Cuenta"></asp:BoundField>
                                <asp:BoundField DataField="CUSTOMER_ID" HeaderText="Customer ID"></asp:BoundField>
                                <asp:BoundField DataField="CENTRAL_RIESGO" HeaderText="Central de Riesgo"></asp:BoundField>
                                <asp:BoundField DataField="CF" HeaderText="Cargo Fijo"></asp:BoundField>
                                <asp:BoundField DataField="PROMEDIO_FACTURADO" HeaderText="Promedio Facturado"></asp:BoundField>
                                <asp:BoundField DataField="MONTO_NO_FACTURADO" HeaderText="Monto aún NO Facturado">
                                </asp:BoundField>
                                <asp:BoundField DataField="MONTO_VENCIDO" HeaderText="Monto Vencido" ItemStyle-Width="35px"></asp:BoundField>
                                <asp:BoundField DataField="MONTO_CASTIGADO" HeaderText="Monto Castigado" ItemStyle-Width="35px"></asp:BoundField>
                                <asp:BoundField DataField="DEUDA_REINTEGRO" HeaderText="Deuda Reintegro de Equipo">
                                </asp:BoundField>
                                <asp:BoundField DataField="CANTIDAD_SERVICIOS" HeaderText="Cantidad de Planes/ Servicios" ItemStyle-Width="50px">
                                </asp:BoundField>
                                <asp:BoundField DataField="CANTIDAD_BLOQUEOS" HeaderText="Cantidad de Bloqueados" ItemStyle-Width="50px">
                                </asp:BoundField>
                                <asp:BoundField DataField="CANTIDAD_SUSPENDIDOS" HeaderText="Cantidad de Suspendidos" ItemStyle-Width="50px">
                                </asp:BoundField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Image ID="btnMostrarDetalle" ImageUrl="~/Imagenes/ico_lupa.gif" Style="cursor: pointer;"
                                            ToolTip="Ver Detalle Linea" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" FooterStyle-CssClass="hiddencol">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCodCuenta" Text='<%# DataBinder.Eval(Container.DataItem, "CODIGO_CUENTA") %>'
                                            runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </td>
            </tr>
            <tr>
				<td class="Header" align="left" height="20">&nbsp;Detalle Venta SISACT</td>
			</tr>
			<tr>
				<td align="left">
                    <div class="clsGridRow">
<!--INI PROY 26963 -  PROMFACT -->
                        <asp:GridView ID="gvListaPendienteSISACT" runat="server" Width="100%" AutoGenerateColumns="False"
                            OnRowDataBound="gvListaPendienteSISACT_RowDataBound" ShowHeaderWhenEmpty="true">
                            <HeaderStyle Wrap="False" HorizontalAlign="Center" CssClass="TablaTitulos" />
                            <RowStyle BackColor="#E9EBEE" HorizontalAlign="Center" CssClass="Arial10" />
                            <AlternatingRowStyle BackColor="#DDDEE2" HorizontalAlign="Center" CssClass="Arial10" />
                            <Columns>
                                <asp:BoundField DataField="CODIGO_CUENTA" HeaderText="C&#243;digo Cuenta"></asp:BoundField>
                                <asp:BoundField DataField="ESTADO_CUENTA" HeaderText="Estado de Cuenta"></asp:BoundField>
                                <asp:BoundField DataField="CUSTOMER_ID" HeaderText="Customer ID"></asp:BoundField>
                                <asp:BoundField DataField="CENTRAL_RIESGO" HeaderText="Central de Riesgo"></asp:BoundField>
                                <asp:BoundField DataField="CF" HeaderText="Cargo Fijo"></asp:BoundField>
                                <asp:BoundField DataField="PROMEDIO_FACTURADO" HeaderText="Promedio Facturado"></asp:BoundField>
                                <asp:BoundField DataField="MONTO_NO_FACTURADO" HeaderText="Monto aún NO Facturado">
                                </asp:BoundField>
                                <asp:BoundField DataField="MONTO_VENCIDO" HeaderText="Monto Vencido" ItemStyle-Width="35px"></asp:BoundField>
                                <asp:BoundField DataField="MONTO_CASTIGADO" HeaderText="Monto Castigado" ItemStyle-Width="35px"></asp:BoundField>
                                <asp:BoundField DataField="DEUDA_REINTEGRO" HeaderText="Deuda Reintegro de Equipo">
                                </asp:BoundField>
                                <asp:BoundField DataField="CANTIDAD_SERVICIOS" HeaderText="Cantidad de Planes/Servicios" ItemStyle-Width="50px">
                                </asp:BoundField>
                                <asp:BoundField DataField="CANTIDAD_BLOQUEOS" HeaderText="Cantidad de Bloqueados" ItemStyle-Width="50px">
                                </asp:BoundField>
                                <asp:BoundField DataField="CANTIDAD_SUSPENDIDOS" HeaderText="Cantidad de Suspendidos" ItemStyle-Width="50px">
                                </asp:BoundField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Image ID="btnMostrarDetalle" ImageUrl="~/Imagenes/ico_lupa.gif" Style="cursor: pointer;"
                                            ToolTip="Ver Detalle Linea" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" FooterStyle-CssClass="hiddencol">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCodCuenta" Text='<%# DataBinder.Eval(Container.DataItem, "CODIGO_CUENTA") %>'
                                            runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </td>
            </tr>
            <!--INI PROY 26963 - PROMFACT-->
            <tr>
				<!--<td class="Header" align="left" height="20">&nbsp;Detalle Aprobada SISACT</td>--> 
                  <!--SC2-->
                <td class="Header" align="left" height="20">&nbsp;Detalle SEC Aprobada Portabilidad</td>
                
			</tr>
			<tr>
				<td align="left">
                    <div class="clsGridRow">
                        <asp:GridView ID="gvListaSISACT" runat="server" Width="100%" AutoGenerateColumns="False"
                            OnRowDataBound="gvListaSISACT_RowDataBound" ShowHeaderWhenEmpty="true">
                            <HeaderStyle Wrap="False" HorizontalAlign="Center" CssClass="TablaTitulos" />
                            <RowStyle BackColor="#E9EBEE" HorizontalAlign="Center" CssClass="Arial10" />
                            <AlternatingRowStyle BackColor="#DDDEE2" HorizontalAlign="Center" CssClass="Arial10" />
                            <Columns>
                                <asp:BoundField DataField="CODIGO_CUENTA" HeaderText="C&#243;digo Cuenta"></asp:BoundField>
                                <asp:BoundField DataField="ESTADO_CUENTA" HeaderText="Estado de Cuenta"></asp:BoundField>
                                <asp:BoundField DataField="CUSTOMER_ID" HeaderText="Customer ID"></asp:BoundField>
                                <asp:BoundField DataField="CENTRAL_RIESGO" HeaderText="Central de Riesgo"></asp:BoundField>
                                <asp:BoundField DataField="CF" HeaderText="Cargo Fijo"></asp:BoundField>
                                <asp:BoundField DataField="PROMEDIO_FACTURADO" HeaderText="Promedio Facturado"></asp:BoundField>
                                <asp:BoundField DataField="MONTO_NO_FACTURADO" HeaderText="Monto aún NO Facturado">
                                </asp:BoundField>
                                <asp:BoundField DataField="MONTO_VENCIDO" HeaderText="Monto Vencido" ItemStyle-Width="35px"></asp:BoundField>
                                <asp:BoundField DataField="MONTO_CASTIGADO" HeaderText="Monto Castigado" ItemStyle-Width="35px"></asp:BoundField>
                                <asp:BoundField DataField="DEUDA_REINTEGRO" HeaderText="Deuda Reintegro de Equipo">
                                </asp:BoundField>
                                <asp:BoundField DataField="CANTIDAD_SERVICIOS" HeaderText="Cantidad de Planes/Servicios" ItemStyle-Width="50px">
                                </asp:BoundField>
                                <asp:BoundField DataField="CANTIDAD_BLOQUEOS" HeaderText="Cantidad de Bloqueados" ItemStyle-Width="50px">
                                </asp:BoundField>
                                <asp:BoundField DataField="CANTIDAD_SUSPENDIDOS" HeaderText="Cantidad de Suspendidos" ItemStyle-Width="50px">
                                </asp:BoundField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Image ID="btnMostrarDetalle" ImageUrl="~/Imagenes/ico_lupa.gif" Style="cursor: pointer;"
                                            ToolTip="Ver Detalle Linea" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" FooterStyle-CssClass="hiddencol">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCodCuenta" Text='<%# DataBinder.Eval(Container.DataItem, "CODIGO_CUENTA") %>'
                                            runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
				</td>
			</tr>
            <!--FIN PROY 26963 - PROMFACT -->
            <!--PROY-140743 IDEA141192 VICTOR CANCHICA INI -->
            <tr>
                <td class="Header" align="left" height="20">&nbsp;Detalle Accesorios</td>
            </tr>
            <tr>
				<td align="left">
                    <div class="clsGridRow">
                        <asp:GridView ID="gvDetalleAccesorios" runat="server" Width="100%" AutoGenerateColumns="False"
                            OnRowDataBound="gvDetalleAccesorios_RowDataBound" ShowHeaderWhenEmpty="true">
                            <HeaderStyle Wrap="False" HorizontalAlign="Center" CssClass="TablaTitulos" />
                            <RowStyle BackColor="#E9EBEE" HorizontalAlign="Center" CssClass="Arial10" />
                            <AlternatingRowStyle BackColor="#DDDEE2" HorizontalAlign="Center" CssClass="Arial10" />
                            <Columns>
                                <asp:BoundField DataField="CODIGO_CUENTA" HeaderText="C&#243;digo Cuenta"></asp:BoundField>
                                <asp:BoundField DataField="ESTADO_CUENTA" HeaderText="Estado de Cuenta"></asp:BoundField>
                                <asp:BoundField DataField="CUSTOMER_ID" HeaderText="Customer ID"></asp:BoundField>
                                <asp:BoundField DataField="CENTRAL_RIESGO" HeaderText="Central de Riesgo"></asp:BoundField>
                                <asp:BoundField DataField="CF" HeaderText="Cargo Fijo"></asp:BoundField>
                                <asp:BoundField DataField="PROMEDIO_FACTURADO" HeaderText="Promedio Facturado"></asp:BoundField>
                                <asp:BoundField DataField="MONTO_NO_FACTURADO" HeaderText="Monto aún NO Facturado">
                                </asp:BoundField>
                                <asp:BoundField DataField="MONTO_VENCIDO" HeaderText="Monto Vencido" ItemStyle-Width="35px"></asp:BoundField>
                                <asp:BoundField DataField="MONTO_CASTIGADO" HeaderText="Monto Castigado" ItemStyle-Width="35px"></asp:BoundField>
                                <asp:BoundField DataField="DEUDA_REINTEGRO" HeaderText="Deuda Reintegro de Equipo">
                                </asp:BoundField>
                                <asp:BoundField DataField="CANTIDAD_SERVICIOS" HeaderText="Cantidad de Planes/Servicios" ItemStyle-Width="50px">
                                </asp:BoundField>
                                <asp:BoundField DataField="CANTIDAD_BLOQUEOS" HeaderText="Cantidad de Bloqueados" ItemStyle-Width="50px">
                                </asp:BoundField>
                                <asp:BoundField DataField="CANTIDAD_SUSPENDIDOS" HeaderText="Cantidad de Suspendidos" ItemStyle-Width="50px">
                                </asp:BoundField>
                                <asp:TemplateField>
                                    <%--<ItemTemplate>
                                        <asp:Image ID="btnMostrarDetalle" ImageUrl="~/Imagenes/ico_lupa.gif" Style="cursor: pointer;"
                                            ToolTip="Ver Detalle Linea" runat="server" />
                                    </ItemTemplate>--%>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" FooterStyle-CssClass="hiddencol">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCodCuenta" Text='<%# DataBinder.Eval(Container.DataItem, "CODIGO_CUENTA") %>'
                                            runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
				</td>
			</tr>
             <!--PROY-140743 IDEA141192 VICTOR CANCHICA FIN-->
            <tr>
				<td class="Header" align="left" height="20">&nbsp;Detalle de Lineas a Recuperar</td>
			</tr>
			<tr>
				<td align="left">
                    <div class="clsGridRow">
                        <asp:GridView ID="gvListaPrepago" runat="server" Width="50%" AutoGenerateColumns="False"
                            ShowHeaderWhenEmpty="true">
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
                    </div>
                </td>
            </tr>
            <tr>
                <td align="center" height="75">
                    <input class="Boton" id="btnCancelar" style="width: 100px; cursor: hand; height: 19px"
                        type="button" value="Cerrar" onclick="window.close();" />
                </td>
            </tr>
		</table>
    </form>
</body>
</html>
