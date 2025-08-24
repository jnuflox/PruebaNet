<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sisact_pool_evaluador_consumer.aspx.cs"
    Inherits="Claro.SISACT.Web.Paginas.Evaluacion_cons.sisact_pool_evaluador_consumer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Pool Evaluador Persona</title>
    <link href="../../Estilos/general.css" type="text/css" rel="stylesheet" />
    <link href="../../Estilos/calendar-blue.css" type="text/css" rel="stylesheet" />
    <script language="javascript" src='<%= ResolveClientUrl("~/Scripts/jquery-1.9.1.js") %>'
        type="text/javascript"></script>
    <script src="../../Scripts/funciones_sec.js" type="text/javascript"></script>
    <script src="../../Scripts/calendar/calendar.js" type="text/javascript"></script>
    <script src="../../Scripts/calendar/calendar_es.js" type="text/javascript"></script>
    <script src="../../Scripts/calendar/calendar_setup.js" type="text/javascript"></script>
    <script src="../../Scripts/calendar/calendario_call.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">

        $(document).ready(function () {

            $("#btnConsultar").click(function () {
                consultar();
            });

            $("#btnLimpiar").click(function () {
                limpiar();
            });

            $("#btnNext").click(function () {
                asignarSECAutomatica();
            });

        });

        var callbacks = {

            verificarUsuario: function (objResponse) {

                if (objResponse.Error) {
                    alert(objResponse.Mensaje);
                    formatoPool('#E9EBEE');
                }
                else {
                    var flgProceso = $('#hidProceso').val();
                    var nroSEC = $('#hidNroSEC').val();
                    var strIdProd = $('#hidcodProd').val();
                    if (flgProceso == 'E') {
                        evaluar(nroSEC, strIdProd);
                    } else {
                        rechazar(nroSEC);
                    }
                }
            },

            asignarSECAutomatica: function (objResponse) {
                if (objResponse.Error) {
                    alert(objResponse.Mensaje);
                }
                else {
                    var nroSEC = objResponse.Cadena;
                    var strIdProd = $('#hidcodProd').val();
                    evaluar(nroSEC, strIdProd);
                }
            }
        }

        function consultar() {
            var $txtFechaInicio = $('#txtFechaInicio');
            var $txtFechaFin = $('#txtFechaFin');

            if (!compararFecha($txtFechaInicio.val(), $txtFechaFin.val(), '0')) {
                alert('La fecha fin debe ser mayor o igual a la fecha de inicio');
                return false;
            }

            var dias = diferenciaFechas($txtFechaInicio.val(), $txtFechaFin.val());
            var rangoDias = $('#hidRangoFecha').val();
            if (parseInt(dias, 10) > parseInt(rangoDias, 10)) {
                alert('Solo está permitido la consulta de SECs con antigüedad menos a ' + rangoDias + ' días');
                $txtFechaInicio.val($('#hidFechaInicioMaximo').val());
                return false;
            }
            return true;
        }

        function procesar(control, flgProceso) {
            var blnOK;
            var arrCadena = control.id.split('_');
            var id = arrCadena[arrCadena.length - 1];
            var nroSEC = getValueHTML('gvPool_lblNroSEC_' + id);
            var codProd = getValue('gvPool_hidIdProd_' + id);

            $('#hidFilaSeleccionado').val(control.id);

            formatoPool('#6699ff');

            if (flgProceso == 'E')
                blnOK = (confirm('¿Esta seguro de Evaluar la solicitud ' + nroSEC + '?'));
            else
                blnOK = (confirm('¿Esta seguro de RECHAZAR la solicitud ' + nroSEC + '?'));

            if (blnOK) {
                var nombre = getValueHTML('gvPool_lblNombres_' + id);
                var nroDoc = getValueHTML('gvPool_lblNroDocumento_' + id);

                $('#hidNombres').val(nombre);
                $('#hidNroDoc').val(nroDoc);
                $('#hidNroSEC').val(nroSEC);
                $('#hidProceso').val(flgProceso);
                $('#hidcodProd').val(codProd);
                PageMethods.verificarUsuario($('#hidNroSEC').val(), $('#hidUsuarioRed').val(), callbacks.verificarUsuario);
            }
            else
                formatoPool('#E9EBEE');
        }

        function asignarSECAutomatica() {
            PageMethods.asignarSECAutomatica($('#hidUsuarioRed').val(), callbacks.asignarSECAutomatica);
        }

        function formatoPool(color) {
            document.getElementById($('#hidFilaSeleccionado').val()).parentElement.parentElement.style.backgroundColor = color;
        }

        function evaluar(nroSEC, strIdProd) {
            var url = '../evaluacion_cons/sisact_reporte_evaluacion_persona.aspx?';
            url += 'nroSEC=' + nroSEC + '&flgSoloConsulta=N' + '&flgOrigenPagina=P';
            url += '&strIdProd=' + strIdProd;
            window.location.href = url;
        }

        function rechazar(nroSEC) {
            var url = '../consultas/sisact_pop_motivo_evaluar_consumer.aspx?';
            url += 'nroSEC=' + nroSEC + '&nroDocumento=' + $('#hidNroDoc').val() + '&nombres=' + $('#hidNombres').val();
            abrirVentana(url, "", '700', '220', '_blank', true);
        }

        function verDetalle(control) {
            var arrCadena = control.id.split('_');
            var id = arrCadena[arrCadena.length - 1];
            var codProd = getValue('gvPool_hidIdProd_' + id);
            var nroSEC = getValueHTML('gvPool_lblNroSEC_' + id);
            var url = '../evaluacion_cons/sisact_reporte_evaluacion_persona.aspx?';
            url += 'nroSEC=' + nroSEC + '&flgSoloConsulta=S' + '&flgOrigenPagina=P';
            url += '&strIdProd=' + codProd;
            abrirVentana(url, "", '800', '600', '_blank', true);
        }

        function PageLoad() {
            if (!window.Sys.WebForms.PageRequestManager.getInstance().get_isInAsyncPostBack()) {
                window.Sys.WebForms.PageRequestManager.getInstance().add_initializeRequest(AjaxBegin);
                window.Sys.WebForms.PageRequestManager.getInstance().add_pageLoading(RecogerDatosHandler);
            }
        }

        function AjaxBegin() {
            document.getElementById('hidFechaInicio').value = document.getElementById('txtFechaInicio').value;
            document.getElementById('hidFechaFin').value = document.getElementById('txtFechaFin').value;
        }

        function RecogerDatosHandler(sender, args) {
            var datos = args.get_dataItems();
            $get("lblMensaje").innerHTML = datos["lblMensaje"];
        }

    </script>
