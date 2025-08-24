		    function Inicio() {
		        var hidCadenaItems = ''
		        if (document.getElementById("hidCadenaItems").value != '') {
		            hidCadenaItems = document.getElementById("hidCadenaItems").value;

		            var arrhidCadenaItems = hidCadenaItems.split('|')

                    //Ordernar Items - Inicio
                    var arrItemsOrdenTemp = [];
                    var pos;
                    for (var i = 0; i < arrhidCadenaItems.length; i++) {
                        arrTemp = arrhidCadenaItems[i].split('&')
                        pos = arrTemp[0]
                        arrItemsOrdenTemp[pos] = arrhidCadenaItems[i]
                    }

                    var arrItemsOrdenado = []; //array Final

                    for (var i = 1; i <= arrItemsOrdenTemp.length - 1; i++) {//i=1?
                        arrItemsOrdenado[i - 1] = arrItemsOrdenTemp[i];
                    }

                    //Ordenar - Items -FIn

                    document.getElementById("hidCadenaItems").value = arrItemsOrdenado.join("|")

                    for (var i = 0; i <= arrItemsOrdenado.length - 1; i++) {
                        var arrItemsOrden = arrItemsOrdenado[i].split('&')
		                AddItem(arrItemsOrden[0], arrItemsOrden[1], arrItemsOrden[2], arrItemsOrden[3]);
		            }
		        }
		        else {
		            alert('Error al consultar la Declaración de Conocimiento.');

		            window.returnValue = "ERROR";
		            window.close();
		        }
		    }

		    function AddItem(orden, texto, obligarotio, idItem) {
		        var table = document.getElementById("tblDeclarConocimiento");
		        var row = table.insertRow(-1);
		        var cell1 = row.insertCell(0);
		        var cell2 = row.insertCell(1);
		        var cell3 = row.insertCell(2);
		        var cell4 = row.insertCell(3);
		        
                cell2.innerHTML = "<label  id='bl"  + idItem + "' type='text' style='FONT-SIZE: 11px; FONT-FAMILY: Arial;'> "+orden+". "+texto+"</label>";
                cell3.innerHTML = "<input id='chek" + idItem + "' type='checkbox' checked='checked'/>";

		        if (obligarotio == '1') { // 1 --> obligatorio
		            document.getElementById("chek" + idItem).disabled = true;  // DESACTIVA
		        }
		        else {
		            document.getElementById("chek" + idItem).disabled = false;
		        }
		    }

		    function GuardarItems() {
		        var hidCadenaItems = document.getElementById("hidCadenaItems").value;
		        var arrItems = hidCadenaItems.split('|')
		        var strItemsFinal = ''
		        var ContItemSelec = 0
		        for (var i = 0; i <= arrItems.length - 1; i++) {
		            var arrItemsOrden = arrItems[i].split('&')
		            if (document.getElementById("chek" + arrItemsOrden[3]).checked)
		                ContItemSelec++
		        }
		        if (ContItemSelec > 0) {
		            if (confirm("¿Desea Guardar la Declaracion de Conocimiento?")) {
		                for (var i = 0; i <= arrItems.length - 1; i++) {
		                    var arrItemsOrden = arrItems[i].split('&')
		                    if (document.getElementById("chek" + arrItemsOrden[3]).checked) {
		                        strItemsFinal = strItemsFinal + ";" + arrItemsOrden[3] + "|" + "1"
		                    }
		                    else {
		                        strItemsFinal = strItemsFinal + ";" + arrItemsOrden[3] + "|" + "0"
		                    }
		                }
		                strItemsFinal = strItemsFinal.substring(1)
		                window.returnValue = strItemsFinal
		                window.close();
		            }
		        }
		        else alert("Debe seleccionar Items.");
		    }
	    