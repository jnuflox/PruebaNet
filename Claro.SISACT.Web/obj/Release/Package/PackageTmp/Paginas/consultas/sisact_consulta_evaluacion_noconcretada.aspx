<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sisact_consulta_evaluacion_noconcretada.aspx.cs" Inherits="Claro.SISACT.Web.Paginas.consultas.sisact_consulta_evaluacion_noconcretada" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="content-type" content="text/html; utf-8">
     <link href="../../Estilos/general.css" type="text/css" rel="stylesheet" />
     <script src="../../Scripts/funciones_sec.js" type="text/javascript"></script>
    <script type="text/javascript" language="JavaScript" src="../../Scripts/funciones_generales.js"></script>
    <script type="text/javascript" language="JavaScript" src="../../Scripts/Lib_FuncValidacion.js"></script>
    <script type="text/javascript" language="JavaScript" src="../../Scripts/JSUtil.js"></script>
    <script src="../../Scripts/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="../../Scripts/json2.js" type="text/javascript"></script>
            <script type="text/javascript" language="javascript" src="../../Scripts/security.js"></script>
    <script type="text/javascript" language="JavaScript" src="../../Scripts/KeySettings/KeySettings.js"></script>

    <script type="text/javascript">
        var constTipoDocumentoDNI = '<%= ConfigurationManager.AppSettings["constCodTipoDocumentoDNI"] %>';
        var constTipoDocumentoCE = '<%= ConfigurationManager.AppSettings["constCodTipoDocumentoCEX"] %>';
        var constTipoDocumentoRUC = '<%= ConfigurationManager.AppSettings["constCodTipoDocumentoRUC"] %>'; 
    </script>
    <script type="text/javascript" language="JavaScript" src="../../Scripts/Paginas/consultas/sisact_consulta_evaluacion_noconcretada.js"></script>
    <style type="text/css">
        #modalpopup_backgroundElement
        {
            background: #000;
            opacity: 0.5;
            filter: alpha(opacity=50);
        }
    </style>
