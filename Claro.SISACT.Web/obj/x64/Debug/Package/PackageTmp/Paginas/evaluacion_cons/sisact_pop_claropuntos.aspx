<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sisact_pop_claropuntos.aspx.cs" Inherits="Claro.SISACT.Web.Paginas.Venta.sisact_pop_claropuntos" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../Estilos/Estilo_General.css" rel="stylesheet" type="text/css" />
    <link href="../../Estilos/General.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/funciones_generales.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.9.1.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">

        $(document).ready(function () {
            var tipo;
            tipo = '<%=Request.QueryString["tipo"]%>';


            if (tipo == 1) {
                $('#tblBotonesAccion').hide();
                $('#tblBotonSalir').show();
                $('#tblDatos2').hide();
            }
            else if (tipo == 2) {
                $('#tblBotonesAccion').show();
                $('#tblBotonSalir').hide();
                $('#tblDatos2').show();
            }
        });

          function f_Aceptar() {
              alert("Aún no se termina, faltar validar.");
              window.close();
          }
  
          function f_Cancelar() {
              if (confirm("Esta seguro de cerrar?"))
                  window.close();
          }
          function f_Salir() {
              window.close();
          }
          function abrirModal() {
              alert("Aún no se termina, disculpe la molestía.");
          }

    </script>
