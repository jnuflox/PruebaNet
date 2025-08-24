<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sisact_pop_comentario.aspx.cs"
    Inherits="Claro.SISACT.Web.Paginas.consultas.sisact_pop_comentario" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Registro Comentarios</title>
    <link href="../../Estilos/general.css" type="text/css" rel="stylesheet" />
    <script src="../../Scripts/funciones_sec.js" type="text/javascript"></script>
    <script type="text/javascript" language="JavaScript">
        function grabar() {
            if (!validarControl('txtComentario', '', 'El comentario es un dato obligatorio.')) return false;

            frmPrincipal.submit();
        }

        function cerrar() {
            window.close();
        }

        function inicio() {
            var mensaje = getValue('hidMensaje');
            if (getValue('hidProceso') != 'OK') {
                alert("No se pudo grabar el comentario");
            } else {
                alert("Su comentario se cargo satisfactoriamente");
                window.opener.document.getElementById('btnComentario').click();
                window.close();
            }
        }
    </script>
</head>
<body>
    <form id="frmPrincipal" runat="server">
    <table style="z-index: 101; left: 0px; position: absolute; top: 1px" width="100%">
        <tr>
            <td class="Arial10B">
                &nbsp;Comentario :
            </td>
        </tr>
        <tr>
            <td>
                <textarea class="inputTextArea" id="txtComentario" onblur="return f_ValidaTextArea(this, 300);"
                    style="width: 624px; height: 43px" name="txtComentario" rows="2" cols="75" runat="server"></textarea>
            </td>
        </tr>
        <tr>
            <td>
                <input class="Boton" id="btnGrabar" style="width: 100px; cursor: hand; height: 19px"
                    onclick="grabar()" type="button" value="Grabar" />&nbsp;<input class="Boton" id="btnCerrar"
                        style="width: 100px; cursor: hand; height: 19px" onclick="cerrar()" type="button"
                        value="Cancelar" />
            </td>
        </tr>
        <tr>
            <td>
                <input id="hidProceso" type="hidden" runat="server" /><input id="hidNroSEC" type="hidden"
                    runat="server" />
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
