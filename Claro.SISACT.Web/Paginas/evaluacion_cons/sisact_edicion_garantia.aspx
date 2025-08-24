<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sisact_edicion_garantia.aspx.cs"
    Inherits="Claro.SISACT.Web.Paginas.evaluacion_cons.sisact_edicion_garantia" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>SISACT Edición DG</title>
    <link href="../../Estilos/general.css" type="text/css" rel="stylesheet" />
    <script language="javascript" type="text/javascript" src='<%= ResolveClientUrl("~/Scripts/jquery-1.9.1.js") %>'></script>
    <script src="../../Scripts/security.js" type="text/javascript"></script>
    <script src="../../Scripts/funciones_sec.js" type="text/javascript"></script>
    <script src="../../Scripts/funciones_creditos.js" type="text/javascript"></script>
    <script type="text/javascript" language="JavaScript" src="../../Scripts/KeySettings/KeySettings.js"></script><!-- FALLAS PROY-140546 -->
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
                        //PROY-29215 INICIO
                        $('#hidFormaPago').val(obtenerCadenaFomarPagoCuota("FP"));
                        $('#hidCuota').val(obtenerCadenaFomarPagoCuota("C"));
                        //PROY-29215 FIN

                        //FALLAS PROY-140546
                        if (getValue('hidFlgProductoHFC') == 'S') {
                            $('#hidMAI').val(obtenerMAI());

                            if ($('#hidCostoInstalacion').val().length > 0 && $('#hidCostoInstalacion').val().indexOf(';') > -1 && parseFloat($('#hidMAI').val()) > 0) {
                                var costoInstala = $('#hidCostoInstalacion').val().split(';')[1].substring(0, $('#hidCostoInstalacion').val().split(';')[1].length - 1);
                                var vMai = $('#hidMAI').val();

                                alert('costoInstala:' + costoInstala + ' - vMai:' + vMai);
                                if (parseFloat(costoInstala) < parseFloat(vMai)) {
                                    alert(Key_MensajeMaiMayor);
                                    return false;
                                }
                            }
                        }
                        //FALLAS PROY-140546 

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

        //PROY-29215 - INICIO
        function cambiarFormapago(valor, obj) {
            var tablaResumen = document.getElementById(obj);
            var contResumen = tablaResumen.rows.length;
            var idFila, fila;
            var objCuota
            var valCuota = getValue('hidCuotaparam').split("|");
            var filaCarrito = tablaResumen.rows[2];

			//INICIO PROY-140546
            var nColumnaCuota = 0;
            var nColumnaFormaPago = 0;
            if (filaCarrito.cells.length > 9) {
                nColumnaCuota = 10;
                nColumnaFormaPago = 9;
            }
            else {
                nColumnaCuota = 8;
                nColumnaFormaPago = 7;
            }
			//FIN PROY-140546

            var objCuotaOpcion = filaCarrito.cells[nColumnaCuota].getElementsByTagName("option"); //PROY-140546
            var objFP = filaCarrito.cells[nColumnaFormaPago].getElementsByTagName("select")[0]; //PROY-140546
            var valFP;

            for (var o = 0; o < objFP.length; o++) {
                if (objFP[o].selected) {
                    valFP = objFP[o].text;
                }
            }

            objCuota = filaCarrito.cells[nColumnaCuota].getElementsByTagName("select")[0]; //PROY-140546
            var option = document.createElement("option");


            if (valFP.toUpperCase() == "CONTRATA") {

                for (var cu = 0; cu < objCuotaOpcion.length; cu++) {
                    objCuota.remove(cu);
                    cu--;
                }

                for (var cp in valCuota) {
                    fila = valCuota[cp].split(";");
                    if (fila[1] == "0") {
                        option.text = fila[1];
                        option.value = fila[0];

                        objCuota.add(option);
                    }

                }
                calcularCM(obj); //PROY-140546
            }
            else if (valFP.toUpperCase() == "FACTURACION") {

                for (var cu = 0; cu < objCuotaOpcion.length; cu++) {
                    objCuota.remove(cu);
                    cu--;
                }

                for (var cp in valCuota) {
                    fila = valCuota[cp].split(";");
                    if (fila[0] != "" && fila[1] != "0") {
                        var option1 = document.createElement("option");

                        option1.text = fila[1];
                        option1.value = fila[0];

                        objCuota.add(option1);
                    }

                }
                calcularCM(obj); //PROY-140546
            }

        }
        //PROY-29215 - FIN

        //PROY-140546 Inicio
        function calcularCM(obj) {
            var tablaResumen = document.getElementById(obj);
            var filaCarrito = tablaResumen.rows[2];

            if (filaCarrito.cells.length > 9) {
                var objCombo = filaCarrito.cells[10].getElementsByTagName("select")[0]; //Cuotas
                var costoInstalacion = filaCarrito.cells[8].getElementsByTagName("input")[0].value; //Costo instalacion
                var nMAI = filaCarrito.cells[11].getElementsByTagName("input")[0].value;
                var nuevoCostoAnticipadoInstalacion = 0;
                var nNuevoCostoInstalacion = costoInstalacion - nMAI;

                for (var o = 0; o < objCombo.length; o++) {
                    if (objCombo[o].selected) {
                        var valCombo = objCombo[o].text;
                    }
                }

                var monto = 0;
                if ((costoInstalacion != "" || costoInstalacion == "0" || costoInstalacion != " " || costoInstalacion != 'undefined' || costoInstalacion != undefined) && valCombo != "0") {
                    monto = nNuevoCostoInstalacion / valCombo;
                }
                else {
                    monto;
                }

                filaCarrito.cells[12].getElementsByTagName("input")[0].value = monto;

                //INICIO FALLAS PROY-140546
                if (!(parseFloat(costoInstalacion) > 0)) {
                    filaCarrito.cells[11].getElementsByTagName("input")[0].value = 0;
                    filaCarrito.cells[12].getElementsByTagName("input")[0].value = 0;
                }
                //FIN FALLAS PROY-140546
            }
        }
        //PROY-140546 Fin

    </script>
