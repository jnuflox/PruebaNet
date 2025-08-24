<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sisact_ifr_consulta_unificada.aspx.cs"
    Inherits="Claro.SISACT.Web.frames.sisact_ifr_consulta_unificada" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>sisact_ifr_consulta_unificada</title>
    <script language='javascript' type='text/javascript' src='../../Scripts/funciones_sec.js'></script>
    <script language="javascript" type="text/javascript" src="../../Scripts/jquery-1.9.1.js" ></script>
    <script language='javascript' type='text/javascript' src='../../Scripts/security.js'></script>
    <script language="JavaScript" type="text/javascript" src='../../Scripts/Paginas/frames/sisact_ifr_consulta_unificada.js'></script>
</head>
<body onload="inicio()">
    <form id="Form1" method="post" runat="server">
    <input id="hidIdFila" type="hidden" name="hidIdFila" runat="server" />
    <input id="hidnResultadoValue" type="hidden" name="hidnResultadoValue" runat="server" />
    <input id="hidMetodo" type="hidden" name="hidMetodo" runat="server" />
    <input id="hidnMensajeValue" type="hidden" name="hidnMensajeValue" runat="server" />
    <%--INC000002510501-INI--%>
    <input id="hidModalidadPortabi" type="hidden" name="hidModalidadPortabi" runat="server" />
    <input id="hidOperadorPortabi" type="hidden" name="hidOperadorPortabi" runat="server" />
    <%--INC000002510501-FIN--%>
    <!--//'PROY-30748-INICIO-->
			<input type="hidden" id="hiddestable" name="hiddestable" runat="server">
			<input type="hidden" id="hidcodmodalidad" name="hidcodmodalidad" runat="server">
			<!--//'PROY-30748-FIN-->
      <!--//'PROY-140736-INICIO-->
    <input id="hdbuyback_frame" type="hidden" name="hdbuyback_frame" runat="server" />
    <!--//'PROY-140736-FIN-->
    </form>
</body>
</html>