</head>
<body onkeydown="cancelarBackSpace();" onload="PageLoad();" style="margin: 0px;">
    <form id="frmPrincipal" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" EnablePartialRendering="true">
    </asp:ScriptManager>
    <table cellspacing="1" cellpadding="0" width="100%" border="0">
        <tr>
            <td class="Header" align="left" height="20">
                &nbsp;Pool&nbsp;de Evaluaciones
            </td>
        </tr>
        <tr>
            <td>
                <table class="Contenido" cellspacing="1" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="Arial10B" width="95">
                            &nbsp;Fecha Reg Inicio
                        </td>
                        <td width="120">
                            <asp:TextBox ID="txtFechaInicio" runat="server" Width="75px" CssClass="clsInputEnable"
                                ReadOnly="True"></asp:TextBox>
                            <img alt="" id="btnFechaInicio" style="border-right: 0px; border-top: 0px; cursor: pointer;
                                border-bottom: 0px; order-left: 0px" src="../../Imagenes/calendario.gif" border="0" />
                            <script type="text/javascript">
                                Calendar.setup(
                                            {
                                                inputField: "txtFechaInicio",      // id of the input field
                                                ifFormat: "%d/%m/%Y",       // format of the input field                                                        
                                                button: "btnFechaInicio",   // trigger for the calendar (button ID)
                                                singleClick: true,           // double-click mode
                                                step: 1                // show all years in drop-down boxes (instead of every other year as default)
                                            }
                                        );
                            </script>
                        </td>
                        <td class="Arial10B" width="80">
                            Fecha Reg Fin
                        </td>
                        <td width="150">
                            <asp:TextBox ID="txtFechaFin" runat="server" Width="75px" CssClass="clsInputEnable"
                                ReadOnly="True"></asp:TextBox>
                            <img alt="" id="btnFechaFin" style="border-right: 0px; border-top: 0px; border-left: 0px;
                                cursor: pointer; border-bottom: 0px" src="../../Imagenes/calendario.gif" border="0" />
                            <script type="text/javascript">
                                Calendar.setup(
                                            {
                                                inputField: "txtFechaFin",      // id of the input field
                                                ifFormat: "%d/%m/%Y",       // format of the input field                                                        
                                                button: "btnFechaFin",   // trigger for the calendar (button ID)
                                                singleClick: true,           // double-click mode
                                                step: 1                // show all years in drop-down boxes (instead of every other year as default)
                                            }
                                        );
                            </script>
                        </td>
                        <td class="Arial10B">
                            <asp:Button ID="btnConsultar" runat="server" CssClass="Boton" onmouseover="this.className='BotonResaltado';"
                                Style="width: 100px; cursor: hand; height: 19px" onmouseout="this.className='Boton';"
                                Text="Consultar" OnClick="btnConsultar_Click" />
                            <input class="Boton" id="btnLimpiar" onmouseover="this.className='BotonResaltado';"
                                style="width: 100px; cursor: hand; height: 19px; display: none" onmouseout="this.className='Boton';"
                                type="button" value="Limpiar" name="btnLimpiar" />
                            <input class="Boton" id="btnNext" onmouseover="this.className='BotonResaltado';"
                                style="width: 100px; cursor: hand; height: 19px" onmouseout="this.className='Boton';"
                                type="button" value="NEXT" name="btnNext" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="trResultado">
            <td>
                <div style="overflow: auto; width: 100%; align: rigth">
                    <asp:UpdatePanel ID="udpGrilla" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <input type="hidden" id="hidFechaInicio" runat="server" />
                            <input type="hidden" id="hidFechaFin" runat="server" />
                            <asp:GridView ID="gvPool" runat="server" Width="1380px" AutoGenerateColumns="False"
                                ShowHeaderWhenEmpty="true" OnRowDataBound="gvPool_RowDataBound" CellSpacing="0"
                                CellPadding="0">
                                <HeaderStyle HorizontalAlign="Center" Wrap="False" CssClass="TablaTitulos" />
                                <RowStyle Wrap="False" HorizontalAlign="Center" CssClass="Arial10B" BackColor="#E9EBEE" />
                                <AlternatingRowStyle Wrap="False" HorizontalAlign="Center" CssClass="Arial10B" BackColor="#DDDEE2" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Acci&#243;n">
                                        <HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:Image ToolTip="Evaluar" ID="imgEvaluar" Style="cursor: hand" ImageUrl="../../Imagenes/activar.gif"
                                                runat="server" onclick='procesar(this, "E")' Visible="false" />
                                            <asp:Image ToolTip="Rechazar" ID="imgRechazar" Style="cursor: hand" ImageUrl="../../Imagenes/rechazar.gif"
                                                runat="server" onclick='procesar(this, "R")' />
                                             <img runat="server" alt="Ver Detalle" id="imgDetalle" style="cursor: hand" src="../../Imagenes/ico_lupa.gif"
                                                onclick='verDetalle(this)' />
                                                                                          
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="SEC">
                                        <HeaderStyle Width="60px"></HeaderStyle>
                                        <ItemStyle Width="60px"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblNroSEC" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "SOLIN_CODIGO") %>'>
                                            </asp:Label>
                                           <input id="hidIdProd" runat="server" type="hidden" value='<%# DataBinder.Eval(Container.DataItem, "PRDC_CODIGO") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="SOLIV_DES_EST" HeaderText="ESTADO" ControlStyle-Width="180px">
                                        <HeaderStyle Width="180px"></HeaderStyle>
                                        <ItemStyle Wrap="false" Width="180px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="FLAG_PORTABILIDAD" HeaderText="TIPO OPERACIÓN">
                                        <HeaderStyle Width="140px"></HeaderStyle>
                                        <ItemStyle Wrap="false" Width="140px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SOLID_FEC_REG" HeaderText="FECHA">
                                        <HeaderStyle HorizontalAlign="Center" Width="130px"></HeaderStyle>
                                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="130px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TDOCV_DESCRIPCION" HeaderText="TIPO DOC">
                                        <HeaderStyle Wrap="True" HorizontalAlign="Center" Width="50px"></HeaderStyle>
                                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="50px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="NRO DOC">
                                        <HeaderStyle Wrap="true" HorizontalAlign="Center" Width="60px"></HeaderStyle>
                                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="60px"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblNroDocumento" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CLIEC_NUM_DOC") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="NOMBRE/RAZON SOCIAL">
                                        <HeaderStyle Wrap="true" HorizontalAlign="Center" Width="220px"></HeaderStyle>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="220px"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblNombres" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CLIEV_NOMBRE") + " " + DataBinder.Eval(Container.DataItem, "CLIEV_APE_PAT") + " " + DataBinder.Eval(Container.DataItem, "CLIEV_APE_MAT" )%>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="SOLIV_DES_OFI_VEN" HeaderText="PDV">
                                        <HeaderStyle HorizontalAlign="Center" Width="120px"></HeaderStyle>
                                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="120px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TOFV_DESCRIPCION" HeaderText="TIPO PRODUCTO">
                                        <HeaderStyle Wrap="True" HorizontalAlign="Center" Width="145px"></HeaderStyle>
                                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="145px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TCESC_DESCRIPCION" HeaderText="CASO ESPECIAL">
                                        <HeaderStyle Wrap="True" HorizontalAlign="Center" Width="120px"></HeaderStyle>
                                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="120px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SOLIV_RES_EXP_CON" HeaderText="RIESGO">
                                        <HeaderStyle Wrap="True" HorizontalAlign="Center" Width="50px"></HeaderStyle>
                                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="50px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SOLIN_CAN_LIN" HeaderText="CANT. LINEAS">
                                        <HeaderStyle Wrap="True" HorizontalAlign="Center" Width="50px"></HeaderStyle>
                                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="50px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TCARV_DESCRIPCION" HeaderText="Tipo Garantia" Visible="False">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SOLIN_IMP_DG" HeaderText="Importe" Visible="False">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SOLIV_MOTIVO_RECHAZO" HeaderText="Motivo Rechazo" Visible="False">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TCLIC_CODIGO" HeaderText="TIPO CLIENTE" Visible="False">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TOPEN_CODIGO" HeaderText="TOPEN_CODIGO" Visible="False">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnConsultar" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblMensaje" runat="server" CssClass="Arial10BRed"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <table id="tblLeyenda" class="Contenido" cellspacing="2" cellpadding="0" border="0"
                    width="300">
                    <tr>
                        <td colspan="2">
                            &nbsp;<asp:Label ID="lblLeyenda" runat="server" CssClass="Arial10B" Font-Underline="True">Leyenda:</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="Arial10B" width="250">
                            &nbsp;Para EVALUAR siguiente SEC :
                        </td>
                        <td>
                            <img alt="Evaluar siguiente SEC" src="../../Imagenes/next.gif" border="0" />
                        </td>
                    </tr>
                    <tr>
                        <td class="Arial10B" width="250">
                            &nbsp;Para EVALUAR presione el icono :
                        </td>
                        <td>
                            <img alt="Evaluar" src="../../Imagenes/activar.gif" border="0" />
                        </td>
                    </tr>
                    <tr>
                        <td class="Arial10B" width="250">
                            &nbsp;Para RECHAZAR presione el icono :
                        </td>
                        <td>
                            <img alt="Rechazar" src="../../Imagenes/rechazar.gif" border="0" />
                        </td>
                    </tr>
                    <tr>
                        <td class="Arial10B" width="250">
                            &nbsp;Para ver el DETALLE presione el icono :
                        </td>
                        <td>
                            <img alt="Ver Detalle" src="../../Imagenes/ico_lupa.gif" border="0" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <input id="hidUsuarioRed" type="hidden" runat="server" />
    <input id="hidNombres" type="hidden" runat="server" />
    <input id="hidNroDoc" type="hidden" runat="server" />
    <input id="hidNroSEC" type="hidden" runat="server" />
    <input id="hidProceso" type="hidden" runat="server" />
    <input id="hidFlgVerEvaluarSEC" type="hidden" runat="server" />
    <input id="hidRangoFecha" type="hidden" runat="server" />
    <input id="hidFechaInicioMaximo" type="hidden" runat="server" />
    <input id="hidFilaSeleccionado" type="hidden" runat="server" />
    <input id="hidcodProd" type="hidden" runat="server" />
    
    </form>
</body>
</html>
