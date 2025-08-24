<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sisact_edicion_garantia.aspx.cs"
    Inherits="Claro.SISACT.Web.Paginas.evaluacion_cons.sisact_edicion_garantia" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>SISACT Edición DG</title>
    <link href="../../Estilos/general.css" type="text/css" rel="stylesheet" />
    <script language="javascript" type="text/javascript" src='<%= ResolveClientUrl("~/Scripts/jquery-1.9.1.js") %>'></script>
    <script src="../../Scripts/funciones_sec.js" type="text/javascript"></script>
    <script src="../../Scripts/funciones_creditos.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">

        var constTipoProductoDTH = '<%= ConfigurationManager.AppSettings["constTipoProductoDTH"] %>';

        $(document).ready(function () {
            var width = new Array();
            var table;
            var nroFila = getValue('hidContFila');

            if (nroFila > 0) {

                if (getValue('hidFlgSecRechazada') != "S") {
                    table = $("table[id*=gvResultadoCredito2]");
                } else {
                    table = $("table[id*=gvResultadoCredito3]");
                }

                var alto = 0;
                if (nroFila >= 3) {
                    alto = 72;
                }
                else {
                    alto = 24 * nroFila;
                }

                table.find("th").each(function (i) {
                    width[i] = $(this).width();
                });
                //headerRow = table.find("tr:first");
                headerRow = table.find("tr:lt(2)");
                headerRow.find("th").each(function (i) {
                    $(this).width(width[i]);
                });
                var header = table.clone();
                header.empty();
                header.append(headerRow);
                header.css("width", width);
                $("#container").before(header);
                var footer = table.clone();
                footer.empty();
                footer.append(table.find("tr:last"));
                table.find("tr:first td").each(function (i) {
                    $(this).width(width[i]);
                });
                footer.find("td").each(function (i) {
                    $(this).width(width[i]);
                });
                $("#container").height(alto);
                $("#container").width(table.width() + 20);
                $("#container").append(table);
                $("#container").after(footer);
            }
        });

        $(document).ready(function () {

            $("#btnBuscar").click(function () {
                buscar();
            });

            $("#btnGrabar").click(function () {
                grabar();
            });

            document.getElementById('idDetalle').style.display = 'none';
            if (getValue('hidProceso') == 'I') {
                $('#hidProceso').val('');
                document.getElementById('idDetalle').style.display = '';
            }
        });

        function validarGrabar() {
            if (getValue('hidFlagSecRechazada') != 'S') {

                if (!validarGarantia()) return false;
                if (!validarCostoInstalacion()) return false;
            }

            return true;
        }

        function buscar() {

            if (!validarControl('txtBuscarNroSEC', '', 'Ingresar número de SEC.')) return false;

            $('#hidProceso').val('B');
            frmPrincipal.submit();
        }


        function grabar() {
            if (getValue('hidFlgSecRechazada') != 'S') {
                if (validarGrabar()) {
                    if (confirm("Esta Seguro de Grabar los cambios?")) {

                        $('#hidGarantia').val(obtenerCadenaGarantia());
                        $('#hidCostoInstalacion').val(obtenerCadenaCostoInstalacion());
                        $('#hidProceso').val('G');

                        frmPrincipal.submit();
                    }
                }
            }
            else {
                $('#hidProceso').val('G');
                frmPrincipal.submit();
            }
        }

    </script>
