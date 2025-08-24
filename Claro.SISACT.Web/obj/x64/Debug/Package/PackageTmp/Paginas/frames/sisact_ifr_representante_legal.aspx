<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sisact_ifr_representante_legal.aspx.cs"
    Inherits="Claro.SISACT.Web.Paginas.frames.sisact_ifr_representante_legal" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../Estilos/general.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" language="JavaScript" src="../../Scripts/funciones_sec.js"></script>
    <script type="text/javascript" language="JavaScript">

        function obtenerRepresentanteSeleccionado() {

            var tbl = document.getElementById('dgRepresentanteLegal');
            var filas = tbl.rows.length;
            var lista_salida = '';
            for (var i = 0; i < filas - 1; i++) {
                var control = 'dgRepresentanteLegal_chkRepresentanteLegal_' + i;
                var chk = document.getElementById(control);
                if (chk != null) {
                    if (chk.checked) {
                        var controlNumero = 'dgRepresentanteLegal_lblNumero_' + i;
                        var controlTipoDocumento = 'dgRepresentanteLegal_hidTipoDocumento_' + i;
                        var controlNombre = 'dgRepresentanteLegal_lblNombre_' + i;
                        var controlApePaterno = 'dgRepresentanteLegal_lblApellidoPaterno_' + i;
                        var controlApeMaterno = 'dgRepresentanteLegal_lblApellidoMaterno_' + i;
                        var controlCargo = 'dgRepresentanteLegal_lblCargo_' + i;

                        var numero = getValueHTML(controlNumero);
                        var tipoDocumento = getValue(controlTipoDocumento);
                        var nombre = getValueHTML(controlNombre).replace("'", "");
                        var apePaterno = getValueHTML(controlApePaterno).replace("'", "");
                        var apeMaterno = getValueHTML(controlApeMaterno).replace("'", "");
                        var cargo = getValueHTML(controlCargo);

                        var datosRepresentante = tipoDocumento + ";" + numero + ";" + nombre + ";" + apePaterno + ";" + apeMaterno + ";" + cargo;
                        if (numero != '') {
                            lista_salida += datosRepresentante + "|";
                        }
                    }
                }
            }
            if (lista_salida != '') {
                lista_salida = lista_salida.substring(0, lista_salida.length - 1);
            }
            return lista_salida;
        }

        function seleccionarTodo(chk) {
            var xState = chk.checked;
            var theBox = chk;
            var nombre = "_chkRepresentanteLegal";
            var idBox;
            var i = 0;
            elm = theBox.form.elements;
            for (i = 0; i < elm.length; i++) {
                if (elm[i].type == "checkbox" && elm[i].id != theBox.id) {
                    idBox = elm[i].id;
                    var index = idBox.indexOf(nombre);
                    if (index > -1) {
                        if (elm[i].checked != xState)
                            elm[i].click();
                    }
                }
            }
        }
    </script>
</head>
<body>
    <form id="frmPrincipal" method="post" runat="server">
    <table style="z-index: 101; left: 0px; position: absolute; top: 1px" cellspacing="0"
        cellpadding="0" width="102%" border="0">
        <tr>
            <td align="left">
                <asp:DataGrid ID="dgRepresentanteLegal" runat="server" Width="100%" AutoGenerateColumns="False">
                    <ItemStyle Wrap="False" HorizontalAlign="Center" CssClass="TablaFilas"></ItemStyle>
                    <HeaderStyle Wrap="False" HorizontalAlign="Center" CssClass="TablaTitulos"></HeaderStyle>
                    <Columns>
                        <asp:TemplateColumn>
                            <ItemStyle HorizontalAlign="Center" Width="5px"></ItemStyle>
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkMarcar" onclick="seleccionarTodo(this);" ToolTip="Seleccionar Todo/Deseleccionar Todo"
                                    AutoPostBack="false" runat="server" Text="" TextAlign="Right"></asp:CheckBox>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input id="chkRepresentanteLegal" runat="server" type="checkbox" name="chkRepresentanteLegal" />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Nombre">
                            <ItemTemplate>
                                <asp:Label ID="lblNombre" CssClass="TablaFilas" Text='<%# DataBinder.Eval(Container.DataItem, "APODV_NOM_REP_LEG")%>'
                                    runat="server">
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Apellido Paterno">
                            <ItemTemplate>
                                <asp:Label ID="lblApellidoPaterno" CssClass="TablaFilas" Text='<%# DataBinder.Eval(Container.DataItem, "APODV_APA_REP_LEG")%>'
                                    runat="server">
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Apellido Materno">
                            <ItemTemplate>
                                <asp:Label ID="lblApellidoMaterno" CssClass="TablaFilas" Text='<%# DataBinder.Eval(Container.DataItem, "APODV_AMA_REP_LEG")%>'
                                    runat="server">
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Tipo Documento">
                            <HeaderStyle Wrap="False" HorizontalAlign="Center" CssClass="TablaTitulos"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblTipoDocumento" runat="server" CssClass="TablaFilas" Text='<%# DataBinder.Eval(Container.DataItem, "TDOCV_DESCRIPCION_REP")%>'>
                                </asp:Label>
                                <input id="hidTipoDocumento" value='<%# DataBinder.Eval(Container.DataItem, "APODC_TIP_DOC_REP")%>'
                                    runat="server" type="hidden" />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="N&#176; Documento">
                            <ItemTemplate>
                                <asp:Label ID="lblNumero" CssClass="TablaFilas" Text='<%# DataBinder.Eval(Container.DataItem, "APODV_NUM_DOC_REP")%>'
                                    runat="server">
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="APODV_CAR_REP" HeaderText="Cargo"></asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="Situaci&#243;n">
                            <HeaderStyle HorizontalAlign="Center" CssClass="TablaTitulos"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblSituacion" runat="server" CssClass="TablaFilas"></asp:Label>
                                <input id="hidMensajeSituacion" runat="server" type="hidden" />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