</head>
<body>
    <form id="FrmPrincipal" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="True" EnableScriptGlobalization="true"
        EnableScriptLocalization="true" />
        <table cellspacing="0" cellpadding="0" width="950" border="0" id="contentEvaluacion">            
            <tr>
				<td class="Header" align="center" height="19">Consulta Evaluación Cliente</td>
			</tr>
            
            <tr>
                <td>
                    <table class="Contenido" cellspacing="1" cellpadding="10" width="100%" border="0">
                        <tr id="trConsultaCliente">
                            <td class="Arial10B" width="125">&nbsp;Tipo Documento:</td>
							<td width="170">
                                <asp:DropDownList ID="ddlTipoDocumentoEvaCli" runat="server" onchange="cambiarTipoDocumento(this.value,'1');" Width="130px" CssClass="clsSelectEnableC"></asp:DropDownList>
                            </td>
							<td class="Arial10B" width="100">Nro. Documento:</td>
							<td width="160">
                                <asp:TextBox runat="server" CssClass="clsInputEnabled" ID="txtNroDocumento" onkeyup="this.value = this.value.toUpperCase()" onkeypress="validaTxtDocGeneral('ddlTipoDocumentoEvaCli');" Width="130px"></asp:TextBox>
							</td>
                            <td>
                                <asp:Button CssClass="Boton" ID="btnConsultarEvaclie" Text="Consultar" runat="server" OnClick="btnConsultarEvaclie_Click" OnClientClick="return validacionDatos();"></asp:Button>
                                 &nbsp;<asp:Button CssClass="Boton" ID="btnLimpiarEvaclie" Text="Limpiar" runat="server" OnClick="btnLimpiarEvaclie_Click" OnClientClick="LimpiarControles();"></asp:Button>
							</td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
				<td><img height="4" alt="" src="../../Imagenes/spacer.gif" /></td>
			</tr>
            <tr id="trDetalleClienteEva" style="display: none">
				<td>
					<table cellspacing="0" cellpadding="0" width="100%" border="0">
						<tr>
							<td class="Header" align="left" height="19">&nbsp;Datos del Cliente</td>
						</tr>
					</table>
					<table class="Contenido" cellspacing="1" cellpadding="4" width="100%" border="0">
						<tr id="trDetalleDNI">
							<td class="Arial10B" width="125">&nbsp;Tipo Documento:</td>
							<td width="250">
                                <asp:TextBox runat="server" CssClass="clsInputDisabled" ID="txtTipoDocumentoEva" Width="240px" ></asp:TextBox>
                            </td>
							<td class="Arial10B" width="125">&nbsp;Nro Documento:</td>
							<td width="250">
                                <asp:TextBox runat="server" CssClass="clsInputDisabled" ID="txtNroDocumentoEva" Width="240px"></asp:TextBox>
                            </td>
						</tr>
                        <tr id="trDatosDNI" style="display: none">
                            <td class="Arial10B" style="width: 150px">
                                &nbsp;Nombres:
                            </td>
                            <td class="Arial10B" style="width: 150">
                                <asp:TextBox runat="server" CssClass="clsInputDisabled" ID="txtNombresEva" Width="240px" ></asp:TextBox>
                            </td>
                            <td class="Arial10B" style="width: 150px">
                                &nbsp;Apellidos:
                            </td>
                            <td class="Arial10B" style="width: 150">
                                <asp:TextBox runat="server" CssClass="clsInputDisabled" ID="txtApellidoEva" Width="240px" ></asp:TextBox>
                            </td>
                        </tr>
                        <tr id="trDatosRUC" style="display: none">
                            <td class="Arial10B" style="width: 150px">
                                &nbsp;Razon Social:
                            </td>
                            <td class="Arial10B" style="width: 150">
                                <asp:TextBox runat="server" CssClass="clsInputDisabled" ID="txtRazonSocial" Width="240px" ></asp:TextBox>
                            </td>                           
                        </tr>
                        <tr>
                            <td class="Arial10B" style="width: 150px">
                                &nbsp;Correo Electronico:
                            </td>
                            <td class="Arial10B" style="width: 150">
                                <asp:TextBox runat="server" CssClass="clsInputDisabled" ID="txtEmail" Width="240px" ></asp:TextBox>
                            </td>
                            <td class="Arial10B" style="width: 150px">
                                &nbsp;Nacionalidad:
                            </td>
                            <td class="Arial10B" style="width: 150">
                                <asp:TextBox runat="server" CssClass="clsInputDisabled" ID="txtNacionalidad" Width="240px"></asp:TextBox>
                            </td>
                        </tr>
					</table>
				</td>
			</tr>
            <tr>
				<td><img height="6" alt="" src="../../Imagenes/spacer.gif" /></td>
			</tr>
            <tr id="trCuadroInformativo" style="display: none">
            <td>            
                <table id="tbCuadroInformativo" style="border-color:#95b7f3" cellspacing="0" cellpadding="0"
                    width="100%" border="0">
                    <tr>
                        <td>
                            <div style="overflow: scroll; width: 950px; height: auto" >
                                <asp:GridView ID="dgEvaluacion"  runat="server" Width="100%" BorderColor="#CDE0F5"
                                        BorderStyle="Solid" CellPadding="5" BorderWidth="1px" AutoGenerateColumns="False"
                                        CssClass="TablePool" AllowPaging="true" ShowHeaderWhenEmpty="true" PageSize="10" 
                                        OnPageIndexChanging="dgEvaluacion_PageIndexChanging">
                                    <RowStyle HorizontalAlign="Center" CssClass="Arial10B" />
                                    <HeaderStyle Wrap="False" CssClass="TablaTitulos" HorizontalAlign="Center" />
                                    <AlternatingRowStyle Wrap="False" HorizontalAlign="Center" CssClass="Arial10B" BackColor="#DDDEE2" />
                                    <Columns>
                                        <asp:BoundField DataField="fechaReg" HeaderText="Fecha" />
                                        <asp:BoundField DataField="descOficina" HeaderText="Punto de venta" />
                                        <asp:BoundField DataField="tipoOperacion" HeaderText="Tipo operacion" />
                                        <asp:BoundField DataField="descCampania" HeaderText="Campaña"/>
                                        <asp:BoundField DataField="descEquipo" HeaderText="Equipo" />
                                        <asp:BoundField DataField="descPlan" HeaderText="Plan" />
                                        <asp:BoundField DataField="lcDisponible" HeaderText="LC disponible" DataFormatString="{0:C}"/>
                                        <asp:BoundField DataField="comportamiento" HeaderText="Comportamiento consolidado cliente" />
                                        <asp:BoundField DataField="modVenta" HeaderText="Modalidad de Venta" />
                                        <asp:BoundField DataField="resultadoEval" HeaderText="Resultado evaluación"/>
                                        <asp:BoundField DataField="linea" HeaderText="Número a portar" />  
                                        <asp:BoundField DataField="operadorCedente" HeaderText="Operador Cedente" />
                                        <asp:BoundField DataField="modalidad" HeaderText="Modalidad" />
                                        <asp:BoundField DataField="rangoLc" HeaderText="Rango de LC Disponible"/>
                                        <asp:BoundField DataField="nroCuotas" HeaderText="Cuotas" />                                   
                                        <asp:BoundField DataField="tipoOferta" HeaderText="Oferta" />
                                        <asp:BoundField DataField="casoEspecial" HeaderText="Caso especial" />
                                        <asp:BoundField DataField="plazo" HeaderText="Plazo" />
                                        <asp:BoundField DataField="familiaPlan" HeaderText="Familia plan" />
                                        <asp:BoundField DataField="cargoFijo" HeaderText="Cargo fijo" DataFormatString="{0:C}"/>
                                        <asp:BoundField DataField="tipoGarantia" HeaderText="Tipo de garantía" />
                                        <asp:BoundField DataField="importeRa" HeaderText="Importe RA" DataFormatString="{0:C}"/> 
                                        <asp:BoundField DataField="consultaPrevia" HeaderText="Estado Consulta Previa" />
                                        <asp:BoundField DataField="FechaCp" HeaderText="Fecha Consulta Previa" />
                                        <asp:BoundField DataField="deudaCp" HeaderText="Deuda Consulta Previa" DataFormatString="{0:0.00}"/>                                  
                                        <asp:TemplateField HeaderText="Servicios adicionales">
                                            <ItemTemplate>
                                                <img id="btnMostrarDetalle" onclick='javascript:mostrarDetalle("<%# DataBinder.Eval(Container.DataItem, "idEval")%>");'
                                                    src="../../Imagenes/ico_lupa.gif" style="cursor: hand;" alt="Mostrar Detalle">
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="tipoServicio" HeaderText="Tipo Servicio"/>                                                                     
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>                      		
                    </tr>
                </table>            
            </td>
        </tr>
        <tr>
				<td><img height="6" alt="" src="../../Imagenes/spacer.gif" /></td>
		</tr>

        </table>
        <asp:HyperLink id="hlOk" runat="server" ></asp:HyperLink>
        <asp:HyperLink id="hlControlID" runat="server" ></asp:HyperLink>
        <asp:HiddenField runat="server" ID="hidMostrar" />
        <cc1:modalpopupextender id="modalpopup" runat="server" 
	        cancelcontrolid="hlCancel" okcontrolid="hlOk" 
	        targetcontrolid="hlControlID" popupcontrolid="PanelCarga" 
	        drag="true" RepositionMode="RepositionOnWindowResize"
	        backgroundcssclass="ModalPopupBG">
        </cc1:modalpopupextender>
        
         <asp:Panel ID="PanelCarga" runat="server" Width="350px" style="display: none" >
                <div id="tablePanel">
            <table class="Contenido" cellpadding="4" bgcolor="white" width="100%">
                <thead>
                    <tr>
                        <th>
                            <label class="Arial10B">Servicios Adicionales</label>
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td class="Arial10B" style="width: 280px; background-color: white" valign="top">
                                                    <u>Servicios Contratados </u>
                        </td>
                    </tr>
                    <tr>
                        <td class="Arial10B" style="width: 280px; background-color: white" valign="top">
                                    <select class="clsSelectEnable" id="lbxServiciosAgregados1" style="width: 310px"size="5" name="lbxServiciosAgregados1">
                            </select>
                        </td>
                    </tr>
                            <tr>
                                <td class="Arial10B" style="background-color: white" align="center">
                                    &nbsp;<input class="Boton" id="btnCancelarServEva" type="button" value="Cerrar" style="width: 100px; cursor: hand; height: 19px;" name="btnCancelarServEva"/></td>
                    </tr>
                </tbody>
            </table>
                </div>
         </asp:Panel>
    </form>
</body>
</html>
