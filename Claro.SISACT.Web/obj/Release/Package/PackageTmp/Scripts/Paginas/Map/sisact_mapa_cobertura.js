/*INICIO INICIATIVA - 932 | MOVILIDAD IFI | BRYAN CHUMBES LIZARRAGA*/

var token;

$(document).ready(function () {

    $('#search').keyup(function (e) {

        $('#hidFlagCoordenadas').val('');
        $('#hidAddress').val('');
        var keyCode = e.which;

        var searchField = $('#search').val();

        if (keyCode == 13) {
            var resultado = validarCoordenadas(searchField);

            if (resultado) {
                return true;
            }
            else {
            event.preventDefault();
            return false;
        }

        }

        $('#result').html('');

        var expression = new RegExp(searchField, "i");

        queryString = searchField;
        token = 'f1cc5ddc-0098-4772-aecd-be8a4647950a';

        if (queryString !== "" && queryString !== undefined && queryString !== null) {
            $.ajax({
                url: 'https://apis.geodir.co/places/autocomplete/v1/json?search=' +
                    queryString + '&key=' + token,
                success: function (respuesta) {
                    predictions = respuesta.predictions;
                    $('#result').html('');
                    $.each(predictions, function (key, value) {

                        $('#result').append('<li class="list-group-item link-class"><span class="text-muted">' + value.description + '</span><span class="text-muted" style="display: none;">' + '|' + value.main_text + '</span><span class="text-muted" style="display: none;">' + '|' + value.place_id + '</span></li>');
                    });
                },
                error: function () {
                    alert('No se ha podido obtener la información');
                    console.log("No se ha podido obtener la información");
                }
            });
        }
        {
            $('#result').html('');
            cleanmarker();
            cleanSegments();
        }

    });

    $('#result').on('click', 'li', function () {
        var click_text = $(this).text().split('|');
        $('#search').val($.trim(click_text[0]));
        handleSelect($.trim(click_text[2]));
        $("#result").html('');
    });
});

var map = L.map('map', {
    center: [-12.12, -77.02],
    zoom: 13,
    minZoom: 1,
    maxZoom: 20,
    scaleControl: false,
    zoomControl: false,
    attributionControl: true
});

function validarCoordenadas(searchField) {

    var splitCoordenadas = '';
    var y = '';
    var x = '';
    var blnOK = true;

    var splitCoordenadas = searchField.split(',');

    if (splitCoordenadas.length == 2) {

        y = splitCoordenadas[0];
        x = splitCoordenadas[1];

        if (y != "" && typeof y != "undefined" & x != "" && typeof x != "undefined") {

            var validos = '0123456789.,- ';

            for (var i = 0; i < searchField.length; i++) {
                var c = searchField.substr(i, 1);
                var existe = validos.indexOf(c);
                if (existe < 0) {
                    blnOK = false;
                    break;
                }
            }

        }
        else {
            //ERROR 
            blnOK = false;
        }

    }
    else {
        //ERROR 
        blnOK = false;
    }

    if (blnOK) {

        coordinateSearch(y, x);
    }

    return blnOK;
}

function cleanmarker() {
    if (this.marker) {
        this.map.removeLayer(this.marker);
    }
}

function handleSelect(place_id) {
    this.cleanmarker();
    var that = this;
    var place_id = place_id;
    cleanSegments();
    $.ajax({
        url: "https://apis.geodir.co/places/fields/v1/json?place_id=" + place_id
            + '&key=' + this.token,
        success: function (respuesta) {
            if (respuesta.status == 'OK') {
                var latitud = respuesta.geometry.coordinates.lat;
                var longitud = respuesta.geometry.coordinates.lon;
                var address = respuesta.standard_address;
                that.address = address;
                that.getInformation(respuesta, that);
                that.printMarker(latitud, longitud, address, that);
                that.map.flyTo([latitud, longitud], 16);

                var lat_lon = latitud + ";" + longitud;
                $('#hidCoordenadas').val(lat_lon);
            }
        },
        error: function () {
            console.error("No se ha podido obtener la información");
        }
    });
}

