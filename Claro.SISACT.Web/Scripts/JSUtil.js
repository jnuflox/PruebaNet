function Hipervinculo_Over(obj) {
    obj.style.textDecoration = "underline";
}
function Hipervinculo_Out(obj) {
    obj.style.textDecoration = "";
}

function Texto_Foco(obj) {
    obj.style.backgroundColor = "#fef09e";
    obj.style.border = "solid 1px #e20000";
    obj.select();
}
function Texto_Blur(obj) {
    obj.style.backgroundColor = "#FFFFFF";
    obj.style.border = "solid 1px gray";
}

function Validar_Valor(obj, msj) {
    obj = document.getElementById(obj);
    if (obj.value == "") {
        alert(msj);
        obj.focus();
        return false;
    }
    return true;
}

/*Limpia la grilla*/
function clearGrid_Util(grid, useFoot, columnFoot) {
    grid.clearCellModel(); 
    grid.clearRowModel();
    grid.clearSelectedModel();
    //grid.clearScrollModel(); 
    //grid.clearSelectionModel(); 
    grid.clearSortModel();
    if (useFoot) grid.setFooterText('Filas: 0', columnFoot);
    grid.refresh();
}
/*calcula el heigth del scroll de la grilla*/
function ScrollGridHeigth_Util(grid, useFoot) {
    var tmpH = grid.getRowHeight(0);
    var tmpT = grid.getRowCount() * tmpH;
    if (useFoot) tmpT += grid.getFooterHeight();
    tmpT += grid.getHeaderHeight();
    grid.setScrollHeight(tmpT);
}
function Progress_Util(display, grid, load) {
    if (display) {
        document.getElementById(grid).style.display = "none";
        document.getElementById(load).style.display = "";
    }
    else {
        document.getElementById(grid).style.display = "";
        document.getElementById(load).style.display = "none";
    }
}

function getLeftPosition(w) {
    var leftPosition = (screen.width) ? (screen.width - w) / 2 : 100;
    return leftPosition;
}
function getTopPosition(h) {
    var topPosition = (screen.height) ? (screen.height - h) / 2 : 100;
    return topPosition;
}

function NumericKeyPress_Util(key) 
{
    if (key > 47 && key < 58) return true;
    else return false;
}

function CreateButton_Util(accion,tooltip,image,text) 
{
    var obj = new AW.UI.Button;
    if (text!=null) obj.setControlText(text);
    else obj.setControlText(accion);
    
    if (!image) obj.setControlImage(accion);
    else obj.setControlImage(image);

    if (accion == "Salir") obj.setControlTooltip("Click para cerrar ventana");
    else if (accion == "Cancelar") obj.setControlTooltip("Click para cancelar proceso");
    else obj.setControlTooltip(tooltip);
    
    obj.setStyle("Cursor", "Pointer");
    obj.setStyle("display", "inline-block");
    return obj;
}

var Flag_ChangeHeaderGrid;
function Change_HeaderGridValue_Util(objGridPage, value, column) {
    Flag_ChangeHeaderGrid = true;
    var max = objGridPage.getRowCount();
    for (con = 0; con < max; con++) {
        objGridPage.setCellValue(value, column, con);
    }
    Flag_ChangeHeaderGrid = false;

//    if (value) DisableButtons_Util(false, Btn, NameImg, strPath);
//    else DisableButtons_Util(true, Btn, NameImg, strPath);

//    if (Btn2) {
//        if (value) DisableButtons_Util(false, Btn2, NameImg2, strPath);
//        else DisableButtons_Util(true, Btn2, NameImg2, strPath);
//    }
}
//function Change_CellGridValue_Util(objGridPage, column) {
//    if (!Flag_ChangeHeaderGrid) {
//        var max = objGridPage.getRowCount();
////        DisableButtons_Util(true, Btn, NameImg, strPath);
////        if (Btn2) DisableButtons_Util(true, Btn2, NameImg2, strPath);

//        for (con = 0; con < max; con++) {
//            if (objGridPage.getCellValue(column, con)) {
////                DisableButtons_Util(false, Btn, NameImg, strPath);
////                if (Btn2) DisableButtons_Util(false, Btn2, NameImg2, strPath);
//                break;
//            }
//        }
//    }
//}

function Trim(str) 
{
    return str.replace(/^\s*|\s*$/g, "");
}

