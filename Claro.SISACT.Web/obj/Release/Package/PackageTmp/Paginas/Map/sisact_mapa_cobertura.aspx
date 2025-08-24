<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sisact_mapa_cobertura.aspx.cs"
    Inherits="Claro.SISACT.Web.Paginas.Map.sisact_mapa_cobertura" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>

    <%-- INICIATIVA - 932 | MOVILIDAD IFI | BRYAN CHUMBES LIZARRAGA--%>

    <title>Mapa de Cobertura</title>
    <meta http-equiv="X-UA-Compatible" content="IE=Edge" />
    <link rel="stylesheet" href="../../Estilos/Map/bootstrap.min.css" />
    <link rel="stylesheet" href="../../Estilos/Map/leaflet.css" />
    <script language="javascript" src="../../Scripts/jquery-1.9.1.js" type="text/javascript"></script>
    <script type="text/javascript" language="JavaScript" src="../../Scripts/KeySettings/KeySettings.js"></script><!-- INICIATIVA 992 -->
    
    <style type="text/css">
        ::-ms-clear
        {
            display: none;
        }
        
        .form
        {
            width: 100%;
            height: 70px;
            max-width: 400px;
            position: relative;
            overflow: hidden;
        }
        
        .form input
        {
            width: 100%;
            height: 100%;
            background: none;
            color: #333;
            padding-top: 20px;
            border: none;
            outline: 0px;
        }
        
        .form .lbl-nombre
        {
            position: absolute;
            bottom: 0;
            left: 0;
            width: 100%;
            height: 100%;
            pointer-events: none;
            border-bottom: 1px solid #c7c7c7;
        }
        
        .form .lbl-nombre:after
        {
            content: '';
            position: absolute;
            left: 0;
            bottom: -1px;
            width: 100%;
            height: 100%;
            border-bottom: 3px solid #2ecece;
            transform: translateX(-100%);
            transition: all 0.3s ease;
        }
        
        .text-nomb
        {
            position: absolute;
            bottom: 5px;
            left: 0;
            transition: all 0.3s ease;
            color: #333;
        }
        
        .form input:focus + .lbl-nombre .text-nomb, .form input:valid + .lbl-nombre .text-nomb
        {
            transform: translateY(-150%);
            font-size: 14px;
            color: #2ecece;
        }
        
        .form input:focus + .lbl-nombre:after, .form input:valid + .lbl-nombre:after
        {
            transform: translateX(0%);
        }
        
        #result
        {
            position: absolute;
            width: 100%;
            max-width: 300px;
            cursor: pointer;
            overflow-y: visible;
            max-height: 400px;
            box-sizing: border-box;
            z-index: 1001;
        }
        
        .link-class:hover
        {
            background-color: #f1f1f1;
        }
        
        body
        {
            background-size: cover;
        }
        
        /* TAMAÑO MAPA */
        #map
        {
            height: 600px;
            max-width: 382px;
            margin: auto;
            padding: 10px;
            z-index: 5;
            border-radius: 4px;
        }
        
        .mapa
        {
            height: 600px;
            width: 382px;
            display: inline-block;
        }
        
        .busqueda
        {
            height: 514px;
            width: 378px;
            display: block;
            padding-top: 10px;
            padding-bottom: 26px;
            padding-left: 40px;
            padding-right: 42px;
            float: right;
            display: inline-block;
        }
        
        .text-resultado
        {
            height: 514px;
            width: 378px;
            display: block;
            padding-top: 48px;
            padding-bottom: 26px;
            padding-left: 40px;
            padding-right: 42px;
            float: right;
            display: inline-block;
        }
        
        .container
        {
            width: 800px;
            height: 620px;
            position: relative;
            margin: 0 auto;
        }
        
        .box
        {
            width: 70%;
            position: absolute;
            padding: 10px 20px;
            top: 0;
            left: 0;
            z-index: 10; /*opacity: 0.9;*/
        }
        
        .stack-top
        {
            z-index: 9;
            margin: 30px;
        }
        
        #buscador .el-input .el-input__inner, #addressSegments .el-input .el-input__inner, .el-autocomplete-suggestion li
        {
            color: #000000;
            font-size: 15px;
        }
        
        .leaflet-control-zoom-fullscreen
        {
            background-image: url(icon-fullscreen.png);
        }
        
        .leaflet-control-layers-selector[type="radio"]
        {
            margin: 4px 4px !important;
            -webkit-appearance: none;
            -moz-appearance: none;
            display: inline-block;
            position: relative;
            background-color: #f1f1f1;
            color: #0a86ff;
            top: 10px;
            height: 20px;
            width: 20px;
            border: 0;
            border-radius: 50px;
            cursor: pointer;
            margin-right: 7px;
            outline: none;
        }
        
        .leaflet-control-layers-selector[type="checkbox"]
        {
            margin: 4px 4px !important;
            -webkit-appearance: none;
            -moz-appearance: none;
            display: inline-block;
            position: relative;
            background-color: #f1f1f1;
            color: #0a86ff;
            top: 10px;
            height: 20px;
            width: 20px;
            border: 0;
            cursor: pointer;
            margin-right: 7px;
            outline: none;
        }
        
        .leaflet-control-layers-selector:checked::before
        {
            position: absolute;
            font: 11px/1 'Open Sans' , sans-serif;
            left: 7px;
            top: 4px;
            content: '\02143';
            transform: rotate(40deg);
        }
        
        .leaflet-control-layers-selector:hover
        {
            background-color: #f7f7f7;
        }
        
        .leaflet-control-layers-selector:checked
        {
            background-color: #f1f1f1;
        }
        
        .leaflet-control-layers-base span
        {
            font: 12px/1.7 'Open Sans' , sans-serif;
            color: #333;
            -webkit-font-smoothing: antialiased;
            -moz-osx-font-smoothing: grayscale;
            cursor: pointer;
        }
        
        .leaflet-control-layers-separator
        {
            height: 0;
            border-top: 1px solid #ddd;
            margin: 15px -10px 0px -6px;
        }
        
        .leaflet-control-layers-list
        {
            margin-bottom: 10px;
        }
        
        .title-geodir
        {
            /*font-family: DIN;*/
            font-weight: 700;
            font-size: 20px;
            color: #333;
            margin-bottom: 10px;
            line-height: 32px;
            letter-spacing: -.02em;
        }
        
        .mt-12
        {
            margin-top: 12px;
        }
        
        .text-geodir
        {
            font-size: 14px;
            color: #000;
            margin-bottom: 24px; /*font-family: Roboto;*/
            line-height: 24px;
        }
        
        .resultado
        {
            /*padding: 0px 5px;*/
            padding-top: 30px;
            padding-bottom: 0px;
            padding-left: 0px;
            padding-right: 0px;
            margin-bottom: 20px;
            line-height: 24px;
        }
        
        .resultado2
        {
            /*padding: 0px 5px;*/
            padding-top: 0px;
            padding-bottom: 0px;
            padding-left: 0px;
            padding-right: 0px;
            margin-bottom: 20px;
            line-height: 24px;
        }
        .enviar
        {
            padding-top: 0px;
            padding-bottom: 0px;
            padding-left: 0px;
            padding-right: 0px;
            margin-bottom: 20px;
            line-height: 24px;
        }
    </style>
