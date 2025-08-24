<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sisact_ifr_representante_legal.aspx.cs"
    Inherits="Claro.SISACT.Web.Paginas.frames.sisact_ifr_representante_legal" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../Estilos/general.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" language="JavaScript" src="../../Scripts/jquery-1.9.1.js"></script> <!-- PROY-31636 -->
    <script type="text/javascript" language="JavaScript" src="../../Scripts/funciones_sec.js"></script>
    <script type="text/javascript" language="JavaScript" src="../../Scripts/security.js"></script>
    <script type="text/javascript" language="JavaScript">

        var constCodTipoDocumentoDNI = '<%= ConfigurationManager.AppSettings["constCodTipoDocumentoDNI"] %>';
        var constCodTipoDocumentoRUC = '<%= ConfigurationManager.AppSettings["constCodTipoDocumentoRUC"] %>';

        function window.confirm(str) {
            execScript('n = msgbox("' + str + '","4132")', "vbscript");
            return (n == 6);
        }

    </script>
    <script type="text/javascript" language="JavaScript" src="../../Scripts/Paginas/frames/sisact_ifr_representante_legal.js"></script>
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
                            <ItemTemplate><%--PROY-29121--%>
                                <input id="chkRepresentanteLegal" runat="server" type="checkbox" onclick='seleccionarItem(this);' 
                                         name="chkRepresentanteLegal" />
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
                        <asp:TemplateColumn HeaderText="Nacionalidad">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlNacionalidad" runat="server" Width="130px" CssClass="clsSelectEnable0" DataSource='<%# CargarNacionalidades() %>' DataValueField="Codigo" DataTextField="Descripcion">
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Cargo">
                            <ItemTemplate>
                                <asp:Label ID="lblCargo" CssClass="TablaFilas" Text='<%# DataBinder.Eval(Container.DataItem, "APODV_CAR_REP")%>'
                                    runat="server">
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
              
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
    <%--PROY-29121-INI--%>
    <input id="hidSituacionOK" style="width: 8px; height: 22px" type="hidden" size="1" name="hidSituacionOK" runat="server" />
    <input id="hidSituacionNOOK" style="width: 8px; height: 22px" type="hidden" size="1" name="hidSituacionNOOK" runat="server" />
    <%--PROY-29121-INI--%>
    </form>
</body>
</html>
