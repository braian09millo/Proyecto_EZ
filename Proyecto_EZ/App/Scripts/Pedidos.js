/// <reference path="jquery-3.3.1.min.js" />
$(document).ready(function () {

    //Cargamos los pedidos en el LOAD
    CargarPedidos();

    //Cargarmos los datepickers
    $('#txtFechaDesde').datepicker({ dateFormat: 'dd-mm-yy' }).datepicker("setDate", new Date());
    $('#txtFechaHasta').datepicker({ dateFormat: 'dd-mm-yy' }).datepicker("setDate", new Date());

    //Función para volver a crear la tabla
    function Limpiar() {
        $("#tablePedidos").dataTable().fnDestroy();
    }

    function LimpiarFormulario() {

        var items = $('#formPedidos').children();
        for (var i = 1; i < items.length; i++) {
            items[i].remove();
        }
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

        LimpiarFormulario();
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

        if (marca == "" || modelo == "" ||
            tamanio == "" || tipo == "" ||
            costo == "" || cantPack == "" ||
            porc == "")
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

                if ($(this).children("option:selected").val() == '') {
                    $(txtPrecio).val('');
                } else {
                    $(txtPrecio).val(Math.round(precio * 100) / 100);
                }
            });

            //Cuando modificamos la cantidad, calculamos el monto total por item
            $('#item > .col-sm-2.cantidad > input[type="text"]').keyup(function () {

                var txtPrecio = $(this).parent().parent().find('.precio').find('input[type="text"]');
                var txtMonto = $(this).parent().parent().find('.col-sm-2.monto').find('input[type="text"]');
                var total = $(this).val() * $(txtPrecio).val();
                $(txtMonto).val(Math.round(total * 100) / 100);

            });

        });

    });

    $('#btnItem').click(function () {

        var nuevoItem = '<div class="row" style="margin-top: 5px;"><div id="item"><div class="col-sm-3">' +
                        '<select class="form-control"> <option value="">Elija un producto</option>@foreach (var item in ViewBag.Productos){<option data-info="@item.IdProducto" data-precio="@item.PrecioVenta" value="@item.IdProducto">@item.Modelo - @item.Tamanio</option>}</select></div>' +
                        '<div class="col-sm-2 cantidad">' +
                        '<input type="text" class="form-control" placeholder="Packs" />' +
                        '</div><div class="col-sm-2 precio"><input type="text" style="text-align: right;" class="form-control" placeholder="Precio" readonly /></div>' +
                        '<div class="col-sm-2 monto"><input type="text" style="text-align: right;" class="form-control" placeholder="Monto" readonly /></div>' +
                        '<div class="col-sm-1"><button type="button" class="btn btn-default btn-block borrarItem"><i class="fas fa-minus"></i></button></div>' +
                        '</div>';

        $('#formPedidos').append(nuevoItem);

    });

    $('#formPedidos').on('click', '.borrarItem', function (e) {
        e.preventDefault();
        $(this).parent().parent().remove();
    });

    $('#btnGenerarPedido').click(function (e) {

        var Total = 0;
        var Facturado = parseFloat($('#txtFacturado').val());
        var CantidadProductos = 0;
        var CantidadPacks = 0;
        var Productos = [];
        var ClienteElegido = $('#cmbClientesPedido').val();
        var Repartidor = $('#cmbUsuariosPedido').val();

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

        var dataToPost = {

            xiCliente: ClienteElegido,
            xsUsuario: Repartidor,
            xdTotal: Total,
            xdFacturado: Facturado,
            xoProductos: Productos
        }

        swal({
            title: "¿Desea finalizar el pedido?",
            text: "Cliente: " + $('#cmbClientes').children('option:selected').html() + ", Cantidad de Productos: " + CantidadProductos + ", Packs: " + CantidadPacks + ", Total: $ " + (Math.round(Total * 100) / 100),
            icon: "warning",
            buttons: ['Cancelar', 'Confirmar'],
            dangerMode: false,
        }).then(function (bConfirma) {
            if (bConfirma) {
                if (Productos.length > 0) {
                    $.ajax({
                        method: 'POST',
                        data: dataToPost,
                        url: '/Pedido/AgregarPedido',
                        success: function (response) {

                            if (response == "") {
                                swal({
                                    title: "Exito!",
                                    text: "Pedido generado correctamente",
                                    icon: "success",
                                    button: "Aceptar",
                                });

                                $('#NuevoForm').modal('hide');
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
    });

    function CargarPedidos() {

        var tabla = $('#tablePedidos').DataTable({
            "info": false,
            "ajax": '/Pedido/GetPedidos',
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
                    "defaultContent": '<button class="btn btn-warning btn-xs" title="Imprimir"><i class="glyphicon glyphicon-print"></i></button> <button class="btn btn-info btn-xs" title="Ver detalle"><i class="fas fa-list-ul"></i></button> <button class="btn btn-danger btn-xs" title="Eliminar"><i class="fas fa-trash-alt"></i></button>'
                }
            ],
            "language": {
                "url": "//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/Spanish.json"
            },
            initComplete: function (settings, json) {

                if (json.error != "") {
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

        imprimir('#tablePedidos tbody', tabla);
        verDetalle('#tablePedidos tbody', tabla);
        eliminarPedido('#tablePedidos tbody', tabla);

    }

    function imprimir(tbody, tabla) {

    }

    function verDetalle(tbody, tabla) {

    }

    function eliminarPedido(tbody, tabla) {

    }


});