/*valida que lo ingresado sea un correo valido*/
function ValidateEmail_Util(obj, msj) {
    var tmpObj = document.getElementById(obj);
    if (tmpObj.value == "") return false;
    if (Verifica_Correo(tmpObj.value)) {
        alert(msj);
        tmpObj.focus();
        return false;
    }
    return true;
}
function Verifica_Correo(emailStr) {
    /* The following pattern is used to check if the entered e-mail address
    fits the user@domain format.  It also is used to separate the username
    from the domain. */
    var emailPat = /^(.+)@(.+)$/
    /* The following string represents the pattern for matching all special
    characters.  We don't want to allow special characters in the address. 
    These characters include ( ) < > @ , ; : \ " . [ ]    */
    var specialChars = "\\(\\)<>@,;:\\\\\\\"\\.\\[\\]"
    /* The following string represents the range of characters allowed in a 
    username or domainname.  It really states which chars aren't allowed. */
    var validChars = "\[^\\s" + specialChars + "\]"
    /* The following pattern applies if the "user" is a quoted string (in
    which case, there are no rules about which characters are allowed
    and which aren't; anything goes).  E.g. "jiminy cricket"@disney.com
    is a legal e-mail address. */
    var quotedUser = "(\"[^\"]*\")"
    /* The following pattern applies for domains that are IP addresses,
    rather than symbolic names.  E.g. joe@[123.124.233.4] is a legal
    e-mail address. NOTE: The square brackets are required. */
    var ipDomainPat = /^\[(\d{1,3})\.(\d{1,3})\.(\d{1,3})\.(\d{1,3})\]$/
    /* The following string represents an atom (basically a series of
    non-special characters.) */
    var atom = validChars + '+'
    /* The following string represents one word in the typical username.
    For example, in john.doe@somewhere.com, john and doe are words.
    Basically, a word is either an atom or quoted string. */
    var word = "(" + atom + "|" + quotedUser + ")"
    // The following pattern describes the structure of the user
    var userPat = new RegExp("^" + word + "(\\." + word + ")*$")
    /* The following pattern describes the structure of a normal symbolic
    domain, as opposed to ipDomainPat, shown above. */
    var domainPat = new RegExp("^" + atom + "(\\." + atom + ")*$")


    /* Finally, let's start trying to figure out if the supplied address is
    valid. */

    /* Begin with the coarse pattern to simply break up user@domain into
    different pieces that are easy to analyze. */
    var matchArray = emailStr.match(emailPat)
    if (matchArray == null) {
        /* Too many/few @'s or something; basically, this address doesn't
        even fit the general mould of a valid e-mail address. */
        //alert("La dirección de correo parece inválida (comprobar @ and .'s)")
        return true
    }
    var user = matchArray[1]
    var domain = matchArray[2]

    // See if "user" is valid 
    if (user.match(userPat) == null) {
        // user is not valid
        //alert("El usuario no parece ser válido.")
        return true
    }

    /* if the e-mail address is at an IP address (as opposed to a symbolic
    host name) make sure the IP address is valid. */
    var IPArray = domain.match(ipDomainPat)
    if (IPArray != null) {
        // this is an IP address
        for (var i = 1; i <= 4; i++) {
            if (IPArray[i] > 255) {
                //alert("IP de destino incorrecta.")
                return true
            }
        }
        return false
    }

    // Domain is symbolic name
    var domainArray = domain.match(domainPat)
    if (domainArray == null) {
        //alert("El dominio no parece ser válido.")
        return true
    }

    /* domain name seems valid, but now make sure that it ends in a
    three-letter word (like com, edu, gov) or a two-letter word,
    representing country (uk, nl), and that there's a hostname preceding 
    the domain or country. */

    /* Now we need to break up the domain to get a count of how many atoms
    it consists of. */
    var atomPat = new RegExp(atom, "g")
    var domArr = domain.match(atomPat)
    var len = domArr.length
    if (domArr[domArr.length - 1].length < 2 ||
        domArr[domArr.length - 1].length > 3) {
        // the address must end in a two letter or three letter word.
        //alert("La dirección debe terminar con un dominio de 3 letras, o un nombre de país de dos letras.")
        return true
    }

    // Make sure there's a host name preceding the domain.
    if (len < 2) {
        //var errStr="¡A esta dirección le falta un nombre de host!"
        //alert(errStr)
        return true
    }

    // If we've gotten this far, everything's valid!
    return false;
}
function MostrarTip(strMessage, strLevel) 
{
    if (strLevel==null || strLevel!="") strLevel = "../";
    Tip("<img src='" + strLevel + "Imagenes/Iconos/ic_info.gif' align='absmiddle' />&nbsp;" + strMessage, SHADOW, true, ABOVE, true, JUMPHORZ, true, BGCOLOR, '#ffffe0', FADEIN, 500, FADEOUT, 500)
}

