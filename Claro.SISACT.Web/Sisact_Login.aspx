<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Sisact_Login.aspx.cs" Inherits="Claro.SISACT.Web.sisact_login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
		<title>.:: SISACT Login ::.</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
		<meta content="JavaScript" name="vs_defaultClientScript" />
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
		<LINK href="estilos/general.css" type="text/css" rel="styleSheet" >
        <script language="javascript" type="text/javascript" src="Scripts/jquery-1.9.1.js" ></script>
        <script type="text/javascript" language="javascript" src="Scripts/security.js"></script>
	</head>
	<body bgColor="#ffffff" MS_POSITIONING="GridLayout">
		<script type="text/javascript" language="Javascript">
		    top.window.resizeTo(screen.availWidth, screen.availHeight);
		    document.oncontextmenu = function () { return false; } 				
		</script>
	    <INPUT id="hidUsuarioId" type="hidden" name="hidUsuarioId" runat="server" />
		<form id="frmPrincipal" method="post" runat="server">
		    <br/>
		    <br/>
		    <br/>
		    <br/>
			<table borderColor="#95b7f3" cellSpacing="0" cellPadding="3" width="740" align="center"
				bgColor="#ffffff" border="1">
				<tr valign="middle">
					<td valign="middle">
						<table cellSpacing="0" cellPadding="0" width="730" bgColor="#ffffff" border="0">
							<tr>
								<td colspan="3" style="PADDING-LEFT: 20px; FONT-WEIGHT: bold; FONT-SIZE: 24px; PADDING-BOTTOM: 5px; CURSOR: default; COLOR: #ffffff; PADDING-TOP: 12px; FONT-FAMILY: Verdana, Arial, Helvetica"
									vAlign="middle" bgColor="#95b7f3" height="75" align="center"><FONT face="Tahoma"> <span>
											Sistema de Activaciones Prepago</span> -&nbsp;SISACT PREPAGO</FONT>
								</td>
							</tr>
							<tr bgColor="#ffffff">
								<td colspan="3"><img height="3" width="1" border="0"></td>
							</tr>
							<tr bgColor="#95b7f3">
								<td colspan="3"><img height="3" width="1" border="0"></td>
							</tr>
							<tr bgColor="#ffffff">
								<td colspan="3"><img height="3" width="1"></td>
							</tr>
							<tr>
								<td colspan="3" bgColor="#ffffff">&nbsp;</td>
							</tr>
							<tr valign="top">
								<td class="login01" valign="top" align="center" width="133" style="WIDTH: 133px">
                                    <IMG src="imagenes/login/hm_tim.gif"><br>
									América Móvil Peru</td>
								<td bgColor="#ffffff" height="170" valign="middle" style="WIDTH: 330px">
									<table width="100%" align="center" cellpadding="3">
										<tr>
											<td width="30%" class="login01" id="lblOficina" align="right"><b>Usuario:</b></td>
											<td width="70%">
												<P><asp:label cssclass="login01" id="lblUsuario" runat="server" Font-Bold="True"></asp:label></P>
											</td>
										</tr>
										<tr>
											<td align="right" class="login01"><b>Area:</b></td>
											<td>
												<asp:Label cssclass="login01" id="lblArea" runat="server" Font-Bold="True"></asp:Label></td>
										</tr>
										<TR>
											<TD class="login01" align="left" colSpan="2">
												<asp:Label id="lblMensaje" runat="server" Font-Bold="True" cssclass="login01" ForeColor="Red"></asp:Label></TD>
										</TR>
									</table>
								</td>
								<td valign="middle" align="center"><IMG src="imagenes/login/LogoAM.gif"></td>
							</tr>
							<tr>
								<td colspan="3" align="center">
                                    <asp:imagebutton id="imgIngresar" runat="server" 
                                        ImageUrl="imagenes/login/hm_ingreso.gif" onclick="imgIngresar_Click"></asp:imagebutton>&nbsp;</td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
			
			<p style="DISPLAY:none">
				<asp:TextBox id="txtCodUsuario" runat="server"></asp:TextBox></p>
		</form>
	</body>
</html>
 
 
 
 
