<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sisact_pop_upload_consumer.aspx.cs"
    Inherits="Claro.SISACT.Web.Paginas.documentos.sisact_pop_upload_consumer" %>

<!DOCTYPE html PUBLIC "-//W3C//Dtd XHTML 1.0 transitional//EN" "http://www.w3.org/tr/xhtml1/Dtd/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>SISACT - Carga de Archivos</title>
    <link href="../../Estilos/general.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" language="JavaScript" src="../../Scripts/funciones_sec.js"></script>
    <script type="text/javascript" language="JavaScript">

        function validarArchivo() {

            var archivo = document.getElementById('txtFile').value;
            if (archivo.indexOf('/**-/') != -1 || archivo == '') {
                alert('Debe ingresar una ruta de archivo válida');
                return false;
            }

            var extension = archivo.substring(archivo.lastIndexOf('.')).toUpperCase();
            if (!(extension == '.GIF' || extension == '.PDF' || extension == '.TIF' || extension == '.JPG' || extension == '.DOC' || extension == '.XLS' ||
                  extension == '.CSV' || extension == '.RTF' || extension == '.TXT' || extension == '.ZIP' || extension == '.RAR')) {
                alert('Debe Seleccionar un archivo con extensión .GIF, .PDF, .TIF, .JPG, .DOC, .XLS, .CSV, .RTF, .TXT, .ZIP, .RAR');
                return false;
            }

            if (!validaCaracteresArchivo(archivo)) {
                alert("El nombre del archivo contiene CARACTERES NO PERMITIDOS.");
                return false;
            }

            return true;
        }

        function enviarDatos() {
            if (validarArchivo()) {
                frmPrincipal.submit();
            }
        }

        function actualizarGrillaArchivo() {
            window.opener.document.getElementById('hidListaArchivos').value += '|' + getValue('hidArchivo');
            window.opener.document.getElementById('btnCargarArchivo').click();

            if (!confirm('Su archivo ha sido incluido satisfactoriamente, Desea anexar otro documento?')) {
                window.close();
            } else {
                document.getElementById('txtFile').value = '';
            }
        }
    </script>
</head>
<body>
    <form id="frmPrincipal" method="post" enctype="multipart/form-data" runat="server">
    <table cellspacing="2" cellpadding="0" align="left" border="0">
        <tr>
            <td align="center">
                <asp:FileUpload CssClass="Boton" ID="txtFile" runat="server" onkeypress="InhabilitarTecla();"
                    Width="400px" Height="22px" />
            </td>
        </tr>
        <tr>
            <td align="left">
                <label class="Arial10B" style="color: #ff0000">
                    CARACTERES INVALIDOS :&lt;&gt;()=?¿¡+{}[]*&amp;%Ññ'|°" (acentos)</label>
            </td>
        </tr>
        <tr>
            <td align="center">
                <input class="Boton" onclick="enviarDatos()" type="button" value="Aceptar" />
                <input class="Boton" onclick="window.close();" type="button" value="Cancelar" />
            </td>
        </tr>
    </table>
    <input id="hidArchivo" style="width: 1px; height: 22px" type="hidden" runat="server" />
    </form>
</body>
</html>