function coordinateSearch(lat, lng) {

    this.cleanmarker();
    var that = this;
    cleanSegments();
    $.ajax({
        url: 'https://apis.geodir.co/geocoding/v1/json?latlon=' +
                lat + ',' + lng + '&key=' + that.token,
        success: function (respuesta) {
            if (respuesta.status == 'OK') {
                var firstResultCoord = respuesta.results[0];
                var latitudCoord = firstResultCoord.geometry.coordinates.lat;
                var longitudCoord = firstResultCoord.geometry.coordinates.lon;
                var addressCoord = firstResultCoord.standard_address;
                that.address = addressCoord;
                that.getInformation(firstResultCoord, that);
                that.printMarker(latitudCoord, longitudCoord, addressCoord, that);
                that.map.flyTo([latitudCoord, longitudCoord], 16);

                var lat_lon_Coord = latitudCoord + ";" + longitudCoord;
                $('#hidCoordenadas').val(lat_lon_Coord);
                $('#hidFlagCoordenadas').val('0');
                $('#hidAddress').val(addressCoord);
            }
            else {
                var lat_lon_C = lat + ";" + lng;
                $('#hidCoordenadas').val(lat_lon_C);
                that.printMarkerLatLong(lat, lng, that);
                $('#hidFlagCoordenadas').val('1');
                $('#hidAddress').val('');

            }
        },
        error: function () {
            console.error("No se ha podido obtener la información");
        }
    });
}
function printMarker(latitud, longitud, address, that) {
    var customIcon = new L.Icon({
        iconUrl: 'geodir.png',
        iconSize: [40, 40],
        iconAnchor: [25, 50],
        popupAnchor: [0, -38]
    });
    that.marker = L.marker([latitud, longitud], {
        icon: customIcon,
        draggable: 'true'
    }).addTo(that.map);
    that.marker.on('dragend', function (event) {
        var marker = event.target;
        var position = marker.getLatLng();
        marker.setLatLng(new L.LatLng(position.lat, position.lng), { draggable: 'true' });
        map.panTo(new L.LatLng(position.lat, position.lng));
        marker.openPopup();
        cleanSegments();
        $.ajax({
            url: 'https://apis.geodir.co/geocoding/v1/json?latlon=' +
                position.lat + ',' + position.lng + '&key=' + that.token,
            success: function (respuesta) {
                if (respuesta.status == 'OK') {
                    var firstResult = respuesta.results[0];
                    that.getInformation(firstResult, that);
                    var address_1 = firstResult.standard_address;
                    $('#search').val(address_1);

                    var lat_lon = firstResult.geometry.coordinates.lat + ";" + firstResult.geometry.coordinates.lon;
                    $('#hidCoordenadas').val(lat_lon);
                }
                else {
                    console.error('fallo reverse');
                }
            },
            error: function () {
                console.error("No se ha podido obtener la información");
            }
        });
    });
}

function printMarkerLatLong(latitud, longitud, that) {
    that.map.flyTo([latitud, longitud], 16);
    var customIcon = new L.Icon({
        iconUrl: 'geodir.png',
        iconSize: [40, 40],
        iconAnchor: [25, 50],
        popupAnchor: [0, -38]
    });
    that.marker = L.marker([latitud, longitud], {
        icon: customIcon,
        draggable: 'true'
    }).addTo(that.map);
    that.marker.on('dragend', function (event) {
        var marker = event.target;
        var position = marker.getLatLng();
        marker.setLatLng(new L.LatLng(position.lat, position.lng), { draggable: 'true' });
        marker.openPopup();
        cleanSegments();
        $.ajax({
            url: 'https://apis.geodir.co/geocoding/v1/json?latlon=' +
                position.lat + ',' + position.lng + '&key=' + that.token,
            success: function (respuesta) {
                if (respuesta.status == 'OK') {
                    var firstResult = respuesta.results[0];
                    that.getInformation(firstResult, that);
                    var address_1 = firstResult.standard_address;
                    $('#search').val(address_1);

                    var lat_lon = firstResult.geometry.coordinates.lat + ";" + firstResult.geometry.coordinates.lon;
                    $('#hidCoordenadas').val(lat_lon);

                    $('#hidFlagCoordenadas').val('0');
                    $('#hidAddress').val(address_1);

                }
                else {
                    //console.error('fallo reverse');
                    var lat_lon_Cd = position.lat + ";" + position.lng;
                    $('#hidCoordenadas').val(lat_lon_Cd);
                    that.printMarkerLatLong(position.lat, position.lng, that);
                    $('#hidFlagCoordenadas').val('1');
                    $('#hidAddress').val('');
                }
            },
            error: function () {
                console.error("No se ha podido obtener la información");
            }
        });
    });

    cleanSegments();
    $('#txtLatitud').val(latitud);
    $('#txtLongitud').val(longitud);

    $('#txtTipoVia').show();
    $('#txtVia').show();
    $('#txtNumero').show();
    $('#txtUrbanizacion').show();
    $('#txtLatitud').show();
    $('#txtLongitud').show();
    $('#btnValidar').show();
    //alert('No se encuentra una direccion para estas coordenadas');
    alert(Key_MsgCoberturaIFI); //INICIATIVA 992 
}
function cleanSegments() {

    $("#txtTipoVia").val('');
    $("#txtVia").val('');
    $("#txtNumero").val('');
    $("#txtUrbanizacion").val('');
    $("#txtLatitud").val('');
    $("#txtLongitud").val('');

    $('#txtTipoVia').hide();
    $('#txtVia').hide();
    $('#txtNumero').hide();
    $('#txtUrbanizacion').hide();
    $('#btnValidar').hide();
    $('#txtLatitud').hide();
    $('#txtLongitud').hide();
    $('#divInfo').show();

}
function getInformation(firstResult) {
    var segments = {
        ROUTE: "route",
        ROUTE_TYPE: "route_type",
        ROUTE_NUMBER: "route_number",
        ADMIN_LEVEL_3: "admin_level_3",
        SUBLOCALITY: "sublocality",
        MANZANA: "manzana"
    };
    var namevia = '';
    var typeVia = '';
    var typeviaabrev = '';
    for (var _i = 0, _a = firstResult.address_segments; _i < _a.length; _i++) {
        var segment = _a[_i];
        var type = segment.types;
        if (includes(type, segments.ROUTE)) {
            namevia = segment.name;
        }
        else if (includes(type, segments.ROUTE_TYPE)) {
            typeVia = segment.name;
            typeviaabrev = segment.name_abbr;
        }
        else if (includes(type, segments.ROUTE_NUMBER)) {
            $("#txtNumero").val(segment.name);
        }
        else if (includes(type, segments.SUBLOCALITY)) {
            $("#txtUrbanizacion").val(segment.name);
        }

        if (namevia) {
            $("#txtVia").val(namevia);
        }
        if (typeVia) {
            $("#txtTipoVia").val(typeVia);
            $("#hidTipoVia").val(typeviaabrev);
        }
        else {
            $("#hidTipoVia").val('');
        }
    }

    $('#txtLatitud').val(firstResult.geometry.coordinates.lat);
    $('#txtLongitud').val(firstResult.geometry.coordinates.lon);
    $('#txtLatitud').show();
    $('#txtLongitud').show();

    $('#txtTipoVia').show();
    $('#txtVia').show();
    $('#txtNumero').show();
    $('#txtUrbanizacion').show();

    $('#btnValidar').show();

}

