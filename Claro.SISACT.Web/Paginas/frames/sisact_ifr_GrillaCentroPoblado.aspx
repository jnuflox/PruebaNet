<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sisact_ifr_GrillaCentroPoblado.aspx.cs"
    Inherits="Claro.SISACT.Web.Paginas.frames.sisact_ifr_GrillaCentroPoblado" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>frmGrillaCentroPoblado</title>
    <link href="../../Estilos/general.css" type="text/css" rel="stylesheet"/>
    <script language="javascript" type="text/javascript" src="../../Scripts/funciones_sec.js"></script>
    <script language="javascript" type="text/javascript" src="../../Scripts/jquery-1.9.1.js" ></script>
    <script language="javascript" type="text/javascript" src="../../Scripts/security.js"></script>
    <script type="text/javascript" language="javascript">

        function inicio() {
            if (window.parent)
                window.parent.validarCargaGrilla(getValue('hidNResponseValue'), getValue('hidNResponseValue'), getValue('hidNroFilas'));
        }

        function itemSeleccionado(it, flagVOD) {
            window.parent.asignarValorCentroPoblado(it, flagVOD, '');
        }

    </script>
</head>
<body onload="inicio();" style="margin: 0px;">
    <form id="frmPrincipal" method="post" runat="server">
    <table align="center">
        <tr>
            <td>
                <asp:DataGrid ID="dgPDV" runat="server" Width="800px" AllowPaging="True" CellSpacing="1"
                    CellPadding="0" BorderColor="#95B7F3" AutoGenerateColumns="False" DataKeyField="IdPlano"
                    PageSize="10" onpageindexchanged="dgPDV_PageIndexChanged">
                    <ItemStyle CssClass="ClsArial09" HorizontalAlign="Center"></ItemStyle>
                    <HeaderStyle Wrap="False" HorizontalAlign="Center" CssClass="TablaTitulos"></HeaderStyle>
                    <Columns>
                        <asp:TemplateColumn>
                            <ItemStyle HorizontalAlign="Center" Width="12px"></ItemStyle>
                            <HeaderTemplate>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input class="clsArial10" id="rbPlano" type="radio" value='<%# DataBinder.Eval(Container,"DataItem.IdPlano") %>'
                                    onclick="itemSeleccionado('<%# DataBinder.Eval(Container,"DataItem.IdPlano") %>', '<%# DataBinder.Eval(Container,"DataItem.FlagVOD") %>');"
                                    name="rbPlano">
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="IdPlano" HeaderText="Id Plano"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Descripcion" HeaderText="Descripcion"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Distrito_Desc" HeaderText="Distrito"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Centro_Poblado" HeaderText="Centro Poblado"></asp:BoundColumn>
                    </Columns>
                    <PagerStyle CssClass="clsArial10" Mode="NumericPages"></PagerStyle>
                </asp:DataGrid>
            </td>
        </tr>
        <tr style="display: none">
            <td>
                <input id="hidCodSerie" type="hidden" name="hidCodImei" runat="server" style="width: 16px;
                    height: 22px" size="1" />
                <input id="hidDatosRetorno" type="hidden" name="hidDatosRetorno" runat="server" style="width: 16px;
                    height: 22px" size="1" />
                <input id="hidNMsgValue" type="hidden" name="hidNMsgValue" runat="server" style="width: 16px;
                    height: 22px" size="1" />
                <input id="hidRequest" type="hidden" name="hidRequest" runat="server" style="width: 16px;
                    height: 22px" size="1" />
                <input id="hidNResponseValue" type="hidden" name="hidNResponseValue" runat="server" style="width: 16px;
                    height: 22px" size="1" />
                <input id="hidNroFilas" type="hidden" name="hidNroFilas" runat="server" style="width: 16px;
                    height: 22px" size="1" />
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
