<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sisact_pop_consulta_sec.aspx.cs"
    Inherits="Claro.SISACT.Web.Paginas.consultas.sisact_pop_consulta_sec" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Consulta SEC Pendiente</title>
    <link href="../../Estilos/general.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" language="javascript">

        function asignarFila(nroSEC, strNombres, strApePaterno, strApeMaterno, strRazonSocial) {
            document.getElementById('hidNroSEC').value = nroSEC;
            document.getElementById('txtApePat').value = strApePaterno;
            document.getElementById('txtApeMat').value = strApeMaterno;
            document.getElementById('txtNombre').value = strNombres;
            document.getElementById('txtRazonSocial').value = strRazonSocial;
        }

        function aceptar() {
            var nroSEC = document.getElementById('hidNroSEC').value;
            if (nroSEC == '') {
                alert('Debe selecionar una evaluación.');
                return false;
            }

            window.opener.document.getElementById('hidNroSEC').value = nroSEC;
            window.opener.document.getElementById('txtApePat').value = document.getElementById('txtApePat').value;
            window.opener.document.getElementById('txtApeMat').value = document.getElementById('txtApeMat').value;
            window.opener.document.getElementById('txtNombre').value = document.getElementById('txtNombre').value;
            window.opener.document.getElementById('txtRazonSocial').value = document.getElementById('txtRazonSocial').value;
            window.opener.retornarConsultaSEC(nroSEC);
            window.close();
        }

        function cerrar() {
            window.opener.nuevaEvaluacion();
            window.close();
        }

        function inicio() {
            for (var num = 0; num <= document.frmPrincipal.length - 1; num++) {
                if (document.frmPrincipal.elements[num].name == "rbtSEC") {
                    if (document.frmPrincipal.elements[num].disabled) {
                        alert("El cliente tiene una SEC que ha sido utilizada en una venta, no se puede generar una nueva SEC.");
                        document.getElementById('btnAceptar').disabled = true;
                        break;
                    }
                }
            }
        }
    </script>
</head>
<body>
    <form id="frmPrincipal" runat="server">
    <table width="100%" cellspacing="2" cellpadding="0" border="0" align="center">
        <tr>
            <td>
                <div id="lista" style="overflow: auto; width: 950px">
                    <asp:GridView ID="gvPool" runat="server" BorderWidth="1px" AutoGenerateColumns="False"
                        OnRowDataBound="gvPool_RowDataBound">
                        <AlternatingRowStyle HorizontalAlign="Center" Height="15px" CssClass="Arial10" />
                        <RowStyle HorizontalAlign="Center" Height="15px" CssClass="Arial10" />
                        <HeaderStyle CssClass="TablaTitulos"></HeaderStyle>
                        <Columns>
                            <asp:TemplateField HeaderText=" ">
                                <ItemTemplate>
                                    <input type="radio" id="rbtSEC" name="rbtSEC" onclick="asignarFila('<%# DataBinder.Eval(Container.DataItem, "SOLIN_CODIGO")%>','<%# DataBinder.Eval(Container.DataItem, "CLIEV_NOMBRE")%>','<%# DataBinder.Eval(Container.DataItem, "CLIEV_APE_PAT")%>','<%# DataBinder.Eval(Container.DataItem, "CLIEV_APE_MAT")%>','<%# DataBinder.Eval(Container.DataItem, "CLIEV_RAZ_SOC")%>');" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="SOLIN_CODIGO" HeaderText="Nro SEC">
                                <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="ESTAV_DESCRIPCION" HeaderText="Estado">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="CLIEC_NUM_DOC" HeaderText="Nro Documento">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="NOMBRE_COMPLETO" HeaderText="Nombre / Raz&#243;n Social">
                                <HeaderStyle HorizontalAlign="Center" Width="350px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Left" Width="350px"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="OVENV_DESCRIPCION" HeaderText="Oficina Venta">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" Width="270px"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="SOLID_FEC_REG" HeaderText="Fecha Registro">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" Width="200px"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="TCARV_DESCRIPCION" HeaderText="Tipo Garantia">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" Width="200px"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="SOLIN_IMP_DG" HeaderText="Importe">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" Width="80px"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="ESTADO_VENTA" HeaderText="Estado de Venta">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="TDOCC_CODIGO" HeaderText="Tipo Documento" ItemStyle-CssClass="hiddencol"
                                HeaderStyle-CssClass="hiddencol" FooterStyle-CssClass="hiddencol"></asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </div>
            </td>
        </tr>
        <tr class="fila_consulta">
            <td height="20" align="left">
                <asp:Label ID="lblFilas" CssClass="Arial10BRed" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <table cellspacing="1" cellpadding="0" width="100%">
                    <tr>
                        <td align="center">
                            <input id="btnAceptar" type="button" class="Boton" style="width: 100px; cursor: hand;
                                height: 19px" onclick="aceptar();" value="Aceptar" />&nbsp;&nbsp;
                            <input id="btnCancelar" type="button" class="Boton" style="width: 100px; cursor: hand;
                                height: 19px" onclick="cerrar();" value="Cancelar" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <input type="hidden" id="hidNroSEC" style="width: 1px; height: 1px" />
    <input type="hidden" id="txtApePat" style="width: 1px; height: 1px" />
    <input type="hidden" id="txtApeMat" style="width: 1px; height: 1px" />
    <input type="hidden" id="txtNombre" style="width: 1px; height: 1px" />
    <input type="hidden" id="txtRazonSocial" style="width: 1px; height: 1px" />
    </form>
</body>
</html>