</head>
<body>
    <form id="frmPrincipal" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
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
<%--PROY-29215 INICIO--%>
                            <asp:TemplateField HeaderText="Forma de Pago:">
                            <ItemTemplate>
                                    <asp:TextBox ID="txtFormaPagoTVSAT" runat="server" CssClass="clsInputEnable" 
                                        MaxLength="5" Style="text-align: right" Width="105px"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Cuotas:">
                              <ItemTemplate>
                                    <asp:TextBox ID="txtCuotaTVSAT" runat="server" CssClass="clsInputEnable" 
                                        MaxLength="5" Style="text-align: right" Width="50px"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--PROY-29215 FIN--%>							
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
                           <%--PROY-29215 INICIO--%>
                            <asp:TemplateField HeaderText="Forma de Pago:">
                             <ItemTemplate>
                                                <asp:DropDownList ID="ddlFormaPagoTVSAT" CssClass="clsSelectEnable" runat="server" onchange="cambiarFormapago(this,'dgCostoInstalacion')"
                                                    Width="105px" AppendDataBoundItems="true">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cuotas:">
                             <ItemTemplate>
                                                <asp:DropDownList ID="ddlNroCuotasTVSAT" CssClass="clsSelectEnable" runat="server"
                                                    Width="70px" AppendDataBoundItems="true">
                                                </asp:DropDownList>
                              </ItemTemplate>
                            </asp:TemplateField>
                            <%--PROY-29215 FIN--%>							
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
                            <%--PROY-29215 INICIO--%>
                            <asp:TemplateField HeaderText="Forma Pago:">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtFormaPagoTVSAT1" runat="server" CssClass="clsInputEnable" 
                                        MaxLength="5" Style="text-align: right" Width="105px"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cuota:">
                             <ItemTemplate>
                                    <asp:TextBox ID="txtCuotaTVSAT1" runat="server" CssClass="clsInputEnable" 
                                        MaxLength="5" 
                                        Style="text-align: right" Width="50px"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--PROY-29215 FIN--%>
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
                            <%--PROY-29215 INICIO--%>
                            <asp:TemplateField HeaderText="Forma de Pago:">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtFormaPago" runat="server" CssClass="clsInputEnable" 
                                        MaxLength="5" onkeypress="return soloMontosIngreso(event, this);" 
                                        Style="text-align: center" Width="105px"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cuotas:  ">
                             <ItemTemplate>
                                    <asp:TextBox ID="txtCuotaPago" runat="server" CssClass="clsInputEnable" 
                                        MaxLength="5" onkeypress="return soloMontosIngreso(event, this);" 
                                        Style="text-align: center" Width="50px"></asp:TextBox>

                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--PROY-29215 FIN--%>
							<%--INICIO PROY-140546--%>
                            <asp:TemplateField HeaderText="MAI">
			                    <ItemTemplate>
				                    <asp:TextBox ID="txtMAINoEdit" ReadOnly="true" CssClass="clsInputEnabled" runat="server" Width="50px"></asp:TextBox>
			                    </ItemTemplate>
		                    </asp:TemplateField>
		                    <asp:TemplateField HeaderText="CM">
			                    <ItemTemplate>
				                    <asp:TextBox ID="txtCMNoEdit" ReadOnly="true" CssClass="clsInputEnabled" runat="server" Width="50px"></asp:TextBox>
			                    </ItemTemplate>
		                    </asp:TemplateField>
							<%--FIN PROY-140546--%>
                            <asp:TemplateField HeaderText="Costo de Instalación (S/.)">
                                <ItemTemplate>
									<%--PROY-140546--%>
                                    <asp:TextBox ID="txtCostoInstalacion" runat="server" CssClass="clsInputEnable" Width="50px" onkeyup="calcularCM('dgCostoInstalacionHFC')"
                                        MaxLength="5" onkeypress="return soloMontosIngreso(event, this);" Style="text-align: right"></asp:TextBox>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtTotalCostoInstalacion" runat="server" CssClass="clsInputDisable"
                                        Width="50px" MaxLength="5" Style="text-align: right"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <%--PROY-29215 INICIO--%>
                            <asp:TemplateField HeaderText="Forma de Pago:">
                             <ItemTemplate>
                                <asp:DropDownList id="ddlFormaPago" CssClass="clsSelectEnable" runat="server" onchange="cambiarFormapago(this,'dgCostoInstalacionHFC')"
                                    Width="105px">
                                </asp:DropDownList>
                            </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cuotas:">
                             <ItemTemplate>
								<%--PROY-140546--%>
                                <asp:DropDownList id="ddlNroCuotas" CssClass="clsSelectEnable" runat="server" onchange="calcularCM('dgCostoInstalacionHFC')"
                                    Width="70px" >
                                </asp:DropDownList>
                            </ItemTemplate>
                            </asp:TemplateField>
                            <%--PROY-29215 FIN--%>
							<%--INICIO PROY-140546--%>
                             <asp:TemplateField HeaderText="MAI">
			                    <ItemTemplate>
				                    <asp:TextBox ID="txtMAIEdit" ReadOnly="false" onkeypress="return soloMontosIngreso(event, this);" onkeyup="calcularCM('dgCostoInstalacionHFC')" CssClass="clsInputEnabled" runat="server" Width="50px"></asp:TextBox>
			                    </ItemTemplate>
		                    </asp:TemplateField>
		                    <asp:TemplateField HeaderText="CM">
			                    <ItemTemplate>
				                    <asp:TextBox ID="txtCMEdit" ReadOnly="true" CssClass="clsInputEnabled" runat="server" Width="50px"></asp:TextBox>
			                    </ItemTemplate>
		                    </asp:TemplateField>
							<%--FIN PROY-140546--%>
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
                            <%--PROY-29215 INICIO--%>
                            <asp:TemplateField HeaderText="Forma de Pago: ">
                            <ItemTemplate>
                                    <asp:TextBox ID="txtFormaPago1" runat="server" CssClass="clsInputEnable" 
                                        MaxLength="5" onkeypress="return soloMontosIngreso(event, this);" 
                                        Style="text-align: right" Width="105px"></asp:TextBox>

                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cuota:">
                            <ItemTemplate>
                                    <asp:TextBox ID="txtCuotaPago1" runat="server" CssClass="clsInputEnable" 
                                        MaxLength="5" onkeypress="return soloMontosIngreso(event, this);" 
                                        Style="text-align: right" Width="50px"></asp:TextBox>

                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--PROY-29215 INICIO--%>
							<%--INICIO PROY-140546--%>
                            <asp:TemplateField HeaderText="MAI">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtMAINoEdit" ReadOnly="true" CssClass="clsInputEnabled" runat="server" Width="50px"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CM">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtCMNoEdit" ReadOnly="true" CssClass="clsInputEnabled" runat="server" Width="50px"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
							<%--FIN PROY-140546--%>
                            <asp:TemplateField HeaderText="Costo de Instalación (S/.)">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container.DataItem, "COSTO_INSTAL", "{0:0.00}")%>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="txtTotalCostoInstalacion" runat="server" CssClass="clsInputDisable"
                                        Width="50px" Style="text-align: right"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                             <%--INICIO PROY-140546--%>
                            <asp:TemplateField HeaderText="Forma de Pago: ">
                            <ItemTemplate>
                                    <asp:TextBox ID="txtFormaPago2" runat="server" CssClass="clsInputEnable" 
                                        MaxLength="5" onkeypress="return soloMontosIngreso(event, this);" 
                                        Style="text-align: right" Width="105px"></asp:TextBox>

                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cuota:">
                            <ItemTemplate>
                                    <asp:TextBox ID="txtCuotaPago2" runat="server" CssClass="clsInputEnable" 
                                        MaxLength="5" onkeypress="return soloMontosIngreso(event, this);" 
                                        Style="text-align: right" Width="50px"></asp:TextBox>

                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="MAI">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtMAINoEdit2" ReadOnly="true" CssClass="clsInputEnabled" runat="server" Width="50px"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CM">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtCMNoEdit2" ReadOnly="true" CssClass="clsInputEnabled" runat="server" Width="50px"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
							<%--FIN PROY-140546--%>
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
    <%--PROY-29215 INICIO--%>
    <input id="hidFormaPago" type="hidden" runat="server" />
    <input id="hidCuota" type="hidden" runat="server" />

    <input id="hidFormaPagoActual" type="hidden" runat="server" />
    <input id="hidCuotaActual" type="hidden" runat="server" />
    <input id="hidCuotaparam" type="hidden" runat="server" />
   <%-- PROY-29215 FIN --%>
    
    <input id="hidGarantia" type="hidden" runat="server" />
    <input id="hidCostoInstalacion" type="hidden" runat="server" />
    <input id="hidContFila" type="hidden" runat="server" />
    <input id="hidFlgConvergente" type="hidden" runat="server" />
    <input id="hidFlgSecRechazada" type="hidden" runat="server" />
    <input id="hidFlgProductoDTH" type="hidden" runat="server" />
    <input id="hidProceso" type="hidden" runat="server" />
    <input id="hidFlgProductoHFC" type="hidden" runat="server" />
    <%--INC000003135879 INICIO--%>
    <input id="hidCostoInstalacionAnterior" type="hidden" runat="server" />
    <%--INC000003135879 FIN--%>
	<%--INICIO PROY-140546--%>
    <input id="hidMAI" type="hidden" name="hidMAI" runat="server" />
    <input id="hidMAIOriginal" type="hidden" name="hidMAIOriginal" runat="server" />
    <input id="hidPdvSEC" type="hidden" name="hidPdvSEC" runat="server" />
    <input id="hidEstadoPa" type="hidden" name="hidEstadoPa" runat="server" />
	<%--FIN PROY-140546--%>
    </form>
</body>
</html>
