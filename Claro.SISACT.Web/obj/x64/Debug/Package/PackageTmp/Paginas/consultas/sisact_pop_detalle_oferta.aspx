<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sisact_pop_detalle_oferta.aspx.cs"
    Inherits="Claro.SISACT.Web.Paginas.consultas.sisact_pop_detalle_oferta" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Oferta Comercial</title>
    <link href="../../Estilos/general.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" language='javascript' src='../../Scripts/funciones_sec.js'></script>
    <script type="text/javascript" language="javascript">
        function pintarRows() {
            var tabla = document.getElementById('dgLista');
            var rows = tabla.getElementsByTagName('tr');
            var cadItems = document.getElementById('hidString').value.split(';');

            for (i = 0; i < rows.length; i++)//recorriendo las filas
            {
                var columns = rows[i].getElementsByTagName('td');
                if (cadItems[i] == '1') {
                    rows[i].ClassName = "TablaTitulos";
                    for (j = 0; j < columns.length; j++)//recorremos las columnas de cada fila
                    {
                        columns[j].className = 'TablaTitulos';
                    }
                }
            }
        }
    </script>
</head>
<body onload="pintarRows();" style="margin: 0px;">
    <form id="frmPrincipal" runat="server">
    <table>
        <tr>
            <td colspan="3">
                <table>
                    <tr>
                        <td class="Arial10B" width="40">
                            &nbsp;Campaña
                        </td>
                        <td class="Arial10B" width="5">
                            :
                        </td>
                        <td>
                            <asp:Label ID="txtCampana" CssClass="Arial10B" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="Arial10B" width="10">
                            &nbsp;Paquete
                        </td>
                        <td class="Arial10B" width="5">
                            :
                        </td>
                        <td>
                            <asp:Label ID="txtPlan" runat="server" CssClass="Arial10B"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="Arial10B" align="center" valign="top">
                <table cellspacing="1" cellpadding="0" border="0" width="100%">
                    <tr>
                        <td width="100%" class="TablaTitulos" align="center">
                            SERVICIOS
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <img height="1" alt="0" src="../../Imagenes/spacer.gif" width="100%" border="0" />
                        </td>
                    </tr>
                </table>
                <table cellspacing="0" cellpadding="0" border="0">
                    <tr>
                        <td>
                            <asp:DataGrid ID="dgCabLista" ShowHeader="True" AutoGenerateColumns="False" 
                                runat="server">
                                <Columns>
                                    <asp:TemplateColumn HeaderText="N°" HeaderStyle-Width="25px">
                                        <HeaderStyle Wrap="False" HorizontalAlign="Center" CssClass="TablaTitulos"></HeaderStyle>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Grupo" HeaderStyle-Width="150px">
                                        <HeaderStyle Wrap="False" HorizontalAlign="Center" CssClass="TablaTitulos"></HeaderStyle>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Servicio" HeaderStyle-Width="150px">
                                        <HeaderStyle Wrap="False" HorizontalAlign="Center" CssClass="TablaTitulos"></HeaderStyle>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Cargo Fijo" HeaderStyle-Width="60px">
                                        <HeaderStyle Wrap="False" HorizontalAlign="Center" CssClass="TablaTitulos"></HeaderStyle>
                                    </asp:TemplateColumn>
                                </Columns>
                            </asp:DataGrid>
                        </td>
                    </tr>
                </table>
                <table cellspacing="0" cellpadding="0" border="0">
                    <tr>
                        <td>
                            <asp:DataGrid ID="dgLista" runat="server" ShowHeader="False" 
                                AutoGenerateColumns="FALSE" onitemdatabound="dgLista_ItemDataBound">
                                <ItemStyle HorizontalAlign="Center" CssClass="Arial10B" BackColor="#E9EBEE"></ItemStyle>
                                <AlternatingItemStyle HorizontalAlign="Center" CssClass="Arial10B" BackColor="#dddee2">
                                </AlternatingItemStyle>
                                <SelectedItemStyle HorizontalAlign="Center" CssClass="Arial10B" BackColor="#ffffff">
                                </SelectedItemStyle>
                                <Columns>
                                    <asp:BoundColumn>
                                        <ItemStyle Wrap="False" HorizontalAlign="Center" CssClass="Arial10B" Width="25px">
                                        </ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="GRUPODESCRIPCION">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" CssClass="Arial10B" Width="150px">
                                        </ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="SERVICIO">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" CssClass="Arial10B" Width="150px">
                                        </ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="PRECIO">
                                        <ItemStyle Wrap="False" HorizontalAlign="Center" CssClass="Arial10B" Width="60px">
                                        </ItemStyle>
                                    </asp:BoundColumn>
                                </Columns>
                            </asp:DataGrid>
                        </td>
                    </tr>
                </table>
                <br />
                Cargo Fijo Mensual: S/.<asp:Label ID="lblCargoFijoMensual" runat="server" CssClass="Arial10B"></asp:Label>
            </td>
            <td>
                &nbsp;
            </td>
            <td align="center" valign="top">
                <table cellspacing="1" cellpadding="0" border="0" width="100%">
                    <tr>
                        <td width="100%" class="TablaTitulos" align="center">
                            <asp:Label ID="lblTitulo" runat="server" Text="TITULO"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <img height="1" alt="0" src="../../Imagenes/spacer.gif" width="100%" border="0" />
                        </td>
                    </tr>
                </table>
                <table cellspacing="0" cellpadding="0" border="0">
                    <tr>
                        <td>
                            <asp:DataGrid ID="dgCabListaEquipo" ShowHeader="True" AutoGenerateColumns="False" runat="server">
                                <Columns>
                                    <asp:TemplateColumn HeaderText="N°" HeaderStyle-Width="25px">
                                        <HeaderStyle Wrap="False" HorizontalAlign="Center" CssClass="TablaTitulos"></HeaderStyle>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Grupo" HeaderStyle-Width="100px">
                                        <HeaderStyle Wrap="False" HorizontalAlign="Center" CssClass="TablaTitulos"></HeaderStyle>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Equipo" HeaderStyle-Width="200px">
                                        <HeaderStyle Wrap="False" HorizontalAlign="Center" CssClass="TablaTitulos"></HeaderStyle>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Cargo Fijo" HeaderStyle-Width="60px">
                                        <HeaderStyle Wrap="False" HorizontalAlign="Center" CssClass="TablaTitulos"></HeaderStyle>
                                    </asp:TemplateColumn>
                                </Columns>
                            </asp:DataGrid>
                        </td>
                    </tr>
                </table>
                <table cellspacing="0" cellpadding="0" border="0">
                    <tr>
                        <td>
                            <asp:DataGrid ID="dgListaEquipo" runat="server" ShowHeader="False" 
                                AutoGenerateColumns="FALSE" onitemdatabound="dgListaEquipo_ItemDataBound">
                                <ItemStyle HorizontalAlign="Center" CssClass="Arial10B" BackColor="#E9EBEE"></ItemStyle>
                                <AlternatingItemStyle HorizontalAlign="Center" CssClass="Arial10B" BackColor="#dddee2">
                                </AlternatingItemStyle>
                                <SelectedItemStyle HorizontalAlign="Center" CssClass="Arial10B" BackColor="#ffffff">
                                </SelectedItemStyle>
                                <Columns>
                                    <asp:BoundColumn>
                                        <ItemStyle Wrap="False" HorizontalAlign="Center" CssClass="Arial10B" Width="25px">
                                        </ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="AGRUPA">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" CssClass="Arial10B" Width="100px">
                                        </ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="SERVICIO">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" CssClass="Arial10B" Width="200px">
                                        </ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="PRECIO">
                                        <ItemStyle Wrap="False" HorizontalAlign="Center" CssClass="Arial10B" Width="60px">
                                        </ItemStyle>
                                    </asp:BoundColumn>
                                </Columns>
                            </asp:DataGrid>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr style="height:25">
            <td align="center" colspan="3">
                <input class="Boton" id="btnCanselar" onmouseover="this.className='BotonResaltado';"
                    style="cursor: hand; width:126; height:19" onclick="window.close();" onmouseout="this.className='Boton';"
                    type="button" value="Cerrar" />
            </td>
        </tr>
    </table>
    <input id="hidString" type="hidden" name="hidString" runat="server" />
    </form>
</body>
</html>
