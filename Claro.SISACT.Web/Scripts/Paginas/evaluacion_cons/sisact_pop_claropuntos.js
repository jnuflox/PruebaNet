        $(document).ready(function () {
            var tipo;
            tipo = requestTipo;


            if (tipo == 1) {
                $('#tblBotonesAccion').hide();
                $('#tblBotonSalir').show();
                $('#tblDatos2').hide();
            }
            else if (tipo == 2) {
                $('#tblBotonesAccion').show();
                $('#tblBotonSalir').hide();
                $('#tblDatos2').show();
            }
			
			//INC000000830856 - INICIO
            var puntosActuales = $('#txtPuntosActual').val() || 0;
            if (parseInt(puntosActuales) == 0) 
            {
                alert(constMsgSinClaroPuntos)
            }
            //INC000000830856 - FIN
        });

          function f_Aceptar() {
              alert("Aún no se termina, faltar validar.");
              window.close();
          }
  
          function f_Cancelar() {
              if (confirm("Esta seguro de cerrar?"))
                  window.close();
          }
          function f_Salir() {
              window.close();
          }
          function abrirModal() {
              alert("Aún no se termina, disculpe la molestía.");
          }