function includes(type, str) {
    var returnValue = false;

    if (type.indexOf(str) !== -1) {
        returnValue = true;
    }

    return returnValue;
}


function validar() {

    var flagCoordenadas = $('#hidFlagCoordenadas').val();
    var direccion = $('#search').val();

    //INICIATIVA 992 INICIO
    var searchField = $('#search').val();
    var splitCoordenadas = searchField.split(',');
    var latitud = '';
    var longitud = '';
     if (splitCoordenadas.length == 2) {
         latitud = splitCoordenadas[0];
         longitud = splitCoordenadas[1];
     }
     var lat_lon = latitud + ";" + longitud;
     $('#hidNewCoordenadas').val(lat_lon); 
     //INICIATIVA 992 FIN

    var latitud_longitud = $('#hidCoordenadas').val();

    PageMethods.ValidarCobertura(lat_lon, latitud_longitud, flagCoordenadas, ValidarCobertura_Callback); //INICIATIVA 992 lat_lon


}

function ValidarCobertura_Callback(objResponse) {

    if (objResponse.Boleano) {
        var direccionSelec = '';

        if ($('#hidFlagCoordenadas').val() == '0') {
            direccionSelec = $('#hidAddress').val();
        } else {
            direccionSelec = $('#search').val();
        }
        
        window.returnValue = $('#hidFlagCoordenadas').val() + '|' + direccionSelec + '|' + $('#hidTipoVia').val() + '|' + $('#txtVia').val() + '|' + $('#txtNumero').val() + '|' + $('#txtUrbanizacion').val() + '|' + $('#hidNewCoordenadas').val(); //INICIATIVA 992 hidNewCoordenadas
        window.close();

    }
    else {
        $('#divInfo').hide();
    }
}


var geodirStreets = L.tileLayer('https://tiles.geodir.co/osm_tiles/{z}/{x}/{y}.png', {
    minZoom: 1,
    maxZoom: 20,
    attribution: '&copy; <a href="https://maps.geodir.co/" target="_blank">Geodir Maps</a> Contribuciones'
}).addTo(this.map);

var googleSatellite = L.tileLayer('http://{s}.google.com/vt/lyrs=s&x={x}&y={y}&z={z}', {
    minZoom: 1,
    maxZoom: 20,
    subdomains: ['mt0', 'mt1', 'mt2', 'mt3']
});

// CONTROL ESCALA
var scale = L.control.scale({
    imperial: false
}).addTo(this.map);
//CONTROL ZOOM
var zoom = L.control.zoom({ position: 'bottomright' }).addTo(this.map);

/*FIN INICIATIVA - 932 | MOVILIDAD IFI | BRYAN CHUMBES LIZARRAGA*/