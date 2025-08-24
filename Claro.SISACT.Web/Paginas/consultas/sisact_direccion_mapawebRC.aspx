<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sisact_direccion_mapawebRC.aspx.cs"
    Inherits="Claro.SISACT.Web.Paginas.consultas.sisact_direccion_mapaweb.aspx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
        <script language="javascript" type="text/javascript" src="../../Scripts/jquery-1.9.1.js"></script>
        <script language="javascript" type="text/javascript" src="../../Scripts/security.js"></script>
    <script language="javascript" type="text/javascript">

        function cerrarVentana() {
            var codPlano = document.getElementById('<%=hidValorRetorno.ClientId %>').value;
            createCookie("pruebaMapaWeb", codPlano, 1);
            window.close();
        }

        function createCookie(name, value, days) {
            if (days) {
                var date = new Date();
                date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
                var expires = "; expires=" + date.toGMTString();
            } else {
                var expires = "";
            }
            document.cookie = name + "=" + value + expires + "; path=/";
        }
			
    </script>
</head>
<body onload="cerrarVentana()">
    <form id="form1" runat="server">
    <input type="hidden" id="hidValorRetorno" name="hidValorRetorno" runat="server" />
    </form>
</body>
</html>
