<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sisact_pop_plan_combo.aspx.cs"
    Inherits="Claro.SISACT.Web.Paginas.consultas.sisact_pop_plan_combo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/tr/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Asociación de Bolsa Compartida</title>
    <link href="../../Estilos/general.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" language='javascript' src='../../Scripts/funciones_sec.js'></script>
        <script language="javascript" type="text/javascript" src="../../Scripts/jquery-1.9.1.js"></script>
        <script language="javascript" type="text/javascript" src="../../Scripts/security.js"></script>
    <script language="javascript" type="text/javascript">

        function window.confirm(str) {
            execScript('n = msgbox("' + str + '","4132")', "vbscript");
            return (n == 6);
        }

        function aceptar() {
            var strPlan = getValue('hidPlanActualizado');
            if (strPlan.length > 0)
                var arrPlan = strPlan.split('|');
            else {
                window.returnValue = '';
                window.close();
                return;
            }

            var strPlanActualizado = arrPlan[1];
            var strBolsaSeleccionada = arrPlan[3];

            if (strBolsaSeleccionada.length > 0) {
                var str = 'Ha seleccionado la ' + strBolsaSeleccionada + ', se actualizará el plan a: ' + strPlanActualizado + '. ¿Desea confirmar su selección?';
                if (!window.confirm(str))
                    return false;
                else {
                    window.returnValue = strPlan;
                    window.close();
                    return;
                }
            }
            else {
                window.returnValue = '';
                window.close();
            }
        }

        function cancelar() {
            window.returnValue = '';
            window.close();
        }

        function seleccionarBolsa(valor) {
            setValue('hidPlanActualizado', '');

            if (valor.split('|')[2] > 0) {
                setValue('hidPlanActualizado', valor);
            }
        }
			
    </script>
</head>
<body>
    <form id="frmPrincipal" runat="server">
    <table border="0" cellspacing="1" cellpadding="0" width="100%">
        <tr>
            <td class="Header" height="18" align="left">
                &nbsp;Asociar Servicio Bolsa
            </td>
        </tr>
    </table>
    <table class="Contenido" border="0" cellspacing="1" cellpadding="0" width="100%">
        <tr>
            <td class="Arial10B" align="left">
                Plan Seleccionado: &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:TextBox ID="txtDesPlanSeleccionado" Width="150px" runat="server" CssClass="clsInputDisable"
                    ReadOnly="True"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <img height="5" alt="0" src="../../Imagenes/spacer.gif" width="100%" border="0" />
            </td>
        </tr>
        <tr>
            <td class="Arial10B">
                <asp:RadioButtonList ID="rblLista" runat="server" RepeatLayout="Flow" Font-Size="8pt"
                    AutoPostBack="True">
                </asp:RadioButtonList>
                <asp:Literal ID="litLista" runat="server"></asp:Literal>
            </td>
        </tr>
    </table>
    <table width="100%">
        <tr>
            <td height="25" valign="bottom" align="center">
                <input style="width: 100px; cursor: hand; height: 19px" id="btnAceptar" class="Boton"
                    onclick="aceptar();" value="Aceptar" type="button" />
                <input style="width: 100px; cursor: hand; height: 19px" id="btnCancelar" class="Boton"
                    onclick="cancelar();" value="Cancelar" type="button" />
            </td>
        </tr>
    </table>
    <input type="hidden" id="hidPlanActualizado" />
    </form>
</body>
</html>
