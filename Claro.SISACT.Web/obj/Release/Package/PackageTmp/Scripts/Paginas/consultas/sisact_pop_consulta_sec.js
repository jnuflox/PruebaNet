
        function asignarFila(nroSEC, strNombres, strApePaterno, strApeMaterno, strRazonSocial) {
            document.getElementById('hidNroSEC').value = nroSEC;
            document.getElementById('txtApePat').value = strApePaterno;
            document.getElementById('txtApeMat').value = strApeMaterno;
            document.getElementById('txtNombre').value = strNombres;
            document.getElementById('txtRazonSocial').value = strRazonSocial;
        }

        function aceptar() {
            var nroSEC = document.getElementById('hidNroSEC').value;
            if (nroSEC == '') {
                alert('Debe selecionar una evaluación.');
                return false;
            }

            window.opener.document.getElementById('hidNroSEC').value = nroSEC;
            window.opener.document.getElementById('txtApePat').value = document.getElementById('txtApePat').value;
            window.opener.document.getElementById('txtApeMat').value = document.getElementById('txtApeMat').value;
            window.opener.document.getElementById('txtNombre').value = document.getElementById('txtNombre').value;
            window.opener.document.getElementById('txtRazonSocial').value = document.getElementById('txtRazonSocial').value;
            window.opener.retornarConsultaSEC(nroSEC);
            window.close();
        }

        function cerrar() {
            window.opener.nuevaEvaluacion();
            window.close();
        }

        function inicio() {
            for (var num = 0; num <= document.frmPrincipal.length - 1; num++) {
                if (document.frmPrincipal.elements[num].name == "rbtSEC") {
                    if (document.frmPrincipal.elements[num].disabled) {
                        alert("El cliente tiene una SEC que ha sido utilizada en una venta, no se puede generar una nueva SEC.");
                        document.getElementById('btnAceptar').disabled = true;
                        break;
                    }
                }
            }
        }