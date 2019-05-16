/// <reference path="jquery-3.3.1.min.js" />
$(document).ready(function () {

    //Cargamos los pedidos en el LOAD
    CargarFechas();
    CargarPedidos();

    $('#btnBuscar').click(function (e) {
        e.preventDefault();
        Limpiar();
        CargarPedidos();
    });

    //Cargarmos los datepickers
    function CargarFechas() {

        var today = new Date();
        var anio = today.getFullYear();
        var mes = today.getMonth();
        var mesSig = today.getMonth() + 1;
        var dia = today.getDate();

        if (mes < 10)
            mes = '0' + mes;

        if (mesSig < 10)
            mesSig = '0' + mesSig;

        if (dia < 10)
            dia = '0' + dia;

        var fechaDesde = anio + '-' + mes + '-' + dia;
        var fechaHasta = anio + '-' + mesSig + '-' + dia;

        document.getElementById("txtFechaDesde").value = fechaDesde;
        document.getElementById("txtFechaHasta").value = fechaHasta;
    }


    //Función para volver a crear la tabla
    function Limpiar() {
        $("#tablePedidos").dataTable().fnDestroy();
    }

    function LimpiarFormulario() {

        var items = $('#formPedidos').children();
        for (var i = 1; i < items.length; i++) {
            items[i].remove();
        }

        $('#txtId').val(0);
        $('#txtFacturado').val(0.00);
    }

    //Funcion para limpiar el modal al cerrarse
    $('#NuevoForm').on('hidden.bs.modal', function (e) {

        $(this)
          .find("input[type=text],textarea,select")
             .val('')
             .end()
          .find("input[type=checkbox], input[type=radio]")
             .prop("checked", "")
             .end();

        document.getElementById("txtFechaEntrega").valueAsDate = null;

        LimpiarFormulario();
    });

    $('#btnNuevoPedido').click(function (e) {
        
        var clienteSeleccionado = $('#cmbClientes').val();

        if (clienteSeleccionado != '' &&
            clienteSeleccionado != undefined &&
            clienteSeleccionado != null)
            $('#cmbClientesPedido').val(clienteSeleccionado);
        else 
            $('#cmbClientesPedido').val('');

    });

    //Habilitamos el botón cuando los campos requeridos esten llenos
    function ValidarFormulario() {

        var marca = $('#cmbMarca').val();
        var modelo = $('#cmbModelo').val();
        var tamanio = $('#cmbTamanio').val();
        var tipo = $('#cmbTipos').val();
        var costo = $('#txtCosto').val();
        var cantPack = $('#txtCantPack').val();
        var porc = $('#txtPorcentaje').val();

        if (marca === "" || modelo === "" ||
            tamanio === "" || tipo === "" ||
            costo === "" || cantPack === "" ||
            porc === "")
            return false;
        else
            return true;
    }

    $('#formPedidos').each(function () {

        ($(this)).on('click', function () {

            //Obtenemos el precio del producto y seteamos el input
            $('#item > .col-sm-3 > select').on('change', function () {

                var precio = $(this).children("option:selected").data('precio');
                var txtPrecio = $(this).parent().parent().find('.precio').find('input[type="text"]');
                var txtMonto = $(this).parent().parent().find('.monto').find('input[type="text"]');
                var txtCantidad = $(this).parent().parent().find('.cantidad').find('input[type="text"]');

                if ($(this).children("option:selected").val() === '') {
                    $(txtPrecio).val('');
                    $(txtMonto).val('');
                } else {
                    $(txtPrecio).val(Math.round(precio));
                    $(txtMonto).val(Math.round(precio * $(txtCantidad).val()));
                }
            });

            //Cuando modificamos la cantidad, calculamos el monto total por item
            $('#item > .col-sm-2.cantidad > input[type="text"]').keyup(function () {

                var txtPrecio = $(this).parent().parent().find('.precio').find('input[type="text"]');
                var txtMonto = $(this).parent().parent().find('.col-sm-2.monto').find('input[type="text"]');
                var total = $(this).val() * $(txtPrecio).val();
                $(txtMonto).val(Math.round(total));

            });

        });

    });

    $('#formPedidos').on('click', '.borrarItem', function (e) {
        e.preventDefault();
        $(this).parent().parent().parent().remove();
    });

    function ValidarDatos(array, objError) {

        var lista1 = array;

        for (var i = 0; i < lista1.length; i++) {

            if (i < lista1.length - 1) {

                for (var j = i + 1; j < lista1.length; j++) {

                    if (lista1[i].IdProducto == lista1[j].IdProducto) {
                        objError.bHayError = true;
                        objError.msj = "No pueden existir dos item con el mismo producto";
                    }
                }
            }
        }

        for (var i = 0; i < lista1.length; i++) {

            if (lista1[i].IdProducto == '' || lista1[i].IdProducto == undefined || lista1[i].IdProducto == null)
            {
                objError.bHayError = true;
                objError.msj = "Debe indicar el producto de cada item";
            }
        }

        for (var i = 0; i < lista1.length; i++) {

            if (lista1[i].Cantidad == "" || lista1[i].Cantidad == undefined || lista1[i].Cantidad == null)
            {
                objError.bHayError = true;
                objError.msj = "Debe indicar la cantidad de packs de cada item";
            }
        }

        if ($('#cmbClientesPedido').val() == '' || $('#cmbClientesPedido').val() == undefined || $('#cmbClientesPedido').val() == null)
        {
            objError.bHayError = true;
            objError.msj = "Debe elegir un cliente";
        }

        if ($('#cmbEstadosPedido').val() == '' || $('#cmbEstadosPedido').val() == undefined || $('#cmbEstadosPedido').val() == null) {
            objError.bHayError = true;
            objError.msj = "Debe elegir un estado";
        }

        if ($('#txtFechaEntrega').val() == '' || $('#txtFechaEntrega').val() == undefined || $('#txtFechaEntrega').val() == null) {
            objError.bHayError = true;
            objError.msj = "Debe elegir una fecha de entrega";
        }

        return objError.bHayError;
    }

    $('#btnGenerarPedido').click(function (e) {

        var objError =
        {
            bHayError: false,
            msj: ""
        };
        var Total = 0;
        var IdPedido = $('#txtId').val();
        var Facturado = parseFloat($('#txtFacturado').val());
        var CantidadProductos = 0;
        var CantidadPacks = 0;
        var Productos = [];
        var Estado = $('#cmbEstadosPedido').val().trim();
        var ClienteElegido = $('#cmbClientesPedido').val();
        var Repartidor = $('#cmbUsuariosPedido').val();
        var FechaEntrega = $('#txtFechaEntrega').val();

        //Prevenimos que se realice un nuevo submit
        e.preventDefault();

        $('#formPedidos > .row > #item').each(function () {

            var monto = $(this).find('.col-sm-2.monto').find('input[type="text"]').val();
            var packs = $(this).find('.col-sm-2.cantidad').find('input[type="text"]').val();
            var montoTotal = $('#total').val();
            Total = parseFloat(Total) + parseFloat(monto);
            CantidadProductos = parseInt(CantidadProductos) + 1;
            CantidadPacks = parseInt(CantidadPacks) + parseInt(packs);
            var id = $(this).find('.col-sm-3').find('select').children('option:selected').data("info");
            var precio = $(this).find('.col-sm-2.precio').find('input[type="text"]').val();

            var item = {
                IdProducto: id,
                Cantidad: packs,
                Precio: precio,
                Monto: monto
            };

            Productos.push(item);
        });

        if (!ValidarDatos(Productos, objError)) {

            var dataToPost = {
                xiPedido: IdPedido,
                xdFechaEntrega: FechaEntrega,
                xsEstado: Estado,
                xiCliente: ClienteElegido,
                xsUsuario: Repartidor,
                xdTotal: Total,
                xdFacturado: Facturado,
                xoProductos: Productos
            }

            swal({
                title: "¿Desea finalizar el pedido?",
                text: "Cliente: " + $('#cmbClientesPedido').children('option:selected').html() + ", Cantidad de Productos: " + CantidadProductos + ", Packs: " + CantidadPacks + ", Total: $ " + (Math.round(Total * 100) / 100),
                icon: "warning",
                buttons: ['Cancelar', 'Confirmar'],
                dangerMode: false,
            }).then(function (bConfirma) {
                if (bConfirma) {
                    if (Productos.length > 0) {
                        $.ajax({
                            method: 'POST',
                            data: dataToPost,
                            url: '/Pedido/GuardarPedido',
                            success: function (response) {

                                if (response == "") {
                                    swal({
                                        title: "Exito!",
                                        text: "Pedido generado correctamente",
                                        icon: "success",
                                        button: "Aceptar"
                                    });

                                    $('#NuevoForm').modal('hide');
                                    Limpiar();
                                    CargarPedidos();
                                }
                                else {
                                    swal({
                                        title: "Error",
                                        text: response,
                                        icon: "error",
                                        button: "Aceptar"
                                    });
                                }
                            },
                            error: function (response) {
                                swal({
                                    title: "Error",
                                    text: response,
                                    icon: "error",
                                    button: "Aceptar"
                                });
                            }
                        });
                    }
                }
            });
        }
        else {
            swal({
                title: "Atención",
                text: objError.msj,
                icon: "warning",
                button: "Aceptar"
            });
        }
    });

    function CargarPedidos() {

        var xsFechaDesde = $('#txtFechaDesde').val();
        var xsFechaHasta = $('#txtFechaHasta').val();
        var xiCliente = $('#cmbClientes').val();
        var xsEstado = $('#cmbEstados').val();
        var xsUsuario = $('#cmbUsuarios').val();

        var url = '/Pedido/GetPedidos?xsFechaDesde=' + xsFechaDesde + '&xsFechaHasta=' + xsFechaHasta +
                  '&xiCliente=' + xiCliente + '&xsEstado=' + xsEstado + '&xsUsuario=' + xsUsuario

        var tabla = $('#tablePedidos').DataTable({
            "info": false,
            "ajax": url,
            aoColumnDefs: [
                { targets: [0], "bSortable": true, "bVisible": false },
                { targets: [1], "bSortable": true },
                { targets: [2], "bSortable": true },
                { targets: [3], "bSortable": true },
                { targets: [4], "bSortable": true },
                { targets: [5], "bSortable": true },
                { targets: [6], "bSortable": true },
                {
                    "targets": 7,
                    "data": null,
                    "bSortable": false,
                    "width": "20%",
                    "defaultContent": '<button class="btn btn-info btn-xs" title="Editar"><i class="glyphicon glyphicon-pencil"></i></button> <button class="btn btn-primary btn-xs" title="Ver detalle"><i class="fas fa-list-ul"></i></button> <button class="btn btn-warning btn-xs" title="Imprimir"><i class="glyphicon glyphicon-print"></i></button> <button class="btn btn-danger btn-xs" title="Eliminar" disabled><i class="fas fa-trash-alt"></i></button>'
                }
            ],
            "createdRow": function (row, data, dataIndex) {
                if (data[7].trim() === "C") {
                    $(row).find("td:eq(2)").css('background-color', 'slategrey').css('color', 'white').css('font-weight', 'bold');
                } else if (data[7].trim() === "E") {
                    $(row).find("td:eq(2)").css('background-color', '#337ab7').css('color', 'white').css('font-weight', 'bold');
                } else if (data[7].trim() === "F") {
                    $(row).find("td:eq(2)").css('background-color', '#4cae4c').css('color', 'white').css('font-weight', 'bold');
                } else if (data[7].trim() === "PP") {
                    $(row).find("td:eq(2)").css('background-color', '#d43f3a').css('color', 'white').css('font-weight', 'bold');
                }
            },
            "language": {
                "url": "//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/Spanish.json"
            },
            initComplete: function (settings, json) {

                if (json.error !== "") {
                    swal({
                        title: "Error",
                        text: json.error,
                        icon: "error",
                        button: "Aceptar"
                    });
                }
                else {
                    data: json.data;
                }
            }
        });

        editarPedido('#tablePedidos tbody', tabla);
        imprimir('#tablePedidos tbody', tabla);
        verDetalle('#tablePedidos tbody', tabla);
        eliminarPedido('#tablePedidos tbody', tabla);

    }

    function imprimir(tbody, tabla) {

        $(tbody).off('click', 'button.btn-warning.btn-xs');
        $(tbody).on('click', 'button.btn-warning.btn-xs', function (e) {

            var data = tabla.row($(this).parents('tr')).data();
            var id = parseInt(data[0]);

            var url = '/Reporte/GetRemito?xiPedido=' + id;
            window.open(url);

        });

    }

    function verDetalle(tbody, tabla) {

        $(tbody).off('click', 'button.btn-primary.btn-xs');
        $(tbody).on('click', 'button.btn-primary.btn-xs', function (e) {

            $('#BodyDetalle > tr').remove();

            var data = tabla.row($(this).parents('tr')).data();
            var items = JSON.parse(data[10]);

            for (var i = 0; i < items.length; i++) {

                var fila = '<tr style="text-align: center;">' +
                           '<td>' + items[i].ProductoDescripcion + '</td>' +
                           '<td>' + items[i].Cantidad + '</td>' +
                           '<td>' + '$' + items[i].Precio + '</td>' +
                           '<td>' + '$' + items[i].Monto + '</td>' +
                           '</tr>';

                $('#BodyDetalle').append(fila);
            }

            $('#detalleModal').modal('show');

        });
    }

    function eliminarPedido(tbody, tabla) {

    }


});