<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sisact_declaracion_conocimiento.aspx.cs" Inherits="Claro.SISACT.Web.Paginas.documentos.sisact_declaracion_conocimiento" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>SISACT - Declaración de Conocimiento</title>
    <link href="../../Estilos/general.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" language="JavaScript" src="../../Scripts/funciones_sec.js"></script>
    <script type="text/javascript" language="JavaScript" src="../../Scripts/Paginas/documentos/sisact_declaracion_conocimiento.js"></script>
        <script language="javascript" type="text/javascript" src="../../Scripts/jquery-1.9.1.js"></script>
        <script language="javascript" type="text/javascript" src="../../Scripts/security.js"></script>
</head>
<body onload="javascript:Inicio();">
		<form id="frmDeclConocimiento" method="post" runat="server">
			<table id="tblDeclarConocimiento" class="contenido" border="0" cellPadding="1" width="100%" style="align: center">
            <tr>
				<td class="Header" align="center" height="19" colspan="4">Declaración de Conocimiento</td>
			</tr>
            <tr>
                <td style="WIDTH: 15PX; align:center"></td>
				<td style="WIDTH:740PX; align: left"></td>
                <td style="WIDTH: 20px; align:center"></td>
                <td style="WIDTH: 10px; align:center"></td> 
			</tr>
			</table>
			<table class="contenido" border="0" cellSpacing="0" cellPadding="1" width="90%" style="text-align: justify">
            <tr><td></td>
			</tr>
			<tr>
				<td align="center"><input style="WIDTH: 126px; HEIGHT: 19px; CURSOR: hand" id="btnGrabar" class="Boton" onmouseover="this.className='BotonResaltado';"
						onmouseout="this.className='Boton';" onclick="javascript:GuardarItems();" value="Aceptar" type="button"
						name="btnGrabar">&nbsp;&nbsp;
				</td>
			</tr>
			</table>
		</form>
		<input id="hidCadenaItems" type="hidden" name="hidCadenaItems" runat="server"/> <!-- PROY-26358 - Evalenzs -  hidCadenaItems -->
		<!--<input id="hidCadenaItemsCheck" type="hidden" name="hidCadenaItemsCheck" runat="server">  PROY-26358 - Evalenzs -  hidCadenaItems -->
	</body>
</html>