</head>
<body>
    <form id="frmPrincipal" runat="server">
    <table width="65%" cellspacing="1" cellpadding="0" border="0">
      <tr>
        <td>
         <table id="tblDatos1" class="Contenido" width="100%" cellpadding="0" border="0">
        <tr>
         <td class="region" align="left" height="20">
         &nbsp;Claro Club - Datos de Claro Puntos
         </td>
        </tr>
        <tr>
         <td>
          <table id="tblSubDatos1" width="100%" class="Contenido" cellpadding="0" border="0">
           <tr>
             <td>
              <table width="100%">
              <tr>
               <td style="width:100px">Campaña</td>
               <td style="width:300px"><asp:TextBox ID="txtCampania" runat="server" ForeColor="#001B8C" CssClass="clsInputDisable" ReadOnly="true" style="width: 250px;" /></td>
               <td style="width:50px">Vigencia</td>
               <td style="width:100px"><asp:TextBox ID="txtInicio" runat="server" ForeColor="#001B8C" CssClass="clsInputDisable" ReadOnly="true" style="width: 80px;"/></td>
               <td style="width:30px">al</td>
               <td style="width:100px"><asp:TextBox ID="txtFin" runat="server" ForeColor="#001B8C" CssClass="clsInputDisable" ReadOnly="true" style="width: 80px;"/></td>
               <td style="width:50px">Segmento</td>
               <td ><asp:TextBox ID="txtSegmento" runat="server" ForeColor="#001B8C" CssClass="clsInputDisable" ReadOnly="true" style="width: 50px;"/></td>
               </tr>
              </table>
             </td>
            </tr>
           <tr>
             <td>
              <table width="100%">
               <tr>
                <td style="width:180px;">Saldo de puntos en Postpago</td>
                <td style="width:150px;">
                <asp:TextBox ID="txtPuntos" runat="server" ForeColor="#001B8C" CssClass="clsInputDisable" ReadOnly="true" style="width: 80px;"/></td>
                <td style="width:150px;">Ver Detalle de Descto. </td>
                <td ><input type="button" id="btnDetalle" value="..." class="Boton" onclick="abrirModal();" style="width:30px;"/></td>
               </tr>
              </table>
             </td>
           </tr>
           <tr>
              <td>
             <table id="tblGrilla" cellpadding="0" border="0" >
              <tr>
                <td style="text-align: center; background-color: #5991bb; color: white; font-weight: bold;
                white-space: nowrap;" >
                Detalle de Descuento
                </td>
              </tr>
              <tr>
               <td>
                &nbsp;
               </td>
              </tr>
              <tr>
              <td style="white-space: nowrap">
                <div class="scroll">
                 <asp:DataGrid ID="dgDescuento" runat="server" AutoGenerateColumns="False" ShowHeader="True">
                   <AlternatingItemStyle BackColor="#DDDEE2"></AlternatingItemStyle>
                        <HeaderStyle Wrap="False" HorizontalAlign="Center" CssClass="TablaTitulos"></HeaderStyle>
                        <ItemStyle CssClass="Arial10B" BackColor="#E9EBEE" HorizontalAlign="Center"></ItemStyle>
                        <Columns>
                            <asp:BoundColumn DataField="NORMAL" HeaderText="Normal" DataFormatString="{0:N2}">
                                <ItemStyle Width="87px" HorizontalAlign="RIGHT" BackColor="White" Font-Bold="False"
                                    CssClass="celda"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="PROMOCIONAL" HeaderText="Promocional" DataFormatString="{0:N2}">
                                <ItemStyle Width="87px" HorizontalAlign="RIGHT" BackColor="White" Font-Bold="False"
                                    CssClass="celda"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="TOTAL" HeaderText="Total" DataFormatString="{0:N2}">
                                <ItemStyle Width="87px" HorizontalAlign="RIGHT" BackColor="White" Font-Bold="False"
                                    CssClass="celda"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="TIPO_DE_DSCTO" HeaderText="Tipo de Dscto.">
                                <ItemStyle Width="140px" HorizontalAlign="Center" BackColor="White" Font-Bold="False">
                                </ItemStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="FACTOR" HeaderText="Factor">
                                <ItemStyle Width="87px" HorizontalAlign="Center" BackColor="White" Font-Bold="False">
                                </ItemStyle>
                            </asp:BoundColumn>
                        </Columns>
                  </asp:DataGrid>
                 </div>
              </td>               
             </tr>
             </table>
              </td>
             </tr>
           <tr><td></td></tr>
           <tr>
             <td>
              <table>
               <tr>
                <td style="width:150px">Saldo actual de Puntos</td>
                <td style="width:120px"><asp:TextBox ID="txtPuntosActual" runat="server" readonly="true" ForeColor="#001B8C" CssClass="clsInputDisable" style="width: 80px;"/></td>
                <td style="width:150px">Soles de Descuento</td>
                <td style="width:120px"><asp:TextBox  ID="txtDescuentoActual" runat="server" readonly="true" ForeColor="#001B8C" CssClass="clsInputDisable" style="width: 80px;"/>              
                </td>
               </tr>
              </table>
             </td>
           </tr> 
           <tr><td></td></tr>          
           </table>
         </td>
        </tr> 
         </table>
        </td>
      </tr>
      <tr>
       <td>
         <table id="tblDatos2" class="Contenido" width="100%" cellpadding="0" border="0" >
        <tr>
         <td class="region" align="left" height="20">
         &nbsp;Claro puntos a utilizar y descuento en soles
         </td>
        </tr>
        <tr>
         <td>
         <table id="tblSubDatos2"  cellpadding="0" border="0" width="100%">
           <tr><td colspan="4"></td></tr>
           <tr>
            <td style="width:150px">Claro puntos a utilizar</td>
            <td style="width:120px"><asp:TextBox ID="txtPuntosUtilizar" runat="server" readonly="false" ForeColor="#001B8C" CssClass="clsInputEnable" style="width: 80px;"/></td>
            <td style="width:150px">Soles de Descuentos</td>
            <td ><asp:TextBox ID="txtDescuentoUtilizar" runat="server" readonly="true" ForeColor="#001B8C" CssClass="clsInputDisable" style="width: 80px;"/></td>
           </tr>
           <tr><td colspan="4"></td></tr>
         </table>
         </td>
        </tr> 
         </table>
       </td>
      </tr>
      <tr>
       <td>
         <table id="tblBotonesAccion" class="Contenido" width="100%" cellpadding="0" border="0" style="display: none">
          <tr>
           <td align="center">
              <input class="Boton" type="button" id="btnAceptar" name="btnAceptar" onclick="f_Aceptar();"
               style="width: 100px; cursor: hand; height: 19px" value="Aceptar" />&nbsp;
              <input class="Boton" type="button" id="btnCancelar" name="btnCancelar" onclick="f_Cancelar();"
              style="width: 100px; cursor: hand; height: 19px" value="Cancelar" />
           </td>
          </tr>
         </table>
         <table id="tblBotonSalir" class="Contenido"  width="100%" cellpadding="0" border="0" >
          <tr>
           <td align="center">
             <input class="Boton" type="button" id="btnCancelar2" name="btnCancelar" onclick="f_Salir();"
             style="width: 100px; cursor: hand; height: 19px" value="Salir" />
           </td>
          </tr>
         </table>
       </td>
      </tr>
     </table>
     <input id="hidFactorClaroClub" type="hidden" name="hidFactorClaroClub" value="" runat="server" />
     <input id="hidCodCampanaCC" type="hidden" name="hidCodCampanaCC" value="" runat="server" />
     <input id="hidDetalleDescuento" type="hidden" name="hidDetalleDescuento" value="" runat="server" />
    </form>
</body>
</html>
