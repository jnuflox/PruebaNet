<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sisact_pop_upload_portabilidad.aspx.cs"
    Inherits="Claro.SISACT.Web.Paginas.documentos.sisact_pop_upload_portabilidad" %>

<!DOCTYPE html PUBLIC "-//W3C//Dtd XHTML 1.0 transitional//EN" "http://www.w3.org/tr/xhtml1/Dtd/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>SISACT - Carga de Archivos</title>
    <link href="../../Estilos/general.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" language="JavaScript" src="../../Scripts/funciones_sec.js"></script>
        <script language="javascript" type="text/javascript" src="../../Scripts/jquery-1.9.1.js"></script>
    <script type="text/javascript" language="JavaScript" src="../../Scripts/security.js"></script>
    <script type="text/javascript" language="JavaScript">

        function validarArchivo() {

            if (getValue('ddlTipoArchivo') == '' || getValue('ddlTipoArchivo') == '00') {
                setFocus('ddlTipoArchivo');
                alert('Debe seleccionar un tipo de archivo');
                return false;
            }

            var archivo = document.getElementById('txtFile').value
            if (archivo.indexOf('/**-/') != -1 || archivo == '') {
                alert('Debe ingresar una ruta de archivo válida');
                return false;
            }

            var extension = archivo.substring(archivo.lastIndexOf('.')).toUpperCase();
            if (!(extension == '.PDF' || extension == '.GIF' || extension == '.TIF')) {
                alert('Solo se permiten archivos de tipo PDF, TIF y GIF.');
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

                var tipoArchivo = document.getElementById('ddlTipoArchivo').options[document.getElementById('ddlTipoArchivo').selectedIndex].text;

                if (getValue('hidTiposArchivosAdjuntados').indexOf(getValue('ddlTipoArchivo')) != -1 ||
                    getValue('hidArchivosAdjuntados').indexOf(';' + getValue('ddlTipoArchivo') + ';') != -1) {
                    alert('Ya ha registrado el documento: ' + tipoArchivo);
                    return false;
                }

                frmPrincipal.submit();
            }
        }

        function actualizarGrillaArchivo() {
            window.opener.document.getElementById('hidListaArchivos').value += '|' + getValue('hidArchivo');
            window.opener.document.getElementById('btnCargarArchivo').click();

            document.getElementById('txtFile').value = '';
            window.close();
        }
    </script>
</head>
<body>
    <form id="frmPrincipal" method="post" enctype="multipart/form-data" runat="server">
    <table cellspacing="2" cellpadding="0" align="left" border="0">
        <tr>
            <td align="left" height="25">
                <label class="Arial10B">
                    Tipo de archivo anexo :&nbsp;&nbsp;</label>
                <asp:DropDownList ID="ddlTipoArchivo" runat="server" CssClass="clsSelectEnable0"
                    Width="180px" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:FileUpload CssClass="Boton" ID="txtFile" runat="server" onkeypress="InhabilitarTecla();"
                    Width="400px" Height="22px" />
            </td>
        </tr>
        <tr>
            <td align="left">
                <label class="Arial10B">
                    CARACTERES INVALIDOS :&nbsp; &lt; &gt; ( ) = ? ¿ ¡ + { } [ ] * &amp; % Ñ ñ ' | ° " &nbsp;(tildes)</label>
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
