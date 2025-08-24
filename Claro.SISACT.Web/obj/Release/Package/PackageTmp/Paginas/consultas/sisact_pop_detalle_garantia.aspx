<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sisact_pop_detalle_garantia.aspx.cs"
    Inherits="Claro.SISACT.Web.Paginas.consultas.sisact_pop_detalle_garantia" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Consulta Detalle Garantía</title>
    <link href="../../Estilos/general.css" type="text/css" rel="stylesheet" />
        <script language="javascript" type="text/javascript" src="../../Scripts/jquery-1.9.1.js"></script>
        <script language="javascript" type="text/javascript" src="../../Scripts/security.js"></script>
</head>
<body>
    <form id="frmPrincipal" method="post" runat="server">
    <asp:GridView ID="gvGarantia" runat="server" Width="100%" AutoGenerateColumns="False"
        OnRowDataBound="gvGarantia_RowDataBound" ShowFooter="True" ShowHeaderWhenEmpty="true">
        <HeaderStyle Wrap="False" HorizontalAlign="Center" CssClass="TablaTitulos" />
        <RowStyle BackColor="#E9EBEE" HorizontalAlign="Center" CssClass="Arial10" />
        <AlternatingRowStyle BackColor="#DDDEE2" HorizontalAlign="Center" CssClass="Arial10" />
        <FooterStyle Wrap="False" CssClass="TablaTitulos" />
        <Columns>
            <asp:BoundField HeaderText="Plan" DataField="plan" HeaderStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Producto" DataField="producto" HeaderStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Cargo Fijo Total (S/.)" DataField="CF" DataFormatString="{0:0.00}"
                HeaderStyle-HorizontalAlign="Center" />
            <asp:TemplateField HeaderText="Tipo Garantía" HeaderStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "garantia")%>
                </ItemTemplate>
                <FooterTemplate>
                    Total Garantías:
                </FooterTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="Monto Garantias (S/.)" DataField="importe" DataFormatString="{0:0.00}"
                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
        </Columns>
    </asp:GridView>
    </form>
</body>
</html>