</head>
<body>
    <form id="frmPrincipal" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="True" EnableScriptGlobalization="true"
        EnableScriptLocalization="true" />
    <script src="../../Scripts/Paginas/Map/leaflet.js" type="text/javascript"></script>
    <script src="../../Scripts/Paginas/Map/Control.FullScreen.js" type="text/javascript"></script>
    <div class="container">
        <div class="mapa">
            <div id="map" class="stack-top">
            </div>
        </div>
        <div class="busqueda">
            <div class="title-geodir">
                <div class="mt-12">
                    <span>¿Dónde te encuentras?</span>
                </div>
            </div>
            <div class="text-geodir">
                <div id="divInfo">
                    <span><span>Ingresa una dirección para validar la cobertura y continuar con el proceso
                        de evaluación IFI.</span> </span>
                </div>
            </div>
            <div>
                <div class="form">
                    <input type="text" name="search" id="search" required />
                    <label class="lbl-nombre">
                        <span class="text-nomb">Dirección de Cobertura IFI</span>
                    </label>
                </div>
            </div>
            <div>
                <ul class="list-group" id="result">
                </ul>
            </div>
            <div class="resultado">
                <input type="text" name="txtTipoVia" id="txtTipoVia" readonly placeholder="Tipo Via" style="display: none"
                    class="form-control" />
            </div>
            <div class="resultado2">
                <input type="text" name="txtVia" id="txtVia" readonly placeholder="Via" style="display: none"
                    class="form-control" />
            </div>
            <div class="resultado2">
                <input type="text" name="txtNumero" id="txtNumero" readonly placeholder="Numero"
                    style="display: none" class="form-control" />
            </div>
            <div class="resultado2">
                <input type="text" name="txtUrbanizacion" id="txtUrbanizacion" readonly placeholder="Urbanizacion"
                    style="display: none" class="form-control" />
            </div>
            <div class="resultado2">
                <input type="text" name="txtLatitud" id="txtLatitud" readonly=readonly placeholder="Latitud"
                      style="display: none" class="form-control" />
            </div>

            <div class="resultado2">
                <input type="text" name="txtLongitud" id="txtLongitud" readonly=readonly placeholder="Logitud"
                       style="display: none" class="form-control" />
            </div>
            <div class="enviar" align="center">
                <input class="btn btn-danger" id="btnValidar" style="cursor: hand; height: 40px;
                    width: 150px; display: none" onclick="validar()" type="button" value="Continuar Registro" />
            </div>
        </div>
    </div>
    <input id="hidCoordenadas" type="hidden" name="hidCoordenadas" runat="server" />
     <input id="hidNewCoordenadas" type="hidden" name="hidNewCoordenadas" runat="server" />
    <input id="hidTipoVia" type="hidden" name="hidTipoVia" runat="server" />
    <input id="hidFlagCoordenadas" type="hidden" name="hidFlagCoordenadas" runat="server" />
    <input id="hidAddress" type="hidden" name="hidAddress" runat="server" />
    </form>
</body>
</html>
<script language="javascript" src="../../Scripts/Paginas/Map/sisact_mapa_cobertura.js"
    type="text/javascript"></script>