function ValidateDateChange_Util(obj) {
    if (obj.value == "") return true;
    if (!ValidateDate_Util(obj.value)) {
        alert("Formato de Fecha Invalido");
        obj.value = "";
        obj.focus();
    }
}
function ValidateDate_Util(dateStr) {
    var format = "DMY";

    if (format.substring(0, 1) == "Y") { // If the year is first
        var reg1 = /^\d{2}(\-|\/|\.)\d{1,2}\1\d{1,2}$/
        var reg2 = /^\d{4}(\-|\/|\.)\d{1,2}\1\d{1,2}$/
    } else if (format.substring(1, 2) == "Y") { // If the year is second
        var reg1 = /^\d{1,2}(\-|\/|\.)\d{2}\1\d{1,2}$/
        var reg2 = /^\d{1,2}(\-|\/|\.)\d{4}\1\d{1,2}$/
    } else { // The year must be third
        var reg1 = /^\d{1,2}(\-|\/|\.)\d{1,2}\1\d{2}$/
        var reg2 = /^\d{1,2}(\-|\/|\.)\d{1,2}\1\d{4}$/
    }

    // If it doesn't conform to the right format (with either a 2 digit year or 4 digit year), fail
    if ((reg1.test(dateStr) == false) && (reg2.test(dateStr) == false)) { return false; }
    var parts = dateStr.split(RegExp.$1); // Split into 3 parts based on what the divider was
    // Check to see if the 3 parts end up making a valid date
    if (format.substring(0, 1) == "M") { var mm = parts[0]; }
    else
        if (format.substring(1, 2) == "M") { var mm = parts[1]; } else { var mm = parts[2]; }
    if (format.substring(0, 1) == "D") { var dd = parts[0]; } else
        if (format.substring(1, 2) == "D") { var dd = parts[1]; } else { var dd = parts[2]; }
    if (format.substring(0, 1) == "Y") { var yy = parts[0]; } else
        if (format.substring(1, 2) == "Y") { var yy = parts[1]; } else { var yy = parts[2]; }
    if (parseFloat(yy) <= 50) { yy = (parseFloat(yy) + 2000).toString(); }
    if (parseFloat(yy) <= 99) { yy = (parseFloat(yy) + 1900).toString(); }
    var dt = new Date(parseFloat(yy), parseFloat(mm) - 1, parseFloat(dd), 0, 0, 0, 0);
    if (parseFloat(dd) != dt.getDate()) { return false; }
    if (parseFloat(mm) - 1 != dt.getMonth()) { return false; }
    return true;
}
function redondear(numero) {
    var original = parseFloat(numero);
    var result = Math.round(original * 100) / 100;
    return result;
}
function Formato_Decimal(num) {
    if (num == null) return "";

    num = num.toString().replace(/\$|\,/g, '');
    if (isNaN(num))
        num = "0";
    sign = (num == (num = Math.abs(num)));
    num = Math.floor(num * 100 + 0.50000000001);
    cents = num % 100;
    num = Math.floor(num / 100).toString();

    if (cents < 0) {
        cents = cents * -1;
    }

    if (cents < 10)
        cents = "0" + cents;
    for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3); i++)
        num = num.substring(0, num.length - (4 * i + 3)) + ',' +
		num.substring(num.length - (4 * i + 3));
    return (((sign) ? '' : '-') + num + '.' + cents);
}

function Abrir_Popup(popup) {
    $find(popup).show();
    $find(popup + "Animacion").get_OnClickBehavior().play();

    var panel = $find(popup).get_PopupControlID();
    setTimeout("document.getElementById('" + panel + "').style.filter=''", 1500);
}
function Validar_Valor(obj, msj) {
    obj = document.getElementById(obj);
    if (obj.value == "") {
        alert(msj);
        try {
            obj.focus();
        }
        catch (ex) { }
        return false;
    }
    return true;
} 
 