</head>
<body>
    <form id="frmPrincipal" runat="server">
    <table cellspacing="1" cellpadding="1" width="100%" align="center" border="0">
        <tr>
            <td class="Header" align="left" colspan="2" height="20">
                &nbsp;Busqueda de Evaluación Experto
            </td>
        </tr>
    </table>
    <table class="Contenido" cellspacing="1" cellpadding="1" width="100%" border="0">
        <tr>
            <td class="Arial10B" width="130">
                &nbsp;Nro de SEC:
            </td>
            <td class="Arial10B">
                <asp:TextBox ID="txtBuscarNroSEC" Width="100px" CssClass="clsInputEnable" Style="text-align: right"
                    runat="server" onkeypress="return validaCaracteres('0123456789');"></asp:TextBox>&nbsp;
                <input class="Boton" id="btnBuscar" style="width: 120px; height: 18px" type="button"
                    value="Buscar" />
            </td>
        </tr>
    </table>
    <div id="idDetalle">
        <table cellspacing="1" cellpadding="1" width="100%" border="0">
            <tr>
                <td class="Header" align="left" colspan="3" height="20">
                    &nbsp;Detalle de Evaluación Experto
                </td>
            </tr>
        </table>
        <table class="Contenido" cellspacing="1" cellpadding="1" width="100%" border="0">
            <tr>
                <td class="Arial10B" width="130">
                    &nbsp;Nro de SEC:
                </td>
                <td class="Arial10B">
                    <asp:TextBox ID="txtNroSEC" ReadOnly="True" Width="100px" CssClass="clsInputDisable"
                        Style="text-align: right" runat="server"></asp:TextBox>
                </td>
                <td width="500">
                </td>
            </tr>
            <tr>
                <td class="Arial10B" width="130">
                    &nbsp;Nro de Documento:
                </td>
                <td class="Arial10B">
                    <asp:TextBox ID="txtNroDocumento" ReadOnly="True" Width="100px" CssClass="clsInputDisable"
                        Style="text-align: right" runat="server"></asp:TextBox>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td class="Arial10B" width="130">
                    &nbsp;Nombres:
                </td>
                <td class="Arial10B">
                    <asp:TextBox ID="txtNombres" ReadOnly="True" Width="300px" CssClass="clsInputDisable"
                        runat="server"></asp:TextBox>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td colspan="3" class="Arial10B">
                    <table cellspacing="1" cellpadding="1" width="100%">
                        <tr>
                            <td>
                                <asp:GridView ID="gvResultadoCredito" runat="server" AllowCustomPaging="False" AllowPaging="False"
                                    AutoGenerateColumns="False" ShowFooter="True" OnRowDataBound="gvResultadoCredito_RowDataBound">
                                    <RowStyle CssClass="TablaFilasGrid" HorizontalAlign="Center" />
                                    <HeaderStyle Wrap="False" CssClass="TablaTitulos" />
                                    <FooterStyle Wrap="False" CssClass="TablaTitulos" HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:BoundField HeaderText="Producto" DataField="PRDV_DESCRIPCION" />
                                        <asp:BoundField HeaderText="Cargo Fijo Total (S/.)" DataField="SOLIN_SUM_CAR_CON"
                                            DataFormatString="{0:0.00}" />
                                        <asp:TemplateField HeaderText="Tipo Garantía">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlTipoGarantia" CssClass="clsSelectEnable" runat="server"
                                                    Width="135px" onclick="cambiarTipoGarantia(this.value);">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Nro de Cargos Fijos" DataField="NRO_CF_EVAL" />
                                        <asp:BoundField HeaderText="Monto Garantias (S/.)" DataField="SOLIN_IMP_DG" DataFormatString="{0:0.00}" />
                                        <asp:TemplateField HeaderText="Nro de Cargos Fijos">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGvNroCargosFijos" runat="server" CssClass="clsInputEnable" Width="50px"
                                                    MaxLength="5" onkeypress="return soloMontosIngreso(event, this);" Style="text-align: right"></asp:TextBox>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                Total Garantías:
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Monto Garantias (S/.)">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGvMontoGarantias" runat="server" CssClass="clsInputEnable" Width="50px"
                                                    MaxLength="5" onkeypress='eventoSoloNumerosEnteros();' Style="text-align: right"></asp:TextBox>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtGvTotalMontoGarantias" runat="server" ReadOnly="True" CssClass="clsInputDisable"
                                                    Width="50px" Style="text-align: right"></asp:TextBox>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="PRDC_CODIGO" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"
                                            FooterStyle-CssClass="hiddencol" />
                                        <asp:BoundField DataField="SOLIN_CODIGO" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"
                                            FooterStyle-CssClass="hiddencol" />
                                        <asp:BoundField DataField="CF" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"
                                            FooterStyle-CssClass="hiddencol" />
                                    </Columns>
                                </asp:GridView>
                                <asp:GridView ID="gvResultadoCredito1" runat="server" AllowCustomPaging="False" AllowPaging="False"
                                    AutoGenerateColumns="False" ShowFooter="True" OnRowDataBound="gvResultadoCredito1_RowDataBound">
                                    <RowStyle HorizontalAlign="Center" CssClass="TablaFilasGrid" />
                                    <HeaderStyle HorizontalAlign="Center" Wrap="False" CssClass="TablaTitulos" />
                                    <FooterStyle HorizontalAlign="Center" Wrap="False" CssClass="TablaTitulos" />
                                    <Columns>
                                        <asp:BoundField HeaderText="Producto" DataField="PRDV_DESCRIPCION" />
                                        <asp:BoundField HeaderText="Cargo Fijo Total (S/.)" DataField="SOLIN_SUM_CAR_CON"
                                            DataFormatString="{0:0.00}" />
                                        <asp:BoundField HeaderText="Tipo Garantía" DataField="GARANTIA_CRED" />
                                        <asp:BoundField HeaderText="Nro de Cargos Fijos" DataField="NRO_CF_EVAL" />
                                        <asp:BoundField HeaderText="Monto Garantias (S/.)" DataField="SOLIN_IMP_DG" DataFormatString="{0:0.00}" />
                                        <asp:TemplateField HeaderText="Nro de Cargos Fijos">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "NRO_CF_CRED")%>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                Total Garantías:
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Monto Garantias (S/.)" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "SOLIN_IMP_DG_MAN", "{0:0.00}")%>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="PRDC_CODIGO" Visible="False" />
                                    </Columns>
                                </asp:GridView>
                                <div style="text-align: left; width: 100%">
                                    <div style="text-align: center; width: 880px">
                                        <div id="container" style="overflow: scroll; overflow-x: hidden">
                                        </div>
                                    </div>
                                </div>
                                <asp:GridView ID="gvResultadoCredito2" runat="server" AllowCustomPaging="False" AllowPaging="False"
                                    AutoGenerateColumns="False" ShowFooter="True" OnRowDataBound="gvResultadoCredito2_RowDataBound"
                                    Width="880px">
                                    <RowStyle CssClass="TablaFilasGrid" HorizontalAlign="Center" />
                                    <HeaderStyle Wrap="False" CssClass="TablaTitulos" />
                                    <FooterStyle Wrap="False" CssClass="TablaTitulos" />
                                    <Columns>
                                        <asp:BoundField HeaderText="Plan" DataField="plnv_descripcion" />
                                        <asp:BoundField HeaderText="Producto" DataField="prdv_descripcion" />
                                        <asp:BoundField HeaderText="Cargo Fijo Total (S/.)" DataField="solin_sum_cf" DataFormatString="{0:0.00}" />
                                        <asp:TemplateField HeaderText="Tipo Garantía">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlTipoGarantia" CssClass="clsSelectEnable" runat="server"
                                                    Width="135px" onchange="cambiarTipoGarantia2(this.value);">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Nro de Cargos Fijos" DataField="solin_num_cf" />
                                        <asp:BoundField HeaderText="Monto Garantias (S/.)" DataField="solin_importe" DataFormatString="{0:0.00}" />
                                        <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Nro de Cargos Fijos<br>Copiar a todos <input type='checkbox' onclick='copiarNroCargoFijo(this.checked);'/>">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGvNroCargosFijos" runat="server" CssClass="clsInputEnable" Width="50px"
                                                    MaxLength="5" onkeypress="return soloMontosIngreso(event, this);" Style="text-align: right"></asp:TextBox>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                Total Garantías:
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Monto Garantias (S/.)">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGvMontoGarantias" runat="server" CssClass="clsInputEnable" Width="50px"
                                                    MaxLength="5" onkeypress='eventoSoloNumerosEnteros();' Style="text-align: right"></asp:TextBox>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtGvTotalMontoGarantias" runat="server" ReadOnly="True" CssClass="clsInputDisable"
                                                    Width="50px" Style="text-align: right"></asp:TextBox>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="prdc_codigo" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"
                                            FooterStyle-CssClass="hiddencol" />
                                        <asp:BoundField DataField="solin_codigo" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"
                                            FooterStyle-CssClass="hiddencol" />
                                        <asp:BoundField DataField="solin_sum_cf" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"
                                            FooterStyle-CssClass="hiddencol" />
                                        <asp:BoundField DataField="slpln_codigo" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"
                                            FooterStyle-CssClass="hiddencol" />
                                    </Columns>
                                </asp:GridView>
                                <asp:GridView ID="gvResultadoCredito3" runat="server" AllowCustomPaging="False" AllowPaging="False"
                                    AutoGenerateColumns="False" ShowFooter="True" OnRowDataBound="gvResultadoCredito3_RowDataBound" Width="880px">
                                    <RowStyle CssClass="TablaFilasGrid" HorizontalAlign="Center" />
                                    <HeaderStyle Wrap="False" CssClass="TablaTitulos" />
                                    <FooterStyle Wrap="False" CssClass="TablaTitulos" />
                                    <Columns>
                                        <asp:BoundField HeaderText="Plan" DataField="plnv_descripcion" />
                                        <asp:BoundField HeaderText="Producto" DataField="prdv_descripcion" />
                                        <asp:BoundField HeaderText="Cargo Fijo Total (S/.)" DataField="solin_sum_cf" DataFormatString="{0:0.00}" />
                                        <asp:BoundField HeaderText="Tipo Garantía" DataField="garantia_man" />
                                        <asp:BoundField HeaderText="Nro de Cargos Fijos" DataField="solin_num_cf" />
                                        <asp:BoundField HeaderText="Monto Garantias (S/.)" DataField="solin_importe" DataFormatString="{0:0.00}" />
                                        <asp:TemplateField HeaderText="Nro de Cargos Fijos">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "solin_num_cf_man")%>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                Total Garantías:
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Monto Garantias (S/.)" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "solin_importe_man", "{0:0.00}")%>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="prdc_codigo" Visible="False" />
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="Arial10B" width="130">
                    &nbsp;
                </td>
                <td colspan="2" class="Arial10B">
                    <asp:GridView ID="dgCostoInstalacion" runat="server" AllowCustomPaging="False" AllowPaging="False"
                        AutoGenerateColumns="False" ShowFooter="True" OnRowDataBound="dgCostoInstalacion_RowDataBound">
                        <RowStyle CssClass="TablaFilasGrid" HorizontalAlign="Center" />
                        <HeaderStyle Wrap="False" CssClass="TablaTitulos" />
                        <FooterStyle Wrap="False" CssClass="TablaTitulos" HorizontalAlign="Center" />
                        <Columns>
                            <asp:BoundField HeaderText="SEC" DataField="SOLIN_CODIGO" ItemStyle-CssClass="hiddencol"
                                HeaderStyle-CssClass="hiddencol" FooterStyle-CssClass="hiddencol" />
                            <asp:TemplateField HeaderText="Producto">
                                <ItemTemplate>
                                    <span>CLARO TV SAT</span>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Kit Seleccionado" DataField="KIT" />
                            <asp:TemplateField HeaderText="Costo de Instalación (S/.)">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container.DataItem, "COSTO_INSTAL_EVAL", "{0:0.00}")%>
                                </ItemTemplate>
                                <FooterTemplate>
                                    Total Costo de Instalación:
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Costo de Instalación (S/.)">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtCostoInstalacion" runat="server" CssClass="clsInputEnable" Width="50px"
                                        MaxLength="5" onkeypress="return soloMontosIngreso(event, this);" Style="text-align: right"></asp:TextBox>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtTotalCostoInstalacion" runat="server" CssClass="clsInputDisable"
                                        Width="50px" MaxLength="5" Style="text-align: right"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:GridView ID="dgCostoInstalacion1" runat="server" AllowCustomPaging="False" AllowPaging="False"
                        AutoGenerateColumns="False" ShowFooter="True" OnRowDataBound="dgCostoInstalacion1_RowDataBound">
                        <RowStyle CssClass="TablaFilasGrid" HorizontalAlign="Center" />
                        <HeaderStyle Wrap="False" CssClass="TablaTitulos" />
                        <FooterStyle Wrap="False" CssClass="TablaTitulos" HorizontalAlign="Center" />
                        <Columns>
                            <asp:BoundField HeaderText="SEC" DataField="SOLIN_CODIGO" ItemStyle-CssClass="hiddencol"
                                HeaderStyle-CssClass="hiddencol" FooterStyle-CssClass="hiddencol" />
                            <asp:TemplateField HeaderText="Producto">
                                <ItemTemplate>
                                    <span>CLARO TV SAT</span>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Kit Seleccionado" DataField="KIT" />
                            <asp:TemplateField HeaderText="Costo de Instalación (S/.)">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container.DataItem, "COSTO_INSTAL_EVAL", "{0:0.00}")%>
                                </ItemTemplate>
                                <FooterTemplate>
                                    Total Costo de Instalación:
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Costo de Instalación (S/.)">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container.DataItem, "COSTO_INSTAL", "{0:0.00}")%>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="txtTotalCostoInstalacion" runat="server" CssClass="clsInputDisable"
                                        Width="50px" Style="text-align: right"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td class="Arial10B" width="130">
                    &nbsp;
                </td>
                <td colspan="2" class="Arial10B">
                    <asp:GridView ID="dgCostoInstalacionHFC" runat="server" AllowCustomPaging="False"
                        AllowPaging="False" AutoGenerateColumns="False" ShowFooter="True" OnRowDataBound="dgCostoInstalacionHFC_RowDataBound">
                        <RowStyle CssClass="TablaFilasGrid" HorizontalAlign="Center" />
                        <HeaderStyle Wrap="False" CssClass="TablaTitulos" />
                        <FooterStyle Wrap="False" CssClass="TablaTitulos" HorizontalAlign="Center" />
                        <Columns>
                            <asp:BoundField HeaderText="SEC" DataField="SOLIN_CODIGO" ItemStyle-CssClass="hiddencol"
                                HeaderStyle-CssClass="hiddencol" FooterStyle-CssClass="hiddencol" />
                            <asp:BoundField HeaderText="Producto" DataField="PRDV_DESCRIPCION" />
                            <asp:BoundField HeaderText="Plan Seleccionado" DataField="PLNV_DESCRIPCION" />
                            <asp:TemplateField HeaderText="Costo de Instalación (S/.)">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container.DataItem, "COSTO_INSTAL_EVAL", "{0:0.00}")%>
                                </ItemTemplate>
                                <FooterTemplate>
                                    Total Costo de Instalación:
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Costo de Instalación (S/.)">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtCostoInstalacion" runat="server" CssClass="clsInputEnable" Width="50px"
                                        MaxLength="5" onkeypress="return soloMontosIngreso(event, this);" Style="text-align: right"></asp:TextBox>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtTotalCostoInstalacion" runat="server" CssClass="clsInputDisable"
                                        Width="50px" MaxLength="5" Style="text-align: right"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:GridView ID="dgCostoInstalacionHFC1" runat="server" AllowCustomPaging="False"
                        AllowPaging="False" AutoGenerateColumns="False" ShowFooter="True" OnRowDataBound="dgCostoInstalacionHFC1_RowDataBound">
                        <RowStyle CssClass="TablaFilasGrid" HorizontalAlign="Center" />
                        <HeaderStyle Wrap="False" CssClass="TablaTitulos" />
                        <FooterStyle Wrap="False" CssClass="TablaTitulos" HorizontalAlign="Center" />
                        <Columns>
                            <asp:BoundField HeaderText="SEC" DataField="SOLIN_CODIGO" ItemStyle-CssClass="hiddencol"
                                HeaderStyle-CssClass="hiddencol" FooterStyle-CssClass="hiddencol" />
                            <asp:BoundField HeaderText="Producto" DataField="PRDV_DESCRIPCION" />
                            <asp:BoundField HeaderText="Plan Seleccionado" DataField="PLNV_DESCRIPCION" />
                            <asp:TemplateField HeaderText="Costo de Instalación (S/.)">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container.DataItem, "COSTO_INSTAL_EVAL", "{0:0.00}")%>
                                </ItemTemplate>
                                <FooterTemplate>
                                    Total Costo de Instalación:
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Costo de Instalación (S/.)">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container.DataItem, "COSTO_INSTAL", "{0:0.00}")%>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="txtTotalCostoInstalacion" runat="server" CssClass="clsInputDisable"
                                        Width="50px" Style="text-align: right"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:CheckBox ID="chkReingresoSec" runat="server" Text="NO PERMITIR EL REINGRESO DE SEC BAJO LAS CONDICIONES DE EVALUACIÓN REGISTRADAS EN ESTA SEC"
                        Visible="True" CssClass="Arial10B"></asp:CheckBox>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="3">
                    <input class="Boton" id="btnGrabar" style="width: 160px; height: 18px" type="button"
                        value="Grabar" />
                </td>
            </tr>
        </table>
    </div>
    <input id="hidGarantia" type="hidden" runat="server" />
    <input id="hidCostoInstalacion" type="hidden" runat="server" />
    <input id="hidContFila" type="hidden" runat="server" />
    <input id="hidFlgConvergente" type="hidden" runat="server" />
    <input id="hidFlgSecRechazada" type="hidden" runat="server" />
    <input id="hidFlgProductoDTH" type="hidden" runat="server" />
    <input id="hidProceso" type="hidden" runat="server" />
    <input id="hidFlgProductoHFC" type="hidden" runat="server" />
    </form>
</body>
</html>
