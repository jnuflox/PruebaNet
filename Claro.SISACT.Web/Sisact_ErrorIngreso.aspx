<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Sisact_ErrorIngreso.aspx.cs" Inherits="Claro.SISACT.Web.Sisact_ErrorIngreso" %>
<%@ Import Namespace="System.Configuration" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
            <script language="javascript" type="text/javascript" src="Scripts/jquery-1.9.1.js" ></script>
            <script type="text/javascript" language="javascript" src="Scripts/security.js"></script>
</head>
<body>
    <form id="Form1" method="post" runat="server">
			<table cellSpacing="0" cellPadding="0" width="665" align="center" border="0">
				<tr>
					<td>
						<table cellSpacing="0" style="Z-INDEX: 101; LEFT: 176px; POSITION: absolute; TOP: 9px"
							cellPadding="0" width="665" align="center" border="0">
							<tr>
								<td colSpan="3" height="94">&nbsp;</td>
							</tr>
							<tr>
								<td vAlign="top" align="center" width="25" bgColor="#ed1c24">
									<IMG height="25" src="imagenes/error/sq_top01.gif" width="25">
								</td>
								<td vAlign="top" align="center" width="621" bgColor="#ed1c24">&nbsp;</td>
								<td vAlign="top" align="center" width="26" bgColor="#ed1c24">
									<IMG height="25" src="imagenes/error/sq_top02.gif" width="26">
								</td>
							</tr>
						</table>
						<table style="Z-INDEX: 104; LEFT: 176px; POSITION: absolute; TOP: 448px" cellSpacing="0"
							cellPadding="0" width="665" align="center" border="0">
							<tr bgColor="#c2e8f5">
								<td vAlign="top" width="25">
									<IMG height="23" src="imagenes/error/sq_pie01.gif" width="25">
								</td>
								<td vAlign="top" bgColor="#c2e8f5">&nbsp;</td>
								<td vAlign="top" align="right" width="26">
									<IMG height="23" src="imagenes/error/sq_pie02.gif" width="26">
								</td>
							</tr>
						</table>
						<table style="Z-INDEX: 103; LEFT: 176px; POSITION: absolute; TOP: 128px" cellSpacing="0"
							cellPadding="0" width="665" align="center" border="0">
							<tr bgColor="#ed1c24">
								<td vAlign="top" width="25" height="90">&nbsp;</td>
								<td vAlign="top" width="129">
									<IMG height="78" src="imagenes/error/graf_ico_error.gif" width="128">
								</td>
								<td style="PADDING-RIGHT: 18px" vAlign="middle" align="right" width="485">&nbsp;</td>
								<td vAlign="top" width="26">&nbsp;</td>
							</tr>
						</table>
						<table style="Z-INDEX: 102; LEFT: 176px; POSITION: absolute; TOP: 216px" cellSpacing="0"
							cellPadding="0" width="665" align="center" border="0">
							<tr bgColor="#c2e8f5">
								<td vAlign="top" width="25" bgColor="#c2e8f5" height="235">&nbsp;</td>
								<td vAlign="bottom" width="257" bgColor="#c2e8f5">
									<IMG height="231" src="imagenes/error/img_marca.jpg" width="222">
								</td>
								<td style="PADDING-BOTTOM: 8px" vAlign="bottom" align="center" width="357" bgColor="#c2e8f5">
									<table height="204" cellSpacing="0" cellPadding="0" width="99%" border="0">
										<tr>
											<td colSpan="2" height="134">
												<strong>
													<font face="Arial" color="#ff0000" size="3">Acceso Incorrecto<br />
                                                Usuario sin permisos para esta Aplicación o Esta intentando acceder por una 
														ruta incorrecta, por favor ingresar por el 
														<br><a href='<%= ConfigurationManager.AppSettings["constPathPortal"] %>'>
														Portal de Aplicaciones</a> o por el link 
														<a href='<%= ConfigurationManager.AppSettings["constPathAplicacion"] %>'><%= ConfigurationManager.AppSettings["constPathAplicacion"]%></a><br>
													</font>
												</strong>
											</td>
										</tr>
										<tr>
											<td width="200" height="25">&nbsp;</td>
											<td align="left">
												<A href="http://eclaro"><IMG height="30" src="imagenes/error/btn_regresar_a.gif" width="110" border="0"></A>
											</td>
										</tr>
									</table>
								</td>
								<td vAlign="top" width="26">&nbsp;</td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
		</form>
</body>
</html>
 
 
 
 
