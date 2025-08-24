<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sisact_ifr_CentroPoblado.aspx.cs"
    Inherits="Claro.SISACT.Web.Paginas.frames.sisact_ifr_CentroPoblado" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script language="javascript" type="text/javascript" src="../../Scripts/funciones_sec.js"></script>
    <script language="javascript" type="text/javascript" src="../../Scripts/jquery-1.9.1.js" ></script>
    <script language="javascript" type="text/javascript" src="../../Scripts/security.js"></script>    
    <script type="text/javascript" language="javascript">

        function inicio() {
            switch (document.getElementById('hidNResponseValue').value) {
                case "ExtraerDatos":
                    if (window.parent) {
                        setValue('hidCodSerie', window.parent.document.getElementById('txtImei').value);
                    }
                    setValue('hidRequest', 'BuscarSerie');
                    document.frmPrincipal.submit();
                    return;
                    break;
                case "RetornarDatos": // Despues de obtener la respuesta hay que volver a padre
                    if (window.parent) {
                        window.parent.LlenarDatosCombo(document.getElementById('hidDatosRetorno').value, document.getElementById('hidNResponseValue').value);
                    }
                    break;
            }
        }

    </script>
</head>
<body onload="inicio()">
    <form id="frmPrincipal" method="post" runat="server">
    <input id="hidCodSerie" type="hidden" name="hidCodImei" runat="server" style="width: 16px;
        height: 22px" size="1" />
    <input id="hidDatosRetorno" type="hidden" name="hidDatosRetorno" runat="server" style="width: 16px;
        height: 22px" size="1" />
    <input id="hidNMsgValue" type="hidden" name="hidNMsgValue" runat="server" style="width: 16px;
        height: 22px" size="1" />
    <input id="hidRequest" type="hidden" name="hidRequest" runat="server" style="width: 16px;
        height: 22px" size="1" />
    <input id="hidNResponseValue" type="hidden" name="hidNResponseValue" runat="server" style="width: 16px;
        height: 22px" size="1" />
    </form>
</body>
</html>